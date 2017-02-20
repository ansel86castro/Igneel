#include "Common.hlsli"
#include "Transform.hlsli"

struct BlendVSOutput
{
	float4 PositionH 	 :SV_POSITION;
	float3 PositionW 	 :TEXCOORD0;
	float3 NormalW	 	 :TEXCOORD1;
	float2 TexCoord	  	 :TEXCOORD2;
	float2 BlendCoord :TEXCOORD3;	
};

cbuffer PerSection {
	float2 Offset;
};

BlendVSOutput main(float2 position  : POSITION0,
				float height : POSITION1,
				float3 normal : NORMAL,
				float2 texCoord : TEXCOORD0,
				float2 blendCoord : TEXCOORD1)
{
	float3 pos;
	BlendVSOutput output = (BlendVSOutput)0;

	pos.xz = position + Offset;
	pos.y = height;

	gPositionH = float4(pos, 1);
	gNormalW = normal;

	TransformPN();
	TransformScreeen();

	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TexCoord = texCoord;	
	output.BlendCoord = blendCoord;
	return output;
}