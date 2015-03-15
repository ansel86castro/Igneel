using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;
using SlimDX;

namespace Igneel.Importers
{
    public class GFrame:Frame
    {
        Matrix combinedTransform;

        public Matrix CombinedTransformMatrix { get { return combinedTransform; } set { combinedTransform = value; } }
    }

    public class GMeshContainer : MeshContainer
    {
        Located<Texture>[] textures;

        public Located<Texture>[] Textures { get { return textures; } set { textures = value; } }

        public Matrix[] BoneOffsetsMatrix { get; set; }     

        public int NumPaletteEntries { get; set; }

        public BoneCombination[] BoneCombinations { get; set; }

        public SlimDX.Direct3D9.Mesh Mesh { get; set; }

        public int MaxVertexInfluences { get; set; }
    }
}
