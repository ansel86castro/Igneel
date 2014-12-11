#pragma once

using namespace Igneel::Graphics;
using namespace System::Runtime::InteropServices;

namespace IgneelD3D10
{
	public ref class SwapChain10:public SwapChain
	{
		IDXGISwapChain* _swapChain;
		ID3D10Texture2D * _pBackBuffer;	
		ID3D10Device * _device;
		int _presentInterval;
	internal:
		SwapChain10(ID3D10Device * device, IDXGISwapChain* swapChain, SwapChainDesc swapChainDesc);

	protected:
		OVERRIDE(void OnDispose(bool));
	public:
		OVERRIDE(void Present());

		OVERRIDE(void ResizeBackBuffer(int width, int height));
	};
}