using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




namespace Igneel.Rendering
{
    /// <summary>
    /// Default skydome render
    /// </summary>
    //public class SkyDomeRender : SceneNodeRender<SkyBoxEffect>
    //{              
    //    public SkyDomeRender()
    //    {
            
    //    }

    //    protected override ShaderEffect LoadEffect()
    //    {
    //        return ShaderStore.SkyBox;
    //    }

    //    protected override void OnRender(SceneNode target)
    //    {
    //        var device = GraphicDeviceFactory.Device;
    //        var zwrite = device.GetRenderState(RenderState.ZWriteEnable);
    //        device.SetRenderState(RenderState.ZWriteEnable, false);
    //        var cull = device.GetRenderState(RenderState.CullMode);
    //        device.SetRenderState(RenderState.CullMode, Cull.None);

    //        var skyDome = (SkyDomeNode)target;
    //        var sky = skyDome.Sky;
    //        device.VertexDeclaration = sky.VertexDescriptor.VertexDeclaration;
    //        device.SetStreamSource(0, sky.VertexBuffer, 0, sky.VertexDescriptor.Size);
    //        device.Indices = sky.IndexBuffer;


    //        Effect _effect = effect.D3DEffect;

    //        _effect.Begin(0);
    //        _effect.BeginPass(0);

    //        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, sky.VertexCount, 0, sky.IndexCount / 3);

    //        _effect.EndPass();
    //        _effect.End();

    //        device.SetRenderState(RenderState.ZWriteEnable, zwrite);
    //        device.SetRenderState(RenderState.CullMode, cull);
    //    }       
      
    //}
}
