#pragma once

#include "IPollable.h"

using namespace System;
using namespace Igneel::Windows;

namespace IgneelDirectInput {

	public ref class DJoystick:public Joystick, IPoollableInput
	{
		LPDIRECTINPUTDEVICE8 joystick;
		LPDIJOYSTATE2 state;
	public:
		DJoystick(LPDIRECTINPUT8 input,  WindowContext^ context);

		virtual void Pool();

		virtual void Reset();

		OVERRIDE( void OnDispose(bool d));
		
	};
}