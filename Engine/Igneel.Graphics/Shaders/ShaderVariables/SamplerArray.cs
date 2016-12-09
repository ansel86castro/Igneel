using Igneel.Graphics;

namespace Igneel.Graphics
{
    public struct SamplerArray<T>
        where T : Texture
    {
        public static readonly Sampler<T> Null = new Sampler<T>();

        public T[] Textures;
        public SamplerState[] States;
    }
}