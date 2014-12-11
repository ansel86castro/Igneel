#pragma once
#include "EnumConverter.h"
#include "Resseting.h"
#include "SubResource.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public ref class D3DDephtStencil:public DepthStencil , IResetable
	{
		ResetTarget^ resseter;
		D3DSURFACE_DESC * _surfDesc;

		internal:
		IDirect3DSurface9 * _pSurface;		
		IDirect3DSurface9 * _pSysMemSurface;
		IDirect3DSurface9 * _pMsaaSurf;

		D3DDephtStencil(LPDIRECT3DDEVICE9 device, int width, int height, Format format, Multisampling sampling, bool cpuReadEnable);

		D3DDephtStencil(LPDIRECT3DDEVICE9 device, SubResource^ subResource , bool readEnable , Multisampling multisampling);

		D3DDephtStencil(LPDIRECT3DDEVICE9 device, IDirect3DSurface9 * surface);

		void Init(LPDIRECT3DDEVICE9 device, bool readEnable , Multisampling multisampling);

	protected:
		OVERRIDE(void OnDispose(bool));

	public:
		virtual void DeviceReset(IDirect3DDevice9 * device);

		virtual void DeviceLost(IDirect3DDevice9 * device);

		void ResourceReset(D3DSubResource^ res);

		void ResourceLost(D3DSubResource^ res);	
	};
}}