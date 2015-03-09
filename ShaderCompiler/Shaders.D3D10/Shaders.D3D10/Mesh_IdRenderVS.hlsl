#include "Transform.hlsli"

struct VSOut
{
	float4 Position : SV_POSITION;
	float4 Color	:COLOR0;
};

cbuffer cbId
{
	float4 Id;
}

VSOut main(float4 position  : POSITION  ) 
{
    gPositionH = position;
	TransformP();

	VSOut o;
	o.Position = gPositionH;
	o.Color = Id;
	return o;
}