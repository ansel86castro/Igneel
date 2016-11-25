#define VERTEX_SHADER

#ifndef LIGTHING_SHADOWED
	#include "LigthingShadowed.fxh"
#endif

SimpleVSOutput SimpleVertexShader(float4 position : POSITION, 
								  float3 normal	 : NORMAL, 
								  float2 texCoord : TEXCOORD0,
								  float occ :TEXCOORD1)
{
    SimpleVSOutput output = (SimpleVSOutput)0;	
	
	gPositionH = position;    
	gNormalW = normal;
	
	TransformPN();
	TransformScreeen();
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TexCoord = texCoord;
	output.Occ = 1 - occ;
	output.ScreenPosition = gScreenCoord;
    return output;
}

BumpVSOutput BumpVertexShader(float4 position : POSITION,
							  float3 tangent  : TANGENT,
							  float3 normal   : NORMAL,
							  float2 texCoord : TEXCOORD0,
							  float occ :TEXCOORD1)
{
	BumpVSOutput output = (BumpVSOutput)0;

	gPositionH = position; 
	gNormalW = normal;
	gTangentW = tangent;
	
	TransformPNT();	
	TransformScreeen();
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TangentW = gTangentW;
	output.BinormalW = gBinormalW;	
	output.TexCoord = texCoord;
	output.Occ = 1 - occ;
	output.ScreenPosition = gScreenCoord;
	
    return output;
	
}

void SampleEnvironmentMapVS(in float4 position:POSITION ,
							in float3 normal:NORMAL,
							in float2 inTexCoord:TEXCOORD0,
							out float4 positionH:POSITION,
							out float3 positionW:TEXCOORD0 ,
							out float3 normalW:TEXCOORD1,
							out float2 texCoord:TEXCOORD2)
{
	gPositionH = position;    
	gNormalW = normal;
	
	TransformPN();	
	
	positionH = gPositionH;
	positionW = gPositionW;
	normalW = gNormalW;
	texCoord = inTexCoord;
}

void SampleReflectionMapVS(in float4 position:POSITION ,
						   in float2 inTexCoord:TEXCOORD0,
					       out float4 positionH:POSITION,
						   out float4 screenCoord:TEXCOORD0,
						   out float2 texCoord:TEXCOORD1)
{
	gPositionH = position;    
	gNormalW = float3(1,1,1);
	
	TransformPN();	
	TransformScreeen();
	
	positionH = gPositionH;
	screenCoord = gScreenCoord;
	texCoord = inTexCoord;
}

void DeferredVS(float4 position : POSITION, 
			 float3 normal	 : NORMAL, 
			 float2 texCoord : TEXCOORD0,
			 float occ :TEXCOORD1,
			 
			 out float4 oPositionH: POSITION,
			 out float3 oPositionW: TEXCOORD0,
			 out float3 oNormalW  : TEXCOORD1,
			 out float2 oTexCoord : TEXCOORD2,
			 out float oOcc	  : TEXCOORD3,
			 out float2 oDepth: TEXCOORD4)
{
    	
	gPositionH = position;    
	gNormalW = normal;
	
	TransformPN();	
	
	oPositionH = gPositionH;
	oPositionW = gPositionW;
	oNormalW = gNormalW;
	oTexCoord = texCoord;
	oOcc = 1 - occ;
    oDepth = float2(gPositionH.z , gPositionH.w);
}

void BumpDeferredVS(float4 position : POSITION,
				  float3 tangent  : TANGENT,
				  float3 normal   : NORMAL,
				  float2 texCoord : TEXCOORD0,
				  float occ :TEXCOORD1,
				  
				  out float4 oPositionH: POSITION,
				  out float3 oPositionW: TEXCOORD0,
				  out float3 oNormalW  : TEXCOORD1,
				  out float2 oTexCoord : TEXCOORD2,
				  out float oOcc	   : TEXCOORD3,
				  out float2 oDepth    : TEXCOORD4,
				  out float3 oTangentW  : TEXCOORD5,
				  out float3 oBinormalW : TEXCOORD6)
{	
	gPositionH = position; 
	gNormalW = normal;
	gTangentW = tangent;
	
	TransformPNT();	
	
	oPositionH = gPositionH;
	oPositionW = gPositionW;
	oNormalW = gNormalW;
	oTexCoord = texCoord;
	oOcc = 1 - occ;
    oDepth = float2(gPositionH.z , gPositionH.w);
	oTangentW = gTangentW;
	oBinormalW = gBinormalW;
    
	
}

SMVSOutput SimpleVertexShaderSM(float4 position : POSITION, 
							  float3 normal	 : NORMAL, 
							  float2 texCoord : TEXCOORD0,
							  float occ:TEXCOORD1)
{
    SMVSOutput output = (SMVSOutput)0;

	gPositionH = position;    
	gNormalW = normal;
	
	TransformPNL();	
	TransformScreeen();
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.PositionL = gPositionL;
	output.TexCoord = texCoord;
	output.Occ = 1 - occ;
	output.ScreenCoord = gScreenCoord;
    return output;
}

SMBumpVSOutput BumpVertexShaderSM(float4 position : POSITION,
							  float3 tangent  : TANGENT,
							  float3 normal   : NORMAL,
							  float2 texCoord : TEXCOORD0,
							  float occ		  : TEXCOORD1)
{
	SMBumpVSOutput output = (SMBumpVSOutput)0;

	
	gPositionH = position; 
	gNormalW = normal;
	gTangentW = tangent;
	
	TransformPNTL();	
	TransformScreeen();
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TangentW = gTangentW;
	output.BinormalW = gBinormalW;	
	output.PositionL = gPositionL;
	output.TexCoord = texCoord;
	output.Occ = 1 - occ;
	output.ScreenCoord = gScreenCoord;
	return output;
}