#include "Transform.hlsli"

struct VSOut
{
	float4 Position : SV_POSITION;
	float4 Color	:COLOR0;
};

VSOut main(float4 position  : POSITION  ) 
{
    gPositionH = position;
	TransformP();

	VSOut o;
	o.Position = gPositionH;
	o.Color = float4(0,0,0,1);	
	return o;
}