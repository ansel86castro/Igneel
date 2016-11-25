#pragma once
#include "EnumConverter.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;
using namespace System::Collections::Generic;

namespace Igneel { namespace D3D9 {

	/*public ref class ConstantTableBinder: public ResourceAllocator, IConstantBinder
	{
		ID3DXConstantTable * _ct;
		IDirect3DDevice9 * _device;
		D3DXHANDLE* _handles;
		int _nbHandles;

	public:
		ConstantTableBinder(ID3DXConstantTable * constantTable ,IDirect3DDevice9 * device)
			:_ct(constantTable), _device(device), _handles(nullptr), _nbHandles(0) { }		

	protected:
		OVERRIDE(void OnDispose(bool));
	public:
		
		virtual void SetConstant(int index, void* value , int size);

		virtual void SetConstantI(int index, int value);

		virtual void SetConstantIArray(int index, int* value, int count);

		virtual void SetConstantB(int index, bool value);

		virtual void SetConstantBArray(int index, bool* value, int count);

		virtual void BeginDefinition(int numConstants);

		virtual void EndDefinitions(){ }

		virtual void DefineConstant(String^ name, int index);

		virtual bool IsConstantValid(String^ name);
	};*/

	public interface class ID3DShader
	{
		ID3DXConstantTable* GetConstantTable();
	};

	public ref class D3DVertexShader: public VertexShader, ID3DShader
	{
	internal:
		IDirect3DVertexShader9* _pvs;	
		ID3DXConstantTable* constantTable;

	internal:
		D3DVertexShader(IDirect3DDevice9 * device, IDirect3DVertexShader9* vs, ID3DXConstantTable* constantTable);

	protected:
		OVERRIDE(void OnDispose(bool));

	public:
		virtual ID3DXConstantTable* GetConstantTable();
	};

	public ref class D3DPixelShader: public PixelShader, ID3DShader
	{
	internal:
		IDirect3DPixelShader9* _pps;		
		ID3DXConstantTable* constantTable;

	internal:
		D3DPixelShader(IDirect3DDevice9 * device, IDirect3DPixelShader9* vs, ID3DXConstantTable* constantTable);

	protected:
		OVERRIDE(void OnDispose(bool));

		public:
		virtual ID3DXConstantTable* GetConstantTable();
	};

	public ref class D3DShaderByteCode: public DataBuffer
	{
	internal:		
		ID3DXConstantTable * _ct;
		LPD3DXBUFFER _buffer;

		!D3DShaderByteCode()
		{
			if(_buffer)
			{
				_buffer->Release();		
				_buffer = NULL;
			}
		}
		~D3DShaderByteCode()
		{
			this->!D3DShaderByteCode();
		}

	public:
		D3DShaderByteCode(LPD3DXBUFFER buffer , ID3DXConstantTable * ct);

	};
}}