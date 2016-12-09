#define TRANSFORM

#ifndef GLOBALS
	#include "Globals.cgi"
#endif

uniform camera 
{
	float4x4 ViewProj		: VIEWPROJ;	

}:BUFFER;

uniform perObject 
{
	float4x4 World			: WORLD;
}:BUFFER;

void TransformP()
{
	gPositionW = mul(gPositionH, World).xyz;
	gPositionH = mul(float4(gPositionW,1), ViewProj);
}

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
