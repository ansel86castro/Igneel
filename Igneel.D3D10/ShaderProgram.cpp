#include "Stdafx.h"
#include "ShaderProgram.h"
#include "ShaderSetter.h"
#include "Shaders.h"
#include "D3DUniformSetter.h"
#include "GraphicDevice.h"
#include <vector>


namespace IgneelD3D10 {

	ParameterClass GetClass(D3D10_SHADER_VARIABLE_CLASS c)
	{
		switch (c)
		{
		case D3D_SVC_SCALAR: return ParameterClass::SCALAR;
		case D3D_SVC_VECTOR:return ParameterClass::VECTOR;
		case D3D_SVC_MATRIX_ROWS: 
		case D3D_SVC_MATRIX_COLUMNS: return ParameterClass::MATRIX;
		case D3D_SVC_STRUCT : return ParameterClass::STRUCT;
		case D3D_SVC_OBJECT: return ParameterClass::OBJECT;
		default:
			break;	
		}
		
	}

	ParameterType GetPType(D3D10_SHADER_VARIABLE_TYPE c)
	{
		switch (c)
		{
		case D3D_SVT_VOID: return (ParameterType)0;//ParameterType::VOID;
		case D3D_SVT_BOOL: return ParameterType::BOOL;
		case D3D_SVT_INT:   return ParameterType::INT;
		case D3D_SVT_FLOAT:  return ParameterType::FLOAT;	
		case D3D_SVT_TEXTURE:  return ParameterType::TEXTURE;
		case D3D_SVT_TEXTURE1D:  return ParameterType::TEXTURE1D;
		case D3D_SVT_TEXTURE2D:  return ParameterType::TEXTURE2D;
		case D3D_SVT_TEXTURE3D:  return ParameterType::TEXTURE3D;
		case D3D_SVT_SAMPLER:  return ParameterType::SAMPLER;
		case D3D_SVT_SAMPLER2D:  return ParameterType::SAMPLER2D;
		case D3D_SVT_SAMPLER3D:  return ParameterType::SAMPLER3D;
		case D3D_SVT_SAMPLERCUBE:  return ParameterType::SAMPLERCUBE;						
		default:
			throw gcnew InvalidOperationException(L"InvalidParamter");
			break;	
		}
	}

	UniformDesc^ GetUniform(ID3D10ShaderReflectionType * typeRefl)
	{				
		D3D10_SHADER_TYPE_DESC td;
		typeRefl->GetDesc(&td);

		UniformDesc^ ud = gcnew UniformDesc();
		ud->Class = GetClass(td.Class);
		ud->Elements = td.Elements;		
		ud->Register= RegisterSet::FLOAT4;
		ud->Name = nullptr;
		ud->Type = GetPType(td.Type);
		ud->Columns = td.Columns;
		ud->Rows = td.Rows;		
		ud->Bytes = 0;

		if(td.Members > 0)
		{
			ud->Members = gcnew array<UniformDesc^>(td.Members);

			for (int j = 0; j < td.Members; j++)
			{
				ud->Members[j] = GetUniform(typeRefl->GetMemberTypeByIndex(j));
				ud->Members[j]->Name = Marshal::PtrToStringAnsi(static_cast<IntPtr>((void*)typeRefl->GetMemberTypeName(j)));
			}
		}

		 return ud;
	}

	D3DShaderProgram::D3DShaderProgram(GraphicDevice^ graphicDevice, ShaderProgramDesc^ desc)
	{
		shaders = desc->GetHandles();
		this->InputDefinition = desc->Input;		
		this->_graphicDevice = graphicDevice;
	}

	void D3DShaderProgram::RegisterSetters(ID3D10Device * device)
	{
		RegisterShaderManager(gcnew VertexShaderSetter(device));
		RegisterShaderManager(gcnew PixelShaderSetter(device));
		RegisterShaderManager(gcnew GeometryShaderSetter(device));
	}	

	IUniformSetter^ D3DShaderProgram::CreateUniformSetter(String^ name)
	{
		LPCSTR pName = (LPCSTR)Marshal::StringToHGlobalAnsi(name).ToPointer();

		List<CBufferVarBinding>^ bindings = gcnew List<CBufferVarBinding>(2);
				
		for each (auto var in shaders)
		{
			D3D10Shader^ shader = static_cast<D3D10Shader^>(var->Function);
			auto reflec = shader->ShaderReflection;			
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
				}
			}
		}
		Marshal::FreeHGlobal(IntPtr((void*)pName));		
		if(bindings->Count == 0)
			return nullptr;
		return gcnew D3DUniformSetter(bindings->ToArray());
	}

	bool D3DShaderProgram::IsUniformDefined(String^ name)
	{
		LPCSTR pName = (LPCSTR)Marshal::StringToHGlobalAnsi(name).ToPointer();

		for each (auto var in shaders)
		{
			D3D10Shader^ shader = static_cast<D3D10Shader^>(var->Function);
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
		}

		Marshal::FreeHGlobal(IntPtr((void*)pName));
		return false;
	}
		 
	 array<UniformDesc^>^ D3DShaderProgram::GetUniformDescriptions()
	 {		 
		 List<UniformDesc^>^ uniforms = gcnew List<UniformDesc^>();		 
		 Dictionary<String^, UniformDesc^>^ names = gcnew Dictionary<String^, UniformDesc^>();

		 for each (auto var in shaders)
		{
			D3D10Shader^ shader = static_cast<D3D10Shader^>(var->Function);
			auto reflec = shader->ShaderReflection;			
			auto cbuffers = shader->ConstantBuffers;
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
							auto uniform = GetUniform(varReflec->GetType());
							uniform->Name = name;
							uniform->Bytes = vd.Size;							
							uniforms->Add(uniform);
							names->Add(name,uniform);
						}
					}
				}
			}
		 }		

		 return uniforms->ToArray();
	 }

	 UniformDesc^ D3DShaderProgram::GetUniformDescription(String^ name)
	 {
		 LPCSTR pName = (LPCSTR)Marshal::StringToHGlobalAnsi(name).ToPointer();

		  for each (auto var in shaders)
		{
			D3D10Shader^ shader = static_cast<D3D10Shader^>(var->Function);
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
					D3D10_SHADER_BUFFER_DESC buffDesc;
					ZeroMemory(&buffDesc, sizeof(D3D10_SHADER_BUFFER_DESC));

					bufferRefle->GetDesc(&buffDesc);				
					auto uniform = GetUniform(varReflec->GetType());
					uniform->Name = name;
					uniform->Bytes = vd.Size;					

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
		 interior_ptr<ShaderHandler^> pShaders = &shaders[0];
		 int count = shaders->Length;
		 for (int i = 0; i < count; i++, pShaders++)
		 {
			 (*pShaders)->Set();
		 }
	 }

}