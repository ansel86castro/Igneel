#pragma once
#include "EnumConverter.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public ref class D3DRasterizerState : RasterizerState
	{
	internal:
		D3DRasterizerState(RasterizerDesc state):RasterizerState(state)
		{

		}

		void apply(LPDIRECT3DDEVICE9 device);

		static D3DRasterizerState^ GetFromDevice(LPDIRECT3DDEVICE9 device);
	};
}}