using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class SphereBuilder : ShapeBuilder<MeshVertex>
    {
        private int stacks;
        private int slices;
        private float radius;
        public SphereBuilder(int stacks, int slices, float radius)
        {
            this.stacks = stacks;
            this.slices = slices;
            this.radius = radius;

            Build();
        }

        private void Build()
        {
            vertices = new MeshVertex[(stacks - 1) * (slices + 1) + 2];
            indices = new ushort[(stacks - 2) * slices * 6 + slices * 6];

            float phiStep = Numerics.PI / stacks;
            float thetaStep = Numerics.TwoPI / slices;
            // do not count the poles as rings
            int numRings = stacks - 1;

            // Compute vertices for each stack ring.
            int k = 0;
            MeshVertex v = new MeshVertex();

            for (int i = 1; i <= numRings; ++i)
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
            // poles: note that there will be texture coordinate distortion
            vertices[Vertices.Length - 2] = new MeshVertex(new Vector3(0.0f, -radius, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), Vector3.Zero, new Vector2(0.0f, 1.0f));
            vertices[Vertices.Length - 1] = new MeshVertex(new Vector3(0.0f, radius, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), Vector3.Zero, new Vector2(0.0f, 0.0f));

            int northPoleIndex = Vertices.Length - 1;
            int southPoleIndex = Vertices.Length - 2;

            int numRingVertices = slices + 1;

            // Compute indices for inner stacks (not connected to poles).
            k = 0;
            for (int i = 0; i < stacks - 2; ++i)
            {
                for (int j = 0; j < slices; ++j)
                {
                    indices[k++] = (ushort)(i * numRingVertices + j);
                    indices[k++] = (ushort)(i * numRingVertices + j + 1);
                    indices[k++] = (ushort)((i + 1) * numRingVertices + j);

                    indices[k++] = (ushort)((i + 1) * numRingVertices + j);
                    indices[k++] = (ushort)(i * numRingVertices + j + 1);
                    indices[k++] = (ushort)((i + 1) * numRingVertices + j + 1);
                }
            }

            // Compute indices for top stack.  The top stack was written 
            // first to the vertex buffer.
            for (int i = 0; i < slices; ++i)
            {
                indices[k++] = (ushort)northPoleIndex;
                indices[k++] = (ushort)(i + 1);
                indices[k++] = (ushort)i;
            }

            // Compute indices for bottom stack.  The bottom stack was written
            // last to the vertex buffer, so we need to offset to the index
            // of first vertex in the last ring.
            int baseIndex = (numRings - 1) * numRingVertices;
            for (int i = 0; i < slices; ++i)
            {
                indices[k++] = (ushort)southPoleIndex;
                indices[k++] = (ushort)(baseIndex + i);
                indices[k++] = (ushort)(baseIndex + i + 1);
            }
        }

    }
}
