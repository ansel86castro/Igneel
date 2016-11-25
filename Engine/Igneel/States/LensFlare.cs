using Igneel.Graphics;

namespace Igneel.States
{
    public class LensFlareState:EnabilitableState
    {
        Texture2D[] _flareTextures;

        public Texture2D[] FlareTextures
        {
            get { return _flareTextures; }
            set { _flareTextures = value; }
        }
    }
}
