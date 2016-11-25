#include "ConstantBuffer.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel;
using namespace Igneel::Graphics;
using namespace System::Collections::Generic;

namespace IgneelD3D10{

	public ref  class D3DUniformSetter sealed: IUniformSetter 
    {	
		array<CBufferVarBinding>^ cb;		
		int lenght;
	public:
		D3DUniformSetter(array<CBufferVarBinding>^ bindings);

        virtual void SetValue(void* value, int size);

        virtual void SetInt(int value);

        virtual void SetIntArray(int* value, int count);

        virtual void SetBool(bool value);

        virtual void SetBoolArray(bool* value, int count);

		virtual void SetFloat(float value);

        virtual void SetMatrix(Matrix value);

        virtual void SetVector(Vector4 value);

		virtual void SetFloatArray(float* value, int count);

        virtual void SetMatrixArray(Matrix* value, int count);

        virtual void SetVectorArray(Vector4* value, int count);


		virtual void SetResource(IShaderResource^ resource)
		{
			  throw gcnew NotImplementedException();
		}

        virtual void SetSampler(SamplerState^ sampler)
		{
			throw gcnew NotImplementedException();
		}

		 virtual void SetResource(array<IShaderResource^>^ resources , int numResources)
		 {
			throw gcnew NotImplementedException();
		 }

		 virtual void SetSampler(array<SamplerState^>^ samplers, int numSamplers)
		 {
			throw gcnew NotImplementedException();
		 }	
    };

	public ref class D3DResourceSetter sealed: public IUniformSetter
	{
		array<CResourceBinding>^ rb;		
		int lenght;
	public:
		D3DResourceSetter(array<CResourceBinding>^ bindings)
		{
			this->rb = bindings;
			lenght = rb->Length;
		}

		virtual void SetValue(void* value, int size){  throw gcnew NotImplementedException();}

        virtual void SetInt(int value){  throw gcnew NotImplementedException();}

        virtual void SetIntArray(int* value, int count){  throw gcnew NotImplementedException();}

        virtual void SetBool(bool value){  throw gcnew NotImplementedException();}

        virtual void SetBoolArray(bool* value, int count){  throw gcnew NotImplementedException();}

		virtual void SetFloat(float value){  throw gcnew NotImplementedException();}

        virtual void SetMatrix(Matrix value){  throw gcnew NotImplementedException();}

        virtual void SetVector(Vector4 value){  throw gcnew NotImplementedException();}

		virtual void SetFloatArray(float* value, int count){  throw gcnew NotImplementedException();}

        virtual void SetMatrixArray(Matrix* value, int count){  throw gcnew NotImplementedException();}

        virtual void SetVectorArray(Vector4* value, int count){  throw gcnew NotImplementedException();}

		virtual void SetResource(IShaderResource^ resource)
		{
			  for (int i = 0; i <lenght; i++)
			  {
				  CResourceBinding  b = rb[i];
				  b.Stage->SetResource(b.BindPoint, resource);
			  }
		}

        virtual void SetSampler(SamplerState^ sampler)
		{
			for (int i = 0; i <lenght; i++)
			{
				CResourceBinding  b = rb[i];
				b.Stage->SetSampler(b.BindPointSampler, sampler);
			}
		}

		 virtual void SetResource(array<IShaderResource^>^ resources , int numResources)
		 {
			 for (int i = 0; i <lenght; i++)
			{
				CResourceBinding  b = rb[i];
				b.Stage->SetResources(b.BindPoint, numResources ,resources);
			}
		 }

		 virtual void SetSampler(array<SamplerState^>^ samplers, int numSamplers)
		 {
			 for (int i = 0; i <lenght; i++)
			{
				CResourceBinding  b = rb[i];
				b.Stage->SetSamplers(b.BindPointSampler, numSamplers ,samplers);
			}
		 }
	};
}