namespace Igneel.SceneManagement
{
    public interface IFrameComponent
    {
        void OnNodeAttach(Frame node);

        void OnNodeDetach(Frame node);

        void OnPoseUpdated(Frame node);
    }
}
