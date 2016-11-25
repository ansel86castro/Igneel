using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering 
{
    class RenderRegistry<TComp, TRender> : RenderRegistry<TComp>
        where TComp : class
        where TRender : Render
    {

        protected override Render CreateRender()
        {
            return Service.Require<TRender>();
        }
    }
}
