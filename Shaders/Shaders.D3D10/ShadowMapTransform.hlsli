
#ifndef TRANSFORM
	#include "Transform.hlsli"
#endif

//GLOBALS
static float4 gPositionL;

cbuffer cbShadowMapCamera
{		
	float4x4 LightVP;			
};

void TransformPL()
{  
	TransformP();	
	gPositionL = mul(float4(gPositionW,1) , LightVP);
	
}

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