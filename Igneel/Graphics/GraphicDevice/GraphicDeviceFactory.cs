using Igneel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{    
    public abstract class GraphicDeviceFactory:ResourceAllocator, IFactory<GraphicDevice, GraphicDeviceDesc>
    {
        public static GraphicDeviceFactory Instance { get { return Service.Get<GraphicDeviceFactory>(); } }

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

        public GraphicDevice CreateInstance(GraphicDeviceType type = GraphicDeviceType.Hardware)
        {
            GraphicDeviceDesc desc = new GraphicDeviceDesc
            {
                DriverType = type                 
            };
            return CreateInstance(desc);
        }

        public abstract GraphicDevice CreateInstance(GraphicDeviceDesc data);        
    }
}
