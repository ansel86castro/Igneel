#include "Stdafx.h"
#include "XActor.h"
#include "Shape.h"
#include "XTriangleMesh.h"
#include "XScene.h"
#include "XSweepCache.h"

namespace Igneel { namespace PhysX {

	void _XActor::SetDispose()
	{
		this->Disposed = true;
	}

	NxShapeDesc* _XActor::GetShapeDescription(ActorShapeDesc^ desc)
	{
		NxShapeDesc* sd;
		switch (desc->Type)
		{
			case ShapeType::BOX:		
			{	auto box = new NxBoxShapeDesc();
				box->setToDefault();
				box->dimensions = ToNxVector3(((BoxShapeDesc^)desc)->Dimensions);
				sd = box;
				break;
			}
			case ShapeType::SPHERE:
			{	auto s = new NxSphereShapeDesc();
				s->setToDefault();
				s->radius = ((SphereShapeDesc^)desc)->Radius;
				sd = s;
				break;
			}
			case ShapeType::PLANE:		
			{	auto p = new NxPlaneShapeDesc();
				p->setToDefault();
				p->d = ((PlaneShapeDesc^)desc)->Plane.D;
				p->normal = ToNxVector3( ((PlaneShapeDesc^)desc)->Plane.Normal );
				sd = p;
				break;
			}
			case ShapeType::CAPSULE:		
			{	auto c = new NxCapsuleShapeDesc();
				c->setToDefault();
				c->radius = ((CapsuleShapeDesc^)desc)->Radius;
				c->height = ((CapsuleShapeDesc^)desc)->Height;
				sd = c;
				break;
			}
			case ShapeType::MESH:
				{
					auto m = new NxTriangleMeshShapeDesc();
					m->setToDefault();
					m->meshFlags = static_cast<NxU32>( ((TriangleMeshShapeDesc^)desc)->MeshFlags) ;
					m->meshData = ((XTriangleMesh^)((TriangleMeshShapeDesc^)desc)->Mesh)->m_mesh;
					sd = m;
					break;
				}
			case ShapeType::WHEEL:
				{
					auto w = new NxWheelShapeDesc();					
					w->setToDefault();
					WheelShapeDesc^ ws = (WheelShapeDesc^)desc;					
					SpringDesc suspention = ws->Suspension;
					auto lontire = ws->LongitudalTireForceFunction;
					auto lattire = ws->LateralTireForceFunction;

					w->radius           =  ws->Radius;
					w->suspensionTravel =  ws->SuspensionTravel;
					w->inverseWheelMass =  ws->InverseWheelMass;
					w->wheelFlags       = static_cast<NxU32>( ws->WheelFlags);
					w->motorTorque      =  ws->MotorTorque;
					w->brakeTorque      =  ws->BrakeTorque;
					w->steerAngle       =  ws->SteerAngle;
					w->suspension       =  *(NxSpringDesc*)&suspention;

					w->longitudalTireForceFunction.asymptoteSlip = lontire.AsymptoteSlip;
					w->longitudalTireForceFunction.asymptoteValue = lontire.AsymptoteValue;
					w->longitudalTireForceFunction.extremumSlip = lontire.ExtremumSlip;
					w->longitudalTireForceFunction.extremumValue = lontire.ExtremumValue;
					w->longitudalTireForceFunction.stiffnessFactor = lontire.StiffnessFactor;

					w->lateralTireForceFunction.asymptoteSlip = lattire.AsymptoteSlip;
					w->lateralTireForceFunction.asymptoteValue = lattire.AsymptoteValue;
					w->lateralTireForceFunction.extremumSlip = lattire.ExtremumSlip;
					w->lateralTireForceFunction.extremumValue = lattire.ExtremumValue;
					w->lateralTireForceFunction.stiffnessFactor = lattire.StiffnessFactor;					
					sd = w;
									
				break;
				}
		}

		auto mask = desc->GroupsMask;
		sd->shapeFlags = static_cast<NxU32>( desc->Flags );
		sd->materialIndex = desc->MaterialIndex;
		sd->density = desc->Density;
		sd->mass = desc->Mass;
		sd->skinWidth = desc->SkinWidth;
		sd->localPose = ToNxMat34( desc->LocalPose );
		sd->group = desc->CollitionGroup;
		sd->groupsMask = *(NxGroupsMask*)&mask;
		sd->nonInteractingCompartmentTypes = desc->NonInteractingCompartmentTypes;
		sd->userData = new gcroot<ActorShapeDesc^>(desc);

		if(!sd->isValid())
		{
			delete sd;
			throw gcnew InvalidOperationException(L"Invalid Shape");			
		}
		return sd;
	}

