using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering.Bindings
{
    public abstract class TechniqueBinding<T> : RenderBinding<T>, ITechniqueBinding<T>
         where T : NodeTechnique
    {
        T lastTechnique;

        public T LastBindedTechnique
        {
            get { return lastTechnique; }
        }

        public override sealed void OnBind(T value)
        {
            if (lastTechnique != value)
            {
                lastTechnique = value;
                OnTechBind(value);
            }
        }

        public override void OnUnBind(T value)
        {
            if (value.NbEntries == 0)
            {
                OnTechUnBind(value);
                lastTechnique = null;
            }
        }

        protected abstract void OnTechBind(T value);

        protected abstract void OnTechUnBind(T value);
    }
}
