using D3D9Testing.Import;
using Igneel;
using Igneel.Components;
using Igneel.Importers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3D9Testing.Techniques
{
    [Test]
    public class ScreenDecals
    {
        [TestMethod]
        public void ShowImage()
        {
            SceneTests.InitializeScene();
            Engine.BackColor = Color.Yellow;

            ScreenDecal decal = new ScreenDecal(null);
            decal.Texture = Engine.Graphics.CreateTexture2DFromFile( @"F:\Pictures\Yo\DSC03854.JPG" );
            //decal.Texture = Engine.Graphics.CreateTexture2DFromFile(@"F:\Pictures\gray.jpg");

            Engine.Presenter.RenderBegin += (p) =>
            {
                ScreenDecalRender render = (ScreenDecalRender)decal.GetRender();
                render.Draw(decal);
            };

            //Import3DContent();
        }

        private void Import3DContent()
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                if (d.ShowDialog() == DialogResult.OK)
                {                
                    var content = ContentImporter.Import(Engine.Scene, d.FileName);
                    if (Engine.Scene.Physics != null)
                        Engine.Scene.Physics.Enable = true;
                }
            }
        }
    }
}
