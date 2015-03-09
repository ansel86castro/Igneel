using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel.Graphics;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using System.Collections;
using Igneel.Collections;
using System.ComponentModel;
using Igneel.Assets;

using Igneel.Physics;
using Igneel.Animations;
using Igneel.Rendering;

namespace Igneel.Components
{       
   
    [OnComplete("LoadComplete")]
      
    [ProviderActivator(typeof(Scene.Activator))]  
    public class Scene : IAssetProvider, IAssetProviderNotificator, IDynamic, INameable
    {
        public const int NbLayers = 3;
        public const int TransparentLayer = NbLayers - 1;
        public const int NonZWrite = 0;
        public const int DefaultLayer = 1;
        public const int InvalidLayer = -1;
            
        string name;
        List<SceneNode> rootNodes = new List<SceneNode>();             
        Camera activeCamera;                            
        Physic physScene;
        GlobalLigth globalLight = new GlobalLigth();
        List<IDynamic> dynamics = new List<IDynamic>();
        List<NodeTechnique> techniques = new List<NodeTechnique>();             
        List<LightInstance> lightNodes = new List<LightInstance>();
        List<LightInstance> activelightNodes = new List<LightInstance>(); 
        QuadTree<SceneNode> quadTree;
        List<SceneNode> nonCullingProviderSuported = new List<SceneNode>();         
        ObservedDictionary<string, Camera> cameras = new ObservedDictionary<string, Camera>(null, null, x => x.Name);
        AnimationManager animationManager;        
        List<IGraphicObject> renderables = new List<IGraphicObject>();
        List<DrawingEntry> renderList = new List<DrawingEntry>();
        List<DrawingEntry> transparentList = new List<DrawingEntry>();
        List<SceneNode> geometries = new List<SceneNode>();
        ObservedDictionary<int, IIdentificable> identificables = new ObservedDictionary<int, IIdentificable>(null, null, x => x.Id);

        public event UpdateEventHandler UpdateEvent;      
        public event Action<Scene> SavingBegin;
        public event Action<Scene> SavingEnd;

        public Scene(string name = null, int maxQuadTreeLevel = 5)
        {
            Engine.Lock();

            try
            {
                this.name = name ?? "scene" + Engine.SceneManager.Scenes.Count;
                animationManager = new AnimationManager();
                Engine.SceneManager.Scenes.Add(this);                            
                quadTree = new QuadTree<SceneNode>(this, maxQuadTreeLevel);
            }
            finally
            {
                Engine.Unlock();
            }
        }              
        
        public string Name 
        { 
            get { return name; }
            set 
            {
                if (name != value)
                    Engine.SceneManager.Scenes.ChangeKey(name, value);         
                name = value;
            }
        }

        [AssetMember]
        public GlobalLigth AmbientLight { get { return globalLight; } set { globalLight = value; } }

       
        public List<DrawingEntry> RenderList { get { return renderList; } }

       
        public List<DrawingEntry> TransparentRenderList { get { return transparentList; } }
     
       
        public AnimationManager AnimManager { get { return animationManager; } }

       
        public ObservedDictionary<string, Camera> Cameras { get { return cameras; } }

       
        public List<IDynamic> Dynamics { get { return dynamics; } }

       
        public List<NodeTechnique> Techniques { get { return techniques; } }

       
        public List<LightInstance> Lights { get { return lightNodes; } }

       
        public List<LightInstance> ActiveLights { get { return activelightNodes; } }
        
        public List<SceneNode> Geometries { get { return geometries; } }

        public ObservedDictionary<int, IIdentificable> Identificables { get { return identificables; } }

               
        public List<SceneNode> Nodes { get { return rootNodes; } }

        public Camera ActiveCamera { get { return activeCamera; } set { activeCamera = value; } }        

        public ICuller<SceneNode> CullingProvider { get { return quadTree; } }

       
        public ICollection<SceneNode> NonCullingProviderSuported { get { return nonCullingProviderSuported; } }

       
        [AssetMember(typeof(CollectionStoreConverter<IGraphicObject>))]
        public List<IGraphicObject> Renderables { get { return renderables; } }

        [AssetMember(storeAs: StoreType.Reference)]
        public Physic Physics
        {
            get { return physScene; }
            set
            {
                physScene = value;
                if (physScene != null)
                    physScene.Scene = this;
            }
        }       
           
        //private void StartPhisics(float deltaT)
        //{
        //    physScene.Simulate(deltaT);
        //    physScene.FlushStream();
        //}

        //private void GetPhysicsResults(Action idleAction = null)
        //{
        //    bool called = false;
        //    while (!physScene.FetchResults(SimulationStatus.NX_RIGID_BODY_FINISHED, false))
        //    {
        //        if (idleAction != null && !called)
        //        {
        //            idleAction();
        //            called = true;
        //        }
        //    }

