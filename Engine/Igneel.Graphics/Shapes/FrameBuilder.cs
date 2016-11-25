using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class FrameBuilder : ComposedShape<MeshVertex>
    {
        public BoxBuilder Box;

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

            this.Box = box;
            Colors = new Vector4[] { new Vector4(1, 0, 0, 1), new Vector4(0, 1, 0, 1), new Vector4(0, 0, 1, 1) };
        }
    }
}
