using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public class LensFlareState:EnabilitableState
    {
        Texture2D[] flareTextures;

        public Texture2D[] FlareTextures
        {
            get { return flareTextures; }
            set { flareTextures = value; }
        }
    }
}
