#pragma once
#include "Common.h"

using namespace System::ComponentModel;
using namespace System;

using namespace Igneel::Physics;

namespace Igneel { namespace PhysX {

	public ref class XShape:public ResourceAllocator, ActorShape
	{
		static int idCounter;

	internal:
		PhysicMaterial^ material;
		NxShape* shape;
		int id;
		Actor^ actor;

	public:
		XShape(NxShape * shape, Actor^ actor);

		virtual property String^ Name;		

		virtual property Object^ UserData;

		virtual property  Igneel::Physics::Actor^ Actor{
			Igneel::Physics::Actor^ get(){ return actor; }
		}

		property int Id{ virtual int get() { return id;}}

		virtual bool GetFlag(ShapeFlag f){ return shape->getFlag(static_cast<NxShapeFlag>(f));}

		virtual void SetFlag(ShapeFlag f, bool value){ shape->setFlag(static_cast<NxShapeFlag>(f), value); }

		virtual property PhysicMaterial^ Material{
			PhysicMaterial^ get(){  return material; }
			void set(PhysicMaterial^ value){
				material = value;
				shape->setMaterial(value->Index);
			}
		}

		virtual property float SkinWidth{
				float get(){ return shape->getSkinWidth(); }
				void set(float v){ shape->setSkinWidth(v);}
			}

        virtual property Matrix LocalPose
		{
			Matrix get(){ return ToDxMatrix(shape->getLocalPose());}
			void set(Matrix v){									
				shape->setLocalPose(ToNxMat34(&v));
			}
		}

		virtual property Vector3 LocalPosition
			{
				Vector3 get()
				{
					NxVec3 v = shape->getLocalPosition();
					return TOVECTOR3(v);
				}
				void set(Vector3 v)
				{
				   shape->setLocalPosition(TONXVEC3(v)); 		   
				}
			}

		virtual property Matrix LocalRotation
			{
				virtual Matrix get(){ 
					return ToDxMatrix(shape->getLocalOrientation()); 
				}
				virtual void set(Matrix v){
					shape->setLocalOrientation(ToNxMat33(&v));
				}
			}
      
        virtual property Matrix GlobalPose
			{
				virtual Matrix get(){ return ToDxMatrix(shape->getGlobalPose());}
				virtual void set(Matrix v){ shape->setGlobalPose(ToNxMat34(&v));}

			}

        virtual property Vector3 GlobalPosition
			{
				virtual Vector3 get(){ 
					NxVec3 v = shape->getGlobalPosition();
					return TOVECTOR3(v);
				}
				virtual void set(Vector3 v){
					shape->setGlobalPosition(TONXVEC3(v));
				}
			}

        virtual property Matrix GlobalOrientation
			{
				Matrix get(){return ToDxMatrix(shape->getGlobalOrientation()); }
				void set(Matrix v){					
					shape->setLocalOrientation(ToNxMat33(&v));
				}
			}

        virtual property NxCollisionGroup CollisionGroup{
				NxCollisionGroup get() { return shape->getGroup(); }
				void set(NxCollisionGroup v){ shape->setGroup(v); }
			}
		virtual property GroupsMask CollisionGroupMask{
				GroupsMask get() 
				{
					auto mask = shape->getGroupsMask(); 
					return *(GroupsMask*)&mask;
				}
				void set(GroupsMask v)
				{
					shape->setGroupsMask(*(NxGroupsMask*)&v); 
				}
			}

        virtual property AABB WorldBounds
			{
				AABB get()
				{ 					
					NxBounds3 bound;
					shape->getWorldBounds(bound);
					return *(AABB*)&bound;					
				} 				
			}
		

        virtual bool CheckOverlapAABB(Bounds3 worldBounds)
			{
				return shape->checkOverlapAABB(*(NxBounds3*)&worldBounds);
			}

        virtual bool CheckOverlapOBB(Box worldBounds)
			{				
				NxBox box = NxBox(TONXVEC3(worldBounds.Translation),TONXVEC3(worldBounds.Extends), ToNxMat33(&worldBounds.Rotation));
				return shape->checkOverlapOBB(box);				
			}

        virtual bool CheckOverlapSphere(Sphere sphere)
			{								
				return shape->checkOverlapSphere(*(NxSphere*)&sphere);
			}

        virtual bool CheckOverlapCapsule(Capsule capsule)
			{				
				return shape->checkOverlapCapsule(*(NxCapsule*)&capsule);
			}       
        
		virtual void CommitChanges() = IDeferreable::CommitChanges{  }			

		virtual String^ ToString()override;
		

	protected:
		virtual void OnDispose(bool) override;
		
		virtual void UserDataChanging();

	};

