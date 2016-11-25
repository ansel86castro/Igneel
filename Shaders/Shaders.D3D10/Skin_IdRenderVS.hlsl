#include "Skinned.hlsli"

struct VSOut
{
	float4 Position : SV_POSITION;
	float4 Color	:COLOR0;
};


VSOut main(float4 position : POSITION,
			float4  BlendWeights : BLENDWEIGHT,
			float4  BlendIndices : BLENDINDICES)
{
	gPositionH = position;    
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedP();	
    
	VSOut v;
	v.Position = gPositionH;
	v.Color = float4(0,0,0,1);
	return v;	
}
