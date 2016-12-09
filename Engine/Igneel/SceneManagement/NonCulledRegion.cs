using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.SceneManagement
{
    public class NonCulledRegion : ICullRegion
    {
        private List<Frame> _items = new List<Frame>();

        public List<Frame> Items { get { return _items; } }

        public Scene Scene { get; set; }

        public void Update(ICullable item)
        {            
            if (Scene != null)
            {
                if (item.BoundingSphere.Radius > 0)
                {
                    _items.Remove((Frame)item);
                    item.CullRegion = Scene.GetCullRegion((Frame)item);
                }
            }
        }

        public bool Add(ICullable item)
        {
            _items.Add((Frame)item);
            return true;
        }

        public bool Remove(ICullable item)
        {
            return _items.Remove((Frame)item);
        }
    }
}
