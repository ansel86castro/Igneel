using Igneel.Graphics;

namespace Igneel.Graphics
{
    public sealed class SamplerVariableArray<T> : ShaderVariable<SamplerArray<T>>
        where T:Texture
    {
        public int Elements;

        public override unsafe void SetValue()
        {
            if (Value.Textures == null)
            {
                Binder.SetResource(null, Elements);
                Binder.SetSampler(null, Elements);
            }
            else
            {
                Binder.SetResource(ClrRuntime.Runtime.StaticCast<IShaderResource[]>(Value.Textures), Elements);
                Binder.SetSampler(Value.States, Elements);
            }
        }
    }
}