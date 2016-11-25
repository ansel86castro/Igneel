using Igneel;
using Igneel.Graphics;
using Igneel.Importers;
using Igneel.Rendering;
using Igneel.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Igneel;
using Igneel.Components;
using Igneel.Effects;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
using Igneel.Techniques;

namespace D3D9Testing.Techniques
{
    [Test]
    public class Deffered
    {
        Frame[] _lights;

        [TestMethod]
        public void Import()
        {
            TestSettings.UseFrameLines = false;
            SceneTests.InitializeScene();
            SceneManager.Scene.AmbientLight.SkyColor = new Vector3(0.0f, 0.0f, 0.0f);
            SceneManager.Scene.AmbientLight.GroundColor = new Vector3(0, 0, 0);

            RenderManager.PushTechnique<DefferedLigthing<DefferedLightingEffect>>();            
            DrawGBuffers<DefferedLightingEffect>();

            SceneManager.Scene.Create("boxMesh", new FrameMesh(new BasicMaterial[]{ new BasicMaterial{
                         //Diffuse = new Vector3(0.2f , 0.2f, 0.2f)
                         Diffuse = new Vector3(1f , 1f, 1f)
                          //SpecularIntensity = 1,
                    }}, Mesh.CreateBox(1000f, 0.1f, 1000f)), Matrix.Identity);

            CreateLights();

            //ContentImporter.Import(SceneManager.Scene, @"E:\Modelos\2PINCHA CASA\terreno.DAE");
            //ContentImporter.Import(SceneManager.Scene, @"I:\3D Media\Elementalist - Soul of the Ultimate Nation Character\Texture\rabbi.DAE");
            ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\nissan2.DAE")
                .OnSceneAttach(SceneManager.Scene);
            //ContentImporter.Import(SceneManager.Scene, @"E:\Modelos\CITIZEN EXTRAS_FEMALE 02.dae");
           
        }

        public static void DrawGBuffers<T>()
            where T: Effect
        {
            var technique = RenderManager.ActiveTechnique as DefferedLigthing<T>;
            if (technique != null)
            {
                Form form = new Form();
                form.BackColor = Color.Blue;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Size = new System.Drawing.Size(800, 600);

                form.SuspendLayout();

                Canvas3D canvas = new Canvas3D()
                    {
                         Width = form.Width,
                         Height  = form.Height
                    };
                canvas.Dock = DockStyle.Fill;
                var presenter = canvas.CreateSwapChainPresenter();
                form.Controls.Add(canvas);
                form.ResumeLayout();                

                Engine.RenderFrame += () =>
                {
                    presenter.Begin(new Color4(Color.Aqua.ToArgb()));

                    var device = GraphicDeviceFactory.Device;                    
                    var untranformed = Service.Require<RenderQuadEffect>();

                    var sprite = Service.Require<Sprite>();
                    sprite.Begin();
                    var width = device.GetRenderTarget(0).Width / 2;
                    var height = device.GetRenderTarget(0).Height / 2;

                    device.Ps.SamplerStacks[0].Push(SamplerState.Point);
                    device.Blend = SceneTechnique.NoBlend;
                    var textures = technique.Textures;
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            device.Ps.SetResource(0, textures[i * 2 + j]);
                            sprite.SetTrasform(untranformed, new Igneel.Rectangle(width * j, height * i, width, height), Matrix.Identity);
                            sprite.DrawQuad(untranformed);
                        }
                    }
                    sprite.End();

                    device.Ps.SamplerStacks[0].Pop();                    

                    presenter.End();
                };

                form.Show();
            }
        }
      

        private void CreateLights()
        {
            _lights = new Frame[6];
            Random ran = new Random();

            Color3[] colors = new Color3[8]
            {
                new Color3(Color.Yellow.ToArgb()) ,new Color3(Color.Red.ToArgb()), new Color3(Color.Green.ToArgb()), new Color3(Color.LightCoral.ToArgb()),
                new Color3(Color.DarkBlue.ToArgb()) ,new Color3(Color.Gray.ToArgb()), new Color3(Color.IndianRed.ToArgb()), new Color3(Color.LightSalmon.ToArgb())
            };

            float step = Numerics.TwoPI / _lights.Length;
            Spherical spherical = new Spherical(Numerics.PIover2,0);
            for (int i = 0; i < _lights.Length; i++)
            {
                var light = new Light()
                {
                    Diffuse = colors[i % 8],
                    //Diffuse = new Vector3((float)ran.NextDouble(),(float)ran.NextDouble(),(float)ran.NextDouble()),
                    Specular = new Vector3(0.2f, 0.2f, 0.2f),
                    Type = LightType.Point,                  
                    Enable = true
                };                
                spherical.Phi = step * i;
                var pos = spherical.ToCartesian() * 300;
                pos.Y = 50;

                var instance =new FrameLight(light);
                var node = SceneManager.Scene.Create("light" + i, instance, 
                    localPosition: pos,
                    localRotationEuler: Euler.FromDirection(new Vector3(0, -1, 0)),
                    localScale:new Vector3(1,1,1));
                SceneManager.Scene.Dynamics.Add(new Dynamic(x=>
                    {
                        Frame n = node;
                        n.LocalPosition = Vector3.TransformCoordinates(n.LocalPosition,
                                            Matrix.RotationY(Numerics.ToRadians(1))); 
                        n.ComputeLocalPose();
                        n.CommitChanges();
                    }));
                _lights[i] = node;
            }
        }
    }
}
