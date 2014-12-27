using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel.Graphics;
using Igneel.Rendering.Effects;
using System.ComponentModel;
using Igneel.Design;

namespace Igneel.Rendering
{
    public interface HDRMap
    {
        int NbTextures { get; set; }       
        float MIDDLE_GRAY { get; set; }     
        float BRIGHT_PASS_THRESHOLD { get; set; }        
        float BLOOM_BLEND { get; set; }
        float STAR_BLEND { get; set; }
        float ElapsedTime { get; set; }
        bool EnableBlueShift { get; set; }
        Vector4[] SampleOffsets { get; set; }
        Vector4[] SampleWeights { get; set; }
        float GaussianScalar { get; set; }
    }

    /// <summary>
    /// This render a scene with HDR effects by performing by setting a texture of render targets
    /// and then call the previus scene`s render, after the scene have been render it apply 
    /// tone mapping and postprocesing effect as bloom
    /// </summary>
    /// 
    [TypeConverter(typeof(DesignTypeConverter))]   
    public class HDRTechnique : Technique<HDREffect>
    {
        HDRState state;
        const int TONEMAPTEXTURES = 6;
        const int BLOOMTEXTURES = 3;
        const int STARMAXLINES = 8;
        const int STARMAXPASSES = 2;
        const int BORDER_COLOR = 0x0000000;        
        RenderTexture2D rtHDRScene;     
        RenderTexture2D rtBrightPassFilter;     
        RenderTexture2D rtStartSource;
        RenderTexture2D[] rtToneMap = new RenderTexture2D[TONEMAPTEXTURES];
        RenderTexture2D[] rtStarLines = new RenderTexture2D[STARMAXLINES];
        RenderTexture2D[] rtStarPasses = new RenderTexture2D[STARMAXPASSES];
        RenderTexture2D[] rtBloom = new RenderTexture2D[3];
        RenderTexture2D rtStarFinal;
        RenderTexture2D rtCurrentAdaptedLuminance;
        RenderTexture2D rtLastAdaptedLuminance;
        Format hdrFormat = Format.R16G16B16A16_FLOAT;
        Format luminanceFormat = Format.R16G16_FLOAT;
        Vector4[] sampleOffsetsH = new Vector4[8];
        Vector4[] sampleOffsetsV = new Vector4[8];
        Vector4[] sampleWeights = new Vector4[16];

        Vector4[] sampleWeightsStar = new Vector4[16];
        Vector4[] sampleOffsetsStar = new Vector4[8];

        Sprite quadRender;

        Vector4[,] s_aaColor = new Vector4[3, 8];
        Vector4 s_colorWhite = new Vector4(0.63f, 0.63f, 0.63f, 0.0f);
        Vector4 white = new Vector4(1, 1, 1, 1);
        float time;
        float gaussainMultiplier;
        float gaussianDev;
        float gaussianMean;
        public HDRState State { get { return state; } }
        public Format HRDFormat { get { return hdrFormat; } }
        public Format LuminanceFormat { get { return luminanceFormat; } }
        public RenderTexture2D HDRScene { get { return rtHDRScene; } }
        public RenderTexture2D BrightPassFilter { get { return rtBrightPassFilter; } }
        public RenderTexture2D AvgLuminance { get { return rtToneMap[0]; } }
        public RenderTexture2D[] ToneMaps { get { return rtToneMap; } }
        public RenderTexture2D[] Bloom { get { return rtBloom; } }      
        public RenderTexture2D CurrentAdaptedLuminance { get { return rtCurrentAdaptedLuminance; } }
        public RenderTexture2D StarSource { get { return rtStartSource; } }
        public RenderTexture2D[] StarLines { get { return rtStarLines; } }
        public RenderTexture2D[] StarPasses { get { return rtStarPasses; } }
        public RenderTexture2D StarFinal { get { return rtStarFinal; } }

        SamplerState pointSampler;
        SamplerState linearSampler;     

