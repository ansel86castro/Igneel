
#define STD_LIGHTING

#ifndef TRANF_LIGHTING
	#include "TransformLighting.fxh"
#endif	

//Flags
bool USE_DIFFUSE_MAP	: DIFFUSE_MAP_FLAG = false;
bool USE_SPECULAR_MAP	: SPECULAR_MAP_FLAG = false;
bool USE_ENVIROMENT_MAP : ENVIROMENT_MAP_FLAG = false;
bool USE_REFLECTION_MAP : REFLECTION_MAP_FLAG = false;
bool USE_REFRACTION_MAP	: REFRACTION_MAP_FLAG = false;
bool USE_EMISSIVE_MAP	: EMISSIVE_MAP_FLAG = false;

bool enableClipping		: ENABLE_CLIPPING = false;
float4 clipPlane		: CLIP_PLANE = {0,0,0,0};  //plane in Homogenius Space
bool fNoRenderTransparency = false;
bool NoRenderOpaque = false;	
int activeLights;	
bool hemisphere;		 

sampler s0 : register(s0); // diffuse map  [a = opacity]
sampler s1 : register(s1); // specular map [a = specularPower]
sampler s2 : register(s2); // emissive map [a = glossFactor]
sampler s3 : register(s3); // normal Map   [a = Occlution Factor]
sampler s4 : register(s4); // Environment Map 
sampler s5 : register(s5); // reflection Map 
sampler s6 : register(s6); // refraction map
sampler s7 : register(s7); // unused

//GLOBALS
static float gAlpha;
static float gGlossFactor;
static float2 gOccTexCoord;

void ComputeNormal()
{	
	float3 normal = tex2D(s3, gTexCoord).rgb;
	normal = (normal * 2) - 1;	
	// Move the normal from tangent space to world space
	float3x3 tangentMatrix = {gTangentW, gBinormalW, gNormalW};	
	gNormalW = normalize(mul(normal, tangentMatrix));		
}

void ComputeEnvMapLigthing()
{			
	float3 eyeVector = normalize(gPositionW - eyePos);
	float3 reflecVec = reflect(eyeVector, gNormalW);
	gColor.rgb += texCUBE(s4, reflecVec).rgb * gGlossFactor;	
	
	float3 refr = refract(eyeVector, gNormalW, surface.Refractitity);
	gColor.rgb += texCUBE(s4, refr).rgb * (1 - gAlpha);	
}

void ComputReflectionLigthing()
{
	gColor.rgb += USE_REFLECTION_MAP ? tex2Dproj(s4, gScreenCoord).rgb * gGlossFactor : float3(0,0,0);
	gColor.rgb += USE_REFRACTION_MAP ? tex2Dproj(s5, gScreenCoord).rgb * (1 - gAlpha): float3(0,0,0);	
}

void InitLighting()
{
	gColor = float4(surface.Emisive, 1);
	gDiffuse = surface.Diffuse;
	gSpecular = surface.Specular;	
	gSpecularPower = surface.SpecularPower;
	gAlpha = surface.Alpha;
	gGlossFactor = surface.Reflectivity;
	float4 c;
	
	[branch]
	if(USE_DIFFUSE_MAP)
	{
		c = tex2D(s0 ,gTexCoord);
		gDiffuse *= c.rgb;	
		gAlpha *= c.a;
	}
	
	clip(-((fNoRenderTransparency && gAlpha != 1.0f) || (NoRenderOpaque && gAlpha == 1.0f)));
	
	[branch]
	if(USE_SPECULAR_MAP)
	{
		c = tex2D(s1 ,gTexCoord);
		gSpecular *= c.rgb;
		gSpecularPower *= c.a;
	}
	
	[branch]
	if(USE_EMISSIVE_MAP)
	{
		c = tex2D(s2 ,gTexCoord);
		gColor.rgb *= c.rgb;
		gGlossFactor *= c.a;
	}
	
	gOcc *= tex2D(s3 ,gOccTexCoord).a;						
	gColor.a = gAlpha;
}

void ComputeLighting()
{
	InitLighting();
	
	[branch]
	if(hemisphere)
		ComputeHemisphere();
	else
		ComputeAmbient();
	 
	 [branch]
	 if(activeLights > 0)
		ComputeDirectLighting();	 			
	
	[branch]
	if(USE_ENVIROMENT_MAP)
	{
		ComputeEnvMapLigthing();					
	}
	else [branch]if (USE_REFLECTION_MAP)		
	{
		gColor.rgb += tex2Dproj(s5, gScreenCoord).rgb;// * gGlossFactor;			
	}
	[branch]
	if (USE_REFRACTION_MAP)	
		gColor.rgb += tex2Dproj(s6, gScreenCoord).rgb * (1 - gAlpha);	
}
