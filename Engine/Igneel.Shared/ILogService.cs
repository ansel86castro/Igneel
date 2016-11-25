using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public interface ILogService
    {
        void Write(string text);

        void WriteLine(string text);
    }
}
