#include "Stdafx.h"
#include "DJoystick.h"

namespace IgneelDirectInput {

	DJoystick::DJoystick(LPDIRECTINPUT8 input,  IntPtr hwnd)
	{
		LPDIRECTINPUTDEVICE8 device;
		SAFECALL( input->CreateDevice(GUID_SysMouse ,&device, NULL) ); 
		
		SAFECALL( device->SetDataFormat(&c_dfDIMouse) ); 
	}

	void DJoystick::Pool()
	{
		
	}

	void DJoystick::Reset()
	{

	}

	void DJoystick::OnDispose(bool d)
	{

	}
}