using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Igneel.Graphics;

namespace Igneel.Rendering
{
    public class Sprite
    {
        public interface IShaderInput
        {
            Matrix Transform {get;set;}
            Matrix TexTransform { get; set; }
        }

        GraphicBuffer vb;
        DepthStencilState state;
        private DepthStencilState oldState;
        private ViewPort vp;

        public Sprite()
            : this(new RectangleF(0, 0, 1, 1))
        {

        }

        public Sprite(RectangleF rect)
        {
            var device = Engine.Graphics;
            var viewport = device.RSViewPort;          

            var vertexes = new VertexPTxH[4];

            vertexes[0].Position = new Vector4(rect.X, rect.Y, 0.5f, 1.0f);
            vertexes[0].TexCoord = new Vector2(0, 0);

            vertexes[1].Position = new Vector4(rect.X + rect.Width, rect.Y, 0.5f, 1.0f);
            vertexes[1].TexCoord = new Vector2(1, 0);

            vertexes[2].Position = new Vector4(rect.X, rect.Y + rect.Height, 0.5f, 1.0f);
            vertexes[2].TexCoord = new Vector2(0, 1);

            vertexes[3].Position = new Vector4(rect.X + rect.Width, rect.Y + rect.Height, 0.5f, 1.0f);
            vertexes[3].TexCoord = new Vector2(1, 1);

            vb = device.CreateVertexBuffer(usage:ResourceUsage.Immutable, cpuAcces: CpuAccessFlags.None , data:vertexes);
            state = device.CreateDepthStencilState(new DepthStencilStateDesc(true)
            {
                DepthEnable = false,
                WriteEnable = false
            });         
         
        }
     
        public void Begin(bool depthWrite = false)
        {
            var device = Engine.Graphics;

            oldState = device.OMDepthStencilState;

            if (!depthWrite)
            {              
                device.OMDepthStencilState = state;
            }
            
            device.IAPrimitiveTopology = IAPrimitive.TriangleStrip;
            var rt = device.OMGetRenderTarget(0);
            vp = new ViewPort(0, 0, rt.Width, rt.Height);

            device.IASetVertexBuffer(0, vb, 0);            
        }

        public void SetTrasform(IShaderInput input, Rectangle rec , Matrix textureTransform)
        {
            var rt = Engine.Graphics.OMGetRenderTarget(0);

            var transform = new Matrix(rec.Width, 0, 0, 0,
                                    0, rec.Height, 0, 0,
                                    0, 0, 1, 0,
                                    rec.X, rec.Y, 0, 1) *
                        Matrix.Scale(1f / (float)rt.Width, 1f / (float)rt.Height, 1) *
                        new Matrix(2, 0, 0, 0,
                                   0, -2, 0, 0,
                                   0, 0, 1, 0,
                                   -1, 1, 0, 1);

            input.Transform = transform;
            input.TexTransform = textureTransform;
        }

        public void SetTrasform(Effect effect, Rectangle rec, Matrix textureTransform)
        {
            var rt = Engine.Graphics.OMGetRenderTarget(0);

            var transform = new Matrix(rec.Width, 0, 0, 0,
                                    0, rec.Height, 0, 0,
                                    0, 0, 1, 0,
                                    rec.X, rec.Y, 0, 1) *
                        Matrix.Scale(1f / (float)rt.Width, 1f / (float)rt.Height, 1) *
                        new Matrix(2, 0, 0, 0,
                                   0, -2, 0, 0,
                                   0, 0, 1, 0,
                                   -1, 1, 0, 1);

            effect.U.Transform = transform;
            effect.U.TexTransform = textureTransform;
        }

        public void SetFullScreenTransform(IShaderInput input, Matrix texture)
        {
            input.Transform = new Matrix(2, 0, 0, 0,
                                        0, -2, 0, 0,
                                        0, 0, 1, 0,
                                        -1, 1, 0, 1);
            input.TexTransform = texture;
        }

        public void SetFullScreenTransform(IShaderInput input)
        {
            SetFullScreenTransform(input, Matrix.Identity);
        }
        public void SetFullScreenTransform(Effect effect, Matrix texture)
        {
            effect.U.Transform = new Matrix(2, 0, 0, 0,
                                         0, -2, 0, 0,
                                         0, 0, 1, 0,
                                         -1, 1, 0, 1);
            effect.U.TexTransform = texture;
        }
        public void SetFullScreenTransform(Effect effect)
        {
            effect.U.Transform = new Matrix(2, 0, 0, 0,
                                         0, -2, 0, 0,
                                         0, 0, 1, 0,
                                         -1, 1, 0, 1);

            effect.U.TexTransform = Matrix.Identity;
        }

