using Igneel.Animations;
using Igneel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{    
    public class SkinBinding : RenderBinding<SkinInstance, SkinBinding.ISkinBinding>, IRenderBinding<MeshPart>
    {
        public interface ISkinBinding
        {
            SArray<Matrix> WorldArray { get; set; }
        }

        private const int maxPaletteMatrices = 32;      
        private const int nbBones = 4;
        Matrix[] boneMatrices = new Matrix[maxPaletteMatrices];

        SkinDeformer skin;
        SceneNode[] bones;
        Matrix bindShapePose;
        Matrix[] boneOffsetMatrices;
        MeshPart lastmeshPart;

        public int MaxVertexInfluences { get { return nbBones; } }      

        public int MaxPaletteMatrices { get { return maxPaletteMatrices; } }

        MeshPart IRenderBinding<MeshPart>.BindedValue
        {
            get { return lastmeshPart; }
        }

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            render.BindWith<MeshPart>(this);
        }
      

        public override void OnBind(SkinInstance value)
        {         
            skin = value.Skin;
            bones = skin.Bones;
            bindShapePose = skin.BindShapePose;
            boneOffsetMatrices = skin.BoneBindingMatrices;                            

            if (bones != null && !skin.HasBonesPerLayer)
            {
                #region Compute Bone Matrices
                int paletteEntry = 0;
                //set all bones
                for (paletteEntry = 0; paletteEntry < bones.Length && paletteEntry < maxPaletteMatrices; paletteEntry++)
                {
                    boneMatrices[paletteEntry] = bindShapePose *
                                                    boneOffsetMatrices[paletteEntry] *
                                                    bones[paletteEntry].GlobalPose;
                }

                if (mapping != null)
                {
                    mapping.WorldArray = new SArray<Matrix>(boneMatrices, paletteEntry);
                    //mapping.WorldArray = boneMatrices;
                }                

                #endregion
            }         
        }

        public void Bind(MeshPart value)
        {
            lastmeshPart = value;
            #region Compute Bone Matrices

            var bonesIDs = skin.GetLayerBones(value);
            if (bonesIDs != null)
            {
                int paletteEntry = 0;
                for (paletteEntry = 0; paletteEntry < bonesIDs.Length; paletteEntry++)
                {
                    int boneIndex = bonesIDs[paletteEntry];

                    boneMatrices[paletteEntry] = skin.BindShapePose * boneOffsetMatrices[boneIndex] * bones[boneIndex].GlobalPose;

                }

                if (mapping != null)
                {
                    mapping.WorldArray = new SArray<Matrix>(boneMatrices, paletteEntry);
                    //mapping.WorldArray = boneMatrices;
                }      
            }
            #endregion
        }

        public void UnBind(MeshPart value)
        {
           
        }

        public override void OnUnBind(SkinInstance value)
        {
           
        }
       
    }
}
