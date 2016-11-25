using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForgeEditor.Effects;
using Igneel;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.Techniques;

namespace ForgeEditor.Components
{
    public class MaterialHitTest : HitTestTechnique
    {
        public MaterialHitTest(int width, int height)
            : base(width, height, Format.D16_UNORM, new MaterialIdRender())
        {

        }

        public static void RegisterRenders()
        {
            Registrator<FrameMesh>.Register<MaterialHitTest, FrameMeshRender<MaterialHitTestEffect>>();
            Registrator<FrameSkin>.Register<MaterialHitTest, FrameSkinRender<MaterialHitSkinneffect>>();
        }
    }

    public class MaterialIdRender : IHitTestRender
    {
        private List<HitTestResult> _resultList = new List<HitTestResult>();
        #region IHitTestRender Members

        public List<HitTestResult> HitTestResults
        {
            get { return _resultList; }
        }

        public void RenderObjects()
        {
            _resultList.Clear();
            var scene = Engine.Scene;
            if (scene != null)
            {
                int renderId = 0;
                foreach (var material in scene.Materials)
                {
                    material.VisualId += ++renderId;   
                }
                
                var visibleComponents = scene.VisibleComponents;
                var drawables = scene.Drawables;                

                foreach (var entry in visibleComponents)
                {                    
                    var item = entry.UpdateRender();
                    if (item.Render == null)
                        continue;
                 
                    item.Draw(PixelClipping.None);                  
                }

                foreach (var item in drawables)
                {                    
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
            var scene = Engine.Scene;
            if (id < 0 || id > scene.Materials.Count)
                return false;

            result.Data = scene.Materials[id];
            return true;
        }

        #endregion
    }
}
