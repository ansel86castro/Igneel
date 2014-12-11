
float alpha	 : ALPHA;
sampler2D s0 :register(s0);

void VertexShaderFunction(in float4 positionIn :POSITION,
						  in float2 texCoordIn :TEXCOORD0, 
						  out float4 positionOut :POSITION, 
						  out float2 texCoordOut :TEXCOORD0)
{
    positionOut = positionIn;
	texCoordOut = texCoordIn;
}

float4 PixelShaderFunction(float2 texCoord :TEXCOORD0 ) : COLOR0
{
	float depth	 = tex2D(s0 , texCoord).r;    
	return float4(depth, depth, depth, alpha);
}

float4 PixelShaderFunction2(in float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, texCoord);
	color.a = alpha;
	return color;
}

float4 PixelShaderFunction3(in float2 texCoord : TEXCOORD0) : COLOR0
{
	return tex2D(s0, texCoord);		
}

// technique Technique1
// {
    // pass Pass1
    // {      
		// //Alpha blend Allow Trasnparency depending of the source color alpha		
        // PixelShader = compile ps_2_0 PixelShaderFunction();
    // }
// }

// technique Technique2
// {
    // pass Pass1
	// {
		// PixelShader = compile ps_2_0 PixelShaderFunction2();
	// }
// }

// technique Technique3
// {
    // pass Pass1
	// {
		// PixelShader = compile ps_2_0 PixelShaderFunction3();
	// }
// }
