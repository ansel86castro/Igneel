using Igneel.Assets;
using Igneel.Rendering;
using Igneel.Scenering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public abstract class NodeTechnique : Technique, IAssetProvider, INodeComponent, IAffectable, ISceneElement
    {
        protected LinkedList<SceneNode> nodes = new LinkedList<SceneNode>();
        protected Matrix globalToAffector = Matrix.Identity;
        private IAffector affector;
        int nbRenderEntries;
        int nbAdded;
        protected NodeTechnique()
        {

        }

        public int NbEntries
        {
            get { return nbRenderEntries; }
            set { nbRenderEntries = value; }
        }

        public Matrix BindAffectorPose
        {
            get
            {
                return globalToAffector;
            }
            set
            {
                globalToAffector = value;
            }
        }

        [AssetMember(StoreType.Reference)]
        public IAffector Affector
        {
            get
            {
                return affector;
            }
            set
            {
                affector = value;
                if (affector != null)
                    affector.Affectable = this;
            }
        }

        public ICollection<SceneNode> Nodes { get { return nodes; } }

        public void OnNodeAttach(SceneNode node)
        {
            nodes.AddLast(node);
            //if (scene == null && node.Scene != null)
            //    OnAddToScene(node.Scene);
        }

        public void OnNodeDetach(SceneNode node)
        {
            nodes.Remove(node);
            //if (nodes.Count == 0 && scene !=null)
            //{
            //    OnRemoveFromScene(scene);
            //}
        }

        public void OnPoseUpdated(SceneNode node) { }

        public virtual void UpdatePose(Matrix affectorPose) { }

        public Asset CreateAsset() { return Asset.Create(this); }

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

        public void OnRemoveFromScene(Scene scene)
        {
            nbAdded--;
            if (scene != null && nbAdded == 0)
            {
                scene.Techniques.Remove(this);
                scene = null;
            }
        }

        public void OnAddToScene(Scene scene)
        {
            if (nbAdded == 0)
            {
                scene.Techniques.Add(this);
            }
            nbAdded++;
        }

        public void Remove()
        {
            foreach (var node in nodes.ToArray())
            {
                node.Technique = null;
            }
        }
    }

    public abstract class BindedSceneNodeTechnique<T> : NodeTechnique
        where T : NodeTechnique
    {

        public override void Bind(Render render)
        {
            render.Bind(ClrRuntime.Runtime.StaticCast<T>(this));
        }

        public override void UnBind(Render render)
        {
            render.UnBind(ClrRuntime.Runtime.StaticCast<T>(this));
        }
    }
}
