#pragma once
#include "EnumConverter.h"
#include "GraphicDevice.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

	public interface class IResetable
	{
		void DeviceReset(IDirect3DDevice9 * device);

		void DeviceLost(IDirect3DDevice9 * device);
	};

	public ref class ResetTarget
	{
		WeakReference<IResetable^>^ _resettable;
	public:
		ResetTarget(IResetable^ target );

		void Reset(D3D9GraphicDevice^ device);

		void Lost(D3D9GraphicDevice^ device);

		void TargetDisposed();
	};
}}