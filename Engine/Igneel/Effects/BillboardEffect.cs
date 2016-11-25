using System;
using Igneel.Rendering;
using Igneel.Graphics;

namespace Igneel.Effects
{
    public class BillboardEffect:Effect
    {

        public BillboardEffect(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            throw new NotImplementedException();
        }
    }
}
