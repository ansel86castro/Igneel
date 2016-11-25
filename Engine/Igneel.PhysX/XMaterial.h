#pragma once
#include "Common.h"
#include "XScene.h"

using namespace System;
using namespace Igneel::Physics;

namespace Igneel { namespace PhysX {

ref class XScene;

	typedef public ref class _XMaterial sealed: public PhysicMaterial
	{
		NxMaterial* mat;
	public:
		_XMaterial(XScene^ scene, PhysicMaterialDesc^ desc);

		_XMaterial(XScene^ scene, NxMaterial* mat);

		virtual property int Index { int get()override;}

		PROP(float, DynamicFriction)        

		PROP(float, DynamicFrictionV)

		PROP(float, StaticFriction)

		PROP(float, StaticFrictionV)

		PROP(float, Restitution)

		PROP(Vector3, DirOfAnisotropy)

		PROP(MaterialFlag, Flags)

		PROP(CombineMode, FrictionCombineMode)

		PROP(CombineMode, RestitutionCombineMode)      

	protected:
		OVERRIDE( void OnDispose(bool));

	} ^XMaterial;
}}