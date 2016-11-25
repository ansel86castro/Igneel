
#define FILTERING

static const float4 temp = float4(1, 1, 1, 1)/9;

float filterPCF4(sampler2D shadowSampler, float2 projCoor, float dist, float step)
{
	// sample the texture offsets
	// 0 1
	// 2 3	
	float4 samples;
    samples.r = tex2D(shadowSampler, projCoor).r;				
	samples.g = tex2D(shadowSampler, projCoor + float2(step , 0)).r; 
	samples.b = tex2D(shadowSampler, projCoor + float2(0 , step)).r; 
	samples.a = tex2D(shadowSampler, projCoor + float2(step , step)).r;
	
	// float4 inLight = (samples > float4(dist,dist, dist,dist));
	return dot((samples > float4(dist,dist, dist,dist)), float4(1, 1, 1, 1) );		
}
float filterPCF4(sampler2D shadowSampler, float2 projCoor, float dist, float step , float4 weights)
{
	// sample the texture offsets
	// 0 1
	// 2 3	
	float4 samples;
    samples.r = tex2D(shadowSampler, projCoor).r;				
	samples.g = tex2D(shadowSampler, projCoor + float2(step , 0)).r; 
	samples.b = tex2D(shadowSampler, projCoor + float2(0 , step)).r; 
	samples.a = tex2D(shadowSampler, projCoor + float2(step , step)).r;
	
	// float4 inLight = (samples > float4(dist,dist, dist,dist));
	return dot((samples > float4(dist,dist, dist,dist)), weights );		
}

float filterPCF9(sampler2D shadowSampler, float2 projCoor, float dist, float step)
{	
	// sample the texture offsets
	// 0 1 4
	// 2 3 5
	// 6 7 8
	float4 samples;
    samples.r = tex2D(shadowSampler, projCoor).r;				
	samples.g = tex2D(shadowSampler, projCoor + float2(step , 0)).r; 
	samples.b = tex2D(shadowSampler, projCoor + float2(0 , step)).r; 
	samples.a = tex2D(shadowSampler, projCoor + float2(step , step)).r;
	
	float4 samples2;
    samples2.r = tex2D(shadowSampler, projCoor + float2(0, 2.0 * step )).r;				
	samples2.g = tex2D(shadowSampler, projCoor + float2(step , 2.0 * step)).r; 
	samples2.b = tex2D(shadowSampler, projCoor + float2(2.0 * step , 2.0 * step)).r; 
	samples2.a = tex2D(shadowSampler, projCoor + float2(2.0 * step , step)).r;

	return dot( samples > float4(dist,dist, dist,dist) ,temp ) + 
		   dot( samples2 > float4(dist,dist, dist,dist) ,temp ) +
			  (tex2D(shadowSampler, projCoor + float2(0 , 2.0 * step)).r > dist) / 9.0;			
}

float filterPCF16(sampler2D shadowSampler, float2 projCoor, float dist, float step)
{
	float bloqStep = 2.0 * step;
	float4 samples;
	samples.r = filterPCF4(shadowSampler, projCoor, dist, step);
	samples.g = filterPCF4(shadowSampler, projCoor + float2(bloqStep, 0), dist, step);
	samples.b = filterPCF4(shadowSampler, projCoor + float2(0, bloqStep), dist, step);
	samples.a = filterPCF4(shadowSampler, projCoor + float2(bloqStep, bloqStep), dist, step);
	
	return dot(samples, float4(1,1, 1, 1) );
}

float filterPCF16(sampler2D shadowSampler, float2 projCoor, float dist, float step, float4 weights[4])
{
	float bloqStep = 2.0 * step;
	float4 samples;		
	samples.r = filterPCF4(shadowSampler, projCoor, dist, step, weights[0]);
	samples.g = filterPCF4(shadowSampler, projCoor + float2(bloqStep, 0), dist, step , weights[1]);
	samples.b = filterPCF4(shadowSampler, projCoor + float2(0, bloqStep), dist, step , weights[2] );
	samples.a = filterPCF4(shadowSampler, projCoor + float2(bloqStep, bloqStep), dist, step, weights[3]);
	
	return dot(samples, float4(1,1,1,1) );
}

float filterPCF36(sampler2D shadowSampler, float2 projCoor, float dist, float step)
{
	float bloqStep = 3.0 * step;
	float4 samples;
	samples.r = filterPCF9(shadowSampler, projCoor, dist, step);
	samples.g = filterPCF9(shadowSampler, projCoor + float2(bloqStep, 0), dist, step);
	samples.b = filterPCF9(shadowSampler, projCoor + float2(0, bloqStep), dist, step);
	samples.a = filterPCF9(shadowSampler, projCoor + float2(bloqStep, bloqStep), dist, step);
	
	return dot(samples, float4(0.25f, 0.25f, 0.25f, 0.25f) );
}

