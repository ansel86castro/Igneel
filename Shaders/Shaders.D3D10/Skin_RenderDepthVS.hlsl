#include "Skinned.hlsli"

struct PSIn
{
	float4 Position : SV_POSITION;
	float2 poszw    : TEXCOORD0;
	float2 texCoord : TEXCOORD1;
};

PSIn main(float4 position  : POSITION,
	      float2 texCoord  : TEXCOORD0,
	      float4  BlendWeights : BLENDWEIGHT,
	      float4  BlendIndices : BLENDINDICES)
{	
	
	gPositionH = position;  
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedP();		
	
	PSIn output;
	output.Position = gPositionH;
	output.poszw    = float2(gPositionH.z , gPositionH.w);
	output.texCoord = texCoord; 	
	return output;
}