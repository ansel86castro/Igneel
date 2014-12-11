
struct PSIn
{
    float4 position : SV_POSITION; 
    float2 texCoord : TEXCOORD0; 
};


Texture2D t0 :register(t0);
SamplerState s0 :register(s0);

float4 main(PSIn i) : SV_TARGET
{
	//return float4(1,0,0,1);
	return t0.Sample(s0, i.texCoord);
}