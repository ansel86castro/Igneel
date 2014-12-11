#include "HDRCommon.hlsli"

float4 main(PS_RenderQuadInput input) : SV_TARGET
{
    float4 color  = t0.SampleLevel(s0, input.texCoord, 0);
	float2 l 	  = t1.SampleLevel(s1, float2(0.0, 0.0), 0).rg;
    float3 bloom  = t2.SampleLevel(s2, input.texCoord, 0).rgb;  
	float3 star	  = t3.SampleLevel(s3, input.texCoord , 0).rgb;	
		
	float pixelLum = dot(color.rgb, LUMINANCE_VECTOR);
	// For very low light conditions, the rods will dominate the perception
    // of light, and therefore color will be desaturated and shifted
    // towards blue.
	[branch]
    if( EnableBlueShift )
    {
		// Define a linear blending from -1.5 to 2.6 (log scale) which
		// determines the lerp amount for blue shift
        float fBlueShiftCoefficient = 1.0f - (l.r + 1.5)/4.1;
        fBlueShiftCoefficient = saturate(fBlueShiftCoefficient);

		// Lerp between current color and blue, desaturated copy
        float3 vRodColor = pixelLum * BLUE_SHIFT_VECTOR;
        color.rgb = lerp( (float3)color, vRodColor, fBlueShiftCoefficient );
    }
    	
	
	//perform tone mapping based on Reinhard's tone mapping equation
	float Lp = (MIDDLE_GRAY / l.r) * pixelLum;
	float LmSqr = (l.g + GaussianScalar * l.g) * (l.g + GaussianScalar * l.g);
	float toneScalar = ( Lp * ( 1.0f + ( Lp / ( LmSqr ) ) ) ) / ( 1.0f + Lp );

	color.rgb *= toneScalar;
	color.a = 1.0f;

 //   // Map the high range of color values into a range appropriate for
 //   // display, taking into account the user's adaptation level, and selected
 //   // values for for middle gray and white cutoff.    
	//vSample.rgb *= MIDDLE_GRAY / (vLum.r + 0.0001f);
	//vSample.rgb /= (1.0f + vSample.rgb);
	//
	////vSample.rgb *= (1.0f + vSample / LUM_WHITE);	

 //   
 //   // Add the bloom post processing effects   
    color.rgb += BLOOM_BLEND * bloom;
	color.rgb += STAR_BLEND *  star;

    return color;
}