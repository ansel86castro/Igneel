using Igneel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    /// <summary>
    /// Provides bindings for the Transform.hlsli file
    /// </summary>
    public static class Transforms
    {
        public interface ICameraMap
        {           
            Matrix ViewProj { get; set; }
            Vector3 eyePos { get; set; }
        }

        public sealed class CameraBinding : RenderBinding<Camera>
        {
            ICameraMap map;

            protected override void OnEffectChanged(Effect effect)
            {
                base.OnEffectChanged(effect);

                map = effect.Map<ICameraMap>();
            }

            public sealed override void OnBind(Camera obj)
            {
                if (map != null)
                {
                    map.eyePos = obj.Position;                 
                    map.ViewProj = obj.ViewProj;
                }
            }

            public override void OnUnBind(Camera value)
            {

            }
        }
    }
}
