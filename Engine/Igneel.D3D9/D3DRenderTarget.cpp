#include "Stdafx.h"
#include "D3DRenderTarget.h"
#include "SubResource.h"
#include "GraphicDevice.h"

namespace Igneel { namespace D3D9 {	

	D3DRenderTargetSurf::D3DRenderTargetSurf(LPDIRECT3DDEVICE9 device, SubResource^ subResource, bool readEnable , Multisampling multisampling)
		:_pSurface(NULL), _pDevice(device) , _pSysMemSurface(NULL) , _pMsaaSurf(NULL)
	{		
		D3DSubResource^ subRes = static_cast<D3DSubResource^>(subResource);		
		_pSurface = subRes->_pSurface;
		
		Init(device, readEnable, multisampling);

		subRes->SubResourceLost += gcnew Action<D3DSubResource^>(this, &D3DRenderTargetSurf::ResourceLost);
		subRes->SubResourceReset += gcnew Action<D3DSubResource^>(this, &D3DRenderTargetSurf::ResourceReset);
	}

	D3DRenderTargetSurf::D3DRenderTargetSurf(LPDIRECT3DDEVICE9 device, IDirect3DSurface9 * surface)
		:_pSurface(surface), _pDevice(device) , _pSysMemSurface(NULL) , _pMsaaSurf(NULL)
	{
		D3DSURFACE_DESC desc;		
		surface->GetDesc(&desc);

		Init(device, false, Multisampling(desc.MultiSampleType, desc.MultiSampleQuality));
	}

	
	D3DRenderTargetSurf::D3DRenderTargetSurf(LPDIRECT3DDEVICE9 device, int width, int height , Format format, Multisampling sampling, bool cpuReadEnable)
		:_pSurface(NULL), _pDevice(device) , _pSysMemSurface(NULL) , _pMsaaSurf(NULL)
	{
		resseter = gcnew ResetTarget(this);

		IDirect3DSurface9* surf;
		SAFECALL(_pDevice->CreateRenderTarget(width, height, GetD3DFORMAT(format),  (D3DMULTISAMPLE_TYPE)sampling.Count, sampling.Quality, FALSE, &surf, NULL));

		_pSurface = surf;
		Init(device, cpuReadEnable, sampling);
	}

	void D3DRenderTargetSurf::Init(LPDIRECT3DDEVICE9 device, bool readEnable , Multisampling multisampling)
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
			device->CreateOffscreenPlainSurface(_width, _height, desc.Format, D3DPOOL_SYSTEMMEM, &ptempSurf, NULL);
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
				device->CreateRenderTarget(_width, _height, desc.Format, static_cast<D3DMULTISAMPLE_TYPE>(multisampling.Count), multisampling.Quality, FALSE, &ptempSurf,NULL);
				_pMsaaSurf = ptempSurf;
			}
			else
			{
				_pMsaaSurf = _pSurface;
			}
		}
	}

	void D3DRenderTargetSurf::OnDispose(bool disposing)
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

	MappedTexture2D D3DRenderTargetSurf::Map()
	{
		if(!_pSysMemSurface)
			throw gcnew InvalidOperationException("mapping is not supported for this resources");

		_pDevice->GetRenderTargetData(_pSurface, _pSysMemSurface);

		D3DLOCKED_RECT loc;
		_pSysMemSurface->LockRect(&loc, NULL, D3DLOCK_READONLY);

		MappedTexture2D map;
		map.DataPointer = static_cast<IntPtr>(loc.pBits);
		map.RowPitch = loc.Pitch;
		return map;
	}

	void D3DRenderTargetSurf::UnMap()
	{
		if(!_pSysMemSurface)
			throw gcnew InvalidOperationException("read is not supported for this resources");

		_pSysMemSurface->UnlockRect();
	}

	void D3DRenderTargetSurf::RenderEnd()
	{
		if(_pMsaaSurf && _pMsaaSurf != _pSurface)
		{
			_pDevice->StretchRect(_pMsaaSurf, NULL, _pSurface, NULL , D3DTEXF_LINEAR);
		}
	}

	void D3DRenderTargetSurf::DeviceReset(IDirect3DDevice9 * device)
	{
		IDirect3DSurface9* surf;
		device->CreateRenderTarget(_surfDesc->Width, _surfDesc->Height,_surfDesc->Format,  _surfDesc->MultiSampleType, _surfDesc->MultiSampleQuality, FALSE, &surf, NULL);
		_pSurface = surf;
		Init(device, _pSysMemSurface != NULL, _multisampling);
		
	}

	void D3DRenderTargetSurf::DeviceLost(IDirect3DDevice9 * device)
	{
		if(_pSurface)
			_pSurface->Release();

		if(_pSysMemSurface)
			_pSysMemSurface->Release();

		if(_pMsaaSurf && _pMsaaSurf != _pSurface)
			_pMsaaSurf->Release();		
	}

	void D3DRenderTargetSurf::ResourceReset(D3DSubResource^ res)
	{
		_pSurface = res->_pSurface;
		
		auto device = static_cast<D3D9GraphicDevice^>(Engine::Graphics)->_pDevice;
		Init(device, _pSysMemSurface != NULL, _multisampling);
	}

	void D3DRenderTargetSurf::ResourceLost(D3DSubResource^ res)
	{
		if(_pSysMemSurface)
			_pSysMemSurface->Release();

		if(_pMsaaSurf && _pMsaaSurf != _pSurface)
			_pMsaaSurf->Release();		
	}
}}