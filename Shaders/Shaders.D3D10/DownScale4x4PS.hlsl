#include "HDRCommon.hlsli"

float4 main(PS_RenderQuadInput input) : SV_TARGET
{		
    float4 color = float4(0,0,0,1);

	[unroll]
	for(int y=0; y < 4; y++)
	{
		[unroll]
		for(int x = 0; x < 4; x++)
		{
			color.rgb += t0.Sample(s0, input.texCoord , int2(x,y)).rgb;
		}
	}	
	
	color.rgb /= 16;

	return color;
}