using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [Serializable]    
    public class GraphicDeviceDesc
    {
        public GraphicDeviceDesc()
        {
            Adapter = 0;
            DriverType = GraphicDeviceType.Hardware;
        }

        public GraphicDeviceDesc(IGraphicContext context)
            :this()
        {
            this.Context = context;
        }

        public int Adapter { get; set; }
        public GraphicDeviceType DriverType { get; set; }
        public IGraphicContext Context { get; set; }
    }
}
