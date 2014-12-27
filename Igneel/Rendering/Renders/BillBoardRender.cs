using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Igneel.Components;
using Igneel.Graphics;
using Igneel.Rendering.Effects;
using Igneel.Rendering.Bindings;

namespace Igneel.Rendering
{
    public class BillBoardRender<T> : GraphicObjectRender<T, BillBoard>
        where T : Effect
    {
        interface IMap
        {
            Matrix WorldViewProj { get; set; }
            Matrix World { get; set; }
            Matrix ViewProj { get; set; }
            Vector4 Color { get; set; }
        }

        IMap map;
        GraphicBuffer vb;
        VertexDescriptor vd;
        RasterizerState rastState;

        /// <summary>
        /// P0----P1
        /// |   / |
        /// | /   |
        /// P3----P2
        /// </summary>
        public readonly static VertexPTx[] Vertices = new VertexPTx[]
                {
                    new VertexPTx(-0.5f ,0.5f ,0 ,0 ,0), //P0
                    new VertexPTx(0.5f ,0.5f ,0 ,1 ,0),  //P1
                    new VertexPTx(-0.5f ,-0.5f ,0 ,0 ,1),//P3
                    new VertexPTx(0.5f ,-0.5f ,0 ,1 ,1)  //P2
                };

        public BillBoardRender()
            :base()
        {

            vd = VertexDescriptor.GetDescriptor<VertexPTx>();
            vb = Engine.Graphics.CreateVertexBuffer(data:Vertices);            

            if (this.effect != null)
            {
                map = effect.Map<IMap>();
                
                //hWorldViewProj = this.effect.TryGetGlobalSematic(ShaderSemantics.WORLDVIEWPROJ);
                //hColor = this.effect.TryGetGlobalSematic(ShaderSemantics.COLOR);
                //hWorld = this.effect.TryGetGlobalSematic(ShaderSemantics.World);
                //hViewProj = this.effect.TryGetGlobalSematic(ShaderSemantics.VIEWPROJ);
            }
            rastState = Engine.Graphics.CreateRasterizerState(new RasterizerDesc
            {
                 Cull = CullMode.None
            });
        }      

        public override void Draw(BillBoard component)
        {
            var device = Engine.Graphics;

            device.IASetVertexBuffer(0, vb, 0);
            device.RasterizerStack.Push(rastState);

            map.Color = component.Color;            

            Bind(component);

            device.PS.SetResource(0, component.Texture);

            map.World = component.GetBillboardMatrix(Engine.Scene.ActiveCamera, Node.GlobalPosition, Node.LocalScale);
            map.WorldViewProj = map.World * Engine.Scene.ActiveCamera.ViewProj;
            map.ViewProj = Engine.Scene.ActiveCamera.ViewProj;

            device.IAPrimitiveTopology = IAPrimitive.TriangleStrip;

            foreach (var pass in effect.Passes())
            {
                effect.Apply(pass);
                device.Draw(0, 2);
            }
            effect.EndPasses();

            device.RasterizerStack.Pop();       

            UnBind(component);
        }
       
    }   

    [Registrator]
    public class BillBoardRegistrator : Registrator<BillBoard>
    {

        public override void RegisterRenders()
        {
            Register<SceneTechnique, BillBoardRender<BillboardEffect>>(() => new BillBoardRender<BillboardEffect>());
            Register<HitTestTechnique, BillBoardRender<RenderMeshIdEffect>>(() => new BillBoardRender<RenderMeshIdEffect>()
                    .BindWith(new IdBinding())                
            );

        }
    }

    //public class BillBoardRender : SceneNodeRender<BillboardEffect>
    //{
    //    EffectHandle hWorldViewProj;
    //    EffectHandle hColor;
    //    VertexBuffer vb;
    //    VertexDescriptor vd;

    //    /// <summary>
    //    /// P0----P1
    //    /// |   / |
    //    /// | /   |
    //    /// P3----P2
    //    /// </summary>
    //    public readonly static VertexPTx[] Vertices = new VertexPTx[]
    //        {
    //            new VertexPTx(-0.5f ,0.5f ,0 ,0 ,0), //P0
    //            new VertexPTx(0.5f ,0.5f ,0 ,1 ,0),  //P1
    //            new VertexPTx(-0.5f ,-0.5f ,0 ,0 ,1),//P3
    //            new VertexPTx(0.5f ,-0.5f ,0 ,1 ,1)  //P2
    //        };

    //    public VertexBuffer VertexBuffer { get { return vb; } }
    //    public EffectHandle WorldViewProjHandle { get { return hWorldViewProj; } }
    //    public EffectHandle ColorHandle { get { return hColor; } }
    //    public VertexDescriptor VertexDescriptor { get { return vd; } }

    //    public BillBoardRender()
    //    {
    //        vd = VertexDescriptor.GetDescriptorVertexDescriptor<VertexPTx>();
    //        vb = Engine.Graphics.CreateVertexBuffer<VertexPTx>(4, Usage.WriteOnly, VertexFormat.None, Pool.Managed);
    //        vb.SetData(Vertices);

    //        hWorldViewProj = effect.TryGetGlobalSematic(ShaderSemantics.WORLDVIEWPROJ);
    //        hColor = effect.TryGetGlobalSematic(ShaderSemantics.COLOR);
    //    }

    //    protected override ShaderEffect LoadEffect()
    //    {
    //        return ShaderStore.Billboard;
    //    }       

    //    protected override void OnRender(SceneNode target)
    //    {
    //        var billbordContainer = (BillBoardContainer)target;
    //        var camera = Engine.Scene.ActiveCamera;
    //        Matrix vp = camera.ViewProj;
    //        var wvp = billbordContainer.GlobalPose * vp;
    //        effect.SetValue(hWorldViewProj, wvp);
    //        var samples = Engine.RenderManager.Samplers;

    //        var samplerRegister = effect.Description.DiffuseSamplerRegister;
    //        var textures = billbordContainer.Textures;

    //        var device = Engine.Graphics;
    //        device.SetStreamSource(0, vb, 0, vd.Size);
    //        device.VertexDeclaration = vd.VertexDeclaration;
    //        Engine.RenderManager.SaveSamplerState(0);

    //        device.SetRenderState(RenderState.CullMode, (int)Cull.None);

    //        for (int i = 0; i < textures.Count; i++)
    //        {
    //            List<Billboar> billbords = billbordContainer.GetBillboards(i);
    //            if (billbords != null)
    //            {
    //                samples[samplerRegister].SetState(textures[i]);
    //                foreach (var b in billbords)
    //                {
    //                    effect.SetValue(hColor, b.Color);
    //                    effect.SetValue(hWorldViewProj, b.WorldMatrix * vp);

    //                    var d3dEffect = effect.D3DEffect;
    //                    int passes = d3dEffect.Begin();
    //                    for (int pass = 0; pass < passes; pass++)
    //                    {
    //                        d3dEffect.BeginPass(pass);
    //                        device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
    //                        d3dEffect.EndPass();
    //                    }
    //                    d3dEffect.End();
    //                }
    //            }

    //        }

    //        device.SetRenderState(RenderState.CullMode, (int)Engine.Shading.CullMode);
    //        Engine.RenderManager.RestoreSamplerState(0);
    //    }


    //}

}
