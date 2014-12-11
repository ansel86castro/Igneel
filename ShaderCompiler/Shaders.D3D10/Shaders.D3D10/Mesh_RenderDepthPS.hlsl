#include "Lighting.hlsli"

struct PSIn
{
	float4 Position : SV_POSITION;
	float2 poszw    : TEXCOORD0;
	float2 texCoord : TEXCOORD1;
};

float4 main(PSIn input) : SV_TARGET0
{
	float alpha = USE_DIFFUSE_MAP ? t0.Sample(s0, input.texCoord).a * surface.Diffuse.a : surface.Diffuse.a;	
	if(alpha == 0)
		discard;	
			
	float z = input.poszw.x / input.poszw.y;
	return float4(z ,z ,z ,1);		
}