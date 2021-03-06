﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{    
    public interface IDependencyResolver<out T> where T : class
    {
        IEnumerable<T> GetDependencies();
    }
}
