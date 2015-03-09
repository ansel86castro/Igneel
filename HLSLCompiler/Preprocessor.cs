using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Igneel.Compiling
{
    public class Preprocessor
    {
        HashSet<string> _includeFiles = new HashSet<string>();
        List<string> _toParse = new List<string>();
        StringBuilder _sb = new StringBuilder();
        Dictionary<string, string> _macros = new Dictionary<string, string>();
        bool skip;
        Stack<Func<bool>> _conditions = new Stack<Func<bool>>();

        public List<string> Includes { get { return _toParse; } }

        public void AddCondition(string simbol, bool isndef)
        {
            var defined = _macros.ContainsKey(simbol);
            _conditions.Push(delegate()
            {
                return isndef ? !defined : defined;
            });
        }

        public void RemoveCondition()
        {
            if (_conditions.Count > 0)
                _conditions.Pop();
        }
        private bool CheckCondition()
        {
            return _conditions.Count > 0 ? _conditions.Peek()() : true;
        }

        public void AddInclude(string file)
        {
            if (CheckCondition())
            {
                var included = file.Substring(1, file.Length - 2);
                if (!_includeFiles.Contains(included))
                    _toParse.Add(included);
            }
        }

        public void AppendLine()
        {
            _sb.AppendLine();
        }


        public void ParseComplete()
        {
            foreach (var item in _toParse)
            {
                _includeFiles.Add(item);
            }
            _toParse.Clear();
            _sb.Clear();
        }
      
        public void Append(string line)
        {
            if (skip) return;

            if (!CheckCondition())
            {
                int linesCount = Regex.Matches(line, @"\r\n|\n").Count;
                for (int i = 0; i < linesCount; i++)
                {
                    _sb.AppendLine();
                }
                return;
            }

            var value = Regex.Replace(line, @"\w+", x =>
            {
                string v;
                if (_macros.TryGetValue(x.Value, out v))
                {
                    return v ?? "";
                }
                return x.Value;
            });

            _sb.Append(value);
        }

        public void AddMacro(string simbol, string value)
        {
            if(CheckCondition())
                _macros[simbol] = value;
        }

        public bool IsDefined(string simbol)
        {
            return skip=!_macros.ContainsKey(simbol);
        }

        public bool IsNotDefined(string simbol)
        {
            return !IsDefined(simbol);
        }      

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
