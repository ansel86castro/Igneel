

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class ShapeLayer
    {
        public int StartIndex;
        public int PrimitiveCount;
        public int StartVertex;
        public int VertexCount;
    }

    public class ShapeBuilder<TVertex> where TVertex : struct
    {
        protected IAPrimitive primitiveType = IAPrimitive.TriangleList;       
        protected TVertex[] vertices;
        protected ushort[] indices;
        public Vector4 Color = new Vector4(1, 1, 1, 1);


        public TVertex[] Vertices
        {
            get { return vertices; }
        }       

        public ushort[] Indices
        {
            get { return indices; }
        }

        public IAPrimitive PrimitiveType
        {
            get { return primitiveType; }
        }

        public ShapeBuilder()
        {

        }
    }
  
    public class ComposedShape<TVertex> where TVertex : struct
    {
        public ShapeBuilder<TVertex>[] Shapes;
        public ComposedShape<TVertex>[] Components;
        public Matrix[] Transforms;
        public Vector4[] Colors;
    }
}
