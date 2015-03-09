#pragma once
#include "Common.h"
#include "NxControllerManager.h"
#include "NxCharacter.h"
#include "CharacterController.h"
#include "CCTAllocator.h"
#include "UserAllocator.h"
#include "NxController.h"
#include "NxBoxController.h"
#include "NxCapsuleController.h"

using namespace System::ComponentModel;
using namespace System;

using namespace Igneel::Physics;

#define EXTOVECTOR3(v) Vector3((float)v.x, (float)v.y, (float)v.z)
#define VECTOR3TOEXT(v) NxExtendedVec3((double)v.X, (double)v.Y, (double)v.Z)

namespace Igneel { namespace PhysX {

	class UserControllerHitReportBrige:public NxUserControllerHitReport
	{
	private:
		gcroot<IUserControllerHitReport^> handle;

	public:
		UserControllerHitReportBrige(IUserControllerHitReport^ target)
		{
			handle = target;
		}				

		virtual NxControllerAction  onShapeHit(const NxControllerShapeHit& hit)
		{
			ControllerShapeHit sh;
			sh.ActorShape = *static_cast<gcroot<ActorShape^>*>( hit.shape->userData);
			sh.Controller = *static_cast<gcroot<CharacterController^>*>( hit.controller->getUserData());
			sh.Dir = TOVECTOR3(hit.dir);
			sh.Length = hit.length;
			sh.WorldNormal = TOVECTOR3(hit.worldNormal);
			sh.WorldPos = TOVECTOR3(hit.worldPos);

			//interior_ptr<ControllerShapeHit> p = &sh;
			return static_cast<NxControllerAction>( handle->OnShapeHit(sh) );
		}

		virtual NxControllerAction  onControllerHit(const NxControllersHit& hit)
		{
			return static_cast<NxControllerAction>( handle->OnControllerHit(*static_cast<gcroot<CharacterController^>*>( hit.controller->getUserData()),
									*static_cast<gcroot<CharacterController^>*>( hit.other->getUserData())));
		}
	};
	

	public ref class XController: public CharacterControllerBase
	{
	internal:
		NxController * controller;
		UserControllerHitReportBrige* callback;
		float stepOffset;
	public:
		XController(CharacterControllerManager^ manager);

			virtual property Vector3 Position
			{
				Vector3 get() override { return EXTOVECTOR3(controller->getPosition()); }
				void set(Vector3 v)override { controller->setPosition(VECTOR3TOEXT(v)); }
			}

			virtual property CCTInteraction Interaction{
				CCTInteraction get() override 
				{ 
					return static_cast<CCTInteraction>(controller->getInteraction());
				}
				void set(CCTInteraction v)override
				{
					controller->setInteraction(static_cast<NxCCTInteractionFlag>(v));
				}				
			}		            

			virtual property float StepOffset
			{
				float get() override { return stepOffset; }
				void set(float v)override 
				{
					stepOffset = v;
					controller->setStepOffset(v); 
				}
			}
		
      virtual void ReportSceneChanged()override
			{
				controller->reportSceneChanged();
			}      

        virtual CollitionResult Move(Vector3 disp, NxU32 activeGroups , float minDist , float sharpness , GroupsMask groupsMask) override;

		virtual CollitionResult Move(Vector3 disp, NxU32 activeGroups , float minDist , float sharpness) override;

		virtual void OnDispose(bool) override;

		void FillControllerDesc(NxControllerDesc & cd, CharacterControllerDesc^ desc);

	protected:
		void SetActor();
	};

	public ref class XBoxController: XController, BoxController
	{
	public:
		XBoxController(BoxControllerDesc^ desc, Physic^ scene, CharacterControllerManager^ manager);

		virtual property Vector3 Extents{
				Vector3 get()  { return TOVECTOR3(CAST(NxBoxController, controller)->getExtents());}
				void set(Vector3 v)  { CAST(NxBoxController,controller)->setExtents(TONXVEC3(v));}
			}
	};

	public ref class XCapsuleController: XController, CapsuleController
	{
	public:
			XCapsuleController(CapsuleControllerDesc^ desc, Physic^ scene, CharacterControllerManager^ manager);

			virtual property float Radius{
				float get(){ return CAST(NxCapsuleController,controller)->getRadius();}
				void set(float v){ CAST(NxCapsuleController,controller)->setRadius(v);}
			}

			virtual property float Height{
				float get(){ return CAST(NxCapsuleController,controller)->getHeight();}
				void set(float v){ CAST(NxCapsuleController,controller)->setHeight(v);}
			}

			virtual property CapsuleClimbingMode ClimbingMode
			{
				CapsuleClimbingMode get(){  return static_cast<CapsuleClimbingMode>(CAST(NxCapsuleController,controller)->getClimbingMode()); }
				void set(CapsuleClimbingMode v){
					CAST(NxCapsuleController,controller)->setClimbingMode(static_cast<NxCapsuleClimbingMode>(v));
				}
			}
	};
}}