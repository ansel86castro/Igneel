#include "Stdafx.h"
#include "PhysicManager.h"
#include "OutputStream.h"
#include "XScene.h"
#include "Stream.h"
#include "XTriangleMesh.h"
#include "XControllerManager.h"

namespace Igneel { namespace PhysX {

	PXPhysicManager::PXPhysicManager()
	{
		NxSDKCreateError error;			
		NxPhysicsSDKDesc desc = NxPhysicsSDKDesc();			
		MyUserOutputStream * m_outputStream = new MyUserOutputStream();

		//sdk = NxCreatePhysicsSDK(NX_PHYSICS_SDK_VERSION);
		sdk =  NxCreatePhysicsSDK(NX_PHYSICS_SDK_VERSION, NULL, m_outputStream, desc, &error);		
		if(!sdk)	
		{
			delete m_outputStream;
			throw gcnew InvalidOperationException(L"Invalid PhysX");
		}

		cooking = NxGetCookingLib(NX_PHYSICS_SDK_VERSION);
		cooking->NxInitCooking();

		GC::AddMemoryPressure(sizeof(NxPhysicsSDK));		
		GC::AddMemoryPressure(sizeof(NxCookingInterface));		
	}

	void PXPhysicManager::OnDispose(bool disposing)
	{
		__super::OnDispose(disposing);		
		if(cooking)
		{
			cooking->NxCloseCooking();
			cooking = NULL; 
		}
		if(sdk)
		{			
			NxReleasePhysicsSDK(sdk);
			sdk = NULL;			
		}		
	}

	Physic^ PXPhysicManager::_CreatePhysic(PhysicDesc^ desc)
	{
		return gcnew XScene(desc);
	}

	TriangleMesh^ PXPhysicManager::_CreateTriangleMesh(TriangleMeshDesc^ desc)
	{
		if(!cooking)
			throw gcnew NullReferenceException(L"cooking sdk null");
		
		NxTriangleMeshDesc md = NxTriangleMeshDesc();
		md.flags = static_cast<NxU32>( desc->Flags );
		md.numTriangles = desc->NumTriangles;
		md.numVertices = desc->NumVertices;
		md.points = desc->Points.ToPointer();
		md.pointStrideBytes = desc->PointStrideBytes;
		md.triangles = desc->Triangles.ToPointer();
		md.triangleStrideBytes = desc->TriangleStrideBytes;		

		if(!md.isValid())
			throw gcnew InvalidOperationException(L"Invalid Mesh");

		MemoryWriteBuffer writer;
		cooking->NxCookTriangleMesh(md, writer);		

		MemoryReadBuffer mb(writer.data);
		NxTriangleMesh* tmesh = sdk->createTriangleMesh(mb);

		auto triangleMesh = gcnew XTriangleMesh(tmesh);
		triangleMesh->Name = desc->Name;
		return triangleMesh;
	}

	TriangleMesh^ PXPhysicManager::_CreateTriangleMeshFromFile(String^ filename)
	{
		IntPtr pter  = Marshal::StringToHGlobalAnsi(filename);

		UserStream us((const char*)pter.ToPointer(),true);

		auto tmesh = sdk->createTriangleMesh(us);
		if(!tmesh)
			throw gcnew InvalidOperationException(L"Invalid Data");

		Marshal::FreeHGlobal(pter);

		auto triangleMesh = gcnew XTriangleMesh(tmesh);		
		return triangleMesh;
	}

	TriangleMesh^ PXPhysicManager::_CreateTriangleMeshFromStream(Stream^ stream)
	{
		BinaryReader^ reader = gcnew BinaryReader(stream);
		BinaryWriter^ wrtter = gcnew BinaryWriter(stream);

		WrapperStream ws(reader, wrtter);

		auto tmesh = sdk->createTriangleMesh(ws);
		if(!tmesh)
			throw gcnew InvalidOperationException(L"Invalid Data");

		stream->Close();

		return gcnew XTriangleMesh(tmesh);
	}

	CharacterControllerManager^ PXPhysicManager::CreateControllerManager()
	{
		return gcnew XControllerManager();
	}
}}