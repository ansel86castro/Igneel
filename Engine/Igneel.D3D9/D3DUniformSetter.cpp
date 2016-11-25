#include "Stdafx.h"
#include "D3DUniformSetter.h"
#include "GraphicDevice.h"

namespace Igneel { namespace D3D9 {

	D3DUniformSetter::D3DUniformSetter(array<ID3DXConstantTable*>^ ct, array<D3DXHANDLE>^ handle)		
	{
		this->_ct = ct;
		this->_handle = handle;
		device = static_cast<D3D9GraphicDevice^>(Engine::Graphics)->_pDevice;
	}

	  void D3DUniformSetter::SetValue(void* value, int size)
	  {
		  for (int i = 0; i < _ct->Length; i++)
		  {
			  _ct[i]->SetValue(device, _handle[i], value, size);
		  }
		  
	  }

      void D3DUniformSetter::SetInt(int value)
	  {
		  for (int i = 0; i < _ct->Length; i++)
		  {
			  _ct[i]->SetInt(device, _handle[i], value);
		  }
		   
	  }

      void D3DUniformSetter::SetIntArray(int* value, int count)
	  {
		  for (int i = 0; i < _ct->Length; i++)
		  {
			  _ct[i]->SetIntArray(device, _handle[i], value, count);
		  }		   
	  }

      void D3DUniformSetter::SetBool(bool value)
	  {
		  for (int i = 0; i < _ct->Length; i++)
		  {
			  _ct[i]->SetBool(device, _handle[i], value);
		  }
		  
	  }

      void D3DUniformSetter::SetBoolArray(bool* value, int count)
	  {
		  for (int i = 0; i < _ct->Length; i++)
		  {
			  _ct[i]->SetBoolArray(device, _handle[i], (BOOL*)value, count);
		  }		  
	  }

	  void D3DUniformSetter::SetFloat(float value)
	  {
		   for (int i = 0; i < _ct->Length; i++)
		  {
			  _ct[i]->SetFloat(device, _handle[i], value);
		  }
	  }

      void D3DUniformSetter::SetMatrix(Matrix value)
	  {
		   for (int i = 0; i < _ct->Length; i++)
		  {
			  _ct[i]->SetMatrix(device, _handle[i], (D3DXMATRIX*)&value);
		  }
	  }

       void D3DUniformSetter::SetVector(Vector4 value)
	   {
		    for (int i = 0; i < _ct->Length; i++)
		  {
			  _ct[i]->SetVector(device, _handle[i],(D3DXVECTOR4*) &value);
		  }
	   }

		void D3DUniformSetter::SetFloatArray(float* value, int count)
		{
			 for (int i = 0; i < _ct->Length; i++)
		  {
			  _ct[i]->SetFloatArray(device, _handle[i], value, count);
		  }		
		}

       void D3DUniformSetter::SetMatrixArray(Matrix* value, int count)
	   {
		    for (int i = 0; i < _ct->Length; i++)
		  {
			  _ct[i]->SetMatrixArray(device, _handle[i], (D3DXMATRIX*)value, count);
		  }		
	   }

       void D3DUniformSetter::SetVectorArray(Vector4* value, int count)
	   {
		   for (int i = 0; i < _ct->Length; i++)
		  {
			  _ct[i]->SetVectorArray(device, _handle[i], (D3DXVECTOR4*)value, count);
		  }		
	   }
}}