#include "Skinned.hlsli"

struct VSOut
{
	float4 Position : SV_POSITION;
	float4 Color	:COLOR0;
};


VSOut main( float4  position : POSITION,
			float4  BlendWeights : BLENDWEIGHT,
			float4  BlendIndices : BLENDINDICES,
			float4  color:COLOR0)
{
	gPositionH = position;    
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedP();	

    VSOut output;
	
	output.Position = gPositionH;	
	output.Color = color;
	return output;
}