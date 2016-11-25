using Igneel.Graphics;

namespace Igneel.Graphics
{
    public sealed class SamplerVariable<T> : ShaderVariable<Sampler<T>>
        where T:Texture
    {
        public override sealed unsafe void SetValue()
        {
            Binder.SetResource(Value.Texture);
            Binder.SetSampler(Value.State);
        }
    }
}