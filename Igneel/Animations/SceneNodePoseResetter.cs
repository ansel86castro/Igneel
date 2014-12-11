using Igneel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{
    public interface ITargetResetter
    {
        void Reset();
    }

    public class BonesResetter:ITargetResetter
    {        
        Vector3 translation;
        Vector3 scale;
        Matrix rotation;
        BonesResetter[] childs;
        SceneNode bone;

        public BonesResetter() { }

        public BonesResetter(SceneNode node)
        {
            Init(node);
        }

        public SceneNode Bone { get { return bone; } }

        private void Init(SceneNode node)
        {
            this.bone = node;
            translation = node.LocalPosition;
            scale = node.LocalScale;
            rotation = node.LocalRotation;

            childs = new BonesResetter[node.Childrens.Count];
            for (int i = 0; i < childs.Length; i++)
            {
                childs[i] = new BonesResetter(node.Childrens[i]);
            }

            node.Disposing += node_Disposing;
        }

        void node_Disposing(object sender, EventArgs e)
        {
            bone = null;
        }

        public void Reset()
        {
            if (bone == null)
                throw new NullReferenceException("SceneNode is Null");

            bone.ComputeLocalPose(scale, rotation, translation);
            foreach (var item in childs)
            {
                item.Reset();
            }
        }

    }

    public class AnimationBonesResetter : ITargetResetter,IDeferreable
    {
        LinkedList<BonesResetter>resetters = new LinkedList<BonesResetter>();

        public AnimationBonesResetter(params Animation[] animations)
        {
            if (animations != null)
            {
                foreach (var item in animations)
                {
                    RegisterHeirarchys(item);
                }
            }
        }

        public void RegisterHeirarchys(Animation animation)
        {
            foreach (var node in animation.Nodes)
            {
                SceneNode sceneNode = node.Context.Target as SceneNode;
                if (sceneNode != null)
                {
                    SceneNode root = sceneNode.GetBoneRoot();
                    if (root != null && resetters.FirstOrDefault(x => x.Bone == root) == null)
                    {
                        resetters.AddLast(new BonesResetter(root));
                    }

                }
            }
        }

        public void Reset()
        {
            foreach (var r in resetters)
            {
                r.Reset();
            }
        }

        public IEnumerable<SceneNode> EnumerateRoots()
        {
            return resetters.Select(x => x.Bone);
        }

        public SceneNode[] GetRoots()
        {
            return resetters.Select(x => x.Bone).ToArray();
        }

        public void CommitChanges()
        {
            foreach (var item in resetters)
            {
                item.Bone.CommitChanges();
            }
        }
    }
}
