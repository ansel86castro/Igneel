#include "Stdafx.h"
#include "D3DUniformSetter.h"
#include "GraphicDevice.h"

namespace IgneelD3D10 {

	D3DUniformSetter::D3DUniformSetter(array<CBufferVarBinding>^ bindings)		
	{
		this->cb = bindings;
		lenght = cb->Length;
	}

	  void D3DUniformSetter::SetValue(void* value, int size)
	  {		  
		  for (int i = 0; i <lenght; i++)
		  {
			  void* data = cb[i].GetBuffer();			 
			  memcpy(data, value, size);
		  }
		  
	  }

      void D3DUniformSetter::SetInt(int value)
	  {
		  for (int i = 0; i <lenght; i++)
		  {
			 void* data = cb[i].GetBuffer();			 
			  memcpy(data, &value, sizeof(int));
		  }
	  }

      void D3DUniformSetter::SetIntArray(int* value, int count)
	  {
		  for (int i = 0; i <lenght; i++)
		  {
			 void* data = cb[i].GetBuffer();			 
			  memcpy(data, value, sizeof(int) * count);
		  }
	  }

      void D3DUniformSetter::SetBool(bool value)
	  {
		  int v = value;
		  for (int i = 0; i <lenght; i++)
		  {
			 void* data = cb[i].GetBuffer();			 
			  memcpy(data, &v, sizeof(int));
		  }
		  
	  }

      void D3DUniformSetter::SetBoolArray(bool* value, int count)
	  {
		  
		  for (int i = 0; i <lenght; i++)
		  {
			 byte* base = (byte*)cb[i].GetBuffer();			 			
			 for (int j = 0; j < count; j++)
			 {
				 int v = value[j];
				 *(base + j)= v;
			 }
			 			 
		  }
	  }

	  void D3DUniformSetter::SetFloat(float value)
	  {
		 for (int i = 0; i <lenght; i++)
		  {
			  void* data = cb[i].GetBuffer();			 
			  memcpy(data, &value, sizeof(float));
		  }
	  }

      void D3DUniformSetter::SetMatrix(Matrix value)
	  {
		   for (int i = 0; i <lenght; i++)
		  {
			  void* data = cb[i].GetBuffer();			 
			  memcpy(data, &value, sizeof(Matrix));
		  }
	  }

       void D3DUniformSetter::SetVector(Vector4 value)
	   {
		   for (int i = 0; i <lenght; i++)
		  {
			  void* data = cb[i].GetBuffer();			 
			  memcpy(data, &value, sizeof(Vector4));
		  }
	   }

		void D3DUniformSetter::SetFloatArray(float* value, int count)
		{
			  for (int i = 0; i <lenght; i++)
			  {
				  void* data = cb[i].GetBuffer();			 
				  memcpy(data, value, sizeof(float) * count);
			  }
		}

       void D3DUniformSetter::SetMatrixArray(Matrix* value, int count)
	   {
			for (int i = 0; i <lenght; i++)
			{
				void* data = cb[i].GetBuffer();			 
				memcpy(data, value, sizeof(Matrix) * count);
			}
	   }

       void D3DUniformSetter::SetVectorArray(Vector4* value, int count)
	   {
		 for (int i = 0; i <lenght; i++)
			{
				void* data = cb[i].GetBuffer();			 
				memcpy(data, value, sizeof(Vector4) * count);
			}
	   }

	   void D3DUniformSetter::OnDispose(bool disposing)
	   {
		  
	   }
}