
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
                Matrix.RotationX(Numerics.PIover2) * Matrix.Translate(0,0, 0.5f * cone.height + cylindre.height),
                Matrix.RotationX(Numerics.PIover2)* Matrix.Translate(0, 0, 0.5f * cylindre.height)
            };
        }
    }

  
}
