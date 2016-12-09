#include "Stdafx.h"
#include "DeviceManager.h"
#include "GraphicDevice.h"
#include "InputLayout.h"

namespace IgneelD3D10
{
	GraphicManager10::GraphicManager10()
	{
		IDXGIFactory * factory;
		SAFECALL( CreateDXGIFactory(__uuidof(IDXGIFactory), (void**)(&factory) ) );
		_factory  = factory;

		IDXGIAdapter * pAdapter; 
		if(_factory->EnumAdapters(0, &pAdapter) == DXGI_ERROR_NOT_FOUND)
		{
			throw gcnew AdapterNotFoundException(0);
		}
		_adapter = pAdapter;

		IDXGIOutput * display;
		if(_adapter->EnumOutputs(0, &display) == DXGI_ERROR_NOT_FOUND)
			throw gcnew GraphicDeviceFailException("Display not found");
		_display = display;
		
	}

	GraphicDevice^  GraphicManager10::CreateInstance(GraphicDeviceDesc^ desc)
	{
		return gcnew GraphicDevice10(desc);
	}	
}