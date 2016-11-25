using System.ComponentModel;

namespace Igneel.States
{
   
    public class LightingState : EngineState
    {             
        HdrState _hdrSettings = new HdrState();
        private SsaoState _ssao = new SsaoState();        
        private PrtState _prt = new PrtState();       
        private ReflectionState _reflection = new ReflectionState() { Enable = true };

        public LightingState()
        {            
            TransparencyEnable = true;
            EnableAmbient = true;
        }       
        
        
        public bool TransparencyEnable { get; set; }
        
        [Category("Reflection")]
        public ReflectionState Reflection { get { return _reflection; } }       

        [Category("HDR")]
        public HdrState Hdr { get { return _hdrSettings; } }        

        [Category("SSAO")]       
        public SsaoState Ssao { get { return _ssao; } }
        

        [Category("PRT")]       
        public PrtState Prt { get { return _prt; } }

        [Category("HemisphericalAmbient")]
        public bool HemisphericalAmbient { get; set; }       

        
        public bool PerPixelLightingEnable { get; set; }              

        public bool EnableAmbient { get; set; }
    }

}


