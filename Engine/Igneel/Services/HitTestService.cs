using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Presenters;
using Igneel.Techniques;

namespace Igneel.Services
{
    public class HitTestService:ResourceAllocator
    {
        private GraphicPresenter _presenter;
        private HitTestTechnique _tecnique;
        private Size _backBufferSize;

        public HitTestTechnique Technique { get { return _tecnique; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="presenter">The region of the Screen asociated with this HitTestService</param>
        public HitTestService(GraphicPresenter presenter, Size backBufferSize)
        {
            this._presenter = presenter;
            this._backBufferSize = backBufferSize;

            _presenter.SizeChanged += _presenter_SizeChanged;

            Initialize();
        }

        private void Initialize()
        {
            _tecnique = new HitTestTechnique(_backBufferSize.Width, _backBufferSize.Height);
        }

        void _presenter_SizeChanged(object sender, Size e)
        {
            _tecnique.Resize(e.Width, e.Height);
        }

        public List<HitTestResult> HitTest(Vector2 point)
        {
            if (Engine.Scene == null)
                return null;

            Engine.Scene.UpdateVisibleComponents();

            _tecnique.Mode = HitTestTechnique.HitTestMode.Single;
            _tecnique.HitTestLocation = point;

            RenderManager.ApplyTechnique(_tecnique);
           

            return _tecnique.HitTestResults;
        }
      

    }
}
