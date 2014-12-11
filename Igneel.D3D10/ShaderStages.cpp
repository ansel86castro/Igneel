#include "Stdafx.h"
#include "ShaderStages.h"
#include "SamplerState10.h"
#include "IResouceContainer.h"
#include "GraphicBuffer10.h"
#include "Texture1D10.h"
#include "Texture2D10.h"
#include "Texture3D10.h"

namespace IgneelD3D10
{
	//**** Shader Stage 10

	ShaderStage10::ShaderStage10(ID3D10Device* device)
	{
		_device = device;
		_holder.Init();
		//_device->VSSetShaderResources(0, 128, _holder._pResouces);
	}

	void ShaderStage10::OnSetSampler(int slot, SamplerState^ state)
	{
		SamplerState10^ stateImp = static_cast<SamplerState10^>(state);
		ID3D10SamplerState* buff[1] = { stateImp->_sampler };
		SetSamplers(slot, 1, buff);		
	}

	void ShaderStage10::OnSetSamplers(int slot, array<SamplerState^,1>^ states)
	{
		array<SamplerState10^,1>^ statesImp = static_cast<array<SamplerState10^,1>^>(states);	
		for (int i = 0; i < statesImp->Length; i++)
		{
			_holder._pSamplers[i] = statesImp[i]->_sampler;
		}
		SetSamplers(slot,  states->Length, _holder._pSamplers);			
	}

	void ShaderStage10::OnSetResource(int index , ShaderResource^ resource)
	{
		ID3D10ShaderResourceView* views[1] = { NULL };
		if(resource != nullptr)		
		{			
			switch (resource->Type)
			{
				case ResourceType::Buffer:
					{
						GraphicBuffer10^ buffer = static_cast<GraphicBuffer10^>( resource);
						if(!buffer->_srv)
							throw gcnew InvalidOperationException("Invalid Buffer");
						views[0] = buffer->_srv;
					}
				break;
				case ResourceType::Texture1D:		
					views[0] = static_cast<Texture1D10^>(resource)->_shaderResource;				
				break;
				case ResourceType::Texture2D:		
					views[0] = static_cast<Texture2D10^>(resource)->_shaderResource;				
				break;
				case ResourceType::Texture3D:		
					views[0] = static_cast<Texture3D10^>(resource)->_shaderResource;
				break;
			}			
		}
		SetResources(index, 1, views);		
	}

	void ShaderStage10::OnSetResources(int index, int nbResources, array<ShaderResource^,1>^ resources)
	{		
		if(resources!=nullptr)
		{
			for (int i = 0; i < nbResources; i++)
			{
				IGraphicResource^ resource = resources[i];									
				if(resource==nullptr)
					_holder._pResouces[i] = NULL;
				else
				{
					switch (resource->Type)
					{
						case ResourceType::Buffer:
						{
							GraphicBuffer10^ buffer = static_cast<GraphicBuffer10^>( resource);
							if(!buffer->_srv)
								throw gcnew InvalidOperationException("Invalid Buffer");
							_holder._pResouces[i]= buffer->_srv;
						}
						break;
						case ResourceType::Texture1D:		
							_holder._pResouces[i] = static_cast<Texture1D10^>(resource)->_shaderResource;					
						break;
						case ResourceType::Texture2D:		
							_holder._pResouces[i] = static_cast<Texture2D10^>(resource)->_shaderResource;						
						break;
						case ResourceType::Texture3D:		
							_holder._pResouces[i] = static_cast<Texture3D10^>(resource)->_shaderResource;						
						break;
					}
				}
			}
		}
		else
		{
			ZeroMemory(_holder._pResouces, nbResources * sizeof(ID3D10ShaderResourceView*));
		}
		SetResources(index, nbResources, _holder._pResouces);
	
	}

	ShaderStage10::!ShaderStage10()
	{
		_holder.Destroy();
	}


	//********* Vertex Shader Stage*************************

	void VSStage10::SetSamplers(int slot, int num , ID3D10SamplerState** samplers)
	{
		_device->VSSetSamplers(slot, num, samplers);
	}

	void VSStage10::SetResources(int index, int num, ID3D10ShaderResourceView** resources)
	{
		_device->VSSetShaderResources(index, num, resources);
	}

	//********** PIXEL SHADER STAGE *****************************
	
	void PSStage10::SetSamplers(int slot, int num , ID3D10SamplerState** samplers)
	{
		_device->PSSetSamplers(slot, num, samplers);
	}

	void PSStage10::SetResources(int index, int num, ID3D10ShaderResourceView** resources)
	{
		_device->PSSetShaderResources(index, num, resources);
	}

	//************** GEOMETRY SHADER STAGE ***********************

	void GSStage10::SetSamplers(int slot, int num , ID3D10SamplerState** samplers)
	{
		_device->GSSetSamplers(slot, num, samplers);
	}

	void GSStage10::SetResources(int index, int num, ID3D10ShaderResourceView** resources)
	{
		_device->GSSetShaderResources(index, num, resources);
	}
}