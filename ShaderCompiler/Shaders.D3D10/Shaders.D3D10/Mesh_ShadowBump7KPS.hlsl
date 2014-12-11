#define KERNEL_SIZE 7

#include "Common.hlsli"
#include "Lighting.hlsli"
#include "LigthingShadowed.hlsli"

float4 main(SMBumpVSOutput input) : SV_TARGET
{   	
	clip(dot(float4(input.PositionW, 1), clipPlane));
	
	gScreenCoord = input.ScreenCoord;
	gPositionW = input.PositionW;
	gNormalW   = input.NormalW;
	gTexCoord  = input.TexCoord;
	gOcc      	= input.Occ;
	gPositionL  = input.PositionL;
	gTangentW =  input.TangentW;
	gBinormalW = input.BinormalW;
	
	ComputeShadowTexCoord();
	ComputeShadowFactor();	
	ComputeNormal();
	ComputeLighting();

	return  gColor;
}
