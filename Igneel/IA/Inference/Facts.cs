using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA
{
    public class Fact
    {
        string name;

        public Fact(string name)
        {
            this.name = name;
        }

       public string Name { get { return name; } }
    }

    public class Fact<T> : Fact
    {
        T value;
        public Fact(string name)
            : base(name)
        {

        }
        public Fact(string name, T value)
            : base(name)
        {
            this.value = value;
        }

        public T Value { get { return value; } set { this.value = value; } }
    }    
    
}
