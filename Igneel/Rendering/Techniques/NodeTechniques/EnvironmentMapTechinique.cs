using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel.Graphics;
using System.Runtime.Serialization;
using Igneel.Assets;
using System.ComponentModel;
using Igneel.Design;
using Igneel.Components;

namespace Igneel.Rendering
{
    [TypeConverter(typeof(DesignTypeConverter))]
    [ProviderActivator(typeof(EnvironmentMapTechnique.Activator))]
    public class EnvironmentMapTechnique : CubeMapTechique
    {        
      
        public EnvironmentMapTechnique()
            : base(new Vector3(0, 0, 0), Engine.Lighting.Reflection.EnvironmentMapSize, 
                                                Engine.Lighting.Reflection.EnvironmentMapZn, 
                                                Engine.Lighting.Reflection.EnvironmentMapZf,
                                                Format.R8G8B8A8_UNORM)
        {            
            if (Engine.Lighting.HDR.Enable)
            {
                format = Engine.Lighting.HDR.Technique.HRDFormat;
            }
            Initialize();
        }

        public EnvironmentMapTechnique(int size)
            : this(size, Engine.Lighting.Reflection.EnvironmentMapZn, Engine.Lighting.Reflection.EnvironmentMapZf)
        {
            
        }

        public EnvironmentMapTechnique(int size, float zn, float zf) 
            : this(Vector3.Zero, size, zn, zf) { }

        public EnvironmentMapTechnique(Vector3 position, int size, float zn, float zf)
            : base(position, size, zn, zf,  Format.R8G8B8A8_UNORM) 
        {
            if (Engine.Lighting.HDR.Enable)
            {
                format = Engine.Lighting.HDR.Technique.HRDFormat;
            }
            Initialize();
        }

        public override void Initialize()
        {
            Engine.Lighting.HDR.EnableChanged += new EventHandler(Lighting_HDREnableChanged);
            base.Initialize();
        }

        void Lighting_HDREnableChanged(object sender, EventArgs e)
        {
            format = Engine.Graphics.BackBuffer.SurfaceFormat;
            if (Engine.Lighting.HDR.Enable)
            {
                format = Engine.Lighting.HDR.Technique.HRDFormat;
            }

            Initialize();
        }       

        public override void Apply()
        {
            if(Engine.Lighting.Reflection.Enable)
            {
                Engine.Lighting.Reflection.Enable = false;

                base.Apply();

                Engine.Lighting.Reflection.Enable = true;
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
                Engine.Lighting.HDR.EnableChanged -= new EventHandler(Lighting_HDREnableChanged);
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
