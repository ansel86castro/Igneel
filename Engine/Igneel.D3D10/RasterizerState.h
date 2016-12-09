#pragma once

using namespace Igneel::Graphics;
using namespace System::Runtime::InteropServices;

namespace IgneelD3D10
{
	public ref class RasterizerState10:public RasterizerState
	{
	internal:
		ID3D10RasterizerState *_state;
	public:
		RasterizerState10(ID3D10RasterizerState *blendState, RasterizerDesc desc);
		
	protected:
		OVERRIDE(void OnDispose(bool));
	};
}