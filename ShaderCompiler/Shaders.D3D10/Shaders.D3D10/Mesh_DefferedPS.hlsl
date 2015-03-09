#include "Lighting.hlsli"
#include "Deffered.hlsli"

struct PSIn
{
	float4 PositionH    : SV_POSITION;
	float4 PositionRhz  : TEXCOORD0;	
	float4 NormalW	    : TEXCOORD1;
	float2 Texcoord	    : TEXCOORD2;	
};

struct PSOut
{
	float4 PositionRhz : SV_TARGET0;
	float4 NormalW	   : SV_TARGET1;
	float4 Diffuse     : SV_TARGET2; //alpha channel store occ
	float4 Specular    : SV_TARGET3;	// alpha channel store specularPower		
};



PSOut main(PSIn input)
{
	//clip(dot(float4(input.PositionW, 1), ClipPlane));			
		
	float4 diffuse = Surface.Diffuse;

	[branch]
	if(USE_DIFFUSE_MAP)	
		diffuse *= DiffuseMap.Sample(sDiffuseMap ,input.Texcoord);			

	clip(-(NoRenderTransparency && diffuse.a != 1.0f));

	PSOut o = (PSOut)0;	
	o.Diffuse = float4(diffuse.rgb , Surface.Emisive);
	o.Specular = float4(Surface.Specular, Surface.Reflectivity ,Surface.SpecularPower , 1);		
	
	[branch]
	if(USE_SPECULAR_MAP)	
		o.Specular.ra *= SpecularMap.Sample(sDiffuseMap ,input.Texcoord).ra;			

	o.PositionRhz = float4(PackPositionForFatFramebuffer(input.PositionRhz), 1);
	//o.PositionRhz = input.PositionRhz;
	o.NormalW = PackNormalForFatFramebuffer(input.NormalW);	

	/*[branch]
	if(USE_ENVIROMENT_MAP)
	{
		o.Indirect = SampleIndirectEvMap();				
	}
	else
	{		
		[branch]
		if (USE_REFLECTION_MAP)				
			o.Indirect.rgb += gDiffuse * t5.Sample(s5, gScreenCoord.xy / gScreenCoord.w).rgb * gGlossFactor;

		[branch]
		if (USE_REFRACTION_MAP)	
			o.Indirect.rgb += gDiffuse * t6.Sample(s6, gScreenCoord.xy / gScreenCoord.w).rgb * (1 - gAlpha);
	}	
*/
	return o;	

	/*PSOut o = (PSOut)0;	
	o.PositionRhz = float4(1,0,0,1);
	o.NormalW =     float4(0,1,0,1);
	o.Diffuse =    float4(0,0,1,1);
	o.Specular =   float4(1,1,1,1);
	o.Indirect.a = 1;
	return o;*/
}