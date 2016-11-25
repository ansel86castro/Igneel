using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Igneel;

namespace Igneel.Windows.Forms
{
    public class IgneelFormsApp:IgneelApplication
    {
        Form _mainForm;
        string _shaderRepositoryDir;
        private Control _canvas;

        public IgneelFormsApp()
            : this("../../../../Shaders/Shaders.D3D10/Binaries/")
        {

        }

        public IgneelFormsApp(string shaderRepositoryDirectory)
        {                        
            this._shaderRepositoryDir = shaderRepositoryDirectory;
        }

        public Form MainForm { get { return _mainForm; } set { _mainForm = value; } }

        public Control MainControl { get { return _canvas; } set { _canvas = value; } }

        public override void Initialize()
        {
            if (!IsInitialized)
            {
                 ShaderRepository.SetupD3D10_SM40(_shaderRepositoryDir);                

                var gfactory = Service.Require<Graphics.GraphicDeviceFactory>();
                var pFactory = Service.Get<Physics.PhysicManager>();
                var iFactory =Service.Get<Input.InputManager>();

                Engine.Initialize(new InputContext(_mainForm.Handle), new GraphicDeviceDesc
                {
                    Adapter = 0,
                    DriverType = GraphicDeviceType.Hardware,
                    Context = new WindowContext(_canvas.Handle)
                    {
                        BackBufferWidth = _canvas.Width,
                        BackBufferHeight = _canvas.Height,
                        BackBufferFormat = Format.R8G8B8A8_UNORM_SRGB,
                        DepthStencilFormat = Format.D24_UNORM_S8_UINT,                        
                        FullScreen = false,
                        Sampling = new Multisampling(1, 0),                        
                        Presentation = PresentionInterval.Default
                    }                      
                });


                _canvas.Resize += canvas_Resize;

                Application.Idle += Application_Idle;

                base.Initialize();
            }
        }

        void canvas_Resize(object sender, EventArgs e)
        {
            GraphicDeviceFactory.Device.ResizeBackBuffer(_canvas.Width, _canvas.Height);
            if (Engine.Scene != null)
            {
                Engine.Scene.ActiveCamera.AspectRatio = (float)_canvas.Width / (float)_canvas.Height;
                Engine.Scene.ActiveCamera.CommitChanges();
            }
            
        }

        void Application_Idle(object sender, EventArgs e)
        {
            NativeMessage message;
            while (!Native.PeekMessage(out message, IntPtr.Zero, 0, 0, 0))
            {
                Engine.LoopStep();
            }
        }

        protected override void InitializeServices()
        {
            base.InitializeServices();                        
        }
    }
}
