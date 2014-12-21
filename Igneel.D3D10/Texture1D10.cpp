#include "Stdafx.h"
#include "Texture1D10.h"

namespace IgneelD3D10
{
	Texture1D10::Texture1D10(ID3D10Device * device, ID3D10Texture1D* texture, String^ filename)				
	{
		this->_location = filename;
		this->device = device;
		_texture = texture;		

		D3D10_TEXTURE1D_DESC td;
		texture->GetDesc(&td);

		_shaderResource = nullptr;
		if(td.BindFlags & D3D10_BIND_SHADER_RESOURCE)
		{
			ID3D10ShaderResourceView * rv;		
			SAFECALL(device->CreateShaderResourceView(texture, NULL, &rv));
			_shaderResource = rv;		
		}

		Texture1DDesc desc;
		desc.ArraySize = td.ArraySize;
		desc.BindFlags = static_cast<BindFlags>(td.BindFlags);
		desc.CPUAccessFlags = static_cast<CpuAccessFlags>( td.CPUAccessFlags );
		desc.Format = static_cast<Graphics::Format>( td.Format);
		desc.MipLevels = td.MipLevels;
		desc.Usage = static_cast<ResourceUsage>( td.Usage );
		desc.Width = td.Width;
		desc.Options = static_cast<ResourceOptionFlags>(td.MiscFlags);		

		Description = desc;		
		_stagingTexture = NULL;
			
		int size = 0;
		for (int i = 0; i < td.MipLevels; i++)
		{
			size += td.Width >> i;
		}

		GC::AddMemoryPressure(size);
		
	}

	Texture1D10::Texture1D10(ID3D10Device * device, Texture1DDesc desc, array<IntPtr>^ data)
		:Texture1DBase(desc)
	{
		this->device = device;
		ID3D10Texture1D* texture;
		D3D10_TEXTURE1D_DESC td;
		ZeroMemory(&td, sizeof(D3D10_TEXTURE1D_DESC));
		td.ArraySize = desc.ArraySize;
		td.BindFlags = (D3D10_BIND_FLAG)desc.BindFlags;		
		td.Format = (DXGI_FORMAT)desc.Format;
		td.MipLevels = desc.MipLevels;
		td.MiscFlags = (UINT)desc.Options;
		td.Usage = (D3D10_USAGE)desc.Usage;
		td.Width = desc.Width;
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
				ini[i].pSysMem = static_cast<void*>( data[i] );
			}			
			SAFECALL(device->CreateTexture1D(&td, ini, &texture));
			delete[] ini;
		}
		else
		{
			SAFECALL(device->CreateTexture1D(&td, NULL, &texture));
		}

		_texture = texture;

		_shaderResource = nullptr;
		if(td.BindFlags & D3D10_BIND_SHADER_RESOURCE)
		{
			ID3D10ShaderResourceView * rv;		
			SAFECALL(device->CreateShaderResourceView(texture, NULL, &rv));
			_shaderResource = rv;		
		}
		
		int size = 0;
		for (int i = 0; i < td.MipLevels; i++)
		{
			size += td.Width >> i;
		}

		GC::AddMemoryPressure(size);
	}

	void Texture1D10::CreateStagingResource()
	{
		auto des = Description;
		if(des.Usage == ResourceUsage::Default && des.CPUAccessFlags != CpuAccessFlags::None && _stagingTexture==NULL)
		{
			D3D10_TEXTURE1D_DESC td;
			_texture->GetDesc(&td);
			D3D10_TEXTURE1D_DESC stagingDesc = td;		
			stagingDesc.Usage = D3D10_USAGE_STAGING;
			stagingDesc.CPUAccessFlags = D3D10_CPU_ACCESS_READ | D3D10_CPU_ACCESS_WRITE;
			stagingDesc.BindFlags = 0;
			ID3D10Texture1D* stTexture;
			SAFECALL( device->CreateTexture1D(&stagingDesc, NULL, &stTexture) );
			_stagingTexture = stTexture;			
		}
	}

	void Texture1D10::OnDispose(bool dispose)
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

	IntPtr Texture1D10::Map(int subResource, MapType map, bool doNotWait)
	{
		void* data;
		auto dxmap = static_cast<D3D10_MAP>(map);
		lastMap = dxmap;
		CreateStagingResource();

		if(_stagingTexture)
		{
			if(dxmap & D3D10_MAP_READ || dxmap & D3D10_MAP_WRITE)
			{
				device->CopySubresourceRegion(_stagingTexture,subResource, 0,0,0, _texture, subResource, NULL);
			}
			_stagingTexture->Map(subResource, dxmap , doNotWait?D3D10_MAP_FLAG_DO_NOT_WAIT:0, &data);
		}
		else
			_texture->Map(subResource, dxmap , doNotWait?D3D10_MAP_FLAG_DO_NOT_WAIT:0, &data);

		return IntPtr(data);
	}

	void Texture1D10::UnMap(int subResource)
	{
		if(_stagingTexture)
		{
			_stagingTexture->Unmap(subResource);
			if( lastMap & D3D10_MAP_WRITE || 
				lastMap & D3D10_MAP_WRITE_DISCARD || 
				lastMap & D3D10_MAP_WRITE_NO_OVERWRITE)
				device->CopySubresourceRegion(_texture, subResource, 0,0,0, _stagingTexture, subResource, NULL);

			_stagingTexture->Release();
			_stagingTexture = NULL;
		}
		else
		{
			_texture->Unmap(subResource);
		}
	}
	
}