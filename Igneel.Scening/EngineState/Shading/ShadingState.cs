
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering
{
    [Serializable]
    public class ShadingState:EngineState
    {
        private DepthOfFieldState dof = new DepthOfFieldState();
        private FogState fog = new FogState();
        private MotionBlurState mblur = new MotionBlurState();
        private LensFlareState lensFlare = new LensFlareState();
        private DeferredLightingState deferredLigthing = new DeferredLightingState();

        public event EventHandler DeferredLightingEnable;

        public ShadingState()
        {
            BumpMappingEnable = true;
            CullMode = CullMode.Front;
            FillMode = FillMode.Solid;
        }

        public CullMode CullMode { get; set; }

        public FillMode FillMode { get; set; }

        
        public bool BumpMappingEnable { get; set; }

        
        public bool ParallaxMappingEnable { get; set; }

        [Category("DepthOfField")]
        public DepthOfFieldState DepthOfField { get { return dof; } }

        [Category("Fog")]
        public FogState Fog { get { return fog; } }

        [Category("MotionBlur")]
        public MotionBlurState MotionBlur { get { return mblur; } }

        [Category("LensFlare")]
        public LensFlareState LensFlare { get { return lensFlare; } }

        [Category("DeferredLigthing")]
        public DeferredLightingState DeferredLigthing
        {
            get { return deferredLigthing; }
        }
       
    }
}
