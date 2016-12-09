using System.Collections.Generic;

namespace Igneel.Rendering
{
    static class TrCache<TTech> where TTech : Technique
    {
        public static TechniqueRegistry Registry = new TechniqueRegistry();

        public static List<IRenderRegistry> RenderRegistries
        {
            get { return Registry.Renders; }
        }

        public static Technique Technique { get { return Registry.technique; } }
    }
}