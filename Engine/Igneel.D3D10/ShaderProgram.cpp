#include "Stdafx.h"
#include "ShaderProgram.h"
#include "Shaders.h"
#include "D3DUniformSetter.h"
#include "GraphicDevice.h"
#include <vector>


namespace IgneelD3D10 {

	TypeClass GetClass(D3D10_SHADER_VARIABLE_CLASS c)
	{
		switch (c)
		{
		case D3D10_SVC_SCALAR: 
			return TypeClass::Scalar;
		case D3D10_SVC_VECTOR:
			return TypeClass::Vector;
		case D3D10_SVC_MATRIX_ROWS: 
		case D3D10_SVC_MATRIX_COLUMNS: 
			return TypeClass::Matrix;
		case D3D10_SVC_STRUCT: 
			return TypeClass::Struct;
		case D3D10_SVC_OBJECT: 
			return TypeClass::Object;
		default:
			break;	
		}
		
	}

	ShaderType GetPType(D3D10_SHADER_VARIABLE_TYPE c)
	{
		switch (c)
		{
		case D3D10_SVT_VOID: return ShaderType::Unsupported;//ParameterType::VOID;
		case D3D10_SVT_BOOL: return ShaderType::Bool;
		case D3D10_SVT_INT:   return ShaderType::Int;
		case D3D10_SVT_FLOAT:  return ShaderType::Float;	
		case D3D10_SVT_TEXTURE:  return ShaderType::Texture;
		case D3D10_SVT_TEXTURE1D:  return ShaderType::Texture1D;
		case D3D10_SVT_TEXTURE2D:  return ShaderType::Texture2D;
		case D3D10_SVT_TEXTURE3D:  return ShaderType::Texture3D;
		case D3D10_SVT_SAMPLER:  return ShaderType::Sampler;
		default:
			throw gcnew InvalidOperationException(L"InvalidParamter");
			break;	
		}
	}

	ShaderType GetPType(D3D10_SHADER_INPUT_BIND_DESC& bindDesc)
	{
		switch (bindDesc.Type)
		{
		case D3D10_SIT_TEXTURE:			
				return ShaderType::Texture;			
		case D3D10_SIT_SAMPLER:			
			return ShaderType::Sampler;
		default:
			break;
		}
	}

	ShaderReflectionVariable^ GetUniform(ID3D10ShaderReflectionType * typeRefl)
	{				
		D3D10_SHADER_TYPE_DESC td;
		typeRefl->GetDesc(&td);

		ShaderReflectionVariable^ ud = gcnew ShaderReflectionVariable();
		ShaderReflectionType^ stype= gcnew ShaderReflectionType();
		ud->Type = stype;		

		stype->Class = GetClass(td.Class);
		stype->Elements = td.Elements;		
		stype->Register= RegisterSet::Float4;		
		stype->Type = GetPType(td.Type);
		stype->Columns = td.Columns;
		stype->Rows = td.Rows;		

		if(td.Members > 0)
		{
			stype->Members = gcnew array<ShaderReflectionVariable^>(td.Members);

			for (int j = 0; j < td.Members; j++)
			{
				auto member = GetUniform(typeRefl->GetMemberTypeByIndex(j));
				member->Name = Marshal::PtrToStringAnsi(static_cast<IntPtr>((void*)typeRefl->GetMemberTypeName(j)));
				stype->Members[j] = member;
			}
		}

		 return ud;
	}

	ShaderReflectionVariable^ GetUniform(D3D10_SHADER_INPUT_BIND_DESC& bindDesc)
	{						
		ShaderReflectionVariable^ ud = gcnew ShaderReflectionVariable();
		ShaderReflectionType^ stype= gcnew ShaderReflectionType();
		ud->Type = stype;
		ud->Size = 4;	

		stype->Class =  TypeClass::Object;			
		stype->Register= bindDesc.Type == D3D10_SIT_TEXTURE? RegisterSet::Texture: RegisterSet::Sampler;		
		stype->Type = GetPType(bindDesc);					
		stype->Elements = bindDesc.BindCount;	

		return ud;
	}

	D3DShaderProgram::D3DShaderProgram(GraphicDevice^ graphicDevice, ShaderProgramDesc^ desc)
		:ShaderProgram(desc)
	{				
		this->_graphicDevice = graphicDevice;		
	}
	

