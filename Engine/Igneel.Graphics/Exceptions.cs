using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class GraphicDeviceFailException : InvalidOperationException
    {
        public GraphicDeviceFailException(string message) : base(message) { }
    }

    public class FormatNotSupportedException : InvalidOperationException
    {
        public FormatNotSupportedException(Format format) : base() 
        {
            this.Format = format;
        }

        public Format Format { get; set; }
    }

    public class CapabiityNotSupportedException : NotImplementedException
    {
        public CapabiityNotSupportedException(string message)
            : base(message)
        {
            
        }
      
    }

    public class FilterNotSupportedException : InvalidOperationException
    {
        public FilterNotSupportedException(Filter f)
        {
            this.Filter = f;
        }

        public Filter Filter { get; set; }
    }

    public class InvalidMappingException : InvalidOperationException
    {
        public InvalidMappingException()
        {
        }

        public InvalidMappingException( string message):base(message)
        {
        }
    }

}
