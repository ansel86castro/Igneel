using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.Inference.Compilation
{
    public interface ICompilationLog
    {
        void AddError(string value, int line, int column);

        void AddWarning(string value, int line, int column);
    }

    public class CompilationScope
    {
        Dictionary<string, Declaration> declarations = new Dictionary<string, Declaration>();

        Dictionary<string, Fact> facts = new Dictionary<string, Fact>();

        public void Add(Declaration dec)
        {
            declarations.Add(dec.Name, dec);
            if (dec is FactDeclaration)
            {
                FactDeclaration factDel = (FactDeclaration)dec;
                facts.Add(factDel.Value.Name, factDel.Value);
            }
        }

        public Declaration this[string name]
        {
            get { return declarations[name]; }
        }

        public Fact GetFact(string name)
        {
            return facts[name];
        }


        public void Add(Fact Value)
        {
            facts[Value.Name] = Value;
        }

        internal System.Reflection.MethodInfo GetMethod(string Name)
        {
            throw new NotImplementedException();
        }
    }
}
