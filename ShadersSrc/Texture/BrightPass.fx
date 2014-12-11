
float brightPassThreshold;
float2 sampleOffsets[4];

sampler	s0	:	register(s0);

//------------------------------------------------------------------
// BRIGHT PASS AND 2x2 DOWN-SAMPLING PIXEL SHADER
//
// performs the 2x2 down sample, and then accepts any pixels
// that are greater or equal to the configured threshold
//------------------------------------------------------------------
float4 BrightPass_PS( in float2 t : TEXCOORD0 ) : COLOR
{    
    float4 color = tex2D( s0, t);
	float luminance = 0;

	// Determine the brightness of this particular pixel. As with the luminance calculations
	// there are 4 possible variations on this calculation:    
	// 1. Do a very simple mathematical average:	
	//float luminance = dot( color.rgb, float3( 0.33f, 0.33f, 0.33f ) );
    
	// 2. Perform a more accurately weighted average:				
	//float luminance = dot( color.rgb, float3( 0.299f, 0.587f, 0.114f ) );
    
	// 3. Take the maximum value of the incoming, same as computing the
	//    brightness/value for an HSV/HSB conversion:		
			luminance = max( color.r, max( color.g, color.b ) );
    
	// 4. Compute the luminance component as per the HSL colour space:	
	//float luminance = 0.5f * ( max( color.r, max( color.g, color.b ) ) + min( color.r, min( color.g, color.b ) ) );
    
	// 5. Use the magnitude of the colour		
	//float luminance = length( color.rgb );
            
    // Determine whether this pixel passes the test...
        if( luminance < brightPassThreshold )
            color = float4( 0.0f, 0.0f, 0.0f, 1.0f );
    
    // Write the colour to the bright-pass render target
        return color;    
}

//------------------------------------------------------------------
// BRIGHT PASS AND 2x2 DOWN-SAMPLING PIXEL SHADER
//
// performs the 2x2 down sample, and then accepts any pixels
// that are greater or equal to the configured threshold
//------------------------------------------------------------------
float4 BrightPassDS_PS( in float2 t : TEXCOORD0 ) : COLOR
{    
		float4 color = { 0.0f, 0.0f, 0.0f, 0.0f };

		// load in and combine the 4 samples from the source HDR texture
		for( int i = 0; i < 4; i++ )
		{
			color += tex2D( s0, t + sampleOffsets[i]);
		}            
		color *= 0.25f;	

	// Determine the brightness of this particular pixel. As with the luminance calculations
	// there are 4 possible variations on this calculation:    
	// 1. Do a very simple mathematical average:	
	//float luminance = dot( color.rgb, float3( 0.33f, 0.33f, 0.33f ) );
    
	// 2. Perform a more accurately weighted average:				
	//float luminance = dot( color.rgb, float3( 0.299f, 0.587f, 0.114f ) );
    
	// 3. Take the maximum value of the incoming, same as computing the
	//    brightness/value for an HSV/HSB conversion:		
		float luminance = max( color.r, max( color.g, color.b ) );
    
	// 4. Compute the luminance component as per the HSL colour space:	
	//float luminance = 0.5f * ( max( color.r, max( color.g, color.b ) ) + min( color.r, min( color.g, color.b ) ) );
    
	// 5. Use the magnitude of the colour		
	//float luminance = length( color.rgb );
            
    // Determine whether this pixel passes the test...
        if( luminance < brightPassThreshold )
            color = float4( 0.0f, 0.0f, 0.0f, 1.0f );
    
    // Write the colour to the bright-pass render target
        return color;    
}

technique BrightPassDS
{
    pass P0
    {
        PixelShader  = compile ps_2_0 BrightPassDS_PS();
    }
}

technique BrightPass
{
    pass P0
    {
        PixelShader  = compile ps_2_0 BrightPass_PS();
    }
}