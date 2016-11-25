#pragma once
#include "Common.h"
#include "ReportBridges.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Physics;

namespace Igneel { namespace PhysX {

	public ref class XScene : public Physic
	{
	internal:
		NxScene * scene;
		UserContactReport * userReport;
		UserTriggerReport* triggerReport;

	public:
		XScene(PhysicDesc^ desc);

		OVERRIDE( void Simulate(float elapsedTime));
        
        OVERRIDE( void SetGravity(Vector3 value));

        OVERRIDE( Actor^ CreateActorImp(ActorDesc^ desc));

        OVERRIDE( PhysicMaterial^ CreateMaterialImp(PhysicMaterialDesc^ desc));

		OVERRIDE( void OnDispose(bool));

		OVERRIDE( SweepCache^ CreateSweepCache() );

		OVERRIDE(void SetGroupCollisionFlag(NxCollisionGroup g1 , NxCollisionGroup g2 ,bool enable ));

		OVERRIDE(void SetActorGroupPairFlags(NxActorGroup g1 , NxActorGroup g2 , ContactPairFlag flags));

		OVERRIDE(void SetActorPairFlags(Actor^ actor1, Actor^ actor2 ,  ContactPairFlag flags) ); 

		OVERRIDE(void SetShapePairFlags(ActorShape^ shape1, ActorShape^ shape2, ContactPairFlag flags));

		OVERRIDE(void SetFilterOperations(FilterOperation op0, FilterOperation op1 , FilterOperation op2));

		OVERRIDE(void SetFilterBool(bool value));

		OVERRIDE(void SetFilterConstant0(GroupsMask mask));

		OVERRIDE(void SetFilterConstant1(GroupsMask mask));

		OVERRIDE(void RaycastAllShapes(Ray ray, IRayCastReport^ report , ShapesType shapeType ,int groups, float maxDistance, RayCastBit hintflags ,GroupsMask mask ));

		OVERRIDE(bool RaycastAnyShapes(Ray ray, ShapesType shapeType ,int groups, float maxDistance, GroupsMask mask ));

		OVERRIDE(RaycastHit RaycastClosestShape(Ray ray, ShapesType shapeType, int groups, float maxDistance ,RayCastBit hintflags, GroupsMask mask ));

		OVERRIDE(void SetUserContactReport(IUserContactReport^ report));

		OVERRIDE(void SetTriggerCallback(ITriggerReport^ report));

		void _AddActor(Actor^ a)
		{
			AddActor(a);
		}

	};

}}