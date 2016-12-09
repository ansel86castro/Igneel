
float4x4 World		: WORLD;
float4x4 ViewProj	: VIEWPROJ;
float4x4 View		: VIEW;
float4x4 Proj		: PROJ;
float4 Color 		: COLOR;
float distance;

void VS(float4 position:POSITION , float4 color:COLOR0 , out float4 positionH :POSITION, out float4 colorOut :COLOR0) 
{
	float4 positionW = mul(position,World);
	positionH= mul(positionW, ViewProj);
	colorOut = color;
}
float4 PS(float4 color:COLOR0) : COLOR0
{
	return color;
}


void VS_fixedColor(float4 position:POSITION ,out float4 positionH :POSITION) 
{
	float4 positionW = mul(position,World);
	positionH= mul(positionW, ViewProj);
}

float4 PS_fixedColor() : COLOR0
{
	return Color;
}

void VS_fixedDistance(float4 position:POSITION , out float4 positionH :POSITION) 
{
	float4 positionW = mul(position,World);
	float4 positionV = mul(positionW, View);
	positionV.z = distance;
	positionH= mul(positionV, Proj);	
}


// technique Technique1
// {
	// pass Pass1
	// {
	
		// //Alpha blend Allow Trasnparency depending of the source color alpha
		// AlphaBlendEnable = TRUE;
        // SrcBlend         = SRCALPHA;
        // DestBlend        = INVSRCALPHA;
		// BlendOp          = ADD;
		
		// VertexShader = compile vs_1_1 VS();
        // PixelShader = compile ps_2_0 PS();
	// }
// }

// technique Technique2
// {
	// pass Pass1
	// {
		// //Alpha blend Allow Trasnparency depending of the source color alpha
		// AlphaBlendEnable = TRUE;
        // SrcBlend         = SRCALPHA;
        // DestBlend        = INVSRCALPHA;
		// BlendOp          = ADD;
		
		// VertexShader = compile vs_1_1 VS_fixedColor();
        // PixelShader = compile ps_2_0 PS_fixedColor();
	// }
// }

// technique Technique3
// {
	// pass Pass1
	// {
		// //Alpha blend Allow Trasnparency depending of the source color alpha
		// AlphaBlendEnable = TRUE;
        // SrcBlend         = SRCALPHA;
        // DestBlend        = INVSRCALPHA;
		// BlendOp          = ADD;
				
        // PixelShader = compile ps_2_0 PS_fixedColor();
	// }
// }

// technique Technique4
// {
	// pass Pass1
	// {
	
		// //Alpha blend Allow Trasnparency depending of the source color alpha
		// AlphaBlendEnable = TRUE;
        // SrcBlend         = SRCALPHA;
        // DestBlend        = INVSRCALPHA;
		// BlendOp          = ADD;
		
		// VertexShader = compile vs_1_1 VS_fixedDistance();
        // PixelShader = compile ps_2_0 PS_fixedColor();
	// }
// }