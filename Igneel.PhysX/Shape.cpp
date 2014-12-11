#include "Stdafx.h"
#include "Shape.h"
#include "XActor.h"

namespace Igneel { namespace PhysX {

	XShape::XShape(NxShape * shape, Igneel::Physics::Actor^ actor)
	{
		this->shape = shape;
		this->actor = actor;				
		this->material  = actor->Scene->Materials[shape->getMaterial()];
		this->id = ++idCounter;

		UserDataChanging();

		gcroot<ActorShapeDesc^>* handle = (gcroot<ActorShapeDesc^>*)shape->userData;
		if(handle)
		{
			Name = (*handle)->Name;
			delete handle;
		}
		else
		{
			Name = L"S"+actor->Shapes->Count;
		}
		shape->userData = new gcroot<ActorShape^>(this);
	}

	void XShape::UserDataChanging(){ }

	void XShape::OnDispose(bool d)
	{
		if(shape)
		{
			_XActor^ _actor = (_XActor^)actor;				
			gcroot<ActorShape^>* ref = (gcroot<ActorShape^>*)shape->userData;
			if(_actor!=nullptr && _actor->actor)
				_actor->actor->releaseShape(*shape);
			shape = NULL;
			delete ref;
		}
		__super::OnDispose(d);
	}

	String^ XShape::ToString()
	{
		return (Name!=nullptr)?Name:__super::ToString();
	}

	WheelContact XWheelShape::GetContact()
	{
		WheelContact data;
		data.Shape = nullptr;

		NxWheelShape * pWheel =	CAST(NxWheelShape, shape);
		NxWheelContactData nativeData;
		NxShape* nxShape = pWheel->getContact(nativeData);		
		if(nxShape)
		{
			memcpy(&data, &nativeData, sizeof(NxWheelContactData));		
			data.Shape = *((gcroot<ActorShape^>*)nxShape->userData);
		}
		
		return data;
	}

	void XTriangleMeshShape::UserDataChanging()
	{
		gcroot<TriangleMeshShapeDesc^>* handle = (gcroot<TriangleMeshShapeDesc^>*)shape->userData;
		mesh = (*handle)->Mesh;		
	}
}}