	IUniformSetter^ D3DShaderProgram::CreateUniformSetter(String^ name)
	{
		LPCSTR pName = (LPCSTR)Marshal::StringToHGlobalAnsi(name).ToPointer();
		LPCSTR pSamplerName = (LPCSTR)Marshal::StringToHGlobalAnsi( L"s" + name).ToPointer();

		List<CBufferVarBinding>^ bindings = gcnew List<CBufferVarBinding>(2);
	    List<CResourceBinding>^ resourceBindings = gcnew List<CResourceBinding>(2);
		List<int>^ samplerBindings = gcnew List<int>(2);

		for each (auto var in shaders)
		{
			D3D10Shader^ shader = static_cast<D3D10Shader^>(var);
			auto reflec = shader->ShaderReflection;	

			//search into the constant buffers
			auto cbuffers = shader->ConstantBuffers;
			for each (auto cb in cbuffers)
			{
				auto bufferRefle = reflec->GetConstantBufferByIndex(cb.index);
				auto varReflec = bufferRefle->GetVariableByName(pName);
				D3D10_SHADER_VARIABLE_DESC vd;
				ZeroMemory(&vd, sizeof(D3D10_SHADER_VARIABLE_DESC));
				varReflec->GetDesc(&vd);
				if(vd.Name)
				{
					CBufferVarBinding bind = CBufferVarBinding(cb.handle, vd.StartOffset, static_cast<GraphicDevice10^>(_graphicDevice));
					bindings->Add(bind);
					break;
				}
			}
			//search into the resources
			D3D10_SHADER_DESC sd;
			reflec->GetDesc(&sd);
			for (int i = 0; i < sd.BoundResources; i++)
			{
				D3D10_SHADER_INPUT_BIND_DESC bindDesc;
				ZeroMemory(&bindDesc, sizeof(D3D10_SHADER_INPUT_BIND_DESC));
				reflec->GetResourceBindingDesc(i, &bindDesc);				
				if(bindDesc.Name)
				{
					if((bindDesc.Type == D3D10_SIT_TEXTURE || bindDesc.Type == D3D10_SIT_SAMPLER) && !strcmp(bindDesc.Name, pName))
					{										
						CResourceBinding rb = CResourceBinding();
						rb.BindPoint = bindDesc.BindPoint;
						rb.BindPointSampler = bindDesc.BindPoint;
						rb.BindCount = bindDesc.BindCount;
						rb.Register = bindDesc.Type == D3D10_SIT_TEXTURE? RegisterSet::Texture : RegisterSet::Sampler;
						rb.Stage = _graphicDevice->GetShaderStage(shader);
						resourceBindings->Add(rb);
					}
					if(bindDesc.Type == D3D10_SIT_SAMPLER && !strcmp(bindDesc.Name, pSamplerName))
					{						
						samplerBindings->Add(bindDesc.BindPoint);
					}
				}
			}

		}
		Marshal::FreeHGlobal(IntPtr((void*)pName));		
		if(bindings->Count == 0 && resourceBindings->Count == 0)
			return nullptr;

		else if(bindings->Count > 0 && resourceBindings->Count > 0)
			throw gcnew InvalidOperationException(L"A resource and a uniform variable can not share the same name");

		else if(bindings->Count > 0)
			return gcnew D3DUniformSetter(bindings->ToArray());

		else if(resourceBindings->Count > 0)
		{
			auto values = resourceBindings->ToArray();
			for (int i = 0; i < values->Length; i++)
			{
				if(i < bindings->Count)
				{
					values[i].BindPointSampler = samplerBindings[i];
				}
			}
			return gcnew D3DResourceSetter(values);
		}
	}

	bool D3DShaderProgram::IsUniformDefined(String^ name)
	{
		LPCSTR pName = (LPCSTR)Marshal::StringToHGlobalAnsi(name).ToPointer();

		for each (auto var in shaders)
		{
			D3D10Shader^ shader = static_cast<D3D10Shader^>(var);
			auto reflec = shader->ShaderReflection;			
			auto cbuffers = shader->ConstantBuffers;
			for each (auto cb in cbuffers)
			{
				auto bufferRefle = reflec->GetConstantBufferByIndex(cb.index);
				auto varReflec = bufferRefle->GetVariableByName(pName);
				D3D10_SHADER_VARIABLE_DESC vd;
				ZeroMemory(&vd, sizeof(D3D10_SHADER_VARIABLE_DESC));
				varReflec->GetDesc(&vd);
				if(vd.Name && (vd.uFlags & D3D10_SVF_USED))
				{
					Marshal::FreeHGlobal(IntPtr((void*)pName));
					return true;
				}
			}

			D3D10_SHADER_DESC sd;
			reflec->GetDesc(&sd);
			for (int i = 0; i < sd.BoundResources; i++)
			{
				D3D10_SHADER_INPUT_BIND_DESC bindDesc;
				ZeroMemory(&bindDesc, sizeof(D3D10_SHADER_INPUT_BIND_DESC));
				reflec->GetResourceBindingDesc(i, &bindDesc);
				if(bindDesc.Name && !strcmp(bindDesc.Name, pName))
				{					
					Marshal::FreeHGlobal(IntPtr((void*)pName));
					return true;					
				}
			}
		}

		Marshal::FreeHGlobal(IntPtr((void*)pName));
		return false;
	}
		 
