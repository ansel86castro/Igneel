using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel.Rendering;
using Igneel.Design;
using Igneel.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Igneel.Assets;
using Igneel.Controllers;
using Igneel.Physics;
using System.Text.RegularExpressions;
using Igneel.Services;
using Igneel.Design.UITypeEditors;
using System.Drawing.Design;
using Igneel.Graphics;

namespace Igneel.Components
{
    public enum NodeType 
    { 
        Node = 0, 
        Bone = 1,
        Root = 2,
    }

    
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
    [ProviderActivator(typeof(SceneNode.Activator))]
    [TypeConverter(typeof(DesignTypeConverter))]
    public sealed class SceneNode : ResourceAllocator, IBoundable, ITransformable,  INameable, IAffectable, IAffector, IAssetProvider,ISceneElement,IShadingInput, IIdentificable
    {
        static Regex nodeMetaRegex = new Regex(@"\w+__a(_(?<TYPE>(d)))?(_b_(?<BINDING>\w+))?(?<KEEP>_k)?__");
        static Regex nodeMetaRegexAlt = new Regex(@"\w+__a((?<TYPE>(d)))?(?<KEEP>k)?(b_(?<BINDING>\w+))?__");

        private string name;
        private bool visible = true;
        private int id = Generator.GenerateId();        
        private Vector3 scale = new Vector3(1, 1, 1);      
        private Matrix rotationMtx = Matrix.Identity;
        private Quaternion rotationQuat = Quaternion.Identity;
        private Matrix globalPose = Matrix.Identity;
        private Matrix localPose = Matrix.Identity;
        private Matrix bindParentMtx = Matrix.Identity;
        private Matrix bindAffectorMtx = Matrix.Identity;
        private Sphere localSphere;
        private Sphere globalSphere;
        private float range;
        private NodeType nodeType = NodeType.Node;        
        private bool culledByProvider;        
        private OrientedBox globalBox;      
        private object userData;        
        private string tag;
        private IAffector affector;
        private IAffectable affectable;        
        private NodeTechnique nodeTechnique;
        private INodeObject nodeObject;                                
        private SceneNode parent;
        private ObservedDictionary<string, SceneNode> nodes;
        private bool isSync;
        public event Action<SceneNode> PoseUpdated;
        public event Action<SceneNode> ComponentChanged;
        private bool isDynamic;        
        
        public SceneNode(string name , INodeObject component = null)
        {
            this.name = name ?? "Node" + id;          

            nodes = new ObservedDictionary<string, SceneNode>(itemAdded: x =>
                {
                    if (x.parent != null)
                        x.Remove();
                    x.parent = this;                   
                },
                itemRemoved: x =>
                {
                    x.parent = null;
                    x.bindParentMtx = Matrix.Identity;                  
                },
                keySelector: x => x.Name);

            this.nodeObject = component;                      
            if (component != null)
                component.OnNodeAttach(this);

            var notiServ = Service.Get<INotificationService>();
            if (notiServ != null)
                notiServ.OnObjectCreated(this);

            var logServ = Service.Get<ILogService>();
            if (logServ != null)
                logServ.WriteLine("SceneNode Created :" + name ?? "");
        }

        public SceneNode(string name, Vector3 localPosition, Matrix localRotation, Vector3 localScale, INodeObject component)
            :this(name,component)
        {
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            LocalScale = localScale;

            UpdateLocalPose();
            CommitChanges();
            ComputeBoundingsShapes();
        }
       
        #region Properties                                                        

        public bool IsGPUSync { get { return isSync; } set { isSync = value; } }

