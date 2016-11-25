#include "Stdafx.h"
#include "BlendState.h"

namespace IgneelD3D10
{
	BlendState10::BlendState10(ID3D10BlendState *blendState, BlendDesc desc)
		:BlendState(desc)
	{
		_blendState = blendState;
	}

	void BlendState10::OnDispose(bool dispose)
	{
		if(_blendState)
		{
			_blendState->Release();
			_blendState = nullptr;
		}
	}
}