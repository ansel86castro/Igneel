//********************************************************
//
//	Phong Shading for the GEngine Graphic Engine
//	developed by Ansel Castro Cabrera copyrights 2011
//
//********************************************************

#include "../Include/SkinnedShadowed.fxh"
#include "../Include/PixelShader.fxh"

int DESCRIPTION
<
	string ShaderModel = "3.0";
	string Version = "1.0.0";
	int NumberOfLights = 1;		
	bool SupportAmbient = true;	
	bool SupportHemispherical = true;
	bool SupportSurfaceInfo = true;			
	int MaxPalleteMetrices = MAX_PALLETE;
>;

SMVSOutput SimpleVertexShader(float4 position : POSITION, 
							  float3 normal	 : NORMAL, 
							  float2 texCoord : TEXCOORD0,
							  float occ:TEXCOORD1,
							  float4  BlendIndices : BLENDINDICES,
							  float4  BlendWeights : BLENDWEIGHT,
							  uniform int NumBones)
{
    SMVSOutput output = (SMVSOutput)0;

	gPositionH = position;    
	gNormalW = normal;
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedPNL(NumBones);	
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TexCoord = texCoord;
	output.PositionL = gPositionL;
	output.Occ = 1 - occ;
	
    return output;
}

SMBumpVSOutput BumpVertexShader(float4 position : POSITION,
								float4  BlendIndices : BLENDINDICES,
							  float4  BlendWeights : BLENDWEIGHT,
							  float3 tangent  : TANGENT,
							  float3 normal   : NORMAL,
							  float2 texCoord : TEXCOORD0,
							  float occ		  : TEXCOORD1,									  
							  uniform int NumBones)
{
	SMBumpVSOutput output = (SMBumpVSOutput)0;

	gPositionH = position; 
	gNormalW = normal;
	gTangentW = tangent;
	
	InitSkinning(BlendWeights, BlendIndices);
	TransformSkinnedPNTL(NumBones);	
	
	output.PositionH = gPositionH;
	output.PositionW = gPositionW;
	output.NormalW = gNormalW;
	output.TangentW = gTangentW;
	output.BinormalW = gBinormalW;	
	output.PositionL = gPositionL;
	output.TexCoord = texCoord;
	output.Occ = 1 - occ;
	
	return output;
}

PixelShader psShaders[8] = {            compile ps_3_0 PhongPixelShaderSM(false, 0),
										compile ps_3_0 PhongPixelShaderSM(false, 1),																				
										compile ps_3_0 PhongPixelShaderSM(true, 0),
										compile ps_3_0 PhongPixelShaderSM(true, 1),										
										
										compile ps_3_0 BumpPixelShaderSM(false, 0),
										compile ps_3_0 BumpPixelShaderSM(false, 1),																				
										compile ps_3_0 BumpPixelShaderSM(true, 0),
										compile ps_3_0 BumpPixelShaderSM(true, 1)										
							  };
  
							  
VertexShader skinnedVSArray[8] = {  compile vs_2_0 SimpleVertexShader(1), 
									compile vs_2_0 SimpleVertexShader(2),
									compile vs_2_0 SimpleVertexShader(3),
									compile vs_2_0 SimpleVertexShader(4),
									
								 	compile vs_2_0 BumpVertexShader(1), 
									compile vs_2_0 BumpVertexShader(2),
									compile vs_2_0 BumpVertexShader(3),
									compile vs_2_0 BumpVertexShader(4)
								 }; 
								

int activeLights: ACTIVE_LIGHTS = 0;
int vsIndex;
int psIndex;
							  

technique T0
{
	pass p0
	{			
		 
		 VertexShader = skinnedVSArray[vsIndex];
		 PixelShader  =	psShaders[activeLights + psIndex];
	}
}
  




//***************************  Planar  *****************************************************
//****************************		  ******************************************************


// SMVSOutput SimpleVertexShader_1Light_Planar(float3 position : POSITION, 
							  // float3 normal	 : NORMAL, 
							  // float2 texCoord : TEXCOORD0)
// {
    // SMVSOutput output = (SMVSOutput)0;

	// float4 posW = mul(float4(position ,1), World);	

	// output.PositionH = mul(posW, ViewProj);
	// output.PositionW = posW;		
	// output.NormalW	 = normalize(mul(normal, (float3x3)World));
	// output.TexCoord  = texCoord;
	// output.PositionL = mul(posW , LightVP[0]);
	// output.ReflSamplingPos = mul(posW, projMtx);	
    // return output;
// }

// float4 PhongPixelShader_1Light_Planar(float3 positionW : TEXCOORD0 ,
						// float3 normalW   : TEXCOORD1 , 
						// float2 texCoord  : TEXCOORD2 ,
						// float4 positionL : TEXCOORD3 ,
						// float Occ        : TEXCOORD4 ,
						// float4 reflSamplingPos : TEXCOORD5,
						// uniform bool hemisphere ) : COLOR0
