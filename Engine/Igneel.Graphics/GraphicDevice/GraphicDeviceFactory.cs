using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{    
    public abstract class GraphicDeviceFactory:ResourceAllocator
    {
        private static GraphicDevice _device;

        public static GraphicDeviceFactory Instance { get { return Service.Get<GraphicDeviceFactory>(); } }

        public static GraphicDevice Device { get { return _device; } }

        protected GraphicDeviceFactory()
        {            
            Service.Set<GraphicDeviceFactory>(this);
        }       

        //public abstract ShaderCode CompileFromMemory(
        //  string shaderCode,       
        //  ShaderMacro[] defines,
        //  Include include,
        //  string functionName,
        //  string profile,
        //  ShaderFlags flags);

        //public abstract ShaderCode CompileFromFile(
        //    string filename,
        //    ShaderMacro[] defines,
        //    Include include,
        //    string functionName,
        //    string profile,
        //    ShaderFlags flags);

        public GraphicDevice CreateDevice(GraphicDeviceType type = GraphicDeviceType.Hardware)
        {
            GraphicDeviceDesc desc = new GraphicDeviceDesc
            {
                DriverType = type                 
            };
            return CreateDevice(desc);
        }

        public GraphicDevice CreateDevice(GraphicDeviceDesc description)
        {
            return _device = CreateInstance(description);
        }

        public GraphicDevice CreateDevice(IGraphicContext context)
        {
            return CreateDevice(new GraphicDeviceDesc(context));
        }

        protected abstract GraphicDevice CreateInstance(GraphicDeviceDesc data);        
    }
}
