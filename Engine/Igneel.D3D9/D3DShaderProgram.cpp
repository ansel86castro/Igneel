#include "Stdafx.h"
#include "D3DShaderProgram.h"
#include "ShaderSetter.h"
#include "Shaders.h"
#include "D3DUniformSetter.h"


namespace Igneel { namespace D3D9 {

	ParameterClass GetClass(D3DXPARAMETER_CLASS c)
	{
		switch (c)
		{
		case D3DXPC_SCALAR: return ParameterClass::SCALAR;
		case D3DXPC_VECTOR:return ParameterClass::VECTOR;
		case D3DXPC_MATRIX_ROWS: 
		case D3DXPC_MATRIX_COLUMNS: return ParameterClass::MATRIX;
		case D3DXPC_STRUCT : return ParameterClass::STRUCT;
		case D3DXPC_OBJECT: return ParameterClass::OBJECT;
		default:
			break;	
		}
	}

	ParameterType GetPType(D3DXPARAMETER_TYPE c)
	{
		switch (c)
		{
		case D3DXPT_VOID: return (ParameterType)0;//ParameterType::VOID;
		case D3DXPT_BOOL: return ParameterType::BOOL;
		case D3DXPT_INT:   return ParameterType::INT;
		case D3DXPT_FLOAT:  return ParameterType::FLOAT;		
		default:
			throw gcnew InvalidOperationException(L"InvalidParamter");
			break;	
		}
	}

	void D3DShaderProgram::RegisterSetters(IDirect3DDevice9 * device)
	{
		RegisterShaderManager(gcnew VertexShaderSetter(device));
		RegisterShaderManager(gcnew PixelShaderSetter(device));
	}

	D3DShaderProgram::D3DShaderProgram(ShaderProgramDesc^ desc)
	{
		shaders = desc->GetHandles();
		this->InputDefinition = desc->Input;
	}

	IUniformSetter^ D3DShaderProgram::CreateUniformSetter(String^ name)
	{
		LPCSTR pName = (LPCSTR)Marshal::StringToHGlobalAnsi(name).ToPointer();

		array<ID3DXConstantTable*>^ tables = gcnew array<ID3DXConstantTable*>(shaders->Length);
		array<D3DXHANDLE>^handles = gcnew array<D3DXHANDLE>(shaders->Length);		

		int i=0;
		for each (auto var in shaders)
		{
			ID3DShader^ shader = (ID3DShader^)var->Function;
			D3DXHANDLE handle = shader->GetConstantTable()->GetConstantByName(NULL,pName);
			if(handle)
			{
				handles[i] = handle;
				tables[i] = shader->GetConstantTable();
				i++;
			}			
		}

		Marshal::FreeHGlobal(IntPtr((void*)pName));
		if(i == 0)
			return nullptr;

		if(i < handles->Length)
		{
			array<ID3DXConstantTable*>^ tables2 = gcnew array<ID3DXConstantTable*>(i);
			array<D3DXHANDLE>^handles2 = gcnew array<D3DXHANDLE>(i);

			for (int j = 0; j < i; j++)
			{
				tables2[j] = tables[j];
				handles2[j] = handles[j];
			}

			tables = tables2;
			handles  = handles2;
		}

		return gcnew D3DUniformSetter(tables, handles);		
	}

	bool D3DShaderProgram::IsUniformDefined(String^ name)
	{
		LPCSTR pName = (LPCSTR)Marshal::StringToHGlobalAnsi(name).ToPointer();

		for each (auto var in shaders)
		{
			if(var->Function->GetType() == D3DVertexShader::typeid)
			{
				D3DVertexShader^ vs = static_cast<D3DVertexShader^>(var->Function);
				D3DXHANDLE handle = vs->constantTable->GetConstantByName(NULL,pName);
				if(handle)
				{
					Marshal::FreeHGlobal(IntPtr((void*)pName));
					return true; 
				}
			}

			else if(var->Function->GetType() == D3DPixelShader::typeid)
			{
				D3DPixelShader^ ps = static_cast<D3DPixelShader^>(var->Function);
				D3DXHANDLE handle = ps->constantTable->GetConstantByName(NULL, pName);
				if(handle)
				{
					Marshal::FreeHGlobal(IntPtr((void*)pName));
					return true;
				}
			}		
		}

		Marshal::FreeHGlobal(IntPtr((void*)pName));
		return false;
	}

	int D3DShaderProgram::GetUniformCount()
	 {
		 int count = 0;
		 Dictionary<String^, IntPtr>^ map = gcnew Dictionary<String^, IntPtr>();

		for each (auto var in shaders)
		{
			ID3DShader^ shader = (ID3DShader^)var->Function;
			D3DXCONSTANTTABLE_DESC desc;
			shader->GetConstantTable()->GetDesc(&desc);
			count = max(count, desc.Constants);
		
		}		 
		 return count;
	 }	 

