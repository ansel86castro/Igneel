using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Igneel.Assets;
using Igneel.Assets.StorageConverters;
using Igneel.Collections;
using Igneel.Physics;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.Animations;

namespace Igneel.SceneManagement
{
    /// <summary>
    /// <remarks>
    /// Use this format for the node's name if you want to automaticaly create
    /// an physics actor.
    ///        
    /// name format : __a[d][k][b_nodeName]__ 
    ///
    /// [] : optional element.
    /// d: create dynamic actor.
    /// k: kept the node in scene else remove it.
    /// b: node binding.
    /// nodeName :name of the node to bind this actor.
    ///    
    /// Remember for create a wheell shape, you need to create a cylindre and rotate it -90 degrees arount Z-axis( Y-axis in 3DMAX)
    /// </remarks>
    /// </summary>
    //[OnComplete("RegisterForCulling")]
    //[ResourceActivator(typeof(Frame.Activator))]
    public sealed class Frame : Resource, ICullable, ITransformable, IAffectable, IAffector, 
        ISceneComponent, IBoundable
    {
        static Regex _nodeMetaRegex = new Regex(@"\w+__a(_(?<TYPE>(d)))?(_b_(?<BINDING>\w+))?(?<KEEP>_k)?__");
        static Regex _nodeMetaRegexAlt = new Regex(@"\w+__a((?<TYPE>(d)))?(?<KEEP>k)?(b_(?<BINDING>\w+))?__");

        #region Private Members    

        private Vector3 _localScale = new Vector3(1, 1, 1);
        private Matrix _localRotationMtx = Matrix.Identity;
        private Quaternion _localRotationQuat = Quaternion.Identity;
        private Matrix _worldTransform = Matrix.Identity;
        private Matrix _localTransform = Matrix.Identity;
        private Matrix _bindParentMtx = Matrix.Identity;
        private Matrix _bindAffectorMtx = Matrix.Identity;

        private BoundInfo _boundInfo;
        private IAffector _affector;
        private IAffectable _affectable;        
        private FrameTechnique _technique;
        private IGraphicsProvider _nodeObject;                    
        private Frame _parent;
        private ICullRegion _cullRegion;
        private ObservedDictionary<string, Frame> _nodes;

        private FrameType _nodeType;     
        private float _range;           
        private object _userData;
        private string _tag;
        private FrameAnimationController _animationController;
        #endregion

        public Frame(string name)
            :this(name, null)
        {

        }

        public Frame(string name , IGraphicsProvider component)
        {
            Name = name;           

            this._nodeObject = component;                      
            if (component != null)
                component.OnNodeAttach(this);

            var notiServ = Service.Get<INotificationService>();
            if (notiServ != null)
                notiServ.OnObjectCreated(this);

            var logServ = Service.Get<ILogService>();
            if (logServ != null)
                logServ.WriteLine("SceneNode Created :" + name ?? "");
        }

        public Frame(string name, 
            Vector3 localPosition, 
            Matrix localRotation, 
            Vector3 localScale, 
            IGraphicsProvider component) :this(name,component)
        {
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            LocalScale = localScale;

            ComputeLocalPose();
            CommitChanges();            
        }
       
        #region Properties       

        public ICullRegion CullRegion { get { return _cullRegion; } set { _cullRegion = value; } }

        [AssetMember(typeof(CollectionStoreConverter<Frame>))]
        public ObservedDictionary<string, Frame> Childrens
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = new ObservedDictionary<string, Frame>(itemAdded: x =>
                        {
                            if (x._parent != null)
                                x.Remove();
                            x._parent = this;
                        },
                       itemRemoved: x =>
                       {
                           x._parent = null;
                           x._bindParentMtx = Matrix.Identity;
                       },
                       keySelector: x => x.Name);
                }
                return _nodes;
            }
        }
       
        public Frame Parent { get { return _parent; } }            
        
        [AssetMember]
        public FrameType Type { get { return _nodeType; } set { _nodeType = value; } }
        
        [AssetMember]
        public string Tag { get { return _tag; } set { _tag = value; } }
               
        [AssetMember(storeAs: StoreType.Reference)]
        public FrameTechnique Technique
        {
            get { return _technique; }
            set
            {
                if (_technique != value)
                {
                    if (_technique != null)
                    {
                        //dettach the tecnique from the node
                        _technique.OnNodeDetach(this);                        
                    }

                    _technique = value;
                    if (value != null)
                        value.OnNodeAttach(this);
                }

            }
        }
               
        [AssetMember(storeAs: StoreType.Reference)]
        public IAffector Affector 
       { 
           get { return _affector; } 
           set { 
               _affector = value;
               if (_affector != null)
                   _affector.Affectable = this;
           } 
       }
                
        public IAffectable Affectable { get { return _affectable; } set { _affectable = value; } }               
        
        public IGraphicsProvider Component
        {
            get { return _nodeObject; }
            set
            {
                if (_nodeObject != value)
                {
                    if (_nodeObject != null)
                        _nodeObject.OnNodeDetach(this);

                    if (value != null)
                    {
                        value.OnNodeAttach(this);
                        _nodeObject = value;

                        var boundable = value as IBoundable;
                        if(boundable!=null)
                        {
                            if(_boundInfo==null)
                                _boundInfo = new BoundInfo();

                            _boundInfo.LocalSphere = boundable.BoundingSphere;
                            if (boundable.BoundingBox != null)
                            {
                                _boundInfo.OrientedBox = boundable.BoundingBox.Clone();                                
                            }

                            _boundInfo.Update(_worldTransform);                                                      
                        }
                    }                    
                }
            }
        }

        public Scene Scene { get; private set; }

        [AssetMember]
        public object UserData { get { return _userData; } set { _userData = value; } }

        #region Transforms 
                     
        public Vector3 LocalPosition
        {
            get 
            {
                Vector3 v;
                v.X = _localTransform.M41;
                v.Y = _localTransform.M42;
                v.Z = _localTransform.M43;
                return v;
            }
            set
            {
                _localTransform.M41 = value.X;
                _localTransform.M42 = value.Y;
                _localTransform.M43 = value.Z;
            }
        }

        public Vector3 GlobalPosition
        {
            get { return _worldTransform.Translation; }
        }

        public float X
        {
            get { return _localTransform.M41; }
            set
            {
                _localTransform.M41 = value;
            }
        }
       
        public float Y
        {
            get { return _localTransform.M42; }
            set
            {
                _localTransform.M42 = value;
            }
        }
       
        public float Z
        {
            get { return _localTransform.M43; }
            set
            {
                _localTransform.M43 = value;
            }
        }                            
       

        public Vector3 LocalScale
        {
            get { return _localScale; }
            set 
            {
                _localScale = value;
                //localPose.set_Rows(0, rotation.get_Rows(0) * scale.X);
                //localPose.set_Rows(1, rotation.get_Rows(1) * scale.Y);
                //localPose.set_Rows(2, rotation.get_Rows(2) * scale.Z);
            }
        }
       
        public float Sx
        {
            get { return _localScale.X; }
            set
            {
                _localScale.X = value;
                //localPose.set_Rows(0, localPose.get_Rows(0) * value);
            }
        }
       
        public float Sy
        {
            get { return _localScale.Y; }
            set
            {
                _localScale.Y = value;
                //localPose.set_Rows(1, localPose.get_Rows(1) * value);
            }
        }
       
        public float Sz
        {
            get { return _localScale.Z; }
            set
            {
                _localScale.Z = value;
                //localPose.set_Rows(2, localPose.get_Rows(2) * value);
            }
        }
            

        public Matrix LocalRotation
        {
            get { return _localRotationMtx; }
            set 
            { 
                _localRotationMtx = value;
                _localRotationQuat = Quaternion.RotationMatrix(_localRotationMtx);
                //localPose.set_Rows(0, value.get_Rows(0) * scale.X);
                //localPose.set_Rows(1, value.get_Rows(1) * scale.Y);
                //localPose.set_Rows(2, value.get_Rows(2) * scale.Z);
            }
        }
       
        public Vector3 Right { get { return _localRotationMtx.Right; } set { _localRotationMtx.Right =value; } }
       
        public Vector3 Up { get { return _localRotationMtx.Up; } set { _localRotationMtx.Up= value; } }
       
        public Vector3 Front { get { return _localRotationMtx.Front; } set { _localRotationMtx.Front = value; } }        

        public Euler LocalEuler
        {
            get { return Euler.FromMatrix(_localRotationMtx); }
            set 
            {
                LocalRotation = value.ToMatrix(); 
            }
        }
       
        public Quaternion LocalRotationQuat
        {
            get { return _localRotationQuat; }
            set 
            { 
                _localRotationQuat = value;
                _localRotationMtx = Matrix.RotationQuaternion(Quaternion.Normalize(_localRotationQuat));
            }
        }      
       
        public float Heading
        {
            get { return LocalEuler.Heading; }
            set
            {
                //value = Euler.NormalizeHeading(value);
                var orientation = Euler.FromMatrix(_localRotationMtx);
                orientation.Heading = value;
                LocalRotation = orientation.ToMatrix();
            }
        }
       
        public float Pitch
        {
            get { return LocalEuler.Pitch; }
            set
            {
                value = Euler.NormalizePitch(value);

                var orientation = Euler.FromMatrix(_localRotationMtx);
                orientation.Pitch = value;
                LocalRotation = orientation.ToMatrix();
            }
        }
       
        public float Roll
        {
            get { return LocalEuler.Roll; }
            set
            {
                value = Euler.NormalizeRoll(value);

                var orientation = Euler.FromMatrix(_localRotationMtx);
                orientation.Roll = value;
                LocalRotation = orientation.ToMatrix();
            }
        }            


        /// <summary>
        /// Local space tranform of the node
        /// </summary>        
        [AssetMember]        
        public Matrix LocalPose
        {
            get { return _localTransform; }
            set
            {
                _localTransform = value;
                Vector3 trans;
                Matrix.DecomposeTranformationMatrix(value, out _localScale, out _localRotationMtx, out trans);
                _localRotationQuat = Quaternion.RotationMatrix(_localRotationMtx);
            }
        }

        /// <summary>
        /// World space tranform
        /// </summary>                
        public Matrix GlobalPose { get { return _worldTransform; } }

        /// <summary>
        /// Allows to transform the node whe the parent is transformed
        /// </summary>        
        [AssetMember]        
        public Matrix BindParentPose { get { return _bindParentMtx; } set { _bindParentMtx = value; } }

        /// <summary>
        /// Allows to tranform the node when actor is transformed (by the user or by the phycis engine).
        /// It's updates automaticaly when you tranform the node, or set it to invert of the actor`s global pose.
        /// </summary>        
        [AssetMember]        
        public Matrix BindAffectorPose { get { return _bindAffectorMtx; } set { _bindAffectorMtx = value; } }

        #endregion           

        [AssetMember]
        public BoundInfo Bounding { get { return _boundInfo; } }

        public Sphere BoundingSphere { get { return _boundInfo != null ? _boundInfo.GlobalSphere : new Sphere(); } }

        public OrientedBox BoundingBox { get { return _boundInfo != null ? _boundInfo.OrientedBox : null; } }
                             
        public float Range { get { return _range; } set { _range = value; } }                   
       
        public bool IsRoot { get { return _nodeType == FrameType.Root; } }

        public bool IsBone { get { return _nodeType == FrameType.Bone; } }

        public bool IsBoneRoot
        {
            get
            {
                return _nodeType == FrameType.Bone &&
                     (_parent == null || _parent._nodeType != FrameType.Bone);
            }
        }

        public FrameAnimationController AnimationController
        {
            get { return _animationController; }
            set
            {
                _animationController = value;
                if (_animationController != null)
                    _animationController.Target = this;
            }
        }

        #endregion                       
       

        //private Frame _ComputeBoundingsShapes()
        //{
        //    foreach (var node in _nodes)
        //        node.ComputeBoundingsShapes();

        //    if (_nodeObject != null)
        //    {
        //        if (_nodeObject.BoundingBox != null)
        //        {
        //            _globalBox = _nodeObject.BoundingBox.Clone();
        //            _globalBox.Update(_worldTransform);
        //        }
        //        _localSphere = _nodeObject.BoundingSphere;
        //        _globalSphere = _localSphere.GetTranformed(_worldTransform);
        //    }
        //    else if (_nodes.Count > 0)
        //    {
        //        Matrix invWorld = Matrix.Invert(_worldTransform);

        //        var positions = _GetVolumePoints().ToArray();
        //        _globalSphere = new Sphere(positions);
        //        _localSphere = _globalSphere.GetTranformed(invWorld);

        //        Vector3.TransformCoordinates(positions, ref invWorld, positions);
        //        _globalBox = new OrientedBox(positions);
        //        _globalBox.Update(_worldTransform);

        //    }
        //    return this;
        //}

        //private IEnumerable<Vector3> _GetVolumePoints()
        //{
        //    foreach (var node in _nodes)
        //    {
        //        var boundingBox = node.BoundingBox;
        //        if (boundingBox != null)
        //        {
        //            BoxBuilder box = new BoxBuilder(2, 2, 2);

        //            for (int i = 0; i < box.Vertices.Length; i++)
        //            {
        //                var mat = Matrix.Scale(boundingBox.Extends) * boundingBox.GlobalPose;
        //                yield return Vector3.Transform(box.Vertices[i].Position, mat);
        //            }
        //        }
        //        else if (node.GlobalRadius > 0)
        //        {
        //            SphereBuilder sphere = new SphereBuilder(16, 16, node.GlobalRadius);
        //            for (int i = 0; i < sphere.Vertices.Length; i++)
        //            {
        //                var mat = Matrix.Translate(node.GlobalPosition);
        //                yield return Vector3.Transform(sphere.Vertices[i].Position, mat);
        //            }
        //        }
        //    }

        //}              

        public void ComputeBindParentPose()
        {
            _bindParentMtx = _parent != null ? Matrix.Invert(_parent._worldTransform) : Matrix.Identity;
        }            

        public bool Remove()
        {           
            if (_parent != null)
            {                
                return _parent._nodes.Remove(this);
            }
            return false;
        }


        public void OnSceneAttach(Scene scene)
        {
            this.Scene = scene;
           
            //add the technique to the scene
            if (_technique != null)
            {
                _technique.OnSceneAttach(scene);
            }

            if (_nodeObject != null)
            {
                //add the object to the scene
                _nodeObject.OnSceneAttach(scene);

                //Add to the region Culler
                _cullRegion = scene.GetCullRegion(this);
            }

            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    item.OnSceneAttach(scene);
                }
            }            
        }

        public void OnSceneDetach(Scene scene)
        {
            this.Scene = null;
            if (_nodeObject != null)
            {
                _nodeObject.OnSceneDetach(scene);
            }

            //remove the technique from scene
            if (_technique != null)
            {
                _technique.OnSceneDetach(scene);
            }

            //call OnRemove on the childrens
            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    item.OnSceneDetach(scene);
                }
            }

            if (_cullRegion != null)
            {
                _cullRegion.Remove(this);
                _cullRegion = null;
            }
        }


        //private Frame AddToCuller(Scene scene)
        //{
        //    _culledByProvider = false;

        //    if (scene == null)
        //        return this;

        //    if (_nodeObject != null)
        //    {
        //        if (_nodeObject is IGraphicObject || _nodeObject is FrameComponentColletion)
        //        {
        //            //add it to the culler
        //            if (scene.CullingProvider != null && LocalRadius > 0 && !IsDynamic)
        //            {
        //                scene.CullingProvider.Add(this);

        //                _culledByProvider = true;
        //            }
        //            else
        //            {
        //                scene.NonCullingProviderSuported.Add(this);
        //            }
        //        }
        //    }

          
        //    return this;
        //}

        public Frame FindNode(string name)
        {                       
            Frame node = null;
            if (Name == name)
                node = this;                

            else if (_nodes !=null && !_nodes.TryGetValue(name, out node))
            {
                foreach (var item in _nodes)
                {
                    node = item.FindNode(name);
                    if (node != null)
                        break;
                }
            }

            return node;
        }

        public Frame FindNode(int id)
        {
            if (Id < 0) return null;

            if (Id == id) return this;

            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    var result = item.FindNode(id);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        //public Frame GetDescendant(int id)
        //{
        //    if (_nodes != null)
        //    {
        //        foreach (var item in _nodes)
        //        {
        //            if (item._id == id)
        //                return item;

        //            var result = item.GetDescendant(id);
        //            if (result != null)
        //                return result;
        //        }
        //    }
        //    return null;
        //}
      
        //public Frame GetDescendant(string name)
        //{
        //    if (_nodes != null)
        //    {
        //        foreach (var item in _nodes)
        //        {
        //            if (item._name == name)
        //                return item;

        //            var result = item.GetDescendant(_id);
        //            if (result != null)
        //                return result;
        //        }
        //    }

        //    return null;
        //}

        public Frame FindNode(Predicate<Frame> predicate)
        {
            if (predicate(this))
                return this;

            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    var r = item.FindNode(predicate);
                    if (r != null)
                        return r;
                }
            }
            return null;
        }

        //public Frame GetDescendant(Predicate<Frame> predicate)
        //{          
        //    if (_nodes != null)
        //    {
        //        foreach (var item in _nodes)
        //        {
        //            if (predicate(item))
        //                return item;

        //            var r = item.GetDescendant(predicate);
        //            if (r != null)
        //                return r;
        //        }
        //    }
        //    return null;
        //}

        public IEnumerable<Frame> FindNodeByTag(string tag)
        {
            foreach (var item in _nodes)
            {
                if (item._tag == tag)
                    yield return item;
                foreach (var child in item.FindNodeByTag(tag))
                {
                    yield return child;
                }
            }
        }

        public IEnumerable<Frame> EnumerateNodesPosOrden()
        {
            if (_nodes != null)
            {
                foreach (var node in _nodes)
                {
                    yield return node;
                    foreach (var item in node.EnumerateNodesPosOrden())
                    {
                        yield return item;
                    }
                }
            }
        }

        public IEnumerable<Frame> EnumerateNodesInPreOrden()
        {
            if (_nodes != null)
            {
                foreach (var node in _nodes)
                {
                    foreach (var item in node.EnumerateNodesPosOrden())
                    {
                        yield return item;
                    }
                    yield return node;
                }
            }
        }

        public void CopyNodes(ICollection<Frame> collection)
        {
            collection.Add(this);
            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    item.CopyNodes(collection);
                }
            }
        }

        public void CopyGraphics(ICollection<Frame> collection)
        {
            IGraphicObject g = _nodeObject as IGraphicObject;
            if (g != null)
                collection.Add(this);
            else
            {
                FrameComponentColletion c = _nodeObject as FrameComponentColletion;
                if (c != null)
                {
                    collection.Add(this);
                }
            }
            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    item.CopyGraphics(collection);
                }
            }
        }

        public Frame GetBoneRoot()
        {
            if (_nodeType == FrameType.Bone)
            {
                if (_parent != null && _parent._nodeType == FrameType.Bone)
                    return _parent.GetBoneRoot();
                return this;
            }
            return null;
        }
       
        public Frame GetRoot()
        {
            if (_parent == null)
                return this;
            else
                return _parent.GetRoot();
        }

        public int SubmitGraphics(Scene scene, ICollection<GraphicSubmit> collection)
        {
            if (_nodeType == FrameType.Bone)
                return 0;

            int entries = 0;
            var camera = scene.ActiveCamera;
            if (_nodeObject != null)
            {
                if (_boundInfo != null && _boundInfo.IsInside(camera.ViewFrustum))
                {
                    _range = camera.Distance(_boundInfo);

                    entries = _nodeObject.SubmitGraphics(scene, this, collection);

                    if (_technique != null)
                        _technique.NbEntries += entries;
                }
            }
            if (_nodes != null)
            {
                foreach (var item in _nodes)
                    entries += item.SubmitGraphics(scene, collection);
            }

            return entries;
        }

        /// <summary>
        /// Transformation orders are scale * rotation * translation
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        /// <param name="translation"></param>
        public void ComputeLocalPose(Vector3 scale, Matrix rotation, Vector3 translation)
        {
            this._localScale = scale;
            this._localRotationMtx = rotation;
            this._localRotationQuat = Quaternion.RotationMatrix(rotation);

            //set translation
            _localTransform.M41 = translation.X;
            _localTransform.M42 = translation.Y;
            _localTransform.M43 = translation.Z;
            _localTransform.M44 = 1;

            //set rotation and scale
            _localTransform.Right = rotation.Right * scale.X;
            _localTransform.Up = rotation.Up * scale.Y;
            _localTransform.Front = rotation.Front * scale.Z;                      
        }

        public void ComputeLocalPose(Vector3 scale, Quaternion rotation, Vector3 translation)
        {
            this._localScale = scale;
            this._localRotationMtx = Matrix.RotationQuaternion(rotation);
            this._localRotationQuat = rotation;

            //set translation
            _localTransform.M41 = translation.X;
            _localTransform.M42 = translation.Y;
            _localTransform.M43 = translation.Z;
            _localTransform.M44 = 1;

            //set rotation and scale
            _localTransform.Right = _localRotationMtx.Right * scale.X;
            _localTransform.Up = _localRotationMtx.Up * scale.Y;
            _localTransform.Front = _localRotationMtx.Front * scale.Z;  
        }

        public void ComputeLocalPose()
        {
            _localTransform.Right = _localRotationMtx.Right * _localScale.X;
            _localTransform.Up = _localRotationMtx.Up * _localScale.Y;
            _localTransform.Front = _localRotationMtx.Front * _localScale.Z;  
        }
       
        private void _ComputeGlobalPose()
        {
            Matrix result;

            if (_parent == null)
                _worldTransform = _localTransform;
            else
            {
                Matrix.Multiply(ref _bindParentMtx, ref _parent._worldTransform, out result);
                Matrix.Multiply(ref _localTransform, ref result, out _worldTransform);
                //globalPose *= bindParentMtx * parent.globalPose;
            }

            if (_affector != null)
            {
                result = _affector.GlobalPose;
                Matrix.Multiply(ref _bindAffectorMtx, ref result, out result);
                Matrix.Multiply(ref _worldTransform, ref result, out _worldTransform);

                //globalPose *= bindAffectorMtx * affector.GlobalPose;
            }
                     
            _OnPoseUpdated();
        }

        private void _UpdateGlobalPose(Matrix affectorPose)
        {
            _worldTransform = _localTransform;

            if (_parent != null)
                _worldTransform *= _bindParentMtx * _parent._worldTransform;

            _worldTransform *= _bindAffectorMtx * affectorPose;


            Matrix result;

            if (_parent == null)
                _worldTransform = _localTransform;
            else
            {
                Matrix.Multiply(ref _bindParentMtx, ref _parent._worldTransform, out result);
                Matrix.Multiply(ref _localTransform, ref result, out _worldTransform);             
            }
            
            Matrix.Multiply(ref _bindAffectorMtx, ref affectorPose, out result);
            Matrix.Multiply(ref _worldTransform, ref result, out _worldTransform);            

            _OnPoseUpdated();
        }

        public void CommitChanges()
        {
            _ComputeGlobalPose();

            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    item.CommitChanges();
                }
            }
        }     

        private void _OnPoseUpdated()
        {
            if (_boundInfo != null)
            {
                _boundInfo.Update(_worldTransform);               
            }
            //var cullingProvider = scene != null ? scene.CullingProvider : null;

            //if (culledByProvider && cullingProvider != null && !newSphere.Equals(globalSphere))
            //{
            //    cullingProvider.Remove(this);
            //    globalSphere = newSphere;
            //    cullingProvider.Add(this);
            //}
            //else
            //    globalSphere = newSphere;
                       
       
            if (_nodeObject != null)
                _nodeObject.OnPoseUpdated(this);

            if (_affectable != null)
                _affectable.UpdatePose(_worldTransform);

            if (_technique != null)
                _technique.OnPoseUpdated(this);

            if (_cullRegion != null)
            {
                _cullRegion.Update(this);
            }
        }                         

        /// <summary>
        /// This method is called when the affector has influenced his affectable instance
        /// and the affactable needs to updates its GlobalPose. For Physics simulated objects this method is called after
        /// a simulation frame is completed
        /// </summary>
        public void UpdatePose(Matrix affectorPose)
        {
            _UpdateGlobalPose(affectorPose);

            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    item.CommitChanges();
                }
            }
        }
      
        //public bool RemoveFromHeirarchy(SceneNode node)
        //{
        //    if (!Remove(node))
        //    {
        //        foreach (var item in nodes)
        //        {
        //            if (item.RemoveFromHeirarchy(node))
        //                return true;
        //        }
        //        return false;
        //    }
        //    return true;
           
        //}

        public bool Contains(string name)
        {
            if (_nodes == null) return false;
            if (!_nodes.ContainsKey(name))
            {
                foreach (var item in _nodes)
                {
                    if (item.Contains(name))
                        return true;
                }
            }
            return false;
        }

        public bool Contains(Frame node)
        {
            return Contains(node.Name);
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (_nodeObject != null)
                    _nodeObject.Dispose();

                if (_technique != null)
                    _technique.Dispose();

                if (_nodes != null)
                {
                    foreach (var node in _nodes)
                    {
                        node.Dispose();
                    }
                }

                var srv = Service.Get<INotificationService>();
                if (srv != null)
                    srv.OnObjectDestroyed(this);
            }
        }        
        
        #region Physics
       
        private static bool _IsActorMeta(string name, Scene scene, out bool dynamic, out Frame bindNode, out string newName, out bool disposed)
        {
            dynamic = false;
            bindNode = null;
            newName = null;
            disposed = true;

            var match = _nodeMetaRegex.Match(name);
            if (!match.Success)
                match = _nodeMetaRegexAlt.Match(name);

            if (match.Success)
            {
                dynamic = match.Groups["TYPE"].Success;

                var bindGroup = match.Groups["BINDING"];

                if (bindGroup.Success)
                    bindNode = scene.FindNode(bindGroup.Value);

                disposed = !match.Groups["KEEP"].Success;

                int index = name.IndexOf("__");
                newName = name.Substring(0, index);
                return true;
            }

            return false;
        }

        private ActorDesc _CreateActorDescription(bool dynamic, float density = 0, float mass = 0)
        {
            if (Engine.Scene == null || Engine.Scene.Physics == null)
                throw new InvalidOperationException("No Scene");

            ActorDesc actorDesc = new ActorDesc();
            actorDesc.Name = Name;
            actorDesc.GlobalPose = _worldTransform;

            if (density > 0)
            {
                actorDesc.Density = density;
                mass = 0;
            }

            if (dynamic)
            {
                actorDesc.Body = new BodyDesc() { };
                if (mass > 0)
                    actorDesc.Body.Mass = mass;
            }

            return actorDesc;
        }

        private ActorShapeDesc _CreateShapeDescriptor(Matrix pose)
        {
            ActorShapeDesc shapeDesc = null;
            if (_nodeObject is IFrameMesh)
            {
                var mesh = ((IFrameMesh)_nodeObject).Mesh;
                shapeDesc = mesh.CreateShapeDescriptor();
                if (shapeDesc == null)
                    shapeDesc = mesh.CreateTriangleMeshDescriptor();

                shapeDesc.Name = Name;
                shapeDesc.LocalPose *= pose;                
            }
            else if (_nodes.Count == 1)
            {
                shapeDesc = _nodes[0]._CreateShapeDescriptor(_nodes[0].LocalPose * pose);
            }
            
            return shapeDesc;

        }

        /// <summary>
        /// These can be specified in several different ways:
        ///
        ///1) actorDesc.density == 0, bodyDesc.mass > 0, bodyDesc.massSpaceInertia.magnitude() > 0
        ///
        ///Here the mass properties are specified explicitly, there is nothing to compute.
        ///
        ///2) actorDesc.density > 0, actorDesc.shapes.size() > 0, bodyDesc.mass == 0, bodyDesc.massSpaceInertia.magnitude() == 0
        ///
        ///Here a density and the shapes are given. From this both the mass and the inertia tensor is computed.
        ///
        ///3) actorDesc.density == 0, actorDesc.shapes.size() > 0, bodyDesc.mass > 0, bodyDesc.massSpaceInertia.magnitude() == 0
        ///
        ///Here a mass and shapes are given. From this the inertia tensor is computed.
        ///
        ///Other combinations of settings are illegal.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dynamic"></param>
        /// <param name="shapeBuilder"></param>
        /// <param name="density"></param>
        /// <param name="mass"></param>
        /// <returns></returns>
        public Actor CreateActor(Physic physic, bool dynamic, Func<Frame, ActorShapeDesc> shapeBuilder, float density = 0, float mass = 0)
        {
            if (physic != null) throw new NullReferenceException("Physics");

            ActorDesc actorDesc =  _CreateActorDescription(dynamic, density, mass);
            if (shapeBuilder != null)
            {
                foreach (var child in EnumerateNodesPosOrden())
                {
                    var shapeDesc = shapeBuilder(child);
                    if (shapeDesc != null)
                        actorDesc.Shapes.Add(shapeDesc);
                }
            }

            var actor = physic.CreateActor(actorDesc);
            this.BindTo(actor);

            return actor;
        }

        public Actor CreateActor(Physic physic, bool dynamic, ActorShapeDesc[] shapes, float density = 0, float mass = 0)
        {
            if (physic != null) throw new ArgumentNullException("physic");

            ActorDesc actorDesc = _CreateActorDescription(dynamic, density, mass);

            foreach (var item in shapes)
                actorDesc.Shapes.Add(item);

            var actor = physic.CreateActor(actorDesc);
            this.BindTo(actor);

            return actor;
        }

        public Actor CreateActor(Physic physic, bool dynamic, float density = 0, float mass = 0)
        {
            if (physic == null) throw new ArgumentNullException("physic");

            ActorDesc actorDesc = _CreateActorDescription(dynamic, density, mass);

            if (_nodeObject is IFrameMesh || _nodes.Count == 1)
            {
                ActorShapeDesc shapeDesc = _CreateShapeDescriptor(Matrix.Identity);

                if (shapeDesc is WheelShapeDesc)
                {
                    //Matrix pose = Matrix.Identity;
                    //pose.set_Rows(3, shapeDesc.LocalPose.get_Rows(3));
                    //shapeDesc.LocalPose = pose;

                    //pose.set_Rows(3, actorDesc.GlobalPose.get_Rows(3));
                    //actorDesc.GlobalPose = pose;
                    actorDesc.GlobalPose = Matrix.RotationZ(-Numerics.PIover2) * actorDesc.GlobalPose;
                }
                if (shapeDesc != null)
                {                  
                    if (density == 0 && dynamic)
                        shapeDesc.Mass = 1.0f;
                    actorDesc.Shapes.Add(shapeDesc);
                }               
            }
            else
            {
                foreach (var child in _nodes)
                {
                    ActorShapeDesc shapeDesc = child._CreateShapeDescriptor(child.LocalPose);
                    if (shapeDesc != null)
                    {
                        if (shapeDesc is WheelShapeDesc)
                        {
                            //Matrix pose = Matrix.Identity;
                            //pose.set_Rows(3, shapeDesc.LocalPose.get_Rows(3));
                            //shapeDesc.LocalPose = pose;
                            shapeDesc.LocalPose = Matrix.RotationZ(-Numerics.PIover2) * shapeDesc.LocalPose;
                        }
                        if (density == 0 && dynamic)
                        {
                            var match = Regex.Match(child._tag, @"__m(?<X>\d+)((\.|_)(?<Y>\d+))?__");
                            if (match.Success)
                            {                             
                               shapeDesc.Mass =  float.Parse(match.Groups["X"].Value + "." + match.Groups["Y"].Value);                               
                            }
                        }
                        actorDesc.Shapes.Add(shapeDesc);
                    }
                }
            }

            if (actorDesc.Shapes.Count == 0)
                throw new InvalidOperationException("There are any shape present and it is not allowed create a empty actor");

            var actor = physic.CreateActor(actorDesc);
            this.BindTo(actor);
            return actor;
        }      

        public void CreateActors(Scene scene)
        {
            if (scene.Physics == null)
                return;

            bool dynamic;
            Frame bindNode;
            string actorName;
            bool disposeNode;
            if (_IsActorMeta(Name, scene, out dynamic, out bindNode, out actorName, out disposeNode))
            {
                var actor = CreateActor(scene.Physics, dynamic, 1);
                actor.Name = actorName;
                Name = actorName;

                if (disposeNode)
                {
                    Remove();                    
                    Dispose();
                }

                if (bindNode != null && bindNode != this)
                    bindNode.BindTo(actor);
            }
            else
            {
                var nodesArray = this._nodes.ToArray();
                foreach (var subNode in nodesArray)
                {
                    subNode.CreateActors(scene);
                }
            }
        }

        #endregion

        #region Helpers

        public Frame Set(Action<Frame> action)
        {
            action(this);
            return this;
        }

        public T GetComponent<T>()
            where T : class, IGraphicsProvider
        {
            return _nodeObject as T;
        }

        public Frame CreateChild<T>(T component) where T : IGraphicsProvider, INameable
        {
            var node = new Frame(component.Name, component);
            Childrens.Add(node);
            return node;
        }

        public Frame CreateChild(string name, IGraphicsProvider component)
        {
            var node = new Frame(name, component);
            _nodes.Add(node);
            return node;
        }

        public Frame CreateChild(string name, IGraphicsProvider component, Vector3 localPosition, Matrix localRotation, Vector3 localScale)            
        {
            var node = new Frame(name, component);          
            node.ComputeLocalPose(localScale, localRotation, localPosition);
            node.CommitChanges();
            Childrens.Add(node);
            return node;
        }

        public Frame CreateChild(string name, IGraphicsProvider component, Matrix localPose)
        {
            var node = new Frame(name, component);
            node.LocalPose = localPose;
            node.CommitChanges();            
            Childrens.Add(node);
            return node;
        }

        public static Frame CreateNode(string name, IGraphicsProvider component, Matrix localPose)
        {
            var node = new Frame(name, component);
            node.LocalPose = localPose;
            node.CommitChanges();                   
            return node;
        }

        public static Frame CreateNode(string name, IGraphicsProvider component, Vector3 localPosition = default(Vector3),
              Matrix localRotation = default(Matrix), Vector3 localScale = default(Vector3))
        {
            var node = new Frame(name, component);
            node.LocalPosition = localPosition;
            node.LocalRotation = localRotation.IsZero ? Matrix.Identity : localRotation;
            node.LocalScale = localScale == Vector3.Zero ? Vector3.One : localScale;
            node.ComputeLocalPose();
            node.CommitChanges();
            return node;
        }

        public Frame Dynamic(Scene scene, Action<float> updateCallback)
        {           
            scene.Dynamics.Add(new Dynamic(updateCallback));
            return this;
        }

        #endregion

        //[Serializable]
        //class Activator : IResourceActivator
        //{
        //    string _name;          
        //    AssetReference _componentRef;
        //    Matrix  _globalPose;          

        //    public void Initialize(IAssetProvider provider)
        //    {
        //        var node = (Frame)provider;
        //        var am = AssetManager.Instance;
        //        _name = node._name;
        //        _globalPose = node._worldTransform;                       

        //        if (node.Component != null)
        //            _componentRef = am.GetAssetReference(node.Component);
        //    }

        //    public IAssetProvider OnCreateResource()
        //    {
        //        var am = Service.Get<AssetManager>();                
        //        IGraphicsProvider comp = null;  
            
        //        if (_componentRef != null)
        //            comp = (IGraphicsProvider)am.GetAssetProvider(_componentRef);

        //        var node = new Frame(_name, comp);
        //        node._worldTransform = _globalPose;               
        //        return node;
        //    }
          
        //}       
    }
     
}