        HDRMap map;
        Sprite.IShaderInput input;

        public HDRTechnique()
        {           
            Service.Set<HDRTechnique>(this);
            this.state = Engine.Lighting.HDR;

            var device = Engine.Graphics;
            Format[] formats = { Format.R16G16B16A16_FLOAT, Format.R16G16B16A16_UNORM, Format.R8G8B8A8_TYPELESS };
            Format[] lumFormat = { Format.R16_FLOAT, Format.R16_UNORM, Format.R8G8B8A8_TYPELESS };            
            for (int i = 0; i < formats.Length; i++)
			{
			    if(device.CheckFormatSupport(formats[i], BindFlags.RenderTarget, ResourceType.Texture2D))
                {
                    hdrFormat = formats[i];
                    break;
                }
			}
           

           CreateResources();
           ComputeSamples();

           device.BackBuffer.Resized += OMBackBuffer_Resized;

            pointSampler = Engine.Graphics.CreateSamplerState(new SamplerDesc(true) 
            {   AddressU = TextureAddressMode.Border,
                AddressV = TextureAddressMode.Border,
                Filter = Filter.MinMagMipPoint , 
                BorderColor = new Color4(0, 0, 0, 0) });
            linearSampler = Engine.Graphics.CreateSamplerState(new SamplerDesc(true)
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp, 
                Filter = Filter.MinMagMipLinear, 
                BorderColor = new Color4(0)
            });
            quadRender = Service.Require<Sprite>();         
           
            map = effect.Map<HDRMap>(true);
            input = effect.Map<Sprite.IShaderInput>(true);

           
        }

        void OMBackBuffer_Resized(RenderTarget obj)
        {
            DisposeResources();
            CreateResources();
            ComputeSamples();
        }

        private void DisposeResources()
        {            
            rtHDRScene.Dispose();
            rtBrightPassFilter.Dispose();
            rtStartSource.Dispose();
            rtCurrentAdaptedLuminance.Dispose();
            rtLastAdaptedLuminance.Dispose();
            rtStarFinal.Dispose();

            int i;

            for (i = 0; i < BLOOMTEXTURES; i++)
                rtBloom[i].Dispose();

            for (i = 0; i < TONEMAPTEXTURES; i++)
                rtToneMap[i].Dispose();        

            for (i = 0; i < STARMAXLINES; i++)
                rtStarLines[i].Dispose();

            for (i = 0; i < STARMAXPASSES; i++)
                rtStarPasses[i].Dispose();
        }

        private int CreateResources()
        {
            GraphicDevice device = Engine.Graphics;
            var backBuffer = device.BackBuffer;
            int width = backBuffer.Width;
            int height = backBuffer.Height;

            Format depthFormat = Format.UNKNOWN;

            rtHDRScene = new RenderTexture2D(width, height, hdrFormat, device.BackDepthBuffer.SurfaceFormat, backBuffer.Sampling);
            rtBrightPassFilter = new RenderTexture2D(width / 2, height / 2, Format.R8G8B8A8_UNORM, depthFormat);
            rtStartSource = new RenderTexture2D(width / 4, height / 4, Format.R8G8B8A8_UNORM, depthFormat);
            rtCurrentAdaptedLuminance = new RenderTexture2D(1, 1, luminanceFormat, depthFormat);
            rtLastAdaptedLuminance = new RenderTexture2D(1, 1, luminanceFormat, depthFormat);
            rtStarFinal = new RenderTexture2D(rtStartSource.Width, rtStartSource.Height, Format.R8G8B8A8_UNORM, depthFormat);

            int i;
            for (i = 0; i < BLOOMTEXTURES; i++)
                rtBloom[i] = new RenderTexture2D(width / 8, height / 8, Format.R8G8B8A8_UNORM);

            for (i = 0; i < STARMAXLINES; i++)
                rtStarLines[i] = new RenderTexture2D(rtStartSource.Width, rtStartSource.Height, hdrFormat);

            for (i = 0; i < STARMAXPASSES; i++)
                rtStarPasses[i] = new RenderTexture2D(rtStartSource.Width, rtStartSource.Height, hdrFormat);

            int size = 1;
            for (i = 0; i < TONEMAPTEXTURES; i++)
            {
                rtToneMap[i] = new RenderTexture2D(size, size, luminanceFormat, depthFormat);
                size *= 3;
            }
            return i;
        }

