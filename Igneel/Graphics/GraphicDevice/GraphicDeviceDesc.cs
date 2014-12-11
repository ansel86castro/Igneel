using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class GraphicDeviceDesc
    {
        public int Adapter = 0;
        public bool FullScreen = false;
        public Format BackBufferFormat = Format.B8G8R8X8_UNORM;
        public Format DepthStencilFormat = Format.D24_UNORM_S8_UINT;
        public int BackBufferWidth = 800;
        public int BackBufferHeight = 600;
        public Multisampling MSAA = new Multisampling(0,0);
        public GraphicDeviceType DriverType = GraphicDeviceType.Hardware;
        public IntPtr WindowsHandle;
        public PresentionInterval Interval = PresentionInterval.Default;
    }
}
