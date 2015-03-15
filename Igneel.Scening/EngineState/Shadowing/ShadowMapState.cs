﻿
using Igneel.Graphics;
using Igneel.Rendering;

using Igneel.Scenering.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering
{
    public class ShadowMapState:EnabilitableState
    {                  
        public ShadowMapState()
        {
            Size = 1024;
            Bias = 0.6e-3f;
            PCFBlurSize = 3;
            GaussianDeviation = (float)Math.Sqrt(3);
        }

        
        public int Size { get; set; }

        public float Bias { get; set; }

        public bool UseGaussianFilter { get; set; }

        public float GaussianDeviation { get; set; }

        public int PCFBlurSize { get; set; }

        public override void OnEnableChanged()
        {
            base.OnEnableChanged();

            if (Enable)
            {
                if (RenderManager.ActiveTechnique is DefferedLigthing<DefferedLightingEffect>)
                {
                    RenderManager.InsertTechnique<SceneShadowMapping>(x => x == typeof(DefferedLigthing<DefferedLightingEffect>));
                    RenderManager.PushTechnique<DefferedLigthing<DefferedShadowRender>>();
                }
                else
                    RenderManager.InsertTechnique<SceneShadowMapping>(x => x == typeof(SceneTechnique));
            }
            else
            {
                if (RenderManager.ActiveTechnique is DefferedLigthing<DefferedShadowRender>)
                {
                    RenderManager.RemoveTechnique<DefferedLigthing<DefferedShadowRender>>();
                }

                RenderManager.RemoveTechnique<SceneShadowMapping>();
                var render = Service.Get<SceneShadowMapping>();
                if (render != null)
                    render.Dispose();
            }
         
        }       

   
       

    }

    public class SoftShadowMapState : EnabilitableState
    {
        public SoftShadowMapState()
        {
            Multiplier = 1f;
            Deviation = 3.0f;
        }

        public float Multiplier { get; set; }

        public float Deviation { get; set; }

        public override void OnEnableChanged()
        {
            //if (Enable)
            //{
            //    Engine.InsertTechnique<SoftShadowMappingTechnique>(x => x == typeof(SceneShadowMapping));
            //}
            //else
            //{
            //    var entry = Engine.RemoveTechnique<SoftShadowMappingTechnique>();
            //    var render = Singleton.GetInstance<SoftShadowMappingTechnique>();
            //    if (render != null)
            //        render.Dispose();
            //}

            base.OnEnableChanged();
        }

        //public override void Dispose()
        //{
        //    IDisposable render = Singleton.GetInstance<SoftShadowMappingTechnique>();
        //    if (render != null)
        //        render.Dispose();

        //    base.Dispose();
        //}
    }
}