	public ref class XPlaneShape:public XShape, PlaneShape
	{
	internal:
		XPlaneShape(NxPlaneShape* nxPlane, Igneel::Physics::Actor^ a): XShape(nxPlane, a)
		{
			GC::AddMemoryPressure(sizeof(NxPlaneShape));
		}			

	public:
		virtual property Plane Plane
		{
			Igneel::Plane get(){
				NxPlane nxPlane =  ((NxPlaneShape*)shape)->getPlane();
				return *((Igneel::Plane*)(&nxPlane));
			}
			void set(Igneel::Plane v){
				((NxPlaneShape*)shape)->setPlane(TONXVEC3(v.Normal),v.D);
			}
		}
	};

	public ref class XBoxShape:public XShape, BoxShape
	{
		internal:
			XBoxShape(NxBoxShape* nxBox, Igneel::Physics::Actor^ a):XShape(nxBox , a)
			{ 
				GC::AddMemoryPressure(sizeof(NxBoxShape));
			}			

		public:
			/// <summary>
			/// brief Retrieves the dimensions of the box.
			/// The dimensions are the 'radii' of the box, meaning 1/2 extents in x dimension, 
			/// 1/2 extents in y dimension, 1/2 extents in z dimension.
			/// The 'radii' of the box.
			/// </summary>		
			virtual property Vector3 Dimensions
			{
				Vector3 get()
				{ 
					NxVec3 v = ((NxBoxShape*)shape)->getDimensions();
					return TOVECTOR3(v);
				}
				void set(Vector3 v)
				{
					((NxBoxShape*)shape)->setDimensions(TONXVEC3(v));
				}
			}
			
	};

	public ref class XSphereShape:public XShape, SphereShape
		{
		internal:
			XSphereShape(NxSphereShape* nxSphere,Igneel::Physics::Actor^ a): XShape(nxSphere, a)
			{
				GC::AddMemoryPressure(sizeof(NxSphereShape));
			}			

		public:		
			virtual property float Radius
			{
				float get(){ return ((NxSphereShape*)shape)->getRadius();}
				void set(float v) { ((NxSphereShape*)shape)->setRadius(v); }
			}
	};

	public ref class XCapsuleShape: public XShape, CapsuleShape
		{
		internal:
			XCapsuleShape(NxCapsuleShape * nxCapsule, Igneel::Physics::Actor^ a):XShape(nxCapsule, a)
			{ 
				GC::AddMemoryPressure(sizeof(NxCapsuleShape));
			}			

		public:
			virtual property float Radius
			{
				float get(){ return ((NxCapsuleShape*)shape)->getRadius(); } 
				void set(float v){ ((NxCapsuleShape*)shape)->setRadius(v); }
			}

			virtual property float Height
			{
				float get(){ return ((NxCapsuleShape*)shape)->getHeight(); } 
				void set(float v){ ((NxCapsuleShape*)shape)->setHeight(v); }
			}
	};


