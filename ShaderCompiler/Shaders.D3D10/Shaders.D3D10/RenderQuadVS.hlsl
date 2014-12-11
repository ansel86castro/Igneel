struct VS_RenderQuadInput
{
    float4 position : POSITIONT; 
    float2 texCoord : TEXCOORD0; 
};

struct PS_RenderQuadInput
{
    float4 position : SV_POSITION; 
    float2 texCoord : TEXCOORD0; 
};

cbuffer cb0
{
	float4x4 Transform;
	float4x4 TexTransform;
}

PS_RenderQuadInput main( VS_RenderQuadInput I )
{
    PS_RenderQuadInput O;
    	
	O.position = mul(I.position, Transform);
	O.texCoord = mul(float4(I.texCoord,0, 1), TexTransform).xy;
    
    return O;    
}