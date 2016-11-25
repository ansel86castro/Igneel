#include "StdLigthing.fxh"

float4 gId :COLOR0;

void VS(float4 position : POSITION , out float4 posOut:POSITION, out float4 id:COLOR0)
{
    float4 posW = mul(position, World);
	posOut =  mul(posW, ViewProj);	
	id = gId;
}

float4 PS(float4 id:COLOR0):COLOR
{
	return id;
}

// technique T0
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.					
        // VertexShader = compile vs_1_1 VS();
        // PixelShader = compile ps_2_0 PS();
    // }
// }
