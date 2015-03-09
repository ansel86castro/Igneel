using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Compiling
{
    class TypeNotFoundException : Exception
    {
        private string typeName;

        public TypeNotFoundException(string typeName)
            :base("Type \""+typeName +"\" Not Found")
        {
            // TODO: Complete member initialization
            this.typeName = typeName;
        }
    }
}
