using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling
{
    public class ErrorLog
    {
        private List<string> _errors = new List<string>();

        public List<string> Errors { get { return _errors; } }

        public void Error(string errorMessage, int line, int column)
        {
            _errors.Add(String.Format("{0} ,Line:{1}, Column:{2}", errorMessage, line,column));
        }
    }
}
