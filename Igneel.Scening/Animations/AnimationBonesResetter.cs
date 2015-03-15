using Igneel.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering.Animations
{
    public class AnimationBonesResetter : ITargetResetter, IDeferreable
    {
        LinkedList<BonesResetter> resetters = new LinkedList<BonesResetter>();

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
