using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Services
{
    public class InitializationService
    {
        public List<IInitializable> values = new List<IInitializable>();

        public void Add(IInitializable item)
        {
            values.Add(item);
        }

        public void InitializeItems()
        {
            foreach (var item in values)
            {
                item.Initialize();
            }
            values.Clear();
        }
    }
}
