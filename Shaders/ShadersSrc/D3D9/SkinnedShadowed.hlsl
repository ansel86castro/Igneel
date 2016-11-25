#define SKINNED_SHADOWED

#ifndef LIGTHING_SHADOWED
	#include "LigthingShadowed.fxh"
#endif

#ifndef SKINNED
	#include "Skinned.hlsl"
#endif

void TransformSkinnedPNL()
{
	TransformSkinnedPN();
	gPositionL = mul(float4(gPositionW,1) , LightVP);
}

void TransformSkinnedPNTL()
{	
	TransformSkinnedPNT();
	gPositionL = mul(float4(gPositionW,1) , LightVP);
}

SMVSOutput SimpleVertexShader(float4 position : POSITION, 
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

SMBumpVSOutput BumpVertexShader(float4 position : POSITION,
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