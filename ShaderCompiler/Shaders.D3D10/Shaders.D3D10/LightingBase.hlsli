#define LIGHTING_BASE

#ifndef GLOBALS
	#include "Globals.hlsli"
#endif

static const int DIRECTIONAL = 1;
static const int POINT = 2;
static const int SPOT = 3;

struct Light
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
	 //float4 data; // x= Transparecy, y =Relectivity , z =Refractivity , w =SpecularPower
};

cbuffer Ambient
{
	float3 sky;
	float3 ground;
	float3 northPole;
	float3 ambient;
	bool hemisphere;
};

cbuffer perCamera
{	
	float3 eyePos;	
};

cbuffer perLight
{	
	Light light;
};

cbuffer transparencyFlags
{
	bool fNoRenderTransparency;
	bool NoRenderOpaque;		
};

cbuffer clippingPlane
{
	float4 clipPlane		: CLIP_PLANE = {0,0,0,0};  //plane in Homogenius Space		
};


cbuffer perSurface
{
	SurfaceInfo surface;
	bool USE_DIFFUSE_MAP;
	bool USE_SPECULAR_MAP;
	bool USE_ENVIROMENT_MAP;
	bool USE_REFLECTION_MAP;
	bool USE_REFRACTION_MAP;
	bool USE_EMISSIVE_MAP;
};

float4 ParallelLight(float3 lightDir, float3 toEye, float3 normal, float3 diffuse ,float3 specular, float specularPower)
{
	// LightColor = l.Ambient * s.Ambient + (kd * s.Diffuse * l.Diffuse + ks * l.Specular * s.Specular) * shadowFactor;
	
	float3 reflecVec = reflect(lightDir, normal);
	float4 l = lit(dot(normal, -lightDir), dot(reflecVec, toEye),specularPower);	
	return float4(l.y * diffuse + l.z * specular, 0);
}

void PointLight()
{	
	float3 lightDir = light.Pos - gPositionW;
	float d = length(lightDir);
	if(d <= light.Range)
	{
		float3 toEye = normalize(eyePos - gPositionW);	
		lightDir /= d;		
	
		float3 h = normalize(lightDir + toEye);
		float4 l = lit(dot(gNormalW, lightDir), dot(gNormalW, h), 1);
		l.z = pow(l.z, gSpecularPower);	
	
		//add diffuse contribution
		float3 color = (gDiffuse * light.Diffuse) * l.y;		
	
		//add specular contribution
		color += (gSpecular * light.Specular) * l.z;	
	
		color *= light.Intensity;		
		color *= gShadowFactor;
		color /= dot(light.Att, float3(1.0f, d, d*d));
		
		gColor.rgb += color;
	}
}

void SpotLight()
{
	float3 lightDir = light.Pos - gPositionW;
	float d = length(lightDir);
	if(d <= light.Range)
	{
		float3 toEye = normalize(eyePos - gPositionW);	
		lightDir /= d;		
	
		float3 h = normalize(lightDir + toEye);
		float4 l = lit(dot(gNormalW, lightDir), dot(gNormalW, h), 1);
		l.z = pow(l.z, gSpecularPower);	
	
		//add diffuse contribution
		float3 color = (gDiffuse * light.Diffuse) * l.y;		
	
		//add specular contribution
		color += (gSpecular * light.Specular) * l.z;	
	
		color *= light.Intensity;		
		color *= gShadowFactor;
		color /= dot(light.Att, float3(1.0f, d, d*d));				

		float kSpot = pow(saturate(dot(-lightDir , light.Dir)) , light.SpotPower);
		color *= kSpot;		

		gColor.rgb += color;
	}	
}

void DirectionalLight()
{
	float3 toEye = normalize(eyePos - gPositionW);	
    float3 lightDir = -light.Dir;

	float3 h = normalize(lightDir + toEye);
	float4 l = lit(dot(gNormalW, lightDir), dot(gNormalW, h), 1);
	l.z = pow(l.z, gSpecularPower);	
	
	//add diffuse contribution
	float3 color = (gDiffuse * light.Diffuse) * l.y;		
	
	//add specular contribution
	color += (gSpecular * light.Specular) * l.z;	
	
	color *= light.Intensity;		
	color *= gShadowFactor;	
	
	gColor.rgb += color;
}

