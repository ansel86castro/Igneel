using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SArray<T>
        where T : struct
    {
        public T[] Array;
        public int Offset;
        public int Count;

        public SArray(T[] array, int offset, int count)
        {
            this.Array = array;
            this.Offset = offset;
            this.Count = count;
        }

        public SArray(T[] array, int count)
        {
            this.Array = array;
            this.Offset = 0;
            this.Count = count;
        }

        public SArray(T[] array)
        {
            this.Array = array;
            this.Offset = 0;
            this.Count = array.Length;
        }
    }

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
            this.State = SamplerState.Linear;
        }
        
    }

    public struct SamplerArray<T>
        where T : Texture
    {
        public static readonly Sampler<T> Null = new Sampler<T>();

        public T[] Textures;
        public SamplerState[] States;
    }


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
