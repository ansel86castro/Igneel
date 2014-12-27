#define KERNEL_SIZE 3

#include "Common.hlsli"
#include "Lighting.hlsli"
#include "LigthingShadowed.hlsli"

Texture2D t8 :register(t8);		// edge texture
SamplerState s8:register(s8);	

float4 main(SMVSOutput input) : SV_TARGET0
{   	
	clip(dot(float4(input.PositionW, 1), clipPlane));
	
	gPositionW   = input.PositionW;
	gNormalW     = input.NormalW;
	gTexCoord    = input.TexCoord;
	gOcc         = input.Occ;
	gPositionL	 = input.PositionL;
	gScreenCoord = input.ScreenCoord;
	
	ComputeShadowTexCoord();

	float edge = t8.Sample(s8, gScreenCoord.xy / gScreenCoord.w).r;

	[branch]
	if(edge > 0)
	 	 return float4(1,0,0,1);//ComputeShadowFactor();	
	else
		gShadowFactor = t7.SampleCmpLevelZero(s7, gShadowTexCoord.xy , gShadowTexCoord.z);	

	ComputeLighting();

	return  gColor;
}