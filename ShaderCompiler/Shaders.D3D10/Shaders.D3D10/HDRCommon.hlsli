#include "Luminance.hlsli"

struct PS_RenderQuadInput
{
    float4 position : SV_POSITION; 
    float2 texCoord : TEXCOORD0; 
};


// The per-color weighting to be used for luminance calculations in RGB order.
static const float3 LUMINANCE_VECTOR  = float3(0.2125f, 0.7154f, 0.0721f);
// The per-color weighting to be used for blue shift under low Light.
static const float3 BLUE_SHIFT_VECTOR = float3(1.05f, 0.97f, 1.27f); 

static const float MIN_FLOAT = -1e10;

cbuffer cbGlobals
{
	float  MIDDLE_GRAY;	
	float  BRIGHT_PASS_THRESHOLD;	
	float  BLOOM_BLEND;
	float  STAR_BLEND;
	float  ElapsedTime;
	bool   EnableBlueShift;
	float  GaussianScalar;
}

cbuffer cbSamples
{
	float4 SampleWeights[16];
	float4 SampleOffsets[8];	
}


static  bool RGBE8 = false;
static	bool RGB16 = false;


Texture2D t0 : register(t0);
Texture2D t1 : register(t1);
Texture2D t2 : register(t2);
Texture2D t3 : register(t3);
Texture2D t4 : register(t4);
Texture2D t5 : register(t5);
Texture2D t6 : register(t6);
Texture2D t7 : register(t7);

SamplerState s0 : register(s0);
SamplerState s1 : register(s1);
SamplerState s2 : register(s2);
SamplerState s3 : register(s3);
SamplerState s4 : register(s4);
SamplerState s5 : register(s5);
SamplerState s6 : register(s6);
SamplerState s7 : register(s7);

