using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.SceneComponents;

namespace Igneel.Rendering
{
    public class ParticleSystemRender<TEffect> : GraphicRender<ParticleSystem, TEffect>
        where TEffect :Effect
    {        
        public ParticleSystemRender()
        {
            //hCamUp = effect.TryGetGlobalParameter("camUp");            
        }

        public override void Draw(ParticleSystem component)
        {
            var device = GraphicDeviceFactory.Device;

            int count = component.UpdateBuffer();

            //device.SetStreamSource(0, component.QuadVertexBuffer, 0, component.VertDescriptor.Size);
            //device.Indices = component.Indices;

            //device.VertexDeclaration = component.VertDescriptor.VertexDeclaration;

            //Bind(component.Material);
            //Bind<TShader>(default(TShader));

            //effect.SetValue(hCamUp, Engine.Scene.ActiveCamera.Up);
         

            //device.SetRenderState(RenderState.CullMode, Cull.None);
            //device.SetRenderState(RenderState.AlphaBlendEnable, component.BlendEnable);
            //device.SetRenderState(RenderState.SourceBlend, (int)component.SourceBlend);
            //device.SetRenderState(RenderState.DestinationBlend, (int)component.DestinationBlend);
            //device.SetRenderState(RenderState.BlendOperation, (int)component.BlendOperation);
            //device.SetRenderState(RenderState.ZWriteEnable, false);            

            //var _effect = effect.D3DEffect;            
            //for (int i = 0, passes = _effect.Begin(); i < passes; i++)
            //{
            //    _effect.BeginPass(i);

            //    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, count * 4, 0, count * 2);

            //    _effect.EndPass();
            //}
            //_effect.End();

            //states.Apply();

            //UnBind(component.Material);
        }
    }
}
