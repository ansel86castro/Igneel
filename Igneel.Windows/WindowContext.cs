﻿using Igneel.Graphics;
using Igneel.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Windows
{
    public class WindowContext : GraphicContext,IInputContext
    {
        private IntPtr hWnd;

        public WindowContext()
        {

        }

        public WindowContext(IntPtr hWnd)
        {
            this.hWnd = hWnd;
        }

        public IntPtr WindowHandle { get { return hWnd; } set { hWnd = value; } }
    }
}