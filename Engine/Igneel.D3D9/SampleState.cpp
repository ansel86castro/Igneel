#include "stdafx.h"
#include "SampleState.h"
#include "EnumConverter.h"

namespace Igneel {
	namespace D3D9 {	
		
		D3D9SampleState::D3D9SampleState(SamplerDesc desc):SamplerState(desc)
		{

		}

		void D3D9SampleState::Apply(int reg, IDirect3DDevice9* device)
		{
			DWORD s = reg;		

			switch (_state.Filter)
			{
			case Igneel::Graphics::Filter::Anisotropic:
				device->SetSamplerState(s, D3DSAMP_MAGFILTER , D3DTEXF_ANISOTROPIC);
				device->SetSamplerState(s, D3DSAMP_MINFILTER , D3DTEXF_ANISOTROPIC);
				device->SetSamplerState(s, D3DSAMP_MIPFILTER , D3DTEXF_NONE);
				break;
			case Igneel::Graphics::Filter::ComparisonAnisotropic:
				device->SetSamplerState(s, D3DSAMP_MAGFILTER , D3DTEXF_ANISOTROPIC);
				device->SetSamplerState(s, D3DSAMP_MINFILTER , D3DTEXF_ANISOTROPIC);
				device->SetSamplerState(s, D3DSAMP_MIPFILTER , D3DTEXF_NONE);
				break;

			case Igneel::Graphics::Filter::          MinLinearMagMipPoint:
			case Igneel::Graphics::Filter::ComparisonMinLinearMagMipPoint:
				device->SetSamplerState(s, D3DSAMP_MAGFILTER , D3DTEXF_POINT);
				device->SetSamplerState(s, D3DSAMP_MINFILTER , D3DTEXF_LINEAR);
				device->SetSamplerState(s, D3DSAMP_MIPFILTER , D3DTEXF_POINT);
				break;

			case Igneel::Graphics::Filter::          MinLinearMagPointMipLinear:
			case Igneel::Graphics::Filter::ComparisonMinLinearMagPointMipLinear:
				device->SetSamplerState(s, D3DSAMP_MAGFILTER , D3DTEXF_POINT);
				device->SetSamplerState(s, D3DSAMP_MINFILTER , D3DTEXF_LINEAR);
				device->SetSamplerState(s, D3DSAMP_MIPFILTER , D3DTEXF_LINEAR);
				break;

			case Igneel::Graphics::Filter::          MinMagLinearMipPoint:
			case Igneel::Graphics::Filter::ComparisonMinMagLinearMipPoint:
				device->SetSamplerState(s, D3DSAMP_MAGFILTER , D3DTEXF_LINEAR);
				device->SetSamplerState(s, D3DSAMP_MINFILTER , D3DTEXF_LINEAR);
				device->SetSamplerState(s, D3DSAMP_MIPFILTER , D3DTEXF_POINT);
				break;

			case Igneel::Graphics::Filter::          MinMagMipLinear:
			case Igneel::Graphics::Filter::ComparisonMinMagMipLinear:
				device->SetSamplerState(s, D3DSAMP_MAGFILTER , D3DTEXF_LINEAR);
				device->SetSamplerState(s, D3DSAMP_MINFILTER , D3DTEXF_LINEAR);
				device->SetSamplerState(s, D3DSAMP_MIPFILTER , D3DTEXF_LINEAR);
				break;

			case Igneel::Graphics::Filter::          MinMagMipPoint:
			case Igneel::Graphics::Filter::ComparisonMinMagMipPoint:
				device->SetSamplerState(s, D3DSAMP_MAGFILTER , D3DTEXF_POINT);
				device->SetSamplerState(s, D3DSAMP_MINFILTER , D3DTEXF_POINT);
				device->SetSamplerState(s, D3DSAMP_MIPFILTER , D3DTEXF_POINT);
				break;

		    case Igneel::Graphics::Filter::          MinMagPointMipLinear:
			case Igneel::Graphics::Filter::ComparisonMinMagPointMipLinear:
				device->SetSamplerState(s, D3DSAMP_MAGFILTER , D3DTEXF_POINT);
				device->SetSamplerState(s, D3DSAMP_MINFILTER , D3DTEXF_POINT);
				device->SetSamplerState(s, D3DSAMP_MIPFILTER , D3DTEXF_LINEAR);
				break;

			case Igneel::Graphics::Filter::          MinPointMagLinearMipPoint:
			case Igneel::Graphics::Filter::ComparisonMinPointMagLinearMipPoint:
				device->SetSamplerState(s, D3DSAMP_MAGFILTER , D3DTEXF_LINEAR);
				device->SetSamplerState(s, D3DSAMP_MINFILTER , D3DTEXF_POINT);
				device->SetSamplerState(s, D3DSAMP_MIPFILTER , D3DTEXF_POINT);
				break;

			case Igneel::Graphics::Filter::          MinPointMagMipLinear:
			case Igneel::Graphics::Filter::ComparisonMinPointMagMipLinear:
				device->SetSamplerState(s, D3DSAMP_MAGFILTER , D3DTEXF_POINT);
				device->SetSamplerState(s, D3DSAMP_MINFILTER , D3DTEXF_LINEAR);
				device->SetSamplerState(s, D3DSAMP_MIPFILTER , D3DTEXF_LINEAR);
				break;	
			default:
				throw gcnew FilterNotSupportedException(_state.Filter);
			}			

			device->SetSamplerState(s, D3DSAMPLERSTATETYPE::D3DSAMP_ADDRESSU ,static_cast<DWORD>(_state.AddressU));
			device->SetSamplerState(s, D3DSAMPLERSTATETYPE::D3DSAMP_ADDRESSV ,static_cast<DWORD>(_state.AddressV));
			device->SetSamplerState(s, D3DSAMPLERSTATETYPE::D3DSAMP_ADDRESSW ,static_cast<DWORD>(_state.AddressW));
			device->SetSamplerState(s, D3DSAMPLERSTATETYPE::D3DSAMP_BORDERCOLOR ,static_cast<DWORD>(_state.BorderColor.ToArgb()));
			device->SetSamplerState(s, D3DSAMPLERSTATETYPE::D3DSAMP_MAXANISOTROPY  , _state.MaxAnisotropy);
			device->SetSamplerState(s, D3DSAMPLERSTATETYPE::D3DSAMP_MAXMIPLEVEL  , _state.MaxLOD);
			device->SetSamplerState(s, D3DSAMPLERSTATETYPE::D3DSAMP_MIPMAPLODBIAS  , _state.MipLODBias);
		}
		
	}
}