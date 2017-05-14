#define KERNEL_SIZE 3

#include "Lighting.hlsli"
#include "LigthingShadowed.hlsli"

struct SMVSOutput
{
	float4 PositionH : SV_POSITION;
	float3 PositionW : TEXCOORD0;
	float3 NormalW	 : TEXCOORD1;
	float2 TexCoord	 : TEXCOORD2;
	float4 PositionL : TEXCOORD3;
	float Occ : TEXCOORD4;
	float4 ScreenCoord : TEXCOORD5;
};

float4 main(SMVSOutput input) : SV_TARGET
{   	
	clip(dot(float4(input.PositionW, 1), ClipPlane));
	
	gPositionW   = input.PositionW;
	gNormalW     = input.NormalW;
	gTexCoord    = input.TexCoord;
	gOcc         = input.Occ;
	gPositionL	 = input.PositionL;
	gScreenCoord = input.ScreenCoord;
	
	//ComputeShadowTexCoord();
	float3 posL = gPositionL.xyz / gPositionL.w;
	gShadowTexCoord = float4(posL.x * 0.5f + 0.5f, -posL.y * 0.5f + 0.5f, posL.z, 1);


	float costheta = saturate(dot(gNormalW, -Light.Dir));
	float bias = 0.005*tan(acos(costheta));

	gShadowTexCoord.z -= SHADOW_EPSILON + bias;

	//ComputeShadowFactor();	
	//compute shadow factor
	gShadowFactor = 0;

	int start = -KERNEL_SIZE / 2;
	int end = KERNEL_SIZE / 2 + 1;

	float width, height;
	ShadowMap.GetDimensions(width, height);
	float tx = 1.0 / width;
	float ty = 1.0 / height;

	[unroll]
	for (float y = start; y < end; y++)
	{
		[unroll]
		for (float x = start; x < end; x++)
		{
			gShadowFactor += ShadowMap.SampleCmpLevelZero(sShadowMap, gShadowTexCoord.xy + float2(x*tx, y*ty), gShadowTexCoord.z);
		}
	}

	gShadowFactor /= (float)(KERNEL_SIZE * KERNEL_SIZE);

	ComputeLighting();

	return  gColor;//gShadowFactor;
}