#include "Stdafx.h"
#include "XController.h"
#include "XControllerManager.h"
#include "XScene.h"
#include "XActor.h"

namespace Igneel { namespace PhysX {

	XController::XController(CharacterControllerManager^ manager)
	{			
		this->manager = manager;
		callback = NULL;
	}

	CollitionResult XController::Move(Vector3 disp, NxU32 activeGroups , float minDist , float sharpness , GroupsMask groupsMask)
	{
		NxU32 flag;
		controller->move(TONXVEC3(disp), activeGroups, minDist, flag ,sharpness, (NxGroupsMask*)&groupsMask); 
		
		return static_cast<CollitionResult>( flag );
	}

	CollitionResult XController::Move(Vector3 disp, NxU32 activeGroups , float minDist , float sharpness)
	{
		NxU32 flag;
		controller->move(TONXVEC3(disp), activeGroups, minDist, flag ,sharpness, NULL); 	

		return static_cast<CollitionResult>( flag );
	}

	
	void XController::OnDispose(bool d)
	{
		if(controller)
		{
			if(controller->getUserData())
			{
				gcroot<CharacterController^>* h = (gcroot<CharacterController^>*)controller->getUserData();
				delete h;
				
			}

			static_cast<XControllerManager^>(manager)->manager->releaseController(*controller);
			controller = NULL;
		}
		if(callback)
		{
			delete callback;
			callback = NULL;
		}

		__super::OnDispose(d);
	}

	void XController::FillControllerDesc(NxControllerDesc & cd, CharacterControllerDesc^ desc)
	{
		cd.position = VECTOR3TOEXT( desc->Position);
		cd.upDirection = static_cast<NxHeightFieldAxis>( desc->UpDirection );
		cd.slopeLimit = desc->SlopeLimit;
		cd.skinWidth = desc->SkinWidth;
		cd.stepOffset = desc->StepOffset;
		cd.interactionFlag = static_cast<NxCCTInteractionFlag>( desc->InteractionFlag );
		if(desc->HitReport !=nullptr)
		{
			callback = new UserControllerHitReportBrige(desc->HitReport);	
			cd.callback= callback;
		}
	}

	void XController::SetActor()
	{
		//auto nxactor = controller->getActor();
		//XScene^ scene =  *(gcroot<XScene^>*)nxactor->getScene().userData;
		//auto shapes = nxactor->getShapes();
		//for (int i = 0; i < nxactor->getNbShapes(); i++)
		//{
		//	if(shapes[i]->userData)
		//	{
		//		//delete shapes[i]->userData;
		//		shapes[i]->userData = NULL;
		//	}
		//}		
		//this->actor = gcnew _XActor(scene, nxactor);
		//scene->_AddActor(actor);
	}

	XBoxController::XBoxController(BoxControllerDesc^ desc, Physic^ scene, CharacterControllerManager^ manager)
		:XController(manager)
	{
		NxBoxControllerDesc bd;
		bd.userData = new gcroot<CharacterController^>(this);
		bd.extents = ToNxVector3( desc->Extents );
		FillControllerDesc(bd, desc);		

		controller = static_cast<XControllerManager^>(manager)->manager->createController(static_cast<XScene^>(scene)->scene, bd);
		SetActor();

		GC::AddMemoryPressure(sizeof(NxBoxController));
	}

	XCapsuleController::XCapsuleController(CapsuleControllerDesc^ desc, Physic^ scene, CharacterControllerManager^ manager)
			:XController(manager)
	{
		NxCapsuleControllerDesc bd;
		bd.userData = new gcroot<CharacterController^>(this);
		bd.height = desc->Height;
		bd.radius = desc->Radius;
		bd.climbingMode = static_cast<NxCapsuleClimbingMode>(desc->ClimbingMode);
		FillControllerDesc(bd, desc);	

		controller = static_cast<XControllerManager^>(manager)->manager->createController(static_cast<XScene^>(scene)->scene, bd);
		SetActor();

		GC::AddMemoryPressure(sizeof(NxCapsuleController));
	}

}}