#pragma once
#include "IPollable.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Threading;
using namespace Igneel::Windows;

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

		virtual Keyboard^ CreateKeyboard(IInputContext^ context) override; 

		virtual Mouse^ CreateMouse(IInputContext^ context) override;

		virtual array<Joystick^>^ CreateJoysticks(IInputContext^context) override;

		virtual bool CheckInputStates() override;

		virtual void ResetInputStates() override;
	};
}
