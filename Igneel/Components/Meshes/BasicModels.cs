using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Igneel.Rendering;
using Igneel.Collections;
using Igneel.Design;
using System.ComponentModel;
using Igneel.Graphics;

namespace Igneel.Components
{
   
    public static class Models
    {        
        //public static MeshNode Sphere(string name, Vector3 position, int stacks, int slices, int radius, LayerSurface surface)
        //{
        //    return new MeshNode(name, CreateMesh(new SphereBuilder(stacks, slices, radius), surface),null)
        //        .Translated(position);                
        //}

        //public static MeshNode Box(string name, Vector3 position, float width, float height, float depth, LayerSurface surface)
        //{
        //    return new MeshNode(name, CreateMesh(new BoxBuilder(width, height, depth), surface), null)
        //    {
        //        GlobalPosition = position
        //    };         
        //}

        //public static Mesh CreateMesh<T>(ShapeBuilder<T> builder, LayerSurface surface)
        //     where T : struct
        //{
        //    Mesh mesh = new Mesh(VertexDescriptor.GetDescriptorVertexDescriptor<ModelVertex>());
        //    int index = Engine.Scene.Materials.Count + 1;
        //    Engine.Scene.Materials.Add(new MeshMaterial { Name = "default_" + index, Surface = surface });
        //    mesh.CreateVertexBuffer<T>(builder.Vertices);
        //    mesh.CreateIndexBuffer(builder.Indices);

        //    MeshPart layer = new MeshPart();
        //    layer.materialIndex = 0;
        //    layer.startIndex = 0;
        //    layer.primitiveCount = builder.Indices.Length / 3;
        //    layer.startVertex = 0;
        //    layer.vertexCount = builder.Vertices.Length;

        //    mesh.SetLayers(new MeshPart[] { layer });

        //    return mesh;
        //}
       
    }

  
}
