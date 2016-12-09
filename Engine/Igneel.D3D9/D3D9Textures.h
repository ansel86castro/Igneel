#pragma once
#include "EnumConverter.h"
#include "Resseting.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public ref class D3D9Texture1D : public Texture1D , IResetable
	{
		DWORD  _d3dUsage;
		D3DPOOL  _d3dPool;
		D3DFORMAT _format;
		ResetTarget^ resseter;
	internal:
		IDirect3DTexture9 * _pTexture;

		
		event Action<Texture1D^>^  TextureReset;
		event Action<Texture1D^>^  TextureLost;

		internal:
		D3D9Texture1D(IDirect3DDevice9* pDevice, Texture1DDesc^ desc);
		D3D9Texture1D(IDirect3DTexture9* tex);

		protected:
		OVERRIDE(void OnDispose(bool));

		public:
		OVERRIDE( IntPtr Map(int subResource, MapType map, bool doNotWait) );

		OVERRIDE( void UnMap(int subResource) );

		OVERRIDE( SubResource^ GetSubResource(int level));
		
		public:
		virtual void DeviceReset(IDirect3DDevice9 * device);

		virtual void DeviceLost(IDirect3DDevice9 * device);
	};

	public ref class D3D9Texture2D : public Texture2D , IResetable	
	{
		DWORD  _d3dUsage;
		D3DPOOL  _d3dPool;
		D3DFORMAT _format;
		ResetTarget^ resseter;

		internal:
		IDirect3DTexture9 * _pTexture;

		event Action<Texture2D^>^  TextureReset;
		event Action<Texture2D^>^  TextureLost;

		internal:
		D3D9Texture2D(IDirect3DDevice9* pDevice, Texture2DDesc^ desc);
		D3D9Texture2D(IDirect3DTexture9* tex);

		protected:
		OVERRIDE(void OnDispose(bool));

		public:
		OVERRIDE(MappedTexture2D Map(int subResource, MapType map, bool doNotWait));

		OVERRIDE(void UnMap(int subResource));

		OVERRIDE( SubResource^ GetSubResource(int level));

		public:
		virtual void DeviceReset(IDirect3DDevice9 * device);

		virtual void DeviceLost(IDirect3DDevice9 * device);
		
	};

	public ref class D3D9Texture3D : public Texture3D , IResetable
	{
		DWORD  _d3dUsage;
		D3DPOOL  _d3dPool;
		D3DFORMAT _format;
		ResetTarget^ resseter;

		internal:
		IDirect3DVolumeTexture9 * _pTexture;

		event Action<D3D9Texture3D^>^  TextureReset;
		event Action<D3D9Texture3D^>^  TextureLost;

		internal:
		D3D9Texture3D(IDirect3DDevice9* pDevice, Texture3DDesc^ desc);
		D3D9Texture3D(IDirect3DVolumeTexture9* tex);

		protected:
			OVERRIDE(void OnDispose(bool));

		public:

			OVERRIDE(MappedTexture3D Map(int subResource, MapType map, bool doNotWait));

			OVERRIDE(void UnMap(int subResource));			

			OVERRIDE( SubResource^ GetSubResource(int level));

		public:
		virtual void DeviceReset(IDirect3DDevice9 * device);

		virtual void DeviceLost(IDirect3DDevice9 * device);
	};

	public ref class D3D9TextureCube : public TextureCube , IResetable
	{
		DWORD  _d3dUsage;
		D3DPOOL  _d3dPool;
		D3DFORMAT _format;
		ResetTarget^ resseter;

		internal:
		IDirect3DCubeTexture9 * _pTexture;
		
		event Action<TextureCube^>^  TextureReset;
		event Action<TextureCube^>^  TextureLost;

		internal:
			D3D9TextureCube (IDirect3DDevice9* pDevice, TextureCubeDesc^ desc);
			D3D9TextureCube(IDirect3DCubeTexture9* tex);

		protected:
			OVERRIDE(void OnDispose(bool));

		public:

			OVERRIDE(MappedTexture2D Map(int face, int subResource, MapType map, bool doNotWait));

			OVERRIDE(void UnMap(int face, int subResource));			

			OVERRIDE( SubResource^ GetSubResource(int face, int level));

		public:
			virtual void DeviceReset(IDirect3DDevice9 * device);

			virtual void DeviceLost(IDirect3DDevice9 * device);
	};

}}


