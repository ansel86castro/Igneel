#include "ConstantBuffer.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;
using namespace System::Collections::Generic;

namespace IgneelD3D10{

	public ref class D3DUniformSetter:public ResourceAllocator, IUniformSetter
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

	protected:
		OVERRIDE(void OnDispose(bool));
    };

}