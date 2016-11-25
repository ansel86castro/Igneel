#include "Common.hlsli"
#include "Skinned.hlsli"

SimpleVSOutput main(float4 position  : POSITION, 
					float4  BlendWeights : BLENDWEIGHT,
					float4  BlendIndices : BLENDINDICES,
					float3 normal	    : NORMAL, 
					float2 texCoord      : TEXCOORD0,
					float occ 		    : TEXCOORD1)
{
	   	
	SimpleVSOutput output = (SimpleVSOutput)0;	
	
	gPositionH = position;    
	gNormalW = normal;
	
	InitSkinning(BlendWeights, BlendIndices);	
	TransformSkinnedPN();			
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TexCoord = texCoord;
	output.Occ = 1 - occ;
   
    return output;
}