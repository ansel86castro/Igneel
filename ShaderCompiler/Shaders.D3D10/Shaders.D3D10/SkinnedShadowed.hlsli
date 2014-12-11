#include "ShadowMapTransform.hlsli"
#include "Skinned.hlsli"

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