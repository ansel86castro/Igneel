using Igneel.Components;
using Igneel.Graphics;
using Igneel.SceneComponents;

namespace Igneel.Rendering
{   

    /// <summary>
    /// A Render Technique that support all supported by the GeometryRenderTechnique and also
    /// textures as  diffuse, normal , cube enviroment and a SurfaceInfo. This techniques is the base of 
    /// technique that render Model3D objects
    /// </summary>
    public class FrameMeshRender<TEffect> : GraphicRender<FrameMesh, TEffect>     
        where TEffect : Effect
    {

        public override void Draw(FrameMesh component)
        {
            var mesh = component.Mesh;
           
            var device = GraphicDeviceFactory.Device;

            device.PrimitiveTopology = IAPrimitive.TriangleList;
            device.SetVertexBuffer(0, mesh.VertexBuffer, 0);
            device.SetIndexBuffer(mesh.IndexBuffer, 0);

            var materials = component.Materials;                               

            if (Clipping == PixelClipping.Opaque)
            {
                var transparents = component.TransparentMaterials;
                for (int i = 0, len = transparents.Length; i < len; i++)
                {
                    Bind(materials[transparents[i]]);
                    Bind<IVisualMaterial>(materials[transparents[i]]);
                    RenderLayers(device, mesh.GetLayersByMaterial(transparents[i]));
                }

            }
            else
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    Bind(materials[i]);
                    Bind<IVisualMaterial>(materials[i]);
                    RenderLayers(device, mesh.GetLayersByMaterial(i));
                }
            }

            //UnBind(component);

        }

        private void RenderLayers(GraphicDevice device, MeshPart[] layers)
        {
            var effect = this.Effect;
            effect.OnRender(this);

            foreach (var pass in effect.Passes())
            {
                effect.Apply(pass);
                foreach (var layer in layers)
                {                                      
                    device.DrawIndexed(layer.primitiveCount * 3 , layer.startIndex, 0);                    
                }
            }
            effect.EndPasses();           
        }
              
    }    

}
