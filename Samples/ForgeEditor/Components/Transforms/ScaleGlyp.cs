using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ForgeEditor.Components.CoordinateSystems;

namespace ForgeEditor.Components.Transforms
{
    public class ScaleGlyp: CoordinateGlyp, ITranformGlyp
    {
        public ScaleGlyp()          
        {
        }
        #region ITranformGlyp Members

        public void Transform(Igneel.SceneManagement.Frame tranlatable, GlypComponent component, Igneel.Vector2 p0, Igneel.Vector2 p1)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