	 UniformDesc^ D3DShaderProgram::GetUniform(D3DXHANDLE handle, ID3DXConstantTable * ct)
	 {
		 UINT i =1;
		 D3DXCONSTANT_DESC desc;
		 ct->GetConstantDesc(handle, &desc ,&i);

		 if((desc.Class == D3DXPARAMETER_CLASS::D3DXPC_OBJECT) ||
			 (desc.Type != D3DXPARAMETER_TYPE::D3DXPT_VOID &&
			 desc.Type != D3DXPARAMETER_TYPE::D3DXPT_INT &&
			 desc.Type != D3DXPARAMETER_TYPE::D3DXPT_FLOAT &&
			 desc.Type != D3DXPARAMETER_TYPE::D3DXPT_BOOL))
			 return nullptr;
		 

		 UniformDesc^ ud = gcnew UniformDesc();
		 ud->Class = GetClass(desc.Class);
		 ud->Elements = desc.Elements;
		 ud->Index = desc.RegisterIndex;
		 ud->Register= static_cast<RegisterSet>(desc.RegisterSet);
		 ud->Name = Marshal::PtrToStringAnsi(static_cast<IntPtr>((void*)desc.Name));
		 ud->Type = GetPType(desc.Type);
		 ud->Columns = desc.Columns;
		 ud->Rows = desc.Rows;		
		 ud->Bytes = desc.Bytes;

		 if(desc.StructMembers > 0)
		 {
			 ud->Members = gcnew array<UniformDesc^>(desc.StructMembers);

			 for (int j = 0; j < desc.StructMembers; j++)
			 {
				 ud->Members[j] = GetUniform(ct->GetConstant(handle, j), ct);
			 }
		 }

		 return ud;
	 }

	 UniformDesc^ D3DShaderProgram::GetUniformDescription(int index)
	 {
			D3DXHANDLE handle;

			for each (auto var in shaders)
			{
				ID3DShader^ s = (ID3DShader^)(var->Function);
				handle = s->GetConstantTable()->GetConstant(NULL, index);
				if(handle != NULL)
					return GetUniform(handle, s->GetConstantTable());
			}		 
		return nullptr;
	 }

	 UniformDesc^ D3DShaderProgram::GetUniformDescription(String^ name)	 
	 {
		 D3DXHANDLE handle;
		 LPCSTR pName  = (LPCSTR)Marshal::StringToHGlobalAnsi(name).ToPointer();
		 UniformDesc^ desc;
			for each (auto var in shaders)
			{
				ID3DShader^ s = (ID3DShader^)(var->Function);
				handle = s->GetConstantTable()->GetConstantByName(NULL, pName);
				if(handle != NULL)
				{
					desc = GetUniform(handle, s->GetConstantTable());
					break;
				}
			}		 

			Marshal::FreeHGlobal(IntPtr((void*)pName));
			return desc;
	 }

	 array<UniformDesc^>^ D3DShaderProgram::GetUniformDescriptions()
	 {		 
		 List<UniformDesc^>^ uniforms = gcnew List<UniformDesc^>();		 
		 Dictionary<String^, UniformDesc^>^ names = gcnew Dictionary<String^, UniformDesc^>();

		 for each (auto var in shaders)
		{
			ID3DShader^ shader = (ID3DShader^)var->Function;
			ID3DXConstantTable* ct = shader->GetConstantTable();
			D3DXCONSTANTTABLE_DESC desc;
			ct->GetDesc(&desc);

			 for (int i = 0; i < desc.Constants; i++)
			 {
				 //auto u = GetUniformDescription(i);
				 auto handle = ct->GetConstant(NULL, i);
				 if(handle != NULL)
				{	
					 UniformDesc^ u = GetUniform(handle, ct);
					 if(u!=nullptr)
					 {
						 if(!names->ContainsKey(u->Name))
						 {
							 uniforms->Add(u); 
							 names->Add(u->Name, u);
						 }
						 else
						 {
							 auto other = names[u->Name];

							 if(u->Class    != other->Class  || 
								u->Type     != other->Type   ||
								u->Register != other->Register)
								throw gcnew InvalidOperationException(L"Invalid Variable");
						 }
					 }
				 }
			 }
		 }		

		 return uniforms->ToArray();
	 }

	 void D3DShaderProgram::SetShaders(LPDIRECT3DDEVICE9 device)
	 {		 
		 interior_ptr<ShaderHandler^> pShaders = &shaders[0];
		 int count = shaders->Length;
		 for (int i = 0; i < count; i++, pShaders++)
		 {
			 (*pShaders)->Set();
		 }
	 }

}}