using Igneel;
using Igneel.Graphics;
using Igneel.Importers;
using Igneel.Rendering;
using Igneel.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Igneel;
using Igneel.Effects;
using Igneel.Presenters;
using Igneel.States;
using Igneel.Techniques;

namespace D3D9Testing.Techniques
{
    [Test]
    public class Hdr
    {
        [TestMethod]
        public void Import()
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                if (d.ShowDialog() == DialogResult.OK)
                {
                    SceneTests.InitializeScene();
                     EngineState.Shadow.ShadowMapping.Bias = 0.9e-2f;

                    ContentImporter.Import(SceneManager.Scene, d.FileName);                   
                    //var content = ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\shadowScene.DAE");

                    if (SceneManager.Scene.Physics != null)
                        SceneManager.Scene.Physics.Enable = true;
                    var light = SceneManager.Scene.Lights.FirstOrDefault();
                    if (light != null)
                    {
                       light.Instance.Intensity = 3;
                       //light.Instance.Specular = new Vector3(5, 5, 5);
                    }                

                    SceneManager.Scene.AmbientLight.GroundColor = new Vector3(0, 0, 0);
                    SceneManager.Scene.AmbientLight.SkyColor = new Vector3(1f, 1f, 1f);
                    EngineState.Lighting.Hdr.Enable = true;
                    EngineState.Lighting.Hdr.EnableBlueShift = false;
                    EngineState.Lighting.Hdr.GlareType = GlareLibType.Disable;
                    EngineState.Lighting.Hdr.MiddleGray = 0.5f;
                    EngineState.Lighting.Hdr.BrightThreshold = 0.8f;
                    EngineState.Lighting.Hdr.GaussianMultiplier = 0.4f;
                    EngineState.Lighting.Hdr.GaussianDeviation = 0.8f;
                    EngineState.Lighting.Hdr.StarBlendFactor = 0.2f;
                    EngineState.Lighting.Hdr.CalculateEyeAdaptation = true;
                    EngineState.Lighting.TransparencyEnable = true;
                    EngineState.Lighting.Hdr.Technique.ComputeSamples();

                    EngineState.Lighting.Reflection.UseDefaultTechnique = true;
                    EngineState.Lighting.Reflection.Enable = false;

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
                    var device = GraphicDeviceFactory.Device;
                    var hdrTechinique = EngineState.Lighting.Hdr.Technique;

                    SwapChainPresenter presenter = canvas.CreateSwapChainPresenter();

                    Texture2DDesc desc = hdrTechinique.ToneMaps[0].Texture.Description;
                    desc.Usage = ResourceUsage.Staging;
                    desc.CpuAccessFlags = CPUAccessFlags.Read;
                    desc.BindFlags = BindFlags.None;
                    ITexture2D tex = GraphicDeviceFactory.Device.CreateTexture2D(desc);

                    Action renderAction = () =>
                    {
                        presenter.Begin(new Color4(Color.Aqua.ToArgb()));
                        sprite.Begin();

                        var textures = new RenderTexture2D[4, 3]
                        {
                             {hdrTechinique.HdrScene,    hdrTechinique.BrightPassFilter, hdrTechinique.Bloom[0]},                         
                             {hdrTechinique.ToneMaps[5], hdrTechinique.ToneMaps[4],  hdrTechinique.ToneMaps[3] },
                             {hdrTechinique.ToneMaps[2] ,hdrTechinique.ToneMaps[1], hdrTechinique.ToneMaps[0] },
                             {hdrTechinique.StarSource,  hdrTechinique.StarFinal,  hdrTechinique.StarLines[1] }
                        };

                        
                        //textures[0, 1].SetTexture(0);
                        //sprite.SetTrasform(untranformed, new Igneel.Rectangle(0, 0, width, height), Matrix.Identity);                           
                        //sprite.DrawQuad(untranformed);

                        GraphicDeviceFactory.Device.Ps.SetSampler(0, SamplerState.Point);

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
