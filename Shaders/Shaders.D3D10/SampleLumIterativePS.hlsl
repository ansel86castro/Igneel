#include "HDRCommon.hlsli"

float4 main(PS_RenderQuadInput input) : SV_TARGET
{
    float4 color = 0.0f;
    float maximum = MIN_FLOAT;
    float average = 0.0f;
	
	[unroll]
	for(int y=-1; y < 2; y++)
	{
		[unroll]
		for(int x = -1; x < 2; x++)
		{
			color  = t0.SampleLevel(s0, input.texCoord , 0 ,int2(x,y));
			average += color.r;
			maximum = max(maximum, color.g);
		}
	}   
    
    // Divide the sum to complete the average
    average /= 9.0f;
		   
	return float4(average, maximum, 0, 1.0f);
}