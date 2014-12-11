using Igneel.Design;
using Igneel.Design.UITypeEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    [Serializable]
    [TypeConverter(typeof(Design.DesignTypeConverter))]
    public class GlobalLigth:IShadingInput
    {
        Vector3 globalAmbient = new Vector3(0.2f, 0.2f, 0.2f);
        Vector3 skyColor = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 grountColor = new Vector3(0, 0, 0);
        Vector3 northPole = new Vector3(0, 1, 0);
        float globalIntensity = 1.0f;
        private bool shaderInputValid;

        [TypeConverter(typeof(DesignTypeConverter))]
        [Editor(typeof(UIColorTypeEditor), typeof(UITypeEditor))]
        public Vector3 SkyColor
        {
            get { return skyColor; }
            set 
            {
                shaderInputValid = false;
                skyColor = value; 
            }
        }

        [TypeConverter(typeof(DesignTypeConverter))]
        [Editor(typeof(UIColorTypeEditor), typeof(UITypeEditor))]
        public Vector3 GroundColor
        {
            get { return grountColor; }
            set
            {
                grountColor = value;
                shaderInputValid = false;
            }
        }    

        [TypeConverter(typeof(DesignTypeConverter))]
        [Editor(typeof(UIColorTypeEditor), typeof(UITypeEditor))]
        public Vector3 GlobalAmbient { get { return globalAmbient; } set { globalAmbient = value; shaderInputValid = false; } }

        [TypeConverter(typeof(DesignTypeConverter))]
        [Editor(typeof(UIVector3TypeEditor), typeof(UITypeEditor))]
        public Vector3 NorthPole { get { return northPole; } set { northPole = value; shaderInputValid = false; } }
      
        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
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