	 array<ShaderReflectionVariable^>^ D3DShaderProgram::GetUniformDescriptions()
	 {		 
		 List<ShaderReflectionVariable^>^ uniforms = gcnew List<ShaderReflectionVariable^>();		 
		 Dictionary<String^, ShaderReflectionVariable^>^ names = gcnew Dictionary<String^, ShaderReflectionVariable^>();

		 for each (auto var in shaders)
		{
			D3D10Shader^ shader = static_cast<D3D10Shader^>(var);
			auto reflec = shader->ShaderReflection;			
			auto cbuffers = shader->ConstantBuffers;
			auto astRefle = shader->ASTReflection;

			for each (auto cb in cbuffers)
			{
				auto bufferRefle = reflec->GetConstantBufferByIndex(cb.index);
				D3D10_SHADER_BUFFER_DESC buffDesc;
				ZeroMemory(&buffDesc, sizeof(D3D10_SHADER_BUFFER_DESC));

				bufferRefle->GetDesc(&buffDesc);
				for (int ivar = 0; ivar < buffDesc.Variables; ivar++)
				{
					auto varReflec = bufferRefle->GetVariableByIndex(ivar);
					D3D10_SHADER_VARIABLE_DESC vd;
					ZeroMemory(&vd, sizeof(D3D10_SHADER_VARIABLE_DESC));

					varReflec->GetDesc(&vd);
					if(vd.uFlags & D3D10_SVF_USED)
					{
						String^name = Marshal::PtrToStringAnsi(static_cast<IntPtr>((void*)vd.Name));
						if(!names->ContainsKey(name))
						{
							ShaderReflectionVariable^uniform = nullptr;
							if(astRefle!=nullptr)
							{
								auto astVar = astRefle->GetGlobal(name);
								if(astVar!=nullptr)
									uniform = astVar;
							}
							else
							{
								uniform = GetUniform(varReflec->GetType());
								uniform->Name = name;
							}
							uniform->Size = vd.Size;							
							
							uniforms->Add(uniform);
							names->Add(name,uniform);
						}
					}
				}
			}			
			
			D3D10_SHADER_DESC sd;
			reflec->GetDesc(&sd);
			for (int i = 0; i < sd.BoundResources; i++)
			{
				D3D10_SHADER_INPUT_BIND_DESC bindDesc;
				ZeroMemory(&bindDesc, sizeof(D3D10_SHADER_INPUT_BIND_DESC));
				reflec->GetResourceBindingDesc(i, &bindDesc);				
				if(bindDesc.Name && (bindDesc.Type == D3D10_SIT_TEXTURE || 
									 bindDesc.Type == D3D10_SIT_SAMPLER))
				{					
					String^name = Marshal::PtrToStringAnsi(static_cast<IntPtr>((void*)bindDesc.Name));
					if(!names->ContainsKey(name))
					{
						auto uniform = GetUniform(bindDesc);
						uniform->Name = name;											
						uniforms->Add(uniform);
						names->Add(name,uniform);
					}
				}
			}
		 }		

		 return uniforms->ToArray();
	 }

	 ShaderReflectionVariable^ D3DShaderProgram::GetUniformDescription(String^ name)
	 {
		 LPCSTR pName = (LPCSTR)Marshal::StringToHGlobalAnsi(name).ToPointer();

		for each (auto var in shaders)
		{
			D3D10Shader^ shader = static_cast<D3D10Shader^>(var);
			auto reflec = shader->ShaderReflection;			
			auto cbuffers = shader->ConstantBuffers;

			auto astReflection = shader->ASTReflection;			

			for each (auto cb in cbuffers)
			{
				auto bufferRefle = reflec->GetConstantBufferByIndex(cb.index);
				auto varReflec = bufferRefle->GetVariableByName(pName);
				D3D10_SHADER_VARIABLE_DESC vd;
				ZeroMemory(&vd, sizeof(D3D10_SHADER_VARIABLE_DESC));

				varReflec->GetDesc(&vd);
				if(vd.Name && (vd.uFlags & D3D10_SVF_USED))
				{
					D3D10_SHADER_BUFFER_DESC buffDesc;
					ZeroMemory(&buffDesc, sizeof(D3D10_SHADER_BUFFER_DESC));

					bufferRefle->GetDesc(&buffDesc);				
					auto uniform = GetUniform(varReflec->GetType());
					uniform->Name = name;
					uniform->Size = vd.Size;					

					Marshal::FreeHGlobal(IntPtr((void*)pName));
					return uniform;
				}				
			}

			D3D10_SHADER_DESC sd;
			reflec->GetDesc(&sd);
			for (int i = 0; i < sd.BoundResources; i++)
			{
				D3D10_SHADER_INPUT_BIND_DESC bindDesc;
				ZeroMemory(&bindDesc, sizeof(D3D10_SHADER_INPUT_BIND_DESC));
				reflec->GetResourceBindingDesc(i, &bindDesc);
				
				if(bindDesc.Name && !strcmp(bindDesc.Name, pName))
				{										
					auto uniform = GetUniform(bindDesc);
					uniform->Name = name;
					Marshal::FreeHGlobal(IntPtr((void*)pName));
					return uniform;
				}
			}
		 }		

		  Marshal::FreeHGlobal(IntPtr((void*)pName));
		  return nullptr;
	 }

	 void D3DShaderProgram::SetShaders(ID3D10Device* device)
	 {		 		 		 
		 interior_ptr<Shader^> pShaders = &shaders[0];
		 int count = shaders->Length;
		 for (int i = 0; i < count; i++, pShaders++)
		 {
			 static_cast<D3D10Shader^>((*pShaders))->Set(device);
		 }
	 }

}