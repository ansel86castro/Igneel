using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.SceneComponents;

namespace Igneel.SceneManagement
{
    public class ListCuller<T> : ICuller<T> , ICullRegion
        where T : ICullable
    {
        List<T> _items = new List<T>(100);
        List<T> _temp = new List<T>(100);
        public bool Add(T item)
        {
            item.CullRegion = this;
            _items.Add(item);
            return true;
        }

        public List<T> Items { get { return _items; } set { _items = value; } }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public bool Remove(T item)
        {            
            if (_items.Remove(item))
            {
                item.CullRegion = null;
                return true;
            }
            return false;
        }

        public void Clear()
        {
            foreach (var item in _items)
            {
                item.CullRegion = null;
            }
            _items.Clear();
        }

        public void GetVisibleObjects(Camera camera, ICollection<T> collection, ICullTester<T> frustumTester = null)
        {
            foreach (var item in _items)
            {
                if (frustumTester != null && frustumTester.Contains(camera, item.BoundingSphere))
                    collection.Add(item);
                else if (camera.TestFrustum(item.BoundingSphere) != FrustumTest.Outside)
                    collection.Add(item);
            }
        }

        public List<T> GetVisibleObjects(Camera camera)
        {
            _temp.Clear();
            GetVisibleObjects(camera, _temp);
            return _temp;
        }

        void ICullRegion.Update(ICullable item)
        {
           
        }

        bool ICullRegion.Add(ICullable item)
        {
            Add((T)item);
            return true;
        }

        bool ICullRegion.Remove(ICullable item)
        {
            return Remove((T)item);
        }
    }
}
