#include "Transform.hlsli"

struct VSOut
{
	float4 Position : SV_POSITION;
	float4 Color	:COLOR0;
};

cbuffer c0
{
	float4 gId;
}

VSOut main(float4 position  : POSITION  ) 
{
    gPositionH = position;
	TransformP();

	VSOut o;
	o.Position = gPositionH;
	o.Color = gId;
	return o;
}