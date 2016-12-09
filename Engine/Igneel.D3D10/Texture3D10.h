#pragma once
#include "IResouceContainer.h"

using namespace System;
using namespace Igneel::Graphics;

namespace IgneelD3D10
{
ref class GraphicDevice10;

	public ref class Texture3D10: public Texture3D, IResourceContainer
	{
	internal:
		ID3D10Texture3D* _texture;
		ID3D10Texture3D* _stagingTexture;
		ID3D10ShaderResourceView* _shaderResource;
		ID3D10Device* _device;
		D3D10_MAP lastMap;
	internal:
		Texture3D10(GraphicDevice10^ graphicDevice, ID3D10Texture3D* texture);
		Texture3D10(GraphicDevice10^ graphicDevice, Texture3DDesc desc, array<MappedTexture3D>^ data);

		void AddSize(D3D10_TEXTURE3D_DESC& td);

	protected:
		OVERRIDE(void OnDispose(bool));

	public:
		OVERRIDE( MappedTexture3D Map(int subResource, MapType map, bool doNotWait) );

		OVERRIDE( void UnMap(int subResource) );		

		virtual ID3D10Resource* GetResource(){ return _texture; }
	};
	
}