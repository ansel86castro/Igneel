
float2 SampleOffsets[15];
float4 SampleWeights[15];

sampler s0 : register(s0);


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