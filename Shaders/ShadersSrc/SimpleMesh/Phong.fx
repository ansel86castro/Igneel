#include "../Include/PixelShader.fxh"
#include "../Include/VertexShader.fxh"

int DESCRIPTION
<
	string ShaderModel = "3.0";
	string Version = "1.0.0";
	int NumberOfLights = 1;		
	bool SupportAmbient = true;	
	bool SupportHemispherical = true;
	bool SupportSurfaceInfo = true;			
>;



// int activeLights: ACTIVE_LIGHTS = 0;
int vsIndex;
int psIndex;
							  

PixelShader psShaders[8] = {            compile ps_3_0 PhongPixelShader(false, 0),
										compile ps_3_0 PhongPixelShader(false, 1),																				
										compile ps_3_0 PhongPixelShader(true, 0),
										compile ps_3_0 PhongPixelShader(true, 1),										
										
										compile ps_3_0 BumpPixelShader(false, 0),
										compile ps_3_0 BumpPixelShader(false, 1),																				
										compile ps_3_0 BumpPixelShader(true, 0),
										compile ps_3_0 BumpPixelShader(true, 1)										
							  };
  
							  
VertexShader vertexShaders[2] = {
									compile vs_1_1 SimpleVertexShader(),
									compile vs_1_1 BumpVertexShader()
								};								

technique T0
{
	pass Default
	{			
		 //Direct color rendering
		 VertexShader = vertexShaders[vsIndex];
		 PixelShader  =	psShaders[activeLights + psIndex];
	}
	
	pass EnvMap
	{
		//environment map rendering
		 ALPHABLENDENABLE = TRUE;
		 SRCBLEND = SRCALPHA;
		 DESTBLEND = ONE;
		 
		 VertexShader = compile vs_1_1 SampleEnvironmentMapVS();
		 PixelShader  =	compile ps_2_0 SampleEnvironmentMapPS();
	}
	
	pass RefleMap
	{
		ALPHABLENDENABLE = TRUE;
		SRCBLEND = SRCALPHA;
		DESTBLEND = ONE;
		
		VertexShader = compile vs_1_1 SampleReflectionMapVS();
		PixelShader  =	compile ps_2_0 SampleReflecionMapPS();
	}
}

//**************** Planar Reflec Map ******************************************
// VSOut SimpleVertexShaderPlanar(float3 position : POSITION, 
								  // float3 normal	 : NORMAL, 
								  // float2 texCoord : TEXCOORD0)
// {
    // VSOut output = (VSOut)0;

	// float4 posW = mul(float4(position ,1),World);
	
	// output.PositionH = mul(posW, ViewProj);
	// output.PositionW = posW;
	// output.NormalW =normalize(mul(normal, (float3x3)World));
	// output.TexCoord = texCoord;
	// output.ReflSamplingPos = mul(posW, projMtx);	
    // return output;
// }

// float4 PhongPixelShaderPlanar(VSOut input, uniform bool hemisphere) : COLOR0
// {   	
		
	// float3 diffuse = USE_DIFFUSE_MAP ? tex2D(s2, input.TexCoord) * surface.Diffuse : surface.Diffuse;
	// float3 specular = USE_SPECULAR_MAP ? tex2D(s3, input.TexCoord) * surface.Specular : surface.Specular;
	
	// float4 color = 0;
	// [branch]
	// if (hemisphere)
		// color = LightingHemispherical(input.PositionW, input.NormalW, diffuse, specular, input.Occ);
	// else 
		// color = LightingAmbient(input.PositionW, input.NormalW, diffuse, specular, input.Occ);
    
	// [branch]	
	// if(USE_REFLECTION_MAP)	
	// {		
		// float4 gloss = USE_GLOSS_MAP ? tex2D(s4, input.TexCoord) : float4(1,1,1,1);
		// color += tex2Dproj(s1 ,input.ReflSamplingPos) * surface.Reflectivity * gloss;
	// }
	// [branch]
	// if(USE_REFRACTION_MAP)	
		// color += tex2Dproj(s5 ,input.ReflSamplingPos)* (1 - surface.Alpha);	
	// color.a = surface.Alpha;
	
    // return color;
