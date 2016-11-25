
sampler2D s0:register(s0);

float4 CheckAlphaChannel(float2 texCoord:TEXCOORD0):COLOR
{
	float4 color = tex2D(s0, texCoord);
	
	if(color.a == 1.0f)
		discard;
	
	return	float4(1,1,1,1);
}

// technique Technique0
// {
    // pass Pass1
	// {
		// ALPHABLENDENABLE = FALSE;
		// SRCBLEND = ONE;
		// DESTBLEND = ONE;
		// PixelShader = compile ps_2_0 CheckAlphaChannel();
	// }
// }