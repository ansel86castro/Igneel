#include "Common.hlsli"
#include "Lighting.hlsli"
#include "LigthingShadowed.hlsli"

float4 main(SMBumpVSOutput input) : SV_TARGET
{   	
	clip(dot(float4(input.PositionW, 1), ClipPlane));
	
	gScreenCoord = input.ScreenCoord;
	gPositionW = input.PositionW;
	gNormalW   = input.NormalW;
	gTexCoord  = input.TexCoord;
	gOcc      	= input.Occ;
	gPositionL  = input.PositionL;
	gTangentW =  input.TangentW;
	gBinormalW = input.BinormalW;
	
	ComputeShadowTexCoord();

	gShadowFactor = ShadowMap.SampleCmpLevelZero(sShadowMap, gShadowTexCoord.xy , gShadowTexCoord.z);

	ComputeNormal();
	ComputeLighting();

	return  gColor;
}