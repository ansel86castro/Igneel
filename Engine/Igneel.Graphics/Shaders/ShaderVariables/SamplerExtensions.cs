using Igneel.Graphics;

namespace Igneel.Graphics
{
    public static class SamplerExtensions
    {
        public static Sampler<T> ToSampler<T>(this T texture)
            where T:Texture
        {
            return new Sampler<T>(texture);
        }

        public static Sampler<T> ToSampler<T>(this T texture, SamplerState state)
            where T : Texture
        {
            return new Sampler<T>(texture, state);
        }
    }
}