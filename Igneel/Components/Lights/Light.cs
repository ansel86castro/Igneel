using Igneel.Assets;


using Igneel.Graphics;
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    public enum LightType { None = 0, Directional = 1, Point = 2, Spot = 3 }
   
    //[SceneComponent(Name = "Light", Descrption = "Scene light")]
   
    public class Light : IAssetProvider, IEnabletable, INameable, IShadingInput
    {
        static int counter = 0;
        string name = "Light" + counter++;
        Color3 diffuse = new Color3(1);
        Color3 specular = new Color3(1);      
        bool enable;
        float intensity = 1f;
        float spotPower;
        float att0 = 1, att1, att2;
        private LightType type = LightType.Directional;    
        float effectiveRange = float.MaxValue;
        GlobalLigth ambient;
        bool isSync;

        private static LightInstance currentInstance;

        public event Action<Light> EnableChanged;
        public event Action<Light> EffectiveRangeChanged;      

        public Light()
        {
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }

        public Light(string name)           
        {
            this.name = name;
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }        

        [AssetMember]
        public GlobalLigth Ambient { get { return ambient; } set { ambient = value; isSync = false; } }

        [AssetMember]
        public string Name { get { return name; } set { name = value; } }

    
        
        [AssetMember]
        public float Intensity { get { return intensity; } set { intensity = value; isSync = false; } }       

    


        public Color3 Diffuse
        {
            get { return diffuse; }
            set { diffuse = value; isSync = false; }
        }

    
       
        
        [AssetMember]
        public Color3 Specular
        {
            get { return specular; }
            set { specular = value; isSync = false; }
        }

    
        [AssetMember]
        public bool Enable
        {
            get { return enable; }
            set
            {
                if (enable != value)
                {
                    enable = value;
                    if (EnableChanged != null)
                        EnableChanged(this);
                }
                isSync = false;
            }
        }

    
        [AssetMember]
        public LightType Type
        {
            get { return type; }
            set
            {
                type = value;
                isSync = false;
            }
        }

    
        
        [AssetMember]
        public float SpotPower
        {
            get { return spotPower; }
            set { spotPower = value; isSync = false; }
        }

        /// <summary>
        /// Constant attenuation.
        /// The Constant attenuation, Linear attenuation, and quadratic_attenuation are
        /// used to calculate the total attenuation of this light given a distance. The equation used is
        /// att = constant_attenuation + ( Dist * linear_attenuation ) + (( Dist^2 ) * quadratic_attenuation)
        /// </summary>
    
        
        [AssetMember]
        public float Attenuation0
        {
            get { return att0; }
            set { att0 = value; isSync = false; }
        }

        /// <summary>
        /// Linear attenuation
        /// The Constant attenuation, Linear attenuation, and quadratic_attenuation are
        /// used to calculate the total attenuation of this light given a distance. The equation used is
        /// att = constant_attenuation + ( Dist * linear_attenuation ) + (( Dist^2 ) * quadratic_attenuation)
        /// </summary>
    
        
        [AssetMember]
        public float Attenuation1
        {
            get { return att1; }
            set { att1 = value; isSync = false; }
        }

        /// <summary>
        /// Quadratic_attenuation
        /// The Constant attenuation, Linear attenuation, and quadratic_attenuation are
        /// used to calculate the total attenuation of this light given a distance. The equation used is
        /// att = constant_attenuation + ( Dist * linear_attenuation ) + (( Dist^2 ) * quadratic_attenuation)
        /// </summary>
    
        
        [AssetMember]
        public float Attenuation2
        {
            get { return att2; }
            set { att2 = value; isSync = false; }
        }

    
        
        [AssetMember]
        public float EffectiveRange
        {
            get { return effectiveRange; }
            set
            {
                if (effectiveRange != value)
                {
                    effectiveRange = value;
                    if (EffectiveRangeChanged != null)
                        EffectiveRangeChanged(this);
                }
                isSync = false;
            }
        }

        public static LightInstance Current
        {
            get { return currentInstance; }
            set { currentInstance = value; }
        }

        public static implicit operator ShaderLight(Light light)
        {
            ShaderLight li = new ShaderLight
            {
                Diffuse = light.diffuse,
                Specular = light.specular,
                Att = new Vector3(light.att0, light.att1, light.att2),
                Range = light.EffectiveRange,
                SpotPower = light.spotPower,                
                Type = (int)light.type,
                Intensity = light.intensity,
            };
            return li;
        }       

        public Asset CreateAsset()
        {
            return Asset.Create(this, name);
        }

        public bool IsGPUSync
        {
            get { return isSync && (ambient == null || ambient.IsGPUSync); }
            set 
            { 
                isSync = value;
                if (ambient != null)
                    ambient.IsGPUSync = value;
            }
        }

        public override string ToString()
        {
            return name ?? base.ToString();
        }

        public static SceneNode CreateNode(Light light, Vector3 position, Vector3 direction)
        {
            return CreateNode(light, position, Euler.FromDirection(direction));
        }

        public static SceneNode CreateNode(Light light, Vector3 position, Euler direction)
        {
            return Engine.Scene.Create("Light" + Engine.Scene.Lights.Count, new LightInstance(light),
                localPosition: position, localRotationEuler: direction);                                 
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ShaderLight
    {
        public Vector3 Pos;
        private float pad0;

        public Vector3 Dir;
        private float pad1;

        public Vector3 Diffuse;
        private float pad2;

        public Vector3 Specular;
        private float pad3;

        public Vector3 Att; // a0 a1,a2 attenuation factors
        public float SpotPower;
        public float Range;
        public int Type;
        public float Intensity;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ShaderDirectionalLight
    {
        public Vector3 Dir;
        public Vector3 Diffuse;
        public Vector3 Specular;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ShaderPointLight
    {
        public Vector3 Pos;
        public Vector3 Diffuse;
        public Vector3 Specular;
        public Vector3 Attenuation; // a0 a1,a2 attenuation factors      
        public float Range;
    }
}
