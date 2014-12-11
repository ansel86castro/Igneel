#include "Skinned.fxh"

float4 gId :COLOR0;

void VS(float4 position : POSITION,
		float4  BlendWeights : BLENDWEIGHT,
		float4  BlendIndices : BLENDINDICES,
		out float4 posOut:POSITION, 
		out float4 id:COLOR0)
{
	gPositionH = position;    
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedPN();	
    
	posOut = gPositionH;
	id = gId;
}

float4 PS(float4 id:COLOR0):COLOR
{
	return id;
}

// VertexShader skinnedVSArray[4] = {  compile vs_1_1 VS(1), 
									// compile vs_1_1 VS(2),
									// compile vs_1_1 VS(3),
									// compile vs_1_1 VS(4)
								 // };

								 
// int vsIndex;

// technique T0
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.					
        // VertexShader = skinnedVSArray[vsIndex];
        // PixelShader = compile ps_2_0 PS();
    // }
// }
