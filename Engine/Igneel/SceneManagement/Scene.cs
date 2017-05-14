using System;
using System.Collections.Generic;
using Igneel.Animations;
using Igneel.Assets;
using Igneel.Collections;
using Igneel.Components;
using Igneel.Physics;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.States;
using Igneel.Techniques;
using Igneel.Graphics;

namespace Igneel.SceneManagement
{

    [Asset("VISUAL_SCENE")]
    [OnComplete("Init")]      
    //[ResourceActivator(typeof(Scene.Activator))]
    public class Scene : Resource, IDynamicNotificable, IDrawable
    {
        //public const int NbLayers = 3;
        //public const int TransparentLayer = NbLayers - 1;
        //public const int NonZWrite = 0;
        //public const int DefaultLayer = 1;
        //public const int InvalidLayer = -1;
        #region Private Members

        UnitOfMeasure _uom;

        /// <summary>
        /// Frames
        /// </summary>
        List<Frame> _nodes = new List<Frame>();
        /// <summary>
        /// Dynamic Objects
        /// </summary>
        List<IDynamic> _dynamics = new List<IDynamic>();
        /// <summary>
        /// Preprocessing Tecniques
        /// </summary>
        List<FrameTechnique> _techniques = new List<Igneel.Rendering.FrameTechnique>();
        /// <summary>
        /// Lights
        /// </summary>
        List<FrameLight> _lightNodes = new List<FrameLight>();
        /// <summary>
        /// Active Lightss
        /// </summary>
        List<FrameLight> _activelightNodes = new List<FrameLight>();       

        /// <summary>
        /// Cameras
        /// </summary>
        ObservedDictionary<string, Camera> _cameras = new ObservedDictionary<string, Camera>(null, null, x => x.Name);

        /// <summary>
        /// contains the items to be rendered in the current frame
        /// </summary>
        List<GraphicSubmit> _renderList = new List<GraphicSubmit>();
        /// <summary>
        /// contains the transparent items to be rendered in the current frame
        /// </summary>
        List<GraphicSubmit> _transparentList = new List<GraphicSubmit>();

        List<Frame> _geometries = new List<Frame>();

        List<IVisualMaterial> _materials = new List<IVisualMaterial>();

        List<Mesh> _meshes = new List<Mesh>();

        List<KeyFrameAnimation> _animations = new List<KeyFrameAnimation>();

        private List<Light> _lights = new List<Light>();    

        Camera _activeCamera;

        Physic _physics;

        GlobalLigth _globalLight = new GlobalLigth();

        QuadTree<Frame> _quadTree;
        ListCuller<Frame> _listCuller;
        NonCulledRegion _nonCulledRegion;

        List<GraphicSubmit> decalRenderList = new List<GraphicSubmit>();
        object userData;
        #endregion                                           

        public event UpdateEventHandler UpdateEvent;
           

        public UnitOfMeasure UnitOfDistance { get { return _uom; } set { _uom = value; } }

        public int RenderId { get; set; }

        [AssetMember]
        public GlobalLigth AmbientLight { get { return _globalLight; } set { _globalLight = value; } }

       
        public List<GraphicSubmit> VisibleComponents { get { return _renderList; } }

       
        public List<GraphicSubmit> TransparentVisibleComponents { get { return _transparentList; } }                           
       
        public ObservedDictionary<string, Camera> Cameras { get { return _cameras; } }
       
        public List<IDynamic> Dynamics { get { return _dynamics; } }
       
        public List<FrameTechnique> Techniques { get { return _techniques; } }
       
        public List<FrameLight> FrameLights { get { return _lightNodes; } }

        public List<Light> Lights { get { return _lights; } }
       
        public List<FrameLight> ActiveLights { get { return _activelightNodes; } }
        
        public List<Frame> Geometries { get { return _geometries; } }        
               
        public List<Frame> Nodes { get { return _nodes; } }

        public Camera ActiveCamera { get { return _activeCamera; } set { _activeCamera = value; } }

        public List<IVisualMaterial> Materials { get { return _materials; } }

        public List<Mesh> Meshes { get { return _meshes; } }

        public List<KeyFrameAnimation> Animations { get { return _animations; } }

        public List<IDrawable> Drawables { get; set; }

        public List<IGraphicsProvider> Decals { get; set; }

        public List<GraphicSubmit> DecalsCompoents { get { return decalRenderList; } }

        [AssetMember(storeAs: StoreType.Reference)]
        public Physic Physics
        {
            get { return _physics; }
            set
            {
                _physics = value;               
            }
        }
      
