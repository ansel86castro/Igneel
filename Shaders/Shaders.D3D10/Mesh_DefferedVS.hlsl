#include "Common.hlsli"
#include "Transform.hlsli"

struct PSIn
{
	float4 PositionH    : SV_POSITION;
	float4 PositionRhz  : TEXCOORD0;	
	float4 NormalW	    : TEXCOORD1;
	float2 Texcoord	    : TEXCOORD2;	
};

PSIn main(	float4 position : POSITION, 
			float3 normal	: NORMAL, 
			float2 texCoord : TEXCOORD0,
			float occ		: TEXCOORD1 )
{
	PSIn output = (PSIn)0;	
	
	gPositionH = position;    
	gNormalW = normal;
	
	TransformPN();	
	
	output.PositionH = gPositionH;
	output.PositionRhz = gPositionH;		
	//output.PositionRhz = float4(gPositionW ,1);		
	output.NormalW = float4(gNormalW, 1 - occ);
	output.Texcoord = texCoord;		
    return output;
}