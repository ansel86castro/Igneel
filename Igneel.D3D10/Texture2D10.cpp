#include "Stdafx.h"
#include "Texture2D10.h"

namespace IgneelD3D10
{
	Texture2D10::Texture2D10(ID3D10Device * device, ID3D10Texture2D* texture ,String^ filename)
	{
		_location = filename;
		_texture = texture;		

		D3D10_TEXTURE2D_DESC td;
		texture->GetDesc(&td);

		_shaderResource = nullptr;
		if(td.BindFlags & D3D10_BIND_SHADER_RESOURCE)
		{
			ID3D10ShaderResourceView * rv;
			SAFECALL( device->CreateShaderResourceView(texture, NULL, &rv) );
			_shaderResource = rv;		
		}

		Texture2DDesc desc;
		desc.ArraySize = td.ArraySize;
		desc.BindFlags = static_cast<BindFlags>(td.BindFlags);		
		desc.Format = static_cast<Graphics::Format>( td.Format);
		desc.MipLevels = td.MipLevels;
		desc.Usage = static_cast<ResourceUsage>( td.Usage );
		desc.Width = td.Width;
		desc.Height = td.Height;
		desc.Options = static_cast<ResourceOptionFlags>(td.MiscFlags);				
		desc.SamplerDesc.Count = td.SampleDesc.Count;
		desc.SamplerDesc.Quality = td.SampleDesc.Quality;				
		desc.CPUAccessFlags = static_cast<CpuAccessFlags>( td.CPUAccessFlags );

		Description = desc;

		AddSize(device, td);
	}

	Texture2D10::Texture2D10(ID3D10Device * device, Texture2DDesc desc, array<MappedTexture2D>^ data)
		:Texture2DBase(desc)
	{
		if(desc.SamplerDesc.Count == 0)
			desc.SamplerDesc.Count = 1;

		ID3D10Texture2D* texture;
		D3D10_TEXTURE2D_DESC td;
		ZeroMemory(&td, sizeof(D3D10_TEXTURE2D_DESC));

		td.ArraySize = desc.ArraySize;
		td.BindFlags = (D3D10_BIND_FLAG)desc.BindFlags;		
		td.Format = (DXGI_FORMAT)desc.Format;
		td.MipLevels = desc.MipLevels;
		td.MiscFlags = (UINT)desc.Options;
		td.Usage = (D3D10_USAGE)desc.Usage;
		td.Width = desc.Width;
		td.Height = desc.Height;
		td.SampleDesc.Count = desc.SamplerDesc.Count;
		td.SampleDesc.Quality = desc.SamplerDesc.Quality;
		if(td.Usage == D3D10_USAGE_DEFAULT)
			td.CPUAccessFlags = 0;
		else
			td.CPUAccessFlags =(D3D10_CPU_ACCESS_FLAG)desc.CPUAccessFlags;

		if(data!=nullptr)
		{
			if(desc.ArraySize != data->Length)
				throw gcnew InvalidOperationException("data lenght mismatch with texture array lenght");

			D3D10_SUBRESOURCE_DATA* ini = new D3D10_SUBRESOURCE_DATA[data->Length];
			for (int i = 0; i < data->Length; i++)
			{
				ini[i].pSysMem = static_cast<void*>( data[i].DataPointer );
				ini[i].SysMemPitch = data[i].RowPitch;
				ini[i].SysMemSlicePitch = 0;
			}			
			SAFECALL(device->CreateTexture2D(&td, ini, &texture));
			delete[] ini;
		}
		else
		{
			SAFECALL(device->CreateTexture2D(&td, NULL, &texture));
		}

		_texture = texture;
		_shaderResource = nullptr;
		if(td.BindFlags & D3D10_BIND_SHADER_RESOURCE)
		{
			ID3D10ShaderResourceView * rv;		
			SAFECALL(device->CreateShaderResourceView(texture, NULL, &rv));
			_shaderResource = rv;		
		}
		AddSize(device, td);
	}

	void Texture2D10::AddSize(ID3D10Device * device, D3D10_TEXTURE2D_DESC& td)
	{		
		_device = device;
		D3D10_TEXTURE2D_DESC stagingDesc = td;
		stagingDesc.Usage = D3D10_USAGE_STAGING;
		stagingDesc.CPUAccessFlags = D3D10_CPU_ACCESS_READ | D3D10_CPU_ACCESS_WRITE;
		stagingDesc.BindFlags = 0;
		ID3D10Texture2D *temp;		
		SAFECALL( device->CreateTexture2D(&stagingDesc, NULL, &temp) );		

		D3D10_MAPPED_TEXTURE2D locRec;
		SAFECALL(temp->Map(0, D3D10_MAP_READ, 0, & locRec)); 
		temp->Unmap(0);		
		temp->Release();		
		temp = NULL;

		int size = 0;
		for (int i = 0; i < td.MipLevels; i++)
		{
			size += (locRec.RowPitch >> i) * (td.Height >> i);
		}

		GC::AddMemoryPressure(size);
	}

	void Texture2D10::CreateStagingResource()
	{
		if(_desc.Usage == ResourceUsage::Default && _desc.CPUAccessFlags != CpuAccessFlags::None && _stagingTexture == NULL)
		{
			D3D10_TEXTURE2D_DESC td;
			_texture->GetDesc(&td);
			D3D10_TEXTURE2D_DESC stagingDesc = td;
			stagingDesc.Usage = D3D10_USAGE_STAGING;
			stagingDesc.CPUAccessFlags = D3D10_CPU_ACCESS_READ | D3D10_CPU_ACCESS_WRITE;
			stagingDesc.BindFlags = 0;
			ID3D10Texture2D *temp;							 			
			SAFECALL( _device->CreateTexture2D(&stagingDesc, NULL, &temp) );
			_stagingTexture = temp;						
		}
	}

	void Texture2D10::OnDispose(bool dispose)
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

	MappedTexture2D Texture2D10::Map(int subResource, MapType map, bool doNotWait)
	{
		D3D10_MAPPED_TEXTURE2D data;
		auto dxmap = static_cast<D3D10_MAP>(map);
		lastMap = dxmap;
		CreateStagingResource();
		if(_stagingTexture)
		{
			if((dxmap & D3D10_MAP_READ) || (dxmap & D3D10_MAP_WRITE))
			{
				_device->CopySubresourceRegion(_stagingTexture,subResource, 0,0,0, _texture, subResource, NULL);
			}
			SAFECALL(_stagingTexture->Map(subResource, dxmap , doNotWait?D3D10_MAP_FLAG_DO_NOT_WAIT:0, &data));
		}
		else
		{
			SAFECALL(_texture->Map(subResource, dxmap , doNotWait?D3D10_MAP_FLAG_DO_NOT_WAIT:0, &data));
		}
		MappedTexture2D result = MappedTexture2D();
		result.DataPointer = static_cast<IntPtr>(data.pData);
		result.RowPitch = data.RowPitch;
		return result;
	}

	void Texture2D10::UnMap(int subResource)
	{
		if(_stagingTexture)
		{
			_stagingTexture->Unmap(subResource);
			if( (lastMap & D3D10_MAP_WRITE) || 
				(lastMap & D3D10_MAP_WRITE_DISCARD) || 
				((lastMap & D3D10_MAP_WRITE_NO_OVERWRITE) == D3D10_MAP_WRITE_NO_OVERWRITE))
				_device->CopySubresourceRegion(_texture, subResource, 0,0,0, _stagingTexture, subResource, NULL);

			_stagingTexture->Release();
			_stagingTexture=NULL;
		}
		else
			_texture->Unmap(subResource);
	}
	
}