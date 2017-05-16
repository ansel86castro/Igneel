using Igneel.Graphics;

namespace Igneel.Graphics
{
    public struct Sampler<T>
        where T : Texture
    {

        public static readonly Sampler<T> Null = new Sampler<T>();

        public T Texture;
        public SamplerState State;

        public Sampler(T texture, SamplerState state)
        {
            this.Texture = texture;
            this.State = state;
        }

        public Sampler(T texture)
        {
            this.Texture = texture;
            this.State = texture!=null ? SamplerState.Linear: null;
        }
        
    }
}