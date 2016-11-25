using System;

namespace Igneel.Rendering
{
    public static class RenderExtensors
    {      
        public static TRender Binding<TRender>(this TRender render, Action<TRender>bindingAction)
            where TRender : Render
        {
            bindingAction(render);
            return  render;
        }



        //public static TRender Binding<TRender, T>(this TRender render)
        //    where TRender:ShaderRender
        //{
        //    var binding = Activator.CreateInstance(typeof(T), render);
        //    render.SetBinding(
        //}
    }
}