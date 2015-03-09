
struct VSOut
{
	float4 Position : SV_POSITION;
	float4 Color	: COLOR0;
};

float4 main(VSOut i) : SV_Target
{

	return i.Color;
}
