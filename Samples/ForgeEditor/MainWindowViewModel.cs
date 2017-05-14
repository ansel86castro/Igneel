using Igneel;
using Igneel.Controllers;
using Igneel.Graphics;
using Igneel.Input;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using Igneel.Services;
using ForgeEditor.Components;
using System.Drawing;
using ForgeEditor.ViewModel;
using Igneel.Techniques;
using Igneel.States;

namespace ForgeEditor
{
    public class MainWindowViewModel:NotifyPropertyChanged
    {
        private Scene defaultScene;
        private Frame defaultCamera;
        private DesignEnvironment grid;
        private CoordinateGlyp coordinateGlyp;
        private Frame gridFrame;
        private IMainShell windows;
        private HitTestService hitTestService;
        TransformationController transformationController;
        private bool initialized;
        private bool enableTranslate;
        private bool enableRotate;
        private bool enableScale;

        public event EventHandler<Scene> SceneCreated;
        public event EventHandler<Scene> ContentLoaded;
        public event EventHandler<HitTestResult> HitTestPerformed;

        public MainWindowViewModel(IMainShell windows){
            this.windows = windows;
            LoadFileCommand = new ActionCommand(LoadFile);
            Service.Set(this);
        }

        public TransformationController TransformationController { get { return transformationController; }
            set { transformationController = value; }
        }

        public CoordinateGlyp CoordinateGlyp { get { return coordinateGlyp; }
            set { coordinateGlyp = value; }
        }

        public void Init()
        {
            var swapChaing = GraphicDeviceFactory.Device.DefaultSwapChain;

            hitTestService = new HitTestService(Engine.Presenter, swapChaing.BackBuffer.Size);

            defaultScene = _CreateScene();
            defaultCamera = _CreateCamera(defaultScene);

            Engine.Scene = defaultScene;
           
            grid = new DesignEnvironment();
            gridFrame = defaultScene.Create("GRID", grid, Matrix.Identity);
            gridFrame.IsDesignOnly = true;


            coordinateGlyp = new CoordinateGlyp(new Igneel.Rectangle(0, 0, 64, 64))
            {
                EnablePlanes = false,
                ArrowRadius = 2,
                ArrowHeight = 3
            }.Initialize();

            defaultScene.Decals.Add(coordinateGlyp);

            transformationController = new TransformationController(windows);

            StartGameLoop();

            initialized = true;

            windows.Canvas3D.KeyDown += Canvas3D_KeyDown;
            if (SceneCreated != null)
            {
                SceneCreated(this, defaultScene);
            }
           
        }

