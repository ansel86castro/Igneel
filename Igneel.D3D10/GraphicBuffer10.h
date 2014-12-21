#pragma once
#include "IResouceContainer.h"

using namespace Igneel::Graphics;


namespace IgneelD3D10
{
	public ref class GraphicBuffer10 : public ShaderBufferBase ,IResourceContainer
	{
	internal:
		ID3D10Buffer * _buffer;
		ID3D10Buffer * _stagingBuffer;
		DXGI_FORMAT _format;
		ID3D10Device* device;
		ID3D10ShaderResourceView* _srv;
	internal:
		GraphicBuffer10(ID3D10Device* device, ID3D10Buffer * _buffer, int size, int stride,  ResourceUsage usage, CpuAccessFlags cpuAcces , D3D10_SUBRESOURCE_DATA* sbData, ResBinding binding);

		/*GraphicBuffer10(ID3D10Device* device, BufferDesc desc);*/	

	protected:		
			OVERRIDE(void OnDispose(bool));

	public:

		    OVERRIDE(IntPtr Map(MapType map, bool doNotWait));

			OVERRIDE(void Unmap());		

			virtual ID3D10Resource* GetResource(){ return _buffer;  }

	private:
		void CreateStaginResource();		
	};

}