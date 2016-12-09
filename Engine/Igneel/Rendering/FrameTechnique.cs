using System.Collections.Generic;
using System.Linq;
using Igneel.Assets;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Rendering
{
    public abstract class FrameTechnique : Technique, IFrameComponent, IAffectable, ISceneComponent
    {
        protected LinkedList<Frame> nodes = new LinkedList<Frame>();
        protected Matrix GlobalToAffector = Matrix.Identity;
        private IAffector _affector;
        int _nbRenderEntries;
        int _nbAdded;
        protected FrameTechnique()
        {

        }

        public int NbEntries
        {
            get { return _nbRenderEntries; }
            set { _nbRenderEntries = value; }
        }

        public Matrix BindAffectorPose
        {
            get
            {
                return GlobalToAffector;
            }
            set
            {
                GlobalToAffector = value;
            }
        }

        [AssetMember(StoreType.Reference)]
        public IAffector Affector
        {
            get
            {
                return _affector;
            }
            set
            {
                _affector = value;
                if (_affector != null)
                    _affector.Affectable = this;
            }
        }

        public ICollection<Frame> Nodes { get { return nodes; } }

        public void OnNodeAttach(Frame node)
        {
            nodes.AddLast(node);
            //if (scene == null && node.Scene != null)
            //    OnAddToScene(node.Scene);
        }

        public void OnNodeDetach(Frame node)
        {
            nodes.Remove(node);
            //if (nodes.Count == 0 && scene !=null)
            //{
            //    OnRemoveFromScene(scene);
            //}
        }

        public void OnPoseUpdated(Frame node) { }

        public virtual void UpdatePose(Matrix affectorPose) { }
     
        public bool IsVisible(Camera camera)
        {
            foreach (var node in nodes)
            {
                if (camera.ViewFrustum.Contains(node.BoundingSphere))
                    return true;
            }
            return false;
        }

        //protected override void OnDispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        foreach (var node in nodes.ToArray())
        //        {
        //            node.Technique = null;
        //        }

        //        scene.Techniques.Remove(this);
        //    }

        //    base.OnDispose(disposing);
        //}

        public void OnSceneDetach(Scene scene)
        {
            _nbAdded--;
            if (scene != null && _nbAdded == 0)
            {
                scene.Techniques.Remove(this);
                scene = null;
            }
        }

        public void OnSceneAttach(Scene scene)
        {
            if (_nbAdded == 0)
            {
                scene.Techniques.Add(this);
            }
            _nbAdded++;
        }

        public void Remove()
        {
            foreach (var node in nodes.ToArray())
            {
                node.Technique = null;
            }
        }
    }
}
