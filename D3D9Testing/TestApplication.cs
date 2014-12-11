using Igneel;
using Igneel.Graphics;
using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3D9Testing
{
    //public class TestApplication : IgneelApplication
    //{
    //    int fps = -1;
    //    float baseTime;
    //    Form1 form1;
    //    public static bool UseFrameLines =true;

    //    public Form1 Form
    //    {
    //        get { return form1; }
    //        set { form1 = value; }
    //    }        

    //    public override void Initialize()
    //    {
    //        ShaderRepository.SetupD3D10_SM40();

    //        form1 = new Form1();                     
    //        var gfactory = new IgneelD3D10.GraphicManager10();
    //        var pFactory = new Igneel.PhysX.PXPhysicManager();
    //        var iFactory = new IgneelDirectInput.DInputManager();

    //        Engine.InitializeEngine(form1.Handle, new GraphicDeviceDesc
    //        {
    //            Adapter = 0,
    //            BackBufferWidth = form1.screen.Width,
    //            BackBufferHeight = form1.screen.Height,
    //            BackBufferFormat = Format.R8G8B8A8_UNORM_SRGB,
    //            DepthStencilFormat = Format.D24_UNORM_S8_UINT,
    //            DriverType = GraphicDeviceType.Hardware,
    //            FullScreen = false,
    //            MSAA = new Multisampling(1, 0),
    //            WindowsHandle = form1.screen.Handle,
    //            Interval = PresentionInterval.Default
    //        });
    //        Engine.Presenter = new SingleViewPresenter();
    //        Engine.Presenter.Rendering += Presenter_Rendering;
    //        form1.screen.Resize += screen_Resize;

    //        form1.InitializeTests();
    //        base.Initialize();

    //        Application.Idle += Application_Idle;
    //        Application.Run(form1);
    //    }

    //    void Application_Idle(object sender, EventArgs e)
    //    {            
    //        Engine.MainLoop();
    //    }

    //    void screen_Resize(object sender, EventArgs e)
    //    {
    //        Engine.Graphics.ResizeBackBuffer(form1.screen.Width, form1.screen.Height);
    //        if (Engine.Scene != null)
    //        {
    //            Engine.Scene.ActiveCamera.AspectRatio = (float)form1.screen.Width / (float)form1.screen.Height;
    //            Engine.Scene.ActiveCamera.CommitChanges();
    //        }
            
    //    }

    //    void Presenter_Rendering()
    //    {
    //        if (fps == -1)
    //        {
    //            fps = 0;
    //            baseTime = Engine.Time.Time;
    //        }
    //        else
    //        {
    //            float time = Engine.Time.Time;
    //            if ((time - baseTime) > 1.0f)
    //            {
    //                form1.fpsCounter.Text = "FPS : " + fps;
    //                fps = 0;
    //                baseTime = time;
    //            }
    //            fps++;
    //        }
    //    }
    //}
}
