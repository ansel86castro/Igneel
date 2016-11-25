#include "Stdafx.h"
#include "SwapChain.h"
#include "RenderTarget.h"

using namespace Igneel::Windows;

namespace IgneelD3D10
{
	SwapChain10::SwapChain10(ID3D10Device* device, IDXGISwapChain* swapChain,  WindowContext^ wcontext)
		:SwapChain(wcontext)
	{
		_device = device;
		_swapChain = swapChain;		
		_presentInterval = (int)wcontext->Presentation;
		ID3D10Texture2D *pBackBuffer;
		SAFECALL(swapChain->GetBuffer( 0, __uuidof( ID3D10Texture2D ), (LPVOID*)&pBackBuffer ));
		_pBackBuffer = pBackBuffer;

		// Create the render target view
		ID3D10RenderTargetView* rtv;
		SAFECALL(device->CreateRenderTargetView(pBackBuffer, NULL, &rtv));

		D3D10_TEXTURE2D_DESC desc;
		pBackBuffer->GetDesc(&desc);

		_backBuffers = gcnew RenderTarget10(rtv, desc.Width, desc.Height, (Format)desc.Format, Multisampling(desc.SampleDesc.Count,desc.SampleDesc.Quality));
	}

	void SwapChain10::OnDispose(bool disposing)
	{
		if(_swapChain)
		{
			_swapChain->Release();
			_swapChain = NULL;
		}
		if(_pBackBuffer)
		{
			_pBackBuffer->Release();
			_pBackBuffer = NULL;
		}		

		__super::OnDispose(disposing);
	}

	void SwapChain10::Present()
	{
		SAFECALL(_swapChain->Present(_presentInterval, 0));		
	}

	void SwapChain10::ResizeBackBuffer(int width, int height)
	{
		DXGI_SWAP_CHAIN_DESC desc;
		_swapChain->GetDesc(&desc);

		auto renderTarget =  static_cast<RenderTarget10^>(_backBuffers);
		if(renderTarget->_rtv)
			renderTarget ->_rtv->Release();
		_pBackBuffer->Release();
		
		_swapChain->ResizeBuffers( 0, width, height, desc.BufferDesc.Format, desc.Flags);

		ID3D10Texture2D *pBackBuffer;
		SAFECALL(_swapChain->GetBuffer( 0, __uuidof( ID3D10Texture2D ), (LPVOID*)&pBackBuffer ));
		_pBackBuffer = pBackBuffer;
		
		D3D10_TEXTURE2D_DESC tdesc;
		pBackBuffer->GetDesc(&tdesc);		

		// Create the render target view
		ID3D10RenderTargetView* rtv;
		SAFECALL(_device->CreateRenderTargetView(pBackBuffer, NULL, &rtv));
		
		renderTarget->setRenderTargetView(rtv, tdesc.Width, tdesc.Height, (Format)tdesc.Format, Multisampling(tdesc.SampleDesc.Count , tdesc.SampleDesc.Quality));		
		renderTarget->OnResized();
	}
}