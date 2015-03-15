
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class CircleBuilder : ShapeBuilder<MeshVertex>
    {
        private float radius;
        private int slices;
        public CircleBuilder(int slices, float radius)
        {
            primitiveType = IAPrimitive.LineList;
            this.slices = slices;
            this.radius = radius;

            Generate();
        }

        private void Generate()
        {
            vertices = new MeshVertex[slices + 1];
            float step = Numerics.TwoPI / slices;
            CyCoord coords = new CyCoord(Numerics.PIover2, 0 ,  radius);
            for (int i = 0; i <= slices; i++)
            {
                coords.Theta = i * step;
                vertices[i] = new MeshVertex(position: coords.ToCartesian());
            }

            indices = new ushort[slices * 2];
            for (int i = 0; i < slices; i++)
            {
                indices[2 * i] = (ushort)i;
                indices[2 * i + 1] = (ushort)(i + 1);
            }
        }
    }

    public class RotatorSphereBuilder : ComposedShape<MeshVertex>
    {
        public RotatorSphereBuilder(CircleBuilder circle)
        {
            Shapes = new ShapeBuilder<MeshVertex>[]
            {
                circle, //x
                circle, //y
                circle //z
            };
            Transforms = new Matrix[]
            {
                Matrix.RotationX(-Numerics.PIover2)*Matrix.RotationY(-Numerics.PIover2),
                Matrix.Identity,
                 Matrix.RotationX(-Numerics.PIover2)
            };
             Colors = new Vector4[]{ Color4.Red, Color4.Blue, Color4.Green};
        }
    }
}
