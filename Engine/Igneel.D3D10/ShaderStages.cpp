#include "Stdafx.h"
#include "ShaderStages.h"
#include "SamplerState10.h"
#include "IResouceContainer.h"
#include "GraphicBuffer10.h"
#include "Texture1D10.h"
#include "Texture2D10.h"
#include "Texture3D10.h"
#include "Shaders.h"
#include "Utils.h"

using namespace Igneel::Compiling;

namespace IgneelD3D10
{
	ShaderCode^ _CompileFromMemory(String^ shaderCode,												 
								array<ShaderMacro>^ defines,								
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
	
	//**** Shader Stage 10

	generic<typename T>
	ShaderStage10<T>::ShaderStage10(ID3D10Device* device, String^ profile)
	{
		_device = device;
		_profile = profile;
		_holder.Init();		
	}

	generic<typename T>
	void ShaderStage10<T>::OnSetSampler(int slot, SamplerState^ state)
	{
		ID3D10SamplerState* buff[1] = { NULL };		
		if(state == nullptr)		
				SetSamplers(slot, 1, buff);		
		else
		{
			SamplerState10^ stateImp = static_cast<SamplerState10^>(state);
			buff[0] = stateImp->_sampler;
			SetSamplers(slot, 1, buff);		
		}
	}

	generic<typename T>
	void ShaderStage10<T>::OnSetSamplers(int slot, int numSamplers, array<SamplerState^,1>^ states)
	{		
		if(states == nullptr)
		{
			for (int i = 0; i < numSamplers; i++)
			{
				_holder._pSamplers[i] = NULL;
			}
		}
		else
		{
			for (int i = 0; i < numSamplers; i++)
			{
				SamplerState10^ samp = static_cast<SamplerState10^>(states[i]);
				if(samp == nullptr)
					_holder._pSamplers[i] = NULL;
				else
					_holder._pSamplers[i] = samp->_sampler;
			}
		}
		SetSamplers(slot,  numSamplers, _holder._pSamplers);			
	}

	generic<typename T>
	void ShaderStage10<T>::OnSetResource(int index , IShaderResource^ resource)
	{
		ID3D10ShaderResourceView* views[1] = { NULL };
		if(resource != nullptr)		
		{			
			switch (resource->Type)
			{
				case ResourceType::Buffer:
					{
						GraphicBuffer10^ buffer = static_cast<GraphicBuffer10^>( resource);
						if(!buffer->_srv)
							throw gcnew InvalidOperationException("Invalid Buffer");
						views[0] = buffer->_srv;
					}
				break;
				case ResourceType::Texture1D:		
					views[0] = static_cast<Texture1D10^>(resource)->_shaderResource;				
				break;
				case ResourceType::Texture2D:		
					views[0] = static_cast<Texture2D10^>(resource)->_shaderResource;				
				break;
				case ResourceType::Texture3D:		
					views[0] = static_cast<Texture3D10^>(resource)->_shaderResource;
				break;
			}			
		}
		SetResources(index, 1, views);		
	}

	generic<typename T>
	void ShaderStage10<T>::OnSetResources(int index, int nbResources, array<IShaderResource^,1>^ resources)
	{		
		if(resources!=nullptr)
		{
			for (int i = 0; i < nbResources; i++)
			{
				IGraphicResource^ resource = resources[i];									
				if(resource==nullptr)
					_holder._pResouces[i] = NULL;
				else
				{
					switch (resource->Type)
					{
						case ResourceType::Buffer:
						{
							GraphicBuffer10^ buffer = static_cast<GraphicBuffer10^>( resource);
							if(!buffer->_srv)
								throw gcnew InvalidOperationException("Invalid Buffer");
							_holder._pResouces[i]= buffer->_srv;
						}
						break;
						case ResourceType::Texture1D:		
							_holder._pResouces[i] = static_cast<Texture1D10^>(resource)->_shaderResource;					
						break;
						case ResourceType::Texture2D:		
							_holder._pResouces[i] = static_cast<Texture2D10^>(resource)->_shaderResource;						
						break;
						case ResourceType::Texture3D:		
							_holder._pResouces[i] = static_cast<Texture3D10^>(resource)->_shaderResource;						
						break;
					}
				}
			}
		}
		else
		{
			ZeroMemory(_holder._pResouces, nbResources * sizeof(ID3D10ShaderResourceView*));
		}
		SetResources(index, nbResources, _holder._pResouces);
	
	}

	generic<typename T>
	ShaderStage10<T>::!ShaderStage10()
	{
		_holder.Destroy();
	}

	generic<typename T>
	ShaderCode^ ShaderStage10<T>::CompileFromMemory(String^ shaderCode, String^ functionName , array<ShaderMacro>^ defines)
	{
		auto locator = Service::Get<IShaderRepository^>();

		auto compiler = HlslCompiler::CreateASTFromString(shaderCode, defines);
		compiler->EntryPoint = functionName;
		auto source = compiler->GenerateCode();

		auto code = _CompileFromMemory(source, nullptr, functionName, _profile+locator->ShaderModel, locator->CompilerFlags);
		code->Reflection = compiler->GetShaderReflection();
		return code;
	}

	generic<typename T>
	ShaderCode^ ShaderStage10<T>::CompileFromFile(String^ filename, String^ functionName, array<ShaderMacro>^ defines)
	{
		auto locator = Service::Get<IShaderRepository^>();

		/*auto compiler = HlslCompiler::CreateASTFromFile(filename, defines);
		compiler->EntryPoint = functionName;
		auto source = compiler->GenerateCode();*/

		String^targetProfile = _profile;
		ShaderFlags compileFlags = ShaderFlags::PackMatrixRowMajor | ShaderFlags::OptimizationLevel3;
		if(locator != nullptr){
			targetProfile += locator->ShaderModel;
			compileFlags = locator->CompilerFlags;
		}else{
			targetProfile += L"4_0";
		}

		//auto code = _CompileFromMemory(source, nullptr, functionName, _profile+locator->ShaderModel, locator->CompilerFlags);
		auto code = _CompileFromFile(filename, nullptr, functionName, targetProfile, compileFlags);
		//code->Reflection = compiler->GetShaderReflection();
		return code;
	}

	//********* Vertex Shader Stage*************************

	void VSStage10::SetSamplers(int slot, int num , ID3D10SamplerState** samplers)
	{
		_device->VSSetSamplers(slot, num, samplers);
	}

	void VSStage10::SetResources(int index, int num, ID3D10ShaderResourceView** resources)
	{
		_device->VSSetShaderResources(index, num, resources);
	}

	VertexShader^ VSStage10::CreateShader(ShaderCode^ bytecode)
	{
		ID3D10VertexShader * vs;

		auto code = bytecode->Code;
		pin_ptr<byte>pterCode = &code[0];

	   SAFECALL( _device->CreateVertexShader(pterCode, code->Length ,&vs) );

		return gcnew D3DVertexShader(_device, vs, bytecode);
	}	


	//********** PIXEL SHADER STAGE *****************************
	
	void PSStage10::SetSamplers(int slot, int num , ID3D10SamplerState** samplers)
	{
		_device->PSSetSamplers(slot, num, samplers);
	}

	void PSStage10::SetResources(int index, int num, ID3D10ShaderResourceView** resources)
	{
		_device->PSSetShaderResources(index, num, resources);
	}
	
	PixelShader^ PSStage10::CreateShader(ShaderCode^ bytecode)
	{
		ID3D10PixelShader * shader;
		auto code = bytecode->Code;
		pin_ptr<byte>pterCode = &code[0];

		SAFECALL( _device->CreatePixelShader(pterCode,code->Length , &shader) );

		return gcnew  D3DPixelShader(_device, shader, bytecode);
	}

	//************** GEOMETRY SHADER STAGE ***********************

	void GSStage10::SetSamplers(int slot, int num , ID3D10SamplerState** samplers)
	{
		_device->GSSetSamplers(slot, num, samplers);
	}

	void GSStage10::SetResources(int index, int num, ID3D10ShaderResourceView** resources)
	{
		_device->GSSetShaderResources(index, num, resources);
	}

	GeometryShader^ GSStage10::CreateShader(ShaderCode^ bytecode)
	{
		ID3D10GeometryShader * shader;
		auto code = bytecode->Code;
		pin_ptr<byte>pterCode = &code[0];

		SAFECALL( _device->CreateGeometryShader(pterCode, code->Length , &shader) );

		return gcnew D3DGeometryShader(_device, shader, bytecode);
	}

	GeometryShader^ GSStage10::CreateShaderWithSO(ShaderCode^ bytecode, array<StreamOutDeclaration>^ declaration ,array<int>^ bufferStrides, bool rasterizedStream0)
	{
		ID3D10GeometryShader * shader;
		auto code = bytecode->Code;
		pin_ptr<byte>pterCode = &code[0];

		D3D10_SO_DECLARATION_ENTRY* e = new D3D10_SO_DECLARATION_ENTRY [declaration->Length];		
		ZeroMemory(e, sizeof(D3D10_SO_DECLARATION_ENTRY) * declaration->Length);
	
		for (int i = 0; i < declaration->Length; i++)
		{
			e[i].SemanticName = getSemanticName(declaration[i].Semantic);;			
			e[i].OutputSlot = declaration[i].OutputSlot;			
			e[i].SemanticIndex = declaration[i].SemanticIndex;
			e[i].ComponentCount = declaration[i].ComponentCount;
			e[i].StartComponent = declaration[i].StartComponent;			
		}

		try{			
			
			SAFECALL( _device->CreateGeometryShaderWithStreamOutput(pterCode, code->Length ,e, declaration->Length, bufferStrides[0] ,&shader) );
			delete[] e;
		}
		catch(Exception^ ex)
		{
			delete[] e;
			throw ex;
		}		

		return gcnew D3DGeometryShader(_device, shader, bytecode);
	}	

	void GSStage10::SetSOBuffer(GraphicBuffer^ buffer, int offset)
	{
		_soTargetBind[0]= buffer;
        _soTargetOffets[0] = offset;

		GraphicBuffer10^ buff = static_cast<GraphicBuffer10^>(buffer);
		ID3D10Buffer* b[1] = { buff->_buffer };
		_device->SOSetTargets(1 , b, (UINT*)&offset);
	}

    void GSStage10::SetSOBuffer(array<GraphicBuffer^>^ buffers, array<int>^ offsets)
	{
		Array::Copy(buffers, _soTargetBind, buffers->Length);
        Array::Copy(offsets, _soTargetOffets, offsets->Length);

		array<GraphicBuffer10^>^ buff = static_cast<array<GraphicBuffer10^>^ >(buffers);
		ID3D10Buffer* b[4];
		UINT o[4];

		ZeroMemory(&b, sizeof(b));
		ZeroMemory(&o, sizeof(o));
		if(buffers!=nullptr)
		{
			for (int i = 0; i < buffers->Length; i++)
			{
				b[i] = buff[i]->_buffer;			
			}
		}
		if(offsets!=nullptr)
		{
			for (int i = 0; i < offsets->Length; i++)
			{
				o[i] = offsets[i];
			}
		}

		_device->SOSetTargets(buffers->Length , b, o);
	}

    void GSStage10::GetSOBuffer(array<GraphicBuffer^>^ buffers, array<int>^ offsets)
	{
		Array::Copy(_soTargetBind, buffers, buffers->Length);
        Array::Copy(_soTargetOffets, offsets, offsets->Length);
	}
}