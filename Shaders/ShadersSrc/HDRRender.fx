#include "Include/Lighting.fxh"

// The per-color weighting to be used for luminance calculations in RGB order.
static const float3 LUMINANCE_VECTOR  = float3(0.2125f, 0.7154f, 0.0721f);
// The per-color weighting to be used for blue shift under low light.
static const float3 BLUE_SHIFT_VECTOR = float3(1.05f, 0.97f, 1.27f); 

float  MIDDLE_GRAY = 0.18f;
float  LUM_WHITE = 1.0f;
float  BRIGHT_PASS_THRESHOLD = 5.0;
float  BRIGHT_PASS_OFFSET = 10.0;
float  BLOOM_BLEND = 0.6f;
float  STAR_BLEND = 0.6;
float  ElapsedTime :TIME;
bool   EnableBlueShift : register(b0);   // Flag indicates if blue shift is performed

float2 SampleOffsets[16];
float4 SampleWeights[16];

sampler s0 : register(s0);
sampler s1 : register(s1);
sampler s2 : register(s2);
sampler s3 : register(s3);
sampler s4 : register(s4);
sampler s5 : register(s5);
sampler s6 : register(s6);
sampler s7 : register(s7);


//-----------------------------------------------------------------------------
// Name: DownScale4x4PS
// Type: Pixel shader                                      
// Desc: Scale the source texture down to 1/16 scale
//-----------------------------------------------------------------------------
float4 DownScale4x4PS(in float2 vScreenPosition : TEXCOORD0) : COLOR0
{	
    float4 vSample = 0.0f;
	//[unroll]
	for( int i=0; i < 16; i++ )	
	{
		vSample += tex2D( s0, vScreenPosition + SampleOffsets[i] );	    
	}
	return vSample / 16;
}

//-----------------------------------------------------------------------------
// Name: DownScale2x2PS
// Type: Pixel shader                                      
// Desc: Scale the source texture down to 1/4 scale
//-----------------------------------------------------------------------------
float4 DownScale2x2PS(in float2 vScreenPosition : TEXCOORD0) :COLOR0
{
	float4 vSample = 0.0f;
	[unroll]
	for( int i=0; i < 4; i++ )	
	{
		vSample += tex2D( s0, vScreenPosition + SampleOffsets[i] );
	}
    
	return vSample / 4;
}
//-----------------------------------------------------------------------------
// Name: GaussBlur5x5PS
// Type: Pixel shader                                      
// Desc: Simulate a 5x5 kernel gaussian blur by sampling the 12 points closest
//       to the center point.
//-----------------------------------------------------------------------------
float4 GaussBlur5x5PS(in float2 vScreenPosition : TEXCOORD0) : COLOR
{	
    float4 sample = 0.0f;
	[unroll]
	for( int i=0; i < 12; i++ )
	{
		sample += SampleWeights[i] * tex2D( s0, vScreenPosition + SampleOffsets[i] );
	}

	return sample;
}
//-----------------------------------------------------------------------------
// Name: BloomPS
// Type: Pixel shader                                      
// Desc: Blur the source image along one axis using a gaussian
//       distribution. Since gaussian blurs are separable, this shader is called 
//       twice; first along the horizontal axis, then along the vertical axis.
//-----------------------------------------------------------------------------
float4 BloomPS(in float2 vScreenPosition : TEXCOORD0) : COLOR
{    
    float4 vSample = 0.0f;
    
    // Perform a one-directional gaussian blur
	[unroll]
    for(int i = 0; i < 15; i++)
    {             
        vSample += SampleWeights[i] * tex2D(s0, vScreenPosition + SampleOffsets[i]);
    }
    
    return vSample;
}

