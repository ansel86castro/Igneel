#include "Stdafx.h"
#include "GraphicBuffer10.h"
#include "Utils.h"

using namespace System::Runtime::InteropServices;

namespace IgneelD3D10
{
	GraphicBuffer10::GraphicBuffer10(ID3D10Device* device, ID3D10Buffer * buffer , int size, int stride,  ResourceUsage usage, CpuAccessFlags cpuAcces, D3D10_SUBRESOURCE_DATA* sbData, ResBinding binding)
	{
		_srv = NULL;
		_stagingBuffer = NULL;		
		this->device = device;
		this->_buffer = buffer;
		_lenght = size;
		_usage = usage;
		_cpuAccesType = cpuAcces;		
		_type = ResourceType::Buffer;	
		_binding = binding;		
		this->_stride = stride;
		if(stride == 2)
			this->_format = DXGI_FORMAT_R16_UINT;
		else if(stride == 4)
			this->_format = DXGI_FORMAT_R32_UINT;
				
		if(FLAG_SET(binding, ResBinding::ShaderResource))
		{
			D3D10_SHADER_RESOURCE_VIEW_DESC viewDesc;
			ZeroMemory(&viewDesc,  sizeof(D3D10_SHADER_RESOURCE_VIEW_DESC));
			viewDesc.ViewDimension = D3D10_SRV_DIMENSION_BUFFER;
			viewDesc.Buffer.FirstElement = 0;
			viewDesc.Buffer.ElementWidth = stride;
			
			ID3D10ShaderResourceView * pview;
			SAFECALL( device->CreateShaderResourceView(_buffer, &viewDesc , &pview) );
			_srv = pview;
		}

		GC::AddMemoryPressure(size);		
	}

	void GraphicBuffer10::CreateStaginResource()
	{
		if(_usage == ResourceUsage::Default && _cpuAccesType != CpuAccessFlags::None && _stagingBuffer == NULL)
		{
			D3D10_BUFFER_DESC stagingDesc;
			_buffer->GetDesc(&stagingDesc);
			stagingDesc.Usage = D3D10_USAGE_STAGING;
			stagingDesc.BindFlags = 0;
			stagingDesc.CPUAccessFlags = D3D10_CPU_ACCESS_READ | D3D10_CPU_ACCESS_WRITE;

			ID3D10Buffer* staginbBuff;
			SAFECALL( device->CreateBuffer(&stagingDesc, NULL, &staginbBuff) );
			_stagingBuffer = staginbBuff;			
		}	
	}

