using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Input
{
    [Flags]
    public enum MouseButton
    {
        None = -1,
        Left = 0,
        Right = 1,
        Middle = 2
    }

    public abstract class Mouse: ResourceAllocator
    {
        private IInputContext _context;

        protected Mouse(IInputContext context)
        {
            this._context = context;
        }
       
        public IInputContext Context { get { return _context; } }

        public abstract int X { get; }

        public abstract int Y { get; }

        public abstract int Z { get; }

        public abstract bool IsButtonPresed(MouseButton button);
       
    }
}
