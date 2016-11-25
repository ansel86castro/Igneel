using Igneel.Graphics;

namespace Igneel.Techniques
{
    public class SceneShadowMapping : DefaultTechnique { }

    public class RenderShadowFactorTechnique : DefaultTechnique 
    {
        public override void Apply()
        {
            var scene = Engine.Scene;            
            var device = GraphicDeviceFactory.Device;          

            //SceneTechnique.RenderSceneRenderList(scene.VisibleComponents, true);
            DefaultTechnique.RenderScene(true);
        }
    }

    public class DepthSceneRender : DefaultTechnique
    {
        public override void Apply()
        {
            var scene = Engine.Scene;

            scene.UpdateVisibleComponents();
            DefaultTechnique.RenderSceneRenderList(scene.VisibleComponents, false);
        }
    }

    //public class SoftShadowMappingTechnique : SceneTechnique
    //{
    //    TextureRT renderTarget;
    //    TextureRT[] blurRenderTarget= new TextureRT[2];

    //    Vector2[] sampleOffsets = new Vector2[15];
    //    Vector4[] sampleWeights = new Vector4[15];
    //    float[] afTexCoordOffset = new float[15];
    //    private ShaderEffect effect;
    //    EffectHandle hSamplerOffsets;
    //    EffectHandle hSamplerWeights;
    //    EffectHandle tcGaussBlur5x5;
    //    EffectHandle tcBlur;           

    //    public SoftShadowMappingTechnique()
    //        :base(true)
    //    {
    //        Singleton.SetInstance<SoftShadowMappingTechnique>(this);

    //        effect = Effect.GetEffect<BlurTextureShader>();
    //        hSamplerOffsets = effect.GetParameter("SampleOffsets");
    //        hSamplerWeights = effect.GetParameter("SampleWeights");
    //        tcGaussBlur5x5 = effect.GetTechique("GaussBlur5x5");
    //        tcBlur = effect.GetTechique("Bloom");           

    //        Initialize();
    //    }


    //    public TextureRT BlurShadowFactorMap { get { return blurRenderTarget[0]; } }

    //    public TextureRT ShadowFactorMap { get { return renderTarget; } }

    //    public override void Apply()
    //    {           
    //            #region Render Shadow Factor

    //            var scene = Engine.Scene;
    //            var renderMgr = Engine.RenderManager;
    //            var device = GraphicDeviceFactory.Device;
    //            var quadRender = renderMgr.QuadRender;
    //            quadRender.Effect = effect;

    //            var oldRT = renderMgr.GetRenderTarget2D(0);
    //            renderMgr.SetRenderTarget2D(0, renderTarget);
                
    //            scene.UpdateRenderLists();

    //            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, unchecked((int)0xFFFFFFFF), 1, 0);
    //            renderMgr.ApplyTechnique<RenderShadowFactorTechnique>();

    //            #endregion

    //            #region Blur Shadow Factor Texture

    //            unsafe
    //            {
    //                fixed (Vector2* ptSampleOffset = sampleOffsets)
    //                {
    //                    fixed (Vector4* ptSampleWeights = sampleWeights)
    //                    {
    //                        fixed (float* ptTexCoordOffset = afTexCoordOffset)
    //                        {
    //                            #region GaussianBlur5x5

    //                            effect.Technique = tcGaussBlur5x5;
    //                            var dest = blurRenderTarget[0];
    //                            var source = renderTarget;

    //                            Convolve.FillSampleOffsetsGaussBlur5x5(source.Width, source.Height, ptSampleOffset, ptSampleWeights, 1);

    //                            effect.SetValue(hSamplerOffsets, sampleOffsets);
    //                            effect.SetValue(hSamplerWeights, sampleWeights);

    //                            renderMgr.SetTextureRT(0, source);
    //                            renderMgr.SetRenderTarget2D(0, dest);

    //                            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, unchecked((int)0xFFFFFFFF), 1, 0);
    //                            quadRender.Draw(dest.Width, dest.Height);

    //                            #endregion

    //                            effect.Technique = tcBlur;
    //                            var state = EngineState.Shadow.SofShadowState;

    //                            #region Blur

