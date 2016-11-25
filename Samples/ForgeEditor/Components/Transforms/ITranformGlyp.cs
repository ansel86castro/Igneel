using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForgeEditor.Components.CoordinateSystems;
using Igneel;
using Igneel.SceneManagement;

namespace ForgeEditor.Components.Transforms
{
    public interface ITranformGlyp : IDecalGlyp
    {
        /// <summary>
        /// The screen displacement is compute as dp = p1 - p0
        /// </summary>
        /// <param name="p0">Hit Point</param>
        /// <param name="p1">Next Hit Point</param>
        void Transform(Frame frame, GlypComponent component, Vector2 p0, Vector2 p1);

    }
}
