#pragma once
#include "EnumConverter.h"
#include "Resseting.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public ref class D3DSwapChain : public SwapChain ,IResetable
	{
		D3DPRESENT_PARAMETERS * _pp;
		ResetTarget ^ resseter;
	internal:
		IDirect3DSwapChain9 * _pSwapChain;

	public:
		D3DSwapChain(IDirect3DDevice9* device, SwapChainDesc desc);

		D3DSwapChain(IDirect3DSwapChain9* swapChain, RenderTarget^ backBuffer, DepthStencil^ stencil, D3DPRESENT_PARAMETERS * pp);

		void Set(IDirect3DSwapChain9* swapChain, RenderTarget^ backBuffer, DepthStencil^ stencil, D3DPRESENT_PARAMETERS * pp);


		OVERRIDE( void Present() );

		OVERRIDE(void ResizeBackBuffer(int width, int height) );

		virtual void DeviceReset(IDirect3DDevice9 * device);

		virtual void DeviceLost(IDirect3DDevice9 * device);

	protected:
		OVERRIDE( void OnDispose(bool) );

	};

}}