        //    if (idleAction != null && !called)
        //        idleAction();
        //}      

        public void Update(float deltaT)
        {
            if (UpdateEvent != null)
                UpdateEvent(this, deltaT);

            foreach (var item in dynamics)            
                item.Update(deltaT);

            if (physScene != null)            
                physScene.Simulate(deltaT);
            if (CharacterControllerManager.Instance != null)
            {
                CharacterControllerManager.Instance.UpdateControllers();
            }
        }

        public void ApplyTechniques()
        {
            if (activeCamera == null) return;

            foreach (var technique in techniques)
            {
                if (technique.Enable && technique.IsVisible(activeCamera))
                    technique.Apply();
            }
        }
       
        public SceneNode GetNode(string name)
        {
            foreach (var item in rootNodes)
            {
                var node = item.GetNode(name);
                if (node != null)
                    return node;
            }
            return null;
        }

        public SceneNode GetNode(int id)
        {
            foreach (var item in rootNodes)
            {
                var node = item.GetNode(id);
                if (node != null)
                    return node;
            }
            return null;
        }

        public void UpdateRenderLists()
        {          
            //clear collections
            renderList.Clear();
            activelightNodes.Clear();
            transparentList.Clear();

            if (activeCamera == null)
                return;

            //cull objects in the quadtree
            if (quadTree != null)
            {
                var values = quadTree.CullItems(activeCamera);
                foreach (var item in values)
                {
                    item.IsCulled = false;
                    var node = item.Value;
                    if (node.Visible)
                    {
                        var component = node.NodeObject;
                        node.Range = activeCamera.GetDistanceTo(node);

                        if (component != null)
                        {
                            var nbEntries = component.GetGraphicObjects(node, renderList);
                            var technique = node.Technique;
                            if (technique != null)
                                technique.NbEntries += nbEntries;
                        }
                    }
                }              
            }
            else 
            {
                //cull by heirarchy
                foreach (var item in rootNodes)
                {
                    item.GetRenderEntries(this, renderList);
                }                                
            }

            foreach (var item in nonCullingProviderSuported)
            {
                if (item.Visible)
                    item.GetRenderEntries(this,renderList);
            }

            foreach (var item in renderables)
            {               
                if (item.Visible)
                {                    
                    renderList.Add(new DrawingEntry
                    {
                        GraphicObject = item,
                        Render = item.GetRender(),    
                        IsTransparent = item.IsTransparent,
                    });
                }
            }
           
            foreach (var item in renderList)
            {
                if (item.IsTransparent)
                    transparentList.Add(item);
            }

            transparentList.Sort((x, y) => -x.Node.Range.CompareTo(y.Node.Range));

            foreach (var item in lightNodes)
            {
                var light = item.Instance;
                Sphere sphere = new Sphere(item.GlobalPosition, light.EffectiveRange);
                if (light.Enable && activeCamera !=null && activeCamera.ViewFrustum.Contains(sphere))
                {
                    activelightNodes.Add(item);
                    item.IsActive = true;
                }
            }
        }

        public List<DrawingEntry> GetRenderList()
        {
            UpdateRenderLists();
            return renderList;
        }                              

        //protected override void OnDispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        //if (rootNode != null)
        //        //    rootNode.Dispose();
        //        //if (physScene != null)
        //        //    physScene.Dispose();
        //        //Engine.SceneManager.Scenes.Remove(this);                
        //    }

        //    base.OnDispose(disposing);
        //}

        public void Remove()
        {
            Engine.SceneManager.Scenes.Remove(this);
        }

        public override string ToString()
        {
            return name ?? base.ToString();
        }

        public Asset CreateAsset()
        {
            return Asset.Create(this, name);
        }

        public void AddToPackage(ContentPackage pk)
        {
            foreach (var item in rootNodes)
            {
                pk.Providers.Add(item);
            }            
            foreach (var anim in animationManager.Animations)
            {
                pk.Providers.Add(anim);
            }
        }

        public void OnSavingBegin()
        {
            if (SavingBegin != null)
                SavingBegin(this);
        }

        public void OnSavingEnd()
        {
            if (SavingEnd != null)
                SavingEnd(this);
        }
   
        [Serializable]
        class Activator : IProviderActivator
        {
            AssetReference package;
            string name;
            private AssetReference physics;