//-----------------------------------------------------------------------------
// Name: StarPS
// Type: Pixel shader                                      
// Desc: Each star is composed of up to 8 lines, and each line is created by
//       up to three passes of this shader, which samples from 8 points along
//       the current line.
//-----------------------------------------------------------------------------
float4 StarPS(in float2 vScreenPosition : TEXCOORD0) : COLOR
{
    float4 vSample = 0.0f;
    float4 vColor = 0.0f;
        
    float2 vSamplePosition;
    
    // Sample from eight points along the star line
	[unroll]
    for(int i = 0; i < 8; i++)
    {
        vSamplePosition = vScreenPosition + SampleOffsets[i];
        vSample = tex2D(s0, vSamplePosition);
        vColor += SampleWeights[i] * vSample;
    }
    	
    return vColor;
}
//-----------------------------------------------------------------------------
// Name: SampleLumInitial
// Type: Pixel shader                                      
// Desc: Sample the luminance of the source image using a kernal of sample
//       points, and return a scaled image containing the log() of averages
//-----------------------------------------------------------------------------
float4 SampleLumInitial(in float2 vScreenPosition : TEXCOORD0,
						  uniform bool RGBE8,
                          uniform bool RGB16) : COLOR0
{
    float4 vSample = 0.0f;
    float  fLogLumSum = 0.0f;
	
	[unroll]
    for(int i = 0; i < 9; i++)
    {
        // Compute the sum of log(luminance) throughout the sample points
        vSample = tex2D(s0, vScreenPosition + SampleOffsets[i]);
		
		if( RGBE8 )
            vSample.rgb = DecodeRGBE8( vSample );
        if( RGB16 )
            vSample.rgb = DecodeRGB16( vSample );
		
        fLogLumSum += log( dot(vSample, LUMINANCE_VECTOR) + 0.0001f);
		//fLogLumSum += dot(vSample, LUMINANCE_VECTOR);
    }
    
    // Divide the sum to complete the average
    fLogLumSum /= 9;

	if( RGBE8 )
        return EncodeRE8( fLogLumSum );
    else if( RGB16 )
        return EncodeR16( fLogLumSum );
    else       
		return float4(fLogLumSum, fLogLumSum, fLogLumSum, 1.0f);
}

//-----------------------------------------------------------------------------
// Name: SampleLumIterative
// Type: Pixel shader                                      
// Desc: Scale down the luminance texture by blending sample points
//-----------------------------------------------------------------------------
float4 SampleLumIterative(in float2 vScreenPosition : TEXCOORD0,
						 uniform bool RGBE8,
                         uniform bool RGB16) : COLOR0
{
    float fResampleSum = 0.0f; 
    float4 vSample;
	
	[unroll]
    for(int i = 0; i < 16; i++)
    {
        // Compute the sum of luminance throughout the sample points
        vSample = tex2D(s0, vScreenPosition + SampleOffsets[i]);
		
		if( RGBE8 )
            fResampleSum += DecodeRE8( vSample );
        else if( RGB16 )
            fResampleSum += DecodeR16( vSample );
        else
            fResampleSum += vSample.r; 
		
    }
    
    // Divide the sum to complete the average
    fResampleSum /= 16;
	
	if( RGBE8 )
        return EncodeRE8( fResampleSum );
    if( RGB16 )
        return EncodeR16( fResampleSum );
    else       
		return float4(fResampleSum, fResampleSum, fResampleSum, 1.0f);
}

//-----------------------------------------------------------------------------
// Name: SampleLumFinal
// Type: Pixel shader                                      
// Desc: Extract the average luminance of the image by completing the averaging
//       and taking the exp() of the result
//-----------------------------------------------------------------------------
float4 SampleLumFinal(in float2 vScreenPosition : TEXCOORD0,
					uniform bool RGBE8,
                    uniform bool RGB16) : COLOR
{
    float fResampleSum = 0.0f;
    float4 vSample;
	
	[unroll]
    for(int i = 0; i < 16; i++)
    {
        // Compute the sum of luminance throughout the sample points
        vSample = tex2D(s0, vScreenPosition + SampleOffsets[i]);
		
		if( RGBE8 )
            fResampleSum += DecodeRE8( vSample );
        else if( RGB16 )
            fResampleSum += DecodeR16( vSample );
        else
            fResampleSum += vSample.r; 
    }
    
    // Divide the sum to complete the average, and perform an exp() to complete
    // the average luminance calculation
    fResampleSum = exp(fResampleSum / 16);
    
    return float4(fResampleSum, fResampleSum, fResampleSum, 1.0f);
}

