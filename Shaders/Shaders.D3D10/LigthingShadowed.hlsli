#ifndef SHADOWING
	#include "Shadowing.hlsli"
#endif

#ifndef KERNEL_SIZE
	#define KERNEL_SIZE 3
#endif

Texture2D ShadowMap;
SamplerComparisonState sShadowMap;

void ComputeShadowFactor()
{	
	gShadowFactor =0;

	int start = -KERNEL_SIZE / 2;
	int end = KERNEL_SIZE / 2 + 1;

	float width, height;
	ShadowMap.GetDimensions(width, height);
	float tx = 1.0 / width;
	float ty = 1.0 / height;

	[unroll]
	for(float y = start ; y < end ; y++)
	{	
		[unroll]
		for(float x = start ; x < end ; x++)
		{
			gShadowFactor += ShadowMap.SampleCmpLevelZero(sShadowMap, gShadowTexCoord.xy + float2(x*tx, y*ty) , gShadowTexCoord.z);
		}
	}
	
	gShadowFactor /= (float)(KERNEL_SIZE * KERNEL_SIZE);
}