        void Canvas3D_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == System.Windows.Forms.Keys.Z)
            {
                Engine.Invoke(() =>
                    transformationController.TransformBack());
            }
        }

        public ActionCommand LoadFileCommand { get; private set; }
        
        private Scene _CreateScene()
        {
            var scene = new Scene("Defaul");

            scene.AmbientLight.SkyColor = new Vector3(0.8f);
            scene.AmbientLight.GroundColor = new Vector3(0.2f);

            Engine.BackColor = new Color4(Color.LightBlue.ToArgb());
            return scene;
        }

        private Frame _CreateCamera(Scene scene)
        {
            var linearAceleration = 1000f;
            var linearVelocity = 100f;
            var angularAceleration = Numerics.ToRadians(1000);
            var angularVelocity = Numerics.ToRadians(90);
            var breakingFactor = 0.0f;
            var localRadius = 1.5f;
            var zn = 0.05f;
            var zf = 3000f;
            var offset = 100;
            var fov = Numerics.ToRadians(60);
            Vector3 pos = new Vector3(0, offset, -offset);
            float aspect = (float)GraphicDeviceFactory.Device.BackBuffer.Width / 
                            (float)GraphicDeviceFactory.Device.BackBuffer.Height;
          
            var controller = new FpController()
            {
                MoveScale = 10.0f,
                RotationScale = 0.5f,
                BreakingTime = 0.2f,
                UpdateCallback = c => Engine.Mouse.IsButtonPresed(Igneel.Input.MouseButton.Middle) ||
                                      (Engine.Mouse.IsButtonPresed(Igneel.Input.MouseButton.Left) &&
                                       Engine.KeyBoard.IsKeyPressed(Keys.Lalt))
            };

            var camera = scene.Create("camera1",
                Camera.FromOrientation("camera1", zn: zn, zf: zf).SetPerspective(fov, aspect),
                localPosition: pos,
                localRotation: new Euler(0, Numerics.ToRadians(30), 0).ToMatrix());            
            camera.CommitChanges();
            controller.Node = camera;
            scene.Dynamics.Add(new Dynamic(x => controller.Update(x)));
            return camera;
        }

        public void StartGameLoop()
        {
            Engine.StartGameLoop();
        }

        public void StopGameLoop()
        {           
            Engine.StopGameLoop();
        }

        public void HitTest(Vector2 location)
        {
            Engine.Invoke(delegate
            {
                
                var component = transformationController.HitTest(location);
                if (component == null)
                {
                    var results = hitTestService.HitTest(location);
                    var result = results.FirstOrDefault();
                    if (result.Drawable != null)
                    {
                        var frame = result.Frame;
                        if (frame != null)
                            transformationController.ShowTransformGlyp(frame);

                        //TODO Render Boundings                        
                    }
                    else
                        transformationController.SelectedFrame = null;

                    this.windows.Dispatcher.Invoke(delegate
                    {
                        if (HitTestPerformed != null)
                        {
                            HitTestPerformed(this, result);
                        }
                    });
                                                
                }
            });

        }

        private void LoadFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();            
            dialog.CheckFileExists = true;

            dialog.DereferenceLinks = true;
            dialog.Filter = "COLLADA Documents|*.dae;*.DAE| All Files|*.*";

            if (dialog.ShowDialog() == true)
            {
                ProgressDialog progressDialog = new ProgressDialog()
                {
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
                };
                progressDialog.Show();
                //windows.ShowProgressDialog();
                Engine.Invoke(delegate
                {
                    Igneel.Importers.ContentImporter.Import(Engine.Scene, dialog.FileName);

                    foreach (var lightInstance in Engine.Scene.FrameLights)
                    {
                        var sm = lightInstance.CreateShadowMap();
                        if (sm != null)
                        {
                            var frustum = new ShadowMapGlyp(Engine.Scene, sm);
                            Engine.Scene.Decals.Add(frustum);
                        }
                    }

                    if (!EngineState.Shadow.ShadowMapping.Enable)
                    {
                        EngineState.Shadow.ShadowMapping.Enable = true;
                        EngineState.Shadow.Enable = true;
                    }

                    windows.Dispatcher.Invoke(delegate 
                    {
                        if (ContentLoaded != null)
                        {
                            ContentLoaded(this, Engine.Scene);
                        }                  

                        progressDialog.Close(); 
                    });
                });
            }
        }
      
        private void OnTransformChanged()
        {
            OnPropertyChanged(() => EnableScale);
            OnPropertyChanged(() => EnableRotate);
            OnPropertyChanged(() => EnableTranslate);
        }

        public bool EnableTranslate
        {
            get { return enableTranslate; }
            set
            {
                enableTranslate = value;
                enableScale = !value;
                enableRotate = !value;
                Engine.Invoke(() => transformationController.Transformation = value ? TransformationController.TransformationType.Translation : TransformationController.TransformationType.None);
                OnTransformChanged();
            }
        }

        public bool EnableRotate
        {
            get { return enableRotate; }
            set
            {
                enableRotate = value;
                enableScale = !value;
                enableTranslate = !value;
               Engine.Invoke(()=>  transformationController.Transformation =value? TransformationController.TransformationType.Rotation: TransformationController.TransformationType.None);
               OnTransformChanged();
            }
        }

        public bool EnableScale
        {
            get { return enableScale; }
            set
            {
                enableScale = value;
                enableRotate = !value;
                enableTranslate = !value;

                Engine.Invoke(() => transformationController.Transformation = value ? TransformationController.TransformationType.Scale : TransformationController.TransformationType.None);
                OnTransformChanged();
            }
        }

        public void HitMove(System.Drawing.Point location)
        {
            Engine.Invoke(delegate
            {
                transformationController.HitMove(new Vector2(location.X, location.Y));
            });
        }

    }
}
