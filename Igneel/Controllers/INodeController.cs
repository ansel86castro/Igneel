using Igneel.Assets;
using Igneel.Components;
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
