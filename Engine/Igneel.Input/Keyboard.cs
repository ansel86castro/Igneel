using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Input
{
    public abstract class Keyboard: ResourceAllocator
    {
        private IInputContext _context;
        protected Keyboard(IInputContext context)
        {
            this._context = context;
        }
        public IInputContext Context { get { return _context; } }

        public abstract bool IsKeyPressed(Keys key);
       
    }
}