	/*GraphicBuffer10::GraphicBuffer10(ID3D10Device* device, BufferDesc desc)
	{		
		D3D10_BUFFER_DESC bd;
		ZeroMemory(&bd, sizeof(D3D10_BUFFER_DESC));

		if((desc.Binding & BufferBinding::VertexBuffer) == BufferBinding::VertexBuffer)
			bd.BindFlags |= D3D10_BIND_VERTEX_BUFFER;

		if((desc.Binding & BufferBinding::ShaderResource) == BufferBinding::ShaderResource)
			bd.BindFlags |= D3D10_BIND_SHADER_RESOURCE;

		if((desc.Binding & BufferBinding::StreamOutput) == BufferBinding::StreamOutput)
			bd.BindFlags |= D3D10_BIND_STREAM_OUTPUT;

		bd.ByteWidth = desc.SizeInBytes;		
		bd.MiscFlags = 0;
		bd.Usage = (D3D10_USAGE)desc.Usage;		
		if(bd.Usage == D3D10_USAGE_DEFAULT)
			bd.CPUAccessFlags = 0;
		else
			bd.CPUAccessFlags =	 (UINT)desc.Access;

		ID3D10Buffer * buffer = NULL;

		D3D10_SUBRESOURCE_DATA sbData;
		ZeroMemory(&sbData, sizeof(D3D10_SUBRESOURCE_DATA));

		if(desc.Data != IntPtr::Zero)
		{						
			sbData.pSysMem = static_cast<void*>(desc.Data);
			sbData.SysMemPitch = 0;
			sbData.SysMemSlicePitch = 0;

			SAFECALL( device->CreateBuffer(&bd, &sbData, &buffer) );					
		}
		else
		{
			SAFECALL(device->CreateBuffer(&bd, NULL, &buffer) );			
		}

		_buffer = buffer;
		_lenght = desc.SizeInBytes;
		_usage =  desc.Usage;
		_cpuAccesType = desc.Access;							
		_stride = desc.Stride;
		binding = desc.Binding;

		if((desc.Binding & BufferBinding::ShaderResource) == BufferBinding::ShaderResource)
		{
			D3D10_SHADER_RESOURCE_VIEW_DESC viewDesc;
			ZeroMemory(&viewDesc,  sizeof(D3D10_SHADER_RESOURCE_VIEW_DESC));
			viewDesc.ViewDimension = D3D10_SRV_DIMENSION_BUFFER;
			viewDesc.Buffer.FirstElement = 0;
			viewDesc.Buffer.ElementWidth = desc.Stride;
			
			ID3D10ShaderResourceView * pview;
			SAFECALL( device->CreateShaderResourceView(_buffer, &viewDesc , &pview) );
			_srv = pview;
		}

		if(desc.Usage == ResourceUsage::Default && desc.Access != CpuAccessFlags::None)
		{
			D3D10_BUFFER_DESC stagingDesc;
			buffer->GetDesc(&stagingDesc);
			stagingDesc.Usage = D3D10_USAGE_STAGING;
			stagingDesc.BindFlags = 0;
			stagingDesc.CPUAccessFlags = D3D10_CPU_ACCESS_READ | D3D10_CPU_ACCESS_WRITE;

			ID3D10Buffer* staginbBuff;
			if (sbData.pSysMem)
			{
				SAFECALL( device->CreateBuffer(&stagingDesc, &sbData, &staginbBuff) );
			}
			else
			{
				SAFECALL( device->CreateBuffer(&stagingDesc, NULL, &staginbBuff) );
			}

			_stagingBuffer = staginbBuff;			
		}	
				

		GC::AddMemoryPressure(desc.SizeInBytes);		
	}*/

	IntPtr GraphicBuffer10::Map(MapType map, bool doNotWait)
	{
		void *pData = NULL;
		CreateStaginResource();
		if(_stagingBuffer)
		{			
			if((map & MapType::Read) == MapType::Read || (map & MapType::Write) == MapType::Write)
			{
				device->CopyResource(_stagingBuffer, _buffer);
			}
			else if(map == MapType::Write_Discard)
			{
				map = MapType::Write;
			}

			SAFECALL( _stagingBuffer->Map(static_cast<D3D10_MAP>( map ), doNotWait ? D3D10_MAP_FLAG_DO_NOT_WAIT : 0, &pData));
		}	
		else
		{
			SAFECALL( _buffer->Map(static_cast<D3D10_MAP>( map ), doNotWait ? D3D10_MAP_FLAG_DO_NOT_WAIT : 0, &pData) );
		}
		return IntPtr(pData);
	}

	void GraphicBuffer10::Unmap()
	{
		if(_stagingBuffer)
		{
			_stagingBuffer->Unmap();
			//void *pData;
			//_stagingBuffer->Map(D3D10_MAP::D3D10_MAP_READ, D3D10_MAP_FLAG_DO_NOT_WAIT , &pData);
			//device->UpdateSubresource(_buffer, 0, NULL, pData, 0, 0);
			device->CopyResource(_buffer,_stagingBuffer);
			_stagingBuffer->Release();
			_stagingBuffer = NULL;
		}
		else
		{
			_buffer->Unmap();
		}		
	}

	void  GraphicBuffer10::OnDispose(bool disposing)
	{
		__super::OnDispose(disposing);
		if(_buffer)
		{
			if(_srv)
			{
				_srv->Release();
				_srv = NULL;
			}
			
			_buffer->Release();
			_buffer = NULL;		

			if(_stagingBuffer)
			{
				_stagingBuffer->Release();
				_stagingBuffer = NULL;
			}			
		}		
	}	

}