    //                            for (int j = 0; j < 2; j++)
    //                            {
    //                                if (j == 0)
    //                                {
    //                                    //horizontal blur
    //                                    source = blurRenderTarget[0];
    //                                    dest = blurRenderTarget[1];
    //                                }
    //                                else
    //                                {
    //                                    //vertical blur
    //                                    source = blurRenderTarget[1];
    //                                    dest = blurRenderTarget[0];
    //                                }

    //                                Convolve.FillSampleOffsetsBloom(source.Width, ptSampleWeights, ptTexCoordOffset, state.Deviation, state.Multiplier);

    //                                for (int i = 1; i < 15; i++)
    //                                {
    //                                    if (i == 0)
    //                                    {
    //                                        ptSampleOffset[i].Y = 0;
    //                                        ptSampleOffset[i].X = afTexCoordOffset[i];
    //                                    }
    //                                    else
    //                                    {
    //                                        ptSampleOffset[i].X = 0;
    //                                        ptSampleOffset[i].Y = afTexCoordOffset[i];
    //                                    }
    //                                }

    //                                effect.SetValue(hSamplerOffsets, sampleOffsets);
    //                                effect.SetValue(hSamplerWeights, sampleWeights);

    //                                renderMgr.SetTextureRT(0, source);
    //                                renderMgr.SetRenderTarget2D(0, dest);
    //                                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, unchecked((int)0xFFFFFFFF), 1, 0);
    //                                quadRender.Draw(dest.Width, dest.Height);
    //                            }
    //                            #endregion

    //                        }
    //                    }
    //                }
    //            }
    //            #endregion

    //            #region Render Scene

    //            renderMgr.ClearTextureSampler(0);

    //            renderMgr.SetRenderTarget2D(0, oldRT);
    //            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Engine.BackColor, 1, 0);

    //            EngineState.Shadow.Enable = false;
    //            SceneTechnique.RenderScene(true);

    //            EngineState.Shadow.Enable = true;

    //            #endregion

    //            #region Test

    //            //quadRender.Effect = ShaderStore.Untransformer;
    //            //quadRender.Effect.Technique = "Technique3";
    //            //renderMgr.SetTexture(0, blurRenderTarget[0]);
    //            //device.Clear(ClearFlags.Target, unchecked((int)0xFFFFFFFF), 1, 0);
    //            //quadRender.DrawToTexture(blurRenderTarget[0].Width, blurRenderTarget[0].Height);

    //            #endregion
            
    //    }

    //    public override void Bind(Render render)
    //    {
    //        render.Bind(this);
    //    }

    //    public override void UnBind(Render render)
    //    {
    //        render.UnBind(this);
    //    }

    //    private void SetGlobalRenderParameters(ShaderEffect render)
    //    {           
    //        if (effect.SupportDescription && effect.Description.ShadowMapRegister >= 0)
    //        {
    //            Engine.RenderManager.SetTextureRT(effect.Description.ShadowMapRegister, blurRenderTarget[0]);
    //        }
    //    }

    //    protected override void OnLostDevice(SlimDX.Direct3D9.Device device)
    //    {           
    //        if (renderTarget != null)
    //            renderTarget.Dispose();
    //        for (int i = 0; i < blurRenderTarget.Length; i++)
    //        {
    //            if (blurRenderTarget[i] != null)
    //                blurRenderTarget[i].Dispose();
    //        }
    //    }

    //    protected override void OnResetDevice(SlimDX.Direct3D9.Device device)
    //    {
    //        renderTarget = new TextureRT( Engine.BackBuffer.Width, Engine.BackBuffer.Height, SlimDX.Direct3D9.Format.X8R8G8B8, Engine.PresentParams.AutoDepthStencilFormat, true);

    //        for (int i = 0; i < blurRenderTarget.Length; i++)
    //        {
    //            blurRenderTarget[i] = new TextureRT(renderTarget.Width, renderTarget.Height, renderTarget.Format);
    //        }
    //    }

    //    protected override void OnDispose()
    //    {
    //        if (renderTarget != null)
    //            renderTarget.Dispose();            

    //        Singleton.RemoveService<SoftShadowMappingTechnique>();
    //        base.OnDispose();
    //    }      
    //}

    
    
}
