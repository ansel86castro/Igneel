#include "Skinned.hlsli"

struct VSOut
{
	float4 Position : SV_POSITION;
	float4 Color	:COLOR0;
};

cbuffer c0
{
	float4 gId;
}


VSOut main(float4 position : POSITION,
			float4  BlendWeights : BLENDWEIGHT,
			float4  BlendIndices : BLENDINDICES)
{
	gPositionH = position;    
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedP();	
    
	VSOut v;
	v.Position = gPositionH;
	v.Color = gId;
	return v;	
}
