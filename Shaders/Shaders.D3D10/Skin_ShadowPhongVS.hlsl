#include "Common.hlsli"
#include "SkinnedShadowed.hlsli"

SMVSOutput main(float4 position : POSITION, 
				float3 normal	 : NORMAL, 
				float2 texCoord : TEXCOORD0,
				float occ:TEXCOORD1,
				float4  BlendIndices : BLENDINDICES,
				float4  BlendWeights : BLENDWEIGHT)
{
    SMVSOutput output = (SMVSOutput)0;

	gPositionH = position;    
	gNormalW = normal;
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedPNL();
	TransformScreeen();
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TexCoord = texCoord;
	output.PositionL = gPositionL;
	output.Occ = 1 - occ;
	output.ScreenCoord = gScreenCoord;
	
    return output;
}