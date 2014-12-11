
#define LIGHTING

#ifndef LIGHTING_BASE
	#include "LightingBase.hlsli"
#endif

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

float4 SampleIndirectEvMap()
{
	float4 global = (float4)0;

	float3 eyeVector = normalize(gPositionW - eyePos);
	float3 reflecVec = reflect(eyeVector, gNormalW);

	global.rgb += t4.Sample(s4, reflecVec).rgb * gGlossFactor;	
	
	float3 refr = refract(eyeVector, gNormalW, surface.Refractitity);
	global.rgb += t4.Sample(s4, refr).rgb * (1 - gAlpha);	
	return global;
}

void ComputReflectionLigthing()
{
	float2 texCoord = gScreenCoord.xy / gScreenCoord.w;
	gColor.rgb += USE_REFLECTION_MAP ? t5.Sample(s5, texCoord).rgb * gGlossFactor : float3(0,0,0);
	gColor.rgb += USE_REFRACTION_MAP ? t6.Sample(s6, texCoord).rgb * (1 - gAlpha): float3(0,0,0);	
}

void InitLighting()
{
	//add emisive light contribution
	gColor = float4(surface.Diffuse.rgb * surface.Emisive, 1);	

	gDiffuse = surface.Diffuse.rgb;
	gSpecular = surface.Diffuse.rgb * surface.Specular;	
	gSpecularPower = surface.SpecularPower;
	gAlpha = surface.Diffuse.a;
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

		gGlossFactor *= c.a;
		gSpecularPower *= c.a;
	}		
	
	//gOcc *= t3.Sample(s3 ,gTexCoord).a;

	//set alpha 
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
	 if(light.Type == DIRECTIONAL)
		DirectionalLight();	 					
	 else [branch] if(light.Type == SPOT)
		SpotLight();
	 else [branch] if(light.Type == POINT)
		PointLight();	 
		
	/*[branch]
	if(USE_ENVIROMENT_MAP)
	{
		ComputeEnvMapLigthing();					
	}
	else
	{
		[branch]
		if (USE_REFLECTION_MAP)				
			gColor.rgb += gDiffuse * t5.Sample(s5, gScreenCoord.xy / gScreenCoord.w).rgb * gGlossFactor;

		[branch]
		if (USE_REFRACTION_MAP)	
			gColor.rgb += gDiffuse * t6.Sample(s6, gScreenCoord.xy / gScreenCoord.w).rgb * (1 - gAlpha);
	}	*/
}


