#include "stdafx.h"
#include "D3D9Textures.h"
#include "SubResource.h"

namespace Igneel { namespace D3D9 {

	D3D9Texture1D::D3D9Texture1D(IDirect3DDevice9* pDevice, Texture1DDesc^ desc): _pTexture(NULL)
	{
		DWORD  d3dUsage;
		D3DPOOL  d3dPool;
		D3DFORMAT format = GetD3DFORMAT(desc->Format);

		GetTextureUsage(desc->Usage, desc->BindFlags, desc->CPUAccessFlags, &d3dUsage, &d3dPool);

		IDirect3DTexture9 * pText;
		SAFECALL(pDevice->CreateTexture(desc->Width, 1, desc->MipLevels, d3dUsage, format, d3dPool, &pText, NULL));

		_pTexture = pText;

		_width = desc->Width;
		Description = desc;	
		_type = ResourceType::Texture1D;

		GC::AddMemoryPressure(sizeof(IDirect3DTexture9));

		_d3dUsage = d3dUsage;
		_d3dPool = d3dPool;
		_format = format;

		if(_d3dPool == D3DPOOL_DEFAULT)
			resseter = gcnew ResetTarget(this);

		else if(_d3dPool == D3DPOOL_MANAGED || _d3dPool == D3DPOOL_SYSTEMMEM)
		{
			D3DLOCKED_RECT locRec;
			SAFECALL(_pTexture->LockRect(0, &locRec, NULL, 0));
			SAFECALL(_pTexture->UnlockRect(0) );
			int size = 0;
			for (int i = 0; i < _pTexture->GetLevelCount(); i++)
			{
				size += locRec.Pitch >> i;
			}

			GC::AddMemoryPressure(size);
		}
	}

	D3D9Texture1D::D3D9Texture1D(IDirect3DTexture9* tex)
	{
		_pTexture  = tex;

		D3DSURFACE_DESC desc;
		tex->GetLevelDesc(0, &desc);
					
		_desc->Format = GetFORMAT(desc.Format);
		_desc->ArraySize = 0;
		_desc->BindFlags = BindFlags::ShaderResource;
		_desc->CPUAccessFlags = CpuAccessFlags::Read | CpuAccessFlags::Write;
		_desc->Usage = ResourceUsage::Default;
		_desc->MipLevels = tex->GetLOD();
		_desc->Width  = desc.Width;
		_desc->Options = ResourceOptionFlags::None;
		TextureDescription = _desc;

		_d3dUsage = desc.Usage;
		_d3dPool = desc.Pool;
		_format = desc.Format;
		_type = ResourceType::Texture1D;
		_width = desc.Width;

		GC::AddMemoryPressure(sizeof(IDirect3DTexture9));		

		if(_d3dPool == D3DPOOL_DEFAULT)
			resseter = gcnew ResetTarget(this);

		else if(_d3dPool == D3DPOOL_MANAGED || _d3dPool == D3DPOOL_SYSTEMMEM)
		{
			D3DLOCKED_RECT locRec;
			SAFECALL(_pTexture->LockRect(0, &locRec, NULL, 0));
			SAFECALL(_pTexture->UnlockRect(0) );
			int size = 0;
			for (int i = 0; i < _pTexture->GetLevelCount(); i++)
			{
				size += locRec.Pitch >> i;
			}

			GC::AddMemoryPressure(size);
		}		
	}

	IntPtr D3D9Texture1D::Map(int subResource, MapType map, bool doNotWait)
	{
		D3DLOCKED_RECT locRec;
		SAFECALL(_pTexture->LockRect(subResource, &locRec, NULL, GetD3DLOCK(map, doNotWait)));
		return  static_cast<IntPtr>(locRec.pBits);
	}	

	void D3D9Texture1D::UnMap(int subResource)
	{
		SAFECALL(_pTexture->UnlockRect(subResource));			
	}

	void D3D9Texture1D::OnDispose(bool disposing)
	{
		if(_pTexture)
			_pTexture->Release();

		if(resseter!=nullptr)
			resseter->TargetDisposed();
		
	}

	SubResource^ D3D9Texture1D::GetSubResource(int level)
	{		
		return gcnew D3DSubResource1D(level, this);
	}

