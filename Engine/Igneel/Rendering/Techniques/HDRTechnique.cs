using System;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.States;

namespace Igneel.Techniques
{
    public interface IHdrMap
    {
        int NbTextures { get; set; }       
        float MiddleGray { get; set; }     
        float BrightPassThreshold { get; set; }        
        float BloomBlend { get; set; }
        float StarBlend { get; set; }
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
      
    public class HdrTechnique : Technique<HdrEffect>
    {
        HdrState _state;
        const int Tonemaptextures = 6;
        const int Bloomtextures = 3;
        const int Starmaxlines = 8;
        const int Starmaxpasses = 2;
        const int BorderColor = 0x0000000;        
        RenderTexture2D _rtHdrScene;     
        RenderTexture2D _rtBrightPassFilter;     
        RenderTexture2D _rtStartSource;
        RenderTexture2D[] _rtToneMap = new RenderTexture2D[Tonemaptextures];
        RenderTexture2D[] _rtStarLines = new RenderTexture2D[Starmaxlines];
        RenderTexture2D[] _rtStarPasses = new RenderTexture2D[Starmaxpasses];
        RenderTexture2D[] _rtBloom = new RenderTexture2D[3];
        RenderTexture2D _rtStarFinal;
        RenderTexture2D _rtCurrentAdaptedLuminance;
        RenderTexture2D _rtLastAdaptedLuminance;
        Format _hdrFormat = Format.R16G16B16A16_FLOAT;
        Format _luminanceFormat = Format.R16G16_FLOAT;
        Vector4[] _sampleOffsetsH = new Vector4[8];
        Vector4[] _sampleOffsetsV = new Vector4[8];
        Vector4[] _sampleWeights = new Vector4[16];

        Vector4[] _sampleWeightsStar = new Vector4[16];
        Vector4[] _sampleOffsetsStar = new Vector4[8];

        Sprite _quadRender;

        Vector4[,] _sAaColor = new Vector4[3, 8];
        Vector4 _sColorWhite = new Vector4(0.63f, 0.63f, 0.63f, 0.0f);
        Vector4 _white = new Vector4(1, 1, 1, 1);
        float _time;
        float _gaussainMultiplier;
        float _gaussianDev;
        float _gaussianMean;
        public HdrState State { get { return _state; } }
        public Format HrdFormat { get { return _hdrFormat; } }
        public Format LuminanceFormat { get { return _luminanceFormat; } }
        public RenderTexture2D HdrScene { get { return _rtHdrScene; } }
        public RenderTexture2D BrightPassFilter { get { return _rtBrightPassFilter; } }
        public RenderTexture2D AvgLuminance { get { return _rtToneMap[0]; } }
        public RenderTexture2D[] ToneMaps { get { return _rtToneMap; } }
        public RenderTexture2D[] Bloom { get { return _rtBloom; } }      
        public RenderTexture2D CurrentAdaptedLuminance { get { return _rtCurrentAdaptedLuminance; } }
        public RenderTexture2D StarSource { get { return _rtStartSource; } }
        public RenderTexture2D[] StarLines { get { return _rtStarLines; } }
        public RenderTexture2D[] StarPasses { get { return _rtStarPasses; } }
        public RenderTexture2D StarFinal { get { return _rtStarFinal; } }

        SamplerState _pointSampler;
        SamplerState _linearSampler;     

        IHdrMap _map;
        Sprite.IShaderInput _input;

        public HdrTechnique():base(GraphicDeviceFactory.Device)
        {           
            Service.Set<HdrTechnique>(this);
            this._state = EngineState.Lighting.Hdr;

            var device = GraphicDeviceFactory.Device;
            Format[] formats = { Format.R16G16B16A16_FLOAT, Format.R16G16B16A16_UNORM, Format.R8G8B8A8_TYPELESS };
            Format[] lumFormat = { Format.R16_FLOAT, Format.R16_UNORM, Format.R8G8B8A8_TYPELESS };            
            for (int i = 0; i < formats.Length; i++)
			{
			    if(device.CheckFormatSupport(formats[i], BindFlags.RenderTarget, ResourceType.Texture2D))
                {
                    _hdrFormat = formats[i];
                    break;
                }
			}
           

           CreateResources();
           ComputeSamples();

           device.BackBuffer.Resized += OMBackBuffer_Resized;

            _pointSampler = GraphicDeviceFactory.Device.CreateSamplerState(new SamplerDesc(true) 
            {   AddressU = TextureAddressMode.Border,
                AddressV = TextureAddressMode.Border,
                Filter = Filter.MinMagMipPoint , 
                BorderColor = new Color4(0, 0, 0, 0) });
            _linearSampler = GraphicDeviceFactory.Device.CreateSamplerState(new SamplerDesc(true)
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp, 
                Filter = Filter.MinMagMipLinear, 
                BorderColor = new Color4(0)
            });
            _quadRender = Service.Require<Sprite>();         
           
