#define PIXEL_SHADER

#ifndef LIGTHING_SHADOWED
	#include "LigthingShadowed.fxh"
#endif


float4 PhongPixelShader(SimpleVSOutput input) : COLOR0
{   	
	clip(dot(float4(input.PositionW, 1), clipPlane));
	
	gScreenCoord = input.ScreenPosition;
	gPositionW = input.PositionW;
	gNormalW   = input.NormalW;
	gTexCoord  = input.TexCoord;
	gOcc     	= input.Occ;
	gShadowFactor = 1;	
	
	ComputeLighting();
	
	return  gColor;
}

float4 SampleEnvironmentMapPS(float3 positionW:TEXCOORD0 ,
							  float3 normalW:TEXCOORD1 ,
							  float2 texCoord:TEXCOORD2):COLOR0
{
	gPositionW = positionW;
	gNormalW   = normalW;
	gColor = float4(0,0,0,1);
	
	gAlpha = surface.Alpha;
	gGlossFactor = surface.Reflectivity;
	
	float4 diffuse =   USE_DIFFUSE_MAP  ? tex2D(s0 ,texCoord) : float4(1,1,1,1);	
	float4 emissive =  USE_EMISSIVE_MAP ? tex2D(s2 ,texCoord) : float4(1,1,1,1);	
	
	gAlpha *= diffuse.a;
	gGlossFactor *= emissive.a;
	
	ComputeEnvMapLigthing();	
	return gColor;
}	

float4 SampleReflecionMapPS(float4 screenCoord:TEXCOORD0, float2 texCoord:TEXCOORD1):COLOR0
{	
	gAlpha = surface.Alpha;
	gGlossFactor = surface.Reflectivity;
	gColor = float4(0,0,0,1);
	
	float4 diffuse =   USE_DIFFUSE_MAP  ? tex2D(s0 ,texCoord) : float4(1,1,1,1);	
	float4 emissive =  USE_EMISSIVE_MAP ? tex2D(s2 ,texCoord) : float4(1,1,1,1);	
	
	gAlpha *= diffuse.a;
	gGlossFactor *= emissive.a;
	
	gColor.rgb += tex2Dproj(s4, screenCoord).rgb * gGlossFactor;
	gColor.rgb += tex2Dproj(s5, screenCoord).rgb * (1 - gAlpha);	
		
	return gColor;
}	


float4 BumpPixelShader(BumpVSOutput input) : COLOR0
{
	clip(dot(float4(input.PositionW, 1), clipPlane));
	
	gPositionW = input.PositionW;
	gNormalW   = input.NormalW;
	gTangentW = input.TangentW;
	gBinormalW = input.BinormalW;
	gTexCoord  = input.TexCoord;
	gOcc     	= input.Occ;
	gShadowFactor = 1;	
		
	ComputeNormal();	
	ComputeLighting();
	
	return  gColor;	
  
}

float4 PhongPixelShaderSM(float3 positionW : TEXCOORD0 ,
						float3 normalW   : TEXCOORD1 , 
						float2 texCoord  : TEXCOORD2 ,
						float4 positionL : TEXCOORD3 ,
						float occ        : TEXCOORD4 ,
						float4 screenCoord : TEXCOORD5) : COLOR0
{   	
	clip(dot(float4(positionW, 1), clipPlane));
	
	gPositionW = positionW;
	gNormalW   = normalW;
	gTexCoord  = texCoord;
	gOcc      	= occ;
	gPositionL  = positionL;
	gScreenCoord = screenCoord;
	
	ComputeShadowTexCoord();
	ComputeShadowFactor();	
	ComputeLighting();

	return  gColor;
}

float4 BumpPixelShaderSM(SMBumpVSOutput input) : COLOR0
{		
	clip(dot(float4(input.PositionW, 1), clipPlane));
	
	gPositionW = input.PositionW;
	gNormalW   = input.NormalW;
	gTexCoord  = input.TexCoord;
	gOcc      	= input.Occ;
	gPositionL  = input.PositionL;
	gTangentW =  input.TangentW;
	gBinormalW = input.BinormalW;
	
	ComputeShadowTexCoord();
	ComputeShadowFactor();	
	ComputeNormal();
	ComputeLighting();

	return  gColor;
}

float4 PhongPixelShaderSSM(SMVSOutputSoft input, uniform bool hemisphere,  uniform int nbLights) : COLOR0
{   	 
	clip(dot(float4(input.PositionW, 1), clipPlane));
	
	gPositionW = input.PositionW;
	gNormalW   = input.NormalW;
	gTexCoord  = input.TexCoord;
	gOcc     	= input.Occ;
	
	gShadowFactor = tex2Dproj(s7, input.ScreenCoord).r;
	
	ComputeLighting();

	return  gColor;
}

