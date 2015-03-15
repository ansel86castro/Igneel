#pragma once
#include "IPollable.h"

using namespace System;

 #define KEYDOWN(name, key) (name[key] & 0x80) 


namespace IgneelDirectInput {

	public ref class DKeyBoard sealed: public Keyboard , IPoollableInput
	{
	internal:
		IDirectInputDevice8* keyboard;
		Byte* buffer;
		bool pooled;
	public:
		DKeyBoard(IDirectInput8* input, InputContext^ context, HANDLE hEvent);

		OVERRIDE( bool IsKeyPressed(Keys key) );

		virtual void Pool();

		virtual void Reset();

	protected :
		OVERRIDE( void OnDispose(bool ));
	};

}