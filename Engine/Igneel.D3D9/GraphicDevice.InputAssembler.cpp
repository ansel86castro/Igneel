#include "Stdafx.h"
#include "GraphicDevice.h"
#include "GraphicDeviceManager.h"
#include "InputAssembler.h"
#include "VertexBuffer.h"

namespace Igneel { namespace D3D9 {

	void D3D9GraphicDevice::InitializeInputAssemblerStage()
	{
		_iaVertexBufferBind = gcnew array<BufferBind>(Info->MaxStreams);
	}	

	void D3D9GraphicDevice::IASetInputLayout(InputLayout^ value)
	{
		D3DInputLayout^ d3dInput = static_cast<D3DInputLayout^>(value);
		if(d3dInput != nullptr)
		{
			SAFECALL( _pDevice->SetVertexDeclaration(d3dInput->_pVertexDecl) );
		}
		else
			SAFECALL( _pDevice->SetVertexDeclaration(NULL) );
	}
	
	void D3D9GraphicDevice::IASetPrimitiveType(IAPrimitive value)
	{
		
	}

	void D3D9GraphicDevice::IASetVertexBufferImpl(int slot, GraphicBuffer^ vertexBuffer, int offset, int stride)
	{
		D3DVertexBuffer^ vb = static_cast<D3DVertexBuffer^>(vertexBuffer);
		if(vb!=nullptr)
	 	{
			SAFECALL( _pDevice->SetStreamSource(slot, vb->_pVertexBufer, offset, stride) );
		}
		else
			SAFECALL( _pDevice->SetStreamSource(slot, NULL, 0, 0) );
	}

	void D3D9GraphicDevice::IASetIndexBufferImpl(GraphicBuffer^ indexBuffer, int offset)
	{
		D3DIndexBuffer^ ib = static_cast<D3DIndexBuffer^>(indexBuffer);
		if(ib!=nullptr)
		{
				SAFECALL( _pDevice->SetIndices(ib->_pIndexBufer) );
		}
		else
			SAFECALL( _pDevice->SetIndices(NULL) );
	}

	InputLayout^ D3D9GraphicDevice::CreateInputLayout(array<VertexElement>^ elements ,DataBuffer^ signature)
	{
		return gcnew D3DInputLayout(_pDevice, elements);
	}

	GraphicBuffer^ D3D9GraphicDevice::CreateVertexBuffer(int size, int stride,  ResourceUsage usage, MapType cpuAcces, Array^ data)
	{
		return gcnew D3DVertexBuffer(_pDevice, size, stride, usage, cpuAcces, data);
	}

	GraphicBuffer^ D3D9GraphicDevice::CreateIndexBuffer(int size,  IndexFormat format , ResourceUsage usage, MapType cpuAcces, Array^ data)
	{
		return gcnew D3DIndexBuffer(_pDevice, size, format, usage, cpuAcces, data);
	}

	/*GraphicBuffer^ D3D9GraphicDevice::CreateConstantBuffer(long size, Array^ data)
	{
		return nullptr;
	}*/

}}