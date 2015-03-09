using Igneel.Graphics;
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Igneel.Windows.Forms
{
    public class IgneelFormsApp:IgneelApplication
    {
        Form mainForm;
        string shaderRepositoryDir;
        private Control canvas;

        public IgneelFormsApp()
            : this("../../../Shaders/SM40/")
        {

        }

        public IgneelFormsApp(string shaderRepositoryDirectory)
        {                        
            this.shaderRepositoryDir = shaderRepositoryDirectory;
        }

        public Form MainForm { get { return mainForm; } set { mainForm = value; } }

        public Control MainControl { get { return canvas; } set { canvas = value; } }

        public override void Initialize()
        {
            if (!IsInitialized)
            {
                 ShaderRepository.SetupD3D10_SM40(shaderRepositoryDir);                

                var gfactory = Service.Require<Graphics.GraphicDeviceFactory>();
                var pFactory = Service.Get<Physics.PhysicManager>();
                var iFactory =Service.Get<Input.InputManager>();

                Engine.InitializeEngine(new WindowContext(mainForm.Handle), new GraphicDeviceDesc
                {
                    Adapter = 0,
                    DriverType = GraphicDeviceType.Hardware,
                    Context = new WindowContext(canvas.Handle)
                    {
                        BackBufferWidth = canvas.Width,
                        BackBufferHeight = canvas.Height,
                        BackBufferFormat = Format.R8G8B8A8_UNORM_SRGB,
                        DepthStencilFormat = Format.D24_UNORM_S8_UINT,                        
                        FullScreen = false,
                        Sampling = new Multisampling(1, 0),                        
                        Presentation = PresentionInterval.Default
                    }                      
                });


                canvas.Resize += canvas_Resize;

                Application.Idle += Application_Idle;

                base.Initialize();
            }
        }

        void canvas_Resize(object sender, EventArgs e)
        {
            Engine.Graphics.ResizeBackBuffer(canvas.Width, canvas.Height);
            if (Engine.Scene != null)
            {
                Engine.Scene.ActiveCamera.AspectRatio = (float)canvas.Width / (float)canvas.Height;
                Engine.Scene.ActiveCamera.CommitChanges();
            }
            
        }

        void Application_Idle(object sender, EventArgs e)
        {
            NativeMessage message;
            while (!Native.PeekMessage(out message, IntPtr.Zero, 0, 0, 0))
            {
                Engine.Render();
            }
        }

        protected override void InitializeServices()
        {
            base.InitializeServices();

            Service.Set<PickingService>(new FormPickService());                    
        }
    }
}
