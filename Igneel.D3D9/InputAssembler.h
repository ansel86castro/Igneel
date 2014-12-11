#pragma once
#include "EnumConverter.h"
#include "Resseting.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public ref class D3DInputLayout:public InputLayout, IResetable
	{
		ResetTarget^ resetter;
		D3DVERTEXELEMENT9* e;
		int count;

		internal:
			IDirect3DVertexDeclaration9 * _pVertexDecl;

		public:
			D3DInputLayout(IDirect3DDevice9* device, array<VertexElement>^ elements);

			virtual void DeviceReset(IDirect3DDevice9 * device);

			virtual void DeviceLost(IDirect3DDevice9 * device);

		protected:
			OVERRIDE(void OnDispose(bool));
	};

}}