using Igneel.SceneManagement;

namespace Igneel.SceneComponents
{
    public class ComponentInstance:FrameComponent, IComponentInstance
    {
        private Frame _node;
        public Frame Node
        {
            get { return _node; }
        }

        public override void OnNodeAttach(Frame node)
        {
            base.OnNodeAttach(node);

            _node = node;
        }

        public override void OnNodeDetach(Frame node)
        {
            base.OnNodeDetach(node);

            _node = null;
        }
    }
}
