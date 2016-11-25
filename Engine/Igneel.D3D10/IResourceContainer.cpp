#include "Stdafx.h"
#include "IResouceContainer.h"
#include "Texture1D10.h"
#include "Texture2D10.h"
#include "Texture3D10.h"
#include "GraphicBuffer10.h"

namespace IgneelD3D10
{
	ID3D10Resource* GetResource(IGraphicResource^ texture)
	{
		switch (texture->Type)
		{
			case ResourceType::Texture1D: return static_cast<Texture1D10^>(texture)->_texture;
			case ResourceType::Texture2D: return static_cast<Texture2D10^>(texture)->_texture;
			case ResourceType::Texture3D: return static_cast<Texture3D10^>(texture)->_texture;		
			case ResourceType::Buffer: return static_cast<GraphicBuffer10^>(texture)->_buffer;
		}
		return nullptr;
	}
}