using Igneel.Graphics;
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
using Igneel;

namespace Igneel.Windows.Wpf
{
    public class IgneelWpfApp : IgneelApplication
    {
        Window _window;
        string _shaderRepositoryDir;
        private Canvas3D _canvas;

        public IgneelWpfApp(string shaderRepositoryDir)
        {
            this._shaderRepositoryDir = shaderRepositoryDir;           
        }

        public IgneelWpfApp()
        :this("../../../Shaders/SM40/")
        {

        }

        public Window MainWindow { get { return _window; } set { _window = value; } }

        public Canvas3D MainControl { get { return _canvas; } set { _canvas = value; } }

        public override void Initialize()
        {
            if (!IsInitialized)
            {
                ShaderRepository.SetupD3D10_SM40(_shaderRepositoryDir);

                var gfactory = Service.Require<Graphics.GraphicDeviceFactory>();
                var pFactory = Service.Get<Physics.PhysicManager>();
                var iFactory = Service.Get<Input.InputManager>();

                var interopHelper = new WindowInteropHelper(_window);
                int width = double.IsNaN(_canvas.Width) ? 100:(int)_canvas.Width;
                int height = double.IsNaN(_canvas.Height) ? 100 : (int)_canvas.Height;

                Engine.Initialize(new InputContext(interopHelper.Handle), new GraphicDeviceDesc
                {
                    Adapter = 0,
                    DriverType = GraphicDeviceType.Hardware,
                    Context = new WindowContext(_canvas.Handle)
                    {
                        BackBufferWidth = width,
                        BackBufferHeight = height,
                        BackBufferFormat = Format.R8G8B8A8_UNORM_SRGB,
                        DepthStencilFormat = Format.D24_UNORM_S8_UINT,                        
                        FullScreen = false,
                        Sampling = new Multisampling(1, 0),                        
                        Presentation = PresentionInterval.Default
                    }                      
                });


                _canvas.SizeChanged += canvas_Resize;

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
            Engine.LoopStep();
        }

        private object DoFrame(object arg)
        {
            Engine.LoopStep();
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
            GraphicDeviceFactory.Device.ResizeBackBuffer((int)_canvas.Width, (int)_canvas.Height);
            if (Engine.Scene != null)
            {
                Engine.Scene.ActiveCamera.AspectRatio = (float)_canvas.Width / (float)_canvas.Height;
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
