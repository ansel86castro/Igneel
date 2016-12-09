#include "Transform.hlsli"

struct VSOut
{
	float4 Position : SV_POSITION;
	float4 Color	:COLOR0;
};

VSOut main(float4 Position : POSITION,
		   float4 Color	:COLOR0)
{
	gPositionH = Position;
	TransformP();

	VSOut o;
	o.Position = gPositionH;
	o.Color = Color;
	return o;
}