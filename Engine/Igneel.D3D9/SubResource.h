#pragma once
#include "EnumConverter.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public ref class  D3DSubResource: public SubResource
	{
	internal:
		IDirect3DSurface9  * _pSurface;
		int _level;

		event Action<D3DSubResource^>^  SubResourceReset;
		event Action<D3DSubResource^>^  SubResourceLost;			

		void OnSubResourceReset()
		{
			SubResourceReset(this);
		}

		void OnSubResourceLost()
		{
			SubResourceLost(this);
		}

	protected:
		OVERRIDE(void OnDispose(bool));
	};

	public ref class D3DSubResource1D: D3DSubResource
	{
	public:
		D3DSubResource1D(int level, Texture1D^ texture);

		void TextureLost(Texture1D^ tex);

		void TextureReset(Texture1D^ tex);
	};

	public ref class D3DSubResource2D: D3DSubResource
	{
	public:
		D3DSubResource2D(int level, Texture2D^ texture);

		void TextureLost(Texture2D^ tex);

		void TextureReset(Texture2D^ tex);
	};

	public ref class D3DSubResourceCube: D3DSubResource
	{
		D3DCUBEMAP_FACES _face;
	public:
		D3DSubResourceCube(D3DCUBEMAP_FACES face , int level, TextureCube^ texture);

		void TextureLost(TextureCube^ tex);

		void TextureReset(TextureCube^ tex);
	};

}}