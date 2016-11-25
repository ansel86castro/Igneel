#define TRANF_LIGHTING

#ifndef LIGHTING
	#include "Lighting.hlsli"
#endif
#ifndef COMMON	
	#include "Common.hlsli"
#endif

#ifndef GLOBALS
	#include "Globals.hlsli"
#endif

cbuffer perScene
{
	float4x4 ViewProj		: VIEWPROJ;	
	float3 sky 			 	: SKY_COLOR = { 0.5f, 0.5f, 0.5f };
	float3 ground 			: GROUND_COLOR = { 0.0f, 0.0f, 0.0f};
	float3 northPole 		: NORTH_POLE = { 0.0f, 1.0f ,0.0f };
	float3 eyePos			: EYEPOSITION;
};
cbuffer perLight
{
	float3 ambient			: AMBIENT;
	Light light				: LIGHT;
};
cbuffer perObject
{
	float4x4 World			: WORLD;
};
cbuffer perMaterial
{
	SurfaceInfo surface		: SURFACE;
};



void TransformPN()
{  
	gPositionW = mul(gPositionH, World).xyz;
	gPositionH = mul(float4(gPositionW,1), ViewProj);		
	gNormalW = normalize(mul(gNormalW, (float3x3)World));	
}

void TransformPNT()
{  
	gPositionW = mul(gPositionH, World).xyz;
	gPositionH = mul(float4(gPositionW,1), ViewProj);
	gNormalW = normalize(mul(gNormalW, (float3x3)World));
	gTangentW = normalize(mul(gTangentW, (float3x3)World));
	gBinormalW= normalize(cross(gNormalW, gTangentW));
}

void TransformScreeen()
{	
	gScreenCoord.x = (gPositionH.x + gPositionH.w) * 0.5f;
    gScreenCoord.y = (gPositionH.w - gPositionH.y) * 0.5f;
    gScreenCoord.zw = gPositionH.w;
}

