#include "Stdafx.h"
#include "RasterizerState.h"


namespace Igneel { namespace D3D9 {

	void D3DRasterizerState::apply(LPDIRECT3DDEVICE9 device)
	{
		device->SetRenderState(D3DRS_CULLMODE, (DWORD)_state.Cull);
		device->SetRenderState(D3DRS_DEPTHBIAS, (DWORD)_state.DepthBias);		
		device->SetRenderState(D3DRS_SLOPESCALEDEPTHBIAS, (DWORD)_state.SlopeScaledDepthBias);
		device->SetRenderState(D3DRS_FILLMODE, (DWORD)_state.Fill);
		device->SetRenderState(D3DRS_MULTISAMPLEANTIALIAS, (DWORD)_state.MultisampleEnable);
		device->SetRenderState(D3DRS_SCISSORTESTENABLE, (DWORD)_state.ScissorEnable);
	}

	D3DRasterizerState^ D3DRasterizerState::GetFromDevice(LPDIRECT3DDEVICE9 device)
	{
		RasterizerDesc _state;
		device->GetRenderState(D3DRS_CULLMODE, (DWORD*)&_state.Cull);
		device->GetRenderState(D3DRS_DEPTHBIAS, (DWORD*)&_state.DepthBias);		
		device->GetRenderState(D3DRS_SLOPESCALEDEPTHBIAS, (DWORD*)&_state.SlopeScaledDepthBias);
		device->GetRenderState(D3DRS_FILLMODE, (DWORD*)&_state.Fill);
		device->GetRenderState(D3DRS_MULTISAMPLEANTIALIAS, (DWORD*)&_state.MultisampleEnable);
		DWORD  b;
		device->GetRenderState(D3DRS_SCISSORTESTENABLE, (DWORD*)&b);
		_state.ScissorEnable = b;

		return gcnew D3DRasterizerState(_state);
	}
}}