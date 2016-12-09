using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA
{
    public class Fact
    {
        string _name;

        public Fact(string name)
        {
            this._name = name;
        }

       public string Name { get { return _name; } }
    }

    public class Fact<T> : Fact
    {
        T _value;
        public Fact(string name)
            : base(name)
        {

        }
        public Fact(string name, T value)
            : base(name)
        {
            this._value = value;
        }

        public T Value { get { return _value; } set { this._value = value; } }
    }    
    
}
