using System;
using Igneel.Rendering;
using Igneel.Graphics;

namespace Igneel.Effects
{
    public class HeightFieldEffect:Effect
    {
        public HeightFieldEffect(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            throw new NotImplementedException();
        }
    }
}
