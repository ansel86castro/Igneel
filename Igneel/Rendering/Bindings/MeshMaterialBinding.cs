using Igneel.Components;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public class MeshMaterialBinding:RenderBinding<MeshMaterial, MeshMaterialBinding.IMaterialFlagsBinding>
    {      
        public interface IMaterialFlagsBinding
        {
            bool USE_DIFFUSE_MAP { get; set; }
            bool USE_SPECULAR_MAP { get; set; }                                         
            LayerSurface surface { get; set; }
        }

        public const int DiffuseSampler = 0;
        public const int SpecularSampler = 1;    
        public const int NormalSampler = 3;
        public const int ReflectionSampler = 5;
        public const int RefractionSampler = 6;

        ShaderStage stage;
        public bool UseNormalMap;
            
        public MeshMaterialBinding() 
        {
            stage = Engine.Graphics.PSStage;
        }
                 
        public sealed override void OnBind(MeshMaterial value)
        {
            if (mapping == null) return;
            UseNormalMap = false;

            var graphic = Engine.Graphics;
            for (int i = 0; i < 4; i++)
            {
                stage.SetSampler(i, SamplerState.Linear);
            }
            
        
            var device = Engine.Graphics;
        
            mapping.surface = value.Surface;            

            var diffuse = value.DiffuseMap;
            var specular = value.SpecularMap;           
            var normal = value.NormalMap;
          
            mapping.USE_DIFFUSE_MAP = false;                         
            mapping.USE_SPECULAR_MAP = false;

            stage.SetResource(DiffuseSampler, diffuse);
            mapping.USE_DIFFUSE_MAP = diffuse != null;

            stage.SetResource(SpecularSampler, specular);
            mapping.USE_SPECULAR_MAP = specular != null;

            if (normal != null)
            {
                stage.SetResource(NormalSampler, normal);
                UseNormalMap = true;
            }
            else
                UseNormalMap  = false;            
                                                    
        }

        public override void OnUnBind(MeshMaterial value)
        {          
            
        }
    }
}
