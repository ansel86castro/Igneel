#pragma once
#include "EnumConverter.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public ref class VertexShaderSetter: public IShaderManager<VertexShader^>
	{		
		IDirect3DDevice9 * _device;
	public:
		VertexShaderSetter(IDirect3DDevice9 * device):_device(device) { }

		virtual VertexShader^ CreateShader(DataBuffer^ bytecode);

        virtual void SetShader(VertexShader^ shader);

		virtual ShaderHandler^ GetHandler(VertexShader^ shader);
	};

	public ref class PixelShaderSetter: public IShaderManager<PixelShader^>
	{		
		IDirect3DDevice9 * _device;
	public:
		PixelShaderSetter(IDirect3DDevice9 * device):_device(device) { }

		virtual PixelShader^ CreateShader(DataBuffer^ bytecode);

        virtual void SetShader(PixelShader^ shader);

		virtual ShaderHandler^ GetHandler(PixelShader^ shader);
	};

	public ref class VertexShaderHandler:public ShaderHandler
	{		
		IDirect3DDevice9 * _device;
	public:
		VertexShaderHandler(IDirect3DDevice9 * _device, VertexShader^ shader);

		OVERRIDE(void Set());
	};

	public ref class PixelShaderHandler:public ShaderHandler
	{
		IDirect3DDevice9 * _device;
	public:
		PixelShaderHandler(IDirect3DDevice9 * _device, PixelShader^ shader);

		OVERRIDE(void Set());
	};
}}
