using System;
using Igneel.Assets;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Components
{
    public enum LightType { None = 0, Directional = 1, Point = 2, Spot = 3 }
      
    [Asset("LIGHT")]
    public class Light : Resource, IEnabletable, IRenderInput
    {
        static int _counter = 0;
        Color3 _diffuse = new Color3(1.0f);
        Color3 _specular = new Color3(1.0f);      
        bool _enable;
        float _intensity = 1f;
        float _spotPower;
        float _att0 = 1, _att1, _att2;
        private LightType _type = LightType.Directional;    
        float _effectiveRange = float.MaxValue;
        GlobalLigth _ambient;
        bool _isSync;       

        public event Action<Light> EnableChanged;
        public event Action<Light> EffectiveRangeChanged;

        public Light()
            : this("Light" + _counter++)
        {

        }

        public Light(string name)  
            :base(name, null)
        {            
         
        }        

        [AssetMember]
        public GlobalLigth Ambient { get { return _ambient; } set { _ambient = value; _isSync = false; } }
     
        [AssetMember]
        public float Intensity { get { return _intensity; } set { _intensity = value; _isSync = false; } }

        [AssetMember]
        public Color3 Diffuse
        {
            get { return _diffuse; }
            set { _diffuse = value; _isSync = false; }
        }           
        
        [AssetMember]
        public Color3 Specular
        {
            get { return _specular; }
            set { _specular = value; _isSync = false; }
        }
    
        [AssetMember]
        public bool Enable
        {
            get { return _enable; }
            set
            {
                if (_enable != value)
                {
                    _enable = value;
                    if (EnableChanged != null)
                        EnableChanged(this);
                }
                _isSync = false;
            }
        }
    
        [AssetMember]
        public LightType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                _isSync = false;
            }
        }
        
        [AssetMember]
        public float SpotPower
        {
            get { return _spotPower; }
            set { _spotPower = value; _isSync = false; }
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
            get { return _att0; }
            set { _att0 = value; _isSync = false; }
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
            get { return _att1; }
            set { _att1 = value; _isSync = false; }
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
            get { return _att2; }
            set { _att2 = value; _isSync = false; }
        }

    
        
        [AssetMember]
        public float EffectiveRange
        {
            get { return _effectiveRange; }
            set
            {
                if (_effectiveRange != value)
                {
                    _effectiveRange = value;
                    if (EffectiveRangeChanged != null)
                        EffectiveRangeChanged(this);
                }
                _isSync = false;
            }
        }

        //public static LightInstance Current
        //{
        //    get { return _currentInstance; }
        //    set { _currentInstance = value; }
        //}

        public static implicit operator ShaderLight(Light light)
        {
            ShaderLight li = new ShaderLight
            {
                Diffuse = light._diffuse,
                Specular = light._specular,
                Att = new Vector3(light._att0, light._att1, light._att2),
                Range = light.EffectiveRange,
                SpotPower = light._spotPower,                
                Type = (int)light._type,
                Intensity = light._intensity,
            };
            return li;
        }       
      
        public bool IsGpuSync
        {
            get { return _isSync && (_ambient == null || _ambient.IsGpuSync); }
            set 
            { 
                _isSync = value;
                if (_ambient != null)
                    _ambient.IsGpuSync = value;
            }
        }
     
        public static Frame CreateNode(Light light, Vector3 position, Vector3 direction)
        {
            return CreateNode(light, position, Euler.FromDirection(direction));
        }

        public static Frame CreateNode(Light light, Vector3 position, Euler direction)
        {
            return Engine.Scene.Create("Light" + Engine.Scene.FrameLights.Count, new FrameLight(light),
                localPosition: position, localRotationEuler: direction);                                 
        }

        protected override void OnDispose(bool disposing)
        {
            
        }

        public static FrameLight Current { get; set; }
    }

   
}