        public void DrawQuad(Effect effect = null)
        {
            var device = Engine.Graphics;
            var rt = device.OMGetRenderTarget(0);
            var oldvp = device.RSViewPort;

            vp.TopLeftX = 0;
            vp.TopLeftY = 0;
            vp.Width = rt.Width;
            vp.Height = rt.Height;
            device.RSViewPort = vp;

            if (effect != null)
            {
                foreach (var pass in effect.Passes())
                {
                    effect.Apply(pass);

                    device.Draw(4, 0);
                }
                effect.EndPasses();
            }
            else
            {
                device.Draw(4, 0);
            }

            device.RSViewPort = oldvp;
        }

        public void DrawQuad(int x, int y, int width, int height, Effect effect = null)
        {
            var device = Engine.Graphics;            
            var oldvp = device.RSViewPort;

            vp.TopLeftX = x;
            vp.TopLeftY = y;
            vp.Width = width;
            vp.Height = height;
            device.RSViewPort = vp;

            if (effect != null)
            {
                foreach (var pass in effect.Passes())
                {
                    effect.Apply(pass);

                    device.Draw(4, 0);
                }
                effect.EndPasses();
            }
            else
            {
                device.Draw(4, 0);
            }

            device.RSViewPort = oldvp;
        }        

        public void End()
        {
            Engine.Graphics.OMDepthStencilState = oldState;
        }

    }
    //public class QuadRender 
    //{
    //    internal protected VertexPTxH[] vertexes;
    //    internal protected VertexDescriptor vd;
    //    internal protected RectangleF rect;
    //    internal protected GraphicBuffer vb;
    //    internal protected DepthStencilState state;      

    //    public QuadRender()
    //    {
    //        var device = Engine.Graphics;
    //        vertexes = new VertexPTxH[4];
    //        vd = VertexDescriptor.GetDescriptor<VertexPTxH>();
    //        var viewport = device.RSViewPort;

    //        rect = new RectangleF(-0.5f, -0.5f, viewport.Width, viewport.Height);

    //        vertexes[0].Position = new Vector4(rect.X, rect.Y, 0.5f, 1.0f);
    //        vertexes[0].TexCoord = new Vector2(0, 0);

    //        vertexes[1].Position = new Vector4(rect.X + rect.Width, rect.Y, 0.5f, 1.0f);
    //        vertexes[1].TexCoord = new Vector2(1, 0);

    //        vertexes[2].Position = new Vector4(rect.X, rect.Y + rect.Height, 0.5f, 1.0f);
    //        vertexes[2].TexCoord = new Vector2(0, 1);

    //        vertexes[3].Position = new Vector4(rect.X + rect.Width, rect.Y + rect.Height, 0.5f, 1.0f);
    //        vertexes[3].TexCoord = new Vector2(1, 1);

    //        vb = device.CreateVertexBuffer(4 * vd.Size, vd.Size, ResourceUsage.Default, MapType.Write_Discard, vertexes);

    //        DepthStencilDesc desc = Engine.Graphics.OMDepthStencilState.State;
    //        desc.DepthEnable = false;
    //        desc.WriteEnable = false;
    //        state = device.CreateDepthStencilState(desc);            
    //    }             

    //    public RectangleF QuadCoords
    //    {
    //        get
    //        {
    //            return rect;
    //        }
    //        set
    //        {
    //            rect = value;
    //            vertexes[0].Position = new Vector4(rect.X, rect.Y, 0.5f, 1.0f);
    //            vertexes[1].Position = new Vector4(rect.X + rect.Width, rect.Y, 0.5f, 1.0f);
    //            vertexes[2].Position = new Vector4(rect.X, rect.Y + rect.Height, 0.5f, 1.0f);
    //            vertexes[3].Position = new Vector4(rect.X + rect.Width, rect.Y + rect.Height, 0.5f, 1.0f);
    //            vb.Write(vertexes, 0);
    //        }
    //    }             

    //    public void SetTextureCoord(Vector2 leftTop, Vector2 rightBottom)
    //    {
    //        vertexes[0].TexCoord = leftTop;
    //        vertexes[1].TexCoord = new Vector2(rightBottom.X, leftTop.Y);
    //        vertexes[2].TexCoord = new Vector2(leftTop.X, rightBottom.Y);
    //        vertexes[3].TexCoord = rightBottom;
    //        vb.Write(vertexes, 0);
    //    }       

    //    public void SetFullDrawTexture(Texture2D texture)
    //    {
    //        var width = texture.Width;
    //        var height = texture.Height;
    //        rect = new RectangleF(-0.5f, -0.5f, width, height);

