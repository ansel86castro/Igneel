using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [Flags]
    public enum IncludeType
    {
        /// <summary>
        /// The local directory. 
        /// </summary>
        Local,

        /// <summary>
        /// The system directory. 
        /// </summary>
        System
    }


    public interface Include
    {
        // Methods
        void Close(IntPtr stream);

        bool Open(IncludeType type, string fileName, IntPtr parentStream, out IntPtr Data , out int bytes);
    }
}
