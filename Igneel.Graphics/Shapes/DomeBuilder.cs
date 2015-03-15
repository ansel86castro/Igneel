
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class DomeBuilder:ShapeBuilder<MeshVertex>
    {
        private int stacks;
        private int slices;
        private float radius;
        private bool closed;

        public DomeBuilder(int stacks, int slices, float radius, bool closed)
        {
            this.stacks = stacks;
            this.slices = slices;
            this.radius = radius;
            this.closed = closed;
            Build();
        }

        private void Build()
        {
            vertices = new MeshVertex[stacks * (slices + 1) + (closed ? 2 : 1)];
            indices = new ushort[(stacks - 1) * slices * 6 + slices * (closed ? 6 : 3)];

            float phiStep = Numerics.PIover2 / stacks;
            float thetaStep = Numerics.TwoPI / slices;           

            // Compute vertices for each stack ring.
            int k = 0;
            MeshVertex v = new MeshVertex();

            for (int i = 1; i <= stacks; ++i)
            {
                float phi = i * phiStep;

                // vertices of ring
                for (int j = 0; j <= slices; ++j)
                {
                    float theta = j * thetaStep;

                    // spherical to cartesian
                    v.Position = Vector3.SphericalToCartesian(phi, theta, radius);                    
                    v.Normal = Vector3.Normalize(v.Position);
                    v.TexCoord = new Vector2(theta / (2.0f * (float)Math.PI), phi / (float)Math.PI);
                    // partial derivative of P with respect to theta
                    v.Tangent = new Vector3(-radius * (float)Math.Sin(phi) * (float)Math.Sin(theta), 0, radius * (float)Math.Sin(phi) * (float)Math.Cos(theta));

                    vertices[k++] = v;
                }
            }

            vertices[Vertices.Length - 1] = new MeshVertex(new Vector3(0.0f, radius, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), Vector3.Zero, new Vector2(0.0f, 0.0f));
            if (closed)            
                vertices[Vertices.Length - 2] = new MeshVertex(new Vector3(0.0f, 0, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), Vector3.Zero, new Vector2(0.0f, 1.0f));

            int numRingVertices = slices + 1;

            // Compute indices for inner stacks (not connected to poles).
            k = 0;
            for (int i = 0; i < stacks - 1; ++i)
            {
                for (int j = 0; j < slices; ++j)
                {
                    indices[k++] = (ushort)((i + 1) * numRingVertices + j); 
                    indices[k++] = (ushort)(i * numRingVertices + j + 1);
                    indices[k++] = (ushort)(i * numRingVertices + j);

                    indices[k++] = (ushort)((i + 1) * numRingVertices + j + 1);
                    indices[k++] = (ushort)(i * numRingVertices + j + 1);
                    indices[k++] = (ushort)((i + 1) * numRingVertices + j);
                }
            }

            int northPoleIndex = Vertices.Length - 1;

            // Compute indices for top stack.  The top stack was written 
            // first to the vertex buffer.
            for (int i = 0; i < slices; ++i)
            {
                indices[k++] = (ushort)i;
                indices[k++] = (ushort)(i + 1);
                indices[k++] = (ushort)northPoleIndex; 
            }

            if (closed)
            {
                int southPoleIndex = Vertices.Length - 2;

                // Compute indices for bottom stack.  The bottom stack was written
                // last to the vertex buffer, so we need to offset to the index
                // of first vertex in the last ring.
                int baseIndex = (stacks - 1) * numRingVertices;
                for (int i = 0; i < slices; ++i)
                {
                    indices[k++] = (ushort)(baseIndex + i + 1);
                    indices[k++] = (ushort)(baseIndex + i);
                    indices[k++] = (ushort)southPoleIndex;
                }
            }

        }
    }
}
