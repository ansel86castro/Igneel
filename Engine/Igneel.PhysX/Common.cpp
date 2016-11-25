#include "Stdafx.h"
#include "Common.h"

namespace Igneel { namespace PhysX {

		NxVec3 ToNxVector3(Vector3 v)
		{
			return TONXVEC3(v);
		}
		NxMat34 ToNxMat34(Matrix* dxMat)
		{			
			NxMat34 nxMat(false);	
			nxMat.M.setColumnMajorStride4((float*)dxMat);
			nxMat.t = *(NxVec3*)(((float*)dxMat) + 12);
			return nxMat;
		}

		NxMat33 ToNxMat33(Matrix* dxMat)
		{
			NxMat33 nxMat(NxMatrixType::NX_IDENTITY_MATRIX);		
			nxMat.setColumnMajorStride4((float*)dxMat);
			return nxMat;
		}

		NxMat34 ToNxMat34(Matrix dxMat)
		{
			return ToNxMat34(&dxMat);
		}

		NxMat33 ToNxMat33(Matrix dxMat)
		{
			return ToNxMat33(&dxMat);
		}

		NxMat33 ToNxMat33(Matrix& dxMat)
		{
			NxMat33 nxMat(NxMatrixType::NX_IDENTITY_MATRIX);		
			nxMat.setColumnMajorStride4((float*)&dxMat);
			return nxMat;
		}

		Matrix ToDxMatrix(NxMat34& nxMat)
		{
			Matrix m;						
			nxMat.getColumnMajor44((float*)&m);
			return m;
		}

		Matrix ToDxMatrix(NxMat33& nxMat){
			Matrix m;				
			nxMat.getColumnMajorStride4((float *)&m);
			return m;
		}


}}