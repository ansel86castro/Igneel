#include "Stdafx.h"
#include "SamplerState10.h"

namespace IgneelD3D10
{
	SamplerState10::SamplerState10(ID3D10Device* device, SamplerDesc desc)
		:SamplerState(desc)
	{
		D3D10_SAMPLER_DESC sd;
		ZeroMemory(&sd, sizeof(D3D10_SAMPLER_DESC));

		sd.AddressU = static_cast<D3D10_TEXTURE_ADDRESS_MODE>(desc.AddressU);
		sd.AddressV = static_cast<D3D10_TEXTURE_ADDRESS_MODE>(desc.AddressV);
		sd.AddressW = static_cast<D3D10_TEXTURE_ADDRESS_MODE>(desc.AddressW);
		*(Color4*)&(sd.BorderColor) = desc.BorderColor;
		sd.ComparisonFunc = (D3D10_COMPARISON_FUNC)desc.ComparisonFunc;
		sd.Filter = static_cast<D3D10_FILTER>( desc.Filter );
		sd.MaxAnisotropy = desc.MaxAnisotropy;
		sd.MaxLOD = desc.MaxLOD;
		sd.MinLOD = desc.MinLOD;
		sd.MipLODBias = desc.MipLODBias;

		ID3D10SamplerState *samplerState;
		device->CreateSamplerState(&sd, &samplerState);
		_sampler = samplerState;
	}

	void SamplerState10::OnDispose(bool dispose)
	{
		if(_sampler)
		{
			_sampler->Release();
			_sampler = nullptr;
		}
	}
}