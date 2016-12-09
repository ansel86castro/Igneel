using System;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.States;

namespace Igneel.Techniques
{
   
   // [ResourceActivator(typeof(EnvironmentMapTechnique.Activator))]
    public class EnvironmentMapTechnique : CubeMapTechique
    {        
      
        public EnvironmentMapTechnique()
            : base(new Vector3(0, 0, 0), EngineState.Lighting.Reflection.EnvironmentMapSize, 
                                                EngineState.Lighting.Reflection.EnvironmentMapZn, 
                                                EngineState.Lighting.Reflection.EnvironmentMapZf,
                                                Format.R8G8B8A8_UNORM)
        {            
            if (EngineState.Lighting.Hdr.Enable)
            {
                format = EngineState.Lighting.Hdr.Technique.HrdFormat;
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
            if (EngineState.Lighting.Hdr.Enable)
            {
                format = EngineState.Lighting.Hdr.Technique.HrdFormat;
            }
            Initialize();
        }

        public override void Initialize()
        {
            EngineState.Lighting.Hdr.EnableChanged += new EventHandler(Lighting_HDREnableChanged);
            base.Initialize();
        }

        void Lighting_HDREnableChanged(object sender, EventArgs e)
        {
            format = GraphicDeviceFactory.Device.BackBuffer.SurfaceFormat;
            if (EngineState.Lighting.Hdr.Enable)
            {
                format = EngineState.Lighting.Hdr.Technique.HrdFormat;
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
                EngineState.Lighting.Hdr.EnableChanged -= new EventHandler(Lighting_HDREnableChanged);
            }
            base.OnDispose(d);
        }

        //[Serializable]
        //class Activator : IResourceActivator
        //{
        //    int _size;
        //    float _zn;
        //    float _zf;
        //    bool _dynamic;
        //    Matrix _pose;
        //    Vector3 _posOffset;

        //    public void Initialize(IAssetProvider provider)
        //    {
        //        var tech = (EnvironmentMapTechnique)provider;
        //        _size = tech.Size;
        //        _zn = tech.NearClipPlane;
        //        _zf = tech.FarClipPlane;
        //        _dynamic = tech.IsDynamic;
        //        _pose = tech.Affector.GlobalPose;
        //        _posOffset = tech.PositionOffset;
        //    }

        //    public IAssetProvider OnCreateResource()
        //    {
        //        var tech = new EnvironmentMapTechnique(_size, _zn, _zf);
        //        tech.PositionOffset = _posOffset;
        //        tech.IsDynamic = _dynamic;
        //        tech.UpdatePose(_pose);                
        //        return tech;
        //    }
        //}
    }
}
