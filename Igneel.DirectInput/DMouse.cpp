#include "Stdafx.h"
#include "DMouse.h"

namespace IgneelDirectInput {

	DMouse::DMouse(IDirectInput8* input, IntPtr hwnd, HANDLE hEvent)
	{
		LPDIRECTINPUTDEVICE8 device;
		SAFECALL( input->CreateDevice(GUID_SysMouse ,&device, NULL) ); 
		mouse = device;

		HRESULT hr = device->SetEventNotification(hEvent);
		if(hr ==  DI_OK || hr == DI_POLLEDDEVICE)
		{

		}
		if(hr == DIERR_ACQUIRED)
		{
			throw gcnew InvalidOperationException();
		}
		else if (hr == DIERR_HANDLEEXISTS)
		{
			throw gcnew InvalidOperationException();
		}
		else if(hr == DIERR_INVALIDPARAM)
		{
			throw gcnew InvalidOperationException();
		}
		else if (hr  == DIERR_NOTINITIALIZED)
		{
			throw gcnew InvalidOperationException();
		}

		SAFECALL( device->SetDataFormat(&c_dfDIMouse2) ); 

		//g_hMouseEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
		//if (g_hMouseEvent == NULL) {
		//		throw gcnew InvalidOperationException();
		//}
		//SAFECALL(mouse->SetEventNotification(g_hMouseEvent));

		//DIPROPDWORD dipdw;
		//// the header
		//dipdw.diph.dwSize       = sizeof(DIPROPDWORD);
		//dipdw.diph.dwHeaderSize = sizeof(DIPROPHEADER);
		//dipdw.diph.dwObj        = 0;
		//dipdw.diph.dwHow        = DIPH_DEVICE;
		//// the data
		//dipdw.dwData            = SAMPLE_BUFFER_SIZE;

		//SAFECALL(mouse->SetProperty(DIPROP_BUFFERSIZE, &dipdw.diph));

		SAFECALL( device->SetCooperativeLevel((HWND)hwnd.ToPointer(),  DISCL_BACKGROUND | DISCL_NONEXCLUSIVE) );
		SAFECALL(  device->Acquire()) ;

		state = new DIMOUSESTATE2();

		GC::AddMemoryPressure(sizeof(DIMOUSESTATE2));
		GC::AddMemoryPressure(sizeof(IDirectInputDevice8));

		SAFECALL( device->Poll() );
		SAFECALL( device->GetDeviceState(sizeof(DIMOUSESTATE2), state));
	}

	void DMouse::SetAcquire()
	{
		auto hr = mouse->Acquire();
        while( hr == DIERR_INPUTLOST )
            hr = mouse->Acquire();

		return;
	}

	void DMouse::Pool()
	{
		auto hr = mouse->Poll();
		if(FAILED(hr))
		{
			return;
		}
		mouse->GetDeviceState(sizeof(DIMOUSESTATE2), state);
		pooled = true;
	}

	void DMouse::Reset()
	{
		if(pooled)
		{
			if(FAILED(mouse->Poll()))
				return;

			mouse->GetDeviceState(sizeof(DIMOUSESTATE2), state);
			pooled = false;
		}
	}

	int DMouse::X::get()
	{
		return state->lX;
	}

	int DMouse::Y::get()
	{
		return state->lY;
	}

	int DMouse::Z::get()
	{
		return state->lZ;
	}

	bool DMouse::IsButtonPresed(MouseButton button)
	{
		int index = (int)button;
		if(index < 0 || index >7) return false;

		return state->rgbButtons[index] & 0x80;
	}

	void DMouse::OnDispose(bool disposing)
	{
		if(mouse)
		{
			mouse->Unacquire(); 
            mouse->Release();
            mouse = NULL; 
			delete state;
		}
		__super::OnDispose(disposing);
	}
}