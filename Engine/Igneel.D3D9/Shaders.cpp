#include "Stdafx.h"
#include "Shaders.h"

namespace Igneel { namespace D3D9 {

	/*void ConstantTableBinder::OnDispose(bool disposing)
	{
		if(_handles)
		{
			delete[] _handles;			
			_handles = NULL;
		}

		if(_ct)
		{
			_ct->Release();
			_ct = NULL;
		}
	}

	void ConstantTableBinder::BeginDefinition(int numConstants)
	{
		_handles = new D3DXHANDLE[numConstants];
		_nbHandles = numConstants;
	}

	void ConstantTableBinder::DefineConstant(String^ name, int index)
	{
		LPSTR str = (LPSTR)Marshal::StringToHGlobalAnsi(name).ToPointer();		

		_handles[index] = _ct->GetConstantByName(NULL, str);			

		Marshal::FreeHGlobal(IntPtr(str));
	}

	bool ConstantTableBinder::IsConstantValid(String^ name)
	{
		LPSTR str = (LPSTR)Marshal::StringToHGlobalAnsi(name).ToPointer();		

		auto handle = _ct->GetConstantByName(NULL, str);

		Marshal::FreeHGlobal(IntPtr(str));
		
		return handle != NULL;
	}
	
	void ConstantTableBinder::SetConstant(int index, void* value , int size)
	{		
		_ct->SetValue(_device, _handles[index], value, size);		
	}

	void ConstantTableBinder::SetConstantI(int index, int value)
	{
		_ct->SetInt(_device, _handles[index], value);
	}

	void ConstantTableBinder::SetConstantB(int index, bool value)
	{
		_ct->SetBool(_device, _handles[index], value);
	}

	void ConstantTableBinder::SetConstantIArray(int index, int* value, int count)
	{
		_ct->SetIntArray(_device, _handles[index], value, count);
	}

	void ConstantTableBinder::SetConstantBArray(int index, bool* value, int count)
	{
		_ct->SetBoolArray(_device, _handles[index], (BOOL*) value, count);
	}*/


	D3DVertexShader::D3DVertexShader(IDirect3DDevice9 * device, IDirect3DVertexShader9* vs, ID3DXConstantTable* constantTable)
	{
		_pvs = vs;
		this->constantTable = constantTable;

		GC::AddMemoryPressure(sizeof(IDirect3DVertexShader9));
		GC::AddMemoryPressure(sizeof(ID3DXConstantTable));
		GC::AddMemoryPressure(constantTable->GetBufferSize());
	}

	 ID3DXConstantTable* D3DVertexShader::GetConstantTable()
	{
		return constantTable;
	}

	void D3DVertexShader::OnDispose(bool disposing)
	{
		__super::OnDispose(disposing);

		if(_pvs)
		{			
			_pvs->Release();
			constantTable->Release();
		}				
	}

	D3DPixelShader::D3DPixelShader(IDirect3DDevice9 * device, IDirect3DPixelShader9* ps, ID3DXConstantTable* constantTable)
	{
		_pps = ps;
		this->constantTable = constantTable;
		GC::AddMemoryPressure(sizeof(IDirect3DPixelShader9));
		GC::AddMemoryPressure(sizeof(ID3DXConstantTable));
		GC::AddMemoryPressure(constantTable->GetBufferSize());
	}

	 ID3DXConstantTable* D3DPixelShader::GetConstantTable()
	{
		return constantTable;
	}

	void D3DPixelShader::OnDispose(bool disposing)
	{
		__super::OnDispose(disposing);

		if(_pps)
		{			
			_pps->Release();
			constantTable->Release();
		}		
	}

	D3DShaderByteCode::D3DShaderByteCode(LPD3DXBUFFER buffer , ID3DXConstantTable * ct)
		:DataBuffer(IntPtr(buffer->GetBufferPointer()), buffer->GetBufferSize())
	{
		_ct = ct;
		GC::AddMemoryPressure(buffer->GetBufferSize());
	}
}}