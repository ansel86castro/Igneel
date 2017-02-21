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

        private float fps;
        private float baseTime;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Bootstraper.Init(this, Canvas);

            CreateScene();

            CreateCamera();

            CreateTerrain();

            CreateLight();

            Engine.Presenter.Rendering += Presenter_Rendering;

            Engine.StartGameLoop();
        }

     

        private void CreateScene()
        {
            //Create a scene
            //The scene is the primary container for all the graphic objects
            var scene = new Scene("Default Scene");

            //Set scene hemispherical ambient colors
            scene.AmbientLight.SkyColor = new Vector3(0.8f);
            scene.AmbientLight.GroundColor = new Vector3(0.2f);
            
            //Set the engine scene
            Engine.Scene = scene;            
        }

        private void CreateCamera()
        {
            var scene = Engine.Scene;             
            
            //Compute the render target aspect ratio                                      
            float aspect = (float)Engine.Graphics.BackBuffer.Width / (float)Engine.Graphics.BackBuffer.Height;

            //Create a First-Person camera controller. The controller will responds to user inputs and move the camera
            var controller = new FpController()
            {
                MoveScale = 10.0f,
                RotationScale = 0.5f,
                BreakingTime = 0.2f,

                //The callback that is called during a frame update ,if it returns true the camera respond to the user input
                UpdateCallback = c => Engine.Mouse.IsButtonPresed(Igneel.Input.MouseButton.Middle) ||
                                      (Engine.Mouse.IsButtonPresed(Igneel.Input.MouseButton.Left) &&
                                       Engine.KeyBoard.IsKeyPressed(Keys.Lalt)),

                //Create the camera and the camera node
                Node = scene.Create("cameraNode", Camera.FromOrientation("camera", zn: 0.05f, zf: 1000f).SetPerspective(Numerics.ToRadians(60), aspect),
                            localPosition: new Vector3(0, 200, -30),
                            localRotation: new Euler(0, Numerics.ToRadians(60), 0).ToMatrix())
            };
                           
            scene.Dynamics.Add(new Dynamic(x => controller.Update(x)));          
        }

        private void CreateLight()
        {
            //Create a light ,the light containg properties like colors and specular powers
            var light = new Light("WhiteLight")
            {
                Diffuse = new Color3(1,1,1),
                Specular = new Color3(0,0,0),
                SpotPower = 8,
                Enable=true
            };

            //Assign the light to a FrameLight wich acts like and adapter for the scene node 
            //so it will set light spatial properties like direccion and position when the scene node change its pose.
            var lightFrame = new FrameLight(light);

            Engine.Scene.Create("LightNode", lightFrame, new Vector3(0, 50, 0), new Euler(0, 60, 0));                              

        }

        private void CreateTerrain()
        {
            //Load the height map
            Texture2D heigthMap = Engine.Graphics.CreateTexture2DFromFile("terrain.png");

            //Create the HeightField using the heigth map ,divide the HeightField into and 8x8 grid of sections 
            //this will improve culling
            HeightField heigthField = new HeightField(heigthMap, 8, 8);

            heigthField.Materials[0].Diffuse =Color3.FromArgb(System.Drawing.Color.DarkGreen.ToArgb());

            //Uncomment this to texture the terrain
            //heigthField.Materials[0].DiffuseMaps = new Texture2D[]
            //{
            //    Engine.Graphics.CreateTexture2DFromFile("grass.jpg")
            //};

            //smoot the height field using a 5x5 gaussian kernel with 4 pass
            heigthField.Smoot(5, 4);

            //Create the HeightField node translat it to the center of the scene, then scaling it to 1000 units in X and Z
            Engine.Scene.Create("HeightFieldNode", heigthField,
                Igneel.Matrix.Translate(-0.5f, 0, -0.5f) *
                Igneel.Matrix.Scale(1000, 100, 1000));
        }


        void Presenter_Rendering()
        {
            if (fps == -1)
            {
                fps = 0;
                baseTime = Engine.Time.Time;
            }
            else
            {
                float time = Engine.Time.Time;
                if ((time - baseTime) > 1.0f)
                {
                    Dispatcher.Invoke(delegate ()
                    {
                        FPS.Text = fps.ToString();
                    });

                    fps = 0;
                    baseTime = time;
                }
                fps++;
            }

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Engine.Presenter.Rendering -= Presenter_Rendering;

            Engine.StopGameLoop();

            base.OnClosing(e);
        }

    }
}
