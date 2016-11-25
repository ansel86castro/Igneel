//-----------------------------------------------------------------------------------------
// VertexShader I/O
//-----------------------------------------------------------------------------------------
struct VSInput
{
    float4 Position : POSITION; 
    float3 Normal : NORMAL;
    float3 Tangent : TANGENT;
    float2 TexCoords : TEXCOORD0;
};

struct VSOutput
{
    float4 PositionVS : SV_POSITION;
    float2 TexCoords : TEXCOORD0; 
    float3 Normal : TEXCOORD1;
    float3 Tangent : TEXCOORD2;
    float3 Binormal : TEXCOORD3;
    float3 Position : TEXCOORD4;
};

cbuffer camera
{
	float4x4 View;	
	float4x4 Projection;
};

cbuffer perObject
{	
	float4x4 World;
};


VSOutput main( VSInput input)
{
	 VSOutput output;
   
    // Transform to clip space by multiplying by the basic transform matrices.
    // An additional rotation is performed to illustrate vertex animation.    
    float4 worldPosition = mul(input.Position, World);
    output.PositionVS = mul(worldPosition, mul(View, Projection));
    
    // Move the incoming normal and tangent into world space and compute the binormal.
    // These three axes will be used by the pixel shader to move the normal map from 
    // tangent space to world space. 
    output.Normal = mul(input.Normal, World);
    output.Tangent = mul(input.Tangent, World);
    output.Binormal = cross(output.Normal, output.Tangent);
    output.Position = worldPosition.xyz;
     
    // Pass texture coordinates on to the pixel shader
    output.TexCoords = input.TexCoords;
    return output;    
}