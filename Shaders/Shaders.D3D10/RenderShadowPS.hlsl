#include "Shadowing.hlsli"

struct VSOut
{
	float4 PositionH :SV_POSITION;
	float4 PositionL :TEXCOORD0;
};

cbuffer cLigth {
	float3 lightDir;
};

Texture2D ShadowMap;
SamplerState sShadowMap;

float4 main(VSOut input) :SV_TARGET
{
	//clip(dot(float4(input.PositionW, 1), ClipPlane));	
	gPositionL	 = input.PositionL;			
	ComputeShadowTexCoord(-lightDir);
	
	float inShadow = (float)(ShadowMap.SampleLevel(sShadowMap, gShadowTexCoord.xy, 0). r > gShadowTexCoord.z);	
	
	return float4(inShadow,inShadow,inShadow,1);	
}

//float4 main(VSOut input) :SV_TARGET
//{
//	//clip(dot(float4(input.PositionW, 1), ClipPlane));	
//	gPositionL	 = input.PositionL;		
//	float2 gradient;
//	ComputeShadowTexCoord();
//	float width, height;
//	t7.GetDimensions(width, height);
//	float tx = 1.0 / width;
//	float ty = 1.0 / height;
//
//	float inShadow1 = (float)(t7.SampleLevel(s7, gShadowTexCoord.xy, 0). r < gShadowTexCoord.z);	
//	float inShadow2 = (float)(t7.SampleLevel(s7, gShadowTexCoord.xy + float2(tx , 0), 0). r < gShadowTexCoord.z);	
//	float inShadow3 = (float)(t7.SampleLevel(s7, gShadowTexCoord.xy + float2(tx, ty), 0). r < gShadowTexCoord.z);	
//	float inShadow4 = (float)(t7.SampleLevel(s7, gShadowTexCoord.xy+  float2(0 , ty), 0). r < gShadowTexCoord.z);	
//
//	gradient = float2(inShadow2 - inShadow1 , inShadow4 - inShadow4);	
//	
//	if(gradient.x==0 && gradient.y == 0)
//	{
//		//no shadow edge gradient = (0,0)
//		//return inShadow ? float4(0,0,0,1) : float4(1,1,1,1);
//		return float4(0,0,0,1);
//	}
//	else
//	{
//		//edge shadow
//		return float4(1,1,1,1);
//	}
//	
//}