//-----------------------------------------------------------------------------
// Name: CalculateAdaptedLumPS
// Type: Pixel shader                                      
// Desc: Calculate the luminance that the camera is current adapted to, using
//       the most recented adaptation level, the current scene luminance, and
//       the time elapsed since last calculated
//-----------------------------------------------------------------------------
float4 CalculateAdaptedLumPS(in float2 vScreenPosition : TEXCOORD0,
							uniform bool RGBE8,
							uniform bool RGB16) : COLOR0
{	
	float4 vSample1 = tex2D(s0, float2(0.5f, 0.5f));
	float4 vSample2 = tex2D(s1, float2(0.5f, 0.5f));
	float fAdaptedLum = vSample1.r; 
	float  fCurrentLum = vSample2.r;
	if(RGBE8)
	{
		fAdaptedLum = DecodeRE8( vSample1 );
		fCurrentLum = DecodeRE8( vSample2 );
	}
	else if(RGB16)
	{
		fAdaptedLum = DecodeR16( vSample1 );
		fCurrentLum = DecodeR16( vSample2 );
	}	
     
    // The user's adapted luminance level is simulated by closing the gap between
    // adapted luminance and current luminance by 2% every frame, based on a
    // 30 fps rate. This is not an accurate model of human adaptation, which can
    // take longer than half an hour.
    float fNewAdaptation = fAdaptedLum + (fCurrentLum - fAdaptedLum) * ( 1 - pow( 0.98f, 30 * ElapsedTime ) );
	
	if( RGBE8 )
        return EncodeRE8( fNewAdaptation );
    if( RGB16 )
        return EncodeR16( fNewAdaptation );
    else
		return float4(fNewAdaptation, fNewAdaptation, fNewAdaptation, 1.0f);
}

//-----------------------------------------------------------------------------
// Name: BrightPassFilterPS
// Type: Pixel shader                                      
// Desc: Perform a high-pass filter on the source texture
//-----------------------------------------------------------------------------
float4 BrightPassFilterPS(in float2 vScreenPosition : TEXCOORD0,
						  uniform bool RGBE8,
						  uniform bool RGB16) : COLOR0
{
	float4 vColor = tex2D( s0, vScreenPosition );
	float4 vLum = tex2D( s1, float2(0.5f, 0.5f) );
	float fAdaptedLum = vLum.r;
	float4 vSample = vColor;
	if( RGBE8 )
	{
		fAdaptedLum = DecodeRE8( vLum );
		vSample = DecodeRE8(vColor);
	}
    else if( RGB16 )
	{
		fAdaptedLum = DecodeR16( vLum );
		vSample = DecodeR16(vColor);
	}
	
	// Determine what the pixel's value will be after tone-mapping occurs
	vSample.rgb *= MIDDLE_GRAY /(fAdaptedLum + 0.001f);
	//vSample *= (1.0f + vSample/LUM_WHITE);
	
	// Subtract out dark pixels
	vSample.rgb -= BRIGHT_PASS_THRESHOLD;
	
	// Clamp to 0
	vSample = max(vSample, 0.0f);
				
	 // Map the resulting value into the 0 to 1 range. Higher values for
	// BRIGHT_PASS_OFFSET will isolate lights from illuminated scene 
	// objects.
	vSample.rgb /= (BRIGHT_PASS_OFFSET + vSample);
    
	 //vSample *= (1.0f + vSample/LUM_WHITE);
	 //vSample /= (1.0f + vSample);	
	
	return vSample;
}

