
struct PSIn
{
    float4 position : SV_POSITION; 
    float2 texCoord : TEXCOORD0; 
};


Texture2D DiffuseMap :register(t0);
SamplerState sDiffuseMap :register(s0);

float4 main(PSIn i) : SV_TARGET
{
	//return float4(1,0,0,1);
	return DiffuseMap.Sample(sDiffuseMap, i.texCoord);
}