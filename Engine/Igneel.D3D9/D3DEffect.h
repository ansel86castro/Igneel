#pragma once

using namespace System;

namespace Igneel {
	namespace D3D9 {

public ref class EffectUT
{
public:
	EffectUT(void);

	void SetMatrixArray(IntPtr comEffectPter, IntPtr hParameter , float* data, int count)
	{
		ID3DXEffect* pEffect = static_cast<ID3DXEffect*>(comEffectPter.ToPointer());
		D3DXHANDLE* pHandle = static_cast<D3DXHANDLE*>(hParameter.ToPointer());
		D3DXMATRIX* matrices = (D3DXMATRIX*)data;

		pEffect->SetMatrixArray(*pHandle, matrices , count);		
	}

	void SetRawValue(IntPtr comEffectPter, IntPtr hParameter , void* data, int bytesOffset ,int bytesCount)
	{
		ID3DXEffect* pEffect = static_cast<ID3DXEffect*>(comEffectPter.ToPointer());
		D3DXHANDLE* pHandle = static_cast<D3DXHANDLE*>(hParameter.ToPointer());

		pEffect->SetRawValue(*pHandle, data, bytesOffset, bytesCount);
	}

};

	}
}

