
#define LIGTHING_SHADOWED

#ifndef STD_LIGHTING
	#include "StdLigthing.fxh"
#endif

#ifndef FILTERING
	#include "Filtering.fxh"
#endif


#define KERNEL_SIZE 3
#define ComputeShadowFactor FilteredPCF

bool USE_STATIC_SM		: STATIC_SM_FLAG = false;

//shadow map paremeters
//shadow sampler in register s7
float SHADOW_EPSILON	: EPSILON;
float4x4 LightVP		: SM_VIEWPROJ;

float2 gShadowTexOffsets[KERNEL_SIZE * KERNEL_SIZE];
//float gShadowTexWeights[KERNEL_SIZE * KERNEL_SIZE];

float SM_SIZE;
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

//--------------------------------------------------------------------------------------
// Use PCF to sample the depth map and return a percent lit value.
//--------------------------------------------------------------------------------------
// void ComputeShadowFactor()
// {
	// //gShadowFactor = pcf16(gPositionL ,s7 ,SHADOW_EPSILON, 1.0f/1024.0f);	
	// // if(USE_STATIC_SM)
	// // {		
		// // float smShadowFactor = pcf16(mul(float4(gPositionW,1) , STSM_LightVP) ,s6 ,STSM_SHADOW_EPSILON, STSM_texSize);
		// // gShadowFactor = min(gShadowFactor, smShadowFactor);	
	// // }
	// // gShadowFactor = 0;	
	// // for(int i = 0; i < gNumSamples; ++i)
	// // {
		// // gShadowFactor += (float)(tex2D(s7, gShadowTexCoord.xy + gShadowTexOffsets[i]).r > gShadowTexCoord.z);
	// // }	
	
	// // gShadowFactor /= gNumSamples;
		
// }

void FilteredPCF()
{	
	gShadowFactor = 0;
	const int NUM_SAMPLES = KERNEL_SIZE * KERNEL_SIZE;
	const int loopStart = -KERNEL_SIZE / 2;
	const int loopEnd = KERNEL_SIZE / 2 + 1;
	const int CONT_KERNEL_SIZE = KERNEL_SIZE - 1;
	
	int x;
	int y;
	
	float fTexelSize = 1.0f / SM_SIZE;
	
	// Transform to texel space.
	float2 texelCoord = SM_SIZE * gShadowTexCoord.xy;
		
	// Determine the interpolation amounts.
	float2 t = frac( texelCoord );

	float samples[NUM_SAMPLES];
	float row[KERNEL_SIZE];
	float column[KERNEL_SIZE];
	float4 tc;
	float4 result;
	
	//presample pcf values of kernel
	for(int i = 0; i < NUM_SAMPLES; ++i)		
	{
		tc = float4(gShadowTexCoord.xy + gShadowTexOffsets[i],0 , 0);
		samples[i] = (float)(tex2Dlod(s7, tc).r > gShadowTexCoord.z);
	}
	
	//presample pcf values of spanded row and  expanded column
	for(int i =  loopStart; i < loopEnd; ++i)	
	{
		int index = i - loopStart;
		tc = float4(gShadowTexCoord.xy + float2(  2.0f * fTexelSize, ((float)i) * fTexelSize), 0, 0 );
		column[index] = (float)(tex2Dlod(s7, tc ).r > gShadowTexCoord.z);			
		
		tc = float4(gShadowTexCoord.xy + float2( ((float)i) * fTexelSize,  2.0f * fTexelSize), 0,0);
		row[index] 	  = (float)(tex2Dlod(s7, tc ).r > gShadowTexCoord.z);				
	}	
	
	//filter subkernel
	for(y = 0; y < CONT_KERNEL_SIZE; ++y)
	{
		for(x = 0; x < CONT_KERNEL_SIZE; ++x)
		{
			result.x  = samples[y * KERNEL_SIZE + x];
			result.y  = samples[y * KERNEL_SIZE + x + 1];
			result.z  = samples[(y + 1) * KERNEL_SIZE + x];
			result.w  = samples[(y + 1) * KERNEL_SIZE + x + 1];
			
			//perform bilinear filter on subkernel of size (CONT_KERNEL_SIZE x CONT_KERNEL_SIZE)
			gShadowFactor += lerp( lerp(result.x, result.y, t.x), lerp(result.z, result.w, t.x), t.y);	
		}
	}
	
	//filter last kernel row
	for(x = 0; x < CONT_KERNEL_SIZE ; ++x)
	{
		result.x  = samples[CONT_KERNEL_SIZE * KERNEL_SIZE + x];
		result.y  = samples[CONT_KERNEL_SIZE * KERNEL_SIZE + x + 1];
		result.z  = row[x];
		result.w  = row[x + 1];
			
		//perform bilinear filter on last kernel row
		gShadowFactor += lerp( lerp(result.x, result.y, t.x), lerp(result.z, result.w, t.x), t.y);						
	}
	
	//filter last kernel column
	for(y = 0; y < CONT_KERNEL_SIZE ; ++y)
	{
		result.x  = samples[y * KERNEL_SIZE + CONT_KERNEL_SIZE];
		result.y  = column[y];
		result.z  = samples[(y + 1) * KERNEL_SIZE + CONT_KERNEL_SIZE];
		result.w  = column[y + 1];
			
		//perform bilinear filter on last kernel column
		gShadowFactor += lerp( lerp(result.x, result.y, t.x), lerp(result.z, result.w, t.x), t.y);						
	}
	
	//perform bilinear filter on bottom right kernel element
	result.x  = samples[CONT_KERNEL_SIZE * KERNEL_SIZE + CONT_KERNEL_SIZE];
	result.y  = column[CONT_KERNEL_SIZE];
	result.z  = row[CONT_KERNEL_SIZE];
	result.w  = (float)(tex2Dlod(s7,float4( gShadowTexCoord.xy + float2( 2.0f * fTexelSize, 2.0f * fTexelSize), 0, 0) ).r > gShadowTexCoord.z);
		
	//perform bilinear filter on last kernel column
	gShadowFactor += lerp( lerp(result.x, result.y, t.x), lerp(result.z, result.w, t.x), t.y);	
	
	gShadowFactor /= (float)NUM_SAMPLES;
}