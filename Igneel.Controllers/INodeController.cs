using Igneel.Assets;
using Igneel.Scenering;
using Igneel.Scenering;
using System;
namespace Igneel.Controllers
{
    public interface INodeController : IAssetProvider
    {
        void Initialize(SceneNode node);

        SceneNode Node { get; }

        void Update(float elapsedTime);
    }
}
