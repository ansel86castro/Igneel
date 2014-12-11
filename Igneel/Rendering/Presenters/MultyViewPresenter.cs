using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public class MultyViewPresenter:GraphicPresenter
    {
        GraphicPresenter[] presenters;

        public GraphicPresenter[] Presenters
        {
            get { return presenters; }
        }             

        public MultyViewPresenter(GraphicPresenter[] presenters)
        {
            this.presenters = presenters;
        }        

        protected override void Render()
        {
            for (int i = 0; i < presenters.Length; i++)
            {
                var p = presenters[i];
                p.RenderFrame();
            }
        }

        public override void Resize(Size size)
        {
            foreach (var item in presenters)
            {
                item.Resize(size);
            }
        }
    }
}
