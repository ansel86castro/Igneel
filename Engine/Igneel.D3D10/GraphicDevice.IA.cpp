#include "Stdafx.h"
#include "GraphicDevice.h"
#include "InputLayout.h"
#include "GraphicBuffer10.h"
#include "Utils.h"

namespace IgneelD3D10
{
	IAInitialization GraphicDevice10::GetIAInitialization()
	{
		IAInitialization ini = IAInitialization();
		ini.NbVertexBuffers = 16;
		return ini;
	}

	void GraphicDevice10::IASetInputLayout(InputLayout^ value)
	{
		InputLayout10 ^ d3dInput = static_cast<InputLayout10^>(value);
		if(d3dInput != nullptr)
		{
			_device->IASetInputLayout(d3dInput->_input);
		}
		else
			_device->IASetInputLayout(NULL);
	}

	void GraphicDevice10::IASetPrimitiveType(IAPrimitive value)
	{
		switch (value)
		{
		case Igneel::Graphics::IAPrimitive::PointList:
			_primitiveVertex = 1;
			break;
		case Igneel::Graphics::IAPrimitive::LineList:
			_primitiveVertex = 2;
			break;
		case Igneel::Graphics::IAPrimitive::LineStrip:
			_primitiveVertex = 2;
			break;
		case Igneel::Graphics::IAPrimitive::TriangleList:
			break;
		case Igneel::Graphics::IAPrimitive::TriangleStrip:
			break;
		case Igneel::Graphics::IAPrimitive::TriangleFan:
			break;
		default:
			break;
		}
		_device->IASetPrimitiveTopology((D3D10_PRIMITIVE_TOPOLOGY)value);
	}

	void GraphicDevice10::IASetVertexBufferImpl(int slot, GraphicBuffer^ vertexBuffer, int offset, int stride)
	{
		GraphicBuffer10^ buffer = static_cast<GraphicBuffer10^>(vertexBuffer);
		ID3D10Buffer* buffers[1] = { buffer->_buffer};		
		_device->IASetVertexBuffers(slot,1,buffers, (UINT*)&stride, (UINT*)&offset);
	}

	void GraphicDevice10::IASetIndexBufferImpl(GraphicBuffer^ indexBuffer, int offset)
	{
		GraphicBuffer10^ buffer = static_cast<GraphicBuffer10^>(indexBuffer);
		_device->IASetIndexBuffer(buffer->_buffer, buffer->_format, offset);
	}

	InputLayout^ GraphicDevice10::CreateInputLayout(array<VertexElement>^ elements ,ShaderCode^ signature)
	{
		D3D10_INPUT_ELEMENT_DESC* e = new D3D10_INPUT_ELEMENT_DESC [elements->Length];		
		ZeroMemory(e, sizeof(D3D10_INPUT_ELEMENT_DESC) * elements->Length);
	
		for (int i = 0; i < elements->Length; i++)
		{
			e[i].InputSlot = elements[i].Stream;			
			e[i].Format = getFormat( elements[i].Format );
			e[i].InputSlotClass = D3D10_INPUT_PER_VERTEX_DATA ;
			e[i].AlignedByteOffset = elements[i].Offset;
			e[i].SemanticName = getSemanticName(elements[i].Semantic);
			e[i].SemanticIndex = elements[i].UsageIndex;
		}
		ID3D10InputLayout * input;
		auto code = signature->Code;
		pin_ptr<byte> pterCode  = &code[0]; 
		HRESULT hr =  _device->CreateInputLayout(e,elements->Length,pterCode, code->Length ,&input);

		/*for (int i = 0; i < elements->Length; i++)
		{
			delete e[i].SemanticName;			
		}*/
		delete[] e;

		if(FAILED(hr))
		{
			throw gcnew InvalidOperationException();
		}
		return gcnew InputLayout10(input);
	}

