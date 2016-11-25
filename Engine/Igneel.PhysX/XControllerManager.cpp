#include "Stdafx.h"
#include "XControllerManager.h"
#include "XController.h"

namespace Igneel { namespace PhysX {

	XControllerManager::XControllerManager()
	{
		_allocator = new UserAllocator;
		manager = NxCreateControllerManager(_allocator);
		assert(manager);

		GC::AddMemoryPressure(sizeof(UserAllocator));
		GC::AddMemoryPressure(sizeof(NxControllerManager));

	}

	CharacterController^ XControllerManager::_CreateController(Physic^ scene, CharacterControllerDesc^ desc)
	{
		if(desc->Type == ControllerType::BOX)
			return gcnew XBoxController(static_cast<BoxControllerDesc^>(desc), scene, this);
		else
			return gcnew XCapsuleController(static_cast<CapsuleControllerDesc^>(desc), scene, this);
	}

	void XControllerManager::UpdateControllers()
	{
		manager->updateControllers();
		auto controllers =  Controllers;
		for (int i = 0, count = controllers->Count; i < count; i++)
		{
			auto controller = controllers[i];
			if(controller->Affectable!=nullptr)
			{
				controller->Affectable->UpdatePose(controller->GlobalPose);
			}

		}
	}

}}