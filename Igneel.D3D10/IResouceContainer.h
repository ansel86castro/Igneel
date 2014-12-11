#pragma once
using namespace Igneel::Graphics;

namespace IgneelD3D10
{
	public interface class IResourceContainer
	{
		ID3D10Resource* GetResource();
	};

	ID3D10Resource* GetResource(IGraphicResource^ texture);	
}