using System;

namespace Igneel.IA.Resources
{
    [Flags]
    public enum GPUAccess
    {
        None = 0,
        Read = 0x00001,
        Write = 0x00002,
        ReadWrite = Read | Write
    }
}
