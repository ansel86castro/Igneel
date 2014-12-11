#include "Transform.hlsli"

struct VSIn
{
	float4 Position :POSITION;
	float2 texCoord :TEXCOORD0;
};

struct PSIn
{
	float4 Position : SV_POSITION;
	float2 poszw    : TEXCOORD0;
	float2 texCoord : TEXCOORD1;
};

PSIn main(VSIn input)
{
	gPositionH = input.Position;
	TransformP();

	PSIn output;
	output.Position = gPositionH;
	output.poszw    = float2(gPositionH.z , gPositionH.w);
	output.texCoord = input.texCoord; 	
	return output;
}