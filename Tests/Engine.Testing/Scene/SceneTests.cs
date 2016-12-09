using Igneel;
using Igneel.Controllers;
using Igneel.Design;
using Igneel.Graphics;
using Igneel.Input;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel;
using Igneel.Effects;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
using Igneel.Techniques;
using Igneel.Components;

namespace D3D9Testing
{
    [Test]
    public class SceneTests
    {
        [TestMethod]
        public void CreateSceneTest()
        {
            Scene scene;
            Frame camera;
            InitializeScene(out scene, out camera);
           

            TestManager.Cleaner = () =>
                {
                    camera.Remove();                  
                };
        }

        [TestMethod]
        public void DrawObject()
        {
            Scene scene;
            Frame camera;
            InitializeScene(out scene, out camera);
            Engine.BackColor = new Color4( Color.LightBlue .ToArgb());

            var box = new BoxMesh();
           // scene.Renderables.Add(box);

            TestManager.Cleaner = () =>
            {
                camera.Remove();              
            };
        }
       

        public static void InitializeScene()
        {
            var scene = CreateScene();
            CreateCamera(scene);

            CreatePhysics(scene);
        }

        private static void CreatePhysics(Scene scene)
        {
            if (Igneel.Physics.PhysicManager.Sigleton != null)
            {
                scene.Physics = Igneel.Physics.PhysicManager.Sigleton.CreatePhysic(new Igneel.Physics.PhysicDesc
                    {
                        Gravity = new Vector3(0, -9.8f, 0),
                        Flags = Igneel.Physics.SceneFlags.EnableActiveTransforms,
                        Name = scene.Name
                    });
            }
        }

        public static void InitializeScene(out Scene scene, out Frame camera)
        {
            if (Engine.Scene != null)
            {
                Engine.Scene = null;                           
            }
            scene = CreateScene();
            camera = CreateCamera(scene);
        }

        private static Scene CreateScene()
        {          
            var scene = new Scene("Default");
            scene.AmbientLight.SkyColor = new Vector3(1);
            scene.AmbientLight.GroundColor = new Vector3(0.5f);

            //if (TestSettings.UseFrameLines)
            //{
            //    var env = new DesignEnvironment();
            //    scene.Renderables.Add(env);
            //}

            Engine.BackColor =new Color4(Color.LightBlue.ToArgb());
            Engine.Scene = scene;
            return scene;
        }

        public static Frame CreateCamera(Scene scene)
        {
            var linearAceleration = 1000f;
            var linearVelocity = 100f;
            var angularAceleration = Numerics.ToRadians(1000);
            var angularVelocity = Numerics.ToRadians(90);
            var breakingFactor = 0.0f;
            var localRadius = 1.5f;
            var zn = 0.5f;
            var zf = 3000f;
            var offset = 100;
            var fov = Numerics.PIover3;
            Vector3 pos = new Vector3(0, offset, -offset);
            float aspect = (float)GraphicDeviceFactory.Device.BackBuffer.Width / (float)GraphicDeviceFactory.Device.BackBuffer.Height;

            var controller = new FpController()
                    {
                        MoveScale = 10.0f,
                        RotationScale = 0.5f,
                        BreakingTime = 0.2f,
                        UpdateCallback = c => Engine.Mouse.IsButtonPresed(MouseButton.Middle) ||
                                              (Engine.Mouse.IsButtonPresed(MouseButton.Left) &&
                                               Engine.KeyBoard.IsKeyPressed(Keys.Lalt))
                    };

            var camera =  Engine.Scene.Create("camera1",
                Camera.FromOrientation("camera1", zn: zn, zf: zf).SetPerspective(fov, aspect),
                localPosition: pos,
                localRotation: new Euler(0, Numerics.ToRadians(30), 0).ToMatrix());
            //camera.LocalRadius = localRadius;            
            camera.CommitChanges();
            controller.Node = camera;
            scene.Dynamics.Add(new Dynamic(x => controller.Update(x)));
            return camera;
        }

        [TestMethod]
        public void DrawObjectWithoutLines()
        {
            Engine.BackColor = new Color4(Color.LightBlue.ToArgb());
          

            var scene = new Scene("DefaultScene");
            Engine.Scene = scene;
            scene.AmbientLight.SkyColor = new Color3(1.0f);
            scene.AmbientLight.GroundColor = new Color3(0.5f);            
                     
            Frame camera = CreateCamera(scene);

            var box = new BoxMesh();
            scene.Create("BoxMesh", box, Matrix.Scale(5, 5, 5));
            //scene.Renderables.Add(box);

            TestManager.Cleaner = () =>
            {
                camera.Remove();

            };
        }
    
    }

    public class BoxMesh : GraphicObject<BoxMesh>
    {
        BoxBuilder _builder;
        GraphicBuffer _vb;
        GraphicBuffer _ib;
        BasicMaterial _mat;

        public BoxMesh()                       
        {
            _builder = new BoxBuilder(10,10, 10);
            _mat = BasicMaterial.CreateDefaultMaterial("boxMaterial");

            //VertexPositionColor[] vertexes = new VertexPositionColor[_builder.Vertices.Length];
            //for (int i = 0; i < vertexes.Length; i++)
            //{
            //    vertexes[i] = new VertexPositionColor(_builder.Vertices[i].Position, new Color4(1, 1, 0, 0));
            //}
            var device = GraphicDeviceFactory.Device;
            _vb = device.CreateVertexBuffer(data: _builder.Vertices);
            _ib = device.CreateIndexBuffer(data: _builder.Indices);      
      
            _mat.Diffuse = new Vector3(1, 1, 1);
            _mat.DiffuseMap = device.CreateTexture2DFromFile(@"I:\Pictures\lufy.png");
            _mat.SpecularIntensity = 0;
            _mat.EmisiveIntensity = 0;
            _mat.Alpha = 1f;
            IsTransparent = _mat.ContainsTrasparency;

            SetRender<DefaultTechnique, BasicMeshEffect>((box, render) =>
            {
                var effect = render.Effect;

                //effect.Constants.gId = new Vector4(1, 1, 0, 0);
                //effect.U.World = Matrix.Identity;
                //map.World = Matrix.Identity;

                device.PrimitiveTopology = IAPrimitive.TriangleList;
                device.SetVertexBuffer(0, _vb, 0);
                device.SetIndexBuffer(_ib, 0);

                render.Bind(_mat);
                foreach (var pass in effect.Passes(0))
                {
                    effect.Apply(pass);
                    device.DrawIndexed(_builder.Indices.Length, 0, 0);
                }
                effect.EndPasses();
            });
            //.BindWith(new CameraBinding())            
            //.BindWith(new MeshMaterialBinding())
            //.BindWith(new LightBinding())
            //.BindWith(new AmbientLightBinding())
            //.BindWith(new PixelClippingBinding());
        }

        public override void OnPoseUpdated(Frame node) { }         
    }
}
