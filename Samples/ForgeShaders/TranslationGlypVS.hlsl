

cbuffer camera
{
	float4x4 View;
	float4x4 Proj;
};
cbuffer perObject
{
	float4x4 World;
	//float3 displacement = float3(0,0,10);
};


static float3 displacement = float3(0,0,13);

struct VSOut
{
	float4 Position : SV_POSITION;
	float4 Color	:COLOR0;
};

VSOut main(float4 Position : POSITION,
		   float4 Color	:COLOR0)
{
	float4 gPositionH = Position;
	float3 gPositionW = mul(gPositionH, World).xyz;
	gPositionW = mul(gPositionW, (float3x3)View) + displacement;
	gPositionH = mul(float4(gPositionW,1), Proj);


	VSOut o;
	o.Position = gPositionH;
	o.Color = Color;
	return o;
}