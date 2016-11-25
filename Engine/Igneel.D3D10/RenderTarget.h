#pragma once

using namespace Igneel::Graphics;
using namespace System::Runtime::InteropServices;

namespace IgneelD3D10
{
	public ref class RenderTarget10: public RenderTarget
	{
	internal:
		ID3D10RenderTargetView* _rtv;		

		RenderTarget10(ID3D10RenderTargetView* rtv, int width , int height, Format formar, Multisampling msaa);

		void setRenderTargetView(ID3D10RenderTargetView * rtv, int width , int height, Format formar, Multisampling msaa);

		protected:
			OVERRIDE(void OnDispose(bool));

	public:
		OVERRIDE( void GetRenderTargetData(Texture2D^ texture) );
				
	};

	public ref class DephtStencil10:public DepthStencil
	{
		internal:
		ID3D10Texture2D* _texture;
		ID3D10DepthStencilView* _dsv;		

		DephtStencil10(ID3D10DepthStencilView* dsv , ID3D10Texture2D* texture);

		void setDepthStencilView(ID3D10DepthStencilView * dsv , ID3D10Texture2D* texture);

		protected:
			OVERRIDE(void OnDispose(bool));
	};
}