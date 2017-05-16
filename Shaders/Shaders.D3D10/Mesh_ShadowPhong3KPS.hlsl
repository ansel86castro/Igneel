#define KERNEL_SIZE 3

#define DIRECTIONAL 1
#define POINT 2
#define SPOT 3

struct ShaderLight
{
	float3 Pos;
	float3 Dir;
	float3 Diffuse;
	float3 Specular;
	float3 Att;		// a0 a1,a2 attenuation factors
	float SpotPower;
	float Range;
	int Type;
	float Intensity;
};

struct SurfaceInfo
{
	float4 Diffuse;
	float Specular;
	float Emisive;
	float Reflectivity;
	float Refractitity;
	float SpecularPower;
};

struct ShadingInfo 
{
	float3 Emissive;
	float3 Diffuse;
	float3 Specular;
	float Alpha;
	float SpecPower;
	float GlossFactor;
	float OccFactor;
};

struct SMVSOutput
{
	float4 PositionH : SV_POSITION;
	float3 PositionW : TEXCOORD0;
	float3 NormalW	 : TEXCOORD1;
	float2 TexCoord	 : TEXCOORD2;
	float4 PositionL : TEXCOORD3;
	float Occ : TEXCOORD4;
	float4 ScreenCoord : TEXCOORD5;
};

cbuffer cbAmbient
{
	float3 SkyColor;
	float3 GroundColor;
	float3 NorthPole;
	float3 AmbientColor;
	bool HemisphericalLighting;
};

cbuffer cbCamera
{
	float3 EyePos;
};

cbuffer cbLight
{
	ShaderLight Light;
};

cbuffer cbTransparency
{
	bool NoRenderTransparency;
	bool NoRenderOpaque;
};

cbuffer cbClipPlane
{
	float4 ClipPlane		: CLIP_PLANE = { 0,0,0,0 };  //plane in Homogenius Space		
};


cbuffer cbMaterial
{
	SurfaceInfo Surface;
	bool USE_DIFFUSE_MAP;
	bool USE_SPECULAR_MAP;
	bool USE_ENVIROMENT_MAP;
	bool USE_REFLECTION_MAP;
	bool USE_REFRACTION_MAP;
	bool USE_EMISSIVE_MAP;
};

cbuffer cbShadowMap
{
	float SHADOW_EPSILON;
	float SmSize;
};

Texture2D    DiffuseMap;     // [a = opacity]
Texture2D    SpecularMap;    // [a = specularPower]
Texture2D    NormalMap;      // [a = Occlution Factor]
TextureCube  EnvironmentMap; // [a = Occlution Factor]
Texture2D    ReflectionMap;
Texture2D    RefractionMap;
Texture2D ShadowMap;


SamplerState sDiffuseMap; // Linear Sampler
SamplerState sSpecularMap;
SamplerComparisonState sShadowMap; //shadow Map sampler


float4 ComputeShadowTexCoord(float4 positionL, float3 normal, float3 toLight)
{
	float3 posL = positionL.xyz / positionL.w;
	float4 shadowTexCoord = float4(posL.x * 0.5f + 0.5f, -posL.y * 0.5f + 0.5f, posL.z, 1);

	float costheta = saturate(dot(normal, toLight));
	float bias = 0.005*tan(acos(costheta));

	shadowTexCoord.z -= bias;
	return shadowTexCoord;
}

float ComputeShadowFactor(float4 positionL, float3 normal, float3 toLight)
{
	float4 shadowCoord = ComputeShadowTexCoord(positionL, normal, toLight);

	float shadowFactor = 0;

	int start = -KERNEL_SIZE / 2;
	int end = KERNEL_SIZE / 2 + 1;

	float width, height;
	ShadowMap.GetDimensions(width, height);
	float tx = 1.0 / width;
	float ty = 1.0 / height;

	[unroll]
	for (float y = start; y < end; y++)
	{
		[unroll]
		for (float x = start; x < end; x++)
		{
			shadowFactor += ShadowMap.SampleCmpLevelZero(sShadowMap, shadowCoord.xy + float2(x * tx, y * ty), shadowCoord.z);
		}
	}

	shadowFactor /= (float)(KERNEL_SIZE * KERNEL_SIZE);
	return shadowFactor;
}

float3 ComputeHemisphere(float3 normal, float3 diffuse, float occ)
{
	float k = 0.5f + 0.5f * dot(normal, NorthPole);
	return diffuse * lerp(GroundColor, SkyColor, k) * occ; //( 1 - occlutionFactor);	
}


ShadingInfo GetShadingInfo(float2 texCoord)
{
	ShadingInfo si = (ShadingInfo)0;

	//add emisive Light contribution
	si.Emissive = Surface.Diffuse.rgb * Surface.Emisive;
	si.Diffuse = Surface.Diffuse.rgb;
	si.Specular = Surface.Diffuse.rgb * Surface.Specular;	
	si.SpecPower = Surface.SpecularPower;
	si.Alpha = Surface.Diffuse.a;

	float4 color;
	
	if (USE_DIFFUSE_MAP)
	{
		color = DiffuseMap.Sample(sDiffuseMap, texCoord);
		si.Diffuse *= color.rgb;
		si.Alpha *= color.a;
	}

	//clip(-((NoRenderTransparency && gAlpha != 1.0f) || (NoRenderOpaque && gAlpha == 1.0f)));
	if ((NoRenderTransparency && si.Alpha  < 1.0) || (NoRenderOpaque && si.Alpha == 1.0))
		discard;
	
	if (USE_SPECULAR_MAP)
	{
		color = SpecularMap.Sample(sSpecularMap, texCoord);
		si.Specular *= color.rgb;
		si.SpecPower *= color.a;		
	}

	return si;
}

float4 main(SMVSOutput input) : SV_TARGET
{   	
	clip(dot(float4(input.PositionW, 1), ClipPlane));		
	
	ShadingInfo si = GetShadingInfo(input.TexCoord);

	float4 color = float4(si.Emissive, si.Alpha);

	//add ambient contribution
	color.rgb += HemisphericalLighting ? 
			ComputeHemisphere(input.NormalW, si.Diffuse, 1) :
			si.Diffuse * AmbientColor;
	
	float3 toEye = normalize(EyePos - input.PositionW);

	if (Light.Type == DIRECTIONAL) {

		float3 toLight = normalize(-Light.Dir);

		float shadow = ComputeShadowFactor(input.PositionL, input.NormalW, toLight);

		float3 h = normalize(toLight + toEye);
		float NdotL =dot(input.NormalW, toLight);
		float NdotH = saturate( dot(input.NormalW, h) );	
		float spec = pow(NdotH, si.SpecPower);

		float intensity = saturate(NdotL);
		//add diffuse contribution
		float3 litColor = si.Diffuse * Light.Diffuse * Light.Intensity * intensity;	
		litColor += si.Specular * Light.Specular * Light.Intensity * spec;
		litColor *= shadow;
		

		color.rgb += litColor;
	}

	return  color;
}