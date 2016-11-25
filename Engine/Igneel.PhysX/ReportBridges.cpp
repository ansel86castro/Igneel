#include "Stdafx.h"
#include "ReportBridges.h"
#include "Common.h"

namespace Igneel { namespace PhysX {

	bool UserRayCastReport::onHit(const NxRaycastHit& hits)
	{
		RaycastHit rhit = *(RaycastHit*)&hits;
		rhit.Shape = *(gcroot<ActorShape^>*)(hits.shape->userData);		
		return handle->OnHit(rhit);
	}

	bool UserDelegateRayCastReport::onHit(const NxRaycastHit& hits)
	{
		RaycastHit rhit = *(RaycastHit*)&hits;
		rhit.Shape = *(gcroot<ActorShape^>*)(hits.shape->userData);		
		return handle->Invoke(rhit);
	}

	UserContactReport::UserContactReport(IUserContactReport^ report)
	{
		this->handle = report;
	}

	void  UserContactReport::onContactNotify(NxContactPair& pair, NxU32 events)
	{
		ContactReport rep = ContactReport();
		if(!pair.isDeletedActor[0])
			rep.Actor1 = *(gcroot<Actor^>*)pair.actors[0]->userData;
		if(!pair.isDeletedActor[1])
			rep.Actor2 = *(gcroot<Actor^>*)pair.actors[1]->userData;
		rep.SumFrictionForce = TOVECTOR3(pair.sumFrictionForce);
		rep.SumNormalForce = TOVECTOR3(pair.sumNormalForce);
		rep.Stream = IntPtr((NxU32*)pair.stream);

		handle->OnContactNotify(rep, (ContactPairFlag)events);
	}

	UserTriggerReport::UserTriggerReport(ITriggerReport^ report)
	{
		this->handle = report;
	}

	void UserTriggerReport::onTrigger(NxShape& triggerShape, NxShape& otherShape, NxTriggerFlag status)
	{
		handle->OnTrigger(*(gcroot<ActorShape^>*)triggerShape.userData,
						  *(gcroot<ActorShape^>*)otherShape.userData,
						  (ShapeFlag)status);
	}

}}