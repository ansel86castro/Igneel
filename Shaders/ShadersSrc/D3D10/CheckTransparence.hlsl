

Texture2D    t0 : register(t0);
SamplerState s0 : register(s0);

float4 main(float2 texCoord:TEXCOORD0): SV_Target
{
	float4 color = t0.Sample(s0, texCoord);
	
	if(color.a == 1.0f)
		discard;
	
	return	float4(1,1,1,1);
}