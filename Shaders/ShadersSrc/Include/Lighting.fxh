
#define LIGHTING

static const int DIRECTIONAL = 0;
static const int POINT = 1;
static const int SPOT = 2;
static const float RGB16_MAX = 100;
static const float RGB16_EXP = 5;

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
	 float3 Diffuse;
     float3 Specular;
	 float3 Emisive;
     float Alpha;
     float Reflectivity;
     float Refractitity;
     float SpecularPower;	 
	 //float4 data; // x= Transparecy, y =Relectivity , z =Refractivity , w =SpecularPower
};

float4 ParallelLight(float3 lightDir, float3 toEye, float3 normal, float3 diffuse ,float3 specular, float specularPower)
{
	// LightColor = l.Ambient * s.Ambient + (kd * s.Diffuse * l.Diffuse + ks * l.Specular * s.Specular) * shadowFactor;
	
	float3 reflecVec = reflect(lightDir, normal);
	float4 l = lit(dot(normal, -lightDir), dot(reflecVec, toEye),specularPower);	
	return float4(l.y * diffuse + l.z * specular, 0);
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

//Compute the Shadow Factor 0 = in shadow 1 = not in Shadow
float ComputeShadowFactorAvg(float4 positionL , sampler2D shadowSampler, float SHADOW_EPSILON, float2 texOffset[9])
{	
	positionL.xyz /= positionL.w;	
	float2 shadowTc = float2(positionL.x * 0.5f + 0.5f, -positionL.y * 0.5f + 0.5f);				
    float fShadowTerm = tex2D(shadowSampler, shadowTc).r; 
		
	 //0 in shadow 1 not in shadow
	 if(fShadowTerm == 1 || fShadowTerm == 0)
		return 1;			
	// else
	// {
		//float depth = positionL.z;
		float depth = positionL.z - SHADOW_EPSILON;				 
	   // Sample each of them checking whether the pixel under test is shadowed or not     
		[unroll(9)]
		for( int i = 0; i < 9; i++ )
		{      
			// Texel is shadowed    
			fShadowTerm += tex2D(shadowSampler, shadowTc + texOffset[i]).r > depth;
			//fShadowTerm += 1 - (tex2D(shadowSampler, shadowTc + texOffset[i]).r > depth);
		}
		// Get the average 
		return fShadowTerm / 9.0f;   
   // }
}


float ComputeShadowFactorDegradated(float4 positionL , sampler2D shadowSampler, float SHADOW_EPSILON, float2 texOffset[9])
{
	// texOffset indices
	// 4 3 5
	// 1 0 2
	// 7 6 8
	
	//complete projection
	positionL.xyz /= positionL.w;	
	
	//transform to texture space
	float2 shadowTc = float2(positionL.x * 0.5 + 0.5, -positionL.y * 0.5f + 0.5f);				
	
	//float SM_SIZE = 1.0f / ( 2 * texOffset[2].x /3);
	float SM_SIZE = 1.0f / texOffset[2].x;
	
	// Transform to texel space.
	float2 p = SM_SIZE * shadowTc;	
	
	//Compute texel center
	float2 c = trunc(p) + float2(0.5f,0.5f);
	
	//Find the closest texel center to p
	int index = 1;	
	float2 displ = p - c;
	float2 s =  normalize(displ);	
	float2 q = trunc(( shadowTc + texOffset[1] ) * SM_SIZE) + float2(0.5f,0.5f);
	float minDistance = -0.5f * dot(normalize(q - c), s) + 0.5f;	
	float dist;
	float2 temp;	
	
	[unroll(9)]
	for( int i = 2; i < 9; i++ )
	{     
		temp = trunc(( shadowTc + texOffset[i] ) * SM_SIZE) + float2(0.5f,0.5f);	
		dist = -0.5f * dot(normalize(temp - c), s) + 0.5f;				
		
		if(dist < minDistance)
		{
			minDistance = dist;
			q = temp;
			index = i;
		}
	}
			
	float2 v = q - c;
	//compute the projection of the vector p-c of in the vector q - c
	float proj = dot(displ, v);
	
	float lerpFactor = proj / length(v);
		
	float k = (1 - positionL.z) + SHADOW_EPSILON;
	
	float shadowFactor0 = 1 - (tex2D(shadowSampler, shadowTc).r > k);
	float shadowFactor1 = 1 - (tex2D(shadowSampler, shadowTc + texOffset[index]).r > k);
	
	return lerp(shadowFactor0 , shadowFactor1 , lerpFactor);	
}

float Cerp(float xs[3] ,float ys[3], float x)
{
	float l0 = ((x - xs[1]) * (x - xs[2])) / ((xs[0] - xs[1]) * (xs[0] - xs[2]));
	float l1 = ((x - xs[0]) * (x - xs[2])) / ((xs[1] - xs[0]) * (xs[1] - xs[2]));
	float l2 = ((x - xs[0]) * (x - xs[1])) / ((xs[2] - xs[0]) * (xs[2] - xs[1]));

	return l0 * ys[0] + l1 * ys[1] + l2 * ys[2];
}

float ComputeShadowFactorTrilinear(float4 positionL , sampler2D shadowSampler, float SHADOW_EPSILON, float2 texOffset[9])
{
	// texOffset indices
	// 4 3 5
	// 1 0 2
	// 7 6 8
	
	//complete projection
	positionL.xyz /= positionL.w;	
	
	//transform to texture space
	float2 shadowTc = float2(positionL.x * 0.5 + 0.5, -positionL.y * 0.5f + 0.5f);				
	
	//float SM_SIZE = 1.0f / ( 2 * texOffset[2].x /3);
	float SM_SIZE = 1.0f / texOffset[2].x;
		
	// Transform to texel space.
	float2 texelPos = SM_SIZE * shadowTc;				
		
	float k =  1 - positionL.z + SHADOW_EPSILON;
	float3 center[9];
	float r[3];
	
	[unroll(9)]
	for(int i = 0; i < 9; i++)
	{
		center[i].xy = trunc(( shadowTc + texOffset[i] ) * SM_SIZE) + float2(0.5f,0.5f);
		center[i].z = 1 - (tex2D(shadowSampler, shadowTc + texOffset[i]).r > k);
	}
	
	float xs[3] = { center[4].x, center[3].x, center[5].x };
	float ys[3] = { center[4].z, center[3].z, center[5].z };	
	r[0] = Cerp(xs , ys, texelPos.x);
	
	xs[0] = center[1].x; xs[1]=center[0].x; xs[2] = center[2].x;
	ys[0] = center[1].z; ys[1]=center[0].z; ys[2]  = center[2].z;
	r[1] = Cerp(xs , ys, texelPos.x);
	
	xs[0] = center[7].x; xs[1]=center[6].x;   xs[2]= center[8].x;
	ys[0] = center[7].z; ys[1] = center[6].z; ys[2] = center[8].z;
	r[2] = Cerp(xs , ys, texelPos.x);
	
	xs[0] = center[4].y; xs[1] = center[1].y; xs[2]= center[7].y;
	float result = Cerp(xs , r, texelPos.y);
	
	return result;
}

float ComputeShadowFactorBilinear(float4 positionL, sampler2D shadowSampler, float SHADOW_EPSILON, float2 texOffset[9])
{
	positionL.xyz /= positionL.w;
	float2 shadowTc = float2(positionL.x * 0.5 + 0.5, -positionL.y * 0.5f + 0.5f);

	//float SM_SIZE = 1.0f / ( 2 * texOffset[2].x /3);
	float SM_SIZE = 1.0f / texOffset[2].x ;
	float fTexelSize = texOffset[2].x;

	//float depth = positionL.z;
	float depth = 1 - positionL.z;
	//float k = depth - SHADOW_EPSILON;
	float k = depth + SHADOW_EPSILON;
	// Is the pixel shadow map value > depth?	
	
	// Sample shadow map to get nearest depth to light.
	
	float result0 = 1 - (tex2D(shadowSampler, shadowTc).r > k);
	float result1 = 1 - (tex2D(shadowSampler, shadowTc + float2(fTexelSize, 0)).r > k);
	float result2 = 1 - (tex2D(shadowSampler, shadowTc + float2(0, fTexelSize)).r > k);
	float result3 = 1 - (tex2D(shadowSampler, shadowTc + float2(fTexelSize, fTexelSize)).r > k);
	
	// Transform to texel space.
	float2 texelPos = SM_SIZE * shadowTc;
	
	// Determine the interpolation amounts.
	float2 t = frac( texelPos );
		
	//Interpolate results.
	return lerp( lerp(result0, result1, t.x), 
	             lerp(result2, result3, t.x), t.y);	

}

float ComputeCubeShadowFactor(float3 ligthDirection, float distFromLight, samplerCUBE shadowSampler,float SHADOW_EPSILON)
{	
	float pointDepth = texCUBE(shadowSampler, ligthDirection).r;
	
	if(pointDepth == 0)
		return 1;	

	float k = distFromLight - SHADOW_EPSILON;	
	return 	pointDepth > k;
}

float3 ComputeHemisphere(float3 sky, float3 ground, float3 diffuse, float3 normal, 
						float3 nortPole, float occlutionFactor )
{
	float k = 0.5f + 0.5f * dot(normal, nortPole);
	float3 color = diffuse * lerp(ground ,sky , k) * ( 1 - occlutionFactor);
	return color;
}

float ComputeLuminance(float4 color , int luminanceType)
{
	float luminance = 0;
		
	// Determine the brightness of this particular pixel. As with the luminance calculations
	// there are 4 possible variations on this calculation:    
    // 1. Do a very simple mathematical average:
		if(luminanceType == 0)
			luminance = dot( color.rgb, float3( 0.33f, 0.33f, 0.33f ) );
    
    // 2. Perform a more accurately weighted average:		
		else if(luminanceType == 1)
			luminance = dot( color.rgb, float3( 0.299f, 0.587f, 0.114f ) );
    
    // 3. Take the maximum value of the incoming, same as computing the
    //    brightness/value for an HSV/HSB conversion:
		else if(luminanceType == 2)
			luminance = max( color.r, max( color.g, color.b ) );
    
    // 4. Compute the luminance component as per the HSL colour space:
		else if(luminanceType == 3)
			luminance = 0.5f * ( max( color.r, max( color.g, color.b ) ) + min( color.r, min( color.g, color.b ) ) );
    
    // 5. Use the magnitude of the colour
		else if(luminanceType == 4)
			luminance = length( color.rgb );

	return  luminance;              
}

//-----------------------------------------------------------------------------
// RGBE8 Encoding/Decoding
// The RGBE8 format stores a mantissa per color channel and a shared exponent 
// stored in alpha. Since the exponent is shared, it's computed based on the
// highest intensity color component. The resulting color is RGB * 2^Alpha,
// which scales the data across a logarithmic scale.
//-----------------------------------------------------------------------------
float4 EncodeRGBE8( in float3 rgb )
{
	float4 vEncoded;

    // Determine the largest color component
	float maxComponent = max( max(rgb.r, rgb.g), rgb.b );
	
	// Round to the nearest integer exponent
	float fExp = ceil( log2(maxComponent) );

    // Divide the components by the shared exponent
	vEncoded.rgb = rgb / exp2(fExp);
	
	// Store the shared exponent in the alpha channel
	vEncoded.a = (fExp + 128) / 255;

	return vEncoded;
}

float3 DecodeRGBE8( in float4 rgbe )
{
	float3 vDecoded;

    // Retrieve the shared exponent
	float fExp = rgbe.a * 255 - 128;
	
	// Multiply through the color components
	vDecoded = rgbe.rgb * exp2(fExp);
	
	return vDecoded;
}

//-----------------------------------------------------------------------------
// RE8 Encoding/Decoding
// The RE8 encoding is simply a single channel version of RGBE8, useful for
// storing non-color floating point data (such as calculated scene luminance)
//-----------------------------------------------------------------------------
float4 EncodeRE8( in float f )
{
    float4 vEncoded = float4( 0, 0, 0, 0 );
    
    // Round to the nearest integer exponent
    float fExp = ceil( log2(f) );
    
    // Divide by the exponent
    vEncoded.r = f / exp2(fExp);
    
    // Store the exponent
    vEncoded.a = (fExp + 128) / 255;
    
    return vEncoded;
}

float DecodeRE8( in float4 rgbe )
{
    float fDecoded;

    // Retrieve the shared exponent
	float fExp = rgbe.a * 255 - 128;
	
	// Multiply through the color components
	fDecoded = rgbe.r * exp2(fExp);

	return fDecoded;  
}


//-----------------------------------------------------------------------------
// RGB16 Encoding/Decoding
// The RGB16 format stores a 16-bit (0 to 65535) value scaled linearly between
// 0.0f and RGB16_MAX (an arbitrary maximum value above which data is clipped).
//-----------------------------------------------------------------------------
float4 EncodeRGB16( in float3 rgb )
{
	float4 vEncoded = 0;

    vEncoded.rgb = rgb / RGB16_MAX;
    
	return vEncoded;
}

float3 DecodeRGB16( in float4 rgbx )
{
	float3 vDecoded;

    vDecoded = rgbx.rgb * RGB16_MAX;
    
	return vDecoded;
}



//-----------------------------------------------------------------------------
// R16 Encoding/Decoding
// The R16 encoding is simply a single channel version of RGB16, useful for
// storing non-color floating point data (such as calculated scene luminance)
//-----------------------------------------------------------------------------
float4 EncodeR16( in float f )
{
    float4 vEncoded = 0;
    
    vEncoded.r = f / RGB16_MAX;
    
    return vEncoded;
}

float DecodeR16( in float4 rgbx )
{
    float fDecoded;

    fDecoded = rgbx.r * RGB16_MAX;
    
	return fDecoded;  
}


