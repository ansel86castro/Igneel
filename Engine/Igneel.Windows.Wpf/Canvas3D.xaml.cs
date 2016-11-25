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
using Igneel;
using Igneel.Presenters;
using Igneel.Rendering.Presenters;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Windows.Wpf
{
    /// <summary>
    /// Interaction logic for Canvas3D.xaml
    /// </summary>
    public partial class Canvas3D : UserControl
    {
        Win32Canvas _canvas;
        bool _isDefault;
        Scene _scene;
        Camera _camera;
        GraphicPresenter _presenter;

        public class Win32Canvas : System.Windows.Forms.UserControl
        {
            public Win32Canvas()
            {
                this.SetStyle(System.Windows.Forms.ControlStyles.Opaque 
                    | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            }            
        }

        public Canvas3D()
        {         
            InitializeComponent();           
        }

        public void Init()
        {
            _canvas = new Win32Canvas();

            var size = RenderSize;
            _canvas.Width = (int)size.Width;
            _canvas.Height = (int)size.Height;
            _canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            FormHost.Child = _canvas;

            this.SizeChanged += Canvas3D_SizeChanged;            
            
        }

        public Win32Canvas GetNativeCanvas()
        {
            return _canvas;
        }

        public int SurfaceWidth 
        {
            get { return _canvas.Width; }
        }

        public int SurfaceHeight
        {
            get { return _canvas.Height; }
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
           // Engine.Lock();

            try
            {
                FormHost.Width = width;
                FormHost.Height = height;
                if (IsValidSize(width) && IsValidSize(height))
                {
                    _canvas.Width = (int)width;
                    _canvas.Height = (int)height;
                }

                Engine.Invoke(delegate
                {
                    var scene = this._scene;
                    var camera = this._camera;
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
                    if (_presenter != null)
                    {
                        if (IsValidSize(height) && IsValidSize(width))
                        {
                            _presenter.Enable = true;
                            _presenter.Resize(new Size((int)width, (int)height));
                        }
                        else
                            _presenter.Enable = false;
                    }
                });
            }
            finally
            {
               // Engine.Unlock();
            }
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

        public IntPtr Handle { get { return _canvas.Handle; } }

        public SwapChainPresenter CreateSwapChainPresenter(Format backBufferFormat = Format.R8G8B8A8_UNORM_SRGB, 
            Format depthStencilFormat = Format.D24_UNORM_S8_UINT, 
            Multisampling msaa = default(Multisampling))
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

        bool IsValidSize(double value)
        {
            return value > 0 && !double.IsNaN(value) && !double.IsInfinity(value) && !double.IsNegativeInfinity(value) && !double.IsPositiveInfinity(value);
        }
    }
}
