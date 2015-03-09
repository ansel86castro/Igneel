using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Input
{
    public abstract class Keyboard: ResourceAllocator
    {
        private IInputContext context;
        protected Keyboard(IInputContext context)
        {
            this.context = context;
        }
        public IInputContext Context { get { return context; } }

        public abstract bool IsKeyPressed(Keys key);
       
    }
}