//-----------------------------------------------------------------------------
// Name: FinalScenePassPS
// Type: Pixel shader                                      
// Desc: Perform blue shift, tone map the scene, and add post-processed light
//       effects
//-----------------------------------------------------------------------------
float4 FinalScenePassPS(in float2 vScreenPosition : TEXCOORD0,
						uniform bool RGBE8,
						uniform bool RGB16) : COLOR
{
    float4 vSample  = tex2D(s0, vScreenPosition);
	float4 vLum 	= tex2D(s1, float2(0.5f, 0.5f));
    float4 vBloom   = tex2D(s2, vScreenPosition);  
	float4 vStar	= tex2D(s3, vScreenPosition);	
	
	if( RGBE8 )
    {
        vSample.rgb = DecodeRGBE8( vSample ); 
        vLum.r = DecodeRE8( vLum );
    }
    else if( RGB16 )
    {
        vSample.rgb = DecodeRGB16( vSample ); 
        vLum.r = DecodeR16( vLum );
    }

	// For very low light conditions, the rods will dominate the perception
    // of light, and therefore color will be desaturated and shifted
    // towards blue.
	[branch]
    if( EnableBlueShift )
    {
		// Define a linear blending from -1.5 to 2.6 (log scale) which
		// determines the lerp amount for blue shift
        float fBlueShiftCoefficient = 1.0f - (vLum.r + 1.5)/4.1;
        fBlueShiftCoefficient = saturate(fBlueShiftCoefficient);

		// Lerp between current color and blue, desaturated copy
        float3 vRodColor = dot( (float3)vSample, LUMINANCE_VECTOR ) * BLUE_SHIFT_VECTOR;
        vSample.rgb = lerp( (float3)vSample, vRodColor, fBlueShiftCoefficient );
    }
    
	
    // Map the high range of color values into a range appropriate for
    // display, taking into account the user's adaptation level, and selected
    // values for for middle gray and white cutoff.    
	vSample.rgb *= MIDDLE_GRAY / (vLum.r + 0.001f);
	vSample.rgb /= (1.0f + vSample);
	
	//vSample.rgb *= (1.0f + vSample / LUM_WHITE);	

    
    // Add the bloom post processing effects   
    vSample += BLOOM_BLEND * vBloom;
	vSample += STAR_BLEND * vStar;
    return vSample;
}

//-----------------------------------------------------------------------------
// Name: DownScale4x4
// Type: Technique                                     
// Desc: Scale the source texture down to 1/16 scale
//-----------------------------------------------------------------------------
technique DownScale4x4
{
    pass P0
    {
        PixelShader  = compile ps_3_0 DownScale4x4PS();
    }
}

//-----------------------------------------------------------------------------
// Name: DownScale2x2
// Type: Technique                                     
// Desc: Scale the source texture down to 1/4 scale
//-----------------------------------------------------------------------------
technique DownScale2x2
{
    pass P0
    {
        PixelShader  = compile ps_3_0 DownScale2x2PS();
    }
}
//-----------------------------------------------------------------------------
// Name: GaussBlur5x5
// Type: Technique                                     
// Desc: Simulate a 5x5 kernel gaussian blur by sampling the 12 points closest
//       to the center point.
//-----------------------------------------------------------------------------
technique GaussBlur5x5
{
    pass P0
    {
        PixelShader  = compile ps_3_0 GaussBlur5x5PS();
    }
}
//-----------------------------------------------------------------------------
// Name: Bloom
// Type: Technique                                     
// Desc: Performs a single horizontal or vertical pass of the blooming filter
//-----------------------------------------------------------------------------
technique Bloom
{
    pass P0
    {        
        PixelShader  = compile ps_3_0 BloomPS();
    }

}
//-----------------------------------------------------------------------------
// Name: Star
// Type: Technique                                     
// Desc: Each star is composed of up to 8 lines, and each line is created by
//       up to three passes of this shader, which samples from 8 points along
//       the current line.
//-----------------------------------------------------------------------------
technique Star
{
    pass P0
    {        
        PixelShader  = compile ps_3_0 StarPS();
    }

}

//-----------------------------------------------------------------------------
// Name: SampleAvgLum
// Type: Technique                                     
// Desc: Takes the HDR Scene texture as input and starts the process of 
//       determining the average luminance by converting to grayscale, taking
//       the log(), and scaling the image to a single pixel by averaging sample 
//       points.
//-----------------------------------------------------------------------------
technique SampleAvgLum_FP16
{
    pass P0
    {
        PixelShader  = compile ps_3_0 SampleLumInitial(false,false);
    }
}
//-----------------------------------------------------------------------------
// Name: ResampleAvgLum
// Type: Technique                                     
// Desc: Continue to scale down the luminance texture
//-----------------------------------------------------------------------------
technique ResampleAvgLum_FP16
{
    pass P0
    {
        PixelShader  = compile ps_3_0 SampleLumIterative(false,false);
    }
}
//-----------------------------------------------------------------------------
// Name: ResampleAvgLumExp
// Type: Technique                                     
// Desc: Sample the texture to a single pixel and perform an exp() to complete
//       the evalutation
//-----------------------------------------------------------------------------
technique ResampleAvgLumExp_FP16
{
    pass P0
    {
        PixelShader  = compile ps_3_0 SampleLumFinal(false,false);
    }
}
//-----------------------------------------------------------------------------
// Name: CalculateAdaptedLum
// Type: Technique                                     
// Desc: Determines the level of the user's simulated light adaptation level
//       using the last adapted level, the current scene luminance, and the
//       time since last calculation
//-----------------------------------------------------------------------------
technique CalculateAdaptedLum_FP16
{
    pass P0
    {
        PixelShader  = compile ps_3_0 CalculateAdaptedLumPS(false, false);
    }
}
//-----------------------------------------------------------------------------
// Name: BrightPassFilter
// Type: Technique                                     
// Desc: Perform a high-pass filter on the source texture
//-----------------------------------------------------------------------------
technique BrightPassFilter_FP16
{
    pass P0
    {
        PixelShader  = compile ps_3_0 BrightPassFilterPS(false, false);
    }
}
//-----------------------------------------------------------------------------
// Name: FinalScenePass
// Type: Technique                                     
// Desc: Minimally transform and texture the incoming geometry
//-----------------------------------------------------------------------------
technique FinalScenePass_FP16
{
    pass P0
    {
        PixelShader  = compile ps_3_0 FinalScenePassPS(false, false);
    }
}

