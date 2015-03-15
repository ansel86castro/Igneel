using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public interface IInitializable
    {
        void Initialize();
    }

    public interface IEnabletable
    {
        bool Enable { get; set; }
    }
}
