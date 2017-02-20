#ifndef LIGHTING_BASE
	#include "LightingBase.hlsli"
#endif


struct BlendVSOutput
{
	float4 PositionH 	 :SV_POSITION;
	float3 PositionW 	 :TEXCOORD0;
	float3 NormalW	 	 :TEXCOORD1;
	float2 TexCoord	  	 :TEXCOORD2;
	float2 BlendCoord :TEXCOORD3;	
};

Texture2D Layers[4] :register(t0);
Texture2D BlendLayer:register(t4);

SamplerState sLayers:register(s0); // Linear Sampler



void InitLighting(BlendVSOutput input)
{
	//add emisive Light contribution
	gColor = float4(Surface.Diffuse.rgb * Surface.Emisive, 1);		

	[branch]
	if (USE_DIFFUSE_MAP)
	{
		gDiffuse = float3(0, 0, 0);
		float4 fblend = USE_SPECULAR_MAP? BlendLayer.Sample(sLayers, input.BlendCoord): float4(1,1,1,1);
		float blendWeights[4] = (float[4])fblend;

		[unroll]
		for (int i = 0; i < 4; ++i)
		{
			gDiffuse.rgb += Layers[i].Sample(sLayers, input.TexCoord).rgb *  blendWeights[i];
		}

		gDiffuse.rgb *= Surface.Diffuse.rgb;
	}
	else
	{
		gDiffuse.rgb = Surface.Diffuse.rgb;
	}

	gSpecular = Surface.Diffuse.rgb * Surface.Specular;
	gSpecularPower = Surface.SpecularPower;
	gAlpha = Surface.Diffuse.a;
	gGlossFactor = Surface.Reflectivity;

	//clip(-((NoRenderTransparency && gAlpha != 1.0f) || (NoRenderOpaque && gAlpha == 1.0f)));
	if ((NoRenderTransparency && gAlpha < 1.0) || (NoRenderOpaque && gAlpha == 1.0))
		discard;

	gColor.a = gAlpha;
}

void ComputeLighting()
{
	
	[branch]
	if (HemisphericalLighting)
		ComputeHemisphere();
	else
		ComputeAmbient();

	[branch]
	if (Light.Type == DIRECTIONAL)
		DirectionalLight();
	else[branch] if (Light.Type == SPOT)
		SpotLight();
	else[branch] if (Light.Type == POINT)
		PointLight();

}

float4 main(BlendVSOutput input) : SV_TARGET
{
	clip(dot(float4(input.PositionW, 1), ClipPlane));
	
	gPositionW = input.PositionW;
	gNormalW = input.NormalW;
	gTexCoord = input.TexCoord;
	gOcc = 1;
	gShadowFactor = 1;

	InitLighting(input);

	ComputeLighting();

	return  gColor;
}