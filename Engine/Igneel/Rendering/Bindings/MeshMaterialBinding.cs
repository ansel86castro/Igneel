using Igneel.Components;
using Igneel.Graphics;

namespace Igneel.Rendering.Bindings
{
    public class MeshMaterialBinding:RenderBinding<BasicMaterial, IBasicMaterialMap>
    {      
                   
        public bool UseNormalMap;            
      
        public sealed override void OnBind(BasicMaterial value)
        {
            if (Mapping == null) return;

            UseNormalMap = false;                                 
            var diffuse = value.DiffuseMap;
            var specular = value.SpecularMap;           
            var normal = value.NormalMap;

            UseNormalMap = normal != null;

            Mapping.Surface = value.Surface;
            Mapping.USE_DIFFUSE_MAP = diffuse != null;
            Mapping.USE_SPECULAR_MAP = specular != null;          

            Mapping.DiffuseMap = diffuse.ToSampler();
            Mapping.SpecularMap = specular.ToSampler();
            Mapping.NormalMap = normal.ToSampler();              
                                                    
        }

        public override void OnUnBind(BasicMaterial value)
        {          
            
        }
    }
}
