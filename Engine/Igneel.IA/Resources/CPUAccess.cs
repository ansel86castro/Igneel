using System;

namespace Igneel.IA.Resources
{
    [Flags]
    public enum CPUAccess
    {
        None = 0,
        /// <summary>
        /// The resource is to be mappable so that the CPU can read its contents. 
        /// </summary>
        Read = 0x20000,
        /// <summary>
        // The resource is to be mappable so that the CPU can change its contents.
        /// </summary>
        Write = 0x10000,

        ReadWrite = Read | Write
    }
}
