#define KERNEL_SIZE 3

struct PS_RenderQuadInput
{
    float4 position : SV_POSITION; 
    float2 texCoord : TEXCOORD0; 
};

Texture2D t0 : register(t0);
SamplerState s0 : register(s0);

float4 main(PS_RenderQuadInput input) : SV_TARGET
{	
   float color = 0;
   int start = -KERNEL_SIZE / 2;
   int end = KERNEL_SIZE / 2 + 1;

    [unroll]
	for(float y = start ; y < end ; y++)
	{
		[unroll]
		for(float x = start ; x < end ; x++)
		{
			color += t0.SampleLevel(s0, input.texCoord , 0, int2(x,y)).r;
		}
	}		

	float r = (float)any(color);	
	return float4(r,r,r,1);
}