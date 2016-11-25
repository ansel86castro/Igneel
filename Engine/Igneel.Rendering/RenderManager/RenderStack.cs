using System.Collections.Generic;

namespace Igneel.Rendering
{
    static class RenderStack<T>
    {
        public readonly static Stack<Render> Renders = new Stack<Render>();
    }
}