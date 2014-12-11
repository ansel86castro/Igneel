#include "../Include/LigthingShadowed.fxh"

int DESCRIPTION
<
	string ShaderModel = "3.0";
	string Version = "1.0.0";
	int NumberOfLights = 0;				
>;

//************************* Simple Phong Per Pixel Lighting **************************************

void SimpleVertexShader(in float4 position : POSITION, 
						out float4 positionH :POSITION, 
						out float3 positionW:TEXCOORD0,
						out float4 positionL:TEXCOORD1)
{   
	gPositionH = position;    	
	
	TransformPNL();	
	
	positionH = gPositionH;
	positionW = gPositionW;	
	positionL = gPositionL;	    
}

float4 PhongPixelShader(float3 positionW:TEXCOORD0, float4 positionL:TEXCOORD1):COLOR0
{   			
	clip(dot(float4(positionW, 1), clipPlane));
	
	gPositionW = positionW;
	gPositionL  = positionL;
	
	ComputeShadowFactor();	
	
	return float4(gShadowFactor,gShadowFactor,gShadowFactor,1);
}

technique RenderShadowFactor
{
    pass Pass1
    {
        // TODO: set renderstates here.					
        VertexShader = compile vs_1_1 SimpleVertexShader();
        PixelShader = compile ps_3_0 PhongPixelShader();
    }
}