	void D3D9Texture1D::DeviceReset(IDirect3DDevice9 * device)
	{
		if(_d3dPool == D3DPOOL_DEFAULT)
		{
			IDirect3DTexture9 * pText;
			SAFECALL(device->CreateTexture(_desc->Width, 1, _desc->MipLevels, _d3dUsage, _format, _d3dPool, &pText, NULL));
			_pTexture = pText;

			TextureReset(this);
		}
	}

	void D3D9Texture1D::DeviceLost(IDirect3DDevice9 * device)
	{
		if(_d3dPool == D3DPOOL_DEFAULT)
		{
			TextureLost(this);

			if(_pTexture)
				_pTexture->Release();		
		}
	}


	D3D9Texture2D::D3D9Texture2D(IDirect3DDevice9* pDevice, Texture2DDesc^ desc): _pTexture(NULL)
	{
		DWORD  d3dUsage;
		D3DPOOL  d3dPool;
		D3DFORMAT format = GetD3DFORMAT(desc->Format);

		GetTextureUsage(desc->Usage, desc->BindFlags, desc->CPUAccessFlags, &d3dUsage, &d3dPool);

		IDirect3DTexture9 * pText;
		SAFECALL(pDevice->CreateTexture(desc->Width, desc->Height, desc->MipLevels, d3dUsage, format, d3dPool, &pText, NULL));

		_pTexture = pText;

		_width = desc->Width;
		_height = desc->Height;
		_desc = desc;	
		TextureDescription = desc;
		_type = ResourceType::Texture2D;

		GC::AddMemoryPressure(sizeof(IDirect3DTexture9));

		_d3dUsage = d3dUsage;
		_d3dPool = d3dPool;
		_format = format;

		if(_d3dPool == D3DPOOL_DEFAULT)
			resseter = gcnew ResetTarget(this);

		else if(_d3dPool == D3DPOOL_MANAGED || _d3dPool == D3DPOOL_SYSTEMMEM)
		{
			D3DLOCKED_RECT locRec;
			SAFECALL(_pTexture->LockRect(0, &locRec, NULL, 0));
			SAFECALL(_pTexture->UnlockRect(0));

			int size = 0;
			for (int i = 0; i < _pTexture->GetLevelCount(); i++)
			{
				size += (locRec.Pitch >> i) * (_height >> i);
			}

			GC::AddMemoryPressure(size);
		}
	}

	D3D9Texture2D::D3D9Texture2D(IDirect3DTexture9* tex)
	{
		_pTexture  = tex;

		D3DSURFACE_DESC desc;
		tex->GetLevelDesc(0, &desc);
				
		_desc->Format = GetFORMAT(desc.Format);
		_desc->ArraySize = 0;
		_desc->BindFlags = BindFlags::ShaderResource;
		_desc->CPUAccessFlags = CpuAccessFlags::Read | CpuAccessFlags::Write;
		_desc->Usage = ResourceUsage::Default;
		_desc->MipLevels = tex->GetLOD();
		_desc->Width  = desc.Width;
		_desc->Height = desc.Height;
		_desc->Options = ResourceOptionFlags::None;
		TextureDescription = _desc;
		_d3dUsage = desc.Usage;
		_d3dPool = desc.Pool;
		_format = desc.Format;
		_type = ResourceType::Texture2D;
		_width = desc.Width;
		_height = desc.Height;

		GC::AddMemoryPressure(sizeof(IDirect3DTexture9));

		if(_d3dPool == D3DPOOL_DEFAULT)
			resseter = gcnew ResetTarget(this);

		else if(_d3dPool == D3DPOOL_MANAGED || _d3dPool == D3DPOOL_SYSTEMMEM)
		{
			D3DLOCKED_RECT locRec;
			SAFECALL(_pTexture->LockRect(0, &locRec, NULL, 0));
			SAFECALL(_pTexture->UnlockRect(0));

			int size = 0;
			for (int i = 0; i < _pTexture->GetLevelCount(); i++)
			{
				size += (locRec.Pitch >> i) * (_height >> i);
			}

			GC::AddMemoryPressure(size);
		}
	}

