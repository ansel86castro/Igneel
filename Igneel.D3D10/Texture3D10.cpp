#include "Stdafx.h"
#include "Texture3D10.h"


namespace IgneelD3D10
{
	Texture3D10::Texture3D10(ID3D10Device * device, ID3D10Texture3D* texture,String^ filename)
	{
		_location = filename;
		_texture = texture;
		
		D3D10_TEXTURE3D_DESC td;
		ZeroMemory(&td, sizeof(D3D10_TEXTURE3D_DESC));
		texture->GetDesc(&td);

		_shaderResource = nullptr;
		if(td.BindFlags & D3D10_BIND_SHADER_RESOURCE)
		{
			ID3D10ShaderResourceView * rv;
			SAFECALL( device->CreateShaderResourceView(texture, NULL, &rv) );
			_shaderResource = rv;		
		}		

		Texture3DDesc desc;		
		desc.BindFlags = static_cast<BindFlags>(td.BindFlags);
		desc.CPUAccessFlags = static_cast<CpuAccessFlags>( td.CPUAccessFlags );
		desc.Format = static_cast<Graphics::Format>( td.Format);
		desc.MipLevels = td.MipLevels;
		desc.Usage = static_cast<ResourceUsage>( td.Usage );
		desc.Width = td.Width;
		desc.Height = td.Height;
		desc.Depth = td.Depth;
		desc.Options = static_cast<ResourceOptionFlags>(td.MiscFlags);

		Description = desc;

		AddSize(device, td);
	}

	Texture3D10::Texture3D10(ID3D10Device * device, Texture3DDesc desc, array<MappedTexture3D>^ data)
		:Texture3DBase(desc)
	{
		ID3D10Texture3D* texture;
		D3D10_TEXTURE3D_DESC td;	
		ZeroMemory(&td, sizeof(D3D10_TEXTURE3D_DESC));
		td.BindFlags = (D3D10_BIND_FLAG)desc.BindFlags;		
		td.Format = (DXGI_FORMAT)desc.Format;
		td.MipLevels = desc.MipLevels;
		td.MiscFlags = (UINT)desc.Options;
		td.Usage = (D3D10_USAGE)desc.Usage;
		td.Width = desc.Width;
		td.Height = desc.Height;
		td.Depth = desc.Depth;
		if(td.Usage == D3D10_USAGE_DEFAULT)
			td.CPUAccessFlags = 0;
		else
			td.CPUAccessFlags =(D3D10_CPU_ACCESS_FLAG)desc.CPUAccessFlags;

		if(data != nullptr)
		{			
			D3D10_SUBRESOURCE_DATA* ini = new D3D10_SUBRESOURCE_DATA[data->Length];
			for (int i = 0; i < data->Length; i++)
			{
				ini[i].pSysMem = static_cast<void*>( data[i].DataPointer );
				ini[i].SysMemPitch = data[i].RowPitch;
				ini[i].SysMemSlicePitch = 0;
			}			
			SAFECALL(device->CreateTexture3D(&td, ini, &texture));
			delete[] ini;
		}
		else
		{
			SAFECALL(device->CreateTexture3D(&td, NULL, &texture));
		}

		_texture = texture;

		_shaderResource = nullptr;
		if(td.BindFlags & D3D10_BIND_SHADER_RESOURCE)
		{
			ID3D10ShaderResourceView * rv;
			SAFECALL( device->CreateShaderResourceView(texture, NULL, &rv) );
			_shaderResource = rv;		
		}			

		AddSize(device, td);
	}

	void Texture3D10::AddSize(ID3D10Device * device, D3D10_TEXTURE3D_DESC& td)
	{		
		_device = device;
		D3D10_TEXTURE3D_DESC stagingDesc = td;
		stagingDesc.Usage = D3D10_USAGE_STAGING;
		stagingDesc.CPUAccessFlags = D3D10_CPU_ACCESS_READ | D3D10_CPU_ACCESS_WRITE;
		stagingDesc.BindFlags = 0;			
		ID3D10Texture3D *temp;
		if(td.Usage == D3D10_USAGE_DEFAULT && _desc.CPUAccessFlags != CpuAccessFlags::None )
		{			 			
			SAFECALL( device->CreateTexture3D(&stagingDesc, NULL, &temp) );
			_stagingTexture = temp;						
		}
		if(_stagingTexture == nullptr)
		{			
			SAFECALL( device->CreateTexture3D(&stagingDesc, NULL, &temp) );
		}

		D3D10_MAPPED_TEXTURE3D locRec;
		temp->Map(0, D3D10_MAP_READ, 0, & locRec); 
		temp->Unmap(0);
		if(_stagingTexture == nullptr)
		{
			temp->Release();
		}

		int size = 0;
		for (int i = 0; i < td.MipLevels; i++)
		{
			size += (locRec.DepthPitch >> i) * (td.Depth >> i);
		}

		GC::AddMemoryPressure(size);
	}

	void Texture3D10::OnDispose(bool dispose)
	{
		if(_texture)
		{
			if(_shaderResource)	
				_shaderResource->Release();
			_texture->Release();
			_texture = NULL;
			_shaderResource = NULL;
			if(_stagingTexture)
			{
				_stagingTexture->Release();
				_stagingTexture = NULL;
			}
		}
	}

	MappedTexture3D Texture3D10::Map(int subResource, MapType map, bool doNotWait)
	{
		D3D10_MAPPED_TEXTURE3D data;
		auto dxmap = static_cast<D3D10_MAP>(map);
		lastMap = dxmap;
		if(_stagingTexture)
		{
			if((dxmap & D3D10_MAP_READ) || (dxmap & D3D10_MAP_WRITE))
			{
				_device->CopySubresourceRegion(_stagingTexture,subResource, 0,0,0, _texture, subResource, NULL);
			}
			_stagingTexture->Map(subResource, dxmap , doNotWait?D3D10_MAP_FLAG_DO_NOT_WAIT:0, &data);
		}
		else
		{
			_texture->Map(subResource, static_cast<D3D10_MAP>(map), doNotWait?D3D10_MAP_FLAG_DO_NOT_WAIT:0, &data);
		}		

		MappedTexture3D result;
		result.DataPointer = static_cast<IntPtr>(data.pData);
		result.RowPitch = data.RowPitch;
		result.DepthPitch = data.DepthPitch;
		return result;
	}

	void Texture3D10::UnMap(int subResource)
	{
		if(_stagingTexture)
		{
			_stagingTexture->Unmap(subResource);
			if( lastMap & D3D10_MAP_WRITE || 
				lastMap & D3D10_MAP_WRITE_DISCARD || 
				lastMap & D3D10_MAP_WRITE_NO_OVERWRITE)
				_device->CopySubresourceRegion(_texture, subResource, 0,0,0, _stagingTexture, subResource, NULL);
		}
		else
			_texture->Unmap(subResource);
	}	
}