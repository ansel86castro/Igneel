#include "Stdafx.h"
#include "GraphicDevice.h"
#include "BlendState.h"
#include "DepthStencilState.h"
#include "RenderTarget.h"
#include "SwapChain.h"
#include "DeviceManager.h"
#include "Texture1D10.h"
#include "Texture2D10.h"
#include "Texture3D10.h"

using namespace Igneel::Windows;

namespace IgneelD3D10
{

	void GraphicDevice10::CreateDepthStencil(ID3D10Device * device, IDXGISwapChain* swapChain , OUT ID3D10Texture2D** dephtTexture,  OUT ID3D10DepthStencilView** depthStencilView )
	{
		//Initialize Depht Buffer
		ID3D10Texture2D* pDephtStencil;

		DXGI_SWAP_CHAIN_DESC swapDesc;
		swapChain->GetDesc(&swapDesc);

		// Create depth stencil texture
		D3D10_TEXTURE2D_DESC descDepth;
		ZeroMemory(&descDepth, sizeof(D3D10_TEXTURE2D_DESC));
		descDepth.Width = swapDesc.BufferDesc.Width;
		descDepth.Height = swapDesc.BufferDesc.Height;
		descDepth.MipLevels = 1;
		descDepth.ArraySize = 1;
		descDepth.Format = DXGI_FORMAT_D24_UNORM_S8_UINT;
		descDepth.SampleDesc = swapDesc.SampleDesc;
		descDepth.Usage = D3D10_USAGE_DEFAULT;
		descDepth.BindFlags = D3D10_BIND_DEPTH_STENCIL;
		descDepth.CPUAccessFlags = 0;
		descDepth.MiscFlags = 0;
		SAFECALL(device->CreateTexture2D( &descDepth, NULL, &pDephtStencil ));		
		
		 // Create the depth stencil view
	    D3D10_DEPTH_STENCIL_VIEW_DESC descDSV;
		ZeroMemory(&descDSV, sizeof(D3D10_DEPTH_STENCIL_VIEW_DESC));
		descDSV.Format = descDepth.Format;
		if(swapDesc.SampleDesc.Count == 1)
		{	
			descDSV.ViewDimension = D3D10_DSV_DIMENSION_TEXTURE2D;
			descDSV.Texture2D.MipSlice = 0;
		}
		else
		{
			descDSV.ViewDimension = D3D10_DSV_DIMENSION_TEXTURE2DMS;			
		}

		ID3D10DepthStencilView* dsv;
		if(FAILED(device->CreateDepthStencilView( pDephtStencil, &descDSV, &dsv )))
		{
			pDephtStencil->Release();
			throw gcnew InvalidOperationException();
		}

		*dephtTexture = pDephtStencil;
		*depthStencilView = dsv;		
	}

	OMInitialization GraphicDevice10::GetOMInitialization()
	{
		OMInitialization ini;
		ZeroMemory(&ini, sizeof(OMInitialization));

		ini.NbRenderTargets = D3D10_SIMULTANEOUS_RENDER_TARGET_COUNT;				

		//initialize blend stage
		ini.BlendState = CreateBlendState(BlendDesc(true));
		_device->OMSetBlendState(((BlendState10^)ini.BlendState)->_blendState,NULL, 0xffffffff );		

		//initialize depth test stage
		ini.DepthTestState = CreateDepthStencilState(DepthStencilStateDesc(true));
		_device->OMSetDepthStencilState(((DepthStencilState10^)ini.DepthTestState)->_pstate,0);

		if(_swapChain)
		{
			// create depth stencil buffer
			ID3D10Texture2D* depthTex;
			ID3D10DepthStencilView *depthView;
			CreateDepthStencil(_device, _swapChain, &depthTex, &depthView);
			ini.DepthBuffer  = gcnew DephtStencil10(depthView, depthTex);

			DXGI_SWAP_CHAIN_DESC swapDesc;
			_swapChain->GetDesc(&swapDesc);

			//initialize default swapChain 
			/*SwapChainDesc swapDesc10 = SwapChainDesc();
			swapDesc10.BackBufferWidth = swapDesc.BufferDesc.Width;
			swapDesc10.BackBufferHeight = swapDesc.BufferDesc.Height;
			swapDesc10.BackBufferFormat = (Format)swapDesc.BufferDesc.Format;
			swapDesc10.OutputWindow = IntPtr(swapDesc.OutputWindow);
			swapDesc10.Sampling.Count = swapDesc.SampleDesc.Count;
			swapDesc10.Sampling.Quality = swapDesc.SampleDesc.Quality;
			swapDesc10.Presentation = _desc->Interval;*/
			ini.SwapChain = gcnew SwapChain10(_device, _swapChain, (WindowContext^)Description->Context);
		}

		return ini;
	}