    //        vertexes[0].Position = new Vector4(rect.X, rect.Y, 0.5f, 1.0f);
    //        vertexes[0].TexCoord = new Vector2(0, 0);

    //        vertexes[1].Position = new Vector4(rect.X + rect.Width, rect.Y, 0.5f, 1.0f);
    //        vertexes[1].TexCoord = new Vector2(1, 0);

    //        vertexes[2].Position = new Vector4(rect.X, rect.Y + rect.Height, 0.5f, 1.0f);
    //        vertexes[2].TexCoord = new Vector2(0, 1);

    //        vertexes[3].Position = new Vector4(rect.X + rect.Width, rect.Y + rect.Height, 0.5f, 1.0f);
    //        vertexes[3].TexCoord = new Vector2(1, 1);

    //        vb.Write(vertexes, 0);

    //    }

    //    public void Draw(int x, int y, int width, int height)
    //    {
    //        rect = new RectangleF(x - 0.5f, y -0.5f, width, height);

    //        vertexes[0].Position = new Vector4(rect.X, rect.Y, 0.5f, 1.0f);
    //        vertexes[0].TexCoord = new Vector2(0, 0);

    //        vertexes[1].Position = new Vector4(rect.X + rect.Width, rect.Y, 0.5f, 1.0f);
    //        vertexes[1].TexCoord = new Vector2(1, 0);

    //        vertexes[2].Position = new Vector4(rect.X, rect.Y + rect.Height, 0.5f, 1.0f);
    //        vertexes[2].TexCoord = new Vector2(0, 1);

    //        vertexes[3].Position = new Vector4(rect.X + rect.Width, rect.Y + rect.Height, 0.5f, 1.0f);
    //        vertexes[3].TexCoord = new Vector2(1, 1);
         
    //       var device = Engine.Graphics;
    //       device.PushPipelineState(state);

    //       for (int i = 0; i < ; i++)
    //       {
               
    //       }
    //        var d3dEffect = effect.D3DEffect;
    //        int passes = d3dEffect.Begin();
    //        for (int i = 0; i < passes; i++)
    //        {
    //            d3dEffect.BeginPass(i);
    //            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 0, 2, vertexes);
    //            d3dEffect.EndPass();
    //        }
    //        d3dEffect.End();

    //        device.SetRenderState(RenderState.ZEnable, zenable);
    //        device.SetRenderState(RenderState.ZWriteEnable, zwrite);
    //    }

    //    public void Draw(int width ,int height)
    //    {
    //        rect = new RectangleF(-0.5f, -0.5f, width, height);

    //        vertexes[0].Position = new Vector4(rect.X, rect.Y, 0.5f, 1.0f);
    //        vertexes[0].TexCoord = new Vector2(0, 0);

    //        vertexes[1].Position = new Vector4(rect.X + rect.Width, rect.Y, 0.5f, 1.0f);
    //        vertexes[1].TexCoord = new Vector2(1, 0);

    //        vertexes[2].Position = new Vector4(rect.X, rect.Y + rect.Height, 0.5f, 1.0f);
    //        vertexes[2].TexCoord = new Vector2(0, 1);

    //        vertexes[3].Position = new Vector4(rect.X + rect.Width, rect.Y + rect.Height, 0.5f, 1.0f);
    //        vertexes[3].TexCoord = new Vector2(1, 1);

    //        //vb.SetData(vertexes, 0, 0);

    //        var zenable = device.GetRenderState<bool>(RenderState.ZEnable);
    //        var zwrite = device.GetRenderState<bool>(RenderState.ZWriteEnable);

    //        device.SetRenderState(RenderState.ZEnable , false);
    //        device.SetRenderState(RenderState.ZWriteEnable , false);
    //        device.VertexDeclaration = vd.VertexDeclaration;
    //        //device.SetStreamSource(0, vb, 0, vd.Size);

    //        var d3dEffect = effect.D3DEffect;
    //        int passes = d3dEffect.Begin();
    //        for (int i = 0; i < passes; i++)
    //        {
    //            d3dEffect.BeginPass(i);
    //            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 0, 2, vertexes);
    //            d3dEffect.EndPass();
    //        }
    //        d3dEffect.End();

    //        device.SetRenderState(RenderState.ZEnable, zenable);
    //        device.SetRenderState(RenderState.ZWriteEnable, zwrite);
    //    }

    //    public void DrawToTexture(Texture renderTarget)
    //    {
    //        var sd = renderTarget.GetLevelDescription(0);
    //        Draw(sd.Width, sd.Height);
    //    }