        public object UserData { get { return userData; } }

        public Scene()
            : this(null)
        {

        }

        public Scene(string name)
            : base(name, null)
        {
            _listCuller = new ListCuller<Frame>();
            _nonCulledRegion = new NonCulledRegion { Scene = this };            
            _uom = new UnitOfMeasure { Type = UOMType.Meters, Meters = 1.0f };

            Drawables = new List<IDrawable>();
            Decals = new List<IGraphicsProvider>();
        }

        public ICullRegion GetCullRegion(Frame frame)
        {
            if (frame.Bounding != null)
            {
                if (_quadTree != null)
                {
                    QuadTree<Frame>.CullRegion region = new QuadTree<Frame>.CullRegion(_quadTree);
                    if (_quadTree.Add(frame, region))
                        return region;
                    else
                    {
                        _listCuller.Add(frame);
                        return _listCuller;
                    }
                }
                else
                {
                    _listCuller.Add(frame);
                    return _listCuller;
                }
            }
            else
            {
                _nonCulledRegion.Add(frame);
                return _nonCulledRegion;
            }

        }

        public void InitCulling(RectangleF bound, int subDivitions)
        {
            if (_quadTree == null)
                _quadTree = new QuadTree<Frame>(bound, subDivitions);
            else
            {
                _quadTree.Resize(bound, subDivitions);
            }

            List<Frame> newItems = new List<Frame>();
            int count = _listCuller.Items.Count;
            for (int i = 0; i < count; i++)
            {
                Frame node = _listCuller.Items[i];
                if (!_quadTree.Add(node))
                {
                    newItems.Add(node);
                }
            }

            _listCuller.Items = newItems;
        }


        /// <summary>
        /// Updates the dynamic objects in the scene
        /// </summary>
        /// <param name="deltaT">The elapsedTime since the last update</param>
        public void Update(float deltaT)
        {
            if (UpdateEvent != null)
                UpdateEvent(this, deltaT);

            foreach (var item in _dynamics)            
                item.Update(deltaT);

            if (_physics != null)            
                _physics.Simulate(deltaT);

            //if (CharacterControllerManager.Instance != null)
            //    CharacterControllerManager.Instance.UpdateControllers();            
        }

        /// <summary>
        /// Update the preprocesing techniques
        /// </summary>
        public void UpdateTecniques()
        {
            if (_activeCamera == null) return;

            foreach (var technique in _techniques)
            {
                if (technique.Enable && technique.IsVisible(_activeCamera))
                    technique.Apply();
            }
        }
       
        public Frame FindNode(string name)
        {
            foreach (var item in _nodes)
            {
                var node = item.FindNode(name);
                if (node != null)
                    return node;
            }
            return null;
        }

        public void GetBoundingBox(out Vector3 min, out Vector3 max, Matrix trasform)
        {
            Vector3 maxWorld = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 minWorld = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            foreach (var n in Geometries)
            {
                if(n.Component is FrameSkin skin)                
                {
                    skin.Skin.GetBoundingBox(out Vector3 skinMinWorld, out Vector3 skinMaxWorld, trasform);
                    minWorld = Vector3.Min(skinMinWorld, minWorld);
                    maxWorld = Vector3.Max(skinMaxWorld, maxWorld);
                }
                else if(n.Component is FrameMesh meshContainer)
                {
                    meshContainer.Mesh.GetBoundingBox(out Vector3 meshMinWorld, out Vector3 meshMaxWorl, n.GlobalPose * trasform);
                    minWorld = Vector3.Min(meshMinWorld, minWorld);
                    maxWorld = Vector3.Max(meshMaxWorl, maxWorld);
                }
            }

            min = minWorld;
            max = maxWorld;
        }

        public Frame FindNode(int id)
        {
            foreach (var item in _nodes)
            {
                var node = item.FindNode(id);
                if (node != null)
                    return node;
            }
            return null;
        }

