   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{   
    public class CapsuleBuilder : ShapeBuilder<MeshVertex>
    {
        float height;
        float radius;
        int capStacks;
        int capSlices;
        int trunkStacks;
        int trunkSlices;
        private ShapeLayer top;

        public ShapeLayer Top
        {
            get { return top; }
            set { top = value; }
        }
        private ShapeLayer bottom;

        public ShapeLayer Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }
        private ShapeLayer cylindre;

        public ShapeLayer Cylindre
        {
            get { return cylindre; }
            set { cylindre = value; }
        }      

        public CapsuleBuilder(float height, float radius, int capStacks, int capSlices, int trunkStacks, int trunkSlices)
        {
            this.radius = radius;
            this.height = height;
            this.capStacks = capStacks;
            this.capSlices = capSlices;
            this.trunkSlices = trunkSlices;
            this.trunkStacks = trunkStacks;            
            Build();
        }       

        public void Build()
        {
            DomeBuilder domeBuilder = new DomeBuilder(capStacks, capSlices, radius, false);
            CylindreBuilder cylindreBuilder = new CylindreBuilder(trunkStacks, trunkSlices, radius, height, true);

            vertices = new MeshVertex[2 * domeBuilder.Vertices.Length + cylindreBuilder.Vertices.Length];
            indices = new ushort[2 * domeBuilder.Indices.Length + cylindreBuilder.Indices.Length];

            #region Create Top Cap

            for (int i = 0; i < domeBuilder.Vertices.Length; i++)            
            {
                domeBuilder.Vertices[i].Position.Y += height * 0.5f;
            }
            
            Array.Copy(domeBuilder.Vertices, vertices, domeBuilder.Vertices.Length);
            Array.Copy(domeBuilder.Indices, indices, domeBuilder.Indices.Length);
            top = new ShapeLayer
            {
                PrimitiveCount = domeBuilder.Indices.Length / 3,
                StartIndex = 0,
                StartVertex = 0,
                VertexCount = domeBuilder.Vertices.Length
            };
            #endregion

            int vIndex = domeBuilder.Vertices.Length;
            int iIndex = domeBuilder.Indices.Length;

            #region Create Bottom Cap           
            for (int i = 0; i < domeBuilder.Indices.Length / 3; i++)
            {
                indices[iIndex++] = (ushort)(vIndex + domeBuilder.Indices[i * 3 + 2]);
                indices[iIndex++] = (ushort)(vIndex + domeBuilder.Indices[i * 3 + 1]);
                indices[iIndex++] = (ushort)(vIndex + domeBuilder.Indices[i * 3]);
            }

            for (int i = 0; i < domeBuilder.Vertices.Length; i++)
            {
                domeBuilder.Vertices[i].Position.Y *= -1;
                vertices[vIndex++] = domeBuilder.Vertices[i];
            }

            bottom = new ShapeLayer
            {
                PrimitiveCount = domeBuilder.Indices.Length / 3,
                StartIndex = domeBuilder.Indices.Length,
                StartVertex = domeBuilder.Vertices.Length,
                VertexCount = domeBuilder.Vertices.Length
            };

            #endregion

            #region Trunk

            for (int i = 0; i < cylindreBuilder.Indices.Length; i++)
            {
                indices[iIndex++] = (ushort)(vIndex + cylindreBuilder.Indices[i]);
            }

            for (int i = 0; i < cylindreBuilder.Vertices.Length; i++)
            {
                vertices[vIndex++] = cylindreBuilder.Vertices[i];
            }

            cylindre = new ShapeLayer
            {
                PrimitiveCount = cylindreBuilder.Indices.Length / 3,
                StartIndex = domeBuilder.Indices.Length * 2,
                StartVertex = domeBuilder.Vertices.Length * 2,
                VertexCount = cylindreBuilder.Vertices.Length
            };

            #endregion

        }

    }
}
