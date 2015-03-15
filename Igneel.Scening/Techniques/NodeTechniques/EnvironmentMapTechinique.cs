using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel.Graphics;
using System.Runtime.Serialization;
using Igneel.Assets;
using System.ComponentModel;
using Igneel.Scenering;



namespace Igneel.Rendering
{
   
    [ProviderActivator(typeof(EnvironmentMapTechnique.Activator))]
    public class EnvironmentMapTechnique : CubeMapTechique
    {        
      
        public EnvironmentMapTechnique()
            : base(new Vector3(0, 0, 0), EngineState.Lighting.Reflection.EnvironmentMapSize, 
                                                EngineState.Lighting.Reflection.EnvironmentMapZn, 
                                                EngineState.Lighting.Reflection.EnvironmentMapZf,
                                                Format.R8G8B8A8_UNORM)
        {            
            if (EngineState.Lighting.HDR.Enable)
            {
                format = EngineState.Lighting.HDR.Technique.HRDFormat;
            }
            Initialize();
        }

        public EnvironmentMapTechnique(int size)
            : this(size, EngineState.Lighting.Reflection.EnvironmentMapZn, EngineState.Lighting.Reflection.EnvironmentMapZf)
        {
            
        }

        public EnvironmentMapTechnique(int size, float zn, float zf) 
            : this(Vector3.Zero, size, zn, zf) { }

        public EnvironmentMapTechnique(Vector3 position, int size, float zn, float zf)
            : base(position, size, zn, zf,  Format.R8G8B8A8_UNORM) 
        {
            if (EngineState.Lighting.HDR.Enable)
            {
                format = EngineState.Lighting.HDR.Technique.HRDFormat;
            }
            Initialize();
        }

        public override void Initialize()
        {
            EngineState.Lighting.HDR.EnableChanged += new EventHandler(Lighting_HDREnableChanged);
            base.Initialize();
        }

        void Lighting_HDREnableChanged(object sender, EventArgs e)
        {
            format = GraphicDeviceFactory.Device.BackBuffer.SurfaceFormat;
            if (EngineState.Lighting.HDR.Enable)
            {
                format = EngineState.Lighting.HDR.Technique.HRDFormat;
            }

            Initialize();
        }       

        public override void Apply()
        {
            if(EngineState.Lighting.Reflection.Enable)
            {
                EngineState.Lighting.Reflection.Enable = false;

                base.Apply();

                EngineState.Lighting.Reflection.Enable = true;
            }
        }

        public override void Bind(Render render)
        {
            render.Bind(this);
        }

        public override void UnBind(Render render)
        {
            render.UnBind(this);
        }

        protected override void OnDispose(bool d)
        {
            if (d)
            {
                EngineState.Lighting.HDR.EnableChanged -= new EventHandler(Lighting_HDREnableChanged);
            }
            base.OnDispose(d);
        }

        [Serializable]
        class Activator : IProviderActivator
        {
            int size;
            float zn;
            float zf;
            bool dynamic;
            Matrix pose;
            Vector3 posOffset;

            public void Initialize(IAssetProvider provider)
            {
                var tech = (EnvironmentMapTechnique)provider;
                size = tech.Size;
                zn = tech.NearClipPlane;
                zf = tech.FarClipPlane;
                dynamic = tech.IsDynamic;
                pose = tech.Affector.GlobalPose;
                posOffset = tech.PositionOffset;
            }

            public IAssetProvider CreateInstance()
            {
                var tech = new EnvironmentMapTechnique(size, zn, zf);
                tech.PositionOffset = posOffset;
                tech.IsDynamic = dynamic;
                tech.UpdatePose(pose);                
                return tech;
            }
        }
    }
}