float4 BumpPixelShaderSSM(SMBumpVSOutputSoft input , uniform bool hemisphere, uniform int nbLights) : COLOR0
{				
	clip(dot(float4(input.PositionW, 1), clipPlane));
	
	gPositionW = input.PositionW;
	gNormalW   = input.NormalW;
	gTangentW = input.TangentW;
	gBinormalW = input.BinormalW;
	gTexCoord  = input.TexCoord;
	gOcc     	= input.Occ;
	
	gShadowFactor = tex2Dproj(s7, input.ScreenCoord).r;
		
	ComputeNormal();	
	ComputeLighting();
	
	return  gColor;	
}

void DeferredMRTPS(float3 positionW : TEXCOORD0 ,
				  float3 normalW   : TEXCOORD1 , 
				  float2 texCoord  : TEXCOORD2 ,				
				  float occ        : TEXCOORD3 ,
				  float2 depth     : TEXCOORD4 ,
				  
				out	float4 oPositionW :COLOR0,
				out	float4 oNormalW   :COLOR1,
				out	float4 oDiffuse   :COLOR2,
				out	float4 oSpecular  :COLOR3)
{

	InitLighting();
	
	// float3 hemi;
	// float3 amb;	
	// ComputeHemisphere(hemi);	
	// ComputeAmbient(amb);
	
	// gColor.rgb += hemi * hemisphere + amb * (1- hemisphere);
	
	oPositionW = float4(positionW , depth.x / depth.y);
	oNormalW   = float4(normalW, gOcc);	
	oDiffuse   = float4(gDiffuse + gColor.rgb, gAlpha);
	oSpecular  = float4(gSpecular, gSpecularPower);
}

void DeferredSampleEnvironmentMapPS(float3 positionW:TEXCOORD0 ,
							  float3 normalW:TEXCOORD1 ,
							  float2 texCoord:TEXCOORD2,
							  out	float4 oPositionW :COLOR0,
							  out	float4 oNormalW   :COLOR1,
				              out	float4 oDiffuse   :COLOR2,
							  out	float4 oSpecular   :COLOR3)
{
	 oPositionW = float4(0,0,0,0);
	 oNormalW = float4(0,0,0,0);
	 oSpecular = float4(0,0,0,0);	 
	 oDiffuse  = float4(SampleEnvironmentMapPS(positionW, normalW ,texCoord).rgb, 0);
}	

void DeferredSampleReflecionMapPS(float4 screenCoord:TEXCOORD0, float2 texCoord:TEXCOORD1,
								out	float4 oPositionW :COLOR0,
							  out	float4 oNormalW   :COLOR1,
				              out	float4 oDiffuse   :COLOR2,
							  out	float4 oSpecular   :COLOR3)
{			
	 oPositionW = float4(0,0,0,0);
	 oNormalW = float4(0,0,0,0);
	 oSpecular = float4(0,0,0,0);	 
	 oDiffuse  = float4(SampleReflecionMapPS(screenCoord, texCoord).rgb, 0);	
}	

bool USE_SHADOWMAP = false;

float4 DeferredLigthing(float2 texCoord:TEXCOORD0):COLOR0
{
	gPositionW 		= tex2D(s0, texCoord).rgb;
	float4 normalW  = tex2D(s1, texCoord);
	float4 diffuse  = tex2D(s2, texCoord);
	float4 specular = tex2D(s3, texCoord);
		
	gNormalW = normalW.rgb;
	gOcc = normalW.a;
	gDiffuse = diffuse.rgb;
	gSpecular = specular.rgb;
	gSpecularPower = specular.a;
	gAlpha = diffuse.a;	
	gColor.a = gAlpha;
	
	if(USE_SHADOWMAP)
	{
		gPositionL = mul(float4(gPositionW,1) , LightVP);
		ComputeShadowFactor();
	}
	//compute local lighting
	ComputeDirectLighting();
		
	return gColor;		
}

float4 DeferredAmbient(float2 texCoord:TEXCOORD0, uniform bool hemisphere):COLOR0
{
	float4 normalW = tex2D(s1, texCoord);
	gDiffuse = tex2D(s2, texCoord).rgb;	
	
	gNormalW = normalW.xyz;
	gOcc = normalW.w;	
	
	float3 hemi;
	float3 amb;	
	
	ComputeHemisphere(hemi);	
	ComputeAmbient(amb);
		
	gColor = float4(hemi * hemisphere + amb * (1- hemisphere) , 0);
	//return gColor;
	
	return float4(gDiffuse, 1);
}