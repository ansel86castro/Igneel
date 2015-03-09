#include "Stdafx.h"
#include "DInputManager.h"
#include "DKeyBoard.h"
#include "DMouse.h"
#include "DJoystick.h"

namespace IgneelDirectInput {

	DInputManager::DInputManager()		
	{
		IDirectInput8* _input;
		HRESULT hr = DirectInput8Create( GetModuleHandle( NULL ), DIRECTINPUT_VERSION,
                                         IID_IDirectInput8, ( VOID** )&_input, NULL );
		 if( FAILED( hr ) )
				throw gcnew InvalidOperationException(L"DirectInput creation fail");

		 input = _input;
		 _handles = new HANDLE[6];
		 ZeroMemory(_handles, sizeof(HANDLE)* 6);
		 _nbHandles = 0;

		 devices = gcnew List<IPoollableInput^>();
		 lastFrame = 3.4E-38;
	}

	HANDLE DInputManager::EnsureHanlesCapasity(int size)
	{
		if(size >= 6)
		{
			auto temp = new HANDLE[size];
			ZeroMemory(temp, sizeof(HANDLE)* size);
			memcpy(temp, _handles, _nbHandles* sizeof(HANDLE));

			delete _handles;
			_handles = temp;
		}
		HANDLE h;
		if(!_handles[_nbHandles])
		{
			_handles[_nbHandles] = CreateEvent(NULL, false, false, NULL);
			 h = _handles[_nbHandles];
			_nbHandles++;
		}
		else
			 h = _handles[_nbHandles];

		return h;
	}

	Keyboard^ DInputManager::CreateKeyboard(IInputContext^ context)
	{
		WindowContext^ wc = static_cast<WindowContext^>(context);
		auto handle = EnsureHanlesCapasity(_nbHandles + 1);
		auto kb = gcnew DKeyBoard(input, wc , handle);			
		devices->Add(kb);
		return kb;
	}

	Mouse^ DInputManager::CreateMouse(IInputContext^ context)
	{
		WindowContext^ wc = static_cast<WindowContext^>(context);
		auto handle = EnsureHanlesCapasity(_nbHandles + 1);
		auto device =  gcnew DMouse(input, wc, handle);				
		devices->Add(device);
		return device;
	}

	array<Joystick^>^ DInputManager::CreateJoysticks(IInputContext^ context)
	{
		return nullptr;
	}

	
	bool DInputManager::CheckInputStates()
	{
		bool updated = false;
		for (int i = 0; i < devices->Count; i++)
		{
			devices[i]->Pool();
		}		
		
		//while (true)
		//{
		//	//DWORD result = WaitForMultipleObjects(_nbHandles, _handles, FALSE, 0);
		//	DWORD result = MsgWaitForMultipleObjects(_nbHandles, _handles, FALSE, 0, QS_INPUT);
		//	if(result <= WAIT_OBJECT_0 + _nbHandles - 1)
		//	{
		//		devices[result]->Pool();				
		//		updated = true;
		//	}
		//	else
		//	{			
		//		break;
		//	}
		//}
		return updated;
	}

	void DInputManager::ResetInputStates()
	{
		/*for (int i = 0; i < devices->Count; i++)
		{
			devices[i]->Reset();
		}*/

	}


	DInputManager::!DInputManager()
	{
		for (int i = 0; i < _nbHandles; i++)
		{
			CloseHandle(_handles[i]);
		}
		delete[] _handles;
	}
}