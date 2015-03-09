
float4x4 invViewProj;

// we store position in the fat framebuffer as post-perspective space 
// the Light shader (usually) converts back into world space where lighting takes place
// the range of the position is 0-1 to maximise integer usage
float4 UnPackPositionFromFatFramebuffer( float3 inp )
{
	float4 o;
	//o.xyzw = mul( float4(inp,1), matProjectionInvScaled );
	inp.xy =  (inp.xy - 0.5) * 2.0f;
	o = mul(float4(inp,1),  invViewProj);
	o.xyz = o.xyz / o.w;

	return o;
}

float3 UnPackNormalFromFatFramebuffer( float3 inp )
{
	return (inp - 0.5) * 2.0;
}

float4 UnPackNormalFromFatFramebuffer( float4 inp )
{
	return (inp - 0.5) * 2.0;
}

// pixel functions and variables that are used in multiple geometry shaders
// included from the actual fx file

// we store position in the fat framebuffer as post-perspective space 
// the Light shader (usually) converts back into world space where lighting takes place
// the range of the position is 0-1 to maximise integer usage
float3 PackPositionForFatFramebuffer( float4 inp )
{
	float3 o;

	o.xyz = inp.xyz / inp.w;
	o.xy = (o.xy * 0.5) + 0.5;
	return o;
}

float3 PackNormalForFatFramebuffer( float3 inp )
{
	return (inp * 0.5) + 0.5;
}

float4 PackNormalForFatFramebuffer( float4 inp )
{
	return (inp * 0.5) + 0.5;
}