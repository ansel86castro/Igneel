using ForgeEditor.Components.CoordinateSystems;
using Igneel;
using Igneel.Components;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
using Igneel.Techniques;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeEditor.Components
{
    public class FrustumGlyp : GraphicObject<FrustumGlyp>
    {
        private static bool init;
        Mesh _box;
        Matrix _translation;
        Camera camera;
        private RasterizerState _rastState;
        private Scene scene;
      

        public FrustumGlyp(Scene scene, Camera camera)
        {
            this.camera = camera;
            _box = Mesh.CreateBox(2, 2, 1);
            _translation = Matrix.Translate(0, 0, 0.5f);

            _rastState = GraphicDeviceFactory.Device.CreateRasterizerState(new RasterizerDesc(
                fill: FillMode.Wireframe, 
                cull: CullMode.None));
            this.scene = scene;
           
            SetupRender();
        }

        private static void SetupRender()
        {
            if (!init)
            {
                SetRender<DefaultTechnique, RenderMeshIdEffect>((component, render)=>
                {
                    component.Draw(render.Effect);
                });
                SetNullRender<DepthSceneRender>();
                init = true;              
            }
        }


        public virtual void Draw(Effect effect)
        {         
            var device = GraphicDeviceFactory.Device;          

            effect.Input.World = _translation * camera.InvViewProjection;
            effect.Input.ViewProj =scene.ActiveCamera.ViewProj;
            effect.Input.Id = new Vector4(1);

            device.RasterizerStack.Push(_rastState);
            _box.Draw(device, effect);
            device.RasterizerStack.Pop();           
        }
        
    }
}
