using Igneel.Components;
using Igneel.Graphics;
using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Igneel.Windows.Wpf
{
    /// <summary>
    /// Interaction logic for Canvas3D.xaml
    /// </summary>
    public partial class Canvas3D : UserControl
    {
        Win32Canvas canvas;
        bool isDefault;
        Scene scene;
        Camera camera;
        GraphicPresenter presenter;

        class Win32Canvas : System.Windows.Forms.UserControl
        {
            public Win32Canvas()
            {
                this.SetStyle(System.Windows.Forms.ControlStyles.Opaque | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            }

            
        }

        public Canvas3D()
        {         
            InitializeComponent();

            canvas = new Win32Canvas();
            canvas.Width = (int)Width;
            canvas.Height = (int)Height;
            canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            formHost.Child = canvas;

            this.SizeChanged += Canvas3D_SizeChanged;
        }

        //protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeBounds)
        //{
        //    ResizeCanvas(arrangeBounds.Width, arrangeBounds.Height);
        //    return base.ArrangeOverride(arrangeBounds);
        //}
        

        void Canvas3D_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = e.NewSize.Width;
            var height = e.NewSize.Height;
            ResizeCanvas(width, height);
        }

        private void ResizeCanvas(double width, double height)
        {
            formHost.Width = width;
            formHost.Height = height;
            if (IsValidSize(width) && IsValidSize(height))
            {
                canvas.Width = (int)width;
                canvas.Height = (int)height;
            }

            var scene = this.scene;
            var camera = this.camera;
            if (scene == null)
                scene = Engine.Scene;
            if (camera == null && scene != null)
                camera = scene.ActiveCamera;

            if (IsValidSize(height) && IsValidSize(width))
            {
                if (camera != null)
                {
                    camera.AspectRatio = (float)width / (float)height;
                    camera.CommitChanges();
                }
                if (scene != null)
                {
                    foreach (var item in scene.Cameras)
                    {
                        if (camera != item)
                        {
                            item.AspectRatio = (float)width / (float)height;
                            item.CommitChanges();
                        }
                    }
                }
            }
            if (presenter != null)
            {
                if (IsValidSize(height) && IsValidSize(width))
                {
                    presenter.Enable = true;
                    presenter.Resize(new Size((int)width, (int)height));
                }
                else
                    presenter.Enable = false;
            }
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

        public IntPtr Handle { get { return canvas.Handle; } }

        public SwapChainPresenter CreateSwapChainPresenter(Format backBufferFormat = Format.R8G8B8A8_UNORM_SRGB, Format depthStencilFormat = Format.D24_UNORM_S8_UINT, Multisampling msaa = default(Multisampling))
        {
            var p = new SwapChainPresenter(new WindowContext
            {
                BackBufferWidth = (int)Width,
                BackBufferHeight = (int)Height,
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

        bool IsValidSize(double value)
        {
            return value > 0 && !double.IsNaN(value) && !double.IsInfinity(value) && !double.IsNegativeInfinity(value) && !double.IsPositiveInfinity(value);
        }
    }
}
