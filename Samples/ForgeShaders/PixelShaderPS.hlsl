
struct Light
{
	float3 Direction;
	float3 Color;
};

struct VSOutput
{
    float4 PositionVS : SV_POSITION;
    float2 TexCoords : TEXCOORD0; 
    float3 Normal : TEXCOORD1;
    float3 Tangent : TEXCOORD2;
    float3 Binormal : TEXCOORD3;
    float3 Position : TEXCOORD4;
};


cbuffer cbLight
{
	float ReflectionRatio;
	float SpecularRatio;
	float SpecularStyleLerp;
	int SpecularPower;	
	float4x4 View;
	Light DirectionalLight;
	float3 CameraPosition;
};



Texture2D DiffuseTexture;
Texture2D NightTexture;
Texture2D NormalMapTexture;
Texture2D ReflectionMask;

SamplerState sDiffuseTexture;

float4 main(VSOutput input) : SV_TARGET
{	
    float3 EyeVector = normalize(input.Position - CameraPosition );

    
    // Look up the normal from the NormalMap texture, and unbias the result
    float3 Normal = NormalMapTexture.Sample(sDiffuseTexture, input.TexCoords).rgb;
    Normal = (Normal * 2) - 1;
    
    // Move the normal from tangent space to world space
    float3x3 tangentFrame = {input.Tangent, input.Binormal, input.Normal};
    Normal = normalize(mul(Normal, tangentFrame));
    
    // Start with N dot L lighting
    float light = saturate( dot( Normal, -DirectionalLight.Direction ) );
    float3 color = DirectionalLight.Color * light;
    
    // Modulate against the diffuse texture color
    float4 diffuse = DiffuseTexture.Sample(sDiffuseTexture, input.TexCoords);
    color *= diffuse.rgb;
    
    // Add ground lights if the area is not in sunlight
    float sunlitRatio = saturate(2*light);
    float4 nightColor =NightTexture.Sample(sDiffuseTexture, input.TexCoords);
    color = lerp( nightColor.xyz, color, float3( sunlitRatio, sunlitRatio, sunlitRatio) );
       
    
   // Add a specular highlight
	float reflectionMask = ReflectionMask.Sample(sDiffuseTexture, input.TexCoords);
    float3 vHalf = normalize( -EyeVector + -DirectionalLight.Direction );
    float PhongSpecular = saturate(dot(vHalf, Normal));

	float3 reflection = normalize( reflect(EyeVector, Normal) );
    float BlinnSpecular = saturate(dot(reflection ,-DirectionalLight.Direction ));
    float specular= lerp(PhongSpecular, BlinnSpecular, SpecularStyleLerp );

    color += saturate(DirectionalLight.Color * ( pow(specular, SpecularPower) * SpecularRatio * reflectionMask));
    
    
    // Add atmosphere
    float atmosphereRatio = 1 - saturate( dot(-EyeVector, input.Normal) );
    color += 0.30f * float3(.3, .5, 1) * pow(atmosphereRatio, 2);
    
    // Set alpha to 1.0 and return
    return float4(color, 1.0);
}