float4 PointLight(float3 lightPos, float3 att ,float range ,float3 pos, float3 normal, float3 toEye, 
				  float3 diffuse ,float3 specular, float specularPower)
{
	// LightColor = (l.Ambient * s.Ambient + ks * s.Diffuse * l.Diffuse + ks * l.Specular * s.Specular) / (l.Att.x  + l.Att.y * d + a.Att.z * d^2)	

	float3 lightDir = lightPos - pos;	
	// The distance from surface to light.
	float d = length(lightDir);
	if( d > range ) return float4(0.0f, 0.0f, 0.0f, 0.0f);
	// Normalize the light vector.
	lightDir /= d;	
		
	float3 reflecVec = reflect(-lightDir, normal);	//otra forma es float vHalf = normalize(toEye + (-lightDir));
	float4 lc = lit(dot(normal, lightDir), dot(reflecVec, toEye), specularPower);
	// attenuate
	return float4((lc.y * diffuse + lc.z * specular) / dot(att, float3(1.0f, d, d*d)) , 0);
}

float4 SpotLight(float3 lightPos, float3 lightDir, float3 att ,float spotPower, 
				float range ,float3 pos, float3 normal, float3 toEye, 
				float3 diffuse ,float3 specular, float specularPower)
{
	// LightColor =kSpot * (l.Ambient * s.Ambient + ks * s.Diffuse * l.Diffuse + ks * l.Specular * s.Specular) / (l.Att.x  + l.Att.y * d + a.Att.z * d^2)

	float4 color = PointLight(lightPos ,att ,range ,pos ,normal ,toEye, diffuse ,specular ,specularPower);
	float3 _lightDir = normalize(pos - lightPos);
	float kSpot = pow(saturate(dot(_lightDir , lightDir)) , spotPower);
	color *= kSpot;
	return color;
}

float3 ComputeHemisphere(float3 sky, float3 ground, float3 diffuse, float3 normal, 
						float3 nortPole, float occlutionFactor )
{
	float k = 0.5f + 0.5f * dot(normal, nortPole);
	float3 color = diffuse * lerp(ground ,sky , k) * ( 1 - occlutionFactor);
	return color;
}

void ComputeHemisphere()
{	
	float k = 0.5f + 0.5f * dot(gNormalW, northPole);
	gColor.rgb += gDiffuse * lerp(ground ,sky , k) * gOcc; //( 1 - occlutionFactor);	
}

void ComputeAmbient()
{
	gColor.rgb += gDiffuse.rgb * ambient * gOcc; //(1 - gOcc)
}

void ComputeHemisphere(out float3 color)
{	
	float k = 0.5f + 0.5f * dot(gNormalW, northPole);
	color = gDiffuse * lerp(ground ,sky , k) * gOcc; //( 1 - occlutionFactor);	
}

void ComputeAmbient(out float3 color)
{
	color = gDiffuse.rgb * ambient * gOcc; //(1 - gOcc)
}

void ComputeDirectLighting()
{
	// LightColor = (l.Ambient * s.Ambient + kd * s.Diffuse * l.Diffuse + ks * l.Specular * s.Specular) /
	//				(l.Att.x  + l.Att.y * d + a.Att.z * d^2)	
	
	float3 toEye = normalize(eyePos - gPositionW);	
	float att = 1;
	float kSpot = 1;
	float3 lightDir = light.Dir;
	
	[branch]	
	if(light.Type != DIRECTIONAL)
	{
		// Point or Spot
		lightDir = gPositionW - light.Pos;
		float d = length(lightDir);
		lightDir /= d;
		
		[branch]
		if(light.Type == SPOT)
			kSpot = pow(saturate(dot(lightDir , light.Dir)) , light.SpotPower);
			
		att = dot(light.Att, float3(1.0f, d, d*d));
		
		if(d > light.Range)
			kSpot = 0.0f;
	}
	
	lightDir = -lightDir;
	//float3 reflecVec = reflect(lightDir, normalW);
	float3 h = normalize(lightDir + toEye);
	float4 l = lit(dot(gNormalW, lightDir), dot(gNormalW, h), 1);
	l.z = pow(l.z, gSpecularPower);	
	
	//add diffuse contribution
	float3 color = (gDiffuse * light.Diffuse) * l.y;		
	
	//add specular contribution
	color += (gSpecular * light.Specular) * l.z;	
	
	color *= light.Intensity;	
	color *= kSpot;
	color *= gShadowFactor;
	color /= att;
	
	gColor.rgb += color;
}