	MappedTexture2D D3D9Texture2D::Map(int subResource, MapType map, bool doNotWait)
	{
		D3DLOCKED_RECT locRec;
		SAFECALL(_pTexture->LockRect(subResource, &locRec, NULL, GetD3DLOCK(map, doNotWait)));

		MappedTexture2D rect;

		rect.DataPointer = static_cast<IntPtr>(locRec.pBits);
		rect.RowPitch = locRec.Pitch;

		return  rect;
	}

	void D3D9Texture2D::UnMap(int subResource)
	{
		SAFECALL(_pTexture->UnlockRect(subResource));			
	}

	void D3D9Texture2D::OnDispose(bool disposing)
	{
		if(_pTexture)
			_pTexture->Release();	

		if(resseter!=nullptr)
			resseter->TargetDisposed();
	}

	SubResource^ D3D9Texture2D::GetSubResource(int level)
	{		
		return gcnew D3DSubResource2D(level, this);
	}

	void D3D9Texture2D::DeviceReset(IDirect3DDevice9 * device)
	{
		if(_d3dPool == D3DPOOL_DEFAULT)
		{
			IDirect3DTexture9 * pText;
			SAFECALL(device->CreateTexture(_desc->Width, _desc->Height, _desc->MipLevels, _d3dUsage, _format, _d3dPool, &pText, NULL));
			_pTexture = pText;

			TextureReset(this);
		}
	}

	void D3D9Texture2D::DeviceLost(IDirect3DDevice9 * device)
	{
		if(_d3dPool == D3DPOOL_DEFAULT)
		{
			TextureLost(this);

			if(_pTexture)
				_pTexture->Release();		
		}
	}


	D3D9Texture3D::D3D9Texture3D(IDirect3DDevice9* pDevice, Texture3DDesc^ desc): _pTexture(NULL)
	{
		DWORD  d3dUsage;
		D3DPOOL  d3dPool;
		D3DFORMAT format = GetD3DFORMAT(desc->Format);

		GetTextureUsage(desc->Usage, desc->BindFlags, desc->CPUAccessFlags, &d3dUsage, &d3dPool);

		IDirect3DVolumeTexture9 * pText;
		SAFECALL(pDevice->CreateVolumeTexture(desc->Width, desc->Height, desc->Depth ,desc->MipLevels, d3dUsage, format, d3dPool, &pText, NULL));

		_pTexture = pText;

		_width = desc->Width;
		_height = desc->Height;
		_depth = desc->Depth;
		_desc = desc;
		_type = ResourceType::Texture3D;
		TextureDescription = _desc;

		GC::AddMemoryPressure(sizeof(IDirect3DVolumeTexture9));

		_d3dUsage = d3dUsage;
		_d3dPool = d3dPool;
		_format = format;

		if(_d3dPool == D3DPOOL_DEFAULT)
			resseter = gcnew ResetTarget(this);
		else if(_d3dPool == D3DPOOL_MANAGED || _d3dPool == D3DPOOL_SYSTEMMEM)
		{
			int size = 0;
			MappedTexture3D map = Map(0, MapType::Read, false);
			UnMap(0);
			for (int i = 0; i < _pTexture->GetLevelCount(); i++)
			{
				size+=( map.DepthPitch >> i) * (_depth >> i);
			}

			GC::AddMemoryPressure(size);
		}
	}

	D3D9Texture3D::D3D9Texture3D(IDirect3DVolumeTexture9* tex)
	{
		_pTexture  = tex;

		D3DVOLUME_DESC desc;
		tex->GetLevelDesc(0, &desc);
				
		_desc->Format = GetFORMAT(desc.Format);		
		_desc->BindFlags = BindFlags::ShaderResource;
		_desc->CPUAccessFlags = CpuAccessFlags::Read | CpuAccessFlags::Write;
		_desc->Usage = ResourceUsage::Default;
		_desc->MipLevels = tex->GetLOD();
		_desc->Width  = desc.Width;
		_desc->Height = desc.Height;
		_desc->Depth = desc.Depth;
		_desc->Options = ResourceOptionFlags::None;
		TextureDescription = _desc;

		GC::AddMemoryPressure(sizeof(IDirect3DVolumeTexture9));

		_d3dUsage = desc.Usage;
		_d3dPool = desc.Pool;
		_format = desc.Format;
		_type = ResourceType::Texture3D;
		_width = desc.Width;
		_height = desc.Height;

		if(_d3dPool == D3DPOOL_DEFAULT)
			resseter = gcnew ResetTarget(this);
		else if(_d3dPool == D3DPOOL_MANAGED || _d3dPool == D3DPOOL_SYSTEMMEM)
		{
			int size = 0;
			MappedTexture3D map = Map(0, MapType::Read, false);
			UnMap(0);
			for (int i = 0; i < _pTexture->GetLevelCount(); i++)
			{
				size+=( map.DepthPitch >> i) * (_depth >> i);
			}

			GC::AddMemoryPressure(size);
		}
	}

