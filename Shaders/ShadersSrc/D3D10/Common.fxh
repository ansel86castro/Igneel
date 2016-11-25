#define COMMON

struct SimpleVSInput
{
    float3 Position : POSITION;
	float3 Normal	: NORMAL;
	float2 TexCoord : TEXCOORD0;
	float Occ		: TEXCOORD1;	
};

struct SimpleVSOutput
{
    float4 PositionH : POSITION;
	float3 PositionW : TEXCOORD0; 
	float3 NormalW	 : TEXCOORD1;
	float2 TexCoord	 : TEXCOORD2;
	float Occ	  	 : TEXCOORD3;
	float4 ScreenPosition: TEXCOORD4; 
};

struct BumpVSInput 
{
	float3 Position : POSITION;
	float3 Tangent  : TANGENT;
	float3 Normal   : NORMAL;
	float2 TexCoord : TEXCOORD0;
	float Occ		: TEXCOORD1;
};

struct BumpVSOutput
{
	float4 PositionH : POSITION0;
	float3 PositionW : TEXCOORD0; 
	float3 NormalW	 : TEXCOORD1;
	float3 TangentW	 : TEXCOORD2;
	float3 BinormalW : TEXCOORD3;
	float2 TexCoord	 : TEXCOORD4;
	float Occ	 	 : TEXCOORD5;
	float4 ScreenPosition: TEXCOORD6; 
};

struct SMVSOutput
{
	float4 PositionH : POSITION;
	float3 PositionW : TEXCOORD0; 
	float3 NormalW	 : TEXCOORD1;
	float2 TexCoord	 : TEXCOORD2;
	float4 PositionL : TEXCOORD3;
	float Occ	 	 : TEXCOORD4;
	float4 ScreenCoord : TEXCOORD5;
};

struct SMBumpVSOutput
{
	float4 PositionH  : POSITION;
	float3 PositionW  : TEXCOORD0; 
	float3 NormalW	  : TEXCOORD1;
	float3 TangentW	  : TEXCOORD2;
	float3 BinormalW  : TEXCOORD3;
	float2 TexCoord	  : TEXCOORD4;
	float4 PositionL  : TEXCOORD5;
	float Occ	 	  : TEXCOORD6;
	float4 ScreenCoord : TEXCOORD7;
};

struct SMVSOutput_2L
{
	float4 PositionH  : POSITION;
	float3 PositionW  : TEXCOORD0; 
	float3 NormalW	  : TEXCOORD1;
	float2 TexCoord	  : TEXCOORD2;
	float4 PositionL0 : TEXCOORD3;
	float4 PositionL1 : TEXCOORD4;
	float Occ	 	  : TEXCOORD5;
	float4 ReflSamplingPos : TEXCOORD6;
};
struct SMVSOutput_3L
{
	float4 PositionH  : POSITION;
	float3 PositionW  : TEXCOORD0; 
	float3 NormalW	  : TEXCOORD1;
	float2 TexCoord	  : TEXCOORD2;
	float4 PositionL0 : TEXCOORD3;
	float4 PositionL1 : TEXCOORD4;
	float4 PositionL2 : TEXCOORD5;
	float Occ	 	  : TEXCOORD6;
};

struct SMVSOutput_4L
{
	float4 PositionH  : POSITION;
	float3 PositionW  : TEXCOORD0; 
	float3 NormalW	  : TEXCOORD1;
	float2 TexCoord	  : TEXCOORD2;
	float4 PositionL0 : TEXCOORD3;
	float4 PositionL1 : TEXCOORD4;
	float4 PositionL2 : TEXCOORD5;
	float4 PositionL3 : TEXCOORD6;
	float Occ	 	  : TEXCOORD7;
};


struct SMBumpVSOutput_2L
{
	float4 PositionH  : POSITION;
	float3 PositionW  : TEXCOORD0; 
	float3 NormalW	  : TEXCOORD1;
	float3 TangentW	  : TEXCOORD2;
	float3 BinormalW  : TEXCOORD3;
	float2 TexCoord	  : TEXCOORD4;
	float4 PositionL0 : TEXCOORD5;
	float4 PositionL1 : TEXCOORD6;
	float Occ		  : TEXCOORD7;
	float4 ReflSamplingPos : COLOR0;
};

struct SMVSOutputSoft
{
	float4 PositionH	: POSITION;
	float3 PositionW	: TEXCOORD0; 
	float3 NormalW		: TEXCOORD1;
	float2 TexCoord		: TEXCOORD2;	
	float4 ScreenCoord	: TEXCOORD3;
	float Occ			: TEXCOORD4;
	float4 ReflSamplingPos : TEXCOORD5;
};

struct SMBumpVSOutputSoft
{
	float4 PositionH 	: POSITION;
	float3 PositionW 	: TEXCOORD0; 
	float3 NormalW	 	: TEXCOORD1;
	float3 TangentW	 	: TEXCOORD2;
	float3 BinormalW 	: TEXCOORD3;
	float2 TexCoord	 	: TEXCOORD4;	
	float4 ScreenCoord 	: TEXCOORD5;
	float Occ		   	: TEXCOORD6;
	float4 ReflSamplingPos : TEXCOORD7;
};

struct CubeSMVSOutput
{
	float4 PositionH : POSITION;
	float3 PositionW : TEXCOORD0; 
	float3 NormalW	 : TEXCOORD1;
	float2 TexCoord	 : TEXCOORD2;
	float3 LightDir	 : TEXCOORD3;
	float  Distance	 : TEXCOORD4;
	float Occ		 : TEXCOORD5;
};

struct CubeBumpVSOutput
{
	float4 PositionH : POSITION;
	float3 PositionW : TEXCOORD0; 
	float3 NormalW	 : TEXCOORD1;
	float3 TangentW	 : TEXCOORD2;
	float3 BinormalW : TEXCOORD3;
	float2 TexCoord	 : TEXCOORD4;
	float3 LightDir	 : TEXCOORD5;
	float  Distance	 : TEXCOORD6;
	float Occ		 : TEXCOORD7;
};
