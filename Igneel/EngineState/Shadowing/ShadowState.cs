using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using Igneel.Design;
using Igneel.Design.UITypeEditors;
using System.Drawing.Design;
using Igneel.Graphics;
using System.Runtime.Serialization;
using System.Reflection;

namespace Igneel
{
    public class ShadowState:EnabilitableState
    {
        public enum Algoritm { SHADOW_MAP, CUBE_SHADOW_MAP, SHADOW_VOLUME, RAY_TRACING }
              
        private ShadowMapState shadowMap = new ShadowMapState();

        private SoftShadowMapState sofSm = new SoftShadowMapState();      

        public ShadowState()
        {            
            
        }        

        [Category("ShadowMapping")]
        public ShadowMapState ShadowMapping { get { return shadowMap; } }                

        [LockOnSet]
        public Algoritm ShadowAlgoritm { get; set; }

        [Category("SoftShadow")]
        public SoftShadowMapState SofShadowState { get { return sofSm; } }        
      
    }
}
