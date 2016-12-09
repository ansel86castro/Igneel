using System.Collections.Generic;
using Igneel.Assets;
using Igneel.SceneManagement;

namespace Igneel.SceneComponents
{                      
    public abstract class FrameComponent :Resource, IGraphicsProvider
    {

        public object UserData { get; set; }

        public virtual int SubmitGraphics(Scene scene,Frame node, ICollection<GraphicSubmit> collection)
        {
            return 0;
        }

        public virtual void OnSceneDetach(Scene scene)
        {
            
        }

        public virtual void OnSceneAttach(Scene scene)
        {
            
        }

        public virtual void OnNodeAttach(Frame node)
        {
            
        }

        public virtual void OnNodeDetach(Frame node)
        {
            
        }

        public virtual void OnPoseUpdated(Frame node)
        {
            
        }


        protected override void OnDispose(bool disposing)
        {
          
        }
    }    

      
    //public abstract class ExclusiveNodeObject : NodeObject
    //{
    //    SceneNode _node;         

    //    public ExclusiveNodeObject() { }

    //    public SceneNode Node { get { return _node; } }

    //    public override void OnNodeAttach(SceneNode node)
    //    {
    //        if (this._node != null)
    //            throw new InvalidOperationException("The object is already attached to another node \"" + this._node.Name + "\"");
    //        this._node = node;            
    //        //if (Scene != node.Scene)
    //        //{
    //        //    if (Scene != null)
    //        //        OnRemoveFromScene(Scene);

    //        //    if (node.Scene != null)
    //        //        OnAddToScene(node.Scene);
    //        //}
    //        //Scene = node.Scene;
    //        base.OnNodeAttach(node);
    //    }

    //    public override void OnNodeDetach(SceneNode node)
    //    {
    //        //if (Scene != null)
    //        //    OnRemoveFromScene(Scene);
    //        //Scene = null;
    //        this._node = null;
    //        base.OnNodeDetach(node);
    //    }

    //    //protected override void OnDispose(bool disposing)
    //    //{
    //    //    if (disposing)
    //    //    {
    //    //        if (Scene != null)
    //    //            OnRemoveFromScene(Scene);
    //    //    }
    //    //    base.OnDispose(disposing);
    //    //}
       
    //}
   
}