// }
// //****************************************************************************************************

// //********************* Normal Mapping with Phong lighting *******************************************
// VSOutBump BumpVertexShaderPlanar(float3 position : POSITION,
							  // float3 tangent  : TANGENT,
							  // float3 normal   : NORMAL,
							  // float2 texCoord : TEXCOORD0)
// {
	// VSOutBump output = (VSOutBump)0;

	// float4 posW = mul(float4(position ,1), World);
	
	// output.PositionH = mul(posW, ViewProj);
	// output.PositionW = posW;
	// output.NormalW = normalize(mul(normal,(float3x3)World));
	// output.TangentW = normalize(mul(tangent,(float3x3)World));	
	// output.BinormalW = normalize(cross(output.NormalW ,output.TangentW));
	// output.TexCoord = texCoord;
	// output.ReflSamplingPos = mul(posW, projMtx);
	// return output;
// }

// float4 BumpPixelShaderPlanar(VSOutBump input, uniform bool hemisphere) : COLOR0
// {
	// // if(enableClipping)
		// // clip(dot(clipPlane, input.PositionH));		
		
	// //pick and decode normal from the normal map 
	// float3 normal = tex2D(s0, input.TexCoord);
	// normal = (normal * 2) - 1;

	// // Move the normal from tangent space to world space
	// float3x3 tangentMatrix = {input.TangentW, input.BinormalW, input.NormalW};
	// normal = normalize(mul(normal, tangentMatrix));
		
	// float3 diffuse = USE_DIFFUSE_MAP ? tex2D(s2, input.TexCoord) * surface.Diffuse : surface.Diffuse;
	// float3 specular = USE_SPECULAR_MAP ? tex2D(s3, input.TexCoord) * surface.Specular : surface.Specular;
	
	// float4 color = 0;
	// [branch]
	// if (hemisphere)
		// color = LightingHemispherical(input.PositionW, normal, diffuse, specular, input.Occ);
	// else 
		// color = LightingAmbient(input.PositionW, normal, diffuse, specular, input.Occ);
    
	// [branch]	
	// if(USE_REFLECTION_MAP)	
	// {		
		// float4 gloss = USE_GLOSS_MAP ? tex2D(s4, input.TexCoord) : float4(1,1,1,1);
		// color += tex2Dproj(s1 ,input.ReflSamplingPos) * surface.Reflectivity * gloss;
	// }
	// [branch]
	// if(USE_REFRACTION_MAP)	
		// color += tex2Dproj(s5 ,input.ReflSamplingPos) * (1 - surface.Alpha);	
	// color.a = surface.Alpha;
	
    // return color;
// }
// //******************************************************************************************************

// technique PhongShading_Ambient_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.					
        // VertexShader = compile vs_1_1 SimpleVertexShaderPlanar();
        // PixelShader = compile ps_3_0 PhongPixelShaderPlanar(false);
    // }
// }

// technique BumpPhongShading_Ambient_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.			
        // VertexShader = compile vs_1_1 BumpVertexShaderPlanar();
        // PixelShader = compile ps_3_0 BumpPixelShaderPlanar(false);
    // }
// }
// technique PhongShading_Hemisphere_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.					
        // VertexShader = compile vs_1_1 SimpleVertexShaderPlanar();
        // PixelShader = compile ps_3_0 PhongPixelShaderPlanar(true);
    // }
// }

// technique BumpPhongShading_Hemisphere_Planar
// {
    // pass Pass1
    // {
        // // TODO: set renderstates here.			
        // VertexShader = compile vs_1_1 BumpVertexShaderPlanar();
        // PixelShader = compile ps_3_0 BumpPixelShaderPlanar(true);
    // }
// }
