using Igneel.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering.Animations
{
    public class BonesResetter : ITargetResetter
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

   
}
