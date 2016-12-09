using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public interface IVisualMaterial
    {
        int VisualId { get; set; }

        Render Render { get; set; }

        void Bind(Render render);

        void UnBind(Render render);        
    }
}
