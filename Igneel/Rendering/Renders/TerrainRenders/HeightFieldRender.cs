using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Igneel.Components;

namespace Igneel.Rendering
{
    public class HeightFieldRender<TShader> : GraphicObjectRender<TShader, HeightField>
        where TShader : Effect
    {
      
        public override void Draw(HeightField component)
        {
            //var device = Engine.Graphics;
            //var rm = Engine.RenderManager;                     

            //device.VertexDeclaration = component.VertDescriptor.VertexDeclaration;
            //device.SetStreamSource(0, component.PositionUVBuffer, 0, 16);
            //device.Indices = component.StripIndices;

            //var sections = component.VisibleSections;
            //var materials = component.Materials;
            //var _effect = effect.D3DEffect;
            //var matBinding = GetBinding<TerrainMaterial>();

            //var vertexCount = component.SectionVertexCount;
            //var primitiveCount = component.SectionPrimitives;

            //Bind(component);

            //foreach (var section in component.VisibleSections)
            //{
            //    device.SetStreamSource(1, section.NormalHeightVb, 0, 24);

            //    if (offset == null)
            //        offset = effect.TryGetGlobalParameter("offset");

            //    if(offset!=null)
            //        effect.SetValue(offset, section.Offset);

            //    if (matBinding != null)
            //        matBinding.Bind(materials[section.MaterialIndex]);

            //    var passes = _effect.Begin(0);
            //    for (int pass = 0; pass < passes; pass++)
            //    {
            //        _effect.BeginPass(pass);

            //        device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, vertexCount, 0, primitiveCount);

            //        _effect.EndPass();
            //    }
            //    _effect.End();

            //    if (matBinding != null)
            //        matBinding.UnBind(materials[section.MaterialIndex]);
            //}
        }
      
    }

    public class HeightFieldMaterialBind : RenderBinding<TerrainMaterial>
    {      

        public HeightFieldMaterialBind(Render render)
            : base(render)
        {

        }

        public override void OnBind(TerrainMaterial value)
        {       
            var effect = render.Effect;          
          
            var layers = value.Layers;           

            for (int i = 0; i < layers.Length && i < 4; i++)
            {
                Engine.Graphics.PS.SetResource(i, value.Layers[i]);
            }

            if (value.BlendLayer != null)
            {
                Engine.Graphics.PS.SetResource(4, value.BlendLayer);
            }
        }

        public override void OnUnBind(TerrainMaterial value)
        {
           
        }
    }

    public class HeightFieldBind : RenderBinding<HeightField>
    {
       
        public HeightFieldBind(Render render)
            : base(render)
        {
          
        }

        public override void OnBind(HeightField value)
        {
           
        }

        public override void OnUnBind(HeightField value)
        {
           
        }
    }
}
