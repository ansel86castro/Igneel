#pragma once

using namespace Igneel::Graphics;


namespace IgneelD3D10
{
	public ref class SamplerState10 : public SamplerState
	{	
	internal:
		ID3D10SamplerState* _sampler;

	internal:
		SamplerState10(ID3D10Device* device, SamplerDesc desc);
	
	protected:
		OVERRIDE(void OnDispose(bool));
	};

}