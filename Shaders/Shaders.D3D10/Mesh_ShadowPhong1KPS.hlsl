#include "Common.hlsli"
#include "Lighting.hlsli"
#include "LigthingShadowed.hlsli"

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
	
	gShadowFactor = ShadowMap.SampleCmpLevelZero(sShadowMap, gShadowTexCoord.xy , gShadowTexCoord.z);

	ComputeLighting();

	return  gColor;
}