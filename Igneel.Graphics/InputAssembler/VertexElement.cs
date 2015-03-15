using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VertexElementAttribute : Attribute
    {
        public VertexElementAttribute()
        {          
            Format = IAFormat.Unused;
            Offset = -1;
        }
        public VertexElementAttribute(IASemantic semantic, byte usageIndex = 0, short stream = 0, IAFormat type = IAFormat.Unused, short offset = -1)
        {
            this.Semantic = semantic;
            this.UsageIndex = usageIndex;
            this.Stream = stream;        
            this.Format = type;
            this.Offset = offset;
        }

        public short Offset { get; set; }
        public short Stream { get; set; }
        public IASemantic Semantic { get; set; }        
        public byte UsageIndex { get; set; }
        public IAFormat Format { get; set; }

    }

    public struct VertexElement
    {
        public short Offset;
        public short Stream;
        public IASemantic Semantic;
        public byte UsageIndex;
        public IAFormat Format;

        public VertexElement(short stream, short offset, IAFormat format, IASemantic semantic, byte usageIndex)
        {
            Stream = stream;
            Offset = offset;
            Format = format;
            Semantic = semantic;
            UsageIndex = usageIndex;
        }
    }
}
