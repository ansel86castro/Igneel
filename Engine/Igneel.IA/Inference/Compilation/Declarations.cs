using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.Inference.Compilation
{
    public abstract class Declaration : ASTNode
    {
        public string Name { get; set; }

        public Declaration(string name)
        {
            Name = name;
        }
    }

    public abstract class ExpressionDeclaration : Declaration
    {
        public Expresion Exp { get; set; }

        public ExpressionDeclaration(string name, Expresion expression)
            : this(name)
        {
            Exp = expression;
        }

        public ExpressionDeclaration(string name)
            : base(name)
        {

        }

        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
            Exp.CheckSemantic(log, scope);
            this.Type = Exp.Type;
        }
    }

    public abstract class FactDeclaration : Declaration
    {
        public FactDeclaration(string name)
            : base(name)
        {

        }

        public Fact Value { get; set; }

        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
            scope.Add(this);
            scope.Add(Value);

            Type = Value.FactType;
        }
    }
}
