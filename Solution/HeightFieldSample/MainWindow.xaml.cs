using Igneel;
using Igneel.Components;
using Igneel.Controllers;
using Igneel.Graphics;
using Igneel.Input;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
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

namespace HeightFieldSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly string[] _shaderRepositoryDir =
       {
            //IGNEEL CORE SHADERS
            "../../../../Shaders/Shaders.D3D10/Binaries/",
         
        };

        private float _fps;
        private float _baseTime;

        public MainWindow()
        {
            InitializeComponent();

            WindowState = System.Windows.WindowState.Maximized;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Bootstraper.Init(this, Canvas);

            Engine.Scene = CreateScene();
            CreateCamera(Engine.Scene);
            CreateTerrain();
            CreateLight(Engine.Scene);

            Engine.Presenter.Rendering += Presenter_Rendering;
            Engine.StartGameLoop();
        }

     

        void Presenter_Rendering()
        {
            if (_fps == -1)
            {
                _fps = 0;
                _baseTime = Engine.Time.Time;
            }
            else
            {
                float time = Engine.Time.Time;
                if ((time - _baseTime) > 1.0f)
                {
                    Dispatcher.Invoke(delegate ()
                    {
                        FPS.Text = _fps.ToString();
                    });

                    _fps = 0;
                    _baseTime = time;
                }
                _fps++;
            }
     
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Engine.Presenter.Rendering -= Presenter_Rendering;
            Engine.StopGameLoop();

            base.OnClosing(e);
        }

        private Scene CreateScene()
        {
            var scene = new Scene("Default");

            scene.AmbientLight.SkyColor = new Vector3(0.8f);
            scene.AmbientLight.GroundColor = new Vector3(0.2f);

            //Engine.BackColor = new Color4(System.Drawing.Color.LightBlue.ToArgb());
            
            return scene;
        }

        private Igneel.SceneManagement.Frame CreateCamera(Scene scene)
        {
                        
            var zn = 0.05f;
            var zf = 1000f;           
            var fov = Numerics.ToRadians(60);          
            float aspect = (float)Engine.Graphics.BackBuffer.Width / (float)Engine.Graphics.BackBuffer.Height;

            var controller = new FpController()
            {
                MoveScale = 10.0f,
                RotationScale = 0.5f,
                BreakingTime = 0.2f,
                UpdateCallback = c => Engine.Mouse.IsButtonPresed(Igneel.Input.MouseButton.Middle) ||
                                      (Engine.Mouse.IsButtonPresed(Igneel.Input.MouseButton.Left) &&
                                       Engine.KeyBoard.IsKeyPressed(Keys.Lalt))
            };

            var camera = scene.Create("cameraNode",
                Camera.FromOrientation("camera", zn: zn, zf: zf).SetPerspective(fov, aspect),
                localPosition: new Vector3(0, 200, -30), 
                localRotation: new Euler(0, Numerics.ToRadians(60), 0).ToMatrix());

            camera.CommitChanges();

            controller.Node = camera;
       
            scene.Dynamics.Add(new Dynamic(x => controller.Update(x)));
            
            return camera;
        }

        private void CreateLight(Scene scene)
        {
            var light = new Light("WhiteLight")
            {
                Diffuse = new Color3(1,1,1),
                Specular = new Color3(0,0,0),
                SpotPower = 8,
                Enable=true
            };

            var lightFrame = new FrameLight(light);
           var node =  scene.Create("LightNode", lightFrame,
                new Vector3(0, 50, 0), new Euler(0, 60, 0));

            node.CommitChanges();
          
            //scene.Lights.Add(light);

        }

        private void CreateTerrain()
        {
            Texture2D heigMap = Engine.Graphics.CreateTexture2DFromFile("terrain.png");

            HeightField heigthField = new HeightField(heigMap, 8, 8);

            heigthField.Materials[0].DiffuseMaps = new Texture2D[]
            {
                Engine.Graphics.CreateTexture2DFromFile("grass.jpg")
            };

            heigthField.Smoot(5, 4);

            Engine.Scene.Create("HeightFieldNode", heigthField,
                Igneel.Matrix.Translate(-0.5f, 0, -0.5f) *
                Igneel.Matrix.Scale(1000, 100, 1000));
        }
    }
}
