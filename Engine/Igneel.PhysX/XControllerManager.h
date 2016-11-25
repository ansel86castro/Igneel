#pragma once
#include "Common.h"
#include "NxControllerManager.h"
#include "NxCharacter.h"
#include "CharacterController.h"
#include "CCTAllocator.h"
#include "UserAllocator.h"

using namespace System::ComponentModel;
using namespace System;
using namespace Igneel::Physics;


namespace Igneel { namespace PhysX {

	public ref class XControllerManager: public CharacterControllerManager
	{
	internal:
		NxControllerManager* manager;
		UserAllocator* _allocator;
		
	public:
		XControllerManager();

		virtual CharacterController^ _CreateController(Physic^ scene, CharacterControllerDesc^ desc)override;

		OVERRIDE(void UpdateControllers());
	};
}}