
#include "ShadowMapTransform.hlsli"

#ifndef SKINNED
	#include "Skinned.hlsli"
#endif

void TransformSkinnedPNL()
{
	TransformSkinnedPN();
	gPositionL = mul(float4(gPositionW,1) , LightVP);
}

void TransformSkinnedPNTL()
{	
	TransformSkinnedPNT();
	gPositionL = mul(float4(gPositionW,1) , LightVP);
}