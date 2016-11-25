using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components.Particles
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ParticleVertex
    {
        [VertexElement(IASemantic.Position)]
        public Vector3 Position;

        [VertexElement(IASemantic.TextureCoordinate, 0)]
        public Vector2 TexCoord;

        [VertexElement(IASemantic.TextureCoordinate, 1)]
        public Vector3 PositionPtc;

        [VertexElement(IASemantic.TextureCoordinate, 2)]
        public float Alpha;

        [VertexElement(IASemantic.TextureCoordinate, 3)]
        public float Size;

        [VertexElement(IASemantic.Color)]
        public uint Color;

        public ParticleVertex(Vector3 position = default(Vector3), Vector2 texCoord = default(Vector2))
        {
            Position = position;
            TexCoord = texCoord;
            PositionPtc = new Vector3();
            Alpha = 0;
            Color = 0xFFFFFFFF;
            Size = 0;
        }

        public ParticleVertex(float x, float y, float z, float u, float v)
            : this(new Vector3(x, y, z), new Vector2(u, v))
        {

        }
    }
}
