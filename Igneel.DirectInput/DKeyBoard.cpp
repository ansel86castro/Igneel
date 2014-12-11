#include "Stdafx.h"
#include "DKeyBoard.h"

namespace IgneelDirectInput {

	DKeyBoard::DKeyBoard(IDirectInput8* input, IntPtr hwnd, HANDLE hEvent)
	{
		LPDIRECTINPUTDEVICE8 device;
		SAFECALL( input->CreateDevice(GUID_SysKeyboard ,&device, NULL) ); 
		keyboard = device;

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

		SAFECALL( keyboard->SetDataFormat(&c_dfDIKeyboard) ); 

//#ifdef DEBUG
//		SAFECALL( keyboard->SetCooperativeLevel(*(HWND*)&hwnd,  DISCL_BACKGROUND | DISCL_NONEXCLUSIVE) );
//#else
//		SAFECALL( keyboard->SetCooperativeLevel(*(HWND*)&hwnd,  DISCL_FOREGROUND | DISCL_NONEXCLUSIVE) );
//#endif

#ifdef DEBUG
		SAFECALL( keyboard->SetCooperativeLevel((HWND)hwnd.ToPointer(),  DISCL_BACKGROUND | DISCL_NONEXCLUSIVE) );
#else
		SAFECALL( keyboard->SetCooperativeLevel((HWND)hwnd.ToPointer(),  DISCL_BACKGROUND | DISCL_NONEXCLUSIVE) );
#endif
		SAFECALL(  keyboard->Acquire()) ;

		buffer = new Byte[256]; 
		GC::AddMemoryPressure(256);
		GC::AddMemoryPressure(sizeof(IDirectInputDevice8));

		SAFECALL( keyboard->Poll());
		SAFECALL( keyboard->GetDeviceState(256, buffer));
	}

	void DKeyBoard::Pool()
	{
		auto hr = keyboard->Poll();
		if(FAILED(hr))
		{
			return;
		}
		 keyboard->GetDeviceState(256, buffer);		
		 pooled = true;
	}

	void DKeyBoard::Reset()
	{
		if(pooled)
		{			
			if(FAILED(keyboard->Poll()))			
				return;			

			keyboard->GetDeviceState(256, buffer);		
			pooled = false;
		}
	}

	bool DKeyBoard::IsKeyPressed(Keys key)
	{		
		return KEYDOWN(buffer, static_cast<int>(key));
	}

	void DKeyBoard::OnDispose(bool disposing)
	{
		 if (keyboard) 
        { 
			 // Always unacquire device before calling Release(). 
            keyboard->Unacquire(); 
            keyboard->Release();
            keyboard = NULL; 

			delete[] buffer;
        } 


		__super::OnDispose(disposing);
	}

}