            _map = effect.Map<IHdrMap>(true);
            _input = effect.Map<Sprite.IShaderInput>(true);

           
        }

        void OMBackBuffer_Resized(RenderTarget obj)
        {
            DisposeResources();
            CreateResources();
            ComputeSamples();
        }

        private void DisposeResources()
        {            
            _rtHdrScene.Dispose();
            _rtBrightPassFilter.Dispose();
            _rtStartSource.Dispose();
            _rtCurrentAdaptedLuminance.Dispose();
            _rtLastAdaptedLuminance.Dispose();
            _rtStarFinal.Dispose();

            int i;

            for (i = 0; i < Bloomtextures; i++)
                _rtBloom[i].Dispose();

            for (i = 0; i < Tonemaptextures; i++)
                _rtToneMap[i].Dispose();        

            for (i = 0; i < Starmaxlines; i++)
                _rtStarLines[i].Dispose();

            for (i = 0; i < Starmaxpasses; i++)
                _rtStarPasses[i].Dispose();
        }

        private int CreateResources()
        {
            GraphicDevice device = GraphicDeviceFactory.Device;
            var backBuffer = device.BackBuffer;
            int width = backBuffer.Width;
            int height = backBuffer.Height;

            Format depthFormat = Format.UNKNOWN;

            _rtHdrScene = new RenderTexture2D(width, height, _hdrFormat, device.BackDepthBuffer.SurfaceFormat, backBuffer.Sampling);
            _rtBrightPassFilter = new RenderTexture2D(width / 2, height / 2, Format.R8G8B8A8_UNORM, depthFormat);
            _rtStartSource = new RenderTexture2D(width / 4, height / 4, Format.R8G8B8A8_UNORM, depthFormat);
            _rtCurrentAdaptedLuminance = new RenderTexture2D(1, 1, _luminanceFormat, depthFormat);
            _rtLastAdaptedLuminance = new RenderTexture2D(1, 1, _luminanceFormat, depthFormat);
            _rtStarFinal = new RenderTexture2D(_rtStartSource.Width, _rtStartSource.Height, Format.R8G8B8A8_UNORM, depthFormat);

            int i;
            for (i = 0; i < Bloomtextures; i++)
                _rtBloom[i] = new RenderTexture2D(width / 8, height / 8, Format.R8G8B8A8_UNORM);

            for (i = 0; i < Starmaxlines; i++)
                _rtStarLines[i] = new RenderTexture2D(_rtStartSource.Width, _rtStartSource.Height, _hdrFormat);

            for (i = 0; i < Starmaxpasses; i++)
                _rtStarPasses[i] = new RenderTexture2D(_rtStartSource.Width, _rtStartSource.Height, _hdrFormat);

            int size = 1;
            for (i = 0; i < Tonemaptextures; i++)
            {
                _rtToneMap[i] = new RenderTexture2D(size, size, _luminanceFormat, depthFormat);
                size *= 3;
            }
            return i;
        }

        public override void Apply()
        {
            if (_gaussianMean != _state.GaussainMean || 
                _gaussianDev != _state.GaussianDeviation || 
                _gaussainMultiplier != _state.GaussianMultiplier)
                ComputeSamples();

            var device = GraphicDeviceFactory.Device;
            device.SaveRenderTarget();

            RenderTexture2D dest;
            RenderTexture2D source;
            RenderTexture2D lumen;
            var stage = device.PS;
            var scene = Engine.Scene;

            #region Render To HDRTexture           

            _rtHdrScene.SetTarget(device);
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Engine.BackColor, 1, 0);

