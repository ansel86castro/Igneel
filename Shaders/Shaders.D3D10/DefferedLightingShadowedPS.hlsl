#define KERNEL_SIZE 3

#include "LightingBase.hlsli"
#include "Deffered.hlsli"
#include "LigthingShadowed.hlsli"

Texture2D    t0                    : register( t0 ); // diffuse map  [a = opacity]
Texture2D    t1                    : register( t1 ); // specular map [a = specularPower]
Texture2D    t2                    : register( t2 ); // emissive map [a = glossFactor]
Texture2D    t3                    : register( t3 ); // normal Map   [a = Occlution Factor]

SamplerState s0 : register(s0);

struct PSIn
{
    float4 position : SV_POSITION; 
    float2 texCoord : TEXCOORD0; 
};
struct PSOut
{
	float4 Color : SV_TARGET;	
};

cbuffer shadowMapCamera
{		
	float4x4 LightVP;			
};

PSOut main(PSIn input)
{	
	float4 positionRhz = t0.SampleLevel(s0,  input.texCoord , 0);
	float4 normalW =     t1.SampleLevel(s0,  input.texCoord , 0);

	[branch]
	if(!any(positionRhz))
	{			
		discard;
	}

	PSOut o = (PSOut)0;	

	float4 Diffuse =  t2.SampleLevel(s0,  input.texCoord , 0);
	float4 Specular = t3.SampleLevel(s0,  input.texCoord , 0);	
		
	gPositionW = UnPackPositionFromFatFramebuffer(positionRhz.xyz).xyz;
	//gPositionW = positionRhz.xyz;

	float4 unpackedNormal = UnPackNormalFromFatFramebuffer(normalW);
	gNormalW = unpackedNormal.xyz;
	gOcc = unpackedNormal.w;

	gShadowFactor = 1;
	gDiffuse = Diffuse.rgb;
	gSpecular = Diffuse.rgb * Specular.r;
	gSpecularPower = Specular.b;
	gAlpha = 1;
	gColor = float4(Diffuse.rgb * Diffuse.a, 1);
	gGlossFactor = Specular.g;

	gPositionL = mul(float4(gPositionW,1) , LightVP);

	ComputeShadowTexCoord(-Light.Dir);
	ComputeShadowFactor();	

	[branch]
	if(HemisphericalLighting)
		ComputeHemisphere();
	else
		ComputeAmbient();
	
	  [branch]
	 if(Light.Type == DIRECTIONAL)
		DirectionalLight();	 					
	 else [branch] if(Light.Type == POINT)
		PointLight();	 
	 else [branch] if(Light.Type == SPOT)
		SpotLight();
	
	 o.Color = gColor;
	 return o;
}