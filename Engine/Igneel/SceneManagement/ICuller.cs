using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.SceneComponents;

namespace Igneel.SceneManagement
{
    public interface ICuller<T>
      where T : IBoundable
    {        
        bool Add(T item);

        bool Contains(T item);

        bool Remove(T item);

        void Clear();

        void GetVisibleObjects(Camera camera, ICollection<T> collection, ICullTester<T> frustumTester = null);
    }

}
