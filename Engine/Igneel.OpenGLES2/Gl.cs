using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.OpenGLES2
{
    public static class Gl
    {       
        [SuppressUnmanagedCodeSecurity, DllImport("libGLESv2.dll", EntryPoint = "glGetError", ExactSpelling = true)]
        public static extern All GetError();

        [SuppressUnmanagedCodeSecurity, DllImport("libGLESv2.dll", EntryPoint = "glGetString", ExactSpelling = true)]
        private static extern IntPtr _GetString(All name);
     
        public static unsafe string GetString(All name)
        {
            return new string((sbyte*)_GetString(name));
        }

 

 

    }
}
