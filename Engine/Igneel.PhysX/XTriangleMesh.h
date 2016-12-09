#pragma once
#include "Common.h"


using namespace System;
using namespace System::IO;
using namespace Igneel::Physics;

namespace Igneel { namespace PhysX {

	public ref class XTriangleMesh: public TriangleMesh
	{
	internal:
		NxTriangleMesh* m_mesh;

		public:
			XTriangleMesh(NxTriangleMesh* nxMesh);

			virtual property int PagesCount 
			{ 
				int	get() override { return m_mesh->getPageCount(); }
			}

			virtual property int SubMeshCount
			{
				int get() override {  return m_mesh->getSubmeshCount(); }
			}

			virtual property int ReferenceCount
			{
				int get()override { return m_mesh->getReferenceCount(); }
			}

		    virtual int GetCount(InternalArray arrayType) override
			{
				return m_mesh->getCount(0, (NxInternalArray)arrayType);
			}

			virtual InternalFormat GetFormat(InternalArray arrayType)override
			{
				return (InternalFormat)m_mesh->getFormat(0,(NxInternalArray)arrayType);
			}

			virtual IntPtr GetBase(InternalArray arrayType) override
			{
				const void* pter = m_mesh->getBase(0, (NxInternalArray)arrayType);

				return static_cast<IntPtr>((void*)pter);
			}

			virtual int GetStride(InternalArray arrayType) override
			{
				return m_mesh->getStride(0,(NxInternalArray)arrayType);
			}
						
			virtual int GetTriangleMaterial(int triangleIndex) override
			{
				return m_mesh->getTriangleMaterial(triangleIndex);
			}

			virtual void Load(Stream^ stream)override;

			virtual void OnDispose(bool) override;

	};

}}