	MappedTexture3D D3D9Texture3D::Map(int subResource, MapType map, bool doNotWait)
	{
		D3DLOCKED_BOX locRec;	
		SAFECALL(_pTexture->LockBox(subResource, &locRec, NULL, GetD3DLOCK(map, doNotWait)));

		MappedTexture3D rect;

		rect.DataPointer = static_cast<IntPtr>(locRec.pBits);
		rect.RowPitch = locRec.RowPitch;
		rect.DepthPitch = locRec.SlicePitch;

		return  rect;
	}

	void D3D9Texture3D::UnMap(int subResource)
	{
		SAFECALL(_pTexture->UnlockBox(subResource));
	}

	void D3D9Texture3D::OnDispose(bool disposing)
	{
		if(_pTexture)
			_pTexture->Release();	
		if(resseter!=nullptr)
			resseter->TargetDisposed();
	}

	SubResource^ D3D9Texture3D::GetSubResource(int level)
	{
		/*IDirect3DSurface9* pSurf;
		LPDIRECT3DVOLUME9 pVol;
		_pTexture->GetVolumeLevel(level, &pVol);
		
		return gcnew D3DSubResource(pSurf, level, this);*/
		throw gcnew NotImplementedException();
	}

	void D3D9Texture3D::DeviceReset(IDirect3DDevice9 * device)
	{
		if(_d3dPool == D3DPOOL_DEFAULT)
		{
			IDirect3DVolumeTexture9 * pText;
			device->CreateVolumeTexture(_desc->Width, _desc->Height, _desc->Depth ,_desc->MipLevels, _d3dUsage, _format, _d3dPool, &pText, NULL);

			_pTexture = pText;

				TextureReset(this);
		}
	}

	void D3D9Texture3D::DeviceLost(IDirect3DDevice9 * device)
	{
		if(_d3dPool == D3DPOOL_DEFAULT)
		{
			TextureLost(this);

			if(_pTexture)
				_pTexture->Release();		
		}
	}


	D3D9TextureCube::D3D9TextureCube(IDirect3DDevice9* pDevice, TextureCubeDesc^ desc): _pTexture(NULL)
	{
		DWORD  d3dUsage;
		D3DPOOL  d3dPool;
		D3DFORMAT format = GetD3DFORMAT(desc->Format);

		GetTextureUsage(desc->Usage, desc->BindFlags, desc->CPUAccessFlags, &d3dUsage, &d3dPool);

		IDirect3DCubeTexture9 * pText;
		SAFECALL(pDevice->CreateCubeTexture(desc->EdgeSize, desc->MipLevels, d3dUsage, format, d3dPool, &pText, NULL));

		_pTexture = pText;

		_edgeSize = desc->EdgeSize;		
		_desc = desc;	
		_type = ResourceType::TextureCube;
		TextureDescription = desc;
		GC::AddMemoryPressure(sizeof(IDirect3DCubeTexture9));

		_d3dUsage = d3dUsage;
		_d3dPool = d3dPool;
		_format = format;

		if(_d3dPool == D3DPOOL_DEFAULT)
			resseter = gcnew ResetTarget(this);

		else if(_d3dPool == D3DPOOL_MANAGED || _d3dPool == D3DPOOL_SYSTEMMEM)
		{
			D3DLOCKED_RECT locRec;
				int size = 0;
			for (int i = 0; i < 6; i++)
			{
				SAFECALL(_pTexture->LockRect(static_cast<D3DCUBEMAP_FACES>( i) , 0, &locRec, NULL, 0));			
				_pTexture->UnlockRect(static_cast<D3DCUBEMAP_FACES>( i), 0);
				for (int i = 0; i < _pTexture->GetLevelCount(); i++)
				{
					size += (locRec.Pitch >> i) * (_edgeSize >> i);
				}				
			}			

			GC::AddMemoryPressure(size);
		}
	}

