#pragma once
#include "Common.h"

using namespace System;
using namespace System::ComponentModel;
using namespace Igneel::Design;
using namespace Igneel::Physics;
using namespace System::Runtime::InteropServices;
using namespace Igneel;

namespace Igneel { namespace PhysX {

	public ref class _XActor sealed: public Actor
	{
	internal:
		NxActor* actor;
	internal :
		void SetDispose();
	public:
			_XActor(Physic^ scene, ActorDesc^ desc);

			_XActor(Physic^ scene, NxActor* a);

			void Init();

			virtual property float Mass
			{
				float get() override { return actor->getMass();}
				void set(float v) override { actor->setMass(v); }
			}

			[Category(L"Velocity")]		
			[PropertyConstrain(L"IsDynamic")]
			virtual property Vector3 LinearVelocity{
				Vector3 get() override
				{
					return TOVECTOR3(actor->getLinearVelocity());
				}
				void set(Vector3 v) override
				{					
					actor->setLinearVelocity(TONXVEC3(v)); 
				} 
			}

			[Category(L"Damping")]			
			[PropertyConstrain(L"IsDynamic")]
			virtual property float LinearDamping{
				float get() override{ return actor->getLinearDamping();}
				void set(float v) override{ actor->setLinearDamping(v);}
			}

			[Category(L"Velocity")]		
			[PropertyConstrain(L"IsDynamic")]
			virtual property Vector3 AngularVelocity{
				Vector3 get() override{ return TOVECTOR3(actor->getAngularVelocity());}
				void set(Vector3 v) override{ actor->setAngularVelocity(TONXVEC3(v)); } 
			}

			[Category(L"Damping")]
			
			[PropertyConstrain(L"IsDynamic")]
			virtual property float AngularDamping{
				float get() override{ return  actor->getAngularDamping();}
				void set(float v) override{  actor->setAngularDamping(v);}
			}

			[Category(L"Velocity")]
			
			[PropertyConstrain(L"IsDynamic")]
			virtual property float MaxAngularVelocity
			{
				float get() override{ return actor->getMaxAngularVelocity();}
				void set(float v) override{ actor->setMaxAngularVelocity(v);}
			}

			[Category(L"Transform")]			
			[Deferred]
			[LockOnSet]
			virtual property Vector3 GlobalPosition
			{
				virtual Vector3 get() override{
					NxVec3 v = actor->getGlobalPosition();
					return TOVECTOR3(v);
				}
				virtual void set(Vector3 v) override{
					actor->setGlobalPosition(TONXVEC3(v));
				}
			}

			[Category(L"Transform")]
			//[EditorAttribute(UIRotationMatrixEditor::typeid,UITypeEditor::typeid)]
			[Deferred]
			[LockOnSet]
			virtual property Matrix GlobalOrientation
			{
				Matrix get() override{ 
					return ToDxMatrix(actor->getGlobalOrientation()); 
				}
				void set(Matrix v) override {
					actor->setGlobalOrientation(ToNxMat33(&v));
				}
			}

			[Category(L"Transform")]
			[Browsable(false)]
			virtual property Quaternion GlobalOrientationQuat{
				Quaternion get() override{ 
					NxQuat q =  actor->getGlobalOrientationQuat();
					return Quaternion(q.x,q.y,q.z,q.w);
				}
				void set(Quaternion v) override {
					NxQuat q;
					q.x= v.X;q.y=v.Y,q.z=v.Z; q.w = v.W;
					actor->setGlobalOrientationQuat(q);
				}
			}

			virtual property Matrix GlobalPose{
				Matrix get() override {  return ToDxMatrix(actor->getGlobalPose()); }
			}

			[Category(L"Mass")]
			[Browsable(false)]
			[Deferred]
			[LockOnSet]
			[PropertyConstrain(L"IsDynamic")]
			virtual property Matrix CMassGlobalPose
			{
				Matrix get() override{ return ToDxMatrix(actor->getCMassGlobalPose()); }
				void set(Matrix v) override { actor->setCMassGlobalPose(ToNxMat34(v)); }
			}

			[Category(L"Mass")]
						
			[Deferred]
			[LockOnSet]
			[PropertyConstrain(L"IsDynamic")]
			virtual property Vector3 CMassGlobalPosition{
				Vector3 get() override{ return TOVECTOR3(actor->getCMassGlobalPosition()); }
				void set(Vector3 v) override { actor->setCMassGlobalPosition(TONXVEC3(v));}
			}

			[Category(L"Mass")]
			//[EditorAttribute(UIRotationMatrixEditor::typeid,UITypeEditor::typeid)] 		
			[Deferred]
			[LockOnSet]
			[PropertyConstrain(L"IsDynamic")]
			virtual property Matrix CMassGlobalOrientation
			{
				Matrix get() override{ return ToDxMatrix(actor->getCMassGlobalOrientation());}
				void set(Matrix v) override { actor->setCMassGlobalOrientation(ToNxMat33(v));}
			}

			[Browsable(false)]
			[Category(L"Mass")]			
			[PropertyConstrain(L"IsDynamic")]			
			virtual property Matrix CMassLocalPose
			{
				Matrix get() override{ return ToDxMatrix(actor->getCMassLocalPose()); }				

			}

			[Category(L"Mass")]			
			[Deferred]
			[LockOnSet]
			[PropertyConstrain(L"IsDynamic")]
			virtual property Vector3 MassSpaceInertiaTensor{
				Vector3 get() override{ return TOVECTOR3(actor->getMassSpaceInertiaTensor());}
				void set(Vector3 v) override{ actor->setMassSpaceInertiaTensor(TONXVEC3(v)); }
			}

			[Category(L"Actor")]
			virtual property bool IsKinematic{
				bool get() override{ return isDynamic ? actor->readBodyFlag(NX_BF_KINEMATIC): false;}
				void set(bool v) override
				{
					if(isDynamic)
					{
						if(v)
							actor->raiseBodyFlag(NX_BF_KINEMATIC);
						else
							actor->clearBodyFlag(NX_BF_KINEMATIC);
					}
				}
			}

			[Category(L"Sleep")]
			[PropertyConstrain(L"IsDynamic")]
			virtual property bool IsSleeping
			{
				bool get() override{ return actor->isSleeping();}				
			} 

			[Category(L"Sleep")]
			[PropertyConstrain(L"IsDynamic")]
			virtual property float SleepEnergyThreshold
			{
				float get() override{ return  actor->getSleepEnergyThreshold(); }
				void set(float v) override { actor->setSleepEnergyThreshold(v); } 
			}

			[Category(L"Sleep")]
			[PropertyConstrain(L"IsDynamic")]
			virtual property float SleepAngularVelocity
			{
				float get() override{ return  actor->getSleepAngularVelocity(); }
				void set(float v) override { actor->setSleepAngularVelocity(v); } 
			}
			
			[Category(L"Sleep")]
			[PropertyConstrain(L"IsDynamic")]
			virtual property float SleepLinearVelocity
			{
				float get() override{ return  actor->getSleepLinearVelocity(); }
				void set(float v) override { actor->setSleepLinearVelocity(v); } 
			}

			[Category(L"Sleep")]
			[PropertyConstrain(L"IsDynamic")]
			virtual property bool IsGroupSleeping{
				bool get() override{ return actor->isGroupSleeping();}
			}			
			

			[PropertyConstrain(L"IsDynamic")]
			virtual property int SolverIterationCount
			{
				int get() override{ return  actor->getSolverIterationCount(); }
				void set(int v) override { actor->setSolverIterationCount(v); } 
			}

			virtual property UInt16 Group
			{
				UInt16 get() override{ return actor->getGroup();}
				void set(UInt16 v) override { actor->setGroup(v);}
			}

			virtual property UInt16 DominanceGroup
			{
				UInt16 get() override{ return actor->getDominanceGroup();}
				void set(UInt16 v) override { actor->setDominanceGroup(v); }
			}

			[PropertyConstrain(L"IsDynamic")]
			virtual property NxReal CCDMotionThreshold{
				NxReal get() override{ return actor->getCCDMotionThreshold();}
				void set(NxReal v) override { actor->setCCDMotionThreshold(v);}
			}

			[PropertyConstrain(L"IsDynamic")]
			virtual property Vector3 LinearMomentum{
				Vector3 get() override{ return TOVECTOR3(actor->getLinearMomentum());}
				void set(Vector3 v) override{ actor->setLinearMomentum(TONXVEC3(v));}
			}
			
			[PropertyConstrain(L"IsDynamic")]
			virtual property Vector3 AngularMomentum{
				Vector3 get() override{ return TOVECTOR3(actor->getAngularMomentum());}
				void set(Vector3 v) override{ actor->setAngularMomentum(TONXVEC3(v));}
			}		
						
			[PropertyConstrain(L"IsDynamic")]
			virtual property NxReal ContactReportThreshold{
				NxReal get() override{ return actor->getContactReportThreshold();}
				void set(NxReal v) override { actor->setContactReportThreshold(v);}
			}

			virtual property ContactPairFlag ContactReportFlags
			{
				ContactPairFlag get() override{ return static_cast<ContactPairFlag>(actor->getContactReportFlags()); }
				void set(ContactPairFlag v) override  { actor->setContactReportFlags(static_cast<NxU32>(v)); }
			}

			virtual void MoveGlobalPose(Matrix pose)override
			{
				actor->moveGlobalPose(ToNxMat34(&pose));
			}
			
			virtual void MoveGlobalOrientation(Matrix rotationMat)override
			{
				actor->moveGlobalOrientation(ToNxMat33(&rotationMat));
			}

			virtual void MoveGlobalPosition(Vector3 position)override
			{
				actor->moveGlobalPosition(TONXVEC3(position));
			}
			
			virtual void MoveGlobalOrientationQuat(Quaternion v)override
			{
				NxQuat q;
				q.x= v.X;q.y=v.Y,q.z=v.Z; q.w = v.W;
				actor->moveGlobalOrientationQuat(q);
			}

			virtual void RaiseActorFlag(ActorFlag flag) override
			{
				actor->raiseActorFlag((NxActorFlag)flag);
			}
			
			virtual bool ReadActorFlag(ActorFlag flag) override
			{
				return	actor->readActorFlag((NxActorFlag)flag);				
			}

			virtual void ClearActorFlag(ActorFlag flag) override
			{
				actor->clearActorFlag((NxActorFlag)flag);
			}
			
			virtual void RaiseBodyFlag(BodyActorFlags flag) override
			{				
				actor->raiseBodyFlag((NxBodyFlag)flag);
			}

			virtual bool ReadBodyFlag(BodyActorFlags flag) override
			{
				return actor->readBodyFlag((NxBodyFlag)flag);				
			}

			virtual void ClearBodyFlag(BodyActorFlags flag) override
			{
				actor->clearBodyFlag((NxBodyFlag)flag);
			}			

			virtual void AddForceAtPos(Vector3 force, Vector3 pos, ForceMode mode, bool wakeup)override
			{
				actor->addForceAtPos(TONXVEC3(force),TONXVEC3(pos),(NxForceMode)mode,wakeup);
			}

			virtual void AddForceAtLocalPos(Vector3 force, Vector3 pos, ForceMode mode, bool wakeup)override
			{
				actor->addForceAtLocalPos(TONXVEC3(force),TONXVEC3(pos),(NxForceMode)mode,wakeup);
			}
			
			virtual void AddLocalForceAtPos(Vector3 force, Vector3 pos, ForceMode mode , bool wakeup)override
			{
				return actor->addLocalForceAtPos(TONXVEC3(force),TONXVEC3(pos),(NxForceMode)mode,wakeup);
			}
			
			virtual void AddLocalForceAtLocalPos(Vector3 force, Vector3 pos, ForceMode mode , bool wakeup) override
			{
				return actor->addLocalForceAtLocalPos(TONXVEC3(force), TONXVEC3(pos), static_cast<NxForceMode>(mode), wakeup);
			}

			virtual void AddForce(Vector3 force, Vector3 pos, ForceMode mode , bool wakeup)override
			{
				actor->addForce(TONXVEC3(force),(NxForceMode)mode,wakeup);
			}

			virtual void AddLocalForce(Vector3 force, Vector3 pos, ForceMode mode , bool wakeup)override
			{
				actor->addLocalForce(TONXVEC3(force),(NxForceMode)mode,wakeup);
			}

			virtual void AddTorque(Vector3 torque, Vector3 pos, ForceMode mode , bool wakeup)override
			{
				actor->addTorque(TONXVEC3(torque),(NxForceMode)mode,wakeup);
			}

			virtual void AddLocalTorque(Vector3 torque, Vector3 pos, ForceMode mode , bool wakeup)override
			{
				actor->addLocalTorque(TONXVEC3(torque),(NxForceMode)mode,wakeup);
			}

			virtual NxReal	ComputeKineticEnergy()override
			{
				return actor->computeKineticEnergy();
			}

			virtual Vector3	GetPointVelocity(Vector3 point)override
			{
				return TOVECTOR3(actor->getPointVelocity(TONXVEC3(point)));
			}

			virtual Vector3	GetLocalPointVelocity(Vector3 point)override
			{
				return TOVECTOR3(actor->getLocalPointVelocity(TONXVEC3(point)));				
			}

			virtual void WakeUp(float wakeCounterValue)override
			{
				actor->wakeUp(wakeCounterValue);
			}

			virtual void PutToSleep()override
			{
				actor->putToSleep();
			}							

			virtual void ResetUserActorPairFiltering()override
			{
				actor->resetUserActorPairFiltering();
			}

			virtual void SetCMassOffsetLocalOrientation(Matrix mat) override {
				actor->setCMassOffsetLocalOrientation(ToNxMat33(mat));
			}

			virtual void SetCMassOffsetLocalPosition(Vector3 v)override{
				actor->setCMassOffsetLocalPosition(TONXVEC3(v));
			}

			virtual void SetCMassOffsetLocalPose(Matrix mat)override{
				actor->setCMassOffsetLocalPose(ToNxMat34(mat));
			}

			virtual void SetCMassOffsetGlobalPose(Matrix mat)override{
				actor->setCMassOffsetGlobalPose(ToNxMat34(mat));				
			}

			virtual void SetCMassOffsetGlobalPosition(Vector3 v)override{
				actor->setCMassOffsetGlobalPosition(TONXVEC3(v));
			}

			virtual void SetCMassOffsetGlobalOrientation(Matrix mat)override{
				actor->setCMassOffsetGlobalOrientation(ToNxMat33(mat));				
			}

			virtual  Matrix GetGlobalInertiaTensor()override{
				return ToDxMatrix(actor->getGlobalInertiaTensor());
			}

			virtual  Matrix GetGlobalInertiaTensorInverse() override{
				return ToDxMatrix(actor->getGlobalInertiaTensorInverse());
			}

			virtual void UpdateMassFromShapes(NxReal density, NxReal totalMass)override{
				actor->updateMassFromShapes(density, totalMass);
			}

		    virtual	SweepQueryHit LinearSweep(Vector3 motion, SweepFlags flags, Predicate<SweepQueryHit>^ callback , SweepCache^ cache) override;

			virtual int LinearSweep(Vector3 motion, SweepFlags flags, ICollection<SweepQueryHit>^ collection, SweepCache^ cache ) override;

		protected:
			virtual	ActorShape^ CreateShapeImp(ActorShapeDesc^ desc) override;

			virtual void OnDispose(bool d)override;

		public:
			NxShapeDesc* GetShapeDescription(ActorShapeDesc^ desc);

			NxActorDesc* GetActorDescription(ActorDesc^ desc);

			ActorShape^ GetShape(NxShape* shap);

	};

}}