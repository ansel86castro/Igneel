#include "Stdafx.h"
#include "ShaderSetter.h"
#include "Shaders.h"

namespace Igneel { namespace D3D9 {

	VertexShader^ VertexShaderSetter::CreateShader(DataBuffer^ bytecode)
	{
		IDirect3DVertexShader9 * vs;
		D3DShaderByteCode^ d3dcode = static_cast<D3DShaderByteCode^>(bytecode);

	   SAFECALL( _device->CreateVertexShader(static_cast<DWORD*>(bytecode->BufferPointer.ToPointer()), &vs) );

		return gcnew  D3DVertexShader(_device, vs, d3dcode->_ct);
	}

	void VertexShaderSetter::SetShader(VertexShader^ shader)
	{
		D3DVertexShader^ vs = static_cast<D3DVertexShader^>(shader);
		_device->SetVertexShader(vs->_pvs);
	}

	ShaderHandler^ VertexShaderSetter::GetHandler(VertexShader^ shader)
	{
		return gcnew VertexShaderHandler(_device, shader);
	}


	PixelShader^ PixelShaderSetter::CreateShader(DataBuffer^ bytecode)
	{
		IDirect3DPixelShader9 * shader;
		D3DShaderByteCode^ d3dcode = static_cast<D3DShaderByteCode^>(bytecode);

		SAFECALL( _device->CreatePixelShader(static_cast<DWORD*>(bytecode->BufferPointer.ToPointer()), &shader) );

		return gcnew  D3DPixelShader(_device, shader, d3dcode->_ct);
	}

	void PixelShaderSetter::SetShader(PixelShader^ shader)
	{
		D3DPixelShader^ vs = static_cast<D3DPixelShader^>(shader);
		_device->SetPixelShader(vs->_pps);
	}

	ShaderHandler^ PixelShaderSetter::GetHandler(PixelShader^ shader)
	{
		return gcnew PixelShaderHandler(_device, shader);
	}

	VertexShaderHandler::VertexShaderHandler(IDirect3DDevice9* device, VertexShader^ shader)
	{
		this->_device = device;
		this->Function = shader;
	}

	void VertexShaderHandler::Set()
	{
		D3DVertexShader^ vs = static_cast<D3DVertexShader^>(Function);
		SAFECALL( _device->SetVertexShader(vs->_pvs) );
	}

	PixelShaderHandler::PixelShaderHandler(IDirect3DDevice9* device, PixelShader^ shader)
	{
		this->_device = device;
		this->Function = shader;
	}

	void PixelShaderHandler::Set()
	{
		D3DPixelShader^ ps = static_cast<D3DPixelShader^>(Function);
		SAFECALL( _device->SetPixelShader(ps->_pps) );
	}
}}