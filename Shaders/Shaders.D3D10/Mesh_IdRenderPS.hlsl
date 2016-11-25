
struct VSOut
{
	float4 Position : SV_POSITION;
	float4 Color	:COLOR0;
};

cbuffer cbId
{
	float4 Id;
}

float4 main(VSOut i) : SV_TARGET
{	
	//return float4(0.5,0,0,0);
	return Id;
}