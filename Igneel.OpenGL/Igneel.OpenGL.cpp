// This is the main DLL file.

#include "stdafx.h"
#include <WinUser.h>
#include "Igneel.OpenGL.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace IgneelOpenGL {

	 Class1::Class1(IntPtr hWnd)
	 {		 
		 //glutInit(nullptr,nullptr);
		 
		 auto hdc = GetDC((HWND)hWnd.ToPointer());
		 auto wgldc = wglCreateContext(hdc);
		 wglMakeCurrent(hdc,wgldc);

		 auto version = glGetString(GL_VERSION_1_1);
		 auto ventor  = glGetString(GL_VENDOR);
		 String^ sVersion = Marshal::PtrToStringAnsi(IntPtr((void*)version));		 

		 wglDeleteContext(wgldc);

	 }
}