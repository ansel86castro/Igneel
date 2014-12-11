using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{

    public interface IGraphicResource : IResourceAllocator
    {
        ResourceType Type { get; }
    }

    public abstract class GraphicResource : ResourceAllocator,IGraphicResource
    {
        protected ResourceType _type;

        public GraphicResource()
        {
            _type = ResourceType.Unknown;
        }

        public GraphicResource(ResourceType type)
        {
            _type = type;
        }

        public ResourceType Type { get { return _type; } }
    }

    public interface ShaderResource : IGraphicResource
    {

    }
    
}
