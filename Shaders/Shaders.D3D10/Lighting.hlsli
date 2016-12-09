#define LIGHTING

#ifndef LIGHTING_BASE
	#include "LightingBase.hlsli"
#endif

Texture2D    DiffuseMap;     // [a = opacity]
Texture2D    SpecularMap;    // [a = specularPower]
Texture2D    NormalMap;      // [a = Occlution Factor]
TextureCube  EnvironmentMap; // [a = Occlution Factor]
Texture2D    ReflectionMap; 
Texture2D    RefractionMap;

SamplerState sDiffuseMap; // Linear Sampler


void ComputeNormal()
{		
	float3 normal = NormalMap.Sample(sDiffuseMap, gTexCoord).rgb;
	normal = (normal * 2) - 1;	
	// Move the normal from tangent space to world space
	float3x3 tangentMatrix = {gTangentW, gBinormalW, gNormalW};	
	gNormalW = normalize(mul(normal, tangentMatrix));		
}

void ComputeEnvMapLigthing()
{			
	float3 eyeVector = normalize(gPositionW - EyePos);
	float3 reflecVec = reflect(eyeVector, gNormalW);
	gColor.rgb += EnvironmentMap.Sample(sDiffuseMap, reflecVec).rgb * gGlossFactor;	
	
	float3 refr = refract(eyeVector, gNormalW, Surface.Refractitity);
	gColor.rgb += EnvironmentMap.Sample(sDiffuseMap, refr).rgb * (1 - gAlpha);	
}

float4 SampleIndirectEvMap()
{
	float4 global = (float4)0;

	float3 eyeVector = normalize(gPositionW - EyePos);
	float3 reflecVec = reflect(eyeVector, gNormalW);

	global.rgb += EnvironmentMap.Sample(sDiffuseMap, reflecVec).rgb * gGlossFactor;	
	
	float3 refr = refract(eyeVector, gNormalW, Surface.Refractitity);
	global.rgb += EnvironmentMap.Sample(sDiffuseMap, refr).rgb * (1 - gAlpha);	
	return global;
}

void ComputReflectionLigthing()
{
	float2 texCoord = gScreenCoord.xy / gScreenCoord.w;
	gColor.rgb += USE_REFLECTION_MAP ? ReflectionMap.Sample(sDiffuseMap, texCoord).rgb * gGlossFactor : float3(0,0,0);
	gColor.rgb += USE_REFRACTION_MAP ? RefractionMap.Sample(sDiffuseMap, texCoord).rgb * (1 - gAlpha): float3(0,0,0);	
}

void InitLighting()
{
	//add emisive Light contribution
	gColor = float4(Surface.Diffuse.rgb * Surface.Emisive, 1);	

	gDiffuse = Surface.Diffuse.rgb;
	gSpecular = Surface.Diffuse.rgb * Surface.Specular;	
	gSpecularPower = Surface.SpecularPower;
	gAlpha = Surface.Diffuse.a;
	gGlossFactor = Surface.Reflectivity;
	float4 c;
	
	[branch]
	if(USE_DIFFUSE_MAP)
	{
		c = DiffuseMap.Sample(sDiffuseMap ,gTexCoord);
		gDiffuse *= c.rgb;	
		gAlpha *= c.a;
	}	
	
	//clip(-((NoRenderTransparency && gAlpha != 1.0f) || (NoRenderOpaque && gAlpha == 1.0f)));
	if((NoRenderTransparency && gAlpha < 1.0) || (NoRenderOpaque && gAlpha == 1.0))
		discard;	

	[branch]
	if(USE_SPECULAR_MAP)
	{
		c = SpecularMap.Sample(sDiffuseMap ,gTexCoord);
		gSpecular *= c.rgb;

		gGlossFactor *= c.a;
		gSpecularPower *= c.a;
	}		
			
	gColor.a = gAlpha;
}

void ComputeLighting()
{
	InitLighting();	
	
	[branch]
	if(HemisphericalLighting)
		ComputeHemisphere();
	else
		ComputeAmbient();
		 
	 [branch]
	 if(Light.Type == DIRECTIONAL)
		DirectionalLight();	 					
	 else [branch] if(Light.Type == SPOT)
		SpotLight();
	 else [branch] if(Light.Type == POINT)
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