//-----------------------------------------------------------------------------
// Name: MergeTextures_NPS
// Type: Pixel shader                                      
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
float4 MergeTextures_1PS
	(
	in float2 vScreenPosition : TEXCOORD0
	) : COLOR
{
	float4 vColor = 0.0f;
	
	vColor += SampleWeights[0] * tex2D(s0, vScreenPosition);
		
	return vColor;
}




//-----------------------------------------------------------------------------
// Name: MergeTextures_NPS
// Type: Pixel shader                                      
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
float4 MergeTextures_2PS
	(
	in float2 vScreenPosition : TEXCOORD0
	) : COLOR
{
	float4 vColor = 0.0f;
	
	vColor += SampleWeights[0] * tex2D(s0, vScreenPosition);
	vColor += SampleWeights[1] * tex2D(s1, vScreenPosition);
		
	return vColor;
}




//-----------------------------------------------------------------------------
// Name: MergeTextures_NPS
// Type: Pixel shader                                      
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
float4 MergeTextures_3PS
	(
	in float2 vScreenPosition : TEXCOORD0
	) : COLOR
{
	float4 vColor = 0.0f;
	
	vColor += SampleWeights[0] * tex2D(s0, vScreenPosition);
	vColor += SampleWeights[1] * tex2D(s1, vScreenPosition);
	vColor += SampleWeights[2] * tex2D(s2, vScreenPosition);
		
	return vColor;
}




//-----------------------------------------------------------------------------
// Name: MergeTextures_NPS
// Type: Pixel shader                                      
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
float4 MergeTextures_4PS
	(
	in float2 vScreenPosition : TEXCOORD0
	) : COLOR
{
	float4 vColor = 0.0f;
	
	vColor += SampleWeights[0] * tex2D(s0, vScreenPosition);
	vColor += SampleWeights[1] * tex2D(s1, vScreenPosition);
	vColor += SampleWeights[2] * tex2D(s2, vScreenPosition);
	vColor += SampleWeights[3] * tex2D(s3, vScreenPosition);
		
	return vColor;
}




//-----------------------------------------------------------------------------
// Name: MergeTextures_NPS
// Type: Pixel shader                                      
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
float4 MergeTextures_5PS
	(
	in float2 vScreenPosition : TEXCOORD0
	) : COLOR
{
	float4 vColor = 0.0f;
	
	vColor += SampleWeights[0] * tex2D(s0, vScreenPosition);
	vColor += SampleWeights[1] * tex2D(s1, vScreenPosition);
	vColor += SampleWeights[2] * tex2D(s2, vScreenPosition);
	vColor += SampleWeights[3] * tex2D(s3, vScreenPosition);
	vColor += SampleWeights[4] * tex2D(s4, vScreenPosition);
		
	return vColor;
}




//-----------------------------------------------------------------------------
// Name: MergeTextures_NPS
// Type: Pixel shader                                      
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
float4 MergeTextures_6PS
	(
	in float2 vScreenPosition : TEXCOORD0
	) : COLOR
{
	float4 vColor = 0.0f;
	
	vColor += SampleWeights[0] * tex2D(s0, vScreenPosition);
	vColor += SampleWeights[1] * tex2D(s1, vScreenPosition);
	vColor += SampleWeights[2] * tex2D(s2, vScreenPosition);
	vColor += SampleWeights[3] * tex2D(s3, vScreenPosition);
	vColor += SampleWeights[4] * tex2D(s4, vScreenPosition);
	vColor += SampleWeights[5] * tex2D(s5, vScreenPosition);
		
	return vColor;
}




