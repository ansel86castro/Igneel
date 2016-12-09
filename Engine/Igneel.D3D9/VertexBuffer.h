#pragma once
#include "EnumConverter.h"
#include "Resseting.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public ref class D3DVertexBuffer:public GraphicBuffer, IResetable
	{
		ResetTarget ^ resetter;		
		DWORD _d3dusage;
		D3DPOOL _pool;
		internal:
			IDirect3DVertexBuffer9 * _pVertexBufer;
		
			D3DVertexBuffer(IDirect3DDevice9* device , int size, int stride,  ResourceUsage usage, MapType cpuAcces, Array^ data);

		protected:		
			OVERRIDE(void OnDispose(bool));

		public:

		    OVERRIDE(IntPtr Map(MapType map, bool doNotWait));

			OVERRIDE(void Unmap());

			virtual void DeviceReset(IDirect3DDevice9 * device);

			virtual void DeviceLost(IDirect3DDevice9 * device);
	};

	public ref class D3DIndexBuffer:public GraphicBuffer, IResetable
	{
		ResetTarget ^ resetter;	
		DWORD _d3dusage;
		D3DPOOL _pool;
		D3DFORMAT _d3dformat ;
		internal:
			IDirect3DIndexBuffer9 * _pIndexBufer;
		
			D3DIndexBuffer(IDirect3DDevice9* device , int size,  IndexFormat format , ResourceUsage usage, MapType cpuAcces, Array^ data);

		protected:			
			OVERRIDE(void OnDispose(bool));

		public:

		    OVERRIDE(IntPtr Map(MapType map, bool doNotWait));

			OVERRIDE(void Unmap());

			virtual void DeviceReset(IDirect3DDevice9 * device);

			virtual void DeviceLost(IDirect3DDevice9 * device);
	};

}}