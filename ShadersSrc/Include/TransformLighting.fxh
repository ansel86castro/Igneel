#define TRANF_LIGHTING

#ifndef LIGHTING
	#include "Lighting.fxh"
#endif
#ifndef COMMON	
	#include "Common.fxh"
#endif

//CONSTANTS
float4x4 World			: WORLD;
float4x4 ViewProj		: VIEWPROJ;
float3 ambient			: AMBIENT;
float3 sky 			 	: SKY_COLOR = { 0.5f, 0.5f, 0.5f };
float3 ground 			: GROUND_COLOR = { 0.0f, 0.0f, 0.0f};
float3 northPole 		: NORTH_POLE = { 0.0f, 1.0f ,0.0f };
Light light				: LIGHT;
SurfaceInfo surface		: SURFACE;
float3 eyePos			: EYEPOSITION;

//GLOBALS
//Transforms globals
static float4 gPositionH;
static float4 gScreenCoord = {0,0,0,0};
static float3 gPositionW;
static float3 gNormalW   =   {0,1,0};
static float3 gTangentW;
static float3 gBinormalW;
static float2 gTexCoord;

//Lighting globals
static float  gShadowFactor;
static float  gOcc;
static float3 gDiffuse;
static float  gSpecularPower;
static float3 gSpecular;
static float4 gColor;


void TransformPN()
{  
	gPositionW = mul(gPositionH, World).xyz;
	gPositionH = mul(float4(gPositionW,1), ViewProj);		
	gNormalW = normalize(mul(gNormalW, (float3x3)World));	
}

void TransformPNT()
{  
	gPositionW = mul(gPositionH, World).xyz;
	gPositionH = mul(float4(gPositionW,1), ViewProj);
	gNormalW = normalize(mul(gNormalW, (float3x3)World));
	gTangentW = normalize(mul(gTangentW, (float3x3)World));
	gBinormalW= normalize(cross(gNormalW, gTangentW));
}

void TransformScreeen()
{	
	gScreenCoord.x = (gPositionH.x + gPositionH.w) * 0.5f;
    gScreenCoord.y = (gPositionH.w - gPositionH.y) * 0.5f;
    gScreenCoord.zw = gPositionH.w;
}

void ComputeHemisphere()
{	
	float k = 0.5f + 0.5f * dot(gNormalW, northPole);
	gColor.rgb += gDiffuse * lerp(ground ,sky , k) * gOcc; //( 1 - occlutionFactor);	
}
void ComputeAmbient()
{
	gColor.rgb += gDiffuse.rgb * ambient * gOcc; //(1 - gOcc)
}

void ComputeHemisphere(out float3 color)
{	
	float k = 0.5f + 0.5f * dot(gNormalW, northPole);
	color = gDiffuse * lerp(ground ,sky , k) * gOcc; //( 1 - occlutionFactor);	
}
void ComputeAmbient(out float3 color)
{
	color = gDiffuse.rgb * ambient * gOcc; //(1 - gOcc)
}

void ComputeDirectLighting()
{
	// LightColor = (l.Ambient * s.Ambient + kd * s.Diffuse * l.Diffuse + ks * l.Specular * s.Specular) / (l.Att.x  + l.Att.y * d + a.Att.z * d^2)	
	
	float3 toEye = normalize(eyePos - gPositionW);	
	float att = 1;
	float kSpot = 1;
	float3 lightDir = light.Dir;
	
	[branch]	
	if(light.Type != DIRECTIONAL)
	{
		// Point or Spot
		lightDir = gPositionW - light.Pos;
		float d = length(lightDir);
		lightDir /= d;
		
		[branch]
		if(light.Type == SPOT)
			kSpot = pow(saturate(dot(lightDir , light.Dir)) , light.SpotPower);
			
		att = dot(light.Att, float3(1.0f, d, d*d));
		
		if(d > light.Range)
			kSpot = 0.0f;
	}
	
	lightDir = -lightDir;
	//float3 reflecVec = reflect(lightDir, normalW);
	float3 h = normalize(lightDir + toEye);
	float4 l = lit(dot(gNormalW, lightDir), dot(gNormalW, h), 1);
	l.z = pow(l.z, gSpecularPower);	
	
	//add diffuse contribution
	float3 color = (gDiffuse * light.Diffuse) * l.y;		
	
	//add specular contribution
	color += (gSpecular * light.Specular) * l.z;	
	
	color *= light.Intensity;	
	color *= kSpot;
	color *= gShadowFactor;
	color /= att;
	
	gColor.rgb += color;
}
