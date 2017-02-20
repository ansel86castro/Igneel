using Igneel.Components.Terrain;
using Igneel.Graphics;
using Igneel.SceneComponents;

namespace Igneel.Rendering.Pendings
{
    public class HeightFieldRender<TEffect> : GraphicRender<HeightField, TEffect>
        where TEffect : Effect
    {
      
        public override void Draw(HeightField component)
        {
            var device = GraphicDeviceFactory.Device;

            device.PrimitiveTopology = IAPrimitive.TriangleStrip;
            device.SetVertexBuffer(0, component.PositionUvBuffer, 0, 16);         
            device.SetIndexBuffer(component.StripIndices, 0);

            var sections = component.VisibleSections;
            var materials = component.Materials;       

            var vertexCount = component.SectionVertexCount;
            var primitiveCount = component.SectionPrimitives;

            Bind(component);

            var effect = this.Effect;
            effect.OnRender(this);

            foreach (var section in component.VisibleSections)
            {
                device.SetVertexBuffer(1, section.NormalHeightVb, 0, 24);

                Bind(section);
                Bind(materials[section.MaterialIndex]);

                foreach (var pass in effect.Passes())
                {
                    effect.Apply(pass);
                    device.DrawIndexed(primitiveCount * 3, 0, 0);
                }

                effect.EndPasses();

                UnBind(materials[section.MaterialIndex]);
            }
        }
      
    }

    public class HeightFieldMaterialBind : RenderBinding<LayeredMaterial>
    {      

        public HeightFieldMaterialBind(Render render)
            : base(render)
        {

        }

        public override void OnBind(LayeredMaterial value)
        {       
            var effect = render.Effect;          
          
            var layers = value.DiffuseMaps;           

            for (int i = 0; i < layers.Length && i < 4; i++)
            {
                GraphicDeviceFactory.Device.PS.SetResource(i, value.DiffuseMaps[i]);
            }

            if (value.BlendFactors != null)
            {
                GraphicDeviceFactory.Device.PS.SetResource(4, value.BlendFactors);
            }
        }

        public override void OnUnBind(LayeredMaterial value)
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
