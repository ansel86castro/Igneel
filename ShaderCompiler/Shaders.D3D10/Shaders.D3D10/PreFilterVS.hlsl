#include "ShadowMapTransform.hlsli"

struct VSOut
{
	float4 PositionH :SV_POSITION;
	float4 PositionL :TEXCOORD0;
};

VSOut main(float4 position : POSITION)
{
	VSOut output = (VSOut)0;

	gPositionH = position;    		
	TransformPL();		
	
	output.PositionH = gPositionH;	
	output.PositionL = gPositionL;	
    return output;
}