#pragma once

#include "IResouceContainer.h"

using namespace System;
using namespace Igneel::Graphics;

namespace IgneelD3D10
{
	public ref class Texture2D10: public Texture2DBase , IResourceContainer
	{
	internal:
		ID3D10Texture2D* _texture;
		ID3D10Texture2D* _stagingTexture;
		ID3D10ShaderResourceView* _shaderResource;
		ID3D10Device* _device;
		D3D10_MAP lastMap;
	internal:
		Texture2D10(ID3D10Device * device, ID3D10Texture2D* texture ,String^ filename);
		Texture2D10(ID3D10Device * device, Texture2DDesc desc, array<MappedTexture2D>^ data);

		void AddSize(ID3D10Device * device, D3D10_TEXTURE2D_DESC& td);

	protected:
		OVERRIDE(void OnDispose(bool));

	public:
		OVERRIDE( MappedTexture2D Map(int subResource, MapType map, bool doNotWait) );

		OVERRIDE( void UnMap(int subResource) );		

		virtual ID3D10Resource* GetResource(){ return _texture;  }
	};
	
}