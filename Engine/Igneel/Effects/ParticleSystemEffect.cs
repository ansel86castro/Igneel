using System;
using Igneel.Rendering;
using Igneel.Graphics;

namespace Igneel.Effects
{
    public class ParticleSystemEffect:Effect
    {
        public ParticleSystemEffect(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            throw new NotImplementedException();
        }
    }
}
