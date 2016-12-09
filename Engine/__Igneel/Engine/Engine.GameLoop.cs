using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Igneel.Rendering;
using Igneel.Imagin;
using SlimDX.Direct3D9;
using SlimDX;

namespace Igneel
{
    public enum EngineLoopStage { None, PreRender, Render, PostRender ,Update ,Present }

    public partial class Engine
    {                    
      
        private static void UpdateScene()
        {
            if (!lostGraphics)
            {              
                engineLoopStage = EngineLoopStage.Update;
                float elapsed = frameRate;
                if (firstFrame)
                {
                    gametimer.Reset();
                    firstFrame = false;
                }
                if(!constantFrameRate)
                     elapsed = gametimer.Elapsed();
                
                elapseTime = elapsed;
                sceneManager.UpdateSceneState();
                
                if (input != null)
                    input.Poll();
               
                if (scene != null)
                {                   
                    OnSceneUpdate();
                    scene.Update(elapsed);
                }                

                engineLoopStage = EngineLoopStage.None;
            }
        }

        private static void RenderScene()
        {
            if (!lostGraphics)
            {
                graphics.SetRenderState(RenderState.MultisampleAntialias, allowAntialiazing);

                graphics.Viewport = new Viewport(0, 0, backBuffer.Width, backBuffer.Height);
                renderManager.SetRenderTarget2D(0, backBuffer);
                renderManager.EndRender();

                #region PreRender Stage

                graphics.SetRenderState(RenderState.FillMode, (int)Engine.Shading.FillMode);
                graphics.SetRenderState(RenderState.CullMode, (int)Engine.Shading.CullMode);              

                graphics.Clear(ClearFlags.Target | ClearFlags.ZBuffer, BackColor, 1, 0);
                graphics.BeginScene();

                engineLoopStage = EngineLoopStage.PreRender;

                if(scene!=null)
                    scene.ApplyTechniques();

                OnPreRenderScene();                    

                graphics.EndScene();

                #endregion

                if (presenter.Enable)
                {
                    engineLoopStage = EngineLoopStage.Render;
                    presenter.Render();

                    engineLoopStage = EngineLoopStage.PostRender;
                    presenter.PostRender();


                    engineLoopStage = EngineLoopStage.Present;
                    presenter.Present();

                    OnPresented();   
                }                            
            }
            engineLoopStage = EngineLoopStage.None;
        }       

       
    }   
}

