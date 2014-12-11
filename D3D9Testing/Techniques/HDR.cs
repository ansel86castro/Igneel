using Igneel;
using Igneel.Graphics;
using Igneel.Importers;
using Igneel.Rendering;
using Igneel.Rendering.Effects;
using Igneel.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3D9Testing.Techniques
{
    [Test]
    public class HDR
    {
        [TestMethod]
        public void Import()
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                if (d.ShowDialog() == DialogResult.OK)
                {
                    SceneTests.InitializeScene();
                     Engine.Shadow.ShadowMapping.Bias = 0.9e-2f;

                    ContentImporter.Import(Engine.Scene, d.FileName);                   
                    //var content = ContentImporter.Import(Engine.Scene, @"C:\Users\ansel\Documents\3dsmax\export\shadowScene.DAE");

                    if (Engine.Scene.Physics != null)
                        Engine.Scene.Physics.Enable = true;
                    var light = Engine.Scene.Lights.FirstOrDefault();
                    if (light != null)
                    {
                       light.Instance.Intensity = 3;
                       //light.Instance.Specular = new Vector3(5, 5, 5);
                    }                

                    Engine.Scene.AmbientLight.GroundColor = new Vector3(0, 0, 0);
                    Engine.Scene.AmbientLight.SkyColor = new Vector3(1f, 1f, 1f);
                    Engine.Lighting.HDR.Enable = true;
                    Engine.Lighting.HDR.EnableBlueShift = false;
                    Engine.Lighting.HDR.GlareType = Igneel.Rendering.GlareLibType.DISABLE;
                    Engine.Lighting.HDR.MiddleGray = 0.5f;
                    Engine.Lighting.HDR.BrightThreshold = 0.8f;
                    Engine.Lighting.HDR.GaussianMultiplier = 0.4f;
                    Engine.Lighting.HDR.GaussianDeviation = 0.8f;
                    Engine.Lighting.HDR.StarBlendFactor = 0.2f;
                    Engine.Lighting.HDR.CalculateEyeAdaptation = true;
                    Engine.Lighting.TransparencyEnable = true;
                    Engine.Lighting.HDR.Technique.ComputeSamples();

                    Engine.Lighting.Reflection.UseDefaultTechnique = true;
                    Engine.Lighting.Reflection.Enable = false;

                    Form form  = new Form();
                    form.SuspendLayout();

                    form.BackColor = Color.Blue;
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Size = new System.Drawing.Size(800,600);
                    Canvas3D canvas = new Canvas3D()
                        {
                            Width = form.Width,
                            Height = form.Height
                        };
                    canvas.Dock = DockStyle.Fill;
                    form.Controls.Add(canvas);                    
                    form.ResumeLayout();

                    int width = form.Width / 3;
                    int height = form.Height / 4;
                    
                    var untranformed = Service.Require<RenderQuadEffect>();
                    var sprite = Service.Require<Sprite>();
                    var device = Engine.Graphics;
                    var hdrTechinique = Engine.Lighting.HDR.Technique;

                    SwapChainPresenter presenter = canvas.CreateSwapChainPresenter();

                    Texture2DDesc desc = hdrTechinique.ToneMaps[0].Texture.Description;
                    desc.Usage = ResourceUsage.Staging;
                    desc.CPUAccessFlags = CpuAccessFlags.Read;
                    desc.BindFlags = BindFlags.None;
                    Texture2D tex = Engine.Graphics.CreateTexture2D(desc);

                    Action renderAction = () =>
                    {
                        presenter.Begin(Color.Aqua);
                        sprite.Begin();

                        var textures = new RenderTexture2D[4, 3]
                        {
                             {hdrTechinique.HDRScene,    hdrTechinique.BrightPassFilter, hdrTechinique.Bloom[0]},                         
                             {hdrTechinique.ToneMaps[5], hdrTechinique.ToneMaps[4],  hdrTechinique.ToneMaps[3] },
                             {hdrTechinique.ToneMaps[2] ,hdrTechinique.ToneMaps[1], hdrTechinique.ToneMaps[0] },
                             {hdrTechinique.StarSource,  hdrTechinique.StarFinal,  hdrTechinique.StarLines[1] }
                        };

                        
                        //textures[0, 1].SetTexture(0);
                        //sprite.SetTrasform(untranformed, new Igneel.Rectangle(0, 0, width, height), Matrix.Identity);                           
                        //sprite.DrawQuad(untranformed);

                        Engine.Graphics.PSStage.SetSampler(0, SamplerState.Point);

                        for (int i = 0; i < textures.GetLength(0); i++)
                        {
                            for (int j = 0; j < textures.GetLength(1); j++)
                            {
                                if (textures[i, j] != null)
                                {
                                    textures[i, j].SetTexture(0);
                                    sprite.SetTrasform(untranformed, new Igneel.Rectangle(width * j, height * i, width, height), Matrix.Identity);
                                    sprite.DrawQuad(untranformed);
                                }
                            }
                        }

                        sprite.End();
                        presenter.End();


                        //device.CopyTexture(tex, hdrTechinique.ToneMaps[0].Texture);
                        //var map =  tex.Map(0, MapType.Read);
                        //unsafe
                        //{
                        //    float* pter = (float*)map.DataPointer;
                        //    float[] data = new float[map.RowPitch];
                        //    Marshal.Copy(map.DataPointer, data, 0, data.Length);
                        //    float value = *pter;
                        //}
                        //tex.UnMap(0);

                    };
                    Engine.RenderFrame += renderAction;

                    form.Show();
                    form.FormClosing += (sender, arg) =>
                        {
                            Engine.RenderFrame -= renderAction;
                            presenter.Dispose();                            
                        };


                }
            }
        }      
    }
}