//-----------------------------------------------------------------------------
// Name: MergeTextures_NPS
// Type: Pixel shader                                      
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
float4 MergeTextures_7PS
	(
	in float2 vScreenPosition : TEXCOORD0
	) : COLOR
{
	float4 vColor = 0.0f;
	
	vColor += SampleWeights[0] * tex2D(s0, vScreenPosition);
	vColor += SampleWeights[1] * tex2D(s1, vScreenPosition);
	vColor += SampleWeights[2] * tex2D(s2, vScreenPosition);
	vColor += SampleWeights[3] * tex2D(s3, vScreenPosition);
	vColor += SampleWeights[4] * tex2D(s4, vScreenPosition);
	vColor += SampleWeights[5] * tex2D(s5, vScreenPosition);
	vColor += SampleWeights[6] * tex2D(s6, vScreenPosition);
		
	return vColor;
}




//-----------------------------------------------------------------------------
// Name: MergeTextures_NPS
// Type: Pixel shader                                      
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
float4 MergeTextures_8PS
	(
	in float2 vScreenPosition : TEXCOORD0
	) : COLOR
{
	float4 vColor = 0.0f;
	
	vColor += SampleWeights[0] * tex2D(s0, vScreenPosition);
	vColor += SampleWeights[1] * tex2D(s1, vScreenPosition);
	vColor += SampleWeights[2] * tex2D(s2, vScreenPosition);
	vColor += SampleWeights[3] * tex2D(s3, vScreenPosition);
	vColor += SampleWeights[4] * tex2D(s4, vScreenPosition);
	vColor += SampleWeights[5] * tex2D(s5, vScreenPosition);
	vColor += SampleWeights[6] * tex2D(s6, vScreenPosition);
	vColor += SampleWeights[7] * tex2D(s7, vScreenPosition);
		
	return vColor;
}

//-----------------------------------------------------------------------------
// Name: MergeTextures_N
// Type: Technique                                     
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
technique MergeTextures_1
{
    pass P0
    {        
        PixelShader  = compile ps_2_0 MergeTextures_1PS();
    }

}




//-----------------------------------------------------------------------------
// Name: MergeTextures_N
// Type: Technique                                     
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
technique MergeTextures_2
{
    pass P0
    {        
        PixelShader  = compile ps_2_0 MergeTextures_2PS();
    }

}




//-----------------------------------------------------------------------------
// Name: MergeTextures_N
// Type: Technique                                     
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
technique MergeTextures_3
{
    pass P0
    {        
        PixelShader  = compile ps_2_0 MergeTextures_3PS();
    }

}




//-----------------------------------------------------------------------------
// Name: MergeTextures_N
// Type: Technique                                     
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
technique MergeTextures_4
{
    pass P0
    {        
        PixelShader  = compile ps_2_0 MergeTextures_4PS();
    }

}




//-----------------------------------------------------------------------------
// Name: MergeTextures_N
// Type: Technique                                     
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
technique MergeTextures_5
{
    pass P0
    {        
        PixelShader  = compile ps_2_0 MergeTextures_5PS();
    }

}




//-----------------------------------------------------------------------------
// Name: MergeTextures_N
// Type: Technique                                     
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
technique MergeTextures_6
{
    pass P0
    {        
        PixelShader  = compile ps_2_0 MergeTextures_6PS();
    }

}




//-----------------------------------------------------------------------------
// Name: MergeTextures_N
// Type: Technique                                     
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
technique MergeTextures_7
{
    pass P0
    {        
        PixelShader  = compile ps_2_0 MergeTextures_7PS();
    }

}
//-----------------------------------------------------------------------------
// Name: MergeTextures_N
// Type: Technique                                     
// Desc: Return the average of N input textures
//-----------------------------------------------------------------------------
technique MergeTextures_8
{
    pass P0
    {        
        PixelShader  = compile ps_2_0 MergeTextures_8PS();
    }

}