	NxActorDesc* _XActor::GetActorDescription(ActorDesc^ desc)
	{
		NxActorDesc ad = NxActorDesc();
		ad.density = desc->Density;
		ad.globalPose = ToNxMat34( desc->GlobalPose );
		ad.flags = static_cast<NxU32>( desc->Flags );
		ad.group = desc->CollitionGroup;
		ad.dominanceGroup = desc->DominanceGroup;
		ad.contactReportFlags = static_cast<NxU32>( desc->ContactReportFlags );
		ad.forceFieldMaterial = desc->ForceFieldMaterial;
		ad.dominanceGroup = desc->DominanceGroup;

		if(desc->Body!=nullptr)
		{
			BodyDesc^ body = desc->Body; 
			NxBodyDesc bd = NxBodyDesc();

			bd.angularDamping = body->AngularDamping;
			bd.angularVelocity = ToNxVector3( body->AngularVelocity );
			bd.CCDMotionThreshold =  body->CCDMotionThreshold;
			bd.contactReportThreshold = body->ContactReportThreshold;
			bd.flags = static_cast<NxU32>( body->Flags);
			bd.linearDamping = body->LinearDamping;
			bd.linearVelocity = ToNxVector3( body->LinearVelocity);
			bd.mass = body->Mass;
			bd.massLocalPose = ToNxMat34( body->MassLocalPose);
			bd.massSpaceInertia = ToNxVector3( body->MassSpaceInertia);
			bd.maxAngularVelocity = body->MaxAngularVelocity;
			bd.sleepAngularVelocity = body->SleepAngularVelocity;
			bd.sleepDamping = body->SleepDamping;
			bd.sleepEnergyThreshold = body->SleepEnergyThreshold;
			bd.sleepLinearVelocity = body->SleepLinearVelocity;
			bd.solverIterationCount = body->SolverIterationCount;
			bd.wakeUpCounter = body->WakeUpCounter;

			ad.body = new NxBodyDesc(bd);			
		}

		for each (ActorShapeDesc^ shape in desc->Shapes)
		{
			auto nxShape = GetShapeDescription(shape);
			ad.shapes.pushBack(nxShape);
		}

		if(!ad.isValid())
		{
			if(ad.body)
				delete ad.body;
			for (auto begin  = ad.shapes.begin(); begin != ad.shapes.end(); begin++)
			{
				delete (*begin);
			}
			throw gcnew InvalidOperationException(L"Invalid Actor");
		}
		return new NxActorDesc(ad);
	}

	ActorShape^ _XActor::GetShape(NxShape* shape)
	{
		switch (shape->getType())
		{
			case NX_SHAPE_BOX:
				return gcnew XBoxShape((NxBoxShape*)shape, this);
			break;		
			case NX_SHAPE_SPHERE:
				return gcnew XSphereShape((NxSphereShape*)shape, this);
			break;		
			case NX_SHAPE_PLANE:
				return gcnew XPlaneShape((NxPlaneShape*)shape, this);
			break;		
			case NX_SHAPE_CAPSULE:
				return gcnew XCapsuleShape((NxCapsuleShape*)shape, this);
			break;		
			case NX_SHAPE_MESH:
				return gcnew XTriangleMeshShape((NxTriangleMeshShape*)shape, this);
			break;		
			case NX_SHAPE_WHEEL:
				return gcnew XWheelShape((NxWheelShape*)shape, this);
			break;
			case NX_SHAPE_CONVEX:
				throw gcnew NotImplementedException();
			break;		
			case NX_SHAPE_HEIGHTFIELD:
				throw gcnew NotImplementedException();
			break;
		}
	}

	_XActor::_XActor(Physic^  scene, ActorDesc^ desc)
	{
		auto xscene = static_cast<XScene^>(scene);
		this->Scene = scene;
		auto nxDesc  = GetActorDescription(desc);
		this->actor = xscene->scene->createActor(*nxDesc);

		NX_ASSERT(this->actor);

		this->actor->userData = new gcroot<_XActor^>(this);
		Name = desc->Name;
		Flags = desc->Flags;

		if(desc->Body!=nullptr)
		{ 
			BodyFlags = desc->Body->Flags;
			IsDynamic = true;
		}

		delete nxDesc->body;		

		Init();

		for (auto begin  = nxDesc->shapes.begin(); begin != nxDesc->shapes.end(); begin++)
		{
			delete (*begin);
		}

		delete nxDesc;		
	}

