#include "../Include/PixelShader.fxh"
#include "../Include/VertexShader.fxh"

int DESCRIPTION
<
	string ShaderModel = "3.0";
	string Version = "1.0.0";
	int NumberOfLights = 0;		
	bool SupportAmbient = true;	
	bool SupportHemispherical = true;
	bool SupportSurfaceInfo = true;			
>;

int vsIndex;

VertexShader vertexShaders[2] = {
									compile vs_1_1 DeferredVS(),
									compile vs_1_1 BumpDeferredVS()
								};			

technique GetBufferData
{
	pass Default
	{			
		 //Direct color rendering
		 VertexShader = vertexShaders[vsIndex];
		 PixelShader  =	compile ps_2_0 DeferredMRTPS();
	}
	
	pass EnvMap
	{
		//environment map rendering
		 ALPHABLENDENABLE = TRUE;
		 SRCBLEND = ONE;
		 DESTBLEND = ONE;
		 
		 VertexShader = compile vs_1_1 SampleEnvironmentMapVS();
		 PixelShader  =	compile ps_2_0 DeferredSampleEnvironmentMapPS();
	}
	
	pass RefleMap
	{
		ALPHABLENDENABLE = TRUE;
		SRCBLEND = ONE;
		DESTBLEND = ONE;
		
		VertexShader = compile vs_1_1 SampleReflectionMapVS();
		PixelShader  =	compile ps_2_0 DeferredSampleReflecionMapPS();
	}
}