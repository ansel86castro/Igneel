
#define STD_LIGHTING

#ifndef TRANF_LIGHTING
	#include "TransformLighting.fxh"
#endif	

cbuffer flags{	
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
};

Texture2D    t0                    : register( t0 ); // diffuse map  [a = opacity]
Texture2D    t1                    : register( t1 ); // specular map [a = specularPower]
Texture2D    t2                    : register( t2 ); // emissive map [a = glossFactor]
Texture2D    t3                    : register( t3 ); // normal Map   [a = Occlution Factor]
TextureCube  t4                    : register( t4 ); // Environment Map 
Texture2D    t5                    : register( t5 ); // reflection Map 
Texture2D    t6                    : register( t6 ); // refraction map


SamplerState s0 : register(s0); // diffuse map  [a = opacity]
SamplerState s1 : register(s1); // specular map [a = specularPower]
SamplerState s2 : register(s2); // emissive map [a = glossFactor]
SamplerState s3 : register(s3); // normal Map   [a = Occlution Factor]
SamplerState s4 : register(s4); // Environment Map 
SamplerState s5 : register(s5); // reflection Map 
SamplerState s6 : register(s6); // refraction map

//GLOBALS
static float gAlpha;
static float gGlossFactor;
static float2 gOccTexCoord;

void ComputeNormal()
{	
	float3 normal = t3.Sample(s3, gTexCoord).rgb;
	normal = (normal * 2) - 1;	
	// Move the normal from tangent space to world space
	float3x3 tangentMatrix = {gTangentW, gBinormalW, gNormalW};	
	gNormalW = normalize(mul(normal, tangentMatrix));		
}

void ComputeEnvMapLigthing()
{			
	float3 eyeVector = normalize(gPositionW - eyePos);
	float3 reflecVec = reflect(eyeVector, gNormalW);
	gColor.rgb += t4.Sample(s4, reflecVec).rgb * gGlossFactor;	
	
	float3 refr = refract(eyeVector, gNormalW, surface.Refractitity);
	gColor.rgb += t4.Sample(s4, refr).rgb * (1 - gAlpha);	
}

void ComputReflectionLigthing()
{
	float2 texCoord = gScreenCoord.xy / gScreenCoord.w;
	gColor.rgb += USE_REFLECTION_MAP ? tex2Dproj(s4, texCoord).rgb * gGlossFactor : float3(0,0,0);
	gColor.rgb += USE_REFRACTION_MAP ? tex2Dproj(s5, texCoord).rgb * (1 - gAlpha): float3(0,0,0);	
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
		c = t0.Sample(s0 ,gTexCoord);
		gDiffuse *= c.rgb;	
		gAlpha *= c.a;
	}
	
	clip(-((fNoRenderTransparency && gAlpha != 1.0f) || (NoRenderOpaque && gAlpha == 1.0f)));
	
	[branch]
	if(USE_SPECULAR_MAP)
	{
		c = t1.Sample(s1 ,gTexCoord);
		gSpecular *= c.rgb;
		gSpecularPower *= c.a;
	}
	
	[branch]
	if(USE_EMISSIVE_MAP)
	{
		c = t2.Sample(s2 ,gTexCoord);
		gColor.rgb *= c.rgb;
		gGlossFactor *= c.a;
	}
	
	gOcc *= t3.Sample(s3 ,gOccTexCoord).a;						
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
		gColor.rgb += gDiffuse * t5.Sample(s5, gScreenCoord.xy / gScreenCoord.w).rgb * gGlossFactor;
	}
	[branch]
	if (USE_REFRACTION_MAP)	
		gColor.rgb += gDiffuse * t6.Sample(s6, gScreenCoord.xy / gScreenCoord.w).rgb * (1 - gAlpha);
}
