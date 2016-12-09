using Igneel.Components;
using Igneel.Graphics;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Rendering.Bindings
{    
    public class SkinBinding : RenderBinding<FrameSkin, ISkinMap>, IRenderBinding<MeshPart>
    {
       
        private const int maxPaletteMatrices = 32;      
        private const int NbBones = 4;
        Matrix[] _boneMatrices = new Matrix[maxPaletteMatrices];

        MeshSkin _skin;
        Frame[] _bones;
        Matrix _bindShapePose;
        Matrix[] _boneOffsetMatrices;
        MeshPart _lastmeshPart;

        public int MaxVertexInfluences { get { return NbBones; } }      

        public int MaxPaletteMatrices { get { return maxPaletteMatrices; } }

        MeshPart IRenderBinding<MeshPart>.BindedValue
        {
            get { return _lastmeshPart; }
        }

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            render.BindWith<MeshPart>(this);
        }
      

        public override void OnBind(FrameSkin value)
        {         
            _skin = value.Skin;
            _bones = _skin.Bones;
            _bindShapePose = _skin.BindShapePose;
            _boneOffsetMatrices = _skin.BoneBindingMatrices;                            

            if (_bones != null && !_skin.HasBonesPerLayer)
            {
                #region Compute Bone Matrices
                int paletteEntry = 0;
                //set all bones
                for (paletteEntry = 0; paletteEntry < _bones.Length && paletteEntry < maxPaletteMatrices; paletteEntry++)
                {
                    Matrix globalPose = _bones[paletteEntry].GlobalPose;
                    Matrix.Multiply(ref _bindShapePose, ref _boneOffsetMatrices[paletteEntry], out _boneMatrices[paletteEntry]);                    
                    Matrix.Multiply(ref _boneMatrices[paletteEntry], ref globalPose, out _boneMatrices[paletteEntry]);

                    //boneMatrices[paletteEntry] = bindShapePose *
                     //                               boneOffsetMatrices[paletteEntry] *
                      //                              bones[paletteEntry].GlobalPose;
                }

                if (Mapping != null)
                {
                    Mapping.WorldArray = new SArray<Matrix>(_boneMatrices, paletteEntry);
                    //mapping.WorldArray = boneMatrices;
                }                

                #endregion
            }         
        }

        public void Bind(MeshPart value)
        {
            _lastmeshPart = value;
            #region Compute Bone Matrices

            var bonesIDs = _skin.GetBones(value);
            if (bonesIDs != null)
            {
                int paletteEntry = 0;
                for (paletteEntry = 0; paletteEntry < bonesIDs.Length; paletteEntry++)
                {
                    int boneIndex = bonesIDs[paletteEntry];

                    Matrix globalPose = _bones[boneIndex].GlobalPose;

                    Matrix.Multiply(ref _bindShapePose, ref _boneOffsetMatrices[boneIndex], out _boneMatrices[paletteEntry]);
                    Matrix.Multiply(ref _boneMatrices[paletteEntry], ref globalPose, out _boneMatrices[paletteEntry]);
                    //boneMatrices[paletteEntry] = skin.BindShapePose * boneOffsetMatrices[boneIndex] * bones[boneIndex].GlobalPose;
                }

                if (Mapping != null)
                {
                    Mapping.WorldArray = new SArray<Matrix>(_boneMatrices, paletteEntry);
                    //mapping.WorldArray = boneMatrices;
                }      
            }
            #endregion
        }

        public void UnBind(MeshPart value)
        {
           
        }

        public override void OnUnBind(FrameSkin value)
        {
           
        }
       
    }
}
