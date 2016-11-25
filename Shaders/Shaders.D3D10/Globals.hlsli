#define GLOBALS

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
static float gAlpha;
static float gGlossFactor;
//static float2 gOccTexCoord;