	D3D9TextureCube::D3D9TextureCube(IDirect3DCubeTexture9* tex)
	{
		_pTexture  = tex;

		D3DSURFACE_DESC desc;
		tex->GetLevelDesc(0, &desc);
				
		_desc->Format = GetFORMAT(desc.Format);		
		_desc->BindFlags = BindFlags::ShaderResource;
		_desc->CPUAccessFlags = CpuAccessFlags::Read | CpuAccessFlags::Write;
		_desc->Usage = ResourceUsage::Default;
		_desc->MipLevels = tex->GetLOD();
		_desc->EdgeSize  = desc.Width;		
		_desc->Options = ResourceOptionFlags::None;
		TextureDescription = _desc;
		GC::AddMemoryPressure(sizeof(IDirect3DCubeTexture9));

		_d3dUsage = desc.Usage;
		_d3dPool = desc.Pool;
		_format = desc.Format;
		_type = ResourceType::TextureCube;
		_edgeSize = desc.Width;

		if(_d3dPool == D3DPOOL_DEFAULT)
			resseter = gcnew ResetTarget(this);

		else if(_d3dPool == D3DPOOL_MANAGED || _d3dPool == D3DPOOL_SYSTEMMEM)
		{
			D3DLOCKED_RECT locRec;
				int size = 0;
			for (int i = 0; i < 6; i++)
			{
				SAFECALL(_pTexture->LockRect(static_cast<D3DCUBEMAP_FACES>( i) , 0, &locRec, NULL, 0));			
				_pTexture->UnlockRect(static_cast<D3DCUBEMAP_FACES>( i), 0);
				for (int i = 0; i < _pTexture->GetLevelCount(); i++)
				{
					size += (locRec.Pitch >> i) * (_edgeSize >> i);
				}				
			}			

			GC::AddMemoryPressure(size);
		}
	}

	MappedTexture2D D3D9TextureCube::Map(int face, int subResource, MapType map, bool doNotWait)
	{
		D3DLOCKED_RECT locRec;			
		SAFECALL(_pTexture->LockRect(static_cast<D3DCUBEMAP_FACES>(face), subResource, &locRec, NULL, GetD3DLOCK(map, doNotWait)));
		
		MappedTexture2D rect;

		rect.DataPointer = static_cast<IntPtr>(locRec.pBits);
		rect.RowPitch = locRec.Pitch;
		return  rect;
	}

	void D3D9TextureCube::UnMap(int face ,int subResource)
	{
		SAFECALL(_pTexture->UnlockRect( static_cast<D3DCUBEMAP_FACES>(face) , subResource));
	}

	void D3D9TextureCube::OnDispose(bool disposing)
	{
		if(_pTexture)
			_pTexture->Release();		

		if(resseter!=nullptr)
			resseter->TargetDisposed();
	}

	SubResource^ D3D9TextureCube::GetSubResource(int face, int level)
	{		
		return gcnew D3DSubResourceCube(static_cast<D3DCUBEMAP_FACES>(face), level, this);
	}	

	void D3D9TextureCube::DeviceReset(IDirect3DDevice9 * device)
	{
		if(_d3dPool == D3DPOOL_DEFAULT)
		{
			IDirect3DCubeTexture9 * pText;
			SAFECALL(device->CreateCubeTexture(_desc->EdgeSize, _desc->MipLevels, _d3dUsage, _format, _d3dPool, &pText, NULL));

			_pTexture = pText;

			TextureReset(this);
		}
	}

	void D3D9TextureCube::DeviceLost(IDirect3DDevice9 * device)
	{
		if(_d3dPool == D3DPOOL_DEFAULT)
		{
			TextureLost(this);

			if(_pTexture)
				_pTexture->Release();		
		}
	}

}}
