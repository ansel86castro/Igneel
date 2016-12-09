using System.Collections.Generic;
using Igneel.SceneManagement;

namespace Igneel.SceneComponents
{
    public interface IGraphicsProvider : IResourceAllocator, ISceneComponent, IFrameComponent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">The location of the GraphicsProvider in the Scene</param>
        /// <param name="collection">the collection for holding the submited items</param>
        /// <returns>the number of entries added to collection</returns>
        int SubmitGraphics(Scene scene, Frame node, ICollection<GraphicSubmit> collection);
    }
}
