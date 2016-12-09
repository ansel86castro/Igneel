using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForgeEditor.Components.CoordinateSystems;
using ForgeEditor.Effects;
using Igneel;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Techniques;

namespace ForgeEditor.Components
{
    public class DecalGlypHitTest : HitTestTechnique
    {
        public DecalGlypHitTest(int width, int height, Format depthFormat, DecalGlyp coordinate)
            : base(width, height, depthFormat, new DecalGlypIdRender(coordinate))
        {

        }
    }

    public class DecalGlypIdRender: IHitTestRender
    {
        private DecalGlyp glyp;
        private List<HitTestResult> _resultList = new List<HitTestResult>();
        RelayRender<DecalGlyp, TranslationGlypHitTestEffect> render;

        public DecalGlypIdRender(DecalGlyp glyp)
        {            
            this.glyp = glyp;
            this.render = new RelayRender<DecalGlyp, TranslationGlypHitTestEffect>(RenderMethod);
                
        }

        private static void RenderMethod(DecalGlyp component, GraphicRender<DecalGlyp, TranslationGlypHitTestEffect> render)
        {
 	            var effect = render.Effect;
                var device = effect.Device;

                device.PrimitiveTopology = IAPrimitive.TriangleList;

                var decal = component.ScreenRectangle;
                var scene = Engine.Scene;
                var camera = scene.ActiveCamera;
                effect.Input.View = camera.View;
                effect.Input.Proj = Matrix.PerspectiveFovLh((float)decal.Width / decal.Height, Numerics.ToRadians(60), 1, 100);
                effect.Input.World = Matrix.Identity;            

                foreach (var item in component.Components)
                {
                    //HitTestId id = new HitTestId(((int)item.Axix << 8) | 0x000000FF);
                    HitTestId id = new HitTestId(item.Id);
                    render.Bind(id);

                    device.SetVertexBuffer(0, item.VertexBuffer);
                    device.SetIndexBuffer(item.IndexBufffer);

                    int indexCount = (int)(item.IndexBufffer.SizeInBytes / item.IndexBufffer.Stride);

                    effect.OnRender(render);
                    foreach (var pass in effect.Passes(0))
                    {
                        effect.Apply(pass);
                        device.DrawIndexed(indexCount, 0, 0);
                    }
                    effect.EndPasses();
                }                
        }

        #region IHitTestRender Members

        public List<HitTestResult> HitTestResults
        {
            get { return _resultList; }
        }

        public void RenderObjects()
        {
            _resultList.Clear();
            render.Draw(glyp);
        }

        public bool FindResult(int abgr, out HitTestResult result)
        {
            result = new HitTestResult();
            var id = Color4.ConvertToRGBA(abgr);
            if (id <= 0 || id > glyp.Components.Length)
                return false;

            result.Data = glyp.Components[id - 1];

            //AxisName axis = (AxisName)id;
            //switch (axis)
            //{
            //    case AxisName.X:
            //        result.Data = coordinate.Components[0];
            //        break;
            //    case AxisName.Y:
            //        result.Data = coordinate.Components[1];
            //        break;
            //    case AxisName.Z:
            //        result.Data = coordinate.Components[2];
            //        break;
            //    case AxisName.XY:
            //        result.Data = coordinate.Components[3];
            //        break;
            //    case AxisName.XZ:
            //        result.Data = coordinate.Components[4];
            //        break;
            //    case AxisName.YZ:
            //        result.Data = coordinate.Components[5];
            //        break;
            //}


            _resultList.Add(result);
            return true;
        }

        #endregion
    }

}