// {   		
	// float3 diffuse = USE_DIFFUSE_MAP  ? tex2D(s2, texCoord) * surface.Diffuse : surface.Diffuse;
	// float3 specular = USE_SPECULAR_MAP ? tex2D(s3, texCoord) * surface.Specular : surface.Specular;
	
	// float4 color;
	// [branch]
	// if(hemisphere)
		// color = LightingHemisphericalShadowed(light[0], positionW, normalW, diffuse, specular, Occ ,positionL , s5);
	// else
		// color =	LightingAmbientShadowed(light[0], positionW, normalW, diffuse, specular, Occ ,positionL , s5);
    		
	// [branch]				
	// if(USE_REFLECTION_MAP)	
	// {		
		// float4 gloss = USE_GLOSS_MAP ? tex2D(s4, texCoord) : float4(1,1,1,1);
		// color += tex2Dproj(s1 , reflSamplingPos) * surface.Reflectivity * gloss;
	// }
	// [branch]
	// if(USE_REFRACTION_MAP)	
		// color += tex2Dproj(s7 , reflSamplingPos) * (1 - surface.Alpha);	
	// color.a = surface.Alpha;
	
    // return color;
// }

// SMBumpVSOutput BumpVertexShader_1Light_Planar(float3 position : POSITION,
									  // float3 tangent  : TANGENT,
									  // float3 normal   : NORMAL,
									  // float2 texCoord : TEXCOORD0)
// {
	// SMBumpVSOutput output = (SMBumpVSOutput)0;

	// float4 posW =mul(float4(position ,1),World);
	
	// output.PositionH = mul(posW, ViewProj);
	// output.PositionW = posW;
	// output.NormalW = normalize(mul(normal, (float3x3)World));
	// output.TangentW = normalize(mul(tangent, (float3x3)World));	
	// output.BinormalW = normalize(cross(output.NormalW ,output.TangentW));
	// output.TexCoord = texCoord;
	// output.PositionL = mul(posW , LightVP[0]);
	// output.ReflSamplingPos = mul(posW, projMtx);	
	// return output;
// }

// float4 BumpPixelShader_1Light_Planar(SMBumpVSOutput input , uniform bool hemisphere) : COLOR0
// {		
	// //pick and decode normal from the normal map 
	// float3 normal = tex2D(s0, input.TexCoord);
	// normal = (normal * 2) - 1;

	// // Move the normal from tangent space to world space
	// float3x3 tangentMatrix = {input.TangentW, input.BinormalW, input.NormalW};
	// normal = normalize(mul(normal, tangentMatrix));
		
	// return PhongPixelShader_1Light_Planar(input.PositionW, normal, input.TexCoord ,input.PositionL, input.Occ, input.ReflSamplingPos ,hemisphere);	
// }

// // ******************************** 2 Light ************************************************
// SMVSOutput_2L SimpleVertexShader_2Light_Planar(float3 position : POSITION, 
							  // float3 normal	 : NORMAL, 
							  // float2 texCoord : TEXCOORD0)
// {
    // SMVSOutput_2L output = (SMVSOutput_2L)0;

	// float4 posW = mul(float4(position ,1), World);	

	// output.PositionH = mul(posW, ViewProj);
	// output.PositionW = posW;		
	// output.NormalW	 = normalize(mul(normal, (float3x3)World));
	// output.TexCoord  = texCoord;
	// output.PositionL0 = mul(posW , LightVP[0]);
	// output.PositionL1 = mul(posW , LightVP[1]);
	// output.ReflSamplingPos = mul(posW, projMtx);	
    // return output;
// }

// float4 PhongPixelShader_2Light_Planar(float3 positionW : TEXCOORD0 ,
						// float3 normalW    : TEXCOORD1 , 
						// float2 texCoord   : TEXCOORD2 ,
						// float4 positionL0 : TEXCOORD3 ,
						// float4 positionL1 : TEXCOORD4 ,
						// float Occ		  : TEXCOORD5 ,
						// float4 reflSamplingPos : TEXCOORD6,
						// uniform bool hemisphere ) : COLOR0
