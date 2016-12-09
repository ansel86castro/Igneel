#include "Stdafx.h"
#include "RasterizerState.h"

namespace IgneelD3D10
{
	RasterizerState10::RasterizerState10(ID3D10RasterizerState *state, RasterizerDesc desc)
		:RasterizerState(desc)
	{
		_state = state;
	}

	void RasterizerState10::OnDispose(bool dispose)
	{
		if(_state)
		{
			_state->Release();
			_state = nullptr;
		}
	}
}