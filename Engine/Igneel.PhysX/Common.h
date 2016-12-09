#pragma once

using namespace Igneel;
using namespace System::Collections::Generic;

#define OVERRIDE(x) virtual x override

#define TOVECTOR3(v) *((Vector3*)(&v))

#define TONXVEC3(v) *((NxVec3*)(&v))

#define PROP(TYPE, NAME) property TYPE NAME { virtual TYPE get()override; virtual void set(TYPE value) override; }

#define CAST(T, O) ( static_cast<T*>(O))

namespace Igneel { namespace PhysX {
	
	NxVec3 ToNxVector3(Vector3 v);

	NxMat34 ToNxMat34(Matrix* dxMat);
	NxMat34 ToNxMat34(Matrix dxMat);

	NxMat33 ToNxMat33(Matrix* dxMat);		
	NxMat33 ToNxMat33(Matrix dxMat);

	Matrix ToDxMatrix(NxMat34& nxMat);	

	Matrix ToDxMatrix(NxMat33& nxMat);	

	

}}