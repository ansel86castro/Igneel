using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Compiling
{
    class TypeNotFoundException : Exception
    {
        private string _typeName;

        public TypeNotFoundException(string typeName)
            :base("Type \""+typeName +"\" Not Found")
        {
            // TODO: Complete member initialization
            this._typeName = typeName;
        }
    }
}
