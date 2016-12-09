using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
using System.IO;
using System.Runtime.InteropServices;
using Igneel.Shaders;
using Igneel.Animations;

namespace Igneel.Importers
{
    public class XFileLoader
    {
        public string xFile;

        public SceneImportResult LoadSkelletalMesh(string xFile)
        {
            this.xFile = xFile;
            var allocator = new MeshAllocator();
            AnimationController aController;
            FileStream file = new FileStream(xFile, FileMode.Open, FileAccess.Read);
            GFrame frameRoot = (GFrame)Frame.LoadHierarchyFromX(Engine.Graphics, file, MeshFlags.Managed, allocator, null, out aController);

            var vd = VertexDescriptor.Get<SkinVertex>();           
            List<Tuple<Bone,GFrame>> bones = new List<Tuple<Bone,GFrame>>();

            Bone root = GetBone(frameRoot, bones);
            QuadTreeSceneNode sceneNode = new QuadTreeSceneNode(Path.GetFileNameWithoutExtension(xFile), 10);

            foreach (var item in bones)
            {
                if (item.Item2.MeshContainer != null)
                {
                    var mesh = GetMesh((GMeshContainer)item.Item2.MeshContainer, root, vd);                 
                    sceneNode.Add(new SkelletalMeshNode(item.Item2.Name, mesh));
                }
            }

            sceneNode.UpdateLayout(true);

           try
           {
               file.Close();
               if (aController != null)
                   aController.Dispose();
               allocator.DestroyFrame(frameRoot);
               root.Dispose();
           }
           catch (Exception)
           {

           }

           //AnimationCollection animations = new AnimationCollection();
           //animations.Load(xFile, model.RootBone);

           return new SceneImportResult { VisualSceneRoot = sceneNode, BoneRoots = new List<Bone> { root } };
        }
       
        private Bone GetBone(GFrame frame, List<Tuple<Bone, GFrame>> bones ,Bone parentBone = null)
        {
            Bone bone = new Bone()
            {
                Parent = parentBone,
                Name = frame.Name,
                TransformationMatrix = frame.TransformationMatrix,               
            };            

            bones.Add(new Tuple<Bone, GFrame>(bone, frame));            

            if (frame.FirstChild != null)
            {
                List<Bone> childrens = new List<Bone>();
                GFrame current = (GFrame)frame.FirstChild;
                while (current != null)
                {
                    Bone childBone = GetBone(current, bones, bone);
                    childrens.Add(childBone);
                    current = (GFrame)current.Sibling;
                }
                bone.Childrens = childrens.ToArray();
            }                        
           
            return bone;
        }

