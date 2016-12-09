using System;
using Igneel.Rendering;
using Igneel.Techniques;

namespace Igneel.States
{
    [Serializable]
   
    public class HdrState : EnabilitableState
    {       
        GlareDefinition _glare;
        private GlareLibType _glareType;

        public HdrState() 
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
       
        public HdrTechnique Technique
        {
            get { return Service.Get<HdrTechnique>(); }
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
            get { return _glareType; }
            set{
                _glareType = value;
                _glare = GlareDefinition.GetLib(_glareType);
                if(_glare !=null)
                    _glare.StarDefinition.Rotation = false;
            }
        }
        
       
        public GlareDefinition Glare { get { return _glare; } }

        public override void OnEnableChanged()
        {         
            
            if (Enable)
            {
                var hdrRender = Technique;
                if (hdrRender == null || hdrRender.Disposed)
                    Service.Require<HdrTechnique>();
                RenderManager.PushTechnique<HdrTechnique>(Technique);
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
