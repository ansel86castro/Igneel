#pragma once

namespace Igneel{
	namespace PhysX{

ref class Physics;		

		class MyUserOutputStream:public NxUserOutputStream
		{	
		public:	

			MyUserOutputStream()
			{

			}		

			virtual void reportError(NxErrorCode code, const char *message, const char* file, int line) override;

			virtual NxAssertResponse reportAssertViolation (const char *message, const char *file,int line) override;

			virtual void print(const char *message) override;	
		};

	}
}

