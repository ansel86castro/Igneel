using Igneel.SceneManagement;

namespace Igneel.Rendering
{
    public interface IGraphicRender : IRenderInputBinder
    {
        void Draw(GraphicSubmit comp, PixelClipping clipping);

        //protected virtual void UpdateConstantsMethod(Render obj)
        //{
        //    var scene = Engine.Scene;
        //    var camera = scene.ActiveCamera;
        //    var ambient = scene.AmbientLight;
        //    var light = Light.Current;
        //    var technique = RenderManager.ActiveTechnique;

        //    if (camera != null)
        //    {
        //        Bind(camera);
        //        camera.IsGPUSync = true;
        //    }
        //    if (EngineState.Lighting.EnableAmbient)
        //    {
        //        Bind(ambient);
        //        ambient.IsGPUSync = true;
        //    }
        //    if (light != null)
        //    {
        //        Bind(light);
        //        light.IsGPUSync = true;
        //        var node = light.Node;
        //        if (node.Technique != null && node.Technique.Enable)
        //            node.Technique.Bind(this);
        //    }
        //    technique.Bind(this);
        //}
    }
}