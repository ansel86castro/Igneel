#include "Stdafx.h"
#include "ConstantBuffer.h"
#include "Shaders.h"
#include "GraphicDevice.h"

namespace IgneelD3D10{

	CbHandle::CbHandle(ID3D10Buffer* constantBuff)		
	{		
		cb = constantBuff;
		buffer = NULL;

		D3D10_BUFFER_DESC bd;
		cb->GetDesc(&bd);
		size = bd.ByteWidth;
	}

	CbHandle::!CbHandle()
	{
		if(cb)
		{			
			cb->Release();			
			ConstantBufferCache::Cache->Remove(size);			
			cb = NULL;			
			buffer = NULL;			
		}
	}

		void* CbHandle::GetBuffer(GraphicDevice10^ device)
		{
			if(!buffer)
			{
				void* data = NULL;
				SAFECALL( cb->Map(D3D10_MAP_WRITE_DISCARD, 0  , &data) );
				buffer = data;
				if(device!=nullptr)
				{					
					device->OpenConstantBuffers->Add(this);
				}
			}
			return buffer;
		}

		void CbHandle::Close()
		{
			if(buffer)
			{
				cb->Unmap();
				buffer  = NULL;
			}
		}

		CbHandle^ ConstantBufferCache::GetBuffer(ID3D10Device* device, int buffSize)
		{			
			WeakReference<CbHandle^>^ ref;
			CbHandle^ handle;
			if(!Cache->TryGetValue(buffSize, ref) || !ref->TryGetTarget(handle))
			{
				//Create the Constant Buffer
				D3D10_BUFFER_DESC cbDesc;
				ZeroMemory(&cbDesc, sizeof(D3D10_BUFFER_DESC));
				cbDesc.ByteWidth = buffSize;
				cbDesc.Usage = D3D10_USAGE_DYNAMIC;
				cbDesc.BindFlags = D3D10_BIND_CONSTANT_BUFFER;
				cbDesc.CPUAccessFlags = D3D10_CPU_ACCESS_WRITE;
				cbDesc.MiscFlags = 0;
				
				ID3D10Buffer* constantBuffer;
				SAFECALL( device->CreateBuffer(&cbDesc,NULL, &constantBuffer));

				handle = gcnew CbHandle(constantBuffer);
				Cache[buffSize] = gcnew WeakReference<CbHandle^>(handle);
				return handle;
			}
			return handle;
		}

		CBufferShaderBinding::CBufferShaderBinding(CbHandle^ handle, int bindPoint, int index)
		{
			this->handle = handle;
			this->bindPoint = bindPoint;
			this->index	 = index;
		}

		CBufferVarBinding::CBufferVarBinding(CbHandle^ handle, int offset, GraphicDevice10^ device)
		{
			this->handle = handle;
			this->offset = offset;
			this->device = device;
		}

		void* CBufferVarBinding::GetBuffer()
		{			
			return (byte*)((byte*)handle->GetBuffer(device) + offset);
		}
}