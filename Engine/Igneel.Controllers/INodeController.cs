using Igneel.Assets;
using System;
using Igneel;
using Igneel.SceneManagement;

namespace Igneel.Controllers
{
    public interface INodeController : IResource
    {
        void Initialize(Frame node);

        Frame Node { get; }

        void Update(float elapsedTime);
    }
}
