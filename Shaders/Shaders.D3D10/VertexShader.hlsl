
struct SMVSOutput
{	float4 PositionH :SV_POSITION;
	float3 PositionW :TEXCOORD0;
	float3 NormalW :TEXCOORD1;
	float2 TexCoord :TEXCOORD2;
	float4 PositionL :TEXCOORD3;
	float Occ :TEXCOORD4;
	float4 ScreenCoord :TEXCOORD5;
};
static float4 gPositionH;
static float4 gScreenCoord = {0, 0, 0, 0} ;
static float3 gPositionW;
static float3 gNormalW = {0, 1, 0} ;
cbuffer camera
{
	float4x4 ViewProj:VIEWPROJ;
};
cbuffer perObject
{
	float4x4 World:WORLD;
};
void TransformScreeen()
{
	gScreenCoord.x = (gPositionH.x + gPositionH.w) * 0.5;
	gScreenCoord.y = (gPositionH.w - gPositionH.y) * 0.5;
	gScreenCoord.zw = gPositionH.w;
}
static float4 gPositionL;
cbuffer cbShadowMapCamera
{
	float4x4 LightVP;
};
cbuffer cbBonesTransforms
{
	float4x4 WorldArray[32];
};
static float gBlendWeightsArray[4];
static int gIndexArray[4];
void InitSkinning(float4 BlendWeights, float4 BlendIndices)
{
	int4 IndexVector = (int4)BlendIndices;
	gBlendWeightsArray = (float[4])BlendWeights;
	gIndexArray = (int[4])IndexVector;
}
void TransformSkinnedPN()
{
	float3 pos = 0;
	float3 normal = 0;
	float lastWeight = 0;
	[unroll]
	for(int iBone = 0; iBone < 4 - 1; iBone++)
	{
		lastWeight = lastWeight + gBlendWeightsArray[iBone];
		int index = gIndexArray[iBone];
		pos += mul(gPositionH, WorldArray[index]).xyz * gBlendWeightsArray[iBone];
		normal += mul(gNormalW, (float3x3)WorldArray[index]) * gBlendWeightsArray[iBone];
	}
	lastWeight = 1 - lastWeight;
	pos += mul(gPositionH, WorldArray[gIndexArray[4 - 1]]).xyz * lastWeight;
	normal += mul(gNormalW, (float3x3)WorldArray[gIndexArray[4 - 1]]) * lastWeight;
	gPositionW = mul(float4(pos, 1), World).xyz;
	gNormalW = mul(normal, (float3x3)World);
	gPositionH = mul(float4(gPositionW, 1), ViewProj);
}
void TransformSkinnedPNL()
{
	TransformSkinnedPN();
	gPositionL = mul(float4(gPositionW, 1), LightVP);
}
SMVSOutput main(float4 position:POSITION, float3 normal:NORMAL, float2 texCoord:TEXCOORD0, float occ:TEXCOORD1, float4 BlendIndices:BLENDINDICES, float4 BlendWeights:BLENDWEIGHT)
{
	SMVSOutput output = (SMVSOutput)0;
	gPositionH = position;
	gNormalW = normal;
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedPNL();
	TransformScreeen();
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TexCoord = texCoord;
	output.PositionL = gPositionL;
	output.Occ = 1 - occ;
	output.ScreenCoord = gScreenCoord;
	return	output;
}
