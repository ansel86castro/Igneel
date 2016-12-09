#pragma once
#include "GraphicDevice.h"

using namespace System;
using namespace System::Collections::Generic;

namespace IgneelD3D10{

	ref class GraphicDevice10;

	public ref class CbHandle
	{		
	public:
		ID3D10Buffer* cb;
		void * buffer;	
		int size;
	
		CbHandle(ID3D10Buffer* cb);
		!CbHandle();	
	
		void* GetBuffer(GraphicDevice10^ device);

		void Close();
	};

	public value struct CBufferShaderBinding
	{			
		CbHandle^ handle;
		int bindPoint;
		int index;		

		CBufferShaderBinding(CbHandle^ handle, int bindPoint, int index);		
	};

	public value struct CBufferVarBinding
	{	
		CbHandle^ handle;
		int offset;
		GraphicDevice10^ device;
		
		CBufferVarBinding(CbHandle^ handle, int offset, GraphicDevice10^ device);

		void* GetBuffer();		
	};

	public value struct  CResourceBinding
	{
		RegisterSet Register;
		int BindPoint;
		int BindPointSampler;
		int BindCount;
		IShaderStage^ Stage;
	};


	public ref class ConstantBufferCache
	{	
	public:
		static CbHandle^ GetBuffer(ID3D10Device* device, int buffSize);
	internal:
		static Dictionary<int , WeakReference<CbHandle^>^>^ Cache = gcnew Dictionary<int , WeakReference<CbHandle^>^>();
	};
}