	 _XActor::_XActor(Physic^  scene, NxActor* a)
	 {
		 this->Scene = scene;
		 this->actor = a;
		 this->actor->userData = new gcroot<_XActor^>(this);

		 Name = "Actor"+ scene->Actors->Count;

		 if(a->readActorFlag(NxActorFlag::NX_AF_CONTACT_MODIFICATION))
			 flags = flags | ActorFlag::CONTACT_MODIFICATION;
		 if(a->readActorFlag(NxActorFlag::NX_AF_DISABLE_COLLISION))
			 flags = flags | ActorFlag::DISABLE_COLLISION;
		 if(a->readActorFlag(NxActorFlag::NX_AF_DISABLE_RESPONSE))
			 flags = flags | ActorFlag::DISABLE_RESPONSE;
		 if(a->readActorFlag(NxActorFlag::NX_AF_FLUID_DISABLE_COLLISION))
			 flags = flags | ActorFlag::FLUID_DISABLE_COLLISION;
		 if(a->readActorFlag(NxActorFlag::NX_AF_FORCE_CONE_FRICTION))
			 flags = flags | ActorFlag::FORCE_CONE_FRICTION;
		 if(a->readActorFlag(NxActorFlag::NX_AF_LOCK_COM))
			 flags = flags | ActorFlag::LOCK_COM;
		 if(a->readActorFlag(NxActorFlag::NX_AF_USER_ACTOR_PAIR_FILTERING))
			 flags = flags | ActorFlag::USER_ACTOR_PAIR_FILTERING;		 


		 if(a->readBodyFlag(NxBodyFlag::NX_BF_DISABLE_GRAVITY))
			 bodyFlags = bodyFlags | BodyActorFlags::DISABLE_GRAVITY;
		 if(a->readBodyFlag(NxBodyFlag::NX_BF_FROZEN_POS_X))
			 bodyFlags = bodyFlags | BodyActorFlags::FROZEN_POS_X;
		 if(a->readBodyFlag(NxBodyFlag::NX_BF_FROZEN_POS_Y))
			 bodyFlags = bodyFlags | BodyActorFlags::FROZEN_POS_Y;
		 if(a->readBodyFlag(NxBodyFlag::NX_BF_FROZEN_POS_Z))
			 bodyFlags = bodyFlags | BodyActorFlags::FROZEN_POS_Z;
		 if(a->readBodyFlag(NxBodyFlag::NX_BF_FROZEN_ROT_X))
			 bodyFlags = bodyFlags | BodyActorFlags::FROZEN_ROT_X;
		 if(a->readBodyFlag(NxBodyFlag::NX_BF_FROZEN_ROT_Y))
			 bodyFlags = bodyFlags | BodyActorFlags::FROZEN_ROT_Y;
		 if(a->readBodyFlag(NxBodyFlag::NX_BF_FROZEN_ROT_Z))
			 bodyFlags = bodyFlags | BodyActorFlags::FROZEN_ROT_Z;		
		 if(a->readBodyFlag(NxBodyFlag::NX_BF_KINEMATIC))
			 bodyFlags = bodyFlags| BodyActorFlags::KINEMATIC;
		 if(a->readBodyFlag(NxBodyFlag::NX_BF_DUMMY_0))
			 bodyFlags = bodyFlags| BodyActorFlags::DUMMY_0;
		 if(a->readBodyFlag(NxBodyFlag::NX_BF_FILTER_SLEEP_VEL))
			 bodyFlags = bodyFlags| BodyActorFlags::FILTER_SLEEP_VEL;
		 if(a->readBodyFlag(NxBodyFlag::NX_BF_ENERGY_SLEEP_TEST))
			 bodyFlags = bodyFlags| BodyActorFlags::ENERGY_SLEEP_TEST;
			 
		 IsDynamic = a->isDynamic();

		 Init();
	 }

	 void _XActor::Init()
	 {
		auto shapes = actor->getShapes();
		for (int i = 0; i < actor->getNbShapes(); i++)
		{
			auto shape = GetShape(shapes[i]);
			AddShape(shape);
		}
	 }

