#ifndef LIGHTING
	#include "Lighting.fxh"
#endif

cbuffer perObject
{
	 float4x4 World			: WORLD;
};
cbuffer perCamera
{
	 float4x4 ViewProj		: VIEWPROJ;
};
cbuffer perMaterial
{
	SurfaceInfo surface	: SURFACE;
	bool USE_DIFFUSE_MAP	: DIFFUSE_MAP_FLAG = false;
}; 
Texture2D t0    :register (t0);
SamplerState s0 : register(s0); //diffuse Sampler


//***************BuildShadowMap**********************************

void BuildShadowMapVS(in  float3 position  : POSITION,
					  in  float2 texCoord  : TEXCOORD0,
					  out float4 positionH : POSITION,
					  out float2 depth	   : TEXCOORD0,
					  out float2 outTexCoord : TEXCOORD1)
{
	float4 positionW = mul(float4(position,1) , World);
	positionH = mul(positionW, ViewProj);
	depth = float2(positionH.z , positionH.w);
	outTexCoord = texCoord;
}

float4 BuildShadowMapPS(float2 depth:TEXCOORD0, float2 texCoord:TEXCOORD1) : COLOR0
{
	float alpha = USE_DIFFUSE_MAP ? t0.Sample(s0, texCoord).a * surface.Alpha : surface.Alpha;	
	if(alpha == 0)
		discard;	
		
	//float z = 1 - depth.x / depth.y;
	float z = depth.x / depth.y;
	return float4(z ,z ,z ,1);		
}