#include "Common.hlsli"
#include "SkinnedShadowed.hlsli"

SMBumpVSOutput main(float4 position : POSITION,
					float4  BlendIndices : BLENDINDICES,
					float4  BlendWeights : BLENDWEIGHT,
					float3 tangent  : TANGENT,
					float3 normal   : NORMAL,
					float2 texCoord : TEXCOORD0,
					float occ		  : TEXCOORD1)
{
	SMBumpVSOutput output = (SMBumpVSOutput)0;

	gPositionH = position; 
	gNormalW = normal;
	gTangentW = tangent;
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedPNTL();	
	TransformScreeen();
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TangentW = gTangentW;
	output.BinormalW = gBinormalW;	
	output.PositionL = gPositionL;
	output.TexCoord = texCoord;
	output.Occ = 1 - occ;
	output.ScreenCoord = gScreenCoord;
	
	return output;
}