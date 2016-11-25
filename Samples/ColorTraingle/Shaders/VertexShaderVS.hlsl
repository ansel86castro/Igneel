struct VSOut
{
	float4 Position : SV_POSITION;
	float4 Color	:COLOR0;
};

cbuffer cb0
{
	float4x4 WorldViewProj;
};


VSOut main(float4 Position : POSITION, float4 Color	:COLOR0)
{	
	VSOut o;
	o.Position = mul(Position, WorldViewProj);
	o.Color = Color;
	return o;
}