float pcf4(float4 posL ,sampler2D shadowSampler, float epsilon, float step)
{	
	posL.xyz /= posL.w;	
	float dist = posL.z - epsilon;	
	float2 projCoor = float2(posL.x * 0.5f + 0.5f, -posL.y * 0.5f + 0.5f);					
	
	return filterPCF4(shadowSampler, projCoor, dist, step) / 4.0;
}

float pcf16(float4 posL , sampler2D shadowSampler, float epsilon, float step)
{
	posL.xyz /= posL.w;	
	float dist = posL.z - epsilon;	
	float2 projCoor = float2(posL.x * 0.5f + 0.5f, -posL.y * 0.5f + 0.5f);	
	
	return filterPCF16(shadowSampler, projCoor, dist, step) / 16.0;
}

float pcf16Mod(float4 posL , sampler2D shadowSampler, float epsilon, float step, float bloqsize)
{
	posL.xyz /= posL.w;	
	float dist = posL.z - epsilon;	
	float2 projCoor = float2(posL.x * 0.5f + 0.5f, -posL.y * 0.5f + 0.5f);	
	
	float filter1 = filterPCF16(shadowSampler, projCoor, dist, step) / 16.0;
	float filter2 = filterPCF16(shadowSampler, projCoor, dist, step / bloqsize) / 16.0;
	
	return filter1 * filter2;
}

float pcf36(float4 posL , sampler2D shadowSampler, float epsilon, float step)
{
	posL.xyz /= posL.w;	
	float dist = posL.z - epsilon;	
	float2 projCoor = float2(posL.x * 0.5f + 0.5f, -posL.y * 0.5f + 0.5f);	
	
	return filterPCF36(shadowSampler, projCoor, dist, step);
}

struct REC
{
	float2 top;
	float2 bottom;	
};

REC rectangle(float2 top ,float width = 1, float height = 1)
{
	REC rec;
	rec.top = top;
	rec.bottom = top + float2(width ,height);
	return rec;
}

float computeArea(REC rec)
{
	return (rec.bottom.x - rec.top.x)  *  (rec.bottom.y - rec.top.y);
}

//rec1 mask 
//rec2 texel
float ComputeCoverage(REC rec1 ,REC rec2)
{
	float2 size = float2(rec2.bottom.x - rec2.top.x, 
						 rec2.bottom.y - rec2.top.y);
	
	//case 1 	
	if(rec1.top.x >= rec2.top.x)
		size.x = rec2.bottom.x - rec1.top.x;
	//case 2
	else if(rec2.bottom.x >= rec1.bottom.x)	
		size.x = rec1.bottom.x - rec2.top.x;
		
	//case 1 	
	if(rec1.top.y >= rec2.top.y)
		size.y = rec2.bottom.y - rec1.top.y;
	
	//case 2	
	else if(rec2.bottom.y >= rec1.bottom.y)	
		size.y = rec1.bottom.y - rec2.top.y;
		
	return size.x / size.y;	
}

float4 ComputeCoverage(REC mask, float2 texelCoord)
{	
	float4 weights;		
	weights.x = ComputeCoverage( mask , rectangle(texelCoord));	
	weights.y = ComputeCoverage( mask , rectangle(texelCoord + float2(1 , 0)));	
	weights.z = ComputeCoverage( mask , rectangle(texelCoord + float2(0 , 1)));
	weights.w = ComputeCoverage( mask , rectangle(texelCoord + float2(1 , 1)));
	return weights;
}

float filterSubPCF16(sampler2D shadowSampler, float2 projCoor, float dist, float step)
{
	float2 texelCoord = projCoor / step;
	REC mask = rectangle(texelCoord, 3 , 3);
	
	texelCoord = floor(texelCoord);	
	float4 weights[4];
	
	weights[0] = ComputeCoverage(mask , texelCoord);
	weights[1] = ComputeCoverage(mask , texelCoord + float2(2 , 0));
	weights[2] = ComputeCoverage(mask , texelCoord + float2(0 , 2));
	weights[3] = ComputeCoverage(mask , texelCoord + float2(2 , 2));
	
	return filterPCF16(shadowSampler, projCoor, dist, step , weights) / 9.0;
	
}

float sub_pcf16(float4 posL , sampler2D shadowSampler, float epsilon, float step)
{
	posL.xyz /= posL.w;	
	float dist = posL.z - epsilon;	
	float2 projCoor = float2(posL.x * 0.5f + 0.5f, -posL.y * 0.5f + 0.5f);	
	
	return filterSubPCF16(shadowSampler, projCoor, dist, step);
}







