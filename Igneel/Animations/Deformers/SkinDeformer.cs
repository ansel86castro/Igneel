using Igneel.Assets;
using Igneel.Components;
using Igneel.Design;
using Igneel.Graphics;
using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{    

    [TypeConverter(typeof(DesignTypeConverter))]   
    public class SkinDeformer : Deformer
    {
        Mesh mesh;
        SceneNode[] bones;
        SceneNode boneRoot;           

        /// <summary>
        /// transforms of the bones when the mesh was bind to the skelleton 
        /// </summary>
        private Matrix[] boneBindingMatrices;
        /// <summary>
        /// transform of the mesh when it was bind to the skelleton
        /// boneMatrix = bindShape * boneBindingMatrix * boneCombinedMatrix
        /// </summary>
        private Matrix bindShapeMatrix = Matrix.Identity;
        internal int[][] layerBonesLookup;
        private int maxVertexInfluences;

        public SkinDeformer()
        {

        }

        public SkinDeformer(Mesh mesh)
        {
            this.mesh = mesh;
        }

        public SceneNode BoneRoot
        {
            get { return boneRoot; }
        }

        [AssetMember]
        public int[][] LayerBonesLookup { get { return layerBonesLookup; } set { layerBonesLookup = value; } }

        [AssetMember(storeAs: StoreType.Reference)]
        public Mesh Mesh
        {
            get { return mesh; }
            set
            {
                mesh = value;
            }
        }

        [AssetMember]
        public int MaxVertexInfluences { get { return maxVertexInfluences; } set { maxVertexInfluences = value; } }

        [AssetMember(typeof(SkinBonesConverter))]
        public SceneNode[] Bones
        {
            get { return bones; }
            set
            {
                bones = value;
                if (bones != null)
                {
                    ComputeUpdater();
                    foreach (var bone in bones)
                    {
                        BoneInfo.AddSkin(bone, this);
                    }
                }
            }
        }

        private void ComputeUpdater()
        {
            if (bones.Length == 1)
                boneRoot = bones[0];
            else if(bones.Length > 0)
            {
                boneRoot = BoneInfo.GetRootBone(bones[0]);
            }                     
        }

        [AssetMember]
        public Matrix[] BoneBindingMatrices { get { return boneBindingMatrices; } set { boneBindingMatrices = value; } }

        [AssetMember]
        public Matrix BindShapePose { get { return bindShapeMatrix; } set { bindShapeMatrix = value; } }

        public bool HasBonesPerLayer { get { return layerBonesLookup != null; } }

        public void ComputeBoneBindingMatrices()
        {
            if (bones == null) throw new NullReferenceException("Bones not Set");

            for (int i = 0; i < bones.Length; i++)
            {
                boneBindingMatrices[i] = Matrix.Invert(bones[i].GlobalPose);
            }
        }

        public int[] GetLayerBones(MeshPart layer)
        {
            if (layerBonesLookup != null)
                return layerBonesLookup[layer.layerID];
            return null;
        }

        public int[] GetLayerBones(int iLayer)
        {
            if (layerBonesLookup != null)
                return layerBonesLookup[iLayer];
            return null;
        }

        public void SetBonesMapping(int layerIndex, int[] bonesIDs)
        {
            if (layerBonesLookup == null)
                layerBonesLookup = new int[mesh.Layers.Length][];

            layerBonesLookup[layerIndex] = new int[bonesIDs.Length];

            for (int i = 0; i < bonesIDs.Length; i++)
            {
                layerBonesLookup[layerIndex][i] = bonesIDs[i];
            }
        }

        public IEnumerable<T> GetVertexAttrib<T>(int boneIndex, IASemantic usage, int index)
           where T : struct
        {
            var attrib = mesh.GetVertexBufferView<T>(usage, index);
            var boneIndices = mesh.GetVertexBufferView<float>(IASemantic.BlendIndices, 0);            
            for (int i = 0; i < attrib.Count; i++)
            {                
                for (int j = 0; j < 4; j++)
                {
                    if (boneIndices[i + j] == boneIndex)
                    {
                        yield return attrib[i];
                    }
                }
            }            
        }

        /// <summary>
        /// Need Fix
        /// </summary>
        /// <param name="maxPalleteEntries"></param>
        public void ReSkinMesh(int maxPalleteEntries)
        {
            ReSkinner skinner = new ReSkinner();
            skinner.ReSkin(this, maxPalleteEntries);

        }                   

        unsafe
        struct ReSkinner
        {
            private Mesh mesh;
            private IntPtr ibDataStream;        
            private bool sixteenBits;
            private int primitiveStride;
            private int vertexStride;
            private int indicesOffset;
            private int weightsOffset;
            private byte[] vbNewData;
            private List<int[]> layerBones;
            private int[] boneLookup;
            private bool[] vertices;          
            private List<MeshPart> newLayers;
            private unsafe byte* pIbData;
            private unsafe byte* pVbDAta;
            int layerBonesCount;
            int maxPalleteEntries;
            HashSet<int> tempBonesSet;

            public void ReSkin(SkinDeformer skin, int maxPalleteEntries)
            {
                this.maxPalleteEntries = maxPalleteEntries;
                this.mesh = skin.Mesh;              
                sixteenBits = mesh.Is16BitIndices;
                primitiveStride = 3 * (sixteenBits ? 2 : 4);
                vertexStride = mesh.VertexDescriptor.Size;
                indicesOffset = mesh.VertexDescriptor.OffsetOf(IASemantic.BlendIndices, 0);
                weightsOffset = mesh.VertexDescriptor.OffsetOf(IASemantic.BlendWeight, 0);
                tempBonesSet = new HashSet<int>();

                ibDataStream = mesh.IndexBuffer.Map();
                vbNewData = mesh.VertexBuffer.ToArray<byte>();                
                //vbDataStream.Read(vbNewData, 0, mesh.VertexBuffer.SizeInBytes);                             

                //holds an array of bones indices into the skin bones array, an array of bones for each layer
                layerBones = new List<int[]>();

                //table containing a mapping betwing a skin bone index and a layer bone index
                boneLookup = new int[skin.bones.Length];

                //a set of vertices indices into the vertex buffer
                vertices = new bool[mesh.VertexCount];

                //list of new layers
                newLayers = new List<MeshPart>(mesh.Layers.Length);

                //reset the bones mapping
                for (int i = 0; i < boneLookup.Length; i++)
                    boneLookup[i] = -1;

                //reset the vertex set                      
                Array.Clear(vertices, 0, vertices.Length);

                //current processing layer
                MeshPart newLayer = null;

                GCHandle pinH = GCHandle.Alloc(vbNewData, GCHandleType.Pinned);
                pVbDAta = (byte*)Marshal.UnsafeAddrOfPinnedArrayElement(vbNewData, 0);

                //get  a  pointer to the indexbuffer stream
                pIbData = (byte*)ibDataStream.ToPointer();
                for (int matIdx = 0; matIdx < mesh.MaterialSlots; matIdx++)
                {
                    //for each material get the corresponding layers
                    var layers = mesh.GetLayersByMaterial(matIdx);

                    //create the first layer for matIdx
                    newLayer = new MeshPart();
                    newLayer.materialIndex = matIdx;
                    newLayer.startIndex = layers[0].startIndex;

                    int primitiveCount = 0;                   

                    foreach (var layer in layers)
                    {
                        //loop through add to the layer as much bones as maxPalleteEntries
                        //newLayer.startIndex = Math.Min(newLayer.startIndex, layer.startIndex);
                        for (int itriangle = 0; itriangle < layer.primitiveCount; itriangle++)
                        {
                            primitiveCount++;
                            //check if the triangle can be added to the newLayer
                            if (!AddTriangleBones(layer, itriangle))
                            {
                                //the layer is full, close it and open a new layer
                                newLayer.primitiveCount = primitiveCount - 1;
                                CloseLayer(newLayer);

                                //start a new Layer       
                                newLayer = new MeshPart();
                                newLayer.materialIndex = matIdx;
                                newLayer.startIndex = layer.startIndex + 3 * itriangle;

                                //add the triangle to the newlayer
                                AddTriangleBones(layer, itriangle);
                                primitiveCount = 1;
                            }
                        }                       
                    }

                    newLayer.primitiveCount = primitiveCount;
                    CloseLayer(newLayer);                 
                }
               
                mesh.Layers = newLayers.ToArray();
                for (int i = 0; i < layerBones.Count; i++)
                {
                    skin.SetBonesMapping(i, layerBones[i]);
                }

                //for (int i = 0; i < mesh.VertexCount; i++)
                //{
                //    byte* bones = GetVertexBones(i);
                //    float* weights = GetVertexWeights(i);
                //    for (int j = 0; j < 4; j++)
                //    {
                //        if (bones[j] == 0xFF)
                //        {
                //            bones[j] = 0;
                //            weights[j] = 0;
                //        }
                //    }
                //}

                pinH.Free();

                mesh.VertexBuffer.Unmap();
                mesh.IndexBuffer.Unmap();

                mesh.VertexBuffer.Write(vbNewData);                
            }

            unsafe private bool AddTriangleBones(MeshPart layer, int iTriangle)
            {
                int* indices = stackalloc int[3];

                indices[0] = GetVertexIndex(layer.startIndex + 3 * iTriangle);
                indices[1] = GetVertexIndex(layer.startIndex + 3 * iTriangle + 1);
                indices[2] = GetVertexIndex(layer.startIndex + 3 * iTriangle + 2); //sixteenBits ? (int)((ushort*)pIbData)[layer.startIndex + 3 * iTriangle + 2] : ((int*)pIbData)[layer.startIndex + 3 * iTriangle + 2];              

                if (AddTriangleBones(indices))
                {
                    foreach (var boneIdx in tempBonesSet)
                    {
                        //add the bone to the layer
                          boneLookup[boneIdx] = layerBonesCount++;                          
                    }
                  
                    //assign new bones indices tp trianglesVertices
                    for (int k = 0; k < 3; k++)
                    {                       
                        if (!vertices[indices[k]])
                        {
                            //mark the vertex as processed
                            vertices[indices[k]] = true;

                            float* pvBones = GetVertexBones(indices[k]);
                            float* pvWeights = GetVertexWeights(indices[k]);

                            for (int j = 0; j < 4; j++)
                            {
                                if (pvWeights[j] > 0)
                                    pvBones[j] = boneLookup[(int)pvBones[j]];
                            }   
                        }
                    }

                    return true;
                }

                return false;
            }

            unsafe private bool AddTriangleBones(int* indices)
            {
                /*
                * First of all you need to check if all the triangle's bones can be added
                * if the test pass then add the 
                */
                int boneCount = 0;
                tempBonesSet.Clear();

                //First of all you need to check if all the triangle's bones can be added
                for (int k = 0; k < 3; k++)
                {
                    float* bones = GetVertexBones(indices[k]);
                    float* weights = GetVertexWeights(indices[k]);

                    for (int ibone = 0; ibone < 4; ibone++)
                    {
                        //check all the bones 
                        int boneIdx = (int)bones[ibone];
                        if (weights[ibone] > 0 && boneLookup[boneIdx] < 0 && !tempBonesSet.Contains(boneIdx))
                        {
                            boneCount++;
                            //if the bones is valid and its not in the layer check if there is space for it
                            if (layerBonesCount + boneCount > maxPalleteEntries)
                                return false;

                            //add the bone to the layer                        
                            tempBonesSet.Add(boneIdx);
                        }
                    }
                }
                return true;
            }

            //unsafe private void AssingBones(MeshPart layer)
            //{
            //    for (int i = 0; i < layer.primitiveCount; i++)
            //    {
            //        for (int k = 0; k < 3; k++)
            //        {
            //            byte* pvBones = GetVertexBones(GetVertexIndex(layer.startIndex + 3 * i + k));
            //            float* pvWeights = GetVertexWeights(GetVertexIndex(layer.startIndex + 3 * i + k));

            //            for (int j = 0; j < 4; j++)
            //            {
            //                if (pvBones[j] < 0xFF)
            //                    pvBones[j] = (byte)boneLookup[pvBones[j]];
            //            }   
            //        }                    
            //    }             
            //}

            unsafe float* GetVertexBones(int ivertex)
            {
                return (float*)(pVbDAta + ivertex * vertexStride + indicesOffset);
            }

            unsafe float* GetVertexWeights(int ivertex)
            {
                return (float*)(pVbDAta + ivertex * vertexStride + weightsOffset);
            }

            int GetVertexIndex(int i)
            {
               return sixteenBits ? (int)((ushort*)pIbData)[i] : ((int*)pIbData)[i];
            }

            void CloseLayer(MeshPart layer)
            {
                layer.IndexCount = layer.PrimitiveCount * 3;

                int vertexCount = 0;
                int startVertex = int.MaxValue;                

                for (int i = 0; i < vertices.Length; i++)
                {
                    if (vertices[i])
                    {
                        vertexCount++;
                        startVertex = Math.Min(startVertex, i);
                    }
                    //reset processed vertex
                    vertices[i] = false;
                }

                layer.vertexCount = vertexCount;
                layer.startVertex = startVertex;
                int[] invbones = new int[layerBonesCount];                          
                for (int i = 0; i < boneLookup.Length; i++)
                {
                    if (boneLookup[i] >= 0)
                        invbones[boneLookup[i]] = i;

                    //reset bones
                    boneLookup[i] = -1;
                }

                layerBones.Add(invbones);
                newLayers.Add(layer);
                layerBonesCount = 0;
            }

        }

        class SkinBonesConverter : IStoreConverter
        {
            public object GetStorage(IAssetProvider provider, object propValue, System.Reflection.PropertyInfo pi)
            {
                var skin = (SkinDeformer)provider;
                SceneNode[] bones = skin.Bones;

                AssetReference[] refe = new AssetReference[bones.Length];
                for (int i = 0; i < refe.Length; i++)
                {
                    refe[i] = AssetManager.Instance.GetAssetReference(bones[i]);
                }

                return refe;
            }

            public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
            {
                AssetReference[] refe = (AssetReference[])storeValue;
                var bones = new SceneNode[refe.Length];

                for (int i = 0; i < refe.Length; i++)
                {
                    bones[i] = (SceneNode)AssetManager.Instance.GetAssetProvider(refe[i]);
                }

                var skin = (SkinDeformer)provider;
                skin.Bones = bones;
            }
        }
    }
}
