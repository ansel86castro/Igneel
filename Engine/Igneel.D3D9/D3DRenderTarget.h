#pragma once
#include "EnumConverter.h"
#include "Resseting.h"
#include "SubResource.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public ref class D3DRenderTargetSurf: public  RenderTarget, IResetable
	{
		ResetTarget ^ resseter;
		D3DSURFACE_DESC * _surfDesc;
	internal:
		IDirect3DSurface9 * _pSurface;
		IDirect3DSurface9 * _pSysMemSurface;
		IDirect3DSurface9 * _pMsaaSurf;
		LPDIRECT3DDEVICE9 _pDevice;		

		D3DRenderTargetSurf(LPDIRECT3DDEVICE9 device, SubResource^ subResource, bool readEnable, Multisampling multisampling);

		D3DRenderTargetSurf(LPDIRECT3DDEVICE9 device, int width, int height , Format format, Multisampling sampling, bool cpuReadEnable);

		D3DRenderTargetSurf(LPDIRECT3DDEVICE9 device, IDirect3DSurface9 * surface);

		void Init(LPDIRECT3DDEVICE9 device, bool readEnable , Multisampling multisampling);

		protected:
			OVERRIDE(void OnDispose(bool));

		internal:
			 void RenderEnd();

		public:
			OVERRIDE(MappedTexture2D Map());

			OVERRIDE(void UnMap());

			virtual void DeviceReset(IDirect3DDevice9 * device);

			virtual void DeviceLost(IDirect3DDevice9 * device);

			void ResourceReset(D3DSubResource^ res);

			void ResourceLost(D3DSubResource^ res);			
	};

}}