#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel {
	namespace D3D9 {

		public ref class D3D9SampleState:public SamplerState
		{
		internal:
			D3D9SampleState(SamplerDesc desc);
		public:
			void Apply(int reg, IDirect3DDevice9* device);


			/*static D3D9SampleState^ GetFromDevice(LPDIRECT3DDEVICE9 device);*/
		};

	}
}
