#include "../Include/LigthingShadowed.fxh"

int DESCRIPTION
<
	string ShaderModel = "3.0";
	string Version = "1.0.0";
	int NumberOfLights = 1;		
	bool SupportAmbient = true;	
	bool SupportHemispherical = true;
	bool SupportSurfaceInfo = true;			
>;

float3 camUp;

float4x4 GetBillboardMatrix(float3 position)
{
   float3 front = normalize(eyePos - position);            
   float3 right = normalize(cross(camUp, front));
   float3 up = normalize(cross(front, right));
   
   return float4x4(float4(right, 0), 
				   float4(up, 0), 
				   float4(front,0),  
				   float4(position,1));
}

void VS(float3 posIn		:POSITION, 
		float2 texCoorIn	: TEXCOORD0,
		float3 posParticle  : TEXCOORD1,
		float alphaIn		: TEXCOORD2,
		float sizeIn		: TEXCOORD3,
		float4 colorIn		: COLOR,
		
		out float4 posH     : POSITION,
		out float3 posW     : TEXCOORD0,
		out float3 normalW	: TEXCOORD1,
		out float2 texCoord	: TEXCOORD2,
		out float alpha 	: TEXCOORD3,		
		out float4 color	: COLOR)
{
	float4x4 world = GetBillboardMatrix(posParticle);
	
	posW = mul(float4(posIn * sizeIn, 1), world);	//posIn * sizeIn;
	posH = mul(float4(posW, 1), ViewProj);
	normalW = float3(world[2].xyz);
	texCoord = texCoorIn;
	alpha = alphaIn;
	color = colorIn;
}

void VSNMap(float3 posIn		:POSITION, 
		float2 texCoorIn	: TEXCOORD0,
		float3 posParticle  : TEXCOORD1,
		float alphaIn		: TEXCOORD2,
		float sizeIn		: TEXCOORD3,
		float4 colorIn		: COLOR,
		
		out float4 posH     : POSITION,
		out float3 posW     : TEXCOORD0,
		out float3 normalW	: TEXCOORD1,
		out float3 tangentW	: TEXCOORD2,
		out float3 binormalW: TEXCOORD3,
		out float2 texCoord	: TEXCOORD4,
		out float alpha 	: TEXCOORD5,		
		out float4 color	: COLOR)
{
	float4x4 world = GetBillboardMatrix(posParticle);
	
	posW = mul(float4(posIn * sizeIn, 1), world);
	posH = mul(float4(posW, 1), ViewProj);
	normalW = float3(world[2].xyz);
	texCoord = texCoorIn;
	alpha = alphaIn;
	color = colorIn;
	
	tangentW = world[0].xyz;
	binormalW = world[1].xyz;
}

float4 PS(float3 posW    : TEXCOORD0,
		 float3 normalW	 : TEXCOORD1,
		 float2 texCoord : TEXCOORD2,
		 float alpha 	 : TEXCOORD3,		
		 float4 color	 : COLOR,
		 uniform bool hemisphere, uniform int activeLights) :COLOR0
 {
	clip(dot(float4(posW, 1), clipPlane));
	
	gPositionW = posW;
	gNormalW   = normalW;
	gTexCoord  = texCoord;
	gOcc     	=  1;
	gShadowFactor = 1;	
	
	ComputeLighting(hemisphere, activeLights);
	
	gColor *= color;
	gColor.a *= alpha;
	
	return  gColor;
	//return float4(1,1,1,1);
 }
 
 float4 PSNMap(float3 posW     : TEXCOORD0,
			float3 normalW	: TEXCOORD1,
			float3 tangentW	: TEXCOORD2,
			float3 binormalW: TEXCOORD3,
			float2 texCoord	: TEXCOORD4,
			float alpha 	: TEXCOORD5,		
			float4 color	: COLOR,
			uniform bool hemisphere, uniform int activeLights) :COLOR0
 {
	clip(dot(float4(posW, 1), clipPlane));
	
	gPositionW = posW;
	gNormalW   = normalW;
	gTangentW = tangentW;
	gBinormalW = binormalW;
	gTexCoord  = texCoord;
	gOcc     	=  1;
	gShadowFactor = 1;	
	
	ComputeNormal();	
	ComputeLighting(hemisphere, activeLights);
	
	gColor *= color;
	gColor.a *= alpha;
	return  gColor;
 
 }
 
 PixelShader psShaders[8] = {           compile ps_3_0 PS(false, 0),
										compile ps_3_0 PS(false, 1),																				
										compile ps_3_0 PS(true, 0),
										compile ps_3_0 PS(true, 1),										
										
										compile ps_3_0 PSNMap(false, 0),
										compile ps_3_0 PSNMap(false, 1),																				
										compile ps_3_0 PSNMap(true, 0),
										compile ps_3_0 PSNMap(true, 1)										
							  };
							  
VertexShader vertexShaders[2] = {
									compile vs_1_1 VS(),
									compile vs_1_1 VSNMap()
								};								

int activeLights: ACTIVE_LIGHTS = 0;
int vsIndex;
int psIndex;

technique T0
{
	pass Default
	{			
		 //Direct color rendering
		 VertexShader = vertexShaders[vsIndex];
		 PixelShader  =	psShaders[activeLights + psIndex];
	}
}