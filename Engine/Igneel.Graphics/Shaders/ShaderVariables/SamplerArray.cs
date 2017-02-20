using Igneel.Graphics;

namespace Igneel.Graphics
{
    public struct SamplerArray<T>
        where T : Texture
    {
        public static readonly Sampler<T> Null = new Sampler<T>();

        public T[] Textures;

        public SamplerState[] States;

        public static implicit operator SamplerArray<T>(T[] textures)
        {
            SamplerArray<T> sampler = new SamplerArray<T>();
            sampler.Textures = textures;
            return sampler;
        }
    }
}