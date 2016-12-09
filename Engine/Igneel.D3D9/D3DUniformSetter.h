
using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;
using namespace System::Collections::Generic;

namespace Igneel { namespace D3D9 {

	public ref class D3DUniformSetter:public IUniformSetter
    {
		IDirect3DDevice9* device;
		array<ID3DXConstantTable*>^ _ct;
		array<D3DXHANDLE>^ _handle;		
	public:
		D3DUniformSetter(array<ID3DXConstantTable*>^ ct, array<D3DXHANDLE>^ _handle);

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
    };

}}