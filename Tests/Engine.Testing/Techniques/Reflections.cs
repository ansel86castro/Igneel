using Igneel;
using Igneel.Graphics;
using Igneel.Importers;
using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Igneel;
using Igneel.Effects;
using Igneel.Techniques;

namespace D3D9Testing.Techniques
{
    [Test]
    class Reflections
    {
         private RasterizerState _rastState;   
        BoxBuilder _boxBuilder;
        private IGraphicBuffer _vb;        

        public Reflections()
        {
            _boxBuilder = new BoxBuilder(2, 2, 1);
            Vector3 translation = new Vector3(0, 0, 0.5f);
            for (int i = 0; i < _boxBuilder.Vertices.Length; i++)
            {
                _boxBuilder.Vertices[i].Position += translation;
            }
            _vb = GraphicDeviceFactory.Device.CreateVertexBuffer<MeshVertex>(data: _boxBuilder.Vertices);
        }

        [TestMethod]
        public void TestEnvironmentMap()
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                if (d.ShowDialog() == DialogResult.OK)
                {
                    SceneTests.InitializeScene();
                    var content = ContentImporter.Import(SceneManager.Scene, d.FileName);                                     
                    var technique = SceneManager.Scene.EnumerateNodesInPreOrden().Where(x => x.Technique is EnvironmentMapTechnique).Select(x => (EnvironmentMapTechnique)x.Technique).FirstOrDefault();
                    Engine.Presenter.Rendering += () =>
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
                        var effect = Service.Require<RenderMeshColorEffect>();

                        effect.U.ViewProj = SceneManager.Scene.ActiveCamera.ViewProj;
                        effect.U.Color = new Vector4(1);

                        device.RasterizerStack.Push(_rastState);
                        device.PrimitiveTopology = IaPrimitive.TriangleList;
                        device.SetVertexBuffer(0, _vb, 0);
                        var oldtech = effect.Technique;
                        foreach (var pass in effect.Passes(1))
                        {
                            effect.Apply(pass);
                            foreach (var camera in technique.Cameras)
                            {
                                effect.U.World = camera.InvViewProjection;
                                device.Draw(_boxBuilder.Vertices.Length, 0);
                                //device.DrawIndexedUser(0, boxBuilder.Vertices.Length, boxBuilder.Indices.Length / 3, boxBuilder.Indices, boxBuilder.Vertices);
                            }
                        }
                        effect.EndPasses();

                        effect.Technique = oldtech;
                        device.RasterizerStack.Pop();
                    };
                }
            }
        }    


        [TestMethod]
        public void TestPlanarReflection()
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                if (d.ShowDialog() == DialogResult.OK)
                {
                    SceneTests.InitializeScene();
                    var content = ContentImporter.Import(SceneManager.Scene, d.FileName);
                    ReflectiveNodeTechnique technique = SceneManager.Scene.EnumerateNodesInPreOrden().Where(x => x.Technique is ReflectiveNodeTechnique).Select(x => (ReflectiveNodeTechnique)x.Technique).FirstOrDefault();
                    Engine.Presenter.Rendering += ()=>
                    {                        
                         var untranformed = Service.Require<RenderQuadEffect>();
                        var sprite = Service.Require<Sprite>();
                        GraphicDeviceFactory.Device.Ps.SetResource(0, technique.ReflectionTexture);

                        untranformed.U.alpha = 1;
                        untranformed.Technique = 1;

                        sprite.Begin();
                        sprite.SetTrasform(untranformed, new Rectangle(0, 0, 256, 256), Matrix.Identity);
                        sprite.DrawQuad(untranformed);
                        sprite.End();           
                    };
                }
            }
        }
    }
}
