#include "Stdafx.h"
#include "GraphicDevice.h"
#include "ShaderSetter.h"
#include "SampleState.h"
#include "D3D9Textures.h"
#include "D3DShaderProgram.h"

namespace Igneel { namespace D3D9 {

	void D3D9GraphicDevice::InitializeShaders()
	{			
		D3DShaderProgram::RegisterSetters(_pDevice);		

		for (int i = 0; i < _psSamplers->Length; i++)
		{
			_psSamplers[i] = gcnew D3D9SampleState(SamplerDesc::GetDefaults());
		}

		for (int i = 0; i < _vsSamplers->Length; i++)
		{
			_vsSamplers[i] = gcnew D3D9SampleState(SamplerDesc::GetDefaults());
		}
	}

	void D3D9GraphicDevice::VSSetSamplerImpl(int slot, SamplerState^ state)
	{
		D3D9SampleState^ sampler = static_cast<D3D9SampleState^>(state);
		sampler->Apply(slot + D3DVERTEXTEXTURESAMPLER0, _pDevice);
	}

	void D3D9GraphicDevice::PSSetSamplerImpl(int slot, SamplerState^ state)
	{
		D3D9SampleState^ sampler = static_cast<D3D9SampleState^>(state);
		sampler->Apply(slot, _pDevice);
	}

	void D3D9GraphicDevice::SetTextureImp(int index, Texture^ texture)
	{
		if(texture == nullptr)
		{
			SAFECALL(_pDevice->SetTexture(index, NULL));
			return;
		}
		D3D9Texture1D^ tex1D;
		D3D9Texture2D^ tex2D;
		D3D9Texture3D^ tex3D;
		D3D9TextureCube^ texCube;

		switch (texture->Type)
		{
		case ResourceType::Texture1D:
			{
				tex1D = static_cast<D3D9Texture1D^>(texture);
				SAFECALL(_pDevice->SetTexture(index, tex1D->_pTexture));
				break;
			}
		case ResourceType::Texture2D:
			{
				tex2D = static_cast<D3D9Texture2D^>(texture);
				SAFECALL(_pDevice->SetTexture(index, tex2D->_pTexture));
				break;
			}
		case ResourceType::Texture3D:
			{
				tex3D = static_cast<D3D9Texture3D^>(texture);
				SAFECALL(_pDevice->SetTexture(index, tex3D->_pTexture));
				break;
			}
		case ResourceType::TextureCube:
			{
				texCube = static_cast<D3D9TextureCube^>(texture);
				SAFECALL(_pDevice->SetTexture(index, texCube->_pTexture));
				break;		
			}
		}
	}

	ShaderProgram^ D3D9GraphicDevice::CreateProgram(ShaderProgramDesc^ desc)
	{
		return gcnew D3DShaderProgram(desc);
	}

	void D3D9GraphicDevice::UseProgram(ShaderProgram^ program)
	{		
		if(program != nullptr)
		{
			D3DShaderProgram^ p = static_cast<D3DShaderProgram^>(program);		
			p->SetShaders(_pDevice);
		}
	}

	void D3D9GraphicDevice::ClearProgram(ShaderProgram^ program)
	{
		SAFECALL( _pDevice->SetVertexShader(NULL));
		SAFECALL( _pDevice->SetPixelShader(NULL));
	}
}}