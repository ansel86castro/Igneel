#include "Stdafx.h"
#include "D3DSwapChain.h"
#include "D3DRenderTarget.h"
#include "DepthStencil.h"

namespace Igneel { namespace D3D9 {

	D3DSwapChain::D3DSwapChain(IDirect3DDevice9* device, SwapChainDesc desc)
	{				
		resseter = gcnew ResetTarget(this);

		D3DPRESENT_PARAMETERS pp;
		pp.BackBufferCount = 1;
		pp.BackBufferFormat = GetD3DFORMAT(desc.BackBufferFormat);
		pp.BackBufferWidth = desc.BackBufferWidth;
		pp.BackBufferHeight = desc.BackBufferHeight;
		pp.EnableAutoDepthStencil = true;
		pp.AutoDepthStencilFormat = GetD3DFORMAT( desc.DepthStencilFormat );
		pp.Windowed = false;
		pp.Flags = D3DPRESENTFLAG_DISCARD_DEPTHSTENCIL;
		pp.SwapEffect  =D3DSWAPEFFECT::D3DSWAPEFFECT_DISCARD;
		pp.hDeviceWindow = *static_cast<HWND*>(desc.OutputWindow.ToPointer());
		pp.PresentationInterval = D3DPRESENT_INTERVAL_DEFAULT;
		pp.MultiSampleQuality = desc.Sampling.Quality;
		pp.MultiSampleType = (D3DMULTISAMPLE_TYPE)desc.Sampling.Count;
		
		IDirect3DSwapChain9* swap;
		SAFECALL( device->CreateAdditionalSwapChain(&pp, &swap) );
		_pSwapChain = swap;
		
		IDirect3DSurface9 * surf;
		SAFECALL( device->CreateDepthStencilSurface(pp.BackBufferWidth, pp.BackBufferHeight, pp.AutoDepthStencilFormat, pp.MultiSampleType, pp.MultiSampleQuality, true, &surf , NULL) );
		_depthStencil = gcnew D3DDephtStencil(device, surf);

		swap->GetBackBuffer(0, D3DBACKBUFFER_TYPE::D3DBACKBUFFER_TYPE_MONO, &surf);		
		_backBuffers = gcnew D3DRenderTargetSurf(device, surf);
		_desc = desc;

		_pp = new D3DPRESENT_PARAMETERS(pp);

		GC::AddMemoryPressure(sizeof(IDirect3DSwapChain9));
		GC::AddMemoryPressure(sizeof(D3DPRESENT_PARAMETERS));
	}

	D3DSwapChain::D3DSwapChain(IDirect3DSwapChain9* swapChain, RenderTarget^ backBuffer, DepthStencil^ stencil, D3DPRESENT_PARAMETERS * pp)
		:_pSwapChain(swapChain)
	{
		_pp = new D3DPRESENT_PARAMETERS(*pp);

		resseter = nullptr;
		this->_backBuffers = backBuffer;
		this->_depthStencil = stencil;
		_desc = SwapChainDesc();
		_desc.BackBufferFormat = GetFORMAT(pp->BackBufferFormat);
		_desc.BackBufferHeight = pp->BackBufferHeight;
		_desc.BackBufferWidth = pp->BackBufferWidth;
		_desc.DepthStencilFormat = GetFORMAT(pp->AutoDepthStencilFormat);
		_desc.OutputWindow = IntPtr(pp->hDeviceWindow);
		_desc.Sampling = Multisampling(pp->MultiSampleType, pp->MultiSampleQuality);		
		GC::AddMemoryPressure(sizeof(IDirect3DSwapChain9));
	}

	void D3DSwapChain::Present()
	{
		_pSwapChain->Present(NULL,NULL, NULL, NULL, 0 );
	}

	void D3DSwapChain::ResizeBackBuffer(int width, int height)
	{
		_desc.BackBufferWidth = width;
		_desc.BackBufferHeight = height;

		D3D9GraphicDevice^ device = static_cast<D3D9GraphicDevice^>(Engine::Graphics);

		DeviceLost(device->_pDevice);
		DeviceReset(device->_pDevice);
	}

	 void D3DSwapChain::OnDispose(bool disposing) 
	 {
		__super::OnDispose(disposing);
		if(_pSwapChain)
		 {
			 _pSwapChain->Release();
			_pSwapChain = NULL;
		  }
		 if(resseter!=nullptr)
			resseter->TargetDisposed();
		 delete _pp;
	 }

	 void D3DSwapChain::DeviceReset(IDirect3DDevice9* device)
	 {
		_pp->BackBufferWidth = _desc.BackBufferWidth;
		_pp->BackBufferHeight = _desc.BackBufferHeight;

		IDirect3DSwapChain9* swap;
		device->CreateAdditionalSwapChain(_pp, &swap);
		_pSwapChain = swap;
		
		IDirect3DSurface9 * surf;
		device->CreateDepthStencilSurface(_desc.BackBufferWidth, _desc.BackBufferHeight, _pp->AutoDepthStencilFormat, _pp->MultiSampleType, _pp->MultiSampleQuality, true, &surf , NULL);
		_depthStencil = gcnew D3DDephtStencil(device,surf);

		swap->GetBackBuffer(0, D3DBACKBUFFER_TYPE::D3DBACKBUFFER_TYPE_MONO, &surf);		
		_backBuffers = gcnew D3DRenderTargetSurf(device, surf);
	 }

	  void D3DSwapChain::DeviceLost(IDirect3DDevice9* device)
	 {
		 if(_pSwapChain)
		  {			 
			  /*_depthStencil->~DepthStencil();
			  _backBuffers->~RenderTarget();*/
			  delete _depthStencil;
			  delete _backBuffers;

			  _pSwapChain->Release();
			  _pSwapChain = NULL;
		 }
	 }

	  void D3DSwapChain::Set(IDirect3DSwapChain9* swapChain, RenderTarget^ backBuffer, DepthStencil^ stencil, D3DPRESENT_PARAMETERS * pp)
	  {		
		*_pp = * pp;
		_backBuffers = backBuffer;
		_depthStencil = stencil;		
		_desc.BackBufferFormat = GetFORMAT(pp->BackBufferFormat);
		_desc.BackBufferHeight = pp->BackBufferHeight;
		_desc.BackBufferWidth = pp->BackBufferWidth;
		_desc.DepthStencilFormat = GetFORMAT(pp->AutoDepthStencilFormat);
		_desc.OutputWindow = IntPtr(pp->hDeviceWindow);
		_desc.Sampling = Multisampling(pp->MultiSampleType, pp->MultiSampleQuality);	
	  }
}}