#include "Common.hlsli"
#include "ShadowMapTransform.hlsli"

SMVSOutput main(float4 position : POSITION, 
				float3 normal	 : NORMAL, 
				float2 texCoord : TEXCOORD0,
				float occ:TEXCOORD1 )
{
	SMVSOutput output = (SMVSOutput)0;

	gPositionH = position;    
	gNormalW = normal;
	
	TransformPNL();	
	TransformScreeen();
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.PositionL = gPositionL;
	output.TexCoord = texCoord;
	output.Occ = 1 - occ;
	output.ScreenCoord = gScreenCoord;
    return output;
}