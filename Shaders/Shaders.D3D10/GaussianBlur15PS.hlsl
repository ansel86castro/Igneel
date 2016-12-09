#include "HDRCommon.hlsli"

float4 main(PS_RenderQuadInput input) : SV_TARGET
{    
    float4 color = float4(0,0,0,1);
    float2 offsets[16] = (float2[16])SampleOffsets;
    // Perform a one-directional gaussian blur
	[unroll]
    for(int i = 0; i < 15; i++)
    {             
        color.rgb += (SampleWeights[i] * t0.SampleLevel(s0, input.texCoord + offsets[i] , 0)).rgb;
    }
    
    return color;
}