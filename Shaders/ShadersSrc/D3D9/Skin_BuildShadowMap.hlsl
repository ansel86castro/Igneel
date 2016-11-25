#include "SkinnedShadowed.hlsl"

// int DESCRIPTION
// <
	// string ShaderModel = "2.0";
	// string Version = "1.0.0";	
	// bool SupportSurfaceInfo = true;	
	// int MaxPalleteMetrices = MAX_PALLETE;	
// >;


//***************BuildShadowMap**********************************

void VS(in  float4 position  : POSITION,
	  in  float2 texCoord  : TEXCOORD0,
	  in float4  BlendWeights : BLENDWEIGHT,
	  in float4  BlendIndices : BLENDINDICES,
	  out float4 positionH : POSITION,
	  out float2 depth	   : TEXCOORD0,
	  out float2 outTexCoord : TEXCOORD1 )
{	
	
	gPositionH = position;  
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedPN();		
	
	positionH = gPositionH;
	depth = float2(positionH.z , positionH.w);
	outTexCoord = texCoord;
}

float4 PS(float2 depth:TEXCOORD0, float2 texCoord:TEXCOORD1) : COLOR0
{
	float alpha = USE_DIFFUSE_MAP ? tex2D(s0, texCoord).a * surface.Alpha : surface.Alpha;	
	if(alpha == 0)
		discard;	
		
	//float z = 1 - depth.x / depth.y;
	float z = depth.x / depth.y;
	return float4(z ,z ,z ,1);		
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
        // VertexShader = compile vs_2_0 VS(4); //skinnedVSArray[3];
        // PixelShader = compile ps_2_0 BuildShadowMapPS();
    // }
// }