	public ref class XWheelShape:public XShape, WheelShape
		{		
					
		internal:
			XWheelShape(NxWheelShape* nxwheel, Igneel::Physics::Actor^ a)
				:XShape(nxwheel, a)
			{				
				GC::AddMemoryPressure(sizeof(NxWheelShape));
			}					

		public:					
			virtual property float Radius
			{
				float get(){return  CAST(NxWheelShape,shape)->getRadius(); }
				void set(float v){ CAST(NxWheelShape,shape)->setRadius (v);}
			}

			
			virtual property float SuspensionTravel
			{
				float get(){return  CAST(NxWheelShape,shape)->getSuspensionTravel(); }
				void set(float v){ CAST(NxWheelShape,shape)->setSuspensionTravel(v);}
			}

			
			virtual property float InverseWheelMass
			{
				float get(){return  CAST(NxWheelShape,shape)->getInverseWheelMass(); }
				void set(float v){ CAST(NxWheelShape,shape)->setInverseWheelMass(v);}
			}

			virtual property WheelShapeFlags WheelFlags
			{
				WheelShapeFlags get(){return static_cast<WheelShapeFlags>(CAST(NxWheelShape,shape)->getWheelFlags());}
				void set(WheelShapeFlags v) { CAST(NxWheelShape,shape)->setWheelFlags(static_cast<NxU32>(v));}
			}

			
			virtual property float MotorTorque
			{
				float get()
				{ 
					return CAST(NxWheelShape,shape)->getMotorTorque();
				}
				void set(float v)
				{ 
					CAST(NxWheelShape,shape)->setMotorTorque(v);
				}
			}

			
			virtual property float BrakeTorque
			{
				float get()
				{
					return CAST(NxWheelShape,shape)->getBrakeTorque(); }

				void set(float v)
				{ 
					CAST(NxWheelShape,shape)->setBrakeTorque(v); 
				}
			}

			/// <summary>
			/// steering angle around Y axis in radians
			/// </summary>
			
			virtual property float SteerAngle
			{
				float get(){ return CAST(NxWheelShape,shape)->getSteerAngle();}
				void set(float v){ CAST(NxWheelShape,shape)->setSteerAngle(v); }
			}

			
			virtual property float AxleSpeed
			{
				float get()
				{
					return CAST(NxWheelShape,shape)->getAxleSpeed(); 
				}
				void set(float v)
				{
					CAST(NxWheelShape,shape)->setAxleSpeed(v);
				}

			}

			virtual property SpringDesc Suspension
			{
				SpringDesc get()
				{			
					NxSpringDesc s = CAST(NxWheelShape,shape)->getSuspension();
					return *(SpringDesc*)&s;
				}
				void set(SpringDesc v)
				{
					CAST(NxWheelShape,shape)->setSuspension(*(NxSpringDesc*)&v);
				}
			}

			virtual property TireFunctionDesc LongitudalTireForceFunction
			{
				TireFunctionDesc get()
				{
					auto v = CAST(NxWheelShape, shape)->getLongitudalTireForceFunction();
					return *(TireFunctionDesc*)&v;
				}

				void set(TireFunctionDesc v)
				{
					CAST(NxWheelShape,shape)->setLongitudalTireForceFunction(*(NxTireFunctionDesc*)&v);
				}
			}

			virtual property TireFunctionDesc LateralTireForceFunction
			{
				TireFunctionDesc get()
				{					
					auto v = CAST(NxWheelShape, shape)->getLateralTireForceFunction();
					return *(TireFunctionDesc*)&v;
				}

				void set(TireFunctionDesc v)
				{
					CAST(NxWheelShape,shape)->setLateralTireForceFunction(*(NxTireFunctionDesc*)&v);
				}
			}

			 virtual WheelContact GetContact();
		};

		public ref class XTriangleMeshShape: public XShape, TriangleMeshShape
		{	
			TriangleMesh^ mesh;
		internal:
			XTriangleMeshShape(NxTriangleMeshShape* nxtriangleMesh, Igneel::Physics::Actor^a ):XShape(nxtriangleMesh, a)
			{ 
				GC::AddMemoryPressure(sizeof(NxTriangleMeshShape));
			}			

		public:
			virtual property TriangleMesh^ Mesh
			{
				TriangleMesh^ get(){ return mesh; }
			}

		protected:
			virtual void UserDataChanging()override;
		};
}}