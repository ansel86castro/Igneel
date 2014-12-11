#include "Stdafx.h"
#include "VertexBuffer.h"

namespace Igneel { namespace D3D9 {

	D3DVertexBuffer::D3DVertexBuffer(IDirect3DDevice9* device , int size, int stride,  ResourceUsage usage, MapType cpuAcces, Array^ data)
	{
		IDirect3DVertexBuffer9 * pvb;

		DWORD d3dusage = 0;
		D3DPOOL pool;
		
		GetBufferUsage(cpuAcces, & d3dusage, & pool);

		SAFECALL( device->CreateVertexBuffer(size, d3dusage, 0, pool, & pvb, NULL) );
		_pVertexBufer = pvb;

		if(pool == D3DPOOL_DEFAULT)			
		{
			resetter = gcnew ResetTarget(this);			
		}

		_lenght = size;
		_stride = stride;
		_usage = usage;
		_cpuAccesType = cpuAcces;

		if(data!=nullptr)
		{
			void* pData;
			SAFECALL( _pVertexBufer->Lock(0, (UINT)_lenght, &pData, 0) );
			
			 auto element = data->GetType()->GetElementType();
			 auto bytes = data->Length * Marshal::SizeOf(element);
			 GCHandle handle  =GCHandle::Alloc(data, System::Runtime::InteropServices::GCHandleType::Pinned);
			 IntPtr pSource = Marshal::UnsafeAddrOfPinnedArrayElement(data , 0);

			 memcpy(pData, pSource.ToPointer(), bytes );
			 handle.Free();

			 SAFECALL( _pVertexBufer->Unlock() );
		}
		_d3dusage = d3dusage;
		_pool = pool;

		if(pool == D3DPOOL_MANAGED || pool == D3DPOOL_SYSTEMMEM)
		{
			GC::AddMemoryPressure(size);
		}
	}

	IntPtr D3DVertexBuffer::Map(MapType map, bool doNotWait)
	{
		DWORD lock = GetD3DLOCK(map, doNotWait);
		void* pData;
		SAFECALL( _pVertexBufer->Lock(0, _lenght, &pData, lock));

		return IntPtr(pData);
	}

	void D3DVertexBuffer::Unmap()
	{
		SAFECALL( _pVertexBufer->Unlock());
	}

	void  D3DVertexBuffer::OnDispose(bool disposing)
	{
		if(_pVertexBufer)
		{
			_pVertexBufer->Release();	
			_pVertexBufer = NULL;

		}
		if(resetter != nullptr)
		{
			resetter->TargetDisposed();
		}
	}

	void D3DVertexBuffer::DeviceReset(IDirect3DDevice9 * device)
	{
		IDirect3DVertexBuffer9 * pvb;
		SAFECALL( device->CreateVertexBuffer(_lenght, _d3dusage, 0, _pool, & pvb, NULL) );
		_pVertexBufer = pvb;
	}

	void D3DVertexBuffer::DeviceLost(IDirect3DDevice9 * device)
	{
		if(_pVertexBufer)
			_pVertexBufer->Release();
	}

	D3DIndexBuffer::D3DIndexBuffer(IDirect3DDevice9* device , int size,  IndexFormat format , ResourceUsage usage, MapType cpuAcces, Array^ data)
	{
		IDirect3DIndexBuffer9 * pib;

		DWORD	d3dusage = 0;
		D3DPOOL pool;
		D3DFORMAT d3dformat;
		switch (format)
		{
		case Igneel::Graphics::IndexFormat::Index16:
			d3dformat = D3DFMT_INDEX16;
			break;
		case Igneel::Graphics::IndexFormat::Index32:
			d3dformat = D3DFMT_INDEX32;
			break;
		default:
			break;
		}

		GetBufferUsage(cpuAcces, & d3dusage, & pool);

		if(pool == D3DPOOL_DEFAULT)
		{
			resetter = gcnew ResetTarget(this);
		}

		device->CreateIndexBuffer(size, d3dusage, d3dformat , pool, & pib, NULL);
		_pIndexBufer = pib;

		_lenght = size;
		_stride = d3dformat == D3DFMT_INDEX16 ? 2 : 4;
		_usage = usage;
		_cpuAccesType = cpuAcces;

		if(data!=nullptr)
		{
			void* pData;
			SAFECALL( _pIndexBufer->Lock(0, (UINT)_lenght, &pData, D3DLOCK_DISCARD) );

			 System::Type^ element = data->GetType()->GetElementType();
			 GCHandle handle  =GCHandle::Alloc(data, System::Runtime::InteropServices::GCHandleType::Pinned);
			 IntPtr pSource = Marshal::UnsafeAddrOfPinnedArrayElement(data , 0);

			 memcpy(pData, pSource.ToPointer(), data->Length * Marshal::SizeOf(element));
			 handle.Free();

			 SAFECALL( _pIndexBufer->Unlock() );

		}
		_d3dusage  = d3dusage;
		_pool = pool;
		_d3dformat = d3dformat;

		if(pool == D3DPOOL_MANAGED || pool == D3DPOOL_SYSTEMMEM)
		{
			GC::AddMemoryPressure(size);
		}
	}

	IntPtr D3DIndexBuffer::Map(MapType map, bool doNotWait)
	{
		DWORD lock = GetD3DLOCK(map, doNotWait);
		void* pData;
		_pIndexBufer->Lock(0, _lenght, &pData, lock);

		return IntPtr(pData);
	}

	void D3DIndexBuffer::Unmap()
	{
		_pIndexBufer->Unlock();
	}

	void  D3DIndexBuffer::OnDispose(bool )
	{
		if(_pIndexBufer)
		{
			_pIndexBufer->Release();
			_pIndexBufer = NULL;
		}		

		if(resetter!=nullptr)
			resetter->TargetDisposed();
	}

	void D3DIndexBuffer::DeviceReset(IDirect3DDevice9 * device)
	{
		IDirect3DIndexBuffer9 * pib;
		SAFECALL( device->CreateIndexBuffer(_lenght, _d3dusage, _d3dformat , _pool, & pib, NULL) );
		_pIndexBufer = pib;
	}

	void D3DIndexBuffer::DeviceLost(IDirect3DDevice9 * device)
	{
		if(_pIndexBufer)
			_pIndexBufer->Release();
	}
}}