	 ActorShape^  _XActor::CreateShapeImp(ActorShapeDesc^ desc)
	 {
		 auto shapeDesc = GetShapeDescription(desc);
		 auto shape = actor->createShape(*shapeDesc);
		 auto xs = GetShape(shape);

		 AddShape(xs);

		 delete shapeDesc;

		 return xs;
	 }

	 void _XActor::OnDispose(bool d)
	 {				 
		 __super::OnDispose(d);
		 if(actor)
		 {			 
			actor->getScene().releaseActor(*actor);
			scene = nullptr;
			actor = NULL;
		 }
	 }

	 struct UserPredicateReport: public NxUserEntityReport<NxSweepQueryHit>
	 {
		 gcroot<Predicate<SweepQueryHit>^> handle;

		 virtual bool onEvent(NxU32 nbEntities, NxSweepQueryHit* entities) override
		 {			 
			 for (int i = 0; i < nbEntities; i++)
			 {
				 SweepQueryHit  hit;
				 hit.FaceID = entities[i].faceID;
				 hit.HitShape = *((gcroot<ActorShape^>*)entities[i].hitShape->userData);
				 hit.InternalFaceID = entities[i].internalFaceID;
				 hit.Normal = TOVECTOR3( entities[i].normal );
				 hit.Point = TOVECTOR3(entities[i].normal);
				 hit.SweepShape = *((gcroot<ActorShape^>*)entities[i].sweepShape->userData);
				 hit.T= entities[i].t;

				 if(!handle->Invoke(hit))
					 return false;
			 }

			 return true;
		 }
	 };

	 struct UserCollectionReport: public NxUserEntityReport<NxSweepQueryHit>
	 {
		 gcroot<ICollection<SweepQueryHit>^> handle;
		 int count;

		 virtual bool onEvent(NxU32 nbEntities, NxSweepQueryHit* entities) override
		 {			 
			 for (int i = 0; i < nbEntities; i++)
			 {
				 SweepQueryHit  hit;
				 hit.FaceID = entities[i].faceID;
				 hit.HitShape = *((gcroot<ActorShape^>*)entities[i].hitShape->userData);
				 hit.InternalFaceID = entities[i].internalFaceID;
				 hit.Normal = TOVECTOR3( entities[i].normal );
				 hit.Point = TOVECTOR3(entities[i].normal);
				 hit.SweepShape = *((gcroot<ActorShape^>*)entities[i].sweepShape->userData);
				 hit.T= entities[i].t;

				 count++;
				 handle->Add(hit);
			 }

			 return true;
		 }
	 };

	 SweepQueryHit  _XActor::LinearSweep(Vector3 motion, SweepFlags flags, Predicate<SweepQueryHit>^ callback, SweepCache^ cache)
	 {
		 NxSweepCache* ncache;

		 if(cache!=nullptr)
			 ncache = static_cast<XSweepCache^>(cache)->cache;

		 NxSweepQueryHit shape;
		 UserPredicateReport rp;
		 rp.handle = callback;

		 int count = actor->linearSweep(TONXVEC3(motion), static_cast<NxU32>( flags ),NULL, 1, &shape, &rp, ncache);

		 SweepQueryHit hit;
		 if(count > 0)
		  {
			hit.FaceID = shape.faceID;
			hit.HitShape = *((gcroot<ActorShape^>*)shape.hitShape->userData);
			hit.InternalFaceID = shape.internalFaceID;
			hit.Normal = TOVECTOR3( shape.normal );
			hit.Point = TOVECTOR3(shape.normal);
			hit.SweepShape = *((gcroot<ActorShape^>*)shape.sweepShape->userData);
			hit.T= shape.t;
		 }

		 return hit;
	 }

	 int _XActor::LinearSweep(Vector3 motion, SweepFlags flags, ICollection<SweepQueryHit>^ collection, SweepCache^ cache )
	 {
		  NxSweepCache* ncache;

		 if(cache!=nullptr)
			 ncache = static_cast<XSweepCache^>(cache)->cache;

		 NxSweepQueryHit shape;
		 UserCollectionReport cp;
		 cp.handle = collection;
		 cp.count = 0;

		 int count = actor->linearSweep(TONXVEC3(motion), static_cast<NxU32>( flags ),NULL, 1, &shape, &cp, ncache );

		 return cp.count;
	 }
}}