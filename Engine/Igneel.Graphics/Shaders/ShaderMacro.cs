﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ShaderMacro
    {
        public string Name;
        public string Value;
    }
}
