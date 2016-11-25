using Igneel.Graphics;
using Igneel.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Windows
{  
    public class WindowContext : GraphicContext
    {
        private IntPtr _hWnd;

        public WindowContext()
        {

        }

        public WindowContext(IntPtr hWnd)
        {
            this._hWnd = hWnd;
        }

        public IntPtr WindowHandle { get { return _hWnd; } set { _hWnd = value; } }
    }

    public class InputContext : IInputContext
    {
        private IntPtr _hWnd;

        public InputContext(IntPtr hWnd)
        {
            this._hWnd = hWnd;
        }


        public IntPtr WindowHandle { get { return _hWnd; } set { _hWnd = value; } }
    }
}
