using Igneel.Design;
using Igneel.Design.UITypeEditors;
using Igneel.Graphics;
using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    [Serializable]
    [TypeConverter(typeof(DesignTypeConverter))]
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
        [Browsable(false)]
        public HDRTechnique Technique
        {
            get { return Service.Get<HDRTechnique>(); }
        }

        public bool EnableBlueShift { get; set; }

        public bool CalculateEyeAdaptation { get; set; }

        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
        public float MiddleGray { get; set; }
     
        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
        public float BrightThreshold { get; set; }

        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
        public float GaussianMultiplier { get; set; }

        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
        public float GaussainMean { get; set; }

        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
        public float GaussianDeviation { get; set; }
      
        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
        public float BloomBlendFactor { get; set; }

        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
        public float StarBlendFactor { get; set; }

        [LockOnSet]
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
        
        [Browsable(false)]
        public GlareDefinition Glare { get { return glare; } }

        public override void OnEnableChanged()
        {         
            
            if (Enable)
            {
                var hdrRender = Technique;
                if (hdrRender == null || hdrRender.Disposed)
                    Service.Require<HDRTechnique>();
                Engine.PushTechnique<HDRTechnique>(Technique);
            }
            else
            {
                Engine.PopTechnique();
                var hdrRender = Technique;
                if (hdrRender != null)
                    hdrRender.Dispose();
            }

            base.OnEnableChanged();
          
        }       
    }
}
