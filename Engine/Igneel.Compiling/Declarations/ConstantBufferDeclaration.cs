using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Compiling.Declarations
{
    public class ConstantBufferDeclaration:Declaration
    {
        public int Index { get; set; }      

        public BufferType Type { get; set; }

        public int Size { get; set; }

        public GlobalVariableDeclaration[] Constants { get; set; }

        public override void AddToScope(Scope scope)
        {
            if (Type == BufferType.Undefined)
            {
                scope.Log.Error("Buffer Type not defined", Line, Column);
            }
            foreach (var item in Constants)
            {
                item.ConstanBuffer = this;
                item.AddToScope(scope);
            }

            CheckSemantic(scope, scope.Log);
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {          
            int size =0;
            foreach (var item in Constants)
            {             
                item.CheckSemantic(scope, log);
                size += item.Type.Size;
            }
            IsChecked = true;
        }

        /// <summary>
        /// Generate 
        /// cbuffer Wold
        /// {
        ///     ...contants
        /// };
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tabsOffset"></param>
        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.AppendFormat("{0} {1}", Type == BufferType.CBuffer ? "cbuffer" : "tbuffer", Name);
            sb.AppendLine();
            sb.Append('\t', tabsOffset);
            sb.Append("{");
            sb.AppendLine();
            foreach (var c in Constants)
            {
                c.GenerateCode(sb, tabsOffset + 1);              
                sb.AppendLine();
            }            
            sb.Append('\t', tabsOffset);
            sb.Append("};");

        }

        public override IEnumerable<AstNode> GetNodes()
        {
            return Constants;
        }
        public override string ToString()
        {
            return String.Format("{0} {1}", Type == BufferType.CBuffer ? "cbuffer" : "tbuffer", Name);
        }

        public void SetIsUsed(GlobalVariableDeclaration globalVariableDeclaration)
        {
            IsUsed = true;
            if (IsChecked)
                return;                       
        }
    }
}
