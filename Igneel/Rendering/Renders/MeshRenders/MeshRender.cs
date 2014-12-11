﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Igneel.Graphics;
using Igneel.Components;

namespace Igneel.Rendering
{   

    /// <summary>
    /// A Render Technique that support all supported by the GeometryRenderTechnique and also
    /// textures as  diffuse, normal , cube enviroment and a SurfaceInfo. This techniques is the base of 
    /// technique that render Model3D objects
    /// </summary>
    public class MeshRender<TShader, T> :  GraphicObjectRender<TShader, T>
        where T : IMeshContainer, IMaterialContainer
        where TShader : Effect, new()
    {

        public MeshRender()
        {
                        
        }

        public override void Draw(T component)
        {
            var mesh = component.Mesh;
           
            var device = Engine.Graphics;

            device.IAPrimitiveTopology = IAPrimitive.TriangleList;
            device.IASetVertexBuffer(0, mesh.VertexBuffer, 0);
            device.IASetIndexBuffer(mesh.IndexBuffer, 0);

            var materials = component.Materials;           
           

            Bind(component);

            if (Clipping == PixelClipping.Opaque)
            {
                var transparents = component.TransparentMaterials;
                for (int i = 0, len = transparents.Length; i < len; i++)
                {
                    Bind(materials[transparents[i]]);                   
                    RenderLayers(device, mesh.GetLayersByMaterial(transparents[i]));
                }

            }
            else
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    Bind(materials[i]);                   
                    RenderLayers(device, mesh.GetLayersByMaterial(i));
                }
            }

            //UnBind(component);

        }

        private void RenderLayers(GraphicDevice device, MeshPart[] layers)
        {
            var effect = Effect;
            effect.OnRender(this);
            foreach (var pass in effect.Passes())
            {
                effect.Apply(pass);
                foreach (var layer in layers)
                {                   
                    Bind(layer);

                    device.DrawIndexed(layer.primitiveCount * 3 , layer.startIndex, 0);
                    //device.DrawIndexed(0, layer.startVertex, layer.vertexCount, layer.startIndex, layer.primitiveCount);
                }
            }
            effect.EndPasses();           
        }
              
    }    

}
