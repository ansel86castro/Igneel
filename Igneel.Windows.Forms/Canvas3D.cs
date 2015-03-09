using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Igneel.Components;
using Igneel.Rendering;
using Igneel.Graphics;

namespace Igneel.Windows.Forms
{
    public partial class Canvas3D: UserControl
    {
        bool isDefault;
        Scene scene;
        Camera camera;
        GraphicPresenter presenter;

        public Canvas3D()
        {
            InitializeComponent();

            this.SetStyle(System.Windows.Forms.ControlStyles.Opaque | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
        }

        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }

        
        public Scene Scene
        {
            get { return scene; }
            set { scene = value; }
        }

        public Camera Camera 
        { 
            get { return camera; } 
            set { camera = value; } 
        }

        public GraphicPresenter Presenter
        {
            get { return presenter; }
            set { presenter = value; }
        }
      
        protected override void OnSizeChanged(EventArgs e)
        {
            var scene = this.scene;
            var camera = this.camera;
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
            if (presenter != null)
            {
                if (Width != 0 && Height != 0)
                {
                    presenter.Enable = true;
                    presenter.Resize(new Size(Width, Height));
                }
                else
                    presenter.Enable = false;
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
            presenter = p;
            return p;
        }

        public SingleViewPresenter CreateDafultPresenter()
        {
            var p = new SingleViewPresenter();
            presenter = p;
            isDefault = true;
            return p;
        }
    }
}
