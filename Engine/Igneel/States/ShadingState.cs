using System;
using System.ComponentModel;
using Igneel.Graphics;

namespace Igneel.States
{
    [Serializable]
    public class ShadingState:EngineState
    {
        private DepthOfFieldState _dof = new DepthOfFieldState();
        private FogState _fog = new FogState();
        private MotionBlurState _mblur = new MotionBlurState();
        private LensFlareState _lensFlare = new LensFlareState();
        private DeferredLightingState _deferredLigthing = new DeferredLightingState();

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
        public DepthOfFieldState DepthOfField { get { return _dof; } }

        [Category("Fog")]
        public FogState Fog { get { return _fog; } }

        [Category("MotionBlur")]
        public MotionBlurState MotionBlur { get { return _mblur; } }

        [Category("LensFlare")]
        public LensFlareState LensFlare { get { return _lensFlare; } }

        [Category("DeferredLigthing")]
        public DeferredLightingState DeferredLigthing
        {
            get { return _deferredLigthing; }
        }
       
    }
}
