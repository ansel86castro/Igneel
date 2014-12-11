
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class ArrowBuilder : ComposedShape<MeshVertex>
    {
        public ArrowBuilder(ConeBuilder cone, CylindreBuilder cylindre)
        {
            Shapes = new ShapeBuilder<MeshVertex>[] { cone, cylindre };
            Transforms = new Matrix[2]
            {
                Matrix.RotationX(Numerics.PIover2) * Matrix.Translate(0,0,cone.height/2.0f + cylindre.height),
                Matrix.RotationX(Numerics.PIover2)* Matrix.Translate(0,0,cylindre.height/2.0f)
            };
        }
    }

    public class FrameBuilder : ComposedShape<MeshVertex>
    {
        public FrameBuilder(ArrowBuilder arrow, BoxBuilder box)
        {
            Components = new ComposedShape<MeshVertex>[]
            {
                arrow, //x
                arrow, //y
                arrow, //z
            };

            Transforms = new Matrix[]
            {
                Matrix.RotationY(Numerics.PIover2),
                Matrix.Identity,
                Matrix.RotationX(-Numerics.PIover2)
            };

            Colors = new Vector4[]{ new Vector4(Color.Red), new Vector4(Color.Blue), new Vector4(Color.Green)};
        }
    }
}