        private SkelletalMesh GetMesh(GMeshContainer meshContainer, Bone root, VertexDescriptor vd)
        {
            SlimDX.Direct3D9.Mesh d3dMesh = meshContainer.Mesh;

            var adjacency = meshContainer.GetAdjacency();
            var meshMaterials = meshContainer.GetMaterials();           
            List<MeshMaterial> materials = new List<MeshMaterial>();

            for (int i = 0; i < meshMaterials.Length; i++)
            {
                var matd3d = meshMaterials[i].MaterialD3D;
                
                string textureFilename = meshMaterials[i].TextureFileName;
                if (textureFilename != null && !Path.IsPathRooted(textureFilename))
                {
                    textureFilename = Path.Combine(Path.GetDirectoryName(xFile), textureFilename);
                }            

                materials.Add(new MeshMaterial()
                {
                    Name = root.Name+"_material" + i,
                    Alpha = matd3d.Diffuse.Alpha,
                    Diffuse = matd3d.Diffuse.ToVector3(),
                    Specular = matd3d.Specular.ToVector3(),
                    SpecularPower =Math.Max(1, matd3d.Power),
                    Reflectivity = 0,
                    Refractitity = 0,
                    EmissiveColor = matd3d.Emissive.ToVector3(),
                    DiffuseMap =  textureFilename
                });
            }


            SkelletalMesh mesh = new SkelletalMesh(vd);
            mesh.Adjacency = adjacency;
            SkinVertex[] vertexes = new SkinVertex[d3dMesh.VertexCount];
            Array indices;
            if (d3dMesh.IndexBuffer.Description.Format == Format.Index16)
                indices = new ushort[d3dMesh.FaceCount * 3];
            else
                indices = new uint[d3dMesh.FaceCount * 3];
         
            VertexDescriptor meshVD = new VertexDescriptor(d3dMesh.GetDeclaration());
            int positionOffset = meshVD.GetOffset(DeclarationUsage.Position, 0);
            int normalOffset = meshVD.GetOffset(DeclarationUsage.Normal, 0);
            int texCoordOffset = meshVD.GetOffset(DeclarationUsage.TextureCoordinate, 0);
            int blendIndicesOffset = meshVD.GetOffset(DeclarationUsage.BlendIndices, 0);
            int blendIndicesSize = meshVD.SizeOfElement(DeclarationUsage.BlendIndices, 0);
            int blendWeightsOffset = meshVD.GetOffset(DeclarationUsage.BlendWeight, 0);
            int blendWeightSize = meshVD.SizeOfElement(DeclarationUsage.BlendWeight, 0);

            byte[] buffer;
            unsafe
            {
                DataStream vertexStream = d3dMesh.LockVertexBuffer(0);
                buffer = new byte[vertexStream.Length];
                vertexStream.Read(buffer, 0, (int)vertexStream.Length);
                fixed (byte* _pter = buffer)
                {
                    fixed (SkinVertex* pVertexes = vertexes)
                    {
                        byte* pter = _pter;
                        int j = 0;
                        for (int i = 0; i < d3dMesh.VertexCount; i++)
                        {                          
                            pVertexes[i].Position = *((Vector3*)(pter + positionOffset));                        

                            if (normalOffset >= 0)
                                pVertexes[i].Normal = *((Vector3*)(pter + normalOffset));
                            if (texCoordOffset >= 0)
                                pVertexes[i].TexCoord = *((Vector2*)(pter + texCoordOffset));
                            if (blendIndicesOffset >= 0)
                            {
                                byte* indicesPter = (pter + blendIndicesOffset);
                                uint* pDest = (uint*)&(pVertexes[i].BlendIndices);
                                for (j = 0; j < blendIndicesSize; j++)
                                    *(((byte*)pDest) + j) = *(indicesPter + j);
                            }
                            if (blendWeightsOffset >= 0)
                            {
                                byte* weightsPter = (pter + blendWeightsOffset);
                                Vector4* pDest = &(pVertexes[i].BlendWeights);

                                for (j = 0; j < blendWeightSize; j++)
                                    *(((byte*)pDest) + j) = *(weightsPter + j);
                            }
                          
                            pter += d3dMesh.BytesPerVertex;
                        }
                    }
                }
                d3dMesh.UnlockVertexBuffer();

                DataStream indexStream = d3dMesh.LockIndexBuffer(0);
                GCHandle handler = GCHandle.Alloc(indices, GCHandleType.Pinned);
                IntPtr indexPter = Marshal.UnsafeAddrOfPinnedArrayElement(indices, 0);

                buffer = new byte[indexStream.Length];
                indexStream.Read(buffer, 0, (int)indexStream.Length);                     
                Marshal.Copy(buffer, 0, indexPter, buffer.Length);

                handler.Free();
                d3dMesh.UnlockIndexBuffer();

            }

            mesh.CreateVertexBuffer(vertexes);
            if (d3dMesh.IndexBuffer.Description.Format == Format.Index16)
                mesh.CreateIndexBuffer((ushort[])indices);
            else
                mesh.CreateIndexBuffer((uint[])indices);

            List<MeshLayer> layers = new List<MeshLayer>();            
            if (meshContainer.BoneCombinations == null || meshContainer.BoneCombinations.Length == 0)
            {
                MeshLayer component = new MeshLayer();
                materials.Add(MeshMaterial.CreateDefaultMaterial(meshContainer.Name + "_default"));
                component.materialIndex = 0;                
                component.primitiveCount = mesh.FaceCount;
                component.startIndex = 0;
                component.startVertex = 0;
                component.vertexCount = mesh.VertexCount;
                layers.Add(component);
            }
            else
            {               
                for (int i = 0; i < meshContainer.BoneCombinations.Length; i++)
                {
                    BoneCombination comb = meshContainer.BoneCombinations[i];                    

                    MeshLayer component = new MeshLayer();                    
                    component.materialIndex = comb.AttributeId;                 
                    component.primitiveCount = comb.FaceCount;
                    component.startIndex = comb.FaceStart * 3;
                    component.startVertex = comb.VertexStart;
                    component.vertexCount = comb.VertexCount;

                    layers.Add(component);
                }
            }

            var layerArray = layers.ToArray();
            mesh.Materials = materials.ToArray();
            mesh.SetLayers(layerArray);

            var skinInfo = meshContainer.SkinInfo;
            mesh.MaxVertexInfluences = skinInfo.MaximumVertexInfluences;

            var bones = new Bone[skinInfo.BoneCount];
            var boneOffsetMatrices = new Matrix[bones.Length];
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i] = root.FindChild(skinInfo.GetBoneName(i));
                boneOffsetMatrices[i] = skinInfo.GetBoneOffsetMatrix(i);
            }

            mesh.Bones = bones;
            mesh.BoneBindingMatrices = boneOffsetMatrices;
           
