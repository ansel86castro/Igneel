#include "HDRCommon.hlsli"


float4 main(PS_RenderQuadInput input) : SV_TARGET
{
    float4 color = 0.0f;
    float average = 0.0f;
    float maximum = MIN_FLOAT;
	
	[unroll]
	for( int y = 0; y < 2; y++ )
    {
		[unroll]
        for( int x = 0; x < 2; x++ )
        {
             color = t0.SampleLevel(s0, input.texCoord , 0, int2(x,y));

			 float lum = dot(color.rgb, LUMINANCE_VECTOR);

			 maximum = max(maximum, lum);

			 float v = log( lum + 0.0001f);
			 if(!isnan(v))
				average += 	v; 
        }
    }	

    average /= 4.0f;	
	return float4(average, maximum, 0, 1.0f);
}