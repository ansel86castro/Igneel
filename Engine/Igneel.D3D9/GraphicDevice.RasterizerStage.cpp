#include "Stdafx.h"
#include "GraphicDevice.h"
#include "RasterizerState.h"

namespace Igneel { namespace D3D9 {

	void D3D9GraphicDevice::InitializeRasterizerStage()
	{
		D3DVIEWPORT9 vp;
		_pDevice->GetViewport(&vp);
		_rsVewPort.Width = vp.Width;
		_rsVewPort.Height = vp.Height;
		_rsVewPort.MaxDepth = vp.MaxZ;
		_rsVewPort.MinDepth = vp.MinZ;
		_rsVewPort.TopLeftX = vp.X;
		_rsVewPort.TopLeftY = vp.Y;

		RECT rec;
		_pDevice->GetScissorRect(&rec);

		_rsScissorRect.X = rec.left;
		_rsScissorRect.Y = rec.top;
		_rsScissorRect.Width = rec.right - rec.left;
		_rsScissorRect.Height = rec.bottom - rec.top;

		_rsState = D3DRasterizerState::GetFromDevice(_pDevice);
	}

	 void D3D9GraphicDevice::RSSetState()
	 {
		 D3DRasterizerState^state = static_cast<D3DRasterizerState^>(_rsState);
		 state->apply(_pDevice);
	 }

	 void D3D9GraphicDevice::RSSetViewPort()
	 {
		 D3DVIEWPORT9 vp;		

		vp.Width  = _rsVewPort.Width;
		vp.Height = _rsVewPort.Height;
		vp.MaxZ   = _rsVewPort.MaxDepth;
		vp.MinZ   = _rsVewPort.MinDepth ;
		vp.X      = _rsVewPort.TopLeftX ;
		vp.Y      = _rsVewPort.TopLeftY ;

		_pDevice->SetViewport(&vp);
	 }

	 void D3D9GraphicDevice::RSSetScissorRects()
	 {
		 RECT rec;
		 rec.left =  _rsScissorRect.X;
		rec.top =  _rsScissorRect.Y;
		rec.bottom = _rsScissorRect.Y + _rsScissorRect.Height;
		rec.right = _rsScissorRect.X + _rsScissorRect.Width;

		_pDevice->SetScissorRect(&rec);
	 }

	 RasterizerState^ D3D9GraphicDevice::CreateRasterizerState(RasterizerDesc desc) 
	 {
		 return gcnew D3DRasterizerState(desc);
	 }
}}