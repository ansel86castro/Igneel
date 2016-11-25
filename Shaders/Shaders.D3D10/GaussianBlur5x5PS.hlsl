#include "HDRCommon.hlsli"

float4 main(PS_RenderQuadInput input) : SV_TARGET
{	
    float4 sample = 0.0f;
	float2 offsets[16] = (float2[16])SampleOffsets;
	[unroll]
	for( int i=0; i < 12; i++ )
	{
		sample += SampleWeights[i] * t0.SampleLevel( s0, input.texCoord + offsets[i] , 0);
	}
	sample.a = 1;
	return sample;
}