using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Rendering;
using Igneel.Techniques;

namespace Igneel.Techniques
{
    public class SceneHitTestRender : IHitTestRender
    {
        private List<HitTestResult> results = new List<HitTestResult>();

        public List<HitTestResult> HitTestResults { get { return results; } }

        #region IHitTestRender Members


        public void RenderObjects()
        {
            results.Clear();
            var scene = Engine.Scene;
            if (scene != null)
            {
                var visibleComponents = scene.VisibleComponents;
                var drawables = scene.Drawables;
                int renderId = 0;

                foreach (var entry in visibleComponents)
                {
                    var hitTestId = new HitTestId(++renderId);

                    var item = entry.UpdateRender();
                    if (item.Render == null)
                        continue;
                    item.Render.Bind(hitTestId);

                    item.Graphic.RenderId = hitTestId.Id;
                    item.Draw(PixelClipping.None);

                    item.Render.UnBind(hitTestId);
                }

                foreach (var item in drawables)
                {
                    item.RenderId = ++renderId;
                    item.Draw();
                }
            }
        }

        public bool FindResult(int abgr, out HitTestResult result)
        {
            result = new HitTestResult();
            var id = Color4.ConvertToRGBA(abgr);
            if (id <= 0)
                return false;

            id -= 1;
            var visibleComponents = Engine.Scene.VisibleComponents;
            var drawables = Engine.Scene.Drawables;

            if (id < visibleComponents.Count)
            {
                var entry = visibleComponents[id];
                result = new HitTestResult
                {
                    Drawable = entry.Graphic,
                    Frame = entry.Node,
                    RenderId = id
                };
                results.Add(result);
                return true;
            }
            else
            {
                id -= visibleComponents.Count;
                if (id < drawables.Count)
                {
                    result = new HitTestResult
                    {
                        Drawable = drawables[id],
                        RenderId = id
                    };
                    results.Add(result);
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
