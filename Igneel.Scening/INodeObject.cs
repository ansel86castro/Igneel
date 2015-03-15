using Igneel.Assets;

using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering
{
    public interface ISceneElement
    {
        void OnRemoveFromScene(Scene scene);

        void OnAddToScene(Scene scene);
    }

      
    public interface INodeComponent
    {     
        void OnNodeAttach(SceneNode node);

        void OnNodeDetach(SceneNode node);

        void OnPoseUpdated(SceneNode node);        
    }
    
    public interface INodeObject:ISceneElement, INodeComponent, IBoundable, IResourceAllocator,IAssetProvider
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="collection"></param>
        /// <returns>the number of entries written to collection</returns>
        int GetGraphicObjects(SceneNode node, ICollection<DrawingEntry> collection);
    }
  
    public interface IInstanceContainer<T> : INodeObject
    {
        T Instance { get; }

        SceneNode Node { get; }
    }

      
    public abstract class NodeObject :ResourceAllocator, INodeObject
    {            
        protected Sphere sphere;
        protected OrientedBox box;
        int reference;

        public event Action<NodeObject, SceneNode> NodeAttached;
        public event Action<NodeObject,SceneNode> NodeDettached;
        public event Action<NodeObject, Scene> Removed;
        public event Action<NodeObject, Scene> Added;        

        public NodeObject() { }

        public int NodeReferences { get { return reference; } }

        [AssetMember]
        public Sphere BoundingSphere
        {
            get { return sphere; }
            set { sphere = value; }
        }

        [AssetMember]
        public OrientedBox BoundingBox
        {
            get { return box; }
            set { box = value; }
        }        

        public virtual void OnNodeAttach(SceneNode node)
        {          
            if (NodeAttached != null)
                NodeAttached(this, node);
        }

        public virtual void OnNodeDetach(SceneNode node)
        {            
            if (NodeDettached != null)
                NodeDettached(this, node);
        }

        public virtual void OnRemoveFromScene(Scene scene)
        {
            reference--;
            if (reference <= 0)
            {
                if (Removed != null)
                    Removed(this, scene);
            }
        }

        public virtual void OnAddToScene(Scene scene)
        {
            if (reference == 0)
            {              
                if (Added != null)
                    Added(this, scene);
            }
            reference++;
        }

        public virtual void OnPoseUpdated(SceneNode node) { }

        public virtual Asset CreateAsset()
        {
            return Asset.Create(this);
        }

        public virtual int GetGraphicObjects(SceneNode node, ICollection<DrawingEntry> collection)
        {
            return 0;
        }        
    }

      
    public abstract class ExclusiveNodeObject : NodeObject
    {
        SceneNode node;         

        public ExclusiveNodeObject() { }

        public SceneNode Node { get { return node; } }

        public override void OnNodeAttach(SceneNode node)
        {
            if (this.node != null)
                throw new InvalidOperationException("The object is already attached to another node \"" + this.node.Name + "\"");
            this.node = node;            
            //if (Scene != node.Scene)
            //{
            //    if (Scene != null)
            //        OnRemoveFromScene(Scene);

            //    if (node.Scene != null)
            //        OnAddToScene(node.Scene);
            //}
            //Scene = node.Scene;
            base.OnNodeAttach(node);
        }

        public override void OnNodeDetach(SceneNode node)
        {
            //if (Scene != null)
            //    OnRemoveFromScene(Scene);
            //Scene = null;
            this.node = null;
            base.OnNodeDetach(node);
        }

        //protected override void OnDispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (Scene != null)
        //            OnRemoveFromScene(Scene);
        //    }
        //    base.OnDispose(disposing);
        //}
       
    }
   
}
