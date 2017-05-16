#define KERNEL_SIZE 3

#include "Common.hlsli"
#include "Lighting.hlsli"
#include "LigthingShadowed.hlsli"

Texture2D EdgeTexture;		// edge texture
SamplerState sEdgeTexture;	

float4 main(SMVSOutput input) : SV_TARGET0
{   	
	clip(dot(float4(input.PositionW, 1), ClipPlane));
	
	gPositionW   = input.PositionW;
	gNormalW     = input.NormalW;
	gTexCoord    = input.TexCoord;
	gOcc         = input.Occ;
	gPositionL	 = input.PositionL;
	gScreenCoord = input.ScreenCoord;
	
	ComputeShadowTexCoord(-Light.Dir);

	float edge = EdgeTexture.Sample(sEdgeTexture, gScreenCoord.xy / gScreenCoord.w).r;

	[branch]
	if(edge > 0)
	 	 return float4(1,0,0,1);//ComputeShadowFactor();	
	else
		gShadowFactor = ShadowMap.SampleCmpLevelZero(sShadowMap, gShadowTexCoord.xy , gShadowTexCoord.z);	

	ComputeLighting();

	return  gColor;
}