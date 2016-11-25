using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.Inference.Compilation
{   
    public class StringLiteral : Expresion
    {
        public string Value { get; set; }

        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
           

        }
    }

    public class FloatLiteral : Expresion
    {
        public float Value { get; set; }

        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
          
        }
    }

    public class FactReference : Expresion
    {
        public string Name;

        public Fact Fact;

        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
            try
            {
                Fact = scope.GetFact(Name);
                Type = Fact.FactType;
            }
            catch (KeyNotFoundException e)
            {
                log.AddError("Fact not defined \"" + Name + "\"", Line, Column);
            }
        }
    }

    public class TriggerReference : Expresion
    {
        public string Name;
        public MethodInfo Method;

        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
            Method = scope.GetMethod(Name);
            Type = typeof(void);
        }
    }
}
