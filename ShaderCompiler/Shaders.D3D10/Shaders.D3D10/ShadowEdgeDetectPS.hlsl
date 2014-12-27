struct PS_RenderQuadInput
{
    float4 position : SV_POSITION; 
    float2 texCoord : TEXCOORD0; 
};

Texture2D t0 : register(t0);
SamplerState s0 : register(s0);

float4 main(PS_RenderQuadInput input) : SV_TARGET
{
	float inShadow = t0.Sample(s0, input.texCoord).r;
	float edge =saturate(abs(ddx(inShadow)) + abs(ddy(inShadow)));	
	return float4(edge,edge,edge,1);
}