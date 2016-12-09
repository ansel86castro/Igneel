
float sampleOffsets[9];
float sampleWeights[9];

sampler     s0  :   register( s0 ); 

//------------------------------------------------------------------
// HORIZONTAL BLUR
//
// Takes 9 samples from the down-sampled texture (4 either side and
// one central) biased by the provided weights. Different weight
// distributions will give more subtle/pronounced blurring.
//------------------------------------------------------------------
float4 HorizontalBlurPS( in float2 t : TEXCOORD0 ) : COLOR
{

    float4 color = { 0.0f, 0.0f, 0.0f, 0.0f };
    
    for( int i = 0; i < 9; i++ )
    {
        color += (tex2D( s0, t + float2( sampleOffsets[i], 0.0f )) * sampleWeights[i]);
    }
        
    return float4( color.rgb, 1.0f );
    
}
            
//------------------------------------------------------------------
// VERTICAL BLUR
//
// Takes 9 samples from the down-sampled texture (4 above/below and
// one central) biased by the provided weights. Different weight
// distributions will give more subtle/pronounced blurring.
//------------------------------------------------------------------
float4 VerticalBlurPS( in float2 t : TEXCOORD0 ) : COLOR
{

    float4 color = { 0.0f, 0.0f, 0.0f, 0.0f };
    
    for( int i = 0; i < 9; i++ )
    {
        color += (tex2D( s0, t + float2(0.0f, sampleOffsets[i])) * sampleWeights[i]);
    }
        
    return float4( color.rgb, 1.0f );
    
}

technique HorizontalBlur
{
    pass P0
    {
        PixelShader  = compile ps_2_0 HorizontalBlurPS();
    }
}

technique VerticalBlur
{
    pass P0
    {
        PixelShader  = compile ps_2_0 VerticalBlurPS();
    }
}
