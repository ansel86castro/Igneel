#include "Common.hlsli"
#include "Lighting.hlsli"
#include "LigthingShadowed.hlsli"

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
	
	gShadowFactor = t7.SampleCmpLevelZero(s7, gShadowTexCoord.xy , gShadowTexCoord.z);

	ComputeLighting();

	return  gColor;
}