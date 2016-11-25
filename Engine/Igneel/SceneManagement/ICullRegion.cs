using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.SceneManagement
{
    public interface ICullRegion        
    {
        void Update(ICullable item);

        bool Add(ICullable item);

        bool Remove(ICullable item);
    }
}
