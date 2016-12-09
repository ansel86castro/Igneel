using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public struct DeviceInfo
    {
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public Format DisplayFormat { get; set; }
        public GraphicDeviceType DriverType { get; set; }
        public VertexProcessing VertexProcessing { get; set;}          
        public int SimultaneousRTCount { get; set; }
        public int MaxTextureHeight { get; set; }
        public int MaxTextureWidth { get; set; }
        public int MaxStreams { get; set; }
        public int MaxSimultaneousTextures { get; set; }
        public string DeviceName { get; set; }
        public int DeviceId { get; set; }
        public int RefreshRate { get; set; }
        public Multisampling MSAA { get; set; }
    }

}