        /// <summary>
        /// Updates the items and lights to be rendered
        /// </summary>
        public void UpdateVisibleComponents()
        {
            //clear collections
            _renderList.Clear();
            _activelightNodes.Clear();
            _transparentList.Clear();

            if (_activeCamera == null)
                return;

            //get the visible objects in the quadtree
            if (_quadTree != null)
            {
                var values = _quadTree.CullItems(_activeCamera);
                foreach (var node in values)
                {
                    _AddComponentToVisibles(node);
                }
            }

            foreach (var node in _listCuller.GetVisibleObjects(_activeCamera))
            {
                _AddComponentToVisibles(node);
            }

            foreach (var node in _nonCulledRegion.Items)
            {
                _AddComponentToVisibles(node);
            }

            //Get the transparent objects
            foreach (var item in _renderList)
            {
                if (item.IsTransparent)
                    _transparentList.Add(item);
            }

            //sort the transparent objects based on the distance to the camera from  Max to Min
            _transparentList.Sort((x, y) => -x.Node.Range.CompareTo(y.Node.Range));

            //Get the visible lights
            foreach (var item in _lightNodes)
            {
                var light = item.Light;
                if (light.Enable && _activeCamera.Contains(item.GlobalPosition, light.EffectiveRange))
                {
                    _activelightNodes.Add(item);
                    item.IsActive = true;
                }
            }
        }

        private void _AddComponentToVisibles(Frame node)
        {
            var component = node.Component;
            node.Range = _activeCamera.Distance(node);

            if (component != null)
            {
                var nbEntries = component.SubmitGraphics(this, node, _renderList);
                var technique = node.Technique;
                if (technique != null)
                    technique.NbEntries += nbEntries;
            }
        }

        public List<GraphicSubmit> GetVisibleComponents()
        {
            UpdateVisibleComponents();
            return _renderList;
        }                              



        //protected override void OnDispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        //if (rootNode != null)
        //        //    rootNode.Dispose();
        //        //if (physScene != null)
        //        //    physScene.Dispose();
        //        //SceneManager.SceneManager.Scenes.Remove(this);                
        //    }

        //    base.OnDispose(disposing);
        //}
               
        public IEnumerable<Frame> EnumerateNodesPosOrden()
        {
            foreach (var item in _nodes)
            {
                foreach (var node in item.EnumerateNodesPosOrden())
                {
                    yield return node;
                }
            }
        }

        public IEnumerable<Frame> EnumerateNodesInPreOrden()
        {
            foreach (var item in _nodes)
            {
                foreach (var node in item.EnumerateNodesInPreOrden())
                {
                    yield return node;
                }
            }
        }       

        public Frame Create(string name = null,
            IGraphicsProvider component = null, 
            Vector3 localPosition=default(Vector3),
            Matrix localRotation =default(Matrix), 
            Vector3 localScale =default(Vector3),
            Action<Frame, float>updateCallback = null)             
        {
            if (name == null)
            {
                if (component != null && component is INameable)
                {
                    name = ((INameable)component).Name;
                }
                else name = "Node" + _nodes.Count;
            }
            var node = new Frame(name, component);
            node.LocalPosition = localPosition;
            node.LocalRotation = localRotation == default(Matrix) ?Matrix.Identity:localRotation;
            node.LocalScale = localScale == default(Vector3) ? Vector3.One : localScale;
            node.ComputeLocalPose();
            node.CommitChanges();            
            _nodes.Add(node);
            if (updateCallback != null)
            {              
                _dynamics.Add(new Dynamic(x => updateCallback(node, x)));
            }
            node.OnSceneAttach(this);
            return node;
        }

        public Frame Create(string name = null,
           IGraphicsProvider component = null,
           Vector3 localPosition = default(Vector3),
           Euler localRotationEuler = default(Euler),
           Vector3 localScale = default(Vector3),
           Action<Frame, float> updateCallback = null)
        {
            if (name == null)
            {
                if (component != null && component is INameable)
                {
                    name = ((INameable)component).Name;
                }
                else name = "Node" + _nodes.Count;
            }

            var node = new Frame(name, component);
            node.LocalPosition = localPosition;
            node.LocalEuler = localRotationEuler;
            node.LocalScale = localScale == default(Vector3) ? Vector3.One : localScale;
            node.ComputeLocalPose();
            node.CommitChanges();            
            _nodes.Add(node);
            if (updateCallback != null)
            {              
                _dynamics.Add(new Dynamic(x => updateCallback(node, x)));
            }
            node.OnSceneAttach(this);
            return node;
        }

        public Frame Create(string name, IGraphicsProvider component, Matrix localPose, Action<Frame, float> updateCallback = null)
        {
            var node = new Frame(name, component);
            node.LocalPose = localPose;            
            node.CommitChanges();            
            _nodes.Add(node);
            if (updateCallback != null)
            {               
                _dynamics.Add(new Dynamic(x => updateCallback(node, x)));
            }
            node.OnSceneAttach(this);
            return node;
        }

