#include "HDRCommon.hlsli"

float4 main(PS_RenderQuadInput input) : SV_TARGET
{	
	float4 vSample1 = t0.Sample(s0, float2(0, 0));
	float4 vSample2 = t1.Sample(s1, float2(0, 0));
	float2 fAdaptedLum = vSample1.rg; 
	float2  fCurrentLum = vSample2.rg;
     
    // The user's adapted luminance level is simulated by closing the gap between
    // adapted luminance and current luminance by 2% every frame, based on a
    // 30 fps rate. This is not an accurate model of human adaptation, which can
    // take longer than half an hour.
    float2 fNewAdaptation = fAdaptedLum + (fCurrentLum - fAdaptedLum) * ( 1 - pow( 0.98f, 30 * ElapsedTime ) );
	
	
	return float4(fNewAdaptation, 0, 1.0f);
}