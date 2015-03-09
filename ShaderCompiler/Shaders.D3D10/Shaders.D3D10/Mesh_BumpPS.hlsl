#include "Common.hlsli"
#include "Lighting.hlsli"

float4 main(BumpVSOutput input) : SV_TARGET
{
	clip(dot(float4(input.PositionW, 1), ClipPlane));
	
	gScreenCoord = input.ScreenPosition;
	gPositionW = input.PositionW;
	gNormalW   = input.NormalW;
	gTangentW = input.TangentW;
	gBinormalW = input.BinormalW;
	gTexCoord  = input.TexCoord;
	gOcc     	= input.Occ;
	gShadowFactor = 1;	
		
	ComputeNormal();	
	ComputeLighting();
	
	return  gColor;	
}