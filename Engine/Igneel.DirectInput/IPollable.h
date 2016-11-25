#pragma once 
using namespace System;


namespace IgneelDirectInput {

	interface class IPoollableInput
	{		
		void Pool();

		void Reset();
	};
}