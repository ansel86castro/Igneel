namespace Igneel.Rendering.Pendings
{
    //public class ParticleSystemRender : CallbackGeometryRender
    //{
    //    Matrix mat;
    //    public ParticleSystemRender()
    //    {           
    //        mat = new Matrix();

    //    }
    //    protected override ShaderEffect LoadEffect()
    //    {
    //        return ShaderStore.ParticleSystem;
    //    }        

    //    protected override void DrawNodeGeometry()
    //    {
    //        if (Enable)
    //        {
    //            EffectDescription ed = effect.Description;                             

    //            var pSystem = (ParticleSystemBase)Node;
    //            Effect _effect = effect.D3DEffect;
    //            var device = GraphicDeviceFactory.Device;
    //            device.VertexDeclaration = pSystem.VertexDescriptor.VertexDeclaration;                

    //            TextureHandle<Texture>[] textures = pSystem.Textures;
    //            var samples = Engine.RenderManager.Samplers;
    //            if (ed.DiffuseSamplerRegister > 0 && textures != null)
    //            {
    //                samples[ed.DiffuseSamplerRegister].SetState(textures[0]);
    //            }

    //            //mat = Matrix.Translation(-0.5f, -0.5f, 0);
    //            //mat.Multiply(Matrix.RotationZ(Environment.TickCount/1000.0f));
    //            //mat.Multiply(Matrix.Translation(0.5f, 0.5f, 0));
    //            //effect.SetValue("Rot", Environment.TickCount / 1000.0f);

    //            Matrix vp = view * projection;
    //            effect.SetViewProjMatrix(vp);
    //            effect.SetEyePosition(eyePosition);
    //            effect.SetViewMatrix(view);                
               

    //            int passes = _effect.Begin(0);
    //            for (int i = 0; i < passes; i++)
    //            {
    //                _effect.BeginPass(i);

    //                base.DrawNodeGeometry();

    //                _effect.EndPass();
    //            }
    //            _effect.End();
               
    //        }
    //    }
    //}
}
