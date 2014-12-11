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
             color.rgb += t0.SampleLevel(s0, input.texCoord , 0 ,int2(x,y)).rgb;
        }
    }	
	color.rgb /= 4;	

	float2 l =  t1.SampleLevel(s1, float2(0, 0) , 0).rg;			
	//color.rgb = max(0.0, color.rgb - BRIGHT_PASS_THRESHOLD);				

	float pixelLum = dot(color.rgb, LUMINANCE_VECTOR);

	[branch]
	if(pixelLum < BRIGHT_PASS_THRESHOLD)
		return float4(0,0,0,1);	
	
	 // Reinhard's tone mapping equation (See Eqn#3 from 
    // "Photographic Tone Reproduction for Digital Images" for more details) is:
    //
    //      (      (   Lp    ))
    // Lp * (1.0f +(---------))
    //      (      ((Lm * Lm)))
    // -------------------------
    //         1.0f + Lp
    //
    // Lp is the luminance at the given point, this is computed using Eqn#2 from the above paper:
    //
    //        exposure
    //   Lp = -------- * HDRPixelIntensity
    //          l.r
    //
    // The exposure ("key" in the above paper) can be used to adjust the overall "balance" of 
    // the image. "l.r" is the average luminance across the scene, computed via the luminance
    // downsampling process. 'HDRPixelIntensity' is the measured brightness of the current pixel
    // being processed.	
	float Lp = (MIDDLE_GRAY / l.r) * pixelLum;
	float LmSqr = l.g * l.g;	
	float toneScalar = ( Lp * ( 1.0f + ( Lp / ( LmSqr ) ) ) ) / ( 1.0f + Lp );				
	color.rgb *= toneScalar;			
	
	return color;
	
}