// {   		
	// float3 diffuse = USE_DIFFUSE_MAP  ? tex2D(s2, texCoord) * surface.Diffuse : surface.Diffuse;
	// float3 specular = USE_SPECULAR_MAP ? tex2D(s3, texCoord) * surface.Specular : surface.Specular;
	
	// float4 color;
	// [branch]
	// if(hemisphere)
	// {
		// color = LightingHemisphericalShadowed(light[0], positionW, normalW, diffuse, specular, Occ ,positionL0 , s5);
		// color +=LightingHemisphericalShadowed(light[1], positionW, normalW, diffuse, specular, Occ ,positionL1 , s6);
	// }
	// else
	// {
		// color =	LightingAmbientShadowed(light[0], positionW, normalW, diffuse, specular,Occ ,positionL0 , s5);
		// color += LightingAmbientShadowed(light[1], positionW, normalW, diffuse, specular,Occ ,positionL1 , s6);
	// }
	
	// [branch]				
	// if(USE_REFLECTION_MAP)	
	// {		
		// float4 gloss = USE_GLOSS_MAP ? tex2D(s4, texCoord) : float4(1,1,1,1);
		// color += tex2Dproj(s1 , reflSamplingPos) * surface.Reflectivity * gloss;
	// }
	// [branch]
	// if(USE_REFRACTION_MAP)	
		// color += tex2Dproj(s7 , reflSamplingPos) * (1 - surface.Alpha);	
	// color.a = surface.Alpha;
	
    // return color;
// }

// SMBumpVSOutput_2L BumpVertexShader_2Light_Planar(float3 position : POSITION,
									  // float3 tangent  : TANGENT,
									  // float3 normal   : NORMAL,
									  // float2 texCoord : TEXCOORD0)
// {
	// SMBumpVSOutput_2L output = (SMBumpVSOutput_2L)0;

	// float4 posW =mul(float4(position ,1),World);
	
	// output.PositionH = mul(posW, ViewProj);
	// output.PositionW = posW;
	// output.NormalW = normalize(mul(normal, (float3x3)World));
	// output.TangentW = normalize(mul(tangent, (float3x3)World));	
	// output.BinormalW = normalize(cross(output.NormalW ,output.TangentW));
	// output.TexCoord = texCoord;
	// output.PositionL0 = mul(posW , LightVP[0]);
	// output.PositionL1 = mul(posW , LightVP[1]);
	// output.ReflSamplingPos = mul(posW, projMtx);	
	// return output;
// }

// float4 BumpPixelShader_2Light_Planar(SMBumpVSOutput_2L input , uniform bool hemisphere) : COLOR0
// {		
	// //pick and decode normal from the normal map 
	// float3 normal = tex2D(s0, input.TexCoord);
	// normal = (normal * 2) - 1;

	// // Move the normal from tangent space to world space
	// float3x3 tangentMatrix = {input.TangentW, input.BinormalW, input.NormalW};
	// normal = normalize(mul(normal, tangentMatrix));
		
	// return PhongPixelShader_2Light_Planar(input.PositionW, normal, input.TexCoord ,input.PositionL0, input.PositionL1, input.Occ, input.ReflSamplingPos ,hemisphere);	
// }




// //******** 1 Light **********************************************************
// technique PhongShading_Ambient_1L_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.					
        // VertexShader = compile vs_1_1 SimpleVertexShader_1Light_Planar();
        // PixelShader = compile ps_3_0 PhongPixelShader_1Light_Planar(false);
    // }
// }

// technique BumpPhongShading_Ambient_1L_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.			
        // VertexShader = compile vs_1_1 BumpVertexShader_1Light_Planar();
        // PixelShader = compile ps_3_0 BumpPixelShader_1Light_Planar(false);
    // }
// }
// technique PhongShading_Hemisphere_1L_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.					
        // VertexShader = compile vs_1_1 SimpleVertexShader_1Light_Planar();
        // PixelShader = compile ps_3_0 PhongPixelShader_1Light_Planar(true);
    // }
// }

// technique BumpPhongShading_Hemisphere_1L_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.			
        // VertexShader = compile vs_1_1 BumpVertexShader_1Light_Planar();
        // PixelShader = compile ps_3_0 BumpPixelShader_1Light_Planar(true);
    // }
// }

// //**************** 2 Light *******************************************
// technique PhongShading_Ambient_2L_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.					
        // VertexShader = compile vs_1_1 SimpleVertexShader_2Light_Planar();
        // PixelShader = compile ps_3_0 PhongPixelShader_2Light_Planar(false);
    // }
// }

// technique BumpPhongShading_Ambient_2L_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.			
        // VertexShader = compile vs_1_1 BumpVertexShader_2Light_Planar();
        // PixelShader = compile ps_3_0 BumpPixelShader_2Light_Planar(false);
    // }
// }
// technique PhongShading_Hemisphere_2L_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.					
        // VertexShader = compile vs_1_1 SimpleVertexShader_2Light_Planar();
        // PixelShader = compile ps_3_0 PhongPixelShader_2Light_Planar(true);
    // }
// }

// technique BumpPhongShading_Hemisphere_2L_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.			
        // VertexShader = compile vs_1_1 BumpVertexShader_2Light_Planar();
        // PixelShader = compile ps_3_0 BumpPixelShader_2Light_Planar(true);
    // }
// }