	void GraphicDevice10::OMSetBlendState(BlendState^ state)
	{
		auto desc = state->State;
		_device->OMSetBlendState(static_cast<BlendState10^>(state)->_blendState, (float*)&desc.BlendFactor, 0xFFFFFFFF);
	}

	void GraphicDevice10::OMSetDepthStencilState(DepthStencilState^ state)
	{
		_device->OMSetDepthStencilState(static_cast<DepthStencilState10^>(state)->_pstate, state->State.StencilRef);		
	}
	
	SwapChain^ GraphicDevice10::CreateSwapChainImp(IGraphicContext^ context)
	{		
		WindowContext^ wcontext =  reinterpret_cast<WindowContext^>(context);
		DXGI_SWAP_CHAIN_DESC d;			

		ZeroMemory( &d, sizeof(DXGI_SWAP_CHAIN_DESC) );
		d.BufferCount = 1;
		d.BufferDesc.Format  =  static_cast<DXGI_FORMAT>(context->BackBufferFormat);
		d.BufferDesc.Width = context->BackBufferWidth;
		d.BufferDesc.Height = context->BackBufferHeight;
		d.BufferDesc.RefreshRate.Denominator = 1;
		d.BufferDesc.RefreshRate.Numerator = 60;
		d.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;		
		d.OutputWindow = (HWND) wcontext->WindowHandle.ToPointer();	
		d.SampleDesc.Count = context->Sampling.Count;
		d.SampleDesc.Quality = context->Sampling.Quality;
		d.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;		
		d.Windowed = TRUE;

	   IDXGISwapChain * swapChain;
	   GraphicManager10^ manager = static_cast<GraphicManager10^>(GraphicDeviceFactory::Instance);
	   SAFECALL( manager->_factory->CreateSwapChain(_device, &d, &swapChain) );

	   return gcnew SwapChain10(_device, swapChain, wcontext);
	}

	RenderTarget^ GraphicDevice10::CreateRenderTarget(Texture2D^ texture, int subResource , int count)
	{				
		D3D10_RENDER_TARGET_VIEW_DESC vd;
		ZeroMemory(&vd, sizeof(D3D10_RENDER_TARGET_VIEW_DESC));

		Texture2D10^ tex = static_cast<Texture2D10^>(texture);
		vd.Format = (DXGI_FORMAT)tex->Format;
		auto description = tex->Description;
		if(description.ArraySize == 1)
		{				
			if(description.SamplerDesc.Count == 1)
			{
				vd.ViewDimension =  D3D10_RTV_DIMENSION_TEXTURE2D;
				vd.Texture2D.MipSlice = subResource % description.MipLevels;
			}
			else
			{				
				vd.ViewDimension = D3D10_RTV_DIMENSION_TEXTURE2DMS;					
			}
		}
		else
		{
			if(description.SamplerDesc.Count == 1)
			{
				vd.ViewDimension =  D3D10_RTV_DIMENSION_TEXTURE2DARRAY;
				vd.Texture2DArray.MipSlice = subResource % description.MipLevels;
				vd.Texture2DArray.FirstArraySlice = subResource / description.MipLevels;
				vd.Texture2DArray.ArraySize = count;				
			}
			else
			{				
				vd.ViewDimension =  D3D10_RTV_DIMENSION_TEXTURE2DMSARRAY;					
				vd.Texture2DMSArray.ArraySize = count;
				vd.Texture2DMSArray.FirstArraySlice =  subResource / description.MipLevels;
			}				
								
		}
		ID3D10RenderTargetView * view;
		SAFECALL(_device->CreateRenderTargetView(GetResource(texture), &vd, &view));
		return gcnew RenderTarget10(view, tex->Width >> vd.Texture2D.MipSlice , tex->Height >> vd.Texture2D.MipSlice, tex->Format, tex->MSAA);

	}

