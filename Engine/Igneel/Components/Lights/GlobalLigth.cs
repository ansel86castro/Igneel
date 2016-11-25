using System;

namespace Igneel.Components
{
    [Serializable]
    
    public class GlobalLigth: IRenderInput
    {
        Color3 _globalAmbient = new Color3(0.2f, 0.2f, 0.2f);
        Color3 _skyColor = new Color3(0.5f, 0.5f, 0.5f);
        Color3 _grountColor = new Color3(0, 0, 0);
        Vector3 _northPole = new Vector3(0, 1, 0);
        float _globalIntensity = 1.0f;
        private bool _shaderInputValid;



        public Color3 SkyColor
        {
            get { return _skyColor; }
            set 
            {
                _shaderInputValid = false;
                _skyColor = value; 
            }
        }



        public Color3 GroundColor
        {
            get { return _grountColor; }
            set
            {
                _grountColor = value;
                _shaderInputValid = false;
            }
        }



        public Color3 GlobalAmbient { get { return _globalAmbient; } set { _globalAmbient = value; _shaderInputValid = false; } }

       
        
        public Vector3 NorthPole { get { return _northPole; } set { _northPole = value; _shaderInputValid = false; } }
      
        
        public float GlobalAmbientIntensity { get { return _globalIntensity; } set { _globalIntensity = value; _shaderInputValid = false; } }

        public bool IsGpuSync
        {
            get
            {
                return _shaderInputValid;
            }
            set
            {
                _shaderInputValid = value;
            }
        }
    }
}
