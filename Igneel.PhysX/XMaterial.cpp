#include "Stdafx.h"
#include "XMaterial.h"

namespace Igneel { namespace PhysX {

	_XMaterial::_XMaterial(XScene^ scene, PhysicMaterialDesc^ desc)
	{
		NxMaterialDesc md;
		md.dirOfAnisotropy = ToNxVector3(desc->DirOfAnisotropy);
		md.dynamicFriction =desc->DynamicFriction;
		md.dynamicFrictionV = desc->DynamicFrictionV;
		md.flags = static_cast<NxU32>(desc->Flags);
		md.frictionCombineMode= static_cast<NxCombineMode>(desc->FrictionCombineMode);
		md.restitutionCombineMode= static_cast<NxCombineMode>(desc->RestitutionCombineMode);
		md.restitution = desc->Restitution;
		md.staticFriction = desc->StaticFriction;
		md.staticFrictionV = desc->StaticFrictionV;		

		if(!md.isValid())
			throw gcnew InvalidOperationException(L"Invalid Material");

		mat = scene->scene->createMaterial(md);
		this->scene = scene;
	}

	_XMaterial::_XMaterial(XScene^ scene, NxMaterial* mat)
	{
		this->mat = mat;
		this->scene = scene;
	}

	int _XMaterial::Index::get(){ return mat->getMaterialIndex(); }	

	float _XMaterial::DynamicFriction::get(){ return mat->getDynamicFriction(); }
	void _XMaterial::DynamicFriction::set(float value){ return mat->setDynamicFriction(value); }

	float _XMaterial::DynamicFrictionV::get(){ return mat->getDynamicFrictionV(); }
	void _XMaterial::DynamicFrictionV::set(float value){ return mat->setDynamicFrictionV(value); }

	float _XMaterial::StaticFriction::get(){ return mat->getStaticFriction(); }
	void _XMaterial::StaticFriction::set(float value){ return mat->setStaticFriction(value); }

	float _XMaterial::StaticFrictionV::get(){ return mat->getStaticFrictionV(); }
	void _XMaterial::StaticFrictionV::set(float value){ return mat->setStaticFrictionV(value); }

	float _XMaterial::Restitution::get(){ return mat->getRestitution(); }
	void _XMaterial::Restitution::set(float value){ return mat->setRestitution(value); }

	Vector3 _XMaterial::DirOfAnisotropy::get(){ return TOVECTOR3(mat->getDirOfAnisotropy()); }
	void _XMaterial::DirOfAnisotropy::set(Vector3 value){ return mat->setDirOfAnisotropy(TONXVEC3( value) ); }

	MaterialFlag _XMaterial::Flags::get(){ return static_cast<MaterialFlag>(mat->getFlags()); }
	void _XMaterial::Flags::set(MaterialFlag value){ return mat->setFlags(static_cast<NxU32>( value) ); }

	CombineMode _XMaterial::FrictionCombineMode::get(){ return static_cast<CombineMode>(mat->getFrictionCombineMode()); }
	void _XMaterial::FrictionCombineMode::set(CombineMode value){ return mat->setFrictionCombineMode(static_cast<NxCombineMode>( value) ); }

	CombineMode _XMaterial::RestitutionCombineMode::get(){ return static_cast<CombineMode>(mat->getRestitutionCombineMode()); }
	void _XMaterial::RestitutionCombineMode::set(CombineMode value){ return mat->setRestitutionCombineMode(static_cast<NxCombineMode>( value) ); }

	void _XMaterial::OnDispose(bool d)
	{
		if(d)
		{
			if(mat)
			{
				static_cast<XScene^>(scene)->scene->releaseMaterial(*mat);
				mat = NULL;
			}

		}
		__super::OnDispose(d);
	}
}}