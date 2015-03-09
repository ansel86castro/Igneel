using Igneel.Compiling.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Statements
{
    public class LValueStatement:Statement
    {
        Expression _lvalueExpression;
        public LValueStatement(Expression lvalueExpression)
        {
            _lvalueExpression = lvalueExpression;
        }

        public Expression LvalueExpression
        {
            get{return _lvalueExpression;}
            set{_lvalueExpression = value;}
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            _lvalueExpression.CheckSemantic(scope, log);
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            _lvalueExpression.GenerateCode(sb, tabsOffset);
            sb.Append(';');
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            yield return _lvalueExpression;
        }
    }
}
