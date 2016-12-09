#include "HDRCommon.hlsli"

float4 main(PS_RenderQuadInput input) : SV_TARGET
{    
    float4 vSample = 0.0f;
    
	float2 offsets[16] = (float2[16])SampleOffsets;
    // Perform a one-directional gaussian blur
	[unroll]
    for(int i = 0; i < 8; i++)
    {             
        vSample += SampleWeights[i] * t0.SampleLevel(s0, input.texCoord + offsets[i] , 0);
    }
    vSample.a = 1;
    return vSample;
}