        public override void Apply()
        {
            if (gaussianMean != state.GaussainMean || 
                gaussianDev != state.GaussianDeviation || 
                gaussainMultiplier != state.GaussianMultiplier)
                ComputeSamples();

            var device = Engine.Graphics;
            device.SaveRenderTarget();

            RenderTexture2D dest;
            RenderTexture2D source;
            RenderTexture2D lumen;
            var stage = device.PS;
            var scene = Engine.Scene;

            #region Render To HDRTexture           

            rtHDRScene.SetTarget(device);
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Engine.BackColor, 1, 0);

            Engine.PopTechnique();
            Engine.ApplyTechnique();
            Engine.PushTechnique(this);

            #endregion

            quadRender.Begin();
            quadRender.SetFullScreenTransform(input);
           
            stage.SetSampler(0, pointSampler);
            stage.SetSampler(1, pointSampler);

            MensureLuminance(device, stage);

            #region CalculateAdaptation

            if (state.CalculateEyeAdaptation)
            {
                effect.Technique = HDREffect.CalculateAdaptedLum;

                //swap last width current
                var rtTemp = rtLastAdaptedLuminance;
                rtLastAdaptedLuminance = rtCurrentAdaptedLuminance;
                rtCurrentAdaptedLuminance = rtTemp;

                rtCurrentAdaptedLuminance.SetTarget(device);

                rtLastAdaptedLuminance.SetTexture(0, device);
                rtToneMap[0].SetTexture(1, device);

                map.ElapsedTime = Engine.ElapsedTime;
                quadRender.DrawQuad(effect);

                lumen = rtCurrentAdaptedLuminance;
            }
            else
                lumen = rtToneMap[0];
            #endregion          

            #region BrightPassFilter

            effect.Technique = HDREffect.BrightPassFilter;
            rtBrightPassFilter.SetTarget(device);

            rtHDRScene.SetTexture(0, device);
            lumen.SetTexture(1, device);

            map.MIDDLE_GRAY = state.MiddleGray;
            map.BRIGHT_PASS_THRESHOLD = state.BrightThreshold; 
       
            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            quadRender.DrawQuad(effect);

            #endregion

            stage.SetSampler(0, linearSampler);

            #region BloomFilter

            #region DownSample 4x4

            effect.Technique = HDREffect.DownSampler4x4;
            dest = rtBloom[2];
            source = rtBrightPassFilter;

