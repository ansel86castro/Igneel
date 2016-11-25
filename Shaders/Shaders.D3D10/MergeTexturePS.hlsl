
#define NUM_TEXTURE 8

struct PS_RenderQuadInput
{
    float4 position : SV_POSITION; 
    float2 texCoord : TEXCOORD0; 
};

cbuffer cb0
{	
	float4 SampleWeights[16];	
};
cbuffer cb1
{
	int NbTextures;
}

Texture2D textures[NUM_TEXTURE] : register(t0);
sampler samplers[NUM_TEXTURE] : register(s0);

float4 main(PS_RenderQuadInput input) : SV_TARGET
{
	float4 color = 0.0;
	[unroll]
	for(int i = 0 ; i< NbTextures; i++)
	{
		color += SampleWeights[i] * textures[i].SampleLevel(samplers[i], input.texCoord, 0);
	}
	color.a = 1;
	return color;
}