	GraphicBuffer^ GraphicDevice10::CreateVertexBuffer(int size, int stride,  ResourceUsage usage, CpuAccessFlags cpuAcces, ResourceBinding binding, IntPtr data)
	{
		D3D10_BUFFER_DESC bd;
		ZeroMemory(&bd, sizeof(D3D10_BUFFER_DESC));

		bd.ByteWidth = size;		
		bd.MiscFlags = 0;
		bd.Usage = (D3D10_USAGE)usage;		
		if(bd.Usage == D3D10_USAGE_DEFAULT)
			bd.CPUAccessFlags = 0;
		else
			bd.CPUAccessFlags =	 (UINT)cpuAcces;

		if(FLAG_SET(binding,ResourceBinding::VertexBuffer))
			bd.BindFlags |= D3D10_BIND_VERTEX_BUFFER;
		if(FLAG_SET(binding,ResourceBinding::ShaderResource))
			bd.BindFlags |= D3D10_BIND_SHADER_RESOURCE;
		if(FLAG_SET(binding,ResourceBinding::StreamOutput))
			bd.BindFlags |= D3D10_BIND_STREAM_OUTPUT;

		if(!bd.BindFlags)
			throw gcnew ArgumentException(L"binding");

		ID3D10Buffer * buffer;
		if(data != IntPtr::Zero)
		{
			D3D10_SUBRESOURCE_DATA sbData;
			
			sbData.pSysMem =static_cast<void*>(data);
			sbData.SysMemPitch = 0;
			sbData.SysMemSlicePitch = 0;

			HRESULT hr = _device->CreateBuffer(&bd, &sbData, &buffer);		
			if(FAILED(hr))
				throw gcnew InvalidOperationException();

			return gcnew GraphicBuffer10(_device, buffer, size, stride, usage, cpuAcces ,&sbData, binding);
		}
		else
		{
			SAFECALL(_device->CreateBuffer(&bd, NULL, &buffer) );
			return gcnew GraphicBuffer10(_device, buffer, size, stride, usage, cpuAcces, NULL, binding);
		}
		

	}

	GraphicBuffer^ GraphicDevice10::CreateIndexBuffer(int size,  IndexFormat format , ResourceUsage usage, CpuAccessFlags cpuAcces, IntPtr data)
	{
		D3D10_BUFFER_DESC bd;
		ZeroMemory(&bd, sizeof(D3D10_BUFFER_DESC));

		bd.BindFlags = D3D10_BIND_INDEX_BUFFER;
		bd.ByteWidth = size;		
		bd.MiscFlags = 0;
		bd.Usage = (D3D10_USAGE)usage;		
		if(bd.Usage == D3D10_USAGE_DEFAULT)
			bd.CPUAccessFlags = 0;
		else
			bd.CPUAccessFlags =	 (UINT)cpuAcces;

		ID3D10Buffer * buffer;
		if(data != IntPtr::Zero)
		{
			D3D10_SUBRESOURCE_DATA sbData;					
			sbData.pSysMem =static_cast<void*>(data);
			sbData.SysMemPitch = 0;
			sbData.SysMemSlicePitch = 0;

			HRESULT hr = _device->CreateBuffer(&bd, &sbData, &buffer);		
			if(FAILED(hr))
				throw gcnew InvalidOperationException();
			return gcnew GraphicBuffer10(_device, buffer, size,  format==IndexFormat::Index16?2:4, usage, cpuAcces ,&sbData, ResourceBinding::IndexBuffer);
		}
		else
		{
			SAFECALL(_device->CreateBuffer(&bd, NULL, &buffer) );
			return gcnew GraphicBuffer10(_device,buffer, size, format==IndexFormat::Index16?2:4, usage, cpuAcces, NULL,ResourceBinding::IndexBuffer);
		}		
	}

	void GraphicDevice10::UpdateBuffer(GraphicBuffer^ buffer, int offset ,IntPtr pterData, int dataSize)
	{
		GraphicBuffer10^ buff = static_cast<GraphicBuffer10^>(buffer);
		if(buff->Usage != ResourceUsage::Default)
			throw gcnew InvalidOperationException(L"Only buffers with Default Usage cant be updated, to update a either Dynamic or Staging buffer you must call Map");

		D3D10_BOX box;
		ZeroMemory(&box, sizeof(D3D10_BOX));
		box.left = offset;
		box.right = offset + dataSize;
		_device->UpdateSubresource(buff->_buffer,0, &box, static_cast<void*>(pterData), 0, 0);
	}
}