#include "Stdafx.h"
#include "InputLayout.h"

namespace IgneelD3D10
{
	InputLayout10::InputLayout10(ID3D10InputLayout * input)
	{
		_input = input;
	}

	void InputLayout10::OnDispose(bool)
	{
		if(_input)
		{
			_input->Release();
			_input = NULL;
		}
	}	
}