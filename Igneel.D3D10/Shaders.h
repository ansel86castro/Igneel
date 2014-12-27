#pragma once
#include "ConstantBuffer.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;
using namespace System::Collections::Generic;

namespace IgneelD3D10 {
	
	public ref class D3D10Shader: public ShaderBase
	{
	public:
		ID3D10ShaderReflection* ShaderReflection;
		array<CBufferShaderBinding>^ ConstantBuffers;
		ID3D10DeviceChild* shader;

		D3D10Shader(ID3D10Device * device,ID3D10DeviceChild* Shader, ShaderCode^ bytecode);
		
		virtual void Set(ID3D10Device * _device) abstract = 0;

	protected:
		OVERRIDE(void OnDispose(bool));

	};

	public ref class D3DVertexShader: public D3D10Shader, VertexShader 
	{
	internal:
		ID3D10VertexShader* _pvs;			
		D3DVertexShader(ID3D10Device * device, ID3D10VertexShader* vs, ShaderCode^ bytecode);
	
	public:
		OVERRIDE(void Set(ID3D10Device * _device));

	};

	public ref class D3DPixelShader: public D3D10Shader, PixelShader
	{
	internal:
		ID3D10PixelShader* _pps;					
		D3DPixelShader(ID3D10Device * device, ID3D10PixelShader* ps,  ShaderCode^ bytecode);	

		public:
		OVERRIDE(void Set(ID3D10Device * _device));
	};

	public ref class D3DGeometryShader: public D3D10Shader, GeometryShader
	{
	internal:
		ID3D10GeometryShader* _shader;					
		D3DGeometryShader(ID3D10Device * device, ID3D10GeometryShader* shader,  ShaderCode^ bytecode);	

		public:
		OVERRIDE(void Set(ID3D10Device * _device));
	};	
}