        [Category("Properties")]
        [AssetMember]
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value && parent!=null)                
                    parent.nodes.ChangeKey(name, value);                
                name = value;
            }
        }

        [Category("Properties")]
        public int Id { get { return id; } }      

        [Category("Properties")]
        [Browsable(false)]
        [AssetMember(typeof(CollectionStoreConverter<SceneNode>))]
        public ObservedDictionary<string, SceneNode> Childrens { get { return nodes; } }

        [Category("Properties")]
        public SceneNode Parent { get { return parent; } }

        [Category("Properties")]
        [AssetMember]
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                foreach (var item in nodes)
                {
                    item.Visible = value;
                }
            }
        }

        [Category("Properties")]
        [AssetMember]
        public NodeType Type { get { return nodeType; } set { nodeType = value; } }

        [Category("Properties")]
        [AssetMember]
        public string Tag { get { return tag; } set { tag = value; } }

       [Category("Properties")]
        [DynamicEditableAttribute]
        [AssetMember(storeAs: StoreType.Reference)]
        public NodeTechnique Technique
        {
            get { return nodeTechnique; }
            set
            {
                if (nodeTechnique != value)
                {
                    if (nodeTechnique != null)
                    {
                        //dettach the tecnique from the node
                        nodeTechnique.OnNodeDetach(this);                        
                    }

                    nodeTechnique = value;
                    if (value != null)
                        value.OnNodeAttach(this);
                }

            }
        }

        [Category("Properties")]
        [TypeConverter(typeof(DesignTypeConverter))]
        [AssetMember(storeAs: StoreType.Reference)]
        public IAffector Affector 
       { 
           get { return affector; } 
           set { 
               affector = value;
               if (affector != null)
                   affector.Affectable = this;
           } 
       }

        [Category("Properties")]        
        public IAffectable Affectable { get { return affectable; } set { affectable = value; } }

        [Category("Properties")]
        [AssetMember]
        public bool IsDynamic
        {
            get { return isDynamic || affector != null; }
            set
            {
                //if (isDynamic != value && scene != null)
                //{
                //    try
                //    {
                //        Engine.Lock();
                //        if (value)
                //            scene.Dynamics.Add(this);
                //        else
                //            scene.Dynamics.Remove(this);
                //    }
                //    finally
                //    {
                //        Engine.Unlock();
                //    }
                //}
                isDynamic = value;
            }
        }

        [Category("Properties")]
        public INodeObject NodeObject
        {
            get { return nodeObject; }
            set
            {
                if (nodeObject != value)
                {
                    if (nodeObject != null)
                        nodeObject.OnNodeDetach(this);

                    if (value != null)
                    {
                        value.OnNodeAttach(this);
                        nodeObject = value;

                        if (nodeObject.BoundingBox != null)
                        {
                            globalBox = nodeObject.BoundingBox.Clone();
                            globalBox.Update(globalPose);
                        }
                        localSphere = nodeObject.BoundingSphere;
                        globalSphere = localSphere.GetTranformed(globalPose);  


                    }                    
                    if (ComponentChanged != null)
                        ComponentChanged(this);
                }
            }
        }

        //[Category("Properties")]
        //[AssetMember(storeAs: StoreType.Reference)]
        //public NodeController Controller
        //{
        //    get { return controller; }
        //    set
        //    {
        //        controller = value;
        //        if (controller != null)
        //            controller.Initialize(this);
        //    }
        //}

        [Category("Properties")]
        public bool IsCulledByProvider
        {
            get { return culledByProvider; }
        }

        [Category("Object")]
        [AssetMember]
        public object UserData { get { return userData; } set { userData = value; } }

        #region Translation 
        
        [Browsable(true)]
        [Editor(typeof(UIVector3TypeEditor), typeof(UITypeEditor))]        
        [Category("Transforms")]
        [Deferred]
        [LockOnSet]
        public Vector3 LocalPosition
        {
            get 
            {
                Vector3 v;
                v.X = localPose.M41;
                v.Y = localPose.M42;
                v.Z = localPose.M43;
                return v;
            }
            set
            {
                localPose.M41 = value.X;
                localPose.M42 = value.Y;
                localPose.M43 = value.Z;
            }
        }

        [Browsable(false)]
        public float Tx
        {
            get { return localPose.M41; }
            set
            {
                localPose.M41 = value;
            }
        }

        [Browsable(false)]
        public float Ty
        {
            get { return localPose.M42; }
            set
            {
                localPose.M42 = value;
            }
        }

        [Browsable(false)]
        public float Tz
        {
            get { return localPose.M43; }
            set
            {
                localPose.M43 = value;
            }
        }

        [ReadOnly(true)]
        [Category("Transforms")]
        public Vector3 GlobalPosition
        {
            get { return globalSphere.Center; }
        }

        #endregion

        #region Scale

        [Editor(typeof(UIVector3TypeEditor), typeof(UITypeEditor))]        
        [Category("Transforms")]        
        [Deferred]
        [LockOnSet]
        public Vector3 LocalScale
        {
            get { return scale; }
            set 
            {
                scale = value;
                //localPose.set_Rows(0, rotation.get_Rows(0) * scale.X);
                //localPose.set_Rows(1, rotation.get_Rows(1) * scale.Y);
                //localPose.set_Rows(2, rotation.get_Rows(2) * scale.Z);
            }
        }

        [Browsable(false)]
        public float Sx
        {
            get { return scale.X; }
            set
            {
                scale.X = value;
                //localPose.set_Rows(0, localPose.get_Rows(0) * value);
            }
        }

        [Browsable(false)]
        public float Sy
        {
            get { return scale.Y; }
            set
            {
                scale.Y = value;
                //localPose.set_Rows(1, localPose.get_Rows(1) * value);
            }
        }

        [Browsable(false)]
        public float Sz
        {
            get { return scale.Z; }
            set
            {
                scale.Z = value;
                //localPose.set_Rows(2, localPose.get_Rows(2) * value);
            }
        }

        #endregion

        #region Rotation

        [Category("Transforms")]
        [Deferred]        
        [EditorAttribute(typeof(UIRotationMatrixEditor), typeof(UITypeEditor))]        
        [Browsable(false)]
        public Matrix LocalRotation
        {
            get { return rotationMtx; }
            set 
            { 
                rotationMtx = value;
                rotationQuat = Quaternion.RotationMatrix(rotationMtx);
                //localPose.set_Rows(0, value.get_Rows(0) * scale.X);
                //localPose.set_Rows(1, value.get_Rows(1) * scale.Y);
                //localPose.set_Rows(2, value.get_Rows(2) * scale.Z);
            }
        }

        [Browsable(false)]
        public Vector3 Right { get { return rotationMtx.Right; } set { rotationMtx.Right =value; } }

        [Browsable(false)]
        public Vector3 Up { get { return rotationMtx.Up; } set { rotationMtx.Up= value; } }

        [Browsable(false)]
        public Vector3 Front { get { return rotationMtx.Front; } set { rotationMtx.Front = value; } }        

        [Deferred]
        [LockOnSet]
        [Category("Transforms")]
        public Euler LocalRotationEuler
        {
            get { return Euler.FromMatrix(rotationMtx); }
            set 
            {
                LocalRotation = value.ToMatrix(); 
            }
        }

        [Browsable(false)]
        public Quaternion LocalRotationQuat
        {
            get { return rotationQuat; }
            set 
            { 
                rotationQuat = value;
                rotationMtx = Matrix.RotationQuaternion(Quaternion.Normalize(rotationQuat));
            }
        }      

        [Browsable(false)]
        public float Heading
        {
            get { return LocalRotationEuler.Heading; }
            set
            {
                value = Euler.NormalizeHeading(value);
                var orientation = Euler.FromMatrix(rotationMtx);
                orientation.Heading = value;
                LocalRotation = orientation.ToMatrix();
            }
        }

        [Browsable(false)]
        public float Pitch
        {
            get { return LocalRotationEuler.Pitch; }
            set
            {
                value = Euler.NormalizePitch(value);

                var orientation = Euler.FromMatrix(rotationMtx);
                orientation.Pitch = value;
                LocalRotation = orientation.ToMatrix();
            }
        }

        [Browsable(false)]
        public float Roll
        {
            get { return LocalRotationEuler.Roll; }
            set
            {
                value = Euler.NormalizeRoll(value);

                var orientation = Euler.FromMatrix(rotationMtx);
                orientation.Roll = value;
                LocalRotation = orientation.ToMatrix();
            }
        }

        #endregion

        #region Transforms

        /// <summary>
        /// Local space tranform of the node
        /// </summary>        
        [AssetMember]
        [Category("Pose")]
        public Matrix LocalPose
        {
            get { return localPose; }
            set
            {
                localPose = value;
                Vector3 trans;
                Matrix.DecomposeTranformationMatrix(value, out scale, out rotationMtx, out trans);
                rotationQuat = Quaternion.RotationMatrix(rotationMtx);
            }
        }

        /// <summary>
        /// World space tranform
        /// </summary>        
        [Category("Pose")]
        public Matrix GlobalPose { get { return globalPose; } }

        /// <summary>
        /// Allows to transform the node whe the parent is transformed
        /// </summary>        
        [AssetMember]
        [Category("Pose")]
        public Matrix BindHeirarchyPose { get { return bindParentMtx; } set { bindParentMtx = value; } }

        /// <summary>
        /// Allows to tranform the node when actor is transformed (by the user or by the phycis engine).
        /// It's updates automaticaly when you tranform the node, or set it to invert of the actor`s global pose.
        /// </summary>        
        [AssetMember]
        [Category("Pose")]
        public Matrix BindAffectorPose { get { return bindAffectorMtx; } set { bindAffectorMtx = value; } }

        #endregion     

        #region Culling

        [Category("Culling")]
        [AssetMember]
        public Sphere LocalSphere { get { return localSphere; } set { localSphere = value; } }
        
        [Browsable(false)]
        public float LocalRadius { get { return localSphere.Radius; } set { localSphere.Radius = value; } }

        [Browsable(false)]
        public Vector3 LocalCenter { get { return localSphere.Center; } set { localSphere.Center = value; } }

        [AssetMember]
        [Category("Culling")]
        public Sphere BoundingSphere { get { return globalSphere; } set { globalSphere = value; } }

        [Browsable(false)]
        public float GlobalRadius { get { return globalSphere.Radius; } set { globalSphere.Radius = value; } }

        [AssetMember]
        [Category("Culling")]
        public OrientedBox BoundingBox { get { return globalBox; } set { globalBox = value; } }       

        [ReadOnly(true)]
        [Category("Culling")]
        [Description("Distance from the Active Camera")]
        [AssetMember]
        public float Range { get { return range; } set { range = value; } }  

        #endregion              
       
        public bool IsBone { get { return nodeType == NodeType.Bone; } }

        public bool IsBoneRoot { get { return nodeType == NodeType.Bone && (parent == null || parent.nodeType != NodeType.Bone); } }

        #endregion                       

       // public bool IsInNonCullingList { get { return !culledByProvider && !(LocalRadius > 0 && !IsDynamic && Affector == null); } }

        #region Layout              

        public SceneNode ComputeBoundingsShapes()
        {
            foreach (var node in nodes)
                node.ComputeBoundingsShapes();

            if (nodeObject != null)
            {
                if (nodeObject.BoundingBox != null)
                {
                    globalBox = nodeObject.BoundingBox.Clone();
                    globalBox.Update(globalPose);
                }
                localSphere = nodeObject.BoundingSphere;
                globalSphere = localSphere.GetTranformed(globalPose);              
            }
            else if (nodes.Count > 0)
            {
                Matrix invWorld = Matrix.Invert(globalPose);

                var positions = _GetVolumePoints().ToArray();
                globalSphere = new Sphere(positions);
                localSphere = globalSphere.GetTranformed(invWorld);

                Vector3.TransformCoordinates(positions, ref invWorld, positions);
                globalBox = new OrientedBox(positions);
                globalBox.Update(globalPose);

            }
            return this;
        }             

        private IEnumerable<Vector3> _GetVolumePoints()
        {
            foreach (var node in nodes)
            {
                var boundingBox = node.BoundingBox;
                if (boundingBox != null)
                {
                    BoxBuilder box = new BoxBuilder(2, 2, 2);

                    for (int i = 0; i < box.Vertices.Length; i++)
                    {
                        var mat = Matrix.Scale(boundingBox.Extends) * boundingBox.GlobalPose;
                        yield return Vector3.Transform(box.Vertices[i].Position, mat);
                    }
                }
                else if (node.GlobalRadius > 0)
                {
                    SphereBuilder sphere = new SphereBuilder(16, 16, node.GlobalRadius);
                    for (int i = 0; i < sphere.Vertices.Length; i++)
                    {
                        var mat = Matrix.Translate(node.GlobalPosition);
                        yield return Vector3.Transform(sphere.Vertices[i].Position, mat);
                    }
                }
            }
            
        }        

        #endregion

        #region Nodes

        public void KeepLocalPose()
        {
            bindParentMtx = parent != null ? Matrix.Invert(parent.globalPose) : Matrix.Identity;
        }            

        public bool Remove()
        {           
            if (parent != null)
            {
                return parent.nodes.Remove(this);                
            }
            return false;
        }            

        public void OnRemoveFromScene(Scene scene)
        {                       
            if (scene != null)
            {
                if (nodeObject != null)
                {
                    //remove the object from culling
                    if (culledByProvider)
                    {
                        scene.CullingProvider.Remove(this);
                        culledByProvider = false;
                    }
                    else
                        scene.NonCullingProviderSuported.Remove(this);

                    nodeObject.OnRemoveFromScene(scene);
                }
                //remove the technique from scene
                if (nodeTechnique != null)
                {
                    nodeTechnique.OnRemoveFromScene(scene);
                }
            }
            //call OnRemove on the childrens
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    item.OnRemoveFromScene(scene);
                }
            }          
        }

        public void OnAddToScene(Scene scene)
        {
            AddToCuller(scene);

            if (nodeObject != null)
            {
                //add the object to the scene
                nodeObject.OnAddToScene(scene);
            }
            //add the technique to the scene
            if (nodeTechnique != null)
            {
                nodeTechnique.OnAddToScene(scene);
            }

            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    item.OnAddToScene(scene);
                }
            }

        }

        private SceneNode AddToCuller(Scene scene)
        {
            culledByProvider = false;

            if (scene == null)
                return this;

            if (nodeObject != null)
            {
                if (nodeObject is IGraphicObject || nodeObject is NodeObjectColletion)
                {
                    //add it to the culler
                    if (scene.CullingProvider != null && LocalRadius > 0 && !IsDynamic)
                    {
                        scene.CullingProvider.Add(this);

                        culledByProvider = true;
                    }
                    else
                    {
                        scene.NonCullingProviderSuported.Add(this);
                    }
                }
            }

          
            return this;
        }

        public SceneNode GetNode(string name)
        {            
            if (name == null) throw new ArgumentNullException("name");

            SceneNode node = null;
            if (this.name == name)
                node = this;                
            else if (nodes !=null && !nodes.TryGetValue(name, out node))
            {
                foreach (var item in nodes)
                {
                    node = item.GetNode(name);
                    if (node != null)
                        break;
                }
            }

            return node;
        }

        public SceneNode GetNode(int id)
        {
            if (id < 0) return null;

            if (this.id == id) return this;

            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    var result = item.GetNode(id);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        public SceneNode GetDescendant(int id)
        {
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    if (item.id == id)
                        return item;

                    var result = item.GetDescendant(id);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }
      
        public SceneNode GetDescendant(string name)
        {
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    if (item.name == name)
                        return item;

                    var result = item.GetDescendant(id);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        public SceneNode GetNode(Predicate<SceneNode> predicate)
        {
            if (predicate(this))
                return this;

            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    var r = item.GetNode(predicate);
                    if (r != null)
                        return r;
                }
            }
            return null;
        }

        public SceneNode GetDescendant(Predicate<SceneNode> predicate)
        {          
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    if (predicate(item))
                        return item;

                    var r = item.GetDescendant(predicate);
                    if (r != null)
                        return r;
                }
            }
            return null;
        }

        public IEnumerable<SceneNode> GetNodeByTag(string tag)
        {
            foreach (var item in nodes)
            {
                if (item.tag == tag)
                    yield return item;
                foreach (var child in item.GetNodeByTag(tag))
                {
                    yield return child;
                }
            }
        }

        public IEnumerable<SceneNode> EnumerateNodesPosOrden()
        {
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    yield return node;
                    foreach (var item in node.EnumerateNodesPosOrden())
                    {
                        yield return item;
                    }
                }
            }
        }

        public IEnumerable<SceneNode> EnumerateNodesInPreOrden()
        {
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    foreach (var item in node.EnumerateNodesPosOrden())
                    {
                        yield return item;
                    }
                    yield return node;
                }
            }
        }

        public SceneNode GetBoneRoot()
        {
            if (nodeType == NodeType.Bone)
            {
                if (parent != null && parent.nodeType == NodeType.Bone)
                    return parent.GetBoneRoot();
                return this;
            }
            return null;
        }
       
        public SceneNode GetRoot()
        {
            if (parent == null)
                return this;
            else
                return parent.GetRoot();
        }
        #endregion
    
        public void GetRenderEntries(Scene scene, List<DrawingEntry> renderList)
        {
            if (!visible) return;
         
            var camera = scene.ActiveCamera;
            if (globalSphere.Radius == 0 || camera.ViewFrustum.Contains(globalSphere))
            {
                range = camera.GetDistanceTo(this);

                if (nodeObject != null)
                {
                    int nb = nodeObject.GetGraphicObjects(this, renderList);
                    if (nodeTechnique != null)
                        nodeTechnique.NbEntries += nb;
                }

                foreach (var item in nodes)
                    item.GetRenderEntries(scene, renderList);
            }
        }                                            

        /// <summary>
        /// Transformation orders are scale * rotation * translation
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        /// <param name="translation"></param>
        public void ComputeLocalPose(Vector3 scale, Matrix rotation, Vector3 translation)
        {
            this.scale = scale;
            this.rotationMtx = rotation;
            this.rotationQuat = Quaternion.RotationMatrix(rotation);

            //set translation
            localPose.M41 = translation.X;
            localPose.M42 = translation.Y;
            localPose.M43 = translation.Z;
            localPose.M44 = 1;

            //set rotation and scale
            localPose.Right = rotation.Right * scale.X;
            localPose.Up = rotation.Up * scale.Y;
            localPose.Front = rotation.Front * scale.Z;                      
        }

        public void ComputeLocalPose(Vector3 scale, Quaternion rotation, Vector3 translation)
        {
            this.scale = scale;
            this.rotationMtx = Matrix.RotationQuaternion(rotation);
            this.rotationQuat = rotation;

            //set translation
            localPose.M41 = translation.X;
            localPose.M42 = translation.Y;
            localPose.M43 = translation.Z;
            localPose.M44 = 1;

            //set rotation and scale
            localPose.Right = rotationMtx.Right * scale.X;
            localPose.Up = rotationMtx.Up * scale.Y;
            localPose.Front = rotationMtx.Front * scale.Z;  
        }

        public void UpdateLocalPose()
        {
            localPose.Right = rotationMtx.Right * scale.X;
            localPose.Up = rotationMtx.Up * scale.Y;
            localPose.Front = rotationMtx.Front * scale.Z;  
        }
       
        private void _UpdateGlobalPose()
        {
            Matrix result;
            if (parent != null)
            {
                Matrix.Multiply(ref bindParentMtx, ref parent.globalPose, out result);
                Matrix.Multiply(ref localPose, ref result, out globalPose);
                //globalPose *= bindParentMtx * parent.globalPose;
            }
            else
            {
                globalPose = localPose;
            }

            if (affector != null)
            {
                result = affector.GlobalPose;
                Matrix.Multiply(ref bindAffectorMtx, ref result, out result);
                Matrix.Multiply(ref globalPose, ref result, out globalPose);

                //globalPose *= bindAffectorMtx * affector.GlobalPose;
            }
                     
            _OnPoseUpdated();
        }

        private void _UpdateGlobalPose(Matrix affectorPose)
        {
            globalPose = localPose;

            if (parent != null)
                globalPose *= bindParentMtx * parent.globalPose;            

            globalPose *= bindAffectorMtx * affectorPose;    
        
            _OnPoseUpdated();
        }       

        public void CommitChanges()
        {
            _UpdateGlobalPose();

            foreach (var item in nodes)
            {
                item.CommitChanges();
            } 
            isSync = false;
        }     

        private void _OnPoseUpdated()
        {
            if (globalBox != null)
                globalBox.Update(globalPose);

            globalSphere = localSphere.GetTranformed(globalPose);

            //var cullingProvider = scene != null ? scene.CullingProvider : null;

            //if (culledByProvider && cullingProvider != null && !newSphere.Equals(globalSphere))
            //{
            //    cullingProvider.Remove(this);
            //    globalSphere = newSphere;
            //    cullingProvider.Add(this);
            //}
            //else
            //    globalSphere = newSphere;
                       

            //inform to nodeObject that pose has changed
            if (nodeObject != null)
                nodeObject.OnPoseUpdated(this);

            if (affectable != null)
                affectable.UpdatePose(globalPose);

            if (PoseUpdated != null)
                PoseUpdated(this);
        }
       
        //public void Update(float elapsedTime)
        //{                    
        //    if (controller != null)
        //        controller.Update(elapsedTime);

        //    if (UpdateEvent != null)
        //        UpdateEvent(this, elapsedTime);

        //}

        public Asset CreateAsset()
        {
            return Asset.Create(this, name);
        }                      

        /// <summary>
        /// This method is called when the affector has influenced his affectable instance
        /// and the affactable needs to updates its GlobalPose. For Physics simulated objects this method is called after
        /// a simulation frame is completed
        /// </summary>
        public void UpdatePose(Matrix affectorPose)
        {
            _UpdateGlobalPose(affectorPose);

            foreach (var item in nodes)
            {
                item.CommitChanges();
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
            if (!nodes.ContainsKey(name))
            {
                foreach (var item in nodes)
                {
                    if (item.Contains(name))
                        return true;
                }
            }
            return false;
        }

        public bool Contains(SceneNode node)
        {
            return Contains(node.Name);
        }
       
        protected override void OnDispose(bool dispose)
        {          
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    node.Dispose();
                }
            }

            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectDestroyed(this);
            base.OnDispose(dispose);
        }

        public override string ToString()
        {
            return name ?? base.ToString();
        }
        
        #region Physics
       
        private static bool _IsActorMeta(string name, Scene scene, out bool dynamic, out SceneNode bindNode, out string newName, out bool disposed)
        {
            dynamic = false;
            bindNode = null;
            newName = null;
            disposed = true;

            var match = nodeMetaRegex.Match(name);
            if (!match.Success)
                match = nodeMetaRegexAlt.Match(name);

            if (match.Success)
            {
                dynamic = match.Groups["TYPE"].Success;

                var bindGroup = match.Groups["BINDING"];

                if (bindGroup.Success)
                    bindNode = scene.GetNode(bindGroup.Value);

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
            actorDesc.Name = name;
            actorDesc.GlobalPose = globalPose;

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
            if (nodeObject is IMeshContainer)
            {
                var mesh = ((IMeshContainer)nodeObject).Mesh;
                shapeDesc = mesh.CreateShapeDescriptor();
                if (shapeDesc == null)
                    shapeDesc = mesh.CreateTriangleMeshDescriptor();

                shapeDesc.Name = name;
                shapeDesc.LocalPose *= pose;                
            }
            else if (nodes.Count == 1)
            {
                shapeDesc = nodes[0]._CreateShapeDescriptor(nodes[0].LocalPose * pose);
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
        public Actor CreateActor(Physic physic, bool dynamic, Func<SceneNode, ActorShapeDesc> shapeBuilder, float density = 0, float mass = 0)
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

            if (nodeObject is IMeshContainer || nodes.Count == 1)
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
                foreach (var child in nodes)
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
                            var match = Regex.Match(child.tag, @"__m(?<X>\d+)((\.|_)(?<Y>\d+))?__");
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
            SceneNode bindNode;
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
                var nodesArray = this.nodes.ToArray();
                foreach (var subNode in nodesArray)
                {
                    subNode.CreateActors(scene);
                }
            }
        }

        #endregion

        public SceneNode Set(Action<SceneNode> action)
        {
            action(this);
            return this;
        }

        public T GetComponent<T>()
            where T : class, INodeObject
        {
            return nodeObject as T;
        }

        public SceneNode CreateChild<T>(T component) where T : INodeObject, INameable
        {
            var node = new SceneNode(component.Name, component);
            nodes.Add(node);
            return node;
        }

        public SceneNode CreateChild(string name, INodeObject component)
        {
            var node = new SceneNode(name, component);
            nodes.Add(node);
            return node;
        }

        public SceneNode CreateChild(string name, INodeObject component, Vector3 localPosition, Matrix localRotation, Vector3 localScale)            
        {
            var node = new SceneNode(name, component);
            node.LocalPosition = localPosition;
            node.LocalRotation = localRotation;
            node.LocalScale = localScale;
            node.UpdateLocalPose();
            node.CommitChanges();
            node.ComputeBoundingsShapes();          
            nodes.Add(node);
            return node;
        }

        public SceneNode CreateChild(string name, INodeObject component, Matrix localPose)
        {
            var node = new SceneNode(name, component);
            node.LocalPose = localPose;
            node.CommitChanges();
            node.ComputeBoundingsShapes();
            nodes.Add(node);
            return node;
        }

        public static SceneNode CreateNode(string name, INodeObject component, Matrix localPose)
        {
            var node = new SceneNode(name, component);
            node.LocalPose = localPose;
            node.CommitChanges();
            node.ComputeBoundingsShapes();            
            return node;
        }

        public static SceneNode CreateNode(string name, INodeObject component, Vector3 localPosition = default(Vector3),
              Matrix localRotation = default(Matrix), Vector3 localScale=default(Vector3))
        {
            var node = new SceneNode(name, component);
            node.LocalPosition = localPosition;            
            node.LocalRotation = localRotation == new Matrix()?Matrix.Identity: localRotation;
            node.LocalScale = localScale == Vector3.Zero? Vector3.One:localScale;
            node.UpdateLocalPose();
            node.CommitChanges();
            node.ComputeBoundingsShapes();                      
            return node;
        }

        public SceneNode Dynamic(Scene scene, Action<float> updateCallback)
        {
            this.isDynamic = true;
            scene.Dynamics.Add(new Dynamic(updateCallback));
            return this;
        }

        [Serializable]
        class Activator : IProviderActivator
        {
            string name;          
            AssetReference componentRef;
            Matrix  globalPose;          

            public void Initialize(IAssetProvider provider)
            {
                var node = (SceneNode)provider;
                var am = AssetManager.Instance;
                name = node.name;
                globalPose = node.globalPose;                       

                if (node.NodeObject != null)
                    componentRef = am.GetAssetReference(node.NodeObject);
            }

            public IAssetProvider CreateInstance()
            {
                var am = Service.Get<AssetManager>();                
                INodeObject comp = null;  
            
                if (componentRef != null)
                    comp = (INodeObject)am.GetAssetProvider(componentRef);

                var node = new SceneNode(name, comp);
                node.globalPose = globalPose;               
                return node;
            }
          
        }       
    }
     
}
