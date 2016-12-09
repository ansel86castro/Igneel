#pragma once

using namespace Igneel::Graphics;

namespace IgneelD3D10
{
	public ref class GraphicManager10: public GraphicDeviceFactory
	{
	private:

	internal:		
		IDXGIFactory * _factory;
		IDXGIAdapter * _adapter; 		
		IDXGIOutput * _display;

	public:
		GraphicManager10 ();
	
		OVERRIDE(GraphicDevice^ CreateInstance(GraphicDeviceDesc^ desc));					
		

	};

	
}