	DepthStencil^ GraphicDevice10::CreateDepthStencil(DepthStencilDesc^ desc)
	{
		int arraySize = desc->ArraySize;
		if(desc->Dimension == DepthStencilDimension::TEXTURECUBE)
		{
			arraySize = 6;
		}
		else if(desc->Dimension == DepthStencilDimension::TEXTURE2D)
			arraySize = 1;

		ID3D10Texture2D *pDephtStencil;
		// Create depth stencil texture
		D3D10_TEXTURE2D_DESC descDepth;
		ZeroMemory(&descDepth, sizeof(D3D10_TEXTURE2D_DESC));

		descDepth.Width = desc->Width;
		descDepth.Height = desc->Height;
		descDepth.MipLevels = 1;
		descDepth.ArraySize = arraySize;
		descDepth.Format = (DXGI_FORMAT)desc->Format;
		descDepth.SampleDesc.Count =  desc->Sampling.Count;
		descDepth.SampleDesc.Quality = desc->Sampling.Quality;
		descDepth.Usage = D3D10_USAGE_DEFAULT;
		descDepth.BindFlags = D3D10_BIND_DEPTH_STENCIL;
		descDepth.CPUAccessFlags = 0;		
		descDepth.MiscFlags = desc->Dimension == DepthStencilDimension::TEXTURECUBE? D3D10_RESOURCE_MISC_TEXTURECUBE:0;
		SAFECALL(_device->CreateTexture2D( &descDepth, NULL, &pDephtStencil ));		
		
		 // Create the depth stencil view
	    D3D10_DEPTH_STENCIL_VIEW_DESC descDSV;
		ZeroMemory(&descDSV, sizeof(D3D10_DEPTH_STENCIL_VIEW_DESC));
		descDSV.Format = descDepth.Format;
		if(descDepth.ArraySize == 1)
		{			
			if(descDepth.SampleDesc.Count == 1)
			{
				descDSV.ViewDimension = D3D10_DSV_DIMENSION_TEXTURE2D;
				descDSV.Texture2D.MipSlice = 0;
			}
			else
			{
				descDSV.ViewDimension = D3D10_DSV_DIMENSION_TEXTURE2DMS;			
			}
		}
		else
		{
			if(descDepth.SampleDesc.Count == 1)
			{
				descDSV.ViewDimension = D3D10_DSV_DIMENSION_TEXTURE2DARRAY;
				descDSV.Texture2DArray.MipSlice = 0;
			    descDSV.Texture2DArray.FirstArraySlice = 0;
				descDSV.Texture2DArray.ArraySize = desc->ArraySize;				
   
			}
			else
			{
				descDSV.ViewDimension = D3D10_DSV_DIMENSION_TEXTURE2DMSARRAY;				
			    descDSV.Texture2DMSArray.FirstArraySlice = 0;
				descDSV.Texture2DMSArray.ArraySize = desc->ArraySize;					
			}
		}

		ID3D10DepthStencilView* dsv;
		if(FAILED(_device->CreateDepthStencilView( pDephtStencil, &descDSV, &dsv )))
		{
			pDephtStencil->Release();
			throw gcnew InvalidOperationException();
		}

		return gcnew DephtStencil10(dsv, pDephtStencil);
	}