            RenderManager.PopTechnique();
            RenderManager.ApplyTechnique();
            RenderManager.PushTechnique(this);

            #endregion

            _quadRender.Begin();
            _quadRender.SetFullScreenTransform(_input);
           
            stage.SetSampler(0, _pointSampler);
            stage.SetSampler(1, _pointSampler);

            MensureLuminance(device, stage);

            #region CalculateAdaptation

            if (_state.CalculateEyeAdaptation)
            {
                effect.Technique = HdrEffect.CalculateAdaptedLum;

                //swap last width current
                var rtTemp = _rtLastAdaptedLuminance;
                _rtLastAdaptedLuminance = _rtCurrentAdaptedLuminance;
                _rtCurrentAdaptedLuminance = rtTemp;

                _rtCurrentAdaptedLuminance.SetTarget(device);

                _rtLastAdaptedLuminance.SetTexture(0, device);
                _rtToneMap[0].SetTexture(1, device);

                _map.ElapsedTime = Engine.ElapsedTime;
                _quadRender.DrawQuad(effect);

                lumen = _rtCurrentAdaptedLuminance;
            }
            else
                lumen = _rtToneMap[0];
            #endregion          

            #region BrightPassFilter

            effect.Technique = HdrEffect.BrightPassFilter;
            _rtBrightPassFilter.SetTarget(device);

            _rtHdrScene.SetTexture(0, device);
            lumen.SetTexture(1, device);

            _map.MiddleGray = _state.MiddleGray;
            _map.BrightPassThreshold = _state.BrightThreshold; 
       
            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            _quadRender.DrawQuad(effect);

            #endregion

            stage.SetSampler(0, _linearSampler);

            #region BloomFilter

            #region DownSample 4x4

            effect.Technique = HdrEffect.DownSampler4X4;
            dest = _rtBloom[2];
            source = _rtBrightPassFilter;

            dest.SetTarget(device);
            source.SetTexture(0, device);
            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);

            _quadRender.DrawQuad(effect);

            #endregion

            #region Horizontal Blur

            effect.Technique = HdrEffect.Bloom;
            //Horizontal Blur
            source = _rtBloom[2];
            dest = _rtBloom[1];

