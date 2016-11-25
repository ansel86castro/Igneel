#pragma once

using namespace Igneel::Graphics;
using namespace System::Runtime::InteropServices;

namespace IgneelD3D10
{
	public ref class DepthStencilState10:public DepthStencilState
	{
	internal:
		ID3D10DepthStencilState *_pstate;
	public:
		DepthStencilState10(ID3D10DepthStencilState *state, DepthStencilStateDesc desc);
		
	protected:
		OVERRIDE(void OnDispose(bool));
	};
}