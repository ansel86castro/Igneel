

using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    [Serializable]
    
    public class GlobalLigth:IShadingInput
    {
        Color3 globalAmbient = new Color3(0.2f, 0.2f, 0.2f);
        Color3 skyColor = new Color3(0.5f, 0.5f, 0.5f);
        Color3 grountColor = new Color3(0, 0, 0);
        Vector3 northPole = new Vector3(0, 1, 0);
        float globalIntensity = 1.0f;
        private bool shaderInputValid;



        public Color3 SkyColor
        {
            get { return skyColor; }
            set 
            {
                shaderInputValid = false;
                skyColor = value; 
            }
        }



        public Color3 GroundColor
        {
            get { return grountColor; }
            set
            {
                grountColor = value;
                shaderInputValid = false;
            }
        }



        public Color3 GlobalAmbient { get { return globalAmbient; } set { globalAmbient = value; shaderInputValid = false; } }

       
        
        public Vector3 NorthPole { get { return northPole; } set { northPole = value; shaderInputValid = false; } }
      
        
        public float GlobalAmbientIntensity { get { return globalIntensity; } set { globalIntensity = value; shaderInputValid = false; } }

        public bool IsGPUSync
        {
            get
            {
                return shaderInputValid;
            }
            set
            {
                shaderInputValid = value;
            }
        }
    }
}
