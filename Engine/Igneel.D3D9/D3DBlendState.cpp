#include "Stdafx.h"
#include "D3DBlendState.h"

namespace Igneel { namespace D3D9 {

	void D3DBlendState::apply(LPDIRECT3DDEVICE9 device)
	{
		device->SetRenderState(D3DRS_ALPHABLENDENABLE, _state.BlendEnable);
		device->SetRenderState(D3DRS_BLENDOP, (DWORD)_state.BlendOp);
		device->SetRenderState(D3DRS_SRCBLEND, (DWORD)_state.SrcBlend);
		device->SetRenderState(D3DRS_DESTBLEND, (DWORD)_state.DestBlend);

		device->SetRenderState(D3DRS_SEPARATEALPHABLENDENABLE, (DWORD)_state.AlphaToCoverageEnable);
		device->SetRenderState(D3DRS_BLENDOPALPHA, (DWORD)_state.BlendOpAlpha);
		device->SetRenderState(D3DRS_SRCBLENDALPHA, (DWORD)_state.SrcBlendAlpha);
		device->SetRenderState(D3DRS_DESTBLENDALPHA, (DWORD)_state.SrcBlendAlpha);

		DWORD v = 0;
		if(_state.RenderTargetWriteMask & 0x00FF0000)
		   v|= D3DCOLORWRITEENABLE_RED;
		if(_state.RenderTargetWriteMask & 0x000000FF)
		   v|= D3DCOLORWRITEENABLE_BLUE;
		if(_state.RenderTargetWriteMask & 0x0000FF00)
		   v|= D3DCOLORWRITEENABLE_GREEN;
		if(_state.RenderTargetWriteMask & 0xFF000000)
		   v|= D3DCOLORWRITEENABLE_ALPHA;
		device->SetRenderState(D3DRS_COLORWRITEENABLE ,v);
		
	}

	D3DBlendState^  D3DBlendState::GetFromDevice(LPDIRECT3DDEVICE9 device)
	{
		DWORD s;
		BlendDesc _state;
		device->GetRenderState(D3DRS_ALPHABLENDENABLE, (DWORD*)&s);
		_state.BlendEnable = s;
		device->GetRenderState(D3DRS_BLENDOP, (DWORD*)&_state.BlendOp);
		device->GetRenderState(D3DRS_SRCBLEND, (DWORD*)&_state.SrcBlend);
		device->GetRenderState(D3DRS_DESTBLEND, (DWORD*)&_state.DestBlend);

		device->GetRenderState(D3DRS_SEPARATEALPHABLENDENABLE, (DWORD*)&s);
		_state.AlphaToCoverageEnable = s;
		device->GetRenderState(D3DRS_BLENDOPALPHA, (DWORD*)&_state.BlendOpAlpha);
		device->GetRenderState(D3DRS_SRCBLENDALPHA, (DWORD*)&_state.SrcBlendAlpha);
		device->GetRenderState(D3DRS_DESTBLENDALPHA, (DWORD*)&_state.SrcBlendAlpha);

		
		device->GetRenderState(D3DRS_COLORWRITEENABLE ,&s);
		_state.RenderTargetWriteMask = (s == 0xF)? 0xFFFFFFFF : 0x00000000;

		return gcnew D3DBlendState(_state);
	}

	void D3DDepthStencilState::apply(LPDIRECT3DDEVICE9 device)
	{
		device->SetRenderState( D3DRS_ZENABLE,_state.DepthEnable);
		device->SetRenderState( D3DRS_ZFUNC,(DWORD)_state.DepthFunc);
		device->SetRenderState( D3DRS_ZWRITEENABLE,(DWORD)_state.WriteEnable);
		
		device->SetRenderState(D3DRS_TWOSIDEDSTENCILMODE , true);
		device->SetRenderState( D3DRS_STENCILENABLE,(DWORD)_state.StencilEnable);		
		device->SetRenderState( D3DRS_STENCILMASK,(DWORD)_state.StencilReadMask);
		device->SetRenderState( D3DRS_STENCILWRITEMASK,(DWORD)_state.StencilWriteMask);

		device->SetRenderState( D3DRS_STENCILFAIL,(DWORD)_state.FrontFace.StencilFailOp);
		device->SetRenderState( D3DRS_STENCILPASS,(DWORD)_state.FrontFace.StencilPassOp);
		device->SetRenderState( D3DRS_STENCILFUNC,(DWORD)_state.FrontFace.StencilFunc);
		device->SetRenderState( D3DRS_STENCILZFAIL,(DWORD)_state.FrontFace.StencilDepthFailOp);

		device->SetRenderState( D3DRS_CCW_STENCILFAIL,(DWORD)_state.BackFace.StencilFailOp);
		device->SetRenderState( D3DRS_CCW_STENCILPASS,(DWORD)_state.BackFace.StencilPassOp);
		device->SetRenderState( D3DRS_CCW_STENCILFUNC,(DWORD)_state.BackFace.StencilFunc);
		device->SetRenderState( D3DRS_CCW_STENCILZFAIL,(DWORD)_state.BackFace.StencilDepthFailOp);
	}

	D3DDepthStencilState^ D3DDepthStencilState::GetFromDevice(LPDIRECT3DDEVICE9 device)
	{
		DepthStencilDesc _state;
		DWORD s;
		device->GetRenderState( D3DRS_ZENABLE, (DWORD*)&s);
		_state.DepthEnable = s;
		device->GetRenderState( D3DRS_ZFUNC,(DWORD*)&_state.DepthFunc);
		device->GetRenderState( D3DRS_ZWRITEENABLE,(DWORD*)&s);
		_state.WriteEnable = s;
		
	
		device->GetRenderState( D3DRS_STENCILENABLE,(DWORD*)&s);		
		_state.StencilEnable = s;
		device->GetRenderState( D3DRS_STENCILMASK,(DWORD*)&s);
		_state.StencilReadMask = s;
		device->GetRenderState( D3DRS_STENCILWRITEMASK,(DWORD*)&s);
		_state.StencilWriteMask = s;

		device->GetRenderState( D3DRS_STENCILFAIL,(DWORD*)&_state.FrontFace.StencilFailOp);
		device->GetRenderState( D3DRS_STENCILPASS,(DWORD*)&_state.FrontFace.StencilPassOp);
		device->GetRenderState( D3DRS_STENCILFUNC,(DWORD*)&_state.FrontFace.StencilFunc);
		device->GetRenderState( D3DRS_STENCILZFAIL,(DWORD*)&_state.FrontFace.StencilDepthFailOp);

		device->GetRenderState( D3DRS_CCW_STENCILFAIL,(DWORD*)&_state.BackFace.StencilFailOp);
		device->GetRenderState( D3DRS_CCW_STENCILPASS,(DWORD*)&_state.BackFace.StencilPassOp);
		device->GetRenderState( D3DRS_CCW_STENCILFUNC,(DWORD*)&_state.BackFace.StencilFunc);
		device->GetRenderState( D3DRS_CCW_STENCILZFAIL,(DWORD*)&_state.BackFace.StencilDepthFailOp);

		return gcnew D3DDepthStencilState(_state);
	}

}}