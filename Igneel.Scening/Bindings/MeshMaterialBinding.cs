
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering.Bindings
{
    public class MeshMaterialBinding:RenderBinding<MeshMaterial, IMeshMaterialMap>
    {      
                   
        public bool UseNormalMap;            
      
        public sealed override void OnBind(MeshMaterial value)
        {
            if (mapping == null) return;

            UseNormalMap = false;                                 
            var diffuse = value.DiffuseMap;
            var specular = value.SpecularMap;           
            var normal = value.NormalMap;

            UseNormalMap = normal != null;

            mapping.Surface = value.Surface;
            mapping.USE_DIFFUSE_MAP = diffuse != null;
            mapping.USE_SPECULAR_MAP = specular != null;          

            mapping.DiffuseMap = diffuse.ToSampler();
            mapping.SpecularMap = specular.ToSampler();
            mapping.NormalMap = normal.ToSampler();              
                                                    
        }

        public override void OnUnBind(MeshMaterial value)
        {          
            
        }
    }
}
