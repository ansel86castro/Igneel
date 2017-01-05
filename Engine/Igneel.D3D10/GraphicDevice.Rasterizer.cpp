#include "Stdafx.h"
#include "GraphicDevice.h"
#include "RasterizerState.h"

namespace IgneelD3D10
{
	RSInitialization GraphicDevice10::GetRSInitialization()
	{
		RSInitialization ini = RSInitialization();

		/*ID3D10RasterizerState* rst;
		_device->RSGetState(&rst);	
		D3D10_RASTERIZER_DESC rd;
		rst->GetDesc(&rd);

		RasterizerDesc desc;
		desc.AntialiasedLineEnable = rd.AntialiasedLineEnable ;
		desc.Cull = (CullMode)rd.CullMode;
		desc.DepthBias = rd.DepthBias;
		desc.DepthBiasClamp = rd.DepthBiasClamp ;
		desc.DepthClipEnable = rd.DepthClipEnable;
		desc.Fill = (FillMode)rd.FillMode;		
		desc.MultisampleEnable = rd.MultisampleEnable; 
		desc.ScissorEnable = rd.ScissorEnable;
		desc.SlopeScaledDepthBias = rd.SlopeScaledDepthBias;*/		
		//ini.RasterizerState = gcnew RasterizerState10(rst, desc);
		ini.RasterizerState = CreateRasterizerState(RasterizerDesc(true));
		UINT n = 1;
		/* 
		RECT rec;
		_device->RSGetScissorRects(&n, &rec);*/		
		//ini.ScissorRect =  Igneel::Rectangle(rec.left,rec.top, rec.right - rec.left,rec.bottom - rec.top); 

		_device->RSSetState(((RasterizerState10^)ini.RasterizerState)->_state);

		D3D10_VIEWPORT vp;
		_device->RSGetViewports(&n, &vp);
		ini.Viewport = Graphics::ViewPort(vp.TopLeftX, vp.TopLeftY, vp.Width, vp.Height);

		return ini;
	}	

	void GraphicDevice10::RSSetState(RasterizerState^ value)
	{
		RasterizerState10^ ras = static_cast<RasterizerState10^>(value);
		_device->RSSetState(ras->_state);
	}

	void GraphicDevice10::RSSetViewPort(Graphics::ViewPort vp)
	{
		_device->RSSetViewports(1, (D3D10_VIEWPORT*)&vp);
	}

	void GraphicDevice10::RSSetScissorRects(Igneel::Rectangle rec) 
	{
		D3D10_RECT r;
		r.left=rec.X;
		r.top =rec.Y;
		r.right = rec.Right;
		r.bottom = rec.Bottom;
		_device->RSSetScissorRects(1, &r);
	}

	RasterizerState^ GraphicDevice10::CreateRasterizerState(RasterizerDesc desc)
	{
		D3D10_RASTERIZER_DESC rd;
		ZeroMemory(&rd, sizeof(D3D10_RASTERIZER_DESC));
		rd.AntialiasedLineEnable= desc.AntialiasedLineEnable;
		rd.CullMode = (D3D10_CULL_MODE)desc.Cull;
		rd.DepthBias = desc.DepthBias;
		rd.DepthBiasClamp= desc.DepthBiasClamp;
		rd.DepthClipEnable = desc.DepthClipEnable;
		rd.FillMode =  (D3D10_FILL_MODE)desc.Fill;
		rd.FrontCounterClockwise = desc.FrontCounterClockwise;
		rd.MultisampleEnable = desc.MultisampleEnable;
		rd.ScissorEnable = desc.ScissorEnable;
		rd.SlopeScaledDepthBias  =desc.SlopeScaledDepthBias;		

		ID3D10RasterizerState* rst;
		SAFECALL( _device->CreateRasterizerState(&rd,&rst));
		return gcnew RasterizerState10(rst, desc);
	}
}