	BlendState^ GraphicDevice10::CreateBlendState(BlendDesc desc)
	{
		D3D10_BLEND_DESC bd;
		ZeroMemory(&bd, sizeof(D3D10_BLEND_DESC));

		bd.AlphaToCoverageEnable =desc.AlphaToCoverageEnable;
		for (int i = 0; i < 8; i++)		
		{	
			bd.BlendEnable[i] = desc.BlendEnable;
			bd.RenderTargetWriteMask[i] = 0;
			if(desc.WriteMask.R)
				bd.RenderTargetWriteMask[i] |= D3D10_COLOR_WRITE_ENABLE_RED;
			if(desc.WriteMask.B)
			   bd.RenderTargetWriteMask[i] |= D3D10_COLOR_WRITE_ENABLE_BLUE;
			if(desc.WriteMask.G)
			   bd.RenderTargetWriteMask[i] |=  D3D10_COLOR_WRITE_ENABLE_GREEN;
			if(desc.WriteMask.A)
				bd.RenderTargetWriteMask[i] |= D3D10_COLOR_WRITE_ENABLE_ALPHA;
		}
		
		bd.BlendOp = (D3D10_BLEND_OP)desc.BlendOp;
		bd.BlendOpAlpha = (D3D10_BLEND_OP)desc.BlendOpAlpha;

		bd.DestBlend  = (D3D10_BLEND)desc.DestBlend;
		bd.DestBlendAlpha =(D3D10_BLEND)desc.DestBlendAlpha;			
		bd.SrcBlend = (D3D10_BLEND) desc.SrcBlend;
		bd.SrcBlendAlpha = (D3D10_BLEND) desc.SrcBlendAlpha;

		ID3D10BlendState* blend;
		SAFECALL(_device->CreateBlendState(&bd,&blend));

		return gcnew BlendState10(blend, desc);

	}

	DepthStencilState^ GraphicDevice10::CreateDepthStencilState(DepthStencilStateDesc desc)
	{
		D3D10_DEPTH_STENCIL_DESC d;
		ZeroMemory(&d, sizeof(D3D10_DEPTH_STENCIL_DESC));
		d.BackFace = *(D3D10_DEPTH_STENCILOP_DESC*)&desc.BackFace;
		d.FrontFace = *(D3D10_DEPTH_STENCILOP_DESC*)&desc.FrontFace;
		d.DepthEnable = desc.DepthEnable;
		d.DepthWriteMask = (D3D10_DEPTH_WRITE_MASK)desc.WriteEnable;
		d.DepthFunc = (D3D10_COMPARISON_FUNC)desc.DepthFunc;
		d.StencilEnable = desc.StencilEnable;
		d.StencilReadMask = desc.StencilReadMask;
		d.StencilWriteMask = desc.StencilWriteMask;	
		
		ID3D10DepthStencilState* state;
		SAFECALL(_device->CreateDepthStencilState(&d, &state));

		return gcnew DepthStencilState10(state, desc);
	}

	void GraphicDevice10::OMSetRenderTargetsImp(int numTargets, array<RenderTarget^>^ renderTargets, DepthStencil^ dephtStencil)
	{
		if(numTargets > 7) 
			throw gcnew IndexOutOfRangeException("numTargets must be least than 8");

		ID3D10RenderTargetView* targets[8];
		ZeroMemory(targets, sizeof(ID3D10RenderTargetView*) * 8);

		ID3D10DepthStencilView* ds = NULL;

		DephtStencil10^ dview = static_cast<DephtStencil10^>(dephtStencil);		
		if(dview != nullptr)
		{
			ds = dview->_dsv;
		}
		if(renderTargets != nullptr)
		{
			interior_ptr<RenderTarget^>rtPter = &renderTargets[0];
			for (int i = 0; i < numTargets; i++, rtPter++)
			{
				//targets[i] = static_cast<RenderTarget10^>(renderTargets[i])->_rtv;
				auto rt = static_cast<RenderTarget10^>(*rtPter);
				targets[i] = rt !=nullptr ? rt->_rtv : NULL;
			}		

		}				
		_device->OMSetRenderTargets(numTargets, targets, ds);
	}

	void GraphicDevice10::OMSetRenderTargetImpl(RenderTarget^ renderTarget, DepthStencil^ dephtStencil)
	{
		RenderTarget10^ view = static_cast<RenderTarget10^>(renderTarget);
		DephtStencil10^ dview = static_cast<DephtStencil10^>(dephtStencil);

		ID3D10RenderTargetView* rt = NULL;
		ID3D10DepthStencilView* ds = NULL;

		if(view != nullptr)
		{
			rt = view->_rtv;			
		}
		if(dview != nullptr)
		{
			ds = dview->_dsv;
		}

		_device->OMSetRenderTargets(1, &rt, ds);
	}
	
}