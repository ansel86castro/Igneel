using Igneel.Graphics;
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace Igneel.Windows.Wpf
{
    public class IgneelWpfApp : IgneelApplication
    {
        Window window;
        string shaderRepositoryDir;
        private Canvas3D canvas;

        public IgneelWpfApp(string shaderRepositoryDir)
        {
            this.shaderRepositoryDir = shaderRepositoryDir;           
        }

        public IgneelWpfApp()
        :this("../../../Shaders/SM40/")
        {

        }

        public Window MainWindow { get { return window; } set { window = value; } }

        public Canvas3D MainControl { get { return canvas; } set { canvas = value; } }

        public override void Initialize()
        {
            if (!IsInitialized)
            {
                ShaderRepository.SetupD3D10_SM40(shaderRepositoryDir);

                var gfactory = Service.Require<Graphics.GraphicDeviceFactory>();
                var pFactory = Service.Get<Physics.PhysicManager>();
                var iFactory = Service.Get<Input.InputManager>();

                var interopHelper = new WindowInteropHelper(window);
                int width = double.IsNaN(canvas.Width) ? 100:(int)canvas.Width;
                int height = double.IsNaN(canvas.Height) ? 100 : (int)canvas.Height;

                Engine.InitializeEngine(interopHelper.Handle, new GraphicDeviceDesc
                {
                    Adapter = 0,
                    BackBufferWidth = width,
                    BackBufferHeight = height,
                    BackBufferFormat = Format.R8G8B8A8_UNORM_SRGB,
                    DepthStencilFormat = Format.D24_UNORM_S8_UINT,
                    DriverType = GraphicDeviceType.Hardware,
                    FullScreen = false,
                    MSAA = new Multisampling(1, 0),
                    WindowsHandle = canvas.Handle,
                    Interval = PresentionInterval.Default
                });


                canvas.SizeChanged += canvas_Resize;

                //DispatcherFrame frame = new DispatcherFrame(true);
                //Dispatcher.CurrentDispatcher.BeginInvoke(new DispatcherOperationCallback(DoFrame), DispatcherPriority.ApplicationIdle, frame);

                CompositionTarget.Rendering += CompositionTarget_Rendering;

                //window.Dispatcher.Hooks.DispatcherInactive += Hooks_DispatcherInactive;
                //window.Dispatcher.Hooks.OperationPosted += Hooks_OperationPosted;                
                base.Initialize();                
            }
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            Engine.Render();
        }

        private object DoFrame(object arg)
        {
            Engine.Render();
            return null;
        }

        void Hooks_OperationPosted(object sender, DispatcherHookEventArgs e)
        {
            
        }
       

        void Hooks_DispatcherInactive(object sender, EventArgs e)
        {
            
        }

        void canvas_Resize(object sender, EventArgs e)
        {
            Engine.Graphics.ResizeBackBuffer((int)canvas.Width, (int)canvas.Height);
            if (Engine.Scene != null)
            {
                Engine.Scene.ActiveCamera.AspectRatio = (float)canvas.Width / (float)canvas.Height;
                Engine.Scene.ActiveCamera.CommitChanges();
            }
        }        

        protected override void InitializeServices()
        {
            base.InitializeServices();

            //Service.Set<PickingService>(new FormPickService());
        }
    }
}
