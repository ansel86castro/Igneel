#include "Stdafx.h"
#include "DepthStencilState.h"

namespace IgneelD3D10
{
	DepthStencilState10::DepthStencilState10(ID3D10DepthStencilState *blendState, DepthStencilStateDesc desc)
		:DepthStencilState(desc)
	{
		_pstate = blendState;
	}

	void DepthStencilState10::OnDispose(bool dispose)
	{
		if(_pstate)
		{
			_pstate->Release();
			_pstate = nullptr;
		}
	}
}