using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Collections
{
    public class ResourceCollecion<T> : IReadOnlyCollection<T>
    {
        T[] resources;

        public ResourceCollecion(T[] resources)
        {
            this.resources = resources;
        }

        public int Count { get { return resources.Length; } }

        public T this[int index]
        {
            get { return resources[index]; }
        }

        public void CopyTo(int startIndex, int elements, T[] array)
        {
            Array.Copy(resources, startIndex, array, 0, elements);
        }

        public IEnumerator<T> GetEnumerator()
        {            
            return resources.Cast<T>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return resources.GetEnumerator();
        }
    }
}
