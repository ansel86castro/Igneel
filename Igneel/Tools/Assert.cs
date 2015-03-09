using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public class Assert
    {
        public void NotNull(object item)
        {
            if (item == null)
                throw new NullReferenceException();
        }
    }
}
