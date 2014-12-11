
#ifndef SHADOWING
	#include "Shadowing.hlsli"
#endif

#define LIGTHING_SHADOWED

#ifndef KERNEL_SIZE
	#define KERNEL_SIZE 3
#endif

Texture2D t7			  : register(t7);
SamplerComparisonState s7 : register(s7);
//s6 static Shadow MAp
//s7 light Shadow Map

void ComputeShadowFactor()
{	
	gShadowFactor = 0;

	int start = -KERNEL_SIZE / 2;
	int end = KERNEL_SIZE / 2 + 1;

	float width, height;
	t7.GetDimensions(width, height);
	float tx = 1.0 / width;
	float ty = 1.0 / height;

	[unroll]
	for(float y = start ; y < end ; y++)
	{
		[unroll]
		for(float x = start ; x < end ; x++)
		{
			gShadowFactor += t7.SampleCmpLevelZero(s7, gShadowTexCoord.xy + float2(x*tx, y*ty) , gShadowTexCoord.z);
		}
	}				
	
	gShadowFactor /= (float)(KERNEL_SIZE * KERNEL_SIZE);
}