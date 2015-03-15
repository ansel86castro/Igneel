using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering
{
   public class VertexBufferBuilder
    {
        public struct VertexCache
        {
            public int PosIndex;
            public int NormalIndex;
            public int TexCoordIndex;
            public int VertexBufferIndex;
        }

        bool[] taken;
        List<VertexCache>[] vertices;
        MemoryStream vertexBufferData;
        int count;

        public VertexBufferBuilder(int positionsCount, int vertexSize)
        {
            taken = new bool[positionsCount];
            vertices = new List<VertexCache>[positionsCount];
            vertexBufferData = new MemoryStream(positionsCount * vertexSize);
        }

        public int Count { get { return count; } }

        public int GetVertexIndex(int posIdx, int normalIdx, int texCoordIdx)
        {
            if (taken[posIdx])
            {
                var list = vertices[posIdx];
                foreach (var item in list)
                {
                    if (item.PosIndex == posIdx && item.NormalIndex == normalIdx && item.TexCoordIndex == texCoordIdx)
                    {
                        return item.VertexBufferIndex;
                    }
                }
            }
            return -1;
        }

        public int WriteVertex(byte[] vertexData, int posIdx, int normalIdx, int texCoordIdx)
        {
            if (vertices[posIdx] == null)
            {
                vertices[posIdx] = new List<VertexCache>();
                taken[posIdx] = true;
            }

            VertexCache vc = new VertexCache
            {
                PosIndex = posIdx,
                NormalIndex = normalIdx,
                TexCoordIndex = texCoordIdx,
                VertexBufferIndex = count
            };

            vertices[posIdx].Add(vc);
            vertexBufferData.Write(vertexData, 0, vertexData.Length);
            count++;
            return vc.VertexBufferIndex;
        }

        public byte[] GetBuffer()
        {
            return vertexBufferData.GetBuffer();
        }

        public List<VertexCache> GetVertices(int posIdx)
        {
            return vertices[posIdx];
        }
    }
}
