using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public class ShapeRender<T>where T:struct
    {
        private VertexDescriptor vd;     
        private Vector4 color;          

        public Vector4 Color
        {
            get { return color; }
            set { color = value; }
        }
      
        public float Distance { get; set; }

        public bool UseShapeColor { get; set; }

        public bool KeepDistance { get; set; }

        public ShapeRender()
        {                     
            vd = VertexDescriptor.GetDescriptor<MeshVertex>();
            color = new Vector4(1, 1, 1, 1);
            Distance = 0.5f;
        }              

        public void Render(ShapeBuilder<T> shape, Matrix world)
        {
            if (Engine.Scene == null || Engine.Scene.ActiveCamera == null)
                return;

            //var device = Engine.Graphics;
            //device.VertexDeclaration = vd.VertexDeclaration;
            //var _fill = device.GetRenderState(RenderState.FillMode);
            //Engine.Graphics.SetRenderState(RenderState.FillMode, fill);

            //effect.Technique = technique;
            //effect.SetViewProjMatrix(Engine.Scene.ActiveCamera.ViewProj);
            //effect.SetWorldMatrix(world);            

            //effect.SetValue(hColor, UseShapeColor?shape.Color:color);

            //effect.Apply(() =>
            // {
            //     if (shape.Indices != null)
            //     {
            //         device.DrawIndexedUserPrimitives(shape.PrimitiveType, 0, shape.Vertices.Length,
            //                            shape.Indices.Length / GetPrimitiveSize(shape.PrimitiveType), shape.Indices,
            //                            Format.Index16, shape.Vertices, vd.Size);
            //     }
            //     else
            //         device.DrawUserPrimitives(shape.PrimitiveType, shape.Vertices.Length / GetPrimitiveSize(shape.PrimitiveType), shape.Vertices);  
            // });

            //Engine.Graphics.SetRenderState(RenderState.FillMode, _fill);
        }

        public void Render(ShapeBuilder<T> shape, Matrix world, ShapeLayer layer)
        {
            if (Engine.Scene == null || Engine.Scene.ActiveCamera == null)
                return;

            //var device = Engine.Graphics;
            //device.VertexDeclaration = vd.VertexDeclaration;
            //var _fill = device.GetRenderState(RenderState.FillMode);
            //Engine.Graphics.SetRenderState(RenderState.FillMode, fill);

            //effect.Technique = technique;
            //effect.SetViewProjMatrix(Engine.Scene.ActiveCamera.ViewProj);
            //effect.SetWorldMatrix(world);
            //effect.SetValue(hColor, UseShapeColor ? shape.Color : color);

            //effect.Apply(() =>
            //  {
            //      if (shape.Indices != null)
            //          device.DrawIndexedUserPrimitives(shape.PrimitiveType, layer.StartIndex, layer.StartVertex,
            //                            layer.VertexCount, layer.PrimitiveCount, shape.Indices, Format.Index16, shape.Vertices, vd.Size);                

            //  });

            //Engine.Graphics.SetRenderState(RenderState.FillMode, _fill);
        }

        public void Render(ComposedShape<T> composeShape, Matrix world)
        {
            if (Engine.Scene == null || Engine.Scene.ActiveCamera == null)
                return;

            //var device = Engine.Graphics;
            //device.VertexDeclaration = vd.VertexDeclaration;
            //var _fill = device.GetRenderState(RenderState.FillMode);
            //Engine.Graphics.SetRenderState(RenderState.FillMode, fill);
            
            //if (KeepDistance)
            //{
            //    effect.Technique = "Technique4";
            //    effect.SetValueByName("distance", Distance);
            //    effect.SetViewMatrix(Engine.Scene.ActiveCamera.View);
            //    effect.SetValueByName("Proj", Engine.Scene.ActiveCamera.Projection);
            //}
            //else
            //{
            //    effect.Technique = technique;
            //    effect.SetViewProjMatrix(Engine.Scene.ActiveCamera.ViewProj);
            //}

            //effect.SetValue(hColor, color);            

            //RenderCompose(composeShape, world, device);

            //Engine.Graphics.SetRenderState(RenderState.FillMode, _fill);
        }

        private void RenderCompose(ComposedShape<T> composeShape, Matrix world, GraphicDevice device)
        {
            //if (composeShape.Shapes != null)
            //{
            //    for (int i = 0; i < composeShape.Shapes.Length; i++)
            //    {
            //        ShapeBuilder<T> shape = composeShape.Shapes[i];
            //        var matrix = composeShape.Transforms[i];
            //        effect.SetWorldMatrix(matrix * world);
            //        if (composeShape.Colors != null)
            //            effect.SetValue(hColor, composeShape.Colors[i]);

            //        effect.Apply(() =>
            //          {
            //              if (shape.Indices != null)
            //              {
            //                  device.DrawIndexedUserPrimitives(shape.PrimitiveType, 0, shape.Vertices.Length,
            //                                     shape.Indices.Length / GetPrimitiveSize(shape.PrimitiveType), shape.Indices, 
            //                                     Format.Index16, shape.Vertices, vd.Size);
            //              }                          
            //              else
            //                 device.DrawUserPrimitives(shape.PrimitiveType, shape.Vertices.Length / GetPrimitiveSize(shape.PrimitiveType), shape.Vertices);                                                        
            //          });
            //    }
            //}
            //else if (composeShape.Components != null)
            //{
            //    for (int i = 0; i < composeShape.Components.Length; i++)
            //    {
            //        var component = composeShape.Components[i];
            //        var matrix = composeShape.Transforms[i];
            //        if (composeShape.Colors != null)
            //            effect.SetValue(hColor, composeShape.Colors[i]);
            //        RenderCompose(component, matrix * world, device);
            //    }
            //}
        }

        private int GetPrimitiveSize(IAPrimitive primitiveType)
        {
            switch (primitiveType)
            {
                case  IAPrimitive.LineList:
                case IAPrimitive.LineStrip:
                    return 2;
                case IAPrimitive.PointList:
                    return 1;
                default:         
                    return 3;
            }           
        }
      
    }
}