    //    public virtual void DrawQuad()
    //    {
    //        if (effect != null)
    //        {
    //            var zenable = device.GetRenderState<bool>(RenderState.ZEnable);
    //            device.SetRenderState(RenderState.ZEnable, false);
    //            device.VertexDeclaration = vd.VertexDeclaration;
    //            device.SetStreamSource(0, vb, 0,vd.Size);

    //            effect.Apply(() => device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2));

    //            device.SetRenderState(RenderState.ZEnable, zenable);
    //        }
    //    }
       
    //    protected override void OnDispose()
    //    {
    //        vd.Dispose();
    //        vb.Dispose();
    //        base.OnDispose();
    //    }
    //}

    //public class SpriteRender : QuadRender
    //{
    //    internal protected Texture texture;
    //    internal Rectangle srcRect;
    //    internal protected int width;
    //    internal protected int height;
    //    internal protected int samplerRegister = -1;
    //    internal protected string paramName;
    //    internal protected string semantic;
    //    internal protected Action<ShaderEffect> constantSetter;

    //    public SpriteRender() { }
    //    public SpriteRender(Device device) : base(device) { }

    //    public Action<ShaderEffect> ConstantSetter { get { return constantSetter; } set { constantSetter = value; } }        
        
    //    public Texture Texture
    //    {
    //        get
    //        {
    //            return texture;
    //        }
    //        set
    //        {
    //            texture = value;
    //            var sd = texture.GetLevelDescription(0);
    //            width = sd.Width;
    //            height = sd.Height;
    //            srcRect = new Rectangle(0, 0, width, height);
    //        }
    //    }

    //    public Rectangle Rectangle
    //    {
    //        get { return srcRect; }
    //        set
    //        {
    //            srcRect = value;
    //            if (texture != null)
    //            {
    //                Vector2 leftTop = new Vector2((float)srcRect.X / (float)width, (float)srcRect.Y / (float)height);
    //                Vector2 rightBottom = new Vector2((float)srcRect.Right / (float)width, (float)srcRect.Bottom / (float)height);
    //                SetTextureCoord(leftTop, rightBottom);
    //            }
    //        }
    //    }

    //    public int SamplerRegister
    //    {
    //        get { return samplerRegister; }
    //        set { samplerRegister = value; }
    //    }

    //    public string TextureParameter
    //    {
    //        get { return paramName; }
    //        set { paramName = value; }
    //    }

    //    public string TextureSemantic
    //    {
    //        get { return semantic; }
    //        set { semantic = value; }
    //    }

    //    public override void DrawQuad()
    //    {
    //        if (Effect != null)
    //        {               
    //            if (samplerRegister >= 0)
    //                device.SetTexture(samplerRegister, texture);
    //            else if (paramName != null)
    //                Effect.SetTextureByName(paramName, texture);
    //            else if (semantic != null)
    //                Effect.SetValueBySemantic(semantic, texture);

    //            if (constantSetter != null)
    //                constantSetter(Effect);

    //            base.DrawQuad();
    //        }
    //    }
       
    //}

    //public class DefaultSpriteRender : SpriteRender
    //{
    //    float alpha = 1;
    //    EffectHandle technique1; // depth 
    //    EffectHandle technique2; //normal
    //    EffectHandle technique3;

    //    public DefaultSpriteRender(Device device)
    //        :base(device)
    //    {
    //        Effect = Effect.GetEffect("Shaders_Untransformed.fxo");
    //        technique1 = Effect.GetTechique("Technique1");
    //        technique2 = Effect.GetTechique("Technique2");
    //        technique3 = Effect.GetTechique("Technique3");
    //        samplerRegister = 0;
    //        paramName = null;
    //        semantic = null; 
    //    }

    //    public DefaultSpriteRender()
    //    {
    //        Effect = Effect.GetEffect("Shaders_Untransformed.fxo");
    //        technique1 = Effect.GetTechique("Technique1");
    //        technique2 = Effect.GetTechique("Technique2");
    //        technique3 = Effect.GetTechique("Technique3");
    //        samplerRegister = 0;
    //        paramName = null;
    //        semantic = null;
    //    }       

    //    public float Alpha
    //    {
    //        get { return alpha; }
    //        set { alpha = value; }
    //    }

    //    public bool UseAlpha { get; set; }

    //    public override void DrawQuad()
    //    {
    //        if (texture != null)
    //        {
    //            var td = texture.GetLevelDescription(0);
    //            if(td.Format == Format.R32F || td.Format == Format.R16F)
    //                Effect.Technique = technique1;
    //            else if (UseAlpha)
    //                Effect.Technique = technique3;
    //            else
    //                Effect.Technique = technique2;

    //            base.DrawQuad();
    //        }
    //    }
      
    //}
}