            public void Initialize(IAssetProvider provider)
            {                
                Scene scene = (Scene)provider;
                AssetContext.SetGlobal<Scene>(scene);
                name = scene.Name;
                //descrip = new SceneDescription
                //{
                //    Name = scene.name,
                //    CreateRootNode = false,
                //    QuadTreeMaxSize = scene.quadTree.BoundRect.Width,
                //    QuadTreeMinSize = scene.quadTree.BoundRect.Width / (float)Math.Pow(2, scene.quadTree.MaxLevel)
                //};

                ContentPackage pk = AssetContext.GetGlobal<ContentPackage>();
                bool removePk = false;
                if (pk == null)
                {
                    pk = new ContentPackage(scene.name);
                    AssetContext.SetGlobal<ContentPackage>(pk);
                    removePk = true;
                }

                scene.AddToPackage(pk);

                if (scene.physScene != null)
                {
                    physics = AssetManager.Instance.GetAssetReference(scene.physScene);
                    AssetContext.SetGlobal<Physic>(scene.physScene);
                    scene.physScene.AddToPackage(pk);
                }

                package = AssetManager.Instance.GetAssetReference(pk);

                if (removePk)
                {
                    AssetContext.SetGlobal<ContentPackage>(null);
                }
            }

            public IAssetProvider CreateInstance()
            {
                Scene scene = new Scene();
                AssetContext.SetGlobal<Scene>(scene);

                if (physics != null)
                {
                    scene.Physics = (Physic)AssetManager.Instance.GetAssetProvider(physics);
                    AssetContext.SetGlobal<Physic>(scene.physScene);
                }

                ContentPackage pk = (ContentPackage)AssetManager.Instance.GetAssetProvider(package);

                AssetContext.SetGlobal<Scene>(null);
                AssetContext.SetGlobal<Physic>(null);
                scene.Nodes.AddRange(pk.Providers.FindAll(x => (x is SceneNode) && ((SceneNode)x).Parent == null).Cast<SceneNode>());

                return scene;
            }
        }

        public IEnumerable<SceneNode> EnumerateNodesPosOrden()
        {
            foreach (var item in rootNodes)
            {
                foreach (var node in item.EnumerateNodesPosOrden())
                {
                    yield return node;
                }
            }
        }

        public IEnumerable<SceneNode> EnumerateNodesInPreOrden()
        {
            foreach (var item in rootNodes)
            {
                foreach (var node in item.EnumerateNodesInPreOrden())
                {
                    yield return node;
                }
            }
        }       

        public SceneNode Create(string name = null,
            INodeObject component = null, 
            Vector3 localPosition=default(Vector3),
            Matrix localRotation =default(Matrix), 
            Vector3 localScale =default(Vector3),
            Action<SceneNode, float>updateCallback = null)             
        {
            if (name == null)
            {
                if (component != null && component is INameable)
                {
                    name = ((INameable)component).Name;
                }
                else name = "Node" + rootNodes.Count;
            }
            var node = new SceneNode(name, component);
            node.LocalPosition = localPosition;
            node.LocalRotation = localRotation == default(Matrix) ?Matrix.Identity:localRotation;
            node.LocalScale = localScale == default(Vector3) ? Vector3.One : localScale;
            node.UpdateLocalPose();
            node.CommitChanges();
            node.ComputeBoundingsShapes();
            rootNodes.Add(node);
            if (updateCallback != null)
            {
                node.IsDynamic = true;
                dynamics.Add(new Dynamic(x => updateCallback(node, x)));
            }
            node.OnAddToScene(this);
            return node;
        }

        public SceneNode Create(string name = null,
           INodeObject component = null,
           Vector3 localPosition = default(Vector3),
           Euler localRotationEuler = default(Euler),
           Vector3 localScale = default(Vector3),
           Action<SceneNode, float> updateCallback = null)
        {
            if (name == null)
            {
                if (component != null && component is INameable)
                {
                    name = ((INameable)component).Name;
                }
                else name = "Node" + rootNodes.Count;
            }

            var node = new SceneNode(name, component);
            node.LocalPosition = localPosition;
            node.LocalRotationEuler = localRotationEuler;
            node.LocalScale = localScale == default(Vector3) ? Vector3.One : localScale;
            node.UpdateLocalPose();
            node.CommitChanges();
            node.ComputeBoundingsShapes();
            rootNodes.Add(node);
            if (updateCallback != null)
            {
                node.IsDynamic = true;
                dynamics.Add(new Dynamic(x => updateCallback(node, x)));
            }
            node.OnAddToScene(this);
            return node;
        }

        public SceneNode Create(string name, INodeObject component, Matrix localPose, Action<SceneNode, float> updateCallback = null)
        {
            var node = new SceneNode(name, component);
            node.LocalPose = localPose;            
            node.CommitChanges();
            node.ComputeBoundingsShapes();
            rootNodes.Add(node);
            if (updateCallback != null)
            {
                node.IsDynamic = true;
                dynamics.Add(new Dynamic(x => updateCallback(node, x)));
            }
            node.OnAddToScene(this);
            return node;
        }

        //public SceneNode Create<T>(T component) where T : INodeObject, INameable
        //{
        //    var node = new SceneNode(component.Name, component);
        //    rootNodes.Add(node);
        //    return node;
        //}
    }   
}
