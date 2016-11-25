#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public ref class D3DBlendState : BlendState
	{
	internal:
		D3DBlendState(BlendDesc state):BlendState(state)
		{

		}

		void apply(LPDIRECT3DDEVICE9 device);

		static D3DBlendState^ GetFromDevice(LPDIRECT3DDEVICE9 device);
	};

	public ref class D3DDepthStencilState : DepthStencilState
	{
	internal:
		D3DDepthStencilState(DepthStencilDesc state):DepthStencilState(state)
		{

		}

		void apply(LPDIRECT3DDEVICE9 device);

		static D3DDepthStencilState^ GetFromDevice(LPDIRECT3DDEVICE9 device);
	};

}}