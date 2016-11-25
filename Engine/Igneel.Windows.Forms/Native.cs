using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Igneel.Windows.Forms
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NativeMessage
    {
        public IntPtr Handle;
        public uint Message;
        public IntPtr WParameter;
        public IntPtr LParameter;
        public uint Time;
        public Point Location;
    }

    public class Native
    {
        [DllImport("user32.dll")]
        public static extern bool PeekMessage(out NativeMessage message,
            IntPtr window, 
            uint messageFilterMinimum, 
            uint messageFilterMaximum, 
            uint shouldRemoveMessage);

        //[DllImport("Kernel32.dll")]
        //public static extern IntPtr CreateEventW(
        //    [MarshalAs(UnmanagedType.LPStruct)]IntPtr lpEventAttributes, 
        //    [MarshalAs(UnmanagedType.Bool)]bool bManualReset, 
        //    [MarshalAs(UnmanagedType.Bool)]bool bInitialState, 
        //    [MarshalAs(UnmanagedType.LPWStr)] String name);
        
        //public static extern bool CloseHandle(IntPtr hObject);
    }
}
