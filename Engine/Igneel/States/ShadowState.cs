using System.ComponentModel;

namespace Igneel.States
{
    public class ShadowState:EnabilitableState
    {
        public enum Algoritm { ShadowMap, CubeShadowMap, ShadowVolume, RayTracing }
              
        private ShadowMapState _shadowMap = new ShadowMapState();

        private SoftShadowMapState _sofSm = new SoftShadowMapState();      

        public ShadowState()
        {            
            
        }        

        [Category("ShadowMapping")]
        public ShadowMapState ShadowMapping { get { return _shadowMap; } }                

        
        public Algoritm ShadowAlgoritm { get; set; }

        [Category("SoftShadow")]
        public SoftShadowMapState SofShadowState { get { return _sofSm; } }        
      
    }
}
