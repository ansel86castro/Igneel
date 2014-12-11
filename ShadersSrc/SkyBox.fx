float4x4 ViewProj 	 : VIEWPROJ;
float lightIntensity : FLOAT  = 1;

samplerCUBE cubeSampler :register(s0);

void VertexShaderFunction(in float4 position : POSITION ,
						  out float4 positionH : POSITION,
						  out float3 positionW : TEXCOORD0)
{    
    // Output the position
    positionH = mul(position, ViewProj).xyww;
    
    // Calculate the cube map texture coordinates
    // Because this is a cube-map, the 3-D texture coordinates are calculated
    // from the world-position of the skybox vertex.
    // position (from the skybox mesh) is considered to be pre-transformed into world space
    positionW = position;
}

float4 PixelShaderFunction(float3 positionW : TEXCOORD0) : COLOR0
{
    return texCUBE(cubeSampler, positionW) * lightIntensity;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.		      

        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
		
    }
}
