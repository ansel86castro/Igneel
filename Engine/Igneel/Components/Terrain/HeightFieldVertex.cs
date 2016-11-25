using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components.Terrain
{
    public struct HeightFieldVertex
    {
        [VertexElement(IASemantic.Position, usageIndex: 0, stream: 0, offset: 0)]
        public Vector2 Position;

        [VertexElement(IASemantic.TextureCoordinate, usageIndex: 0, stream: 0, offset: 8)]
        public Vector2 TexCoord;

        [VertexElement(IASemantic.Position, usageIndex: 1, stream: 1, offset: 0)]
        public float Height;

        [VertexElement(IASemantic.Normal, usageIndex: 0, stream: 1, offset: 4)]
        public Vector3 Normal;

        [VertexElement(IASemantic.TextureCoordinate, usageIndex: 1, stream: 1, offset: 16)]
        public Vector2 BlendCoord;
    }
}
