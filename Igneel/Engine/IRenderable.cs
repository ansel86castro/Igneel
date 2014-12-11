using Igneel.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{   
    [TypeConverter(typeof(DesignTypeConverter))]
    public interface IRenderable
    {
        void Draw();
    }
}
