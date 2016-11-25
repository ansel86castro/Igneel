#include "Common.hlsli"
#include "Transform.hlsli"

SimpleVSOutput main( float4 position : POSITION, 
					 float3 normal	 : NORMAL, 
			         float2 texCoord : TEXCOORD0,
			         float occ :TEXCOORD1 )
{
	 SimpleVSOutput output = (SimpleVSOutput)0;	
	
	gPositionH = position;    
	gNormalW = normal;
	
	TransformPN();
	TransformScreeen();
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TexCoord = texCoord;
	output.Occ = 1 - occ;
	output.ScreenPosition = gScreenCoord;
    return output;
}