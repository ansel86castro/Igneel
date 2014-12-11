#pragma once
#include "IPollable.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Threading;
namespace IgneelDirectInput {

	public ref class DInputManager: public InputManager
	{
		IDirectInput8* input;
		HANDLE* _handles;
		int _nbHandles;
		List<IPoollableInput^>^ devices;
		float lastFrame;
	public :
		DInputManager();
		!DInputManager();

		HANDLE EnsureHanlesCapasity(int capasity);

		virtual Keyboard^ CreateKeyboard(IntPtr hwnd) override; 

		virtual Mouse^ CreateMouse(IntPtr hwnd) override;

		virtual array<Joystick^>^ CreateJoysticks(IntPtr hwnd) override;

		virtual bool CheckInputStates() override;

		virtual void ResetInputStates() override;
	};
}
