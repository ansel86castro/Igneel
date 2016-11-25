using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    public abstract class Declaration:AstNode, INameable
    {        
        private string _name;

        public string Name { get { return _name; } set { _name = value; } }

        public virtual bool IsUsed { get; set; }

        public bool IsChecked { get; set; }

        public override string ToString()
        {
            return _name ?? base.ToString();
        }

        public abstract void AddToScope(Scope scope);
    }    

    
}