            dest.SetTarget(device);
            source.SetTexture(0, device);
            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);

            _map.SampleOffsets = _sampleOffsetsH;
            _map.SampleWeights = _sampleWeights;

            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            _quadRender.DrawQuad(effect);

            #endregion

            #region Vertical Blur
            source = _rtBloom[1];
            dest = _rtBloom[0];

            dest.SetTarget(device);
            source.SetTexture(0, device);

            _map.SampleOffsets = _sampleOffsetsV;
            _map.SampleWeights = _sampleWeights;

            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            _quadRender.DrawQuad(effect);

            #endregion

            #endregion

            #region StarFilter

            RenderStar(device);

            #endregion

            #region FinalPass

            effect.Technique = HdrEffect.FinalScenePass;

            device.RestoreRenderTarget();

            stage.SetSampler(0, _pointSampler);
            stage.SetSampler(1, _pointSampler);
            stage.SetSampler(2, _linearSampler);
            stage.SetSampler(3, _linearSampler);

            _rtHdrScene.SetTexture(0, device);
            lumen.SetTexture(1, device);
            _rtBloom[0].SetTexture(2, device);
            _rtStarFinal.SetTexture(3, device);

            _map.MiddleGray = _state.MiddleGray;

            var glare = _state.Glare;
            if (glare == null || glare.glareLuminance <= 0.0f || glare.starLuminance <= 0.0f)
            {
                _map.GaussianScalar = _state.GaussianMultiplier;
            }
            else
                _map.GaussianScalar = 2 * _state.GaussianMultiplier;
            
            _map.EnableBlueShift = _state.EnableBlueShift;
            _map.StarBlend = _state.StarBlendFactor;
            _map.BloomBlend = _state.BloomBlendFactor;

            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            _quadRender.DrawQuad(effect);

            stage.SetResource(0, null);
            stage.SetResource(1, null);
            stage.SetResource(2, null);
            stage.SetResource(3, null);

            #endregion

            _quadRender.End();
            //device.PopGraphicState<BlendState>();

        }

        private void MensureLuminance(GraphicDevice device, IShaderStage stage)
        {         
            effect.Technique = HdrEffect.SampleAvgLum;

            RenderTexture2D dest = _rtToneMap[Tonemaptextures - 1];
            RenderTexture2D source = _rtHdrScene;

            dest.SetTarget(device);
            source.SetTexture(0, device);
         
            stage.SetSampler(0, _linearSampler);

            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            _quadRender.DrawQuad(effect);

            effect.Technique = HdrEffect.ResampleAvgLum;
            stage.SetSampler(0, _pointSampler);

            for (int i = Tonemaptextures - 2; i > 0; i--)
            {
                dest = _rtToneMap[i];
                source = _rtToneMap[i + 1];

                dest.SetTarget(device);
                source.SetTexture(0, device);
                device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
                _quadRender.DrawQuad(effect);
            }

            effect.Technique = HdrEffect.ResampleAvgLumExp;
            source = _rtToneMap[1];
            dest = _rtToneMap[0];

            dest.SetTarget(device);
            source.SetTexture(0, device);

            device.Clear(ClearFlags.Target, Color4.Black, 1, 0);
            _quadRender.DrawQuad(effect);
        }  
     
        private unsafe void RenderStar(GraphicDevice device)
        {
            RenderTexture2D source;
            RenderTexture2D dest;

            var glare = _state.Glare;
            //renderMgr.Fill(rtStarFinal, 0);            
            // Avoid rendering the star if it's not being used in the current glare
            if (glare == null || glare.glareLuminance <= 0.0f || glare.starLuminance <= 0.0f)
            {
                _rtStarFinal.SetTarget(device);
                device.Clear(ClearFlags.Target, Color4.Black, 1, 0);

                return;
            }

            fixed (Vector4* _ptSampleOffsets = _sampleOffsetsStar)
            {
                Vector2* ptSampleOffsets = (Vector2*)_ptSampleOffsets;
                fixed (Vector4* ptSampleWeights = _sampleWeightsStar)
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

                    effect.Technique = HdrEffect.GaussBlur5X5;
                    source = _rtBrightPassFilter;
                    dest = _rtStartSource;                    

                    dest.SetTarget(device);
                    source.SetTexture(0, device);

                    FillSampleOffsets_GaussBlur5x5(source.Width, source.Height, ptSampleOffsets, ptSampleWeights, 1);

                    _map.SampleOffsets = _sampleOffsetsStar;
                    _map.SampleWeights = _sampleWeightsStar;
                  
                    _quadRender.DrawQuad(effect);

                    #endregion

                    //float fTanFoV = (float)Math.Atan(Numerics.PI / 8.0f);
                    const float fTanFoV = 0.37419668052268495F;
                    const int maxPasses = 3;
                    const int samples = 8;
                    int i; // Loop variables
                    StarDefinition starDef = glare.StarDef;

                    device.PS.SetSampler(0, _linearSampler);

                    float srcW = _rtStartSource.Width;
                    float srcH = _rtStartSource.Height;


                    for (int p = 0; p < maxPasses; p++)
                    {
                        float ratio = (float)(p + 1) / (float)maxPasses;
                        for (int s = 0; s < samples; s++)
                        {
                            Vector4 chromaticAberrColor = Numerics.Lerp(StarDefinition.ChromaticAberrationColor[s], _sColorWhite, ratio);
                            _sAaColor[p, s] = Numerics.Lerp(_sColorWhite, chromaticAberrColor, glare.chromaticAberration);
                        }
                    }

                    _time += starDef.rotation ? Engine.ElapsedTime : 0;
                    float radOffset = glare.starInclination + starDef.inclination + _time;
                  
                    Vector2 stepUv;
                    int lines = starDef.StarLines.Length;

                    float attnPowScaleInicial = (fTanFoV + 0.1f) * 1.0f * (160.0f + 120.0f) / (srcW + srcH) * 1.2f;

                    for (int d = 0; d < lines; d++)
                    {
                        Starline starLine = starDef.StarLines[d];
                        float rad = radOffset + starLine.Inclination;
                        float sn = (float)Math.Sin(rad);
                        float cs = (float)Math.Cos(rad);
                        stepUv.X = sn / srcW * starLine.SampleLength;
                        stepUv.Y = cs / srcH * starLine.SampleLength;

                        float attnPowScale = attnPowScaleInicial;

                        #region PASSES
                        // 1 direction expansion loop
                        source = _rtStartSource;
                        dest = _rtStarPasses[0];
                        for (int p = 0; p < starLine.Passes; p++)
                        {
                            if (p == starLine.Passes - 1)
                            {
                                source = dest;
                                dest = _rtStarLines[d];
                            }
                            // Sampling configration for each stage
                            for (i = 0; i < samples; i++)
                            {
                                float lum = (float)Math.Pow(starLine.Attenuation, attnPowScale * i);

                                ptSampleWeights[i] = _sAaColor[starLine.Passes - 1 - p, i] * lum * (p + 1.0f) * 0.5f;

                                // Offset of sampling coordinate
                                ptSampleOffsets[i].X = stepUv.X * i;
                                ptSampleOffsets[i].Y = stepUv.Y * i;
                                if (Math.Abs(ptSampleOffsets[i].X) >= 0.9f || Math.Abs(ptSampleOffsets[i].Y) >= 0.9f)
                                {
                                    ptSampleOffsets[i] = Vector2.Zero;
                                    ptSampleWeights[i] = Vector4.Zero;
                                }
                            }

                            effect.Technique = HdrEffect.Star;
                            dest.SetTarget(device);
                            source.SetTexture(0, device);

                            _map.SampleOffsets = _sampleOffsetsStar;
                            _map.SampleWeights = _sampleWeightsStar;

                            _quadRender.DrawQuad(effect);

                            // Setup next expansion
                            stepUv *= samples;
                            attnPowScale *= samples;

                            // Set the work drawn just before to next texture source.
                            if (p == 0)
                            {
                                source = dest;
                                dest = _rtStarPasses[1];
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

                    dest = _rtStarFinal;
                    dest.SetTarget(device);
                  
                    float invLines = 1.0f / (float)lines;
                    for (i = 0; i < lines; i++)
                    {
                        _rtStarLines[i].SetTexture(i, device);
                        device.PS.SetSampler(i, _linearSampler);
                        ptSampleWeights[i] = new Vector4(invLines, invLines, invLines, invLines);
                    }

                    effect.Technique = HdrEffect.MergeTexture;
                    _map.NbTextures = lines;
                    _map.SampleWeights = _sampleWeightsStar;
                    
                    _quadRender.DrawQuad(effect);

                    //for (i = 0; i < lines; i++)
                    //{
                    //    device.PSStage.SetSampler(i, pointSampler);
                    //}
                   
                }
            }
        }    
        
        private unsafe void FillDownSampler2X2(int srcWidth, int srcHeight, Vector2* pter)
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
        
        private unsafe void FillDownSampler3X3(int srcWidth, int srcHeight, Vector2* pter)
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
        
        private unsafe void FillDownSampler4X4(int srcWidth, int srcHeight, Vector2* pter , bool shift = true)
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
                _pointSampler.Dispose();
                _linearSampler.Dispose();
                Service.Remove<HdrTechnique>();

                GraphicDeviceFactory.Device.BackBuffer.Resized -= OMBackBuffer_Resized;
            }
            base.OnDispose(d);
        }

        public void ComputeSamples()
        {
            _gaussainMultiplier = _state.GaussianMultiplier;
            _gaussianDev = _state.GaussianDeviation;
            _gaussianMean = _state.GaussainMean;

            unsafe
            {
                fixed (Vector4* ptSamplesWeight = _sampleWeights)
                {
                    float* ptOffset = stackalloc float[15];
                    FillSampleOffsets_Bloom(_rtBloom[0].Width, ptSamplesWeight, ptOffset, _state.GaussianDeviation, _state.GaussianMultiplier);
                    fixed (Vector4* pter = _sampleOffsetsH)
                    {
                        Vector2* pterOffset = (Vector2*)pter;
                        for (int i = 0; i < 15; i++)
                        {
                            pterOffset[i].Y = 0;
                            pterOffset[i].X = ptOffset[i];
                        }
                    }

                    FillSampleOffsets_Bloom(_rtBloom[0].Height, ptSamplesWeight, ptOffset, _state.GaussianDeviation, _state.GaussianMultiplier);
                    fixed (Vector4* pter = _sampleOffsetsV)
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
