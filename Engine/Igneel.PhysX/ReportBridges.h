#pragma once

using namespace System;
using namespace Igneel::Physics;

namespace Igneel { namespace PhysX {

	public class UserRayCastReport:public  NxUserRaycastReport
	{
		gcroot<IRayCastReport^>handle;
	public:
		UserRayCastReport(IRayCastReport^ report)
		{
			this->handle = report;
		}
		virtual bool onHit(const NxRaycastHit& hits);
	};

	public class UserDelegateRayCastReport: public NxUserRaycastReport
	{
		gcroot<Func<RaycastHit, bool>^>handle;
	public:
		UserDelegateRayCastReport(Func<RaycastHit, bool>^ report)
		{
			this->handle = report;
		}
		virtual bool onHit(const NxRaycastHit& hits);
	};

	public class UserContactReport: public NxUserContactReport
	{		
	public:
		gcroot<IUserContactReport^>handle;

		UserContactReport(IUserContactReport^ report);

		virtual void  onContactNotify(NxContactPair& pair, NxU32 events);
	};

	public class UserTriggerReport: public NxUserTriggerReport
	{		
	public:
		gcroot<ITriggerReport^>handle;

		UserTriggerReport(ITriggerReport^ report);

		virtual void onTrigger(NxShape& triggerShape, NxShape& otherShape, NxTriggerFlag status);
	};
}}