        //public SceneNode Create<T>(T component) where T : INodeObject, INameable
        //{
        //    var node = new SceneNode(component.Name, component);
        //    rootNodes.Add(node);
        //    return node;
        //}

        public void BindScene(Render render)
        {            
            var camera = ActiveCamera;
            var ambient = AmbientLight;
            var light = Light.Current;
            var technique = RenderManager.ActiveTechnique;

            if (camera != null )
            {
                render.Bind(camera);
                camera.IsGpuSync = true;
            }
            if (EngineState.Lighting.EnableAmbient )
            {
                render.Bind(ambient);
                ambient.IsGpuSync = true;
            }
            if (light != null)
            {
                
                render.Bind(light);
                light.IsGpuSync = true;
                
                var node = light.Node;
                if (node.Technique != null && node.Technique.Enable)
                    node.Technique.Bind(render);
            }
            technique.Bind(render);
        }

        public void UnBindScene(Render render)
        {            
            if (ActiveCamera != null)
                render.UnBind(ActiveCamera);

            render.UnBind(Light.Current);

            var technique = RenderManager.ActiveTechnique;
            technique.UnBind(render);
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in _nodes)
                {
                    item.Dispose();
                }
                foreach (var item in _techniques)
                {
                    item.Dispose();
                }
                if (_physics != null)
                    _physics.Dispose();
            }
        }

        //[Serializable]
        //class Activator : IResourceActivator
        //{
        //    AssetReference _package;
        //    string _name;
        //    private AssetReference _physics;

        //    public void Initialize(IAssetProvider provider)
        //    {                
        //        Scene scene = (Scene)provider;
        //        AssetContext.SetGlobal<Scene>(scene);
        //        _name = scene.Name;
        //        //descrip = new SceneDescription
        //        //{
        //        //    Name = scene.name,
        //        //    CreateRootNode = false,
        //        //    QuadTreeMaxSize = scene.quadTree.BoundRect.Width,
        //        //    QuadTreeMinSize = scene.quadTree.BoundRect.Width / (float)Math.Pow(2, scene.quadTree.MaxLevel)
        //        //};

        //        ContentPackage pk = AssetContext.GetGlobal<ContentPackage>();
        //        bool removePk = false;
        //        if (pk == null)
        //        {
        //            pk = new ContentPackage(scene._name);
        //            AssetContext.SetGlobal<ContentPackage>(pk);
        //            removePk = true;
        //        }

        //        scene.AddToPackage(pk);

        //        if (scene._physics != null)
        //        {
        //            _physics = AssetManager.Instance.GetAssetReference(scene._physics);
        //            AssetContext.SetGlobal<Physic>(scene._physics);
        //            //scene.physScene.AddToPackage(pk);
        //        }

        //        _package = AssetManager.Instance.GetAssetReference(pk);

        //        if (removePk)
        //        {
        //            AssetContext.SetGlobal<ContentPackage>(null);
        //        }
        //    }

        //    public IAssetProvider OnCreateResource()
        //    {
        //        Scene scene = new Scene();
        //        AssetContext.SetGlobal<Scene>(scene);

        //        if (_physics != null)
        //        {
        //            scene.Physics = (Physic)AssetManager.Instance.GetAssetProvider(_physics);
        //            AssetContext.SetGlobal<Physic>(scene._physics);
        //        }

        //        ContentPackage pk = (ContentPackage)AssetManager.Instance.GetAssetProvider(_package);

        //        AssetContext.SetGlobal<Scene>(null);
        //        AssetContext.SetGlobal<Physic>(null);
        //        scene.Nodes.AddRange(pk.Providers.FindAll(x => (x is SceneNode) && ((SceneNode)x).Parent == null).Cast<SceneNode>());

        //        return scene;
        //    }
        //}      

        public virtual void Draw()
        {
            RenderManager.ApplyTechnique();
            if (_physics != null && _physics.Visible)
            {
                var tech = Service.Require<PhysicDisplayTechnique>();
                tech.Apply();
            }
          
            foreach (var item in Drawables)
            {
                item.Draw();
            }

            foreach (var entry in GetDecalGraphics())
            {
                entry.Draw(PixelClipping.None);
            } 
        }

        public List<GraphicSubmit> GetDecalGraphics()
        {
            decalRenderList.Clear();
            foreach (var decal in Decals)
            {                
                decal.SubmitGraphics(this, null, decalRenderList);
            }          
            return decalRenderList;
        }
    }   
}
