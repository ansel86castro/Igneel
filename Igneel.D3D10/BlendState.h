#pragma once

using namespace Igneel::Graphics;
using namespace System::Runtime::InteropServices;

namespace IgneelD3D10
{
	public ref class BlendState10:public BlendState
	{
	internal:
		ID3D10BlendState *_blendState;
	public:
		BlendState10(ID3D10BlendState *blendState, BlendDesc desc);
		
	protected:
		OVERRIDE(void OnDispose(bool));
	};
}