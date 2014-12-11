#include "Stdafx.h"
#include "XScene.h"
#include "PhysicManager.h"
#include "XMaterial.h"
#include "XActor.h"
#include "XSweepCache.h"
#include "Shape.h"
#include "ReportBridges.h"

namespace Igneel { namespace PhysX {

	XScene::XScene(PhysicDesc^ desc)
	{
		Name = desc->Name;
		
		NxSceneDesc sd = NxSceneDesc();
		sd.flags |= static_cast<NxU32>(desc->Flags) | NX_SF_ENABLE_ACTIVETRANSFORMS;
		sd.gravity =  ToNxVector3(desc->Gravity);
		sd.simType = NX_SIMULATION_HW;
		sd.upAxis = 1;
		sd.userData = new gcroot<XScene^>(this);		

		flags = desc->Flags;
		Simulation =  SimulationType::SIMULATION_HW;
		scene = static_cast<PXPhysicManager^>(PhysicManager::Sigleton)->sdk->createScene(sd);

		if(!scene)
		{
			sd.simType = NX_SIMULATION_SW;
			Simulation =  SimulationType::SIMULATION_SW;
			scene = static_cast<PXPhysicManager^>(PhysicManager::Sigleton)->sdk->createScene(sd);
		}

		if(!scene)
			throw gcnew InvalidOperationException(L"Scene fail creation");

		AddMaterial(gcnew _XMaterial(this, scene->getMaterialFromIndex(0)));

		GC::AddMemoryPressure(sizeof(NxScene));		
	}

	void XScene::SetGravity(Vector3 value)
	{
		scene->setGravity(TONXVEC3(value));		
	}

	void XScene::OnDispose(bool d)
	{		
		__super::OnDispose(d);

		if(scene)
		{
			gcroot<XScene^>* handle = static_cast<gcroot<XScene^>*>(scene->userData);
			delete handle;
			static_cast<PXPhysicManager^>(PhysicManager::Sigleton)->sdk->releaseScene(*scene);
			scene = NULL;
		}
	}

	void XScene::Simulate(float elapsedTime)
	{
		if(!Enable) return;

		NxU32 nbTransforms = 0;    
		NxActiveTransform *activeTransforms = NULL;

		if(scene->getFlags() & NX_SF_ENABLE_ACTIVETRANSFORMS)			
			activeTransforms = scene->getActiveTransforms(nbTransforms);			

		scene->simulate(elapsedTime);
		
		for (int i = 0; i < nbTransforms; ++i)
		{			
			gcroot<Actor^>* link = static_cast<gcroot<Actor^>*>(activeTransforms[i].actor->userData);
			if(link)
			{
				Actor^ actor = *link;
				if(actor->Affectable!=nullptr)
					actor->Affectable->UpdatePose(ToDxMatrix(activeTransforms[i].actor2World));
			}
		}

		scene->flushStream();
		scene->fetchResults(NX_RIGID_BODY_FINISHED, true);
	}

	PhysicMaterial^ XScene::CreateMaterialImp(PhysicMaterialDesc^desc)
	{ 
		return gcnew _XMaterial(this,desc);
	}

	Actor^ XScene::CreateActorImp(ActorDesc^ desc)
	{
		return gcnew _XActor(this, desc);
	}

	SweepCache^ XScene::CreateSweepCache()
	{
		return gcnew XSweepCache(scene);	
	}

	void XScene::SetGroupCollisionFlag(NxCollisionGroup g1 , NxCollisionGroup g2 ,bool enable )
	{
		scene->setGroupCollisionFlag(g1, g2, enable);
	}

	void XScene::SetActorGroupPairFlags(NxActorGroup g1 , NxActorGroup g2 , ContactPairFlag flags)
	{
		scene->setActorGroupPairFlags(g1,g2, (NxU32)flags);
	}

	void XScene::SetActorPairFlags(Actor^ actor1, Actor^ actor2 ,  ContactPairFlag flags)
	{
		NxActor* a1 = static_cast<_XActor^>(actor1)->actor;
		NxActor* a2 = static_cast<_XActor^>(actor2)->actor;
		scene->setActorPairFlags(*a1, *a2, (NxU32)flags);
	}

	void XScene::SetShapePairFlags(ActorShape^ shape1, ActorShape^ shape2, ContactPairFlag flags)
	{
		NxShape* s1 = static_cast<XShape^>(shape1)->shape;
		NxShape* s2 = static_cast<XShape^>(shape2)->shape;
		scene->setShapePairFlags(*s1, *s2, (NxU32)flags);		
	}

	void XScene::SetFilterOperations(FilterOperation op0, FilterOperation op1 , FilterOperation op2)
	{
		scene->setFilterOps((NxFilterOp)op0 , (NxFilterOp)op1, (NxFilterOp)op2);
	}

	void XScene::SetFilterBool(bool value)
	{
		scene->setFilterBool(value);
	}

	void XScene::SetFilterConstant0(GroupsMask mask)
	{
		scene->setFilterConstant0(*(NxGroupsMask*)&mask);
	}

	void XScene::SetFilterConstant1(GroupsMask mask)
	{
		scene->setFilterConstant1(*(NxGroupsMask*)&mask);	
	}

	void XScene::RaycastAllShapes(Ray ray, IRayCastReport^ report , ShapesType shapeType ,int groups, float maxDistance, RayCastBit hintflags ,GroupsMask mask)
	{
		UserRayCastReport rep = UserRayCastReport(report);		
		scene->raycastAllShapes(*reinterpret_cast<NxRay*>(&ray),rep, (NxShapesType)shapeType,groups, maxDistance, (NxU32)flags, (NxGroupsMask*)&mask);
	}

	bool XScene::RaycastAnyShapes(Ray ray, ShapesType shapeType ,int groups, float maxDistance, GroupsMask mask )
	{
	   return scene->raycastAnyShape(*(NxRay*)&ray, (NxShapesType)shapeType,groups, maxDistance, (NxGroupsMask*)&mask);
	}

	RaycastHit XScene::RaycastClosestShape(Ray ray, ShapesType shapeType, int groups, float maxDistance, RayCastBit hintflags, GroupsMask mask )
	{
		NxRaycastHit nxHit;
		NxShape* shape  = scene->raycastClosestShape(*(NxRay*)&ray, (NxShapesType)shapeType, nxHit, groups, maxDistance, (NxU32)hintflags, (NxGroupsMask*)&mask);
		if(!shape)
		{
			return RaycastHit();
		}
		ActorShape ^ hitShape = * (gcroot<ActorShape^>*)shape->userData;
		RaycastHit rhit = *(RaycastHit*)&nxHit;
		rhit.Shape = *(gcroot<ActorShape^>*)(shape->userData);	
		rhit;
	}

	void XScene::SetUserContactReport(IUserContactReport^ report)
	{
		if(report!=nullptr)
		{
			if(!userReport)
			{
				userReport = new UserContactReport(report);
			}
			else
				userReport->handle = report;
		}
		else if(userReport)
		{			
			delete userReport;
			userReport = NULL;			
		}

		scene->setUserContactReport(userReport);
	}

	void XScene::SetTriggerCallback(ITriggerReport^ report)
	{
		if(report!=nullptr)
		{
			if(!triggerReport)
			{
				triggerReport = new UserTriggerReport(report);
			}
			else
				triggerReport->handle = report;
		}
		else if(triggerReport)
		{			
			delete triggerReport;
			triggerReport = NULL;			
		}

		scene->setUserTriggerReport(triggerReport);
	}
}}