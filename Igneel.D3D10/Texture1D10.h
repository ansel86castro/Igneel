#pragma once
#include "IResouceContainer.h"

using namespace System;
using namespace Igneel::Graphics;

namespace IgneelD3D10
{
	public ref class Texture1D10: public Texture1DBase, IResourceContainer
	{
	internal:
		ID3D10Device * device;
		ID3D10Texture1D* _texture;		
		ID3D10Texture1D* _stagingTexture;	
		ID3D10ShaderResourceView* _shaderResource;
		D3D10_MAP lastMap;
	internal:
		Texture1D10(ID3D10Device * device, ID3D10Texture1D* texture, String^ filename);
		Texture1D10(ID3D10Device * device, Texture1DDesc desc, array<IntPtr>^ data);

	protected:
		OVERRIDE(void OnDispose(bool));

	public:
		OVERRIDE( IntPtr Map(int subResource, MapType map, bool doNotWait) );

		OVERRIDE( void UnMap(int subResource) );

		virtual ID3D10Resource* GetResource(){ return _texture;  }

	private:
		void CreateStagingResource();
	};

	
}