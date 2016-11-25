using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Techniques
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct HitTestId
    {
        public int Id;

        public HitTestId(int value)
        {
            Id = value;
        }

        public static implicit operator HitTestId(int id)
        {
            return new HitTestId { Id = id };
        }
    }
}