            dest.SetTarget(device);
            source.SetTexture(0, device);
            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);

            quadRender.DrawQuad(effect);

            #endregion

            #region Horizontal Blur

            effect.Technique = HDREffect.Bloom;
            //Horizontal Blur
            source = rtBloom[2];
            dest = rtBloom[1];

            dest.SetTarget(device);
            source.SetTexture(0, device);
            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);

            map.SampleOffsets = sampleOffsetsH;
            map.SampleWeights = sampleWeights;

            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            quadRender.DrawQuad(effect);

            #endregion

            #region Vertical Blur
            source = rtBloom[1];
            dest = rtBloom[0];

            dest.SetTarget(device);
            source.SetTexture(0, device);

            map.SampleOffsets = sampleOffsetsV;
            map.SampleWeights = sampleWeights;

            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            quadRender.DrawQuad(effect);

            #endregion

            #endregion

            #region StarFilter

            RenderStar(device);

            #endregion

            #region FinalPass

            effect.Technique = HDREffect.FinalScenePass;

            device.RestoreRenderTarget();

            stage.SetSampler(0, pointSampler);
            stage.SetSampler(1, pointSampler);
            stage.SetSampler(2, linearSampler);
            stage.SetSampler(3, linearSampler);

            rtHDRScene.SetTexture(0, device);
            lumen.SetTexture(1, device);
            rtBloom[0].SetTexture(2, device);
            rtStarFinal.SetTexture(3, device);

            map.MIDDLE_GRAY = state.MiddleGray;

            var glare = state.Glare;
            if (glare == null || glare.glareLuminance <= 0.0f || glare.starLuminance <= 0.0f)
            {
                map.GaussianScalar = state.GaussianMultiplier;
            }
            else
                map.GaussianScalar = 2 * state.GaussianMultiplier;
            
            map.EnableBlueShift = state.EnableBlueShift;
            map.STAR_BLEND = state.StarBlendFactor;
            map.BLOOM_BLEND = state.BloomBlendFactor;

            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            quadRender.DrawQuad(effect);

            stage.SetResource(0, null);
            stage.SetResource(1, null);
            stage.SetResource(2, null);
            stage.SetResource(3, null);

            #endregion

            quadRender.End();
            //device.PopGraphicState<BlendState>();

        }

        private void MensureLuminance(GraphicDevice device, IShaderStage stage)
        {         
            effect.Technique = HDREffect.SampleAvgLum;

            RenderTexture2D dest = rtToneMap[TONEMAPTEXTURES - 1];
            RenderTexture2D source = rtHDRScene;

            dest.SetTarget(device);
            source.SetTexture(0, device);
         
            stage.SetSampler(0, linearSampler);

            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            quadRender.DrawQuad(effect);

            effect.Technique = HDREffect.ResampleAvgLum;
            stage.SetSampler(0, pointSampler);

            for (int i = TONEMAPTEXTURES - 2; i > 0; i--)
            {
                dest = rtToneMap[i];
                source = rtToneMap[i + 1];

                dest.SetTarget(device);
                source.SetTexture(0, device);
                device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
                quadRender.DrawQuad(effect);
            }

            effect.Technique = HDREffect.ResampleAvgLumExp;
            source = rtToneMap[1];
            dest = rtToneMap[0];

            dest.SetTarget(device);
            source.SetTexture(0, device);

            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            quadRender.DrawQuad(effect);
        }  
     
        private unsafe void RenderStar(GraphicDevice device)
        {
            RenderTexture2D source;
            RenderTexture2D dest;

            var glare = state.Glare;
            //renderMgr.Fill(rtStarFinal, 0);            
            // Avoid rendering the star if it's not being used in the current glare
            if (glare == null || glare.glareLuminance <= 0.0f || glare.starLuminance <= 0.0f)
            {
                rtStarFinal.SetTarget(device);
                device.Clear(ClearFlags.Target, Color4.Black, 1, 0);

                return;
            }

            fixed (Vector4* _ptSampleOffsets = sampleOffsetsStar)
            {
                Vector2* ptSampleOffsets = (Vector2*)_ptSampleOffsets;
                fixed (Vector4* ptSampleWeights = sampleWeightsStar)
                {                  
                    //#region StarSourceToBloomSource (DownSampler2x2)

                    //effect.Technique = HDREffect.DownSampler2x2;
                    //dest = rtBrightPassFilter;
                    //source = rtStartSource;

                    //dest.SetTarget(device);
                    //source.SetTexture(0, device);
                    //device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
                    //quadRender.DrawQuad(effect);            
                    //#endregion

                    #region BrighPassToStarSource

                    effect.Technique = HDREffect.GaussBlur5x5;
                    source = rtBrightPassFilter;
                    dest = rtStartSource;                    

                    dest.SetTarget(device);
                    source.SetTexture(0, device);

                    FillSampleOffsets_GaussBlur5x5(source.Width, source.Height, ptSampleOffsets, ptSampleWeights, 1);

                    map.SampleOffsets = sampleOffsetsStar;
                    map.SampleWeights = sampleWeightsStar;
                  
                    quadRender.DrawQuad(effect);

                    #endregion

                    //float fTanFoV = (float)Math.Atan(Numerics.PI / 8.0f);
                    const float fTanFoV = 0.37419668052268495F;
                    const int maxPasses = 3;
                    const int samples = 8;
                    int i; // Loop variables
                    StarDefinition starDef = glare.starDef;

                    device.PS.SetSampler(0, linearSampler);

                    float srcW = rtStartSource.Width;
                    float srcH = rtStartSource.Height;


                    for (int p = 0; p < maxPasses; p++)
                    {
                        float ratio = (float)(p + 1) / (float)maxPasses;
                        for (int s = 0; s < samples; s++)
                        {
                            Vector4 chromaticAberrColor = Numerics.Lerp(StarDefinition.chromaticAberrationColor[s], s_colorWhite, ratio);
                            s_aaColor[p, s] = Numerics.Lerp(s_colorWhite, chromaticAberrColor, glare.chromaticAberration);
                        }
                    }

                    time += starDef.rotation ? Engine.ElapsedTime : 0;
                    float radOffset = glare.starInclination + starDef.inclination + time;
                  
                    Vector2 stepUV;
                    int lines = starDef.starLines.Length;

                    float attnPowScaleInicial = (fTanFoV + 0.1f) * 1.0f * (160.0f + 120.0f) / (srcW + srcH) * 1.2f;

                    for (int d = 0; d < lines; d++)
                    {
                        STARLINE starLine = starDef.starLines[d];
                        float rad = radOffset + starLine.Inclination;
                        float sn = (float)Math.Sin(rad);
                        float cs = (float)Math.Cos(rad);
                        stepUV.X = sn / srcW * starLine.SampleLength;
                        stepUV.Y = cs / srcH * starLine.SampleLength;

                        float attnPowScale = attnPowScaleInicial;

                        #region PASSES
                        // 1 direction expansion loop
                        source = rtStartSource;
                        dest = rtStarPasses[0];
                        for (int p = 0; p < starLine.Passes; p++)
                        {
                            if (p == starLine.Passes - 1)
                            {
                                source = dest;
                                dest = rtStarLines[d];
                            }
                            // Sampling configration for each stage
                            for (i = 0; i < samples; i++)
                            {
                                float lum = (float)Math.Pow(starLine.Attenuation, attnPowScale * i);

                                ptSampleWeights[i] = s_aaColor[starLine.Passes - 1 - p, i] * lum * (p + 1.0f) * 0.5f;

                                // Offset of sampling coordinate
                                ptSampleOffsets[i].X = stepUV.X * i;
                                ptSampleOffsets[i].Y = stepUV.Y * i;
                                if (Math.Abs(ptSampleOffsets[i].X) >= 0.9f || Math.Abs(ptSampleOffsets[i].Y) >= 0.9f)
                                {
                                    ptSampleOffsets[i] = Vector2.Zero;
                                    ptSampleWeights[i] = Vector4.Zero;
                                }
                            }

                            effect.Technique = HDREffect.Star;
                            dest.SetTarget(device);
                            source.SetTexture(0, device);

                            map.SampleOffsets = sampleOffsetsStar;
                            map.SampleWeights = sampleWeightsStar;

                            quadRender.DrawQuad(effect);

                            // Setup next expansion
                            stepUV *= samples;
                            attnPowScale *= samples;

                            // Set the work drawn just before to next texture source.
                            if (p == 0)
                            {
                                source = dest;
                                dest = rtStarPasses[1];
                            }
                            else if (p != starLine.Passes - 1)
                            {
                                var rtTemp = source;
                                source = dest;
                                dest = source;
                            }
                        }

                        #endregion PASSES

                    }

                    dest = rtStarFinal;
                    dest.SetTarget(device);
                  
                    float invLines = 1.0f / (float)lines;
                    for (i = 0; i < lines; i++)
                    {
                        rtStarLines[i].SetTexture(i, device);
                        device.PS.SetSampler(i, linearSampler);
                        ptSampleWeights[i] = new Vector4(invLines, invLines, invLines, invLines);
                    }

                    effect.Technique = HDREffect.MergeTexture;
                    map.NbTextures = lines;
                    map.SampleWeights = sampleWeightsStar;
                    
                    quadRender.DrawQuad(effect);

                    //for (i = 0; i < lines; i++)
                    //{
                    //    device.PSStage.SetSampler(i, pointSampler);
                    //}
                   
                }
            }
        }    
        
        private unsafe void FillDownSampler2x2(int srcWidth, int srcHeight, Vector2* pter)
        {
            float tU = 1.0f / srcWidth;
            float tV = 1.0f / srcHeight;

            // Sample from the 4 surrounding points. Since the center point will be in
            // the exact center of 4 texels, a 0.5f offset is needed to specify a texel
            // center.
            int index = 0;

            for (float y = 0; y < 2; y++)
            {
                for (float x = 0; x < 2; x++)
                {
                    //pter[index].X = (x - 0.5f) * tU;
                    //pter[index].Y = (y - 0.5f) * tV;
                    pter[index].X = x *  tU;
                    pter[index].Y = y * tV;

                    index++;
                }
            }
        }
        
        private unsafe void FillDownSampler3x3(int srcWidth, int srcHeight, Vector2* pter)
        {
            float tU = 1.0f / srcWidth;
            float tV = 1.0f / srcHeight;
            // Sample from the 9 surrounding points. 
            int index = 0;
            for (float y = -1; y <= 1; y++)
            {
                for (float x = -1; x <= 1; x++)
                {
                    pter[index].X = x * tU;
                    pter[index].Y = y * tV;

                    index++;
                }
            }
        }
        
        private unsafe void FillDownSampler4x4(int srcWidth, int srcHeight, Vector2* pter , bool shift = true)
        {
            int idx = 0;
            float tU = 1.0f / (float)srcWidth;
            float tV = 1.0f / (float)srcHeight;
            for (float y = 0; y < 4; y++)
            {
                for (float x = 0; x < 4; x++)
                {
                    if (shift)
                    {
                        pter[idx].X = (x - 1.0f) * tU;
                        pter[idx].Y = (y - 1.0f) * tV;
                    }
                    else
                    {
                        pter[idx].X = x * tU;
                        pter[idx].Y = y * tV;
                    }

                    idx++;
                }
            }
        }

        private unsafe void FillSampleOffsets_Bloom(int texSize, Vector4* ptSampleWeights, float* afTexCoordOffset, float deviation, float multiplier)
        {

            int i = 0;
            float tu = 1.0f / (float)texSize;

            // Fill the center texel
            float weight = multiplier * Numerics.GaussianDistribution(0, 0, deviation);
            ptSampleWeights[0] = new Vector4(weight, weight, weight, 1.0f);

            afTexCoordOffset[0] = 0.0f;

            // Fill the first half
            for (i = 1; i < 8; i++)
            {
                // Get the Gaussian intensity for this offset
                weight = multiplier * Numerics.GaussianDistribution((float)i, 0, deviation);
                afTexCoordOffset[i] = i * tu;

                ptSampleWeights[i] = new Vector4(weight, weight, weight, 1.0f);
            }

            // Mirror to the second half
            for (i = 8; i < 15; i++)
            {
                ptSampleWeights[i] = ptSampleWeights[i - 7];
                afTexCoordOffset[i] = -afTexCoordOffset[i - 7];
            }

            //int i = 0;
            //float tu = 1.0f / (float)texSize;

            //// Fill the center texel
            //float weight = 1.0f * Numerics.GaussianDistribution(0, mean, deviation);
            //ptsampleWeights[0] = new Vector4(weight, weight, weight, 1.0f);

            //ptafTexCoordOffset[0] = 0.0f;

            //    // Fill the right side
            //    for (i = 1; i < 8; i++)
            //    {
            //        weight = multiplier * Numerics.GaussianDistribution((float)i, mean, deviation);                   
            //        ptafTexCoordOffset[i] = i * tu;

            //        ptsampleWeights[i] = new Vector4(weight, weight, weight, 1.0f);
            //    }

            //    // Copy to the left side
            //    for (i = 8; i < 15; i++)
            //    {
            //        ptsampleWeights[i] = ptsampleWeights[i - 7];
            //        ptafTexCoordOffset[i] = -ptafTexCoordOffset[i - 7];
            //    }

        }
        
        private unsafe void FillSampleOffsets_GaussBlur5x5(int srcWidth, int srcHeight,
                                                            Vector2* ptTexCoordOffset,
                                                            Vector4* ptSampleWeight,
                                                            float fMultiplier)
        {
            float tu = 1.0f / (float)srcWidth;
            float tv = 1.0f / (float)srcHeight;

            Vector4 vWhite = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

            float totalWeight = 0.0f;
            int index = 0;
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    // Exclude pixels with a block distance greater than 2. This will
                    // create a kernel which approximates a 5x5 kernel using only 13
                    // sample points instead of 25; this is necessary since 2.0 shaders
                    // only support 16 texture grabs.
                    if (Math.Abs(x) + Math.Abs(y) > 2)
                        continue;

                    // Get the unscaled Gaussian intensity for this offset
                    ptTexCoordOffset[index].X = x * tu;
                    ptTexCoordOffset[index].Y = y * tv;
                    ptSampleWeight[index] = vWhite * Numerics.GaussianDistribution((float)x, (float)y, 1.0f);
                    totalWeight += ptSampleWeight[index].X;

                    index++;
                }
            }

            // Divide the current weight by the total weight of all the samples; Gaussian
            // blur kernels add to 1.0f to ensure that the intensity of the image isn't
            // changed when the blur occurs. An optional multiplier variable is used to
            // add or remove image intensity during the blur.
            float invTotalWeight = 1 / totalWeight;
            for (int i = 0; i < index; i++)
            {
                ptSampleWeight[i] *= invTotalWeight;
                ptSampleWeight[i] *= fMultiplier;
            }
        }
     
             
        protected override void OnDispose(bool d)
        {
            if (d)
            {
                DisposeResources();
                pointSampler.Dispose();
                linearSampler.Dispose();
                Service.Remove<HDRTechnique>();

                Engine.Graphics.BackBuffer.Resized -= OMBackBuffer_Resized;
            }
            base.OnDispose(d);
        }

        public void ComputeSamples()
        {
            gaussainMultiplier = state.GaussianMultiplier;
            gaussianDev = state.GaussianDeviation;
            gaussianMean = state.GaussainMean;

            unsafe
            {
                fixed (Vector4* ptSamplesWeight = sampleWeights)
                {
                    float* ptOffset = stackalloc float[15];
                    FillSampleOffsets_Bloom(rtBloom[0].Width, ptSamplesWeight, ptOffset, state.GaussianDeviation, state.GaussianMultiplier);
                    fixed (Vector4* pter = sampleOffsetsH)
                    {
                        Vector2* pterOffset = (Vector2*)pter;
                        for (int i = 0; i < 15; i++)
                        {
                            pterOffset[i].Y = 0;
                            pterOffset[i].X = ptOffset[i];
                        }
                    }

                    FillSampleOffsets_Bloom(rtBloom[0].Height, ptSamplesWeight, ptOffset, state.GaussianDeviation, state.GaussianMultiplier);
                    fixed (Vector4* pter = sampleOffsetsV)
                    {
                        Vector2* pterOffset = (Vector2*)pter;
                        for (int i = 0; i < 15; i++)
                        {
                            pterOffset[i].Y = ptOffset[i];
                            pterOffset[i].X = 0;
                        }
                    }
                }
            }
        }
    }      

}
