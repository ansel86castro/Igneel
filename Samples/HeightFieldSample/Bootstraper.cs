using Igneel;
using Igneel.Graphics;
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
using System.Windows;
using System.Windows.Interop;

namespace HeightFieldSample
{
    /// <summary>
    /// Initialize the Engine 
    /// </summary>
    public class Bootstraper
    {
        static readonly string[] _shaderRepositoryDir =
        {
            //IGNEEL CORE SHADERS
            "../../../../../Shaders/Shaders.D3D10/Debug/Binaries/",

            //APPLICATION SHADERS
            "../../../../ForgeShaders/Binaries/",
        };

        public static void Init(Window window, Canvas3D mainCanvas)
        {
            //Initialize Graphics API
            Service.Set<GraphicDeviceFactory>(new IgneelD3D10.GraphicManager10());          

            //Initialize Input API
            Service.Set<Igneel.Input.InputManager>(new IgneelDirectInput.DInputManager());

            //Initialize shader respository
            ShaderRepository.SetupD3D10_SM40(_shaderRepositoryDir);
            
            var gfactory = Service.Require<GraphicDeviceFactory>();         
            var iFactory = Service.Get<Igneel.Input.InputManager>();

            //Helper to get the HWND Handle
            var interopHelper = new WindowInteropHelper(window);

            //The canvas size
            var size = mainCanvas.RenderSize;
            EngineState.Shading.FillMode = FillMode.Wireframe;

            //Initialize the Canvas. The Canvas3D is used to render to the screen in a WPF context
            //this is achived by creating a GraphicPresenter
            mainCanvas.Init();

            //Initialize the Engine and creating the IGraphicContext by setting the BackBuffer and DepthStencil descriptions
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

            //Set the Engine presenter to the Canvas3D presenter 
            //this instruct the Engine to render the content using the Canvas3D GraphicPresenter
            Engine.Presenter = mainCanvas.CreateDafultPresenter();

            //Set default Lighting and Shading properties
            EngineState.Lighting.HemisphericalAmbient = true;
            EngineState.Lighting.Reflection.Enable = true;
            EngineState.Lighting.TransparencyEnable = true;
            EngineState.Shading.BumpMappingEnable = true;         
 
            //Initialize the Hight Level rendering System
            //This will register the renders fo each components for each supported shading techniques
            InitializeRendering();

            //InitializeServices();
            
        }

        
        private static void InitializeRendering()
        {
            // Register Engine components First
            foreach (var type in typeof(IgneelApplication).Assembly.ExportedTypes)
            {
                if (!type.IsAbstract && !type.IsInterface && type.IsClass && type.GetInterface("IRenderRegistrator") != null)
                {
                    ((IRenderRegistrator)Activator.CreateInstance(type)).RegisterInstance();
                }
            }

            //Register Custom components here

        }

        //private static void InitializeServices()
        //{            
        //    Service.Set<IFactory<GraphicPresenter>>(new Factory<SingleViewPresenter>());            
        //    Service.Set<INotificationService>(new NotificationService());           
        //}
    }
}
