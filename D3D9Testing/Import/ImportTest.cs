using Igneel;
using Igneel.Scenering;
using Igneel.Graphics;
using Igneel.Importers;
using Igneel.Scenering.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3D9Testing.Import
{
     [Test]
    public class ImportTest
    {         
         [TestMethod]
         public void Import()
         {
             EngineState.Lighting.TransparencyEnable = true;
             TestSettings.UseFrameLines = false;

             using (OpenFileDialog d = new OpenFileDialog())
             {
                 if (d.ShowDialog() == DialogResult.OK)
                 {
                     SceneTests.InitializeScene();
                     ContentImporter.Import(SceneManager.Scene, d.FileName)
                         .OnAddToScene(SceneManager.Scene);

                     if (SceneManager.Scene.Physics != null)
                         SceneManager.Scene.Physics.Enable = true;

                     var box = SceneManager.Scene.Nodes.First().BoundingBox;
                     var boxMesh = Mesh.CreateBox(2, 2, 2);                     
                     var rastState = GraphicDeviceFactory.Device.CreateRasterizerState(new RasterizerDesc(true)
                        {
                            Fill = FillMode.Wireframe,
                            Cull = CullMode.None
                        });

                     //Engine.Presenter.Rendering += () =>
                     //    {
                     //        var device = GraphicDeviceFactory.Device;
                     //        var effect = Service.Require<RenderMeshIdEffect>();
                     //        effect.Constants.World = Matrix.Scale(box.Extends) * box.GlobalPose;
                     //        effect.Constants.ViewProj = SceneManager.Scene.ActiveCamera.ViewProj;
                     //        effect.Constants.gId = new Vector4(1);

                     //        device.PushGraphicState<RasterizerState>(rastState);

                     //        boxMesh.Draw(device, effect);

                     //        device.PopGraphicState<RasterizerState>();
                     //    };
                 }
             }
         }

         [TestMethod]
         public void MultipleImport()
         {
             SceneTests.InitializeScene();
             ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\talia.DAE");
             ContentImporter.Import(SceneManager.Scene, @"E:\Modelos\2PINCHA CASA\terreno.DAE");
         }

         private void Presenter_Rendering()
         {             
            
         }
    }
}
