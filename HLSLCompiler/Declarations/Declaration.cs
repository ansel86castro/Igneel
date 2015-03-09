using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    public abstract class Declaration:ASTNode, INameable
    {        
        private string name;

        public string Name { get { return name; } set { name = value; } }

        public virtual bool IsUsed { get; set; }

        public bool IsChecked { get; set; }

        public override string ToString()
        {
            return name ?? base.ToString();
        }

        public abstract void AddToScope(Scope scope);
    }    

    
}
