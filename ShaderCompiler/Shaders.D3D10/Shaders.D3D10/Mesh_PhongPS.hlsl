#include "Common.hlsli"
#include "Lighting.hlsli"

float4 main(SimpleVSOutput input) : SV_TARGET
{
	clip(dot(float4(input.PositionW, 1), clipPlane));
	
	gScreenCoord = input.ScreenPosition;
	gPositionW = input.PositionW;
	gNormalW   = input.NormalW;
	gTexCoord  = input.TexCoord;
	gOcc     	= input.Occ;
	gShadowFactor = 1;	
	
	ComputeLighting();
	
	return  gColor;				
}