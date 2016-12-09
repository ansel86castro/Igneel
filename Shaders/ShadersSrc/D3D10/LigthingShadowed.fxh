
#define LIGTHING_SHADOWED

#ifndef STD_LIGHTING
	#include "StdLigthing.fxh"
#endif


#define KERNEL_SIZE 3
#define ComputeShadowFactor FilteredPCF

bool USE_STATIC_SM		: STATIC_SM_FLAG = false;

cbuffer shadowMap
{
	//shadow map paremeters
	//shadow sampler in register s7
	float SHADOW_EPSILON	: EPSILON;
	float4x4 LightVP		: SM_VIEWPROJ;		
	float2 gShadowTexOffsets[KERNEL_SIZE * KERNEL_SIZE];
	//float gShadowTexWeights[KERNEL_SIZE * KERNEL_SIZE];

	float SM_SIZE;
};

Texture2D t7  :register(t7);
SamplerComparisonState s7 : register(s7);

//s6 static Shadow MAp
//s7 light Shadow Map

//GLOBALS
static float4 gPositionL;
static float4 gShadowTexCoord;


void TransformPNL()
{  
	TransformPN();	
	gPositionL = mul(float4(gPositionW,1) , LightVP);
	
}

void TransformPNTL()
{
	TransformPNT();	
	gPositionL = mul(float4(gPositionW,1) , LightVP);
}

void ComputeShadowTexCoord()
{
	float3 posL = gPositionL.xyz / gPositionL.w;
	gShadowTexCoord = float4(posL.x * 0.5f + 0.5f, -posL.y * 0.5f + 0.5f, posL.z , 1);
	gShadowTexCoord.z -= SHADOW_EPSILON;
}



void FilteredPCF()
{	
	gShadowFactor = 0;
	const int NUM_SAMPLES = KERNEL_SIZE * KERNEL_SIZE;	
	//presample pcf values of kernel
	for(int i = 0; i < NUM_SAMPLES; ++i)		
	{		
		gShadowFactor += t7.SampleCmpLevelZero(s7, gShadowTexCoord.xy + gShadowTexOffsets[i] , gShadowTexCoord.z);
	}
			
	
	gShadowFactor /= (float)NUM_SAMPLES;
}