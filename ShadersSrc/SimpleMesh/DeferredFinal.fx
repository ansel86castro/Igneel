#include "../Include/PixelShader.fxh"

int DESCRIPTION
<
	string ShaderModel = "3.0";
	string Version = "1.0.0";
	int NumberOfLights = 1;		
	bool SupportAmbient = true;	
	bool SupportHemispherical = true;
	bool SupportSurfaceInfo = true;			
>;

bool hemisphere;

technique Default
{
	pass Ambient
	{		
		 ALPHABLENDENABLE = FALSE;
		 //SRCBLEND = ONE;
		 //DESTBLEND = ONE;	
		 PixelShader  =	compile ps_2_0 DeferredAmbient(hemisphere);
	}
	pass Direct
	{
		ALPHABLENDENABLE = TRUE;
		SRCBLEND = ONE;
		DESTBLEND = ONE;	
		PixelShader  =	compile ps_3_0 DeferredLigthing();
	}
}

technique GlobalAmbient
{
	pass Ambient
	{		
		 ALPHABLENDENABLE = FALSE;
		 //SRCBLEND = ONE;
		 //DESTBLEND = ONE;	
		 PixelShader  =	compile ps_2_0 DeferredAmbient(hemisphere);
	}
	pass Direct
	{
		ALPHABLENDENABLE = TRUE;
		SRCBLEND = ONE;
		DESTBLEND = ONE;	
		PixelShader  =	compile ps_3_0 DeferredLigthing();
	}
}