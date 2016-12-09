int DESCRIPTION
<
	string ShaderModel = "3.0";
	string Version = "1.0.0";	
	int DiffuseSamplersNumbers = 1;
	int DiffuseSamplerStartRegister = 0;	
>;

float4 color : COLOR = {1.0, 1.0, 1.0, 1.0};
float4x4 wvp : WORLDVIEWPROJ;

sampler s0 : register(s0);


struct VSOut
{
	float4 positionH:POSITION;
	float2 texCoord	:TEXCOORD0;
};

VSOut VS(float4 position:POSITION, float2 texCoord:TEXCOORD0)
{
	VSOut output = (VSOut)0;
	output.positionH = mul(position, wvp);
	output.texCoord = texCoord;
	return output;
}

float4 PS(VSOut input):COLOR0
{
	return tex2D(s0, input.texCoord) * color;
}

technique T0
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS();
        PixelShader = compile ps_2_0 PS();
	}
}