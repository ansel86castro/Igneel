// Direct3D.h

#pragma once

using namespace System;

namespace Igneel {
	namespace D3D9 {

	public ref class Direct3DUT
	{
	public:

		static IntPtr CreateDirect3D9()
		{
			IDirect3D9 *pD3D =  Direct3DCreate9(D3D_SDK_VERSION);
			if(!pD3D)
				throw gcnew InvalidOperationException();			
		
			
			return  static_cast<IntPtr>(pD3D);
		}

		static void GetAdapters(IDirect3D9 *pD3D)
		{
			D3DDISPLAYMODE mode;
			pD3D->GetAdapterDisplayMode(0, &mode);			
			D3DFORMAT format =  mode.Format;
		}
	};

	}
}
