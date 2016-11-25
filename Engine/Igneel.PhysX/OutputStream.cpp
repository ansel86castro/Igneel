#include "stdafx.h"
#include "OutputStream.h"

namespace Igneel{
	namespace PhysX{

		void MyUserOutputStream::reportError(NxErrorCode code, const char *message, const char* file, int line)
		{
			/*if (code < NXE_DB_INFO)
			{
			 	Physics::reportError((PhysXErrorCode)code,message,file,line);
			}*/
		}

		NxAssertResponse MyUserOutputStream::reportAssertViolation (const char *message, const char *file,int line)
		{        	
			return NxAssertResponse::NX_AR_BREAKPOINT;	//Physics::reportAssertViolation(message,file,line);
		}

		void MyUserOutputStream::print(const char* message)
		{
			
			//Physics::print(message);
		}

	}
}