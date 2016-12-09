
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    /// <summary>
    /// build a cylindre center at (0,0,0)
    /// </summary>
    public class CylindreBuilder : ShapeBuilder<MeshVertex>
    {
        public int slices;
        public int stacks;
        public float radius;
        public float height;
        public bool open;

        public CylindreBuilder(int stacks, int slices, float radius, float height, bool open)
        {
            this.stacks = stacks;
            this.slices = slices;
            this.radius = radius;
            this.height = height;
            this.open = open;
            Build();
        }

        private void Build()
        {
            vertices = new MeshVertex[(stacks + 1) * (slices + 1) + (open ? 0 : 2 * (slices + 1) + 2)];
            indices = new ushort[stacks * slices * 6 + (open ? 0 : slices * 6)];

            float dTheta = Numerics.TwoPI / (float)slices;
            float dy = height / stacks;
            int vidx = 0;
            int iIdx = 0;
            float halfHeight = height * 0.5f;
            CyCoord coords = new CyCoord(0, halfHeight, radius);
            if (!open)
            {
                CreateCap(dTheta, ref vidx, ref iIdx, new Vector3(0, 1, 0), coords.Y);
                CreateCap(dTheta, ref vidx, ref iIdx, new Vector3(0, -1, 0), -coords.Y);
            }
            ushort offset = (ushort)vidx;

            for (int i = 0; i <= stacks; i++)
            {
                coords.Y = halfHeight - i * dy;
                for (int j = 0; j <= slices; j++)
                {
                    coords.Theta = j * dTheta;
                    vertices[vidx++] = CreateVertex(coords, dTheta);
                }
            }
            int numRingVertices = slices + 1;
            for (int i = 0; i < stacks; i++)
            {
                for (int j = 0; j < slices; j++)
                {
                    indices[iIdx++] = (ushort)(offset + (ushort)(i * numRingVertices + j));
                    indices[iIdx++] = (ushort)(offset + (ushort)(i * numRingVertices + j + 1));
                    indices[iIdx++] = (ushort)(offset + (ushort)((i + 1) * numRingVertices + j));

                    indices[iIdx++] = (ushort)(offset + (ushort)((i + 1) * numRingVertices + j));
                    indices[iIdx++] = (ushort)(offset + (ushort)(i * numRingVertices + j + 1));
                    indices[iIdx++] = (ushort)(offset + (ushort)((i + 1) * numRingVertices + j + 1));
                }
            }
        }

        private void CreateCap(float dTheta, ref int vidx, ref int iIdx, Vector3 normal, float y)
        {
            int start = vidx;
            CyCoord coords = new CyCoord(0, y, radius);
            //pole
            vertices[vidx++] = new MeshVertex(position: new Vector3(0, coords.Y, 0),
                                            normal: normal,
                                            tangent: new Vector3(1, 0, 0),
                                            texCoord: new Vector2(0.5f, 0.5f));
            for (int i = 0; i <= slices; i++)
            {
                coords.Theta = i * dTheta;
                vertices[vidx++] = CreateVertex(coords, dTheta);
            }

            for (int i = 0; i < slices; i++)
            {
                //create triangle
                if (normal.Y == 1)
                {
                    indices[iIdx++] = (ushort)(start + i + 1);
                    indices[iIdx++] = (ushort)(start);
                    indices[iIdx++] = (ushort)(start + i + 2);
                }
                else
                {
                    indices[iIdx++] = (ushort)start;
                    indices[iIdx++] = (ushort)(start + i + 1);
                    indices[iIdx++] = (ushort)(start + i + 2);
                }

            }
        }


        private unsafe MeshVertex CreateVertex(CyCoord coords, float dTheta)
        {
            Vector3* pos = stackalloc Vector3[3];
            CyCoord tcoord = coords;
            for (int i = 0; i < 3; i++)
            {
                //compute 3 positions for computing the derivate respect theta                
                pos[i] = tcoord.ToCartesian();
                tcoord.Theta += dTheta;
            }

            var tangent = Vector3.Normalize(Numerics.DerivateForward1(pos, dTheta));
            var normal = Vector3.Normalize(Vector3.Cross(tangent, new Vector3(0, -1, 0)));
            var texCoord = new Vector2(coords.Theta / Numerics.TwoPI, 0.5f - coords.Y / height);

            return new MeshVertex(pos[0], normal, tangent, texCoord);
        }
    }
}
