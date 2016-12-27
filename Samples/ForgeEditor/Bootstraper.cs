using Igneel;
using Igneel.Graphics;
using Igneel.Physics;
using Igneel.Presenters;
using Igneel.Rendering;
using Igneel.Rendering.Presenters;
using Igneel.States;
using Igneel.Windows;
using Igneel.Windows.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace ForgeEditor
{
    public class Bootstraper
    {
        static readonly string[] _shaderRepositoryDir =
        {
            //IGNEEL CORE SHADERS
            "../../../../Shaders/Shaders.D3D10/Binaries/",

            //APPLICATION SHADERS
            "../../../ForgeShaders/Binaries/",
        };

        public static void Init(MainWindow window, Canvas3D mainCanvas)
        {
            Service.Set<GraphicDeviceFactory>(new IgneelD3D10.GraphicManager10());
           // Service.Set<PhysicManager>(new Igneel.PhysX.PXPhysicManager());
            Service.Set<Igneel.Input.InputManager>(new IgneelDirectInput.DInputManager());

            ShaderRepository.SetupD3D10_SM40(_shaderRepositoryDir);

            var gfactory = Service.Require<GraphicDeviceFactory>();
            var pFactory = Service.Get<PhysicManager>();
            var iFactory = Service.Get<Igneel.Input.InputManager>();

            var interopHelper = new WindowInteropHelper(window);
            var size = mainCanvas.RenderSize;

            mainCanvas.Init();

            Engine.Initialize(new InputContext(interopHelper.Handle), new GraphicDeviceDesc
            {
                Adapter = 0,
                DriverType = GraphicDeviceType.Hardware,
                Context = new WindowContext(mainCanvas.Handle)
                {
                    BackBufferWidth = (int)size.Width,
                    BackBufferHeight = (int)size.Height,
                    BackBufferFormat = Format.R8G8B8A8_UNORM_SRGB,
                    DepthStencilFormat = Format.D24_UNORM_S8_UINT,
                    FullScreen = false,
                    Sampling = new Multisampling(1, 0),
                    Presentation = PresentionInterval.Default
                }
            });
            Engine.Presenter = mainCanvas.CreateDafultPresenter();

            EngineState.Lighting.HemisphericalAmbient = true;
            EngineState.Lighting.Reflection.Enable = true;
            EngineState.Lighting.TransparencyEnable = true;
            EngineState.Shading.BumpMappingEnable = true;
 

            InitializeRendering();
            InitializeServices();
            
        }

        
        private static void InitializeRendering()
        {
            foreach (var type in typeof(IgneelApplication).Assembly.ExportedTypes)
            {
                if (!type.IsAbstract && !type.IsInterface && type.IsClass && type.GetInterface("IRenderRegistrator") != null)
                {
                    ((IRenderRegistrator)Activator.CreateInstance(type)).RegisterInstance();
                }
            }
        }

        private static void InitializeServices()
        {            
            Service.Set<IFactory<GraphicPresenter>>(new Factory<SingleViewPresenter>());            
            Service.Set<INotificationService>(new NotificationService());           
        }
    }
}
