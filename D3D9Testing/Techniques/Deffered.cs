﻿using Igneel;
using Igneel.Components;
using Igneel.Graphics;
using Igneel.Importers;
using Igneel.Rendering;
using Igneel.Rendering.Effects;
using Igneel.Windows.Forms;
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
    public class Deffered
    {
        SceneNode[] lights;

        [TestMethod]
        public void Import()
        {
            TestSettings.UseFrameLines = false;
            SceneTests.InitializeScene();
            Engine.Scene.AmbientLight.SkyColor = new Vector3(0.0f, 0.0f, 0.0f);
            Engine.Scene.AmbientLight.GroundColor = new Vector3(0, 0, 0);

            Engine.PushTechnique<DefferedLigthing<DefferedLightingEffect>>();            
            DrawGBuffers<DefferedLightingEffect>();

            Engine.Scene.Create("boxMesh", new MeshInstance(new MeshMaterial[]{ new MeshMaterial{
                         //Diffuse = new Vector3(0.2f , 0.2f, 0.2f)
                         Diffuse = new Vector3(1f , 1f, 1f)
                          //SpecularIntensity = 1,
                    }}, Mesh.CreateBox(1000f, 0.1f, 1000f)), Matrix.Identity);

            CreateLights();

            //ContentImporter.Import(Engine.Scene, @"E:\Modelos\2PINCHA CASA\terreno.DAE");
            //ContentImporter.Import(Engine.Scene, @"I:\3D Media\Elementalist - Soul of the Ultimate Nation Character\Texture\rabbi.DAE");
            ContentImporter.Import(Engine.Scene, @"C:\Users\ansel\Documents\3dsmax\export\nissan2.DAE")
                .OnAddToScene(Engine.Scene);
            //ContentImporter.Import(Engine.Scene, @"E:\Modelos\CITIZEN EXTRAS_FEMALE 02.dae");
           
        }

        public static void DrawGBuffers<T>()
            where T: Effect
        {
            var technique = Engine.ActiveTechnique as DefferedLigthing<T>;
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
                    presenter.Begin(Color.Aqua);

                    var device = Engine.Graphics;                    
                    var untranformed = Service.Require<RenderQuadEffect>();

                    var sprite = Service.Require<Sprite>();
                    sprite.Begin();
                    var width = device.OMGetRenderTarget(0).Width / 2;
                    var height = device.OMGetRenderTarget(0).Height / 2;

                    device.PSStage.SetSampler(0, SamplerState.Point);
                    device.OMBlendState = SceneTechnique.NoBlend;
                    var textures = technique.Textures;
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            device.PSStage.SetResource(0, textures[i * 2 + j]);
                            sprite.SetTrasform(untranformed, new Igneel.Rectangle(width * j, height * i, width, height), Matrix.Identity);
                            sprite.DrawQuad(untranformed);
                        }
                    }
                    sprite.End();

                    presenter.End();
                };

                form.Show();
            }
        }
      

        private void CreateLights()
        {
            lights = new SceneNode[6];
            Random ran = new Random();

            Vector3[] colors = new Vector3[8]
            {
                new Vector3(Color.Yellow) ,new Vector3(Color.Red), new Vector3(Color.Green), new Vector3(Color.LightCoral),
                new Vector3(Color.DarkBlue) ,new Vector3(Color.Gray), new Vector3(Color.IndianRed), new Vector3(Color.LightSalmon)
            };

            float step = Numerics.TwoPI / lights.Length;
            Spherical spherical = new Spherical(Numerics.PIover2,0);
            for (int i = 0; i < lights.Length; i++)
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

                var instance =new LightInstance(light);
                var node = Engine.Scene.Create("light" + i, instance, 
                    localPosition: pos,
                    localRotationEuler: Euler.FromDirection(new Vector3(0, -1, 0)),
                    localScale:new Vector3(1,1,1));
                Engine.Scene.Dynamics.Add(new Dynamic(x=>
                    {
                        SceneNode n = node;
                        n.LocalPosition = Vector3.TransformCoordinates(n.LocalPosition,
                                            Matrix.RotationY(Numerics.ToRadians(1))); 
                        n.UpdateLocalPose();
                        n.CommitChanges();
                    }));
                lights[i] = node;
            }
        }
    }
}
