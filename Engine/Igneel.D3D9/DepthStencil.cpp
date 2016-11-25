#include "Stdafx.h"
#include "DepthStencil.h"
#include "SubResource.h"

namespace Igneel { namespace D3D9 {

	D3DDephtStencil::D3DDephtStencil(LPDIRECT3DDEVICE9 device, int width, int height, Format format, Multisampling sampling, bool cpuReadEnable)
		:_pSurface(NULL),  _pSysMemSurface(NULL) , _pMsaaSurf(NULL)
	{
		IDirect3DSurface9* surf;

		SAFECALL( device->CreateDepthStencilSurface(width, height, GetD3DFORMAT(format),  (D3DMULTISAMPLE_TYPE)sampling.Count, sampling.Quality, TRUE, &surf, NULL) );
		_pSurface = surf;

		resseter = gcnew ResetTarget(this);

		Init(device, cpuReadEnable, sampling);
	}

	D3DDephtStencil::D3DDephtStencil(LPDIRECT3DDEVICE9 device, SubResource^ subResource, bool readEnable , Multisampling multisampling)
		:_pSurface(NULL),  _pSysMemSurface(NULL) , _pMsaaSurf(NULL)
	{
		D3DSubResource^ subRes = static_cast<D3DSubResource^>(subResource);		
		_pSurface = subRes->_pSurface;

		subRes->SubResourceLost += gcnew Action<D3DSubResource^>(this, &D3DDephtStencil::ResourceLost);
		subRes->SubResourceReset += gcnew Action<D3DSubResource^>(this, &D3DDephtStencil::ResourceReset);

		Init(device, readEnable, multisampling);
	}

	D3DDephtStencil::D3DDephtStencil(LPDIRECT3DDEVICE9 device, IDirect3DSurface9 * surface)
		:_pSurface(surface),  _pSysMemSurface(NULL) , _pMsaaSurf(NULL)
	{
		resseter = nullptr;
		D3DSURFACE_DESC desc;		
		surface->GetDesc(&desc);

		Init(device, false, Multisampling(desc.MultiSampleType, desc.MultiSampleQuality));
	}

	void D3DDephtStencil::Init(LPDIRECT3DDEVICE9 device, bool readEnable , Multisampling multisampling)
	{
		D3DSURFACE_DESC desc;
		_pSurface->GetDesc(&desc);
		_surfDesc = new D3DSURFACE_DESC(desc);

		_width = desc.Width;
		_height= desc.Height;				
		_format = GetFORMAT(desc.Format);		
		_multisampling = multisampling;

		IDirect3DSurface9* ptempSurf;

		if(readEnable)
		{			
			SAFECALL( device->CreateOffscreenPlainSurface(_width, _height, desc.Format, D3DPOOL_SYSTEMMEM, &ptempSurf, NULL) );
			_pSysMemSurface = ptempSurf;

			D3DLOCKED_RECT loc;
			_pSysMemSurface->LockRect(&loc, NULL, D3DLOCK_READONLY);
			_pSysMemSurface->UnlockRect();
			GC::AddMemoryPressure(loc.Pitch * _height);
		}

		if(multisampling.Count > 0)
		{
			if(desc.MultiSampleType == D3DMULTISAMPLE_NONE)
			{
				SAFECALL( device->CreateDepthStencilSurface(_width, _height, desc.Format, static_cast<D3DMULTISAMPLE_TYPE>(multisampling.Count), multisampling.Quality, TRUE, &ptempSurf,NULL) );
				_pMsaaSurf = ptempSurf;
			}
			else
			{
				_pMsaaSurf = _pSurface;
			}
		}
	}

	void D3DDephtStencil::OnDispose(bool disposing)
	{
		if(_pSurface)
			_pSurface->Release();

		if(_pSysMemSurface)
			_pSysMemSurface->Release();

		if(_pMsaaSurf && _pMsaaSurf != _pSurface)
			_pMsaaSurf->Release();		

		delete _surfDesc;

		 if(resseter!=nullptr)
			resseter->TargetDisposed();
	}

	void D3DDephtStencil::DeviceReset(IDirect3DDevice9 * device)
	{
		IDirect3DSurface9* surf;
		SAFECALL( device->CreateDepthStencilSurface(_surfDesc->Width, _surfDesc->Height,_surfDesc->Format,  _surfDesc->MultiSampleType, _surfDesc->MultiSampleQuality, TRUE, &surf, NULL) );

		_pSurface = surf;
		Init(device, _pSysMemSurface != NULL, _multisampling);
	}

	void D3DDephtStencil::DeviceLost(IDirect3DDevice9 * device)
	{
		if(_pSurface)
			_pSurface->Release();

		if(_pSysMemSurface)
			_pSysMemSurface->Release();

		if(_pMsaaSurf && _pMsaaSurf != _pSurface)
			_pMsaaSurf->Release();		
	}

	void D3DDephtStencil::ResourceReset(D3DSubResource^ res)
	{
		_pSurface = res->_pSurface;
		
		auto device = static_cast<D3D9GraphicDevice^>(Engine::Graphics)->_pDevice;
		Init(device, _pSysMemSurface != NULL, _multisampling);
	}

	void D3DDephtStencil::ResourceLost(D3DSubResource^ res)
	{
		if(_pSysMemSurface)
			_pSysMemSurface->Release();

		if(_pMsaaSurf && _pMsaaSurf != _pSurface)
			_pMsaaSurf->Release();		
	}
}}