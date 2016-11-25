using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Igneel;
using Igneel.Presenters;
using Igneel.Rendering;
using Igneel.Graphics;
using Igneel.Rendering.Presenters;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Windows.Forms
{
    public partial class Canvas3D: UserControl
    {
        bool _isDefault;
        Scene _scene;
        Camera _camera;
        GraphicPresenter _presenter;

        public Canvas3D()
        {
            InitializeComponent();

            this.SetStyle(System.Windows.Forms.ControlStyles.Opaque | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
        }

        public bool IsDefault
        {
            get { return _isDefault; }
            set { _isDefault = value; }
        }

        
        public Scene Scene
        {
            get { return _scene; }
            set { _scene = value; }
        }

        public Camera Camera 
        { 
            get { return _camera; } 
            set { _camera = value; } 
        }

        public GraphicPresenter Presenter
        {
            get { return _presenter; }
            set { _presenter = value; }
        }
      
        protected override void OnSizeChanged(EventArgs e)
        {
            var scene = this._scene;
            var camera = this._camera;
            if (scene == null)
                scene = Engine.Scene;
            if (camera == null && scene != null)
                camera = scene.ActiveCamera;

            if (this.Height != 0)
            {
                if (camera != null)
                {
                    camera.AspectRatio = (float)Width / (float)Height;
                    camera.CommitChanges();
                }
                if (scene != null)
                {
                    foreach (var item in scene.Cameras)
                    {
                        if (camera != item)
                        {
                            item.AspectRatio = (float)Width / (float)Height;
                            item.CommitChanges();
                        }
                    }
                }
            }
            if (_presenter != null)
            {
                if (Width != 0 && Height != 0)
                {
                    _presenter.Enable = true;
                    _presenter.Resize(new Size(Width, Height));
                }
                else
                    _presenter.Enable = false;
            }
            base.OnSizeChanged(e);
        }


        public SwapChainPresenter CreateSwapChainPresenter(Format backBufferFormat = Format.R8G8B8A8_UNORM_SRGB, Format depthStencilFormat = Format.D24_UNORM_S8_UINT, Multisampling msaa = default(Multisampling))
        {
            var p = new SwapChainPresenter(new WindowContext
            {
                BackBufferWidth =  Width,
                BackBufferHeight = Height,
                BackBufferFormat = backBufferFormat, 
                DepthStencilFormat = depthStencilFormat,
                Sampling = msaa,
                WindowHandle = Handle
            });
            _presenter = p;
            return p;
        }

        public SingleViewPresenter CreateDafultPresenter()
        {
            var p = new SingleViewPresenter();
            _presenter = p;
            _isDefault = true;
            return p;
        }
    }
}
