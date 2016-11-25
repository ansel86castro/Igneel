#include "Stdafx.h"
#include "GraphicDevice.h"
#include "D3DRenderTarget.h"
#include "DepthStencil.h"
#include "D3DBlendState.h"
#include "SubResource.h"
#include "D3DSwapChain.h"

#define Surface IDirect3DSurface9*
#define Text IDirect3DTexture9*

namespace Igneel { namespace D3D9 {

	void D3D9GraphicDevice::InitializeOMStage()
	{		
		Array::Clear(_omRenderTargets,0, _omRenderTargets->Length);
		_omDepthStencil = nullptr;
		_omBackBuffer = nullptr;
		_omBackDepthStencil = nullptr;

		IDirect3DSurface9 * surf;
		SAFECALL( _pDevice->GetBackBuffer(0,0, D3DBACKBUFFER_TYPE::D3DBACKBUFFER_TYPE_MONO, & surf));

		D3DSURFACE_DESC desc;
		surf->GetDesc(&desc);

		Multisampling msaa(desc.MultiSampleType, desc.MultiSampleQuality);		

		_omRenderTargets[0] = gcnew D3DRenderTargetSurf(_pDevice, surf);
		_pDevice->GetDepthStencilSurface(&surf);
		_omDepthStencil = gcnew D3DDephtStencil(_pDevice, surf);
		_omBackDepthStencil = _omDepthStencil;
		_omBackBuffer = _omRenderTargets[0];

		IDirect3DSwapChain9* swapChain;
		SAFECALL( _pDevice->GetSwapChain(0, &swapChain));
		if(_swapChains->Count == 0)
		{				
			_swapChains->Add(gcnew D3DSwapChain(swapChain, _omBackBuffer, _omBackDepthStencil, _pp));
		}
		else
		{
			D3DSwapChain^ sc = static_cast<D3DSwapChain^>(_swapChains[0]);
			sc->Set(swapChain, _omBackBuffer, _omBackDepthStencil,_pp);

		}

		_omBlendState = D3DBlendState::GetFromDevice(_pDevice);
		_omDepthStencilStage = D3DDepthStencilState::GetFromDevice(_pDevice);
	}

	void D3D9GraphicDevice::OMSetBlendState()
	{
		static_cast<D3DBlendState^>(_omBlendState)->apply(_pDevice);
	}

	void D3D9GraphicDevice::OMSetDepthStencilState()
	{
		static_cast<D3DDepthStencilState^>(_omDepthStencilStage)->apply(_pDevice);
	}

	void D3D9GraphicDevice::OMSetDepthStencil(DepthStencil^ stencil)
	{
		D3DDephtStencil^ d3dRT = static_cast<D3DDephtStencil^>(stencil);
		SAFECALL( _pDevice->SetDepthStencilSurface(d3dRT->_pSurface));
	}

	RenderTarget^ D3D9GraphicDevice::CreateRenderTarget(int width, int height , Format format, Multisampling sampling, bool cpuReadEnable)
	{		
		return gcnew D3DRenderTargetSurf(_pDevice, width, height, format,sampling, cpuReadEnable);
	}

	RenderTarget^ D3D9GraphicDevice::CreateRenderTarget(SubResource^ subResource, Multisampling sampling, bool cpuReadEnable )
	{	
		return gcnew D3DRenderTargetSurf(_pDevice, subResource, cpuReadEnable, sampling);
	}

	DepthStencil^ D3D9GraphicDevice::CreateDepthStencil(int width, int height, Format format, Multisampling sampling, bool cpuReadEnable)
	{		
		return gcnew D3DDephtStencil(_pDevice, width, height , format, sampling, cpuReadEnable);
	}

	DepthStencil^ D3D9GraphicDevice::CreateDepthStencil(SubResource^ subResource)
	{		
		return gcnew D3DDephtStencil(_pDevice, subResource, false, Multisampling::Disable);
	}

	BlendState^ D3D9GraphicDevice::CreateBlendState(BlendDesc desc)
	{
		return gcnew D3DBlendState(desc);
	}

	DepthStencilState^ D3D9GraphicDevice::CreateDepthStencilState(DepthStencilDesc desc)
	{
		return gcnew D3DDepthStencilState(desc);
	}

	void D3D9GraphicDevice::OMSetRenderTargetsImp(int numTargets, array<RenderTarget^>^ renderTargets, DepthStencil^ dephtStencil)
	{
		for (int i = 0; i < numTargets ; i++)
		{
			D3DRenderTargetSurf^ d3dRT = static_cast<D3DRenderTargetSurf^>(renderTargets[i]);
			if(d3dRT->_pMsaaSurf)
			{
				SAFECALL( _pDevice->SetRenderTarget(i, d3dRT->_pMsaaSurf) );
			}
			else
			{
				SAFECALL( _pDevice->SetRenderTarget(i, d3dRT->_pSurface) );
			}

			if(_omRenderTargets[i]!=nullptr)
			{
				auto oldRT = static_cast<D3DRenderTargetSurf^>(_omRenderTargets[i]);
				oldRT->RenderEnd();
			}
		}
	}

	void D3D9GraphicDevice::OMSetRenderTargetImpl(int slot, RenderTarget^ renderTarget, DepthStencil^ dephtStencil)
	{
		D3DRenderTargetSurf^ d3dRT = static_cast<D3DRenderTargetSurf^>(renderTarget);
		if(d3dRT->_pMsaaSurf)
		{
			SAFECALL( _pDevice->SetRenderTarget(slot, d3dRT->_pMsaaSurf) );
		}
		else
		{
			SAFECALL( _pDevice->SetRenderTarget(slot, d3dRT->_pSurface) );
		}

		if(_omRenderTargets[slot]!=nullptr)
		{
			auto oldRT = static_cast<D3DRenderTargetSurf^>(_omRenderTargets[slot]);
			oldRT->RenderEnd();
		}

		if(dephtStencil!=nullptr)
		{
			SAFECALL( _pDevice->SetDepthStencilSurface(static_cast<D3DDephtStencil^>(dephtStencil)->_pSurface) );
		}
		
	}

	SwapChain^ D3D9GraphicDevice::CreateSwapChainImp(SwapChainDesc desc)
	{
		return gcnew D3DSwapChain(_pDevice, desc);
	}
	
}}