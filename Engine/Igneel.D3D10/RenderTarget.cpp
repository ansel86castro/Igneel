#include "Stdafx.h"
#include "RenderTarget.h"
#include "Texture2D10.h"

using namespace Igneel;

namespace IgneelD3D10
{
	RenderTarget10::RenderTarget10(ID3D10RenderTargetView* rtv , int width , int height, Format formar, Multisampling msaa)
		:RenderTarget(width, height, formar, msaa)
	{
		_rtv = rtv;				
	}

	void RenderTarget10::OnDispose(bool dispose)
	{
		if(_rtv)
		{
			_rtv->Release();			
		}
	}	

	void RenderTarget10::setRenderTargetView(ID3D10RenderTargetView * rtv , int width , int height, Format formar, Multisampling msaa)
	{
		this->_rtv  = rtv;
		_width = width;
		_height = height;
		_format = formar;
		_multisampling = msaa;
	}

	void RenderTarget10::GetRenderTargetData(Texture2D^ texture)
	{
		Texture2D10^tex = static_cast<Texture2D10^>(texture);
		ID3D10Resource* res;
		ID3D10Device* device;

		_rtv->GetResource(&res);
		_rtv->GetDevice(&device);

		device->CopySubresourceRegion(tex->_texture, 0, 0,0,0, res, 0, NULL);

		res->Release();
		device->Release();
	}

	DephtStencil10::DephtStencil10(ID3D10DepthStencilView* dsv ,ID3D10Texture2D* texture)		
	{
		D3D10_TEXTURE2D_DESC desc;
		texture->GetDesc(&desc);
		_dsv = dsv;		
		_texture = texture;
		_width = desc.Width;
		_height = desc.Height;
		_format =(Format)desc.Format;
		_multisampling.Count  =desc.SampleDesc.Count;
		_multisampling.Quality  = desc.SampleDesc.Quality;
	}

	void DephtStencil10::setDepthStencilView(ID3D10DepthStencilView * dsv , ID3D10Texture2D* texture)
	{
		D3D10_TEXTURE2D_DESC desc;
		texture->GetDesc(&desc);
		_dsv = dsv;		
		_texture = texture;
		_width = desc.Width;
		_height = desc.Height;
		_format =(Format)desc.Format;
		_multisampling.Count  =desc.SampleDesc.Count;
		_multisampling.Quality  = desc.SampleDesc.Quality;
	}

	void DephtStencil10::OnDispose(bool dispose)
	{
		if(_dsv)
		{
			_texture->Release();
			_dsv->Release();
			_dsv = NULL;
		}
	}	
}