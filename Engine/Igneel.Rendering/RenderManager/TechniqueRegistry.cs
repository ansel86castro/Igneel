using System.Collections.Generic;

namespace Igneel.Rendering
{
    public class TechniqueRegistry
    {
        internal Technique technique;
        internal List<IRenderRegistry> Renders = new List<IRenderRegistry>();

        public Technique Technique
        {
            get { return technique; }
        }

        public override string ToString()
        {
            if (technique != null)
                return technique.ToString();
            else
                return "Empty";
        }
    }
}