using Igneel.Components;
using Igneel.Graphics;
using Igneel.Rendering.Bindings;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Rendering
{
    public class FrameSkinRender<TEffect> : GraphicRender<FrameSkin, TEffect>, Igneel.Rendering.IFrameSkinRender     
        where TEffect : Effect
    {
        ISkinMap SkinMap;
        private const int maxPaletteMatrices = 32;
        private const int NbBones = 4;
        Matrix[] _boneMatrices = new Matrix[maxPaletteMatrices];
       
        public FrameSkinRender()
            : base() 
        {
            SkinMap = Effect.Map<ISkinMap>();
        }

        public int MaxPaletteMatrices { get { return maxPaletteMatrices; } }

        public int MaxNbBones { get { return NbBones; } }

        public override void Draw(FrameSkin component)
        {           
            var skin = component.Skin;
            var mesh = skin.Mesh;            

            var device = GraphicDeviceFactory.Device;

            device.PrimitiveTopology = IAPrimitive.TriangleList;
            device.SetVertexBuffer(0, mesh.VertexBuffer, 0);
            device.SetIndexBuffer(mesh.IndexBuffer, 0);

            var materials = component.Materials;
       
            var bones = skin.Bones;
            var bindShapePose = skin.BindShapePose;
            var boneOffsetMatrices = skin.BoneBindingMatrices;

            if (bones != null && !skin.HasBonesPerLayer)
            {
                #region Compute Bone Matrices

                int paletteEntry = 0;
                //set all bones
                for (paletteEntry = 0; paletteEntry < bones.Length && paletteEntry < maxPaletteMatrices; paletteEntry++)
                {
                    Matrix globalPose = bones[paletteEntry].GlobalPose;
                    Matrix.Multiply(ref bindShapePose, ref boneOffsetMatrices[paletteEntry], out _boneMatrices[paletteEntry]);
                    Matrix.Multiply(ref _boneMatrices[paletteEntry], ref globalPose, out _boneMatrices[paletteEntry]);

                    //boneMatrices[paletteEntry] = bindShapePose *
                    //                               boneOffsetMatrices[paletteEntry] *
                    //                              bones[paletteEntry].GlobalPose;
                }


                SkinMap.WorldArray = new SArray<Matrix>(_boneMatrices, paletteEntry);
                //mapping.WorldArray = boneMatrices;

                #endregion
            }       


            if (Clipping == PixelClipping.Opaque)
            {
                var transparents = component.TransparentMaterials;
                for (int i = 0, len = transparents.Length; i < len; i++)
                {
                    Bind(materials[transparents[i]]);
                    Bind<IVisualMaterial>(materials[transparents[i]]);
                    RenderLayers(device, skin, bones, boneOffsetMatrices, ref bindShapePose, mesh.GetLayersByMaterial(transparents[i]));
                }

            }
            else
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    Bind(materials[i]);
                    Bind<IVisualMaterial>(materials[i]);
                    RenderLayers(device, skin, bones, boneOffsetMatrices, ref bindShapePose, mesh.GetLayersByMaterial(i));
                }
            }         
        }

        private void RenderLayers(GraphicDevice device, 
            MeshSkin skin,
            Frame[] bones,
            Matrix[] boneOffsetMatrices,
            ref Matrix bindShapePose,
            MeshPart[] layers)
        {
            var effect = this.Effect;
            effect.OnRender(this);

            foreach (var pass in effect.Passes())
            {
                effect.Apply(pass);
                foreach (var layer in layers)
                {
                    #region Compute Bone Matrices

                    var bonesIDs = skin.GetBones(layer);
                    if (bonesIDs != null)
                    {
                        int paletteEntry = 0;
                        for (paletteEntry = 0; paletteEntry < bonesIDs.Length; paletteEntry++)
                        {
                            int boneIndex = bonesIDs[paletteEntry];

                            Matrix globalPose = bones[boneIndex].GlobalPose;

                            Matrix.Multiply(ref bindShapePose, ref boneOffsetMatrices[boneIndex], out _boneMatrices[paletteEntry]);
                            Matrix.Multiply(ref _boneMatrices[paletteEntry], ref globalPose, out _boneMatrices[paletteEntry]);
                            //boneMatrices[paletteEntry] = skin.BindShapePose * boneOffsetMatrices[boneIndex] * bones[boneIndex].GlobalPose;
                        }

                        SkinMap.WorldArray = new SArray<Matrix>(_boneMatrices, paletteEntry);
                        //mapping.WorldArray = boneMatrices;                      
                    }

                    #endregion

                    device.DrawIndexed(layer.primitiveCount * 3, layer.startIndex, 0);
                }
            }
            effect.EndPasses();
        }
              
    }
}
