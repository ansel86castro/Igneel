#include "HDRCommon.hlsli"

float4 main(PS_RenderQuadInput input) : SV_TARGET
{	
   float4 color = float4(0,0,0,1);

   [unroll]
	for( int y = 0; y < 2; y++ )
    {
		[unroll]
        for( int x = 0; x < 2; x++ )
        {
             color.rgb += t0.SampleLevel(s0, input.texCoord , 0, int2(x,y)).rgb;
        }
    }
	color.rgb /= 4;

	return color;
}