#include "Stdafx.h"
#include "Resseting.h"

namespace Igneel { namespace D3D9 {

	ResetTarget::ResetTarget(IResetable^ target)
	{
		auto device = static_cast<D3D9GraphicDevice^>(Engine::Graphics);
		_resettable = gcnew WeakReference<IResetable^>(target);

		device->Reset += gcnew Action<D3D9GraphicDevice^>(this, &ResetTarget::Reset);
		device->Lost += gcnew Action<D3D9GraphicDevice^>(this, &ResetTarget::Lost);		
	}

	void ResetTarget::Reset(D3D9GraphicDevice^ device)
	{
		IResetable^ target;
		if(_resettable->TryGetTarget(target))
		{
			target->DeviceReset(device->_pDevice);
		}
	}

	void ResetTarget::Lost(D3D9GraphicDevice^ device)
	{
		IResetable^ target;
		if(_resettable->TryGetTarget(target))
		{
			target->DeviceLost(device->_pDevice);
		}
	}

	void ResetTarget::TargetDisposed()
	{
		auto device = static_cast<D3D9GraphicDevice^>(Engine::Graphics);
		device->Reset -= gcnew Action<D3D9GraphicDevice^>(this, &ResetTarget::Reset);
		device->Lost -= gcnew Action<D3D9GraphicDevice^>(this, &ResetTarget::Lost);
	}


}}