#define SKINNED_SHADOWED

#ifndef LIGTHING_SHADOWED
	#include "LigthingShadowed.fxh"
#endif

#ifndef SKINNED
	#include "Skinned.fxh"
#endif

void TransformSkinnedPNL(uniform int NumBones)
{
	TransformSkinnedPN(NumBones);
	gPositionL = mul(float4(gPositionW,1) , LightVP);
}

void TransformSkinnedPNTL(uniform int NumBones)
{	
	TransformSkinnedPNT(NumBones);
	gPositionL = mul(float4(gPositionW,1) , LightVP);
}