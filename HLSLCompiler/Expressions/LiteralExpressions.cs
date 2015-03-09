using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Expressions
{
    public abstract class LiteralExpression:Expression
    {     
        public override IEnumerable<ASTNode> GetNodes()
        {
            yield break;
        }
    }
    public class LiteralExpression<T> : LiteralExpression
    {
        public T Value { get; set; }

        public LiteralExpression()
        {
            
        }

        public LiteralExpression(T value)
        {
            this.Value = value;
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            if (Value is int)
                Type = ShaderRuntime.Int;
            else if (Value is bool)
                Type = ShaderRuntime.Boolean;
            else if (Value is String)
                Type = ShaderRuntime.String;
            else if (Value is float)
                Type = ShaderRuntime.Float;
            else
                log.Error("Type not supported", Line, Column);
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append(Value.ToString());
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class SimbolExpression : LiteralExpression<string>
    {
        public SimbolExpression(string value)
            : base(value) { }
    }
}
