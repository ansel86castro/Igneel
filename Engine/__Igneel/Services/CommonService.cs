using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Services
{
    public class DynamicService
    {
        List<IDynamic> dynamics = new List<IDynamic>();

        public List<IDynamic> Dynamics
        {
            get { return dynamics; }          
        }

    }

    public class PostRenderService
    {
        List<IRenderable> rendereables = new List<IRenderable>();

        public List<IRenderable> Rendereables
        {
            get { return rendereables; }         
        }

        public void Add(IRenderable item)
        {
            rendereables.Add(item);
        }

        public bool Remove(IRenderable item)
        {
            return rendereables.Remove(item);
        }

        public bool Contains(IRenderable item)
        {
            return rendereables.Contains(item);
        }

    }
   
    public class PresentService
    {
        List<IRenderable> rendereables = new List<IRenderable>();

        public List<IRenderable> Rendereables
        {
            get { return rendereables; }
        }
    }     
}
