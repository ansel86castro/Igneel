using Igneel.SceneManagement;

namespace Igneel.SceneComponents
{
    public interface ISceneComponent
    {
        void OnSceneDetach(Scene scene);

        void OnSceneAttach(Scene scene);
    }
}
