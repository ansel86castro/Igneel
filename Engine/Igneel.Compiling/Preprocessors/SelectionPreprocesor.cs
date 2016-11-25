using Igneel.Compiling.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Preprocessors
{
    public class SelectionPreprocesor : PrepNode
    {
        public SelectionPreprocesor()
        {
            Nodes = new List<ConditionalCompilation>();
        }

        public List<ConditionalCompilation> Nodes { get; set; }

        public override string GetContent(Preprocessor p)
        {
            StringBuilder sb = new StringBuilder();
            bool skip = false;

            foreach (var item in Nodes)
            {
                sb.AppendLine();
                if (p != null)
                {
                    var simbol = item.Simbol;
                    if (simbol != null)
                    {
                        if (!skip)
                        {
                            skip = item.Condition == ConditionalCompilation.Directive.Def ?
                               p.IsNotDefined(simbol):
                               p.IsDefined(simbol);
                            if (skip)
                                sb.Append(item.GetContent(p));
                        }
                        else
                            sb.Append(item.GetLines());
                    }
                    else if (skip == false)
                        sb.Append(item.GetContent(p));
                }
                else
                    sb.Append(item.GetContent(null));
                
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }

    public class ConditionalCompilation:PrepNode
    {
        public ConditionalCompilation()
        {
            Nodes = new List<PrepNode>();
        }
        public enum Directive
        {
            Def, Ndef
        }

        public Directive Condition;

        public string Simbol { get; set;}

        public List<PrepNode> Nodes { get; set; }

        public override string GetContent(Preprocessor p)
        {          
            StringBuilder sb = new StringBuilder();
            foreach (var item in Nodes)
            {
                var s = item.GetContent(p);
                sb.Append(s);
            }            
            return sb.ToString();
            
        }


        public string GetLines()
        {
             StringBuilder sb = new StringBuilder();
             foreach (var item in Nodes)
             {
                 var s = item.GetContent(null);

                 if (!(item is Comment))
                 {
                     var skipped = s.Where(x => x == '\n')
                         .Select(x => x.ToString())
                         .Aggregate((x, y) => x + y);

                     sb.Append(skipped);
                 }
                 else
                     sb.Append(s);
             }
             return sb.ToString();
            
        }
    }
}
