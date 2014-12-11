#define LUMINANCE

#define RGB16_MAX  100;
#define RGB16_EXP = 5;

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
