#pragma once
#include "Common.h"
#include "NxCooking.h"

using namespace System;
using namespace System::Runtime;
using namespace Igneel::Physics;
using namespace System::IO;

namespace Igneel { namespace PhysX {

	public ref class PXPhysicManager: public PhysicManager
	{
	internal:
		NxPhysicsSDK * sdk;
		NxCookingInterface * cooking;

	public:
		PXPhysicManager();

		OVERRIDE(CharacterControllerManager^ CreateControllerManager());

		OVERRIDE(Physic^ _CreatePhysic(PhysicDesc^ desc));

        OVERRIDE(TriangleMesh^ _CreateTriangleMesh(TriangleMeshDesc^ desc));

		OVERRIDE(TriangleMesh^ _CreateTriangleMeshFromFile(String^ filename) );        

		OVERRIDE(TriangleMesh^ _CreateTriangleMeshFromStream(Stream^ stream));

		OVERRIDE(void OnDispose(bool));
		
	};

	#define PManager static_cast<PXPhysicManager^>(PhysicManager::Sigleton);
	#define NxSdk static_cast<PXPhysicManager^>(PhysicManager::Sigleton)->sdk;

}}