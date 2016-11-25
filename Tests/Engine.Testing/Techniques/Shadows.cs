using Igneel;
using Igneel.Assets;
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
using Igneel.Assets;
using Igneel.Components;
using Igneel.Effects;
using Igneel.SceneComponents;
using Igneel.States;
using Igneel.Techniques;

namespace D3D9Testing.Techniques
{
    [Test]
    public class Shadows
    {       
        private RasterizerState _rastState;
        Camera _targetCamera;
        Mesh _box;
        Matrix _translation;
        private ShadowMapTechnique _technique;
        IGraphicBuffer _vb;

        public Shadows()
        {
            _box = Mesh.CreateBox(2, 2, 1);
            _translation = Matrix.Translate(0, 0, 0.5f);                       
        }

        ContentPackage ImportContent()
        {
            ContentPackage content = null;
            using (OpenFileDialog d = new OpenFileDialog())
            {
                if (d.ShowDialog() == DialogResult.OK)
                {
                    SceneTests.InitializeScene();
                    content = ContentImporter.Import(SceneManager.Scene, d.FileName);
                }
            }
            return content;
        }

        [TestMethod]
        public void Import()
        {         
                    SceneTests.InitializeScene();
            
                    //RenderManager.PushTechnique<DefferedLigthing<DefferedLightingEffect>>();
                    //EngineState.Shadow.Enable = true;
                    //EngineState.Shadow.ShadowMapping.Enable = true;
                    //Deffered.DrawGBuffers<DefferedShadowRender>();

                   // var content = ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\shadowScene.DAE");
                    var content = ImportContent();
                    if(content == null)
                        return;

                    if (SceneManager.Scene.Physics != null)
                        SceneManager.Scene.Physics.Enable = true;

                    if (SceneManager.Scene.Lights.Count == 0)
                    {
                        var light = new Light()
                        {
                            Diffuse = new Vector3(1, 1, 1),
                            Specular = new Vector3(1, 1, 1),
                            Type = LightType.Directional,
                            Enable = true
                        };

                        SceneManager.Scene.Create("DirectionalLight0", new FrameLight(light),
                            localRotationEuler:new Euler(0, Numerics.ToRadians(70), 0));                           

                        FrameLight.CreateShadowMapForAllLights(SceneManager.Scene);
                    }
                    _technique = SceneManager.Scene.Lights.Where(x => x.Node.Technique is ShadowMapTechnique).Select(x => (ShadowMapTechnique)x.Node.Technique).FirstOrDefault();
                    if (_technique == null)
                    {
                        FrameLight.CreateShadowMapForAllLights(SceneManager.Scene);
                        _technique = SceneManager.Scene.Lights.Where(x => x.Node.Technique is ShadowMapTechnique).Select(x => (ShadowMapTechnique)x.Node.Technique).FirstOrDefault();
                    }
                    _targetCamera = _technique.Camera;
                    _technique.KernelSize = 3;      
                    //targetCamera.ZNear = 1;
                    //targetCamera.ZFar = 1000;
                    //targetCamera.AspectRatio = 1;
                    //targetCamera.FieldOfView = Numerics.PIover6;
                    //targetCamera.Type = ProjectionType.Perspective;
                    //targetCamera.CommitChanges();
                    _technique.Bias = 0.9e-2f;                  

                    SceneManager.Scene.AmbientLight.GroundColor = new Vector3(0, 0, 0);
                    SceneManager.Scene.AmbientLight.SkyColor = new Vector3(0.2f, 0.2f, 0.2f);
                    //EngineState.Lighting.Reflection.UseDefaultTechnique = true;
                  
                    Engine.Presenter.Rendering += Presenter_Rendering;
                    SceneManager.Scene.Dynamics.Add(new Dynamic(x =>
                    {
                        if (Engine.KeyBoard.IsKeyPressed(Igneel.Input.Keys.D1))
                            _technique.KernelSize = 3;
                        else if (Engine.KeyBoard.IsKeyPressed(Igneel.Input.Keys.D2))
                            _technique.KernelSize = 5;
                        else if (Engine.KeyBoard.IsKeyPressed(Igneel.Input.Keys.D3))
                            _technique.KernelSize = 7;
                        else if (Engine.KeyBoard.IsKeyPressed(Igneel.Input.Keys.D4))
                            _technique.KernelSize = 1;
                    }));
            //    }
            //}
        }

        [TestMethod]
        public void AutomaticShadowMapping()
        {
            SceneTests.InitializeScene();
            var content = ImportContent();
            content.OnSceneAttach(SceneManager.Scene);

            //ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\nissan2.DAE");
            //ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\talia.DAE");            

            if (SceneManager.Scene.Physics != null)
                SceneManager.Scene.Physics.Enable = true;

            if (SceneManager.Scene.Lights.Count == 0)
            {
                var light = new Light()
                {
                    Diffuse = new Vector3(1, 1, 1),
                    Specular = new Vector3(1, 1, 1),
                    Type = LightType.Directional,
                    Enable = true
                };

                SceneManager.Scene.Create("DirectionalLight0", new FrameLight(light),
                    localRotationEuler: new Euler(0, Numerics.ToRadians(70), 0));                
            }
            FrameLight.CreateShadowMapForAllLights(SceneManager.Scene);
            _technique = SceneManager.Scene.Lights.Where(x => x.Node.Technique is ShadowMapTechnique).Select(x => (ShadowMapTechnique)x.Node.Technique).FirstOrDefault();
            _targetCamera = _technique.Camera;

            _technique.Bias = 0.9e-2f;

            SceneManager.Scene.AmbientLight.GroundColor = new Vector3(0, 0, 0);
            SceneManager.Scene.AmbientLight.SkyColor = new Vector3(0.2f, 0.2f, 0.2f);

            Engine.Presenter.Rendering += Presenter_Rendering;
            SceneManager.Scene.Dynamics.Add(new Dynamic(x =>
            {
                if (Engine.KeyBoard.IsKeyPressed(Igneel.Input.Keys.D1))
                    _technique.KernelSize = 3;
                else if (Engine.KeyBoard.IsKeyPressed(Igneel.Input.Keys.D2))
                    _technique.KernelSize = 5;
                else if (Engine.KeyBoard.IsKeyPressed(Igneel.Input.Keys.D3))
                    _technique.KernelSize = 7;
                else if (Engine.KeyBoard.IsKeyPressed(Igneel.Input.Keys.D4))
                    _technique.KernelSize = 1;
            }));
        }

        [TestMethod]
        public void EdgeFiltering()
        {
            SceneTests.InitializeScene();
            
            //var content = ImportContent();
            var content = ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\shadowScene.DAE");

            EngineState.Shadow.ShadowMapping.PcfBlurSize = 5;
            content.OnSceneAttach(SceneManager.Scene);

            //ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\nissan2.DAE");
            //ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\talia.DAE");            

            if (SceneManager.Scene.Physics != null)
                SceneManager.Scene.Physics.Enable = true;

            //if (SceneManager.Scene.Lights.Count == 0)
            //{
            //    var light = new Light()
            //    {
            //        Diffuse = new Vector3(1, 1, 1),
            //        Specular = new Vector3(1, 1, 1),
            //        Type = LightType.Directional,
            //        Enable = true
            //    };

            //    SceneManager.Scene.Create("DirectionalLight0", new LightInstance(light),
            //        localRotationEuler: new Euler(0, Numerics.ToRadians(70), 0));
            //}
            FrameLight.CreateShadowMapForAllLights(SceneManager.Scene);
            _technique = SceneManager.Scene.Lights.Where(x => x.Node.Technique is ShadowMapTechnique).Select(x => (ShadowMapTechnique)x.Node.Technique).FirstOrDefault();
            _targetCamera = _technique.Camera;

            _technique.Bias = 0.9e-2f;

            SceneManager.Scene.AmbientLight.GroundColor = new Vector3(0, 0, 0);
            SceneManager.Scene.AmbientLight.SkyColor = new Vector3(0.2f, 0.2f, 0.2f);

            EngineState.Shadow.ShadowMapping.PcfBlurSize = 3;
            var edgeTechnique = new EdgeShadowFilteringTechnique();
            RenderManager.PushTechnique(edgeTechnique);
            bool debug = true;

            if (debug)
            {
                Form form = new Form();
                form.BackColor = Color.Blue;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Size = new System.Drawing.Size(edgeTechnique.ShadowFactorTex.Width, edgeTechnique.ShadowFactorTex.Height);

                form.SuspendLayout();

                Canvas3D canvas = new Canvas3D()
                {
                    Width = form.Width,
                    Height = form.Height
                };
                canvas.Dock = DockStyle.Fill;
                var presenter = canvas.CreateSwapChainPresenter();
                form.Controls.Add(canvas);
                form.ResumeLayout();

                Engine.RenderFrame += () =>
                {
                    presenter.Begin(new Color4(Color.Aqua.ToArgb()));

                    var device = GraphicDeviceFactory.Device;
                    device.Ps.SamplerStacks[0].Push(SamplerState.Linear);                    
                    device.Blend = SceneTechnique.NoBlend;

                    var texture = edgeTechnique.EdgeSrcTexture;
                    RenderTexture(device, texture, width: texture.Width, height: texture.Height);

                    device.Ps.SamplerStacks[0].Pop();

                    presenter.End();
                };

                form.Show();
            }            
         
        }
       

        void screen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                    _technique.KernelSize = 3;
                    break;
                case Keys.D2:
                    _technique.KernelSize = 5;
                    break;
                case Keys.D3:
                    _technique.KernelSize = 7;
                    break;
            }                   
        }        

        void Presenter_Rendering()
        {
            if (_rastState == null)
            {
                _rastState = GraphicDeviceFactory.Device.CreateRasterizerState(new RasterizerDesc(true)
                {
                    Fill = FillMode.Wireframe,
                    Cull = CullMode.None
                });
            }
            var device = GraphicDeviceFactory.Device;
            var effect = Service.Require<RenderMeshIdEffect>();            

            effect.U.World = _translation * _targetCamera.InvViewProjection;
            effect.U.ViewProj = SceneManager.Scene.ActiveCamera.ViewProj;
            effect.U.gId = new Vector4(1);

            device.RasterizerStack.Push(_rastState);
            _box.Draw(device, effect);            
            device.RasterizerStack.Pop();

            RenderTexture(device, _technique.DepthTexture);
        }
      
        private void RenderTexture(GraphicDevice device, ITexture texture , int x = 0, int y=0, int width =256, int height = 256)
        {
            var untranformed = Service.Require<RenderQuadEffect>();
            var sprite = Service.Require<Sprite>();
            device.Ps.SetResource(0, texture );
            device.Ps.SetSampler(0, SamplerState.Linear);

            sprite.Begin();
            sprite.SetTrasform(untranformed, new Igneel.Rectangle(x, y, width, height), Matrix.Identity);
            sprite.DrawQuad(untranformed);
            sprite.End();

            device.Ps.SetResource(0, null);
        }
    }
}
