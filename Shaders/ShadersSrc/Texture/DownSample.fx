
float2 sampleOffsets[16] : SAMPLER_OFFSET;

sampler	s0  : register( s0 );

//------------------------------------------------------------------
//  This entry point will, using a 3x3 set of reads will downsample
//  from one luminance map to another.
//------------------------------------------------------------------
float4 DownSample2x2PS( in float2 t : TEXCOORD0 ) : COLOR0
{       
    float4 color = 0.0f;      		        
    for( int i = 0; i < 4; i++ )
    {
         color += tex2D( s0, t + sampleOffsets[i]);            
    }          
    color /= 4.0f;            
    return color;

}
//------------------------------------------------------------------
//  This entry point will, using a 3x3 set of reads will downsample
//  from one luminance map to another.
//------------------------------------------------------------------
float4 DownSample3x3PS( in float2 t : TEXCOORD0 ) : COLOR0
{
    
    float4 color = 0.0f;      		        
    for( int i = 0; i < 9; i++ )
    {
         color += tex2D( s0, t + sampleOffsets[i]);            
    }          
    color /= 9.0f;            
    return color;

}

//------------------------------------------------------------------
//  This entry point will, using a 3x3 set of reads will downsample
//  from one luminance map to another.
//------------------------------------------------------------------
float4 DownSample4x4PS( in float2 t : TEXCOORD0 ) : COLOR0
{
    
    float4 color = 0.0f;      		        

    for( int i = 0; i < 16; i++ )
    {
         color += tex2D( s0, t + sampleOffsets[i]);            
    }          

    color /= 16.0f; 
	           
    return color;
}

technique DownSample2x2
{
    pass P0
    {
        PixelShader  = compile ps_2_0 DownSample2x2PS();
    }
}

technique DownSample3x3
{
    pass P0
    {
        PixelShader  = compile ps_2_0 DownSample3x3PS();
    }
}

technique DownSample4x4
{
    pass P0
    {
        PixelShader  = compile ps_2_0 DownSample4x4PS();
    }
}