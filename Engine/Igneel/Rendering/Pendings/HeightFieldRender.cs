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
            //var device = GraphicDeviceFactory.Device;
            //var rm = Engine.RenderManager;                     

            //device.VertexDeclaration = component.VertDescriptor.VertexDeclaration;
            //device.SetStreamSource(0, component.PositionUVBuffer, 0, 16);
            //device.Indices = component.StripIndices;

            //var sections = component.VisibleSections;
            //var materials = component.Materials;
            //var _effect = effect.D3DEffect;
            //var matBinding = GetBinding<LayeredMaterial>();

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