            if (meshContainer.BoneCombinations != null && meshContainer.BoneCombinations.Length > 0)
            {                         
                for (int layerIndex = 0; layerIndex < layerArray.Length; layerIndex++)
                    mesh.SetLayerBones(layerIndex, meshContainer.BoneCombinations[layerIndex].BoneIds);
            }

            mesh.LockVertexBuffer();
            int[] vertices;
            float[] weights;
            for (int i = 0; i < mesh.bones.Length; i++)
            {
                skinInfo.GetBoneInfluence(i, out vertices, out weights);
                VertexStreamView<Vector3> positions = mesh.GetVertexViewStream<Vector3>(DeclarationUsage.Position, 0, vertices);              
            }
            mesh.UnlockVertexBuffer();

            meshVD.Dispose();

            if (normalOffset < 0)
                mesh.ComputeNormals();
            if (texCoordOffset < 0)
                mesh.ComputeTextureCoords(CoordMappingType.Spherical);

            mesh.ComputeTangents();

            return mesh;
        }

        public SceneImportResult LoadStaticMesh(string filename)
        {
            var file = new FileInfo(filename);
            VertexDescriptor vd = VertexDescriptor.Get<ModelVertex>();
            List<MeshLayer> layers = new List<MeshLayer>();
            int[] adjacency;
            ExtendedMaterial[] meshMaterials;
            Mesh mesh = new Mesh(vd);
            List<MeshMaterial> material = new List<MeshMaterial>();
            using (SlimDX.Direct3D9.Mesh d3dMesh = SlimDX.Direct3D9.Mesh.FromFile(Engine.Graphics, filename, MeshFlags.Managed))
            {
                adjacency = d3dMesh.GetAdjacency();
                meshMaterials = d3dMesh.GetMaterials();             

                for (int i = 0; i < meshMaterials.Length; i++)
                {
                    var matd3d = meshMaterials[i].MaterialD3D;
                    string textureFilename = meshMaterials[i].TextureFileName;
                    if (textureFilename != null && !Path.IsPathRooted(textureFilename))
                    {
                        textureFilename = Path.Combine(Path.GetDirectoryName(filename), textureFilename);
                    }
                    material.Add(new MeshMaterial()
                    {
                        Name = file.Name + "_material" + i,
                        Alpha = matd3d.Diffuse.Alpha,
                        Diffuse = matd3d.Diffuse.ToVector3(),
                        Specular = matd3d.Specular.ToVector3(),
                        SpecularPower = Math.Max(1, matd3d.Power),
                        Reflectivity = 0,
                        Refractitity = 0,
                        EmissiveColor = matd3d.Emissive.ToVector3(),
                        DiffuseMap = textureFilename != null && File.Exists(textureFilename) ?
                                        textureFilename : null
                    });
                }

                ModelVertex[] vertexes = new ModelVertex[d3dMesh.VertexCount];
                Array indices;

                if (d3dMesh.IndexBuffer.Description.Format == Format.Index16)
                    indices = new ushort[d3dMesh.FaceCount * 3];
                else
                    indices = new uint[d3dMesh.FaceCount * 3];

                DataStream vertexStream = d3dMesh.LockVertexBuffer(0);
                VertexDescriptor meshVD = new VertexDescriptor(d3dMesh.GetDeclaration());
                int positionOffset = meshVD.GetOffset(DeclarationUsage.Position, 0);
                int normalOffset = meshVD.GetOffset(DeclarationUsage.Normal, 0);
                int texCoordOffset = meshVD.GetOffset(DeclarationUsage.TextureCoordinate, 0);
                byte[] buffer;
                unsafe
                {
                    buffer = new byte[vertexStream.Length];
                    vertexStream.Read(buffer, 0, (int)vertexStream.Length);
                    fixed (byte* _pter = buffer)
                    {
                        byte* pter = _pter;
                        for (int i = 0; i < d3dMesh.VertexCount; i++)
                        {
                            vertexes[i].Position = *((Vector3*)(pter + positionOffset));
                            if (normalOffset > 0)
                                vertexes[i].Normal = *((Vector3*)(pter + normalOffset));
                            if (texCoordOffset > 0)
                                vertexes[i].TexCoord = *((Vector2*)(pter + texCoordOffset));

                            pter += d3dMesh.BytesPerVertex;
                        }
                    }
                    d3dMesh.UnlockVertexBuffer();

                    DataStream indexStream = d3dMesh.LockIndexBuffer(0);
                    GCHandle handler = GCHandle.Alloc(indices, GCHandleType.Pinned);
                    byte* indexPter = (byte*)Marshal.UnsafeAddrOfPinnedArrayElement(indices, 0);

                    buffer = new byte[indexStream.Length];
                    indexStream.Read(buffer, 0, (int)indexStream.Length);
                    for (int i = 0; i < indexStream.Length; i++)
                    {
                        indexPter[i] = buffer[i];
                    }
                    handler.Free();
                    d3dMesh.UnlockIndexBuffer();

                }

                mesh.CreateVertexBuffer(vertexes);
                if (d3dMesh.IndexBuffer.Description.Format == Format.Index16)
                    mesh.CreateIndexBuffer((ushort[])indices);
                else
                    mesh.CreateIndexBuffer((uint[])indices);

                var d3dComponents = d3dMesh.GetAttributeTable();
                if (d3dComponents == null)
                {
                    MeshLayer component = new MeshLayer();
                    layers.Add(component);
                    material.Add(MeshMaterial.CreateDefaultMaterial(file.Name+"_default"));

                    component.materialIndex = 0;
                    component.primitiveCount = mesh.FaceCount;
                    component.startIndex = 0;
                    component.startVertex = 0;
                    component.vertexCount = mesh.VertexCount;
                }
                else for (int i = 0; i < d3dComponents.Length; i++)
                    {
                        AttributeRange ar = d3dComponents[i];

                        MeshLayer component = new MeshLayer();
                        layers.Add(component);

                        component.materialIndex = ar.AttribId;
                        component.primitiveCount = ar.FaceCount;
                        component.startIndex = ar.FaceStart * 3;
                        component.startVertex = ar.VertexStart;
                        component.vertexCount = ar.VertexCount;
                    }

                mesh.Materials = material.ToArray();
                mesh.SetLayers(layers.ToArray());

                if (normalOffset < 0)
                    mesh.ComputeNormals();
                if (texCoordOffset < 0)
                    mesh.ComputeTextureCoords(CoordMappingType.Spherical);

                mesh.ComputeTangents();

                meshVD.Dispose();
            }

            StaticMeshSceneNode meshNode = new StaticMeshSceneNode(file.Name, mesh);
            return new SceneImportResult { VisualSceneRoot = meshNode, VisualMaterials = material };
        }
    }

    public class MeshAllocator : IAllocateHierarchy
    {

        #region IAllocateHierarchy Members

        public Frame CreateFrame(string name)
        {
            return new GFrame() 
            { 
                Name = name ,
                TransformationMatrix = Matrix.Identity,
                CombinedTransformMatrix = Matrix.Identity
            };
        }

        public MeshContainer CreateMeshContainer(string name, MeshData meshData, ExtendedMaterial[] materials, EffectInstance[] effectInstances, int[] adjacency, SkinInfo skinInfo)
        {
            var device = meshData.Mesh.Device;
            GMeshContainer meshContainer = new GMeshContainer();
            meshContainer.Name = name;
            SlimDX.Direct3D9.Mesh mesh = meshData.Mesh;
            if (!mesh.VertexFormat.HasFlag(VertexFormat.Normal))
            {
                meshContainer.MeshData = new MeshData(mesh.Clone(device, mesh.CreationOptions, mesh.VertexFormat | VertexFormat.Normal));
                meshContainer.MeshData.Mesh.ComputeNormals();
            }
            else
            {
                meshContainer.MeshData = new MeshData(mesh);
            }

            meshContainer.SetMaterials(materials);
            meshContainer.SetAdjacency(adjacency);

            meshContainer.SkinInfo = skinInfo;

            meshContainer.BoneOffsetsMatrix = new Matrix[skinInfo.BoneCount];
            for (int i = 0; i < skinInfo.BoneCount;i++ )
            {
                meshContainer.BoneOffsetsMatrix[i] = skinInfo.GetBoneOffsetMatrix(i);
            }

            GenerateSkinnedMesh(device, meshContainer);

            return meshContainer;
        }

        public void DestroyFrame(Frame frame)
        {
            frame.Dispose();
        }

        public void DestroyMeshContainer(MeshContainer container)
        {
            container.Dispose();
        }

        #endregion

        private void GenerateSkinnedMesh(Device device, GMeshContainer container)
        {
            //max number of bone influences per vertex
            int maxVertexInfluence;
            BoneCombination[] combinedBones;

            var skeletalRender =(SkelletalMeshRender)Engine.RenderManager.GetRender<SkelletalMeshNode>();
            if(skeletalRender == null)throw new InvalidOperationException("An skeletalModelRender is not defined");

            int maxPaletteEntries = skeletalRender.MaxPaletteMatrices;

            int numPaletteEntries = Math.Min(maxPaletteEntries, container.SkinInfo.BoneCount);
            container.NumPaletteEntries = numPaletteEntries;
            container.Mesh = container.SkinInfo.ConvertToIndexedBlendedMesh(container.MeshData.Mesh,
                                                            container.NumPaletteEntries,
                                                             container.GetAdjacency(),
                                                             out maxVertexInfluence,
                                                             out combinedBones);
            container.BoneCombinations = combinedBones;
            container.MaxVertexInfluences = maxVertexInfluence;
        }
    }
}
