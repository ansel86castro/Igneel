#include "../Include/SkinnedShadowed.fxh"

int DESCRIPTION
<
	string ShaderModel = "3.0";
	string Version = "1.0.0";
	int NumberOfLights = 0;				
>;

//************************* Simple Phong Per Pixel Lighting **************************************

void VS(in float4 position : POSITION, 
						float4  BlendWeights : BLENDWEIGHT,
						float4  BlendIndices : BLENDINDICES,
						out float4 positionH :POSITION, 
						out float3 positionW:TEXCOORD0,
						out float4 positionL:TEXCOORD1,
						uniform int NumBones)
{   
	gPositionH = position;    	
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedPNL(NumBones);	
	
	positionH = gPositionH;
	positionW = gPositionW;	
	positionL = gPositionL;	    
}

float4 PS(float3 positionW:TEXCOORD0, float4 positionL:TEXCOORD1):COLOR0
{   			
	clip(dot(float4(positionW, 1), clipPlane));
	
	gPositionW = positionW;
	gPositionL  = positionL;
	
	ComputeShadowFactor();	
	
	return float4(gShadowFactor,gShadowFactor,gShadowFactor,1);
}

VertexShader skinnedVSArray[4] = {  compile vs_1_1 VS(1), 
									compile vs_1_1 VS(2),
									compile vs_1_1 VS(3),
									compile vs_1_1 VS(4)
								 };

								 
int vsIndex;

technique T0
{
    pass Pass1
    {
        // TODO: set renderstates here.					
        VertexShader = skinnedVSArray[vsIndex];
        PixelShader = compile ps_2_0 PS();
    }
}