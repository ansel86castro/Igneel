#include "Stdafx.h"
#include "ShaderSetter.h"
#include "Shaders.h"

namespace IgneelD3D10 {

	ShaderCode^ _CompileFromMemory(String^ shaderCode,												 
								array<ShaderMacro>^ defines,
								Include^ include,
								String^ functionName,
								String^ profile,
								ShaderFlags flags)
	{
		LPCSTR srcData = (LPCSTR)Marshal::StringToHGlobalAnsi(shaderCode).ToPointer();				
		LPCSTR srcfunctionName = (LPCSTR)Marshal::StringToHGlobalAnsi(functionName).ToPointer();
		LPCSTR srcprofile = (LPCSTR)Marshal::StringToHGlobalAnsi(profile).ToPointer();	
		ID3D10Blob * pShaderCode = NULL;
		ID3D10Blob* errorBuffer = NULL;
		ShaderMacro* mcros = NULL;

		if(defines!=nullptr)
		{
			pin_ptr<ShaderMacro> macros = &defines[0];
			mcros = macros;
		}
		try
		{						
			if(FAILED( D3DX10CompileFromMemory(srcData, shaderCode->Length,  NULL, (D3D10_SHADER_MACRO*)mcros ,NULL, srcfunctionName, srcprofile, (UINT)flags, NULL, NULL,&pShaderCode, &errorBuffer, NULL) ))
			{
				 Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcData));
				 Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcfunctionName));
				 Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcprofile));

				 String^ errString = Marshal::PtrToStringAnsi(IntPtr(errorBuffer->GetBufferPointer()), errorBuffer->GetBufferSize());
				 if(pShaderCode)
					 pShaderCode->Release();
				 errorBuffer->Release();
				 throw gcnew InvalidOperationException(errString);
			}

		}
		finally
		{
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcData));
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcfunctionName));
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcprofile));
		}
		auto code = gcnew ShaderCode(IntPtr(pShaderCode->GetBufferPointer()),pShaderCode->GetBufferSize(),true);
		pShaderCode->Release();

		return code;
	}


	ShaderCode^ _CompileFromFile(String^ filename,												
								array<ShaderMacro>^ defines,
								Include^ include,
								String^ functionName,
								String^ profile,
								ShaderFlags flags)
	{
		LPWSTR srcFilename = (LPWSTR)Marshal::StringToHGlobalUni(filename).ToPointer();				
		LPCSTR srcfunctionName = (LPCSTR)Marshal::StringToHGlobalAnsi(functionName).ToPointer();
		LPCSTR srcprofile = (LPCSTR)Marshal::StringToHGlobalAnsi(profile).ToPointer();	

		ShaderMacro* mcros = NULL;
		auto dir = Environment::CurrentDirectory;
		ID3D10Blob * pShaderCode;
		ID3D10Blob* errorBuffer;
		if(defines!=nullptr)
		{
			pin_ptr<ShaderMacro> macros = &defines[0];
			mcros = macros;
		}
		try
		{									
			if(FAILED( D3DX10CompileFromFile(srcFilename, (D3D10_SHADER_MACRO*)mcros, NULL, srcfunctionName, srcprofile, (UINT)flags, NULL, NULL, &pShaderCode, &errorBuffer, NULL) ))
			{
				 Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcFilename));
				 Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcfunctionName));
				 Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcprofile));

				 String^ errString = Marshal::PtrToStringAnsi(IntPtr(errorBuffer->GetBufferPointer()), errorBuffer->GetBufferSize());				 
				  if(pShaderCode)
					 pShaderCode->Release();
				 errorBuffer->Release();
				 throw gcnew InvalidOperationException(errString);
			}
		}
		finally
		{
			Environment::CurrentDirectory = dir;
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcFilename));
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcfunctionName));
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcprofile));
		}

		auto code = gcnew ShaderCode(IntPtr(pShaderCode->GetBufferPointer()),pShaderCode->GetBufferSize(),true);
		pShaderCode->Release();

		return code;
	}

	VertexShader^ VertexShaderSetter::CreateShader(ShaderCode^ bytecode)
	{
		ID3D10VertexShader * vs;

		auto code = bytecode->Code;
		pin_ptr<byte>pterCode = &code[0];

	   SAFECALL( _device->CreateVertexShader(pterCode, code->Length ,&vs) );

		return gcnew  D3DVertexShader(_device, vs, bytecode);
	}

	void VertexShaderSetter::SetShader(VertexShader^ shader)
	{
		D3DVertexShader^ vs = static_cast<D3DVertexShader^>(shader);
		_device->VSSetShader(vs->_pvs);
	}

	ShaderHandler^ VertexShaderSetter::GetHandler(VertexShader^ shader)
	{
		return gcnew VertexShaderHandler(_device, shader);
	}

	ShaderCode^ VertexShaderSetter::CompileFromMemory(String^ shaderCode, String^ functionName , array<ShaderMacro>^ defines)
	{
		auto locator = Service::Get<IShaderRepository^>();
		return _CompileFromMemory(shaderCode, defines, nullptr, functionName, L"vs_"+locator->ShaderModel, locator->CompilerFlags);
	}

	ShaderCode^ VertexShaderSetter::CompileFromFile(String^ filename, String^ functionName, array<ShaderMacro>^ defines)
	{
		auto locator = Service::Get<IShaderRepository^>();
		return _CompileFromFile(filename, defines, nullptr, functionName, L"vs_"+locator->ShaderModel, locator->CompilerFlags);
	}


	PixelShader^ PixelShaderSetter::CreateShader(ShaderCode^ bytecode)
	{
		ID3D10PixelShader * shader;
		auto code = bytecode->Code;
		pin_ptr<byte>pterCode = &code[0];

		SAFECALL( _device->CreatePixelShader(pterCode,code->Length , &shader) );

		return gcnew  D3DPixelShader(_device, shader, bytecode);
	}

	void PixelShaderSetter::SetShader(PixelShader^ shader)
	{
		D3DPixelShader^ vs = static_cast<D3DPixelShader^>(shader);
		_device->PSSetShader(vs->_pps);
	}

	ShaderHandler^ PixelShaderSetter::GetHandler(PixelShader^ shader)
	{
		return gcnew PixelShaderHandler(_device, shader);
	}

	ShaderCode^ PixelShaderSetter::CompileFromMemory(String^ shaderCode, String^ functionName , array<ShaderMacro>^ defines)
	{
		auto locator = Service::Get<IShaderRepository^>();
		return _CompileFromMemory(shaderCode, defines, nullptr, functionName, L"ps_"+locator->ShaderModel, locator->CompilerFlags);
	}

	ShaderCode^ PixelShaderSetter::CompileFromFile(String^ filename, String^ functionName, array<ShaderMacro>^ defines)
	{
		auto locator = Service::Get<IShaderRepository^>();
		return _CompileFromFile(filename, defines, nullptr, functionName, L"ps_"+locator->ShaderModel, locator->CompilerFlags);
	}


	GeometryShader^ GeometryShaderSetter::CreateShader(ShaderCode^ bytecode)
	{
		ID3D10GeometryShader * shader;
		auto code = bytecode->Code;
		pin_ptr<byte>pterCode = &code[0];

		SAFECALL( _device->CreateGeometryShader(pterCode, code->Length , &shader) );

		return gcnew  D3DGeometryShader(_device, shader, bytecode);
	}

	void GeometryShaderSetter::SetShader(GeometryShader^ shader)
	{
		D3DGeometryShader^ gs = static_cast<D3DGeometryShader^>(shader);
		_device->GSSetShader(gs->_shader);
	}

	ShaderCode^ GeometryShaderSetter::CompileFromMemory(String^ shaderCode, String^ functionName , array<ShaderMacro>^ defines)
	{
		auto locator = Service::Get<IShaderRepository^>();
		return _CompileFromMemory(shaderCode, defines, nullptr, functionName, L"gs_"+locator->ShaderModel, locator->CompilerFlags);
	}

	ShaderCode^ GeometryShaderSetter::CompileFromFile(String^ filename, String^ functionName, array<ShaderMacro>^ defines)
	{
		auto locator = Service::Get<IShaderRepository^>();
		return _CompileFromFile(filename, defines, nullptr, functionName, L"gs_"+locator->ShaderModel, locator->CompilerFlags);
	}

	ShaderHandler^ GeometryShaderSetter::GetHandler(GeometryShader^ shader)
	{
		return gcnew GeometryShaderHandler(_device, shader);
	}

	


	VertexShaderHandler::VertexShaderHandler(ID3D10Device* device, VertexShader^ shader)
	{
		this->_device = device;
		this->Function = shader;
	}

	void VertexShaderHandler::Set()
	{
		D3DVertexShader^ vs = static_cast<D3DVertexShader^>(Function);
		auto buffers = vs->ConstantBuffers;		

		for (int i = 0;	i < buffers->Length; i++)
		{
			auto bind = buffers[i];
			bind.handle->Close();			
			ID3D10Buffer* cb = bind.handle->cb; 
			_device->VSSetConstantBuffers(bind.bindPoint, 1, &cb);
		}		
		 _device->VSSetShader(vs->_pvs);
	}

	PixelShaderHandler::PixelShaderHandler(ID3D10Device* device, PixelShader^ shader)
	{
		this->_device = device;
		this->Function = shader;
	}

	void PixelShaderHandler::Set()
	{
		D3DPixelShader^ ps = static_cast<D3DPixelShader^>(Function);
		auto buffers = ps->ConstantBuffers;
		
		for (int i = 0;	i < buffers->Length; i++)
		{
			auto bind = buffers[i];
			bind.handle->Close();
			ID3D10Buffer* cb = bind.handle->cb; 
			_device->PSSetConstantBuffers(bind.bindPoint, 1, &cb);
		}
		 _device->PSSetShader(ps->_pps);
	}


	GeometryShaderHandler::GeometryShaderHandler(ID3D10Device* device, GeometryShader^ shader)
	{
		this->_device = device;
		this->Function = shader;
	}

	void GeometryShaderHandler::Set()
	{
		D3DGeometryShader^ gs = static_cast<D3DGeometryShader^>(Function);		
		auto buffers = gs->ConstantBuffers;
	
		for (int i = 0;	i < buffers->Length; i++)
		{
			auto bind = buffers[i];
			bind.handle->Close();
			ID3D10Buffer* cb = bind.handle->cb;
			_device->GSSetConstantBuffers(bind.bindPoint, 1, &cb);
		}
		 _device->GSSetShader(gs->_shader);
	}
}