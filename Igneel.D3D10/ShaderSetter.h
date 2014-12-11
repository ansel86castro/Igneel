#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace IgneelD3D10 {
	
	public ref class VertexShaderSetter: public IShaderFactory<VertexShader^>
	{		
		ID3D10Device * _device;
	public:
		VertexShaderSetter(ID3D10Device * device):_device(device) { }

		virtual VertexShader^ CreateShader(ShaderCode^ bytecode);

        virtual void SetShader(VertexShader^ shader);

		virtual ShaderHandler^ GetHandler(VertexShader^ shader);

		virtual ShaderCode^ CompileFromMemory(String^ shaderCode, String^ functionName , array<ShaderMacro>^ defines);

        virtual ShaderCode^ CompileFromFile(String^ filename, String^ functionName, array<ShaderMacro>^ defines);
	};

	public ref class PixelShaderSetter: public IShaderFactory<PixelShader^>
	{		
		ID3D10Device * _device;
	public:
		PixelShaderSetter(ID3D10Device * device):_device(device) { }

		virtual PixelShader^ CreateShader(ShaderCode^ bytecode);

        virtual void SetShader(PixelShader^ shader);

		virtual ShaderHandler^ GetHandler(PixelShader^ shader);

		virtual ShaderCode^ CompileFromMemory(String^ shaderCode, String^ functionName , array<ShaderMacro>^ defines);

        virtual ShaderCode^ CompileFromFile(String^ filename, String^ functionName, array<ShaderMacro>^ defines);
	};

	public ref class GeometryShaderSetter: public IShaderFactory<GeometryShader^>
	{		
		ID3D10Device * _device;
	public:
		GeometryShaderSetter(ID3D10Device * device):_device(device) { }

		virtual GeometryShader^ CreateShader(ShaderCode^ bytecode);

        virtual void SetShader(GeometryShader^ shader);

		virtual ShaderHandler^ GetHandler(GeometryShader^ shader);

		virtual ShaderCode^ CompileFromMemory(String^ shaderCode, String^ functionName , array<ShaderMacro>^ defines);

        virtual ShaderCode^ CompileFromFile(String^ filename, String^ functionName, array<ShaderMacro>^ defines);
	};

	public ref class VertexShaderHandler:public ShaderHandler
	{		
		ID3D10Device * _device;
	public:
		VertexShaderHandler(ID3D10Device * _device, VertexShader^ shader);

		OVERRIDE(void Set());
	};

	public ref class PixelShaderHandler:public ShaderHandler
	{
		ID3D10Device * _device;
	public:
		PixelShaderHandler(ID3D10Device * _device, PixelShader^ shader);

		OVERRIDE(void Set());
	};

	public ref class GeometryShaderHandler:public ShaderHandler
	{
		ID3D10Device * _device;
	public:
		GeometryShaderHandler(ID3D10Device * _device, GeometryShader^ shader);

		OVERRIDE(void Set());
	};
}
