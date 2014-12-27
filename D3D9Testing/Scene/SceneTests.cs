using Igneel;
using Igneel.Components;
using Igneel.Controllers;
using Igneel.Design;
using Igneel.Graphics;
using Igneel.Input;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using Igneel.Rendering.Effects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3D9Testing
{
    [Test]
    public class SceneTests
    {
        [TestMethod]
        public void CreateSceneTest()
        {
            Scene scene;
            SceneNode camera;
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
            SceneNode camera;
            InitializeScene(out scene, out camera);
            Engine.BackColor = Color.LightBlue;

            var box = new BoxMesh();
            scene.Renderables.Add(box);

            TestManager.Cleaner = () =>
            {
                camera.Remove();              
            };
        }

        [TestMethod]
        public void DrawObjectWithoutLines()
        {            
            var scene = new Scene();
            scene.AmbientLight.SkyColor = new Vector3(1);
            scene.AmbientLight.GroundColor = new Vector3(0.5f);           

            Engine.BackColor = Color.LightBlue;
            Engine.Scene = scene;

            Engine.BackColor = Color.LightBlue;
            SceneNode camera = CreateCamera(scene); 

            var box = new BoxMesh();
            scene.Renderables.Add(box);

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

        public static void InitializeScene(out Scene scene, out SceneNode camera)
        {
            if (Engine.Scene != null)
            {
                Engine.Scene.Remove();                           
            }
            scene = CreateScene();
            camera = CreateCamera(scene);
        }

        private static Scene CreateScene()
        {          
            var scene = new Scene();
            scene.AmbientLight.SkyColor = new Vector3(1);
            scene.AmbientLight.GroundColor = new Vector3(0.5f);

            if (TestSettings.UseFrameLines)
            {
                var env = new DesignEnvironment();
                scene.Renderables.Add(env);
            }

            Engine.BackColor = Color.LightBlue;
            Engine.Scene = scene;
            return scene;
        }

        public static SceneNode CreateCamera(Scene scene)
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
            float aspect = (float)Engine.Graphics.BackBuffer.Width / (float)Engine.Graphics.BackBuffer.Height;

            var controller = new FPController()
                    {
                        MoveScale = 10.0f,
                        RotationScale = 0.5f,
                        BreakingTime = 0.2f,
                        UpdateCallback = c => Engine.Mouse.IsButtonPresed(MouseButton.Middle) ||
                                              (Engine.Mouse.IsButtonPresed(MouseButton.Left) &&
                                               Engine.KeyBoard.IsKeyPressed(Keys.LALT))
                    };

            var camera =  Engine.Scene.Create("camera1",
                Camera.FromOrientation("camera1", zn: zn, zf: zf).SetPerspective(fov, aspect),
                localPosition: pos,
                localRotation: new Euler(0, Numerics.ToRadians(30), 0).ToMatrix());
            camera.LocalRadius = localRadius;            
            camera.CommitChanges();
            controller.Node = camera;
            scene.Dynamics.Add(new Dynamic(x => controller.Update(x)));
            return camera;
        }
    }

    public class BoxMesh : GraphicObject<BoxMesh>
    {
        BoxBuilder builder;
        GraphicBuffer vb;
        GraphicBuffer ib;
        MeshMaterial mat;

        public BoxMesh()                       
        {
            builder = new BoxBuilder(10,10, 10);
            mat = MeshMaterial.CreateDefaultMaterial("boxMaterial");

            VertexPositionColor[] vertexes = new VertexPositionColor[builder.Vertices.Length];
            for (int i = 0; i < vertexes.Length; i++)
            {
                vertexes[i] = new VertexPositionColor(builder.Vertices[i].Position, new Color4(1, 1, 0, 0));
            }

            vb = Engine.Graphics.CreateVertexBuffer(data: builder.Vertices);
            ib = Engine.Graphics.CreateIndexBuffer(data: builder.Indices);            
            mat.Diffuse = new Vector3(1, 1, 1);
            mat.SpecularIntensity = 0;
            mat.EmisiveIntensity = 0;
            mat.Alpha = 1f;
            IsTransparent = mat.ContainsTrasparency;

            SetRender<SceneTechnique, BasicMeshEffect>((box, render) =>
            {
                var device = Engine.Graphics;
                var effect = render.Effect;

                //effect.Constants.gId = new Vector4(1, 1, 0, 0);
                effect.U.World = Matrix.Identity;

                device.PrimitiveTopology = IAPrimitive.TriangleList;
                device.SetVertexBuffer(0, vb, 0);
                device.SetIndexBuffer(ib, 0);

                render.Bind(mat);
                foreach (var pass in effect.Passes(0))
                {
                    effect.Apply(pass);
                    device.DrawIndexed(builder.Indices.Length, 0, 0);
                }
                effect.EndPasses();
            })
            .BindWith(new CameraBinding())
            .BindWith(new MeshMaterialBinding())
            .BindWith(new LightBinding())
            .BindWith(new AmbientLightBinding())
            .BindWith(new PixelClippingBinding());
        }

        public override void OnPoseUpdated(SceneNode node) { }         
    }
}
