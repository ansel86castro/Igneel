

using Igneel.Graphics;
using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering
{
    [Serializable]
   
    public class HDRState : EnabilitableState
    {       
        GlareDefinition glare;
        private GlareLibType glareType;

        public HDRState() 
        {
            GaussianMultiplier = 0.4f;
            GaussianDeviation = 0.8f;
            GaussainMean = 0;
            BrightThreshold = 5.0f;
            MiddleGray = 0.5f;          
            BloomBlendFactor = 0.6f;
            EnableBlueShift = true;
            CalculateEyeAdaptation = true;
            StarBlendFactor = 0.6f;            
        }
        
        [NonSerializedProp]
       
        public HDRTechnique Technique
        {
            get { return Service.Get<HDRTechnique>(); }
        }

        public bool EnableBlueShift { get; set; }

        public bool CalculateEyeAdaptation { get; set; }

        
        public float MiddleGray { get; set; }
     
        
        public float BrightThreshold { get; set; }

        
        public float GaussianMultiplier { get; set; }

        
        public float GaussainMean { get; set; }

        
        public float GaussianDeviation { get; set; }
      
        
        public float BloomBlendFactor { get; set; }

        
        public float StarBlendFactor { get; set; }

        
        public GlareLibType GlareType
        {
            get { return glareType; }
            set{
                glareType = value;
                glare = GlareDefinition.GetLib(glareType);
                if(glare !=null)
                    glare.StarDefinition.Rotation = false;
            }
        }
        
       
        public GlareDefinition Glare { get { return glare; } }

        public override void OnEnableChanged()
        {         
            
            if (Enable)
            {
                var hdrRender = Technique;
                if (hdrRender == null || hdrRender.Disposed)
                    Service.Require<HDRTechnique>();
                RenderManager.PushTechnique<HDRTechnique>(Technique);
            }
            else
            {
                RenderManager.PopTechnique();
                var hdrRender = Technique;
                if (hdrRender != null)
                    hdrRender.Dispose();
            }

            base.OnEnableChanged();
          
        }       
    }
}
