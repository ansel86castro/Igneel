#define SKINNED
#define MAX_PALLETE 32

#ifndef STD_LIGHTING
	#include "StdLigthing.fxh"
#endif

#define NumBones 4

cbuffer bones
{
	//uniform int NumBones;
	float4x4 WorldArray[MAX_PALLETE] : WORLDMATRIXARRAY;
};
//GLOBALS
static float gBlendWeightsArray[4];
static int   gIndexArray[4];

void InitSkinning(float4 BlendWeights , float4 BlendIndices)
{
	// Compensate for lack of UBYTE4 on Geforce3
    int4 IndexVector = D3DCOLORtoUBYTE4(BlendIndices);	
    // cast the vectors to arrays for use in the for loop below
    gBlendWeightsArray = (float[4])BlendWeights;
    gIndexArray        = (int[4])IndexVector;		
}

void TransformSkinnedPN()
{
	float3      pos = 0.0f;
    float3      normal = 0.0f;    
    float       lastWeight = 0.0f;
	
	// calculate the pos/normal using the "normal" weights 
    //        and accumulate the weights to calculate the last weight	
	[unroll]
    for (int iBone = 0; iBone < NumBones-1; iBone++)
    {
        lastWeight = lastWeight + gBlendWeightsArray[iBone];
        int index = gIndexArray[iBone];       
				         
		pos += mul(gPositionH, WorldArray[index]).xyz * gBlendWeightsArray[iBone];
		normal += mul(gNormalW, (float3x3)WorldArray[index]) * gBlendWeightsArray[iBone];		
		        
    }
    lastWeight = 1.0f - lastWeight; 

    //Now that we have the calculated weight, add in the final influence
    pos += mul(gPositionH, WorldArray[gIndexArray[NumBones-1]]).xyz * lastWeight;
    normal += mul(gNormalW,(float3x3)WorldArray[gIndexArray[NumBones-1]]) * lastWeight;
	
	gPositionW =  mul(float4(pos,1 ), World).xyz;
	gNormalW   =  mul(normal, (float3x3)World);	
	// gPositionW =  pos; 
	// gNormalW   = normal;
	
	gPositionH = mul(float4(gPositionW,1), ViewProj);
}

void TransformSkinnedPNT()
{
	
	TransformSkinnedPN();
	
    float       lastWeight = 0.0f;
	float3		tangent = 0.0f;	
	// calculate the pos/normal using the "normal" weights 
    //        and accumulate the weights to calculate the last weight	
	[unroll]
    for (int iBone = 0; iBone < NumBones-1; iBone++)
    {
        lastWeight = lastWeight + gBlendWeightsArray[iBone];
        int index = gIndexArray[iBone];       				         		
		tangent += mul(gTangentW, (float3x3)WorldArray[index]) * gBlendWeightsArray[iBone];				        
    }
    lastWeight = 1.0f - lastWeight; 
    //Now that we have the calculated weight, add in the final influence
    tangent += mul(gTangentW, (float3x3)WorldArray[gIndexArray[NumBones-1]]).xyz * lastWeight;    
		
	gTangentW = mul(tangent, (float3x3)World);	
}

SimpleVSOutput SkinnedVertexShader(float4 position      : POSITION, 
								   float4  BlendWeights : BLENDWEIGHT,
								   float4  BlendIndices : BLENDINDICES,
								   float3 normal	    : NORMAL, 
								   float2 texCoord      : TEXCOORD0,
								   float occ 		    : TEXCOORD1)
{
	   	
	SimpleVSOutput output = (SimpleVSOutput)0;	
	
	gPositionH = position;    
	gNormalW = normal;
	
	InitSkinning(BlendWeights, BlendIndices);	
	TransformSkinnedPN();			
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TexCoord = texCoord;
	output.Occ = 1 - occ;
   
    return output;
}


BumpVSOutput SkinnedBumpVertexShader(float4 position : POSITION,
							  float4  BlendIndices : BLENDINDICES,
							  float4  BlendWeights : BLENDWEIGHT,
							  float3 tangent  : TANGENT,
							  float3 normal   : NORMAL,
							  float2 texCoord : TEXCOORD0,
							  float occ       :TEXCOORD1)
{
	BumpVSOutput output = (BumpVSOutput)0;

	gPositionH = position; 
	gNormalW = normal;
	gTangentW = tangent;
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedPNT();	
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TangentW = gTangentW;
	output.BinormalW = gBinormalW;	
	output.TexCoord = texCoord;
	output.Occ = 1 - occ;
	
    return output;
}
