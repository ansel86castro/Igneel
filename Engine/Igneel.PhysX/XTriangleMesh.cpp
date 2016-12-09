#include "Stdafx.h"
#include "XTriangleMesh.h"
#include "Stream.h"
#include "PhysicManager.h"

namespace Igneel { namespace PhysX {

	XTriangleMesh::XTriangleMesh(NxTriangleMesh* nxMesh)
	{
		this->m_mesh = nxMesh;		

		int size = nxMesh->getCount(0, NxInternalArray::NX_ARRAY_NORMALS)  * sizeof(Vector3) +
			nxMesh->getCount(0, NxInternalArray::NX_ARRAY_VERTICES) *  sizeof(Vector3) +
			nxMesh->getCount(0, NxInternalArray::NX_ARRAY_TRIANGLES) * sizeof(short) + 
			nxMesh->getCount(0, NxInternalArray::NX_ARRAY_HULL_POLYGONS) +
			nxMesh->getCount(0, NxInternalArray::NX_ARRAY_HULL_VERTICES) * sizeof(Vector3);

		GC::AddMemoryPressure(size + sizeof(NxTriangleMesh));
	}

	void XTriangleMesh::Load(Stream^ stream)
	{
		BinaryReader^ reader = gcnew BinaryReader(stream);
		BinaryWriter^ writter = gcnew BinaryWriter(stream);
		WrapperStream ws = WrapperStream(reader, writter);

		try
		{
		m_mesh->load(ws);

		}
		finally
		{
			stream->Close();
		}
	}

	void XTriangleMesh::OnDispose(bool d)
	{
		if(m_mesh)
		{
			auto physic =  static_cast<PXPhysicManager^>(PhysicManager::Sigleton);
			physic->sdk->releaseTriangleMesh(*m_mesh);
			m_mesh = NULL;
		}
		__super::OnDispose(d);
	}
}}