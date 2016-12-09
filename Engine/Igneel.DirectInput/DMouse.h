#pragma once
#include "IPollable.h"

using namespace System;

#define KEYDOWN(name, key) (name[key] & 0x80) 
#define SAMPLE_BUFFER_SIZE 16

namespace IgneelDirectInput {

	public ref class DMouse sealed: public Mouse , IPoollableInput
	{
	internal:
		LPDIRECTINPUTDEVICE8 mouse;
		DIMOUSESTATE2* state;
		bool pooled;
	public:
		DMouse(IDirectInput8* input, InputContext^ context, HANDLE hEvent);

        virtual property int X { 
			int get() override; }

        virtual property int Y  {
			int get()override; }

        virtual property int Z { int get() override; }

        virtual bool IsButtonPresed(MouseButton button) override;

		virtual void Pool();

		virtual void Reset();

		void SetAcquire();

	protected :
		OVERRIDE( void OnDispose(bool ));
	};

}