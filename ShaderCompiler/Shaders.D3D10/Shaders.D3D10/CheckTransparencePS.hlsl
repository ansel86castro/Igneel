

Texture2D    DiffuseMap :register(t0);
SamplerState sDiffuseMap :register(s0);

struct PSIn
{
    float4 position : SV_POSITION; 
    float2 texCoord : TEXCOORD0; 
};

float4 main(PSIn i): SV_Target
{
	float4 color = DiffuseMap.Sample(sDiffuseMap, i.texCoord);
	
	if(color.a == 1.0f)
		discard;
	
	return	float4(1,1,1,1);
}