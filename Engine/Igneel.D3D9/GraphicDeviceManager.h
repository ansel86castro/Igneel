#pragma once
#include "EnumConverter.h"
#include <vcclr.h>
#include "GraphicDevice.h"
#include <map>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public ref  class D3DGrahicDeviceManager sealed : public GraphicManager 
	{
		internal: 
			IDirect3D9 *_pD3D;

			Action<D3D9GraphicDevice^>^ OnLost;
			Action<D3D9GraphicDevice^>^ OnReset;
	
		protected:
			OVERRIDE(void OnDispose(bool));

		public: 
			D3DGrahicDeviceManager();

			OVERRIDE(GraphicDevice^ CreateDevice(GraphicDeviceDesc^ desc));

			OVERRIDE(bool CheckFormatSupport(int adapter, GraphicDeviceType devType, Format format, BindFlags binding, ResourceType type));		

			OVERRIDE(int CheckMultisampleQualityLevels(int adapter, GraphicDeviceType devType, Format format, int multySampleCount , bool windowed));

			OVERRIDE(bool CheckDeviceType(int adapter, GraphicDeviceType type, Format backBufferFormat ,bool windowed));						

			OVERRIDE( DataBuffer^ CompileFromMemory(
												  String^ shaderCode,												 
												  array<ShaderMacro>^ defines,
												  Include^ include,
												  String^ functionName,
												  String^ profile,
												  ShaderFlags flags) );

			OVERRIDE( DataBuffer^ CompileFromFile(
												  String^ filename,												
												  array<ShaderMacro>^ defines,
												  Include^ include,
												  String^ functionName,
												  String^ profile,
												  ShaderFlags flags) );
	};

	ref class ActionTarget
	{
	public:
		WeakReference^ target;

		virtual void OnLost(D3D9GraphicDevice ^ device) = 0;

		virtual void OnReset(D3D9GraphicDevice ^ device) = 0;

	};

	class DefaultInclude:public ID3DXInclude
	{		
	public:
		STDMETHOD(Open)(THIS_ D3DXINCLUDE_TYPE IncludeType, LPCSTR pFileName, LPCVOID pParentData, LPCVOID *ppData, UINT *pBytes);
		STDMETHOD(Close)(THIS_ LPCVOID pData);
	};

}}
