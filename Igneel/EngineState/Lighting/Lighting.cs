using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;



using Igneel.Graphics;
using System.Runtime.Serialization;
using System.Reflection;
using Igneel.Physics;

namespace Igneel
{
   
    public class LightingState : EngineState
    {             
        HDRState hdrSettings = new HDRState();
        private SSAOState ssao = new SSAOState();
        private GSAOState gsao = new GSAOState();
        private PRTState prt = new PRTState();       
        private ReflectionState reflection = new ReflectionState() { Enable = true };

        public LightingState()
        {            
            TransparencyEnable = true;
            EnableAmbient = true;
        }       
        
        
        public bool TransparencyEnable { get; set; }
        
        [Category("Reflection")]
        public ReflectionState Reflection { get { return reflection; } }       

        [Category("HDR")]
        public HDRState HDR { get { return hdrSettings; } }        

        [Category("SSAO")]       
        public SSAOState SSAO { get { return ssao; } }

        [Category("GSSAO")]       
        public GSAOState GSAO { get { return gsao; } }

        [Category("PRT")]       
        public PRTState PRT { get { return prt; } }

        [Category("HemisphericalAmbient")]
        public bool HemisphericalAmbient { get; set; }       

        
        public bool PerPixelLightingEnable { get; set; }              

        public bool EnableAmbient { get; set; }
    }

}


