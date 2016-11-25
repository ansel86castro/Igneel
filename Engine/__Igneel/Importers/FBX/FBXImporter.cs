using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using Igneel.Design;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.IO;
using Igneel.Animations;
using SlimDX.Direct3D9;
using Igneel.Shaders;
using Igneel.Importers;

namespace Igneel.Importers.FBX
{
    [ImportFormat(".fbx")]
    public class FBXImporter : AssetImporter
    {
        public enum ImportType { BasicModel, SkeletalModel, TreeModel }

        FBXDocument doc;
        FBXObject objectDefitions;
        FBXObject globalSettings;
        FBXObject sceneInfo;
        List<int>[] materialPolygons;                
               
        float scalingFactor = 1;
        Dictionary<string, Matrix> poses = new Dictionary<string,Matrix>();

        // new implementation
        List<Bone> rootBones = new List<Bone>();        
        Dictionary<FBXNode, Bone> sceneBones = new Dictionary<FBXNode, Bone>();
        Dictionary<FBXNode, SceneNode> nodes = new Dictionary<FBXNode, SceneNode>();
       List<MeshMaterial> materials = new List<MeshMaterial>();

       protected override Assets.IAssetProvider Import(string filename)
       {
           SceneImportResult result = new SceneImportResult();
           doc = Igneel.Importers.FBX.FBXDocument.Load(filename);
           objectDefitions = (FBXObject)doc.Declarations[FBXDocument.Objects][0];
           materialPolygons = new List<int>[objectDefitions.DeclarationCount("Material")];
           globalSettings = objectDefitions.GetDeclaration<FBXObject>("GlobalSettings");
           sceneInfo = objectDefitions.GetDeclaration<FBXObject>("SceneInfo");

           LoadPosesInfo();

           var scalingFactorProp = globalSettings.GetProperty("UnitScaleFactor");
           if (scalingFactorProp != null)
               scalingFactor = float.Parse(scalingFactorProp.Values[3]);

           LoadMaterials();
           result.VisualMaterials = materials;

           LoadRootBones();
           result.BoneRoots = rootBones;

           result.VisualSceneRoot = CreateSceneNode(doc.BindingManager.SceneGraph);

           result.Animations = LoadAnimations();

           return result;
       }      
            
        private void LoadPosesInfo()
        {
            var posesInfo = objectDefitions.GetDeclaration<FBXObject>("Pose");
            if (posesInfo == null) return;
            foreach (FBXObject pose in posesInfo.Declarations["PoseNode"])
            {
                string nodeId = pose.GetDeclaration<FBXListProperty>("Node").Values[0];
                Matrix matrix = GetMatrix4x4(pose.GetDeclaration<FBXFloatListProperty>("Matrix").FloatValues);
                poses.Add(nodeId, matrix);
            }
        }

        #region Materials Load

        private void LoadMaterials()
        {
            foreach (var fbxObject in objectDefitions.Declarations["Material"])
            {
                var mat = CreateMaterial(doc.BindingManager.Nodes.First(x => x.Target == fbxObject));
                materials.Add(mat);
            }                         
        }

        private MeshMaterial CreateMaterial(FBXNode fbxMaterial)
        {
            var target = fbxMaterial.Target;
            MeshMaterial material = new MeshMaterial(target.Name);
            var diffuse = target.GetProperty("DiffuseColor");
            var specular = target.GetProperty("SpecularColor");
            var emisive = target.GetProperty("EmissiveColor");
            var alpha = target.GetProperty("TransparencyFactor");
            var specularPower = target.GetProperty("ShininessExponent");
            var reflectivity = target.GetProperty("Reflectivity");
            var refractivity = target.GetProperty("TransparencyFactor");

            if (alpha == null)
                alpha = target.GetProperty("3dsMax|extendedParameters|opacity");

            if (diffuse != null)
                material.Diffuse = new Vector3(float.Parse(diffuse.Values[3]), float.Parse(diffuse.Values[4]), float.Parse(diffuse.Values[5]));
            if (specular != null)
                material.Specular = new Vector3(float.Parse(specular.Values[3]), float.Parse(specular.Values[4]), float.Parse(specular.Values[5]));
            if (emisive != null)
                material.EmissiveColor = new Vector3(float.Parse(emisive.Values[3]), float.Parse(emisive.Values[4]), float.Parse(emisive.Values[5]));
            if (alpha != null)
                material.Alpha = 1 - float.Parse(alpha.Values[3]);
            if (specularPower != null)
                material.SpecularPower = float.Parse(specularPower.Values[3]);
            if (reflectivity != null)
                material.Reflectivity = float.Parse(reflectivity.Values[3]);
            if (refractivity != null)
                material.Refractitity = float.Parse(refractivity.Values[3]);

            var textures = fbxMaterial.FindBindings(x => x.Value.Target.Type == "Texture");
            if (textures != null)
            {
                foreach (var binding in textures)
                {
                    var property = binding.Property;
                    if (property.Contains("DiffuseColor") ||
                        property.Contains("diff_color_map") ||
                        property.Contains("diffuse_color") ||
                        property.Contains("diff_color"))
                    {
                        var difTex = CreateTexture(binding.Value);
                        material.DiffuseMap = difTex ?? null;
                    }
                    else if (property.Contains("Bump") ||
                       property.Contains("bump_map"))
                    {
                        material.NormalMap = CreateTexture(binding.Value);
                    }
                    else if (property.Contains("SpecularColor"))
                    {
                        material.SpecularMap = CreateTexture(binding.Value);
                    }
                    else if (property.Contains("ShininessExponent") ||
                             property.Contains("refl_gloss_map"))
                    {
                        material.GlossMap = CreateTexture(binding.Value);
                    }
                }
            }

            return material;
        }

        private Located<SlimDX.Direct3D9.Texture> CreateTexture(FBXNode fBXTextureNode)
        {
            string file = fBXTextureNode.Target.GetDeclaration<FBXListProperty>("FileName").Values[0];

            if (string.IsNullOrWhiteSpace(file))
            {
                var fbxNormalMapTex = fBXTextureNode.Connections.Find(x => x.ConType == ConnectionType.OP && x.Property == "3dsMax|params|normal_map");
                if (fbxNormalMapTex != null)
                    file = fbxNormalMapTex.Value.Target.GetDeclaration<FBXListProperty>("FileName").Values[0];
                else
                {
                    fbxNormalMapTex = fBXTextureNode.Connections.Find(x => x.ConType == ConnectionType.OP && x.Property == "3dsMax|params|bump_map");
                    if (fbxNormalMapTex != null)
                        file = fbxNormalMapTex.Value.Target.GetDeclaration<FBXListProperty>("FileName").Values[0];
                }
                if (string.IsNullOrWhiteSpace(file)) return null;
            }

            if (!File.Exists(file))
                return null;

            return Igneel.Resources.IgneelResources.LoadTexture<Texture> (file);
        }

        #endregion 

        #region Bones Load

        private void LoadRootBones()
        {            
            foreach (var conn in doc.BindingManager.SceneGraph.Connections)
            {
                var target = conn.Value.Target;
                var typeFlags = target.GetDeclaration<FBXListProperty>("TypeFlags");
                if (target.RelatedType == "LimbNode" || 
                    (target.RelatedType == "Root" && (typeFlags!=null && typeFlags.Values.Contains("Skeleton"))))
                {
                    var bone = CreateBoneHeirarchy(conn.Value);
                    rootBones.Add(bone);
                }             
            }
        }

        private Bone CreateBoneHeirarchy(FBXNode fBXNode)
        {
            Bone bone = CreateBone(fBXNode.Target);
            sceneBones.Add(fBXNode, bone);

            List<Bone> childrens = new List<Bone>();

            foreach (var item in fBXNode.Connections)
            {
                var target = item.Value.Target;
                if (target.RelatedType == "LimbNode")
                {
                    childrens.Add(CreateBoneHeirarchy(item.Value));
                }
            }

            bone.Childrens = childrens.ToArray();            
            return bone;
        }

        private Bone CreateBone(FBXObject fbxObject)
        {

            var translationProp = fbxObject.GetProperty("Lcl Translation");
            var rotationProp = fbxObject.GetProperty("Lcl Rotation");
            var scalingProp = fbxObject.GetProperty("Lcl Scaling");
            var pivot = fbxObject.GetProperty("RotationPivot");

            Bone bone = new Bone(fbxObject.Name);

            Vector3 scaling = new Vector3(float.Parse(scalingProp.Values[3]), float.Parse(scalingProp.Values[5]), float.Parse(scalingProp.Values[4]));

            Vector3 rotation = new Vector3(-GMaths.ToRadians(float.Parse(rotationProp.Values[3])),
                                           -GMaths.ToRadians(float.Parse(rotationProp.Values[5])),
                                           -GMaths.ToRadians(float.Parse(rotationProp.Values[4])));

            Vector3 translation = new Vector3(float.Parse(translationProp.Values[3]),
                                              float.Parse(translationProp.Values[5]),
                                              float.Parse(translationProp.Values[4]));

            Matrix rot = Matrix.RotationX(rotation.X) * Matrix.RotationY(rotation.Y) * Matrix.RotationZ(rotation.Z);
            Matrix transformation = Matrix.Scaling(scaling) * rot * Matrix.Translation(translation);

            bone.Pivot = new Vector3(float.Parse(pivot.Values[3]), float.Parse(pivot.Values[5]), float.Parse(pivot.Values[4]));
            bone.Translation = translation;
            bone.Orientation = Euler.FromMatrix(rot);
            bone.Scale = scaling;

            bone.TransformationMatrix = transformation;

            Matrix bindMatrix;
            if (!poses.TryGetValue(fbxObject.Id, out bindMatrix))
                bindMatrix = Matrix.Identity;

            bone.BindingParentMatrix = Matrix.Invert(bindMatrix);

            return bone;
        }

        #endregion

        #region SceneNodes Load

        public SceneNode CreateSceneNode(FBXNode fbxNode)
        {
            SceneNode sceneNode;

            if (nodes.TryGetValue(fbxNode, out sceneNode)) return sceneNode;

            var target = fbxNode.Target;

            if (target.RelatedType == "Root" || target.RelatedType == "Null")            
                sceneNode = new QuadTreeSceneNode(target.Name);                            

            else if (target.RelatedType == "Mesh")
                sceneNode = CreateMeshNode(fbxNode);

            if (sceneNode == null) return null;

            TransformSceneNode(sceneNode, fbxNode.Target);

            nodes.Add(fbxNode, sceneNode);

            foreach (var item in fbxNode.Connections)
            {
                var node = CreateSceneNode(item.Value);
                if (node != null)
                    sceneNode.Add(node, RegisterNodeType.Render);
            }

            return sceneNode;
        }

        private void TransformSceneNode(SceneNode sceneNode, FBXObject fbxObject)
        {
            var translationProp = fbxObject.GetProperty("Lcl Translation");
            var rotationProp = fbxObject.GetProperty("Lcl Rotation");
            var scalingProp = fbxObject.GetProperty("Lcl Scaling");
            var pivot = fbxObject.GetProperty("RotationPivot");

            Vector3 scaling = new Vector3(float.Parse(scalingProp.Values[3]), float.Parse(scalingProp.Values[5]), float.Parse(scalingProp.Values[4]));

            Vector3 rotation = new Vector3(-GMaths.ToRadians(float.Parse(rotationProp.Values[3])),
                                           -GMaths.ToRadians(float.Parse(rotationProp.Values[5])),
                                           -GMaths.ToRadians(float.Parse(rotationProp.Values[4])));

            Vector3 translation = new Vector3(float.Parse(translationProp.Values[3]),
                                              float.Parse(translationProp.Values[5]),
                                              float.Parse(translationProp.Values[4]));

            Matrix rot = Matrix.RotationX(rotation.X) * Matrix.RotationY(rotation.Y) * Matrix.RotationZ(rotation.Z);
            Matrix transformation = Matrix.Scaling(scaling) * rot * Matrix.Translation(translation);
           

            sceneNode.Pivot = new Vector3(float.Parse(pivot.Values[3]), float.Parse(pivot.Values[5]), float.Parse(pivot.Values[4]));
            sceneNode.Translation = translation;
            sceneNode.Orientation = Euler.FromMatrix(rot);
            sceneNode.Scale = scaling;

            Matrix bindMatrix;
            if (!poses.TryGetValue(fbxObject.Id, out bindMatrix))
                bindMatrix = Matrix.Identity;

            sceneNode.BindingParentMatrix = Matrix.Invert(bindMatrix);
            sceneNode.CommitChanges();
        }

        private SceneNode CreateMeshNode(FBXNode fbxNode)
        {
            var skinNode = fbxNode.Connections.Single(x => x.Value.Target.RelatedType == "Skin").Value;
            if (skinNode != null)            
                return CreateSkelletalMeshNode(fbxNode, skinNode);            
            return CreateStaticMeshNode(fbxNode);
        }

        private StaticMeshSceneNode CreateStaticMeshNode(FBXNode fbxNode)
        {
            Dictionary<int, List<int>> vertexMappingTable;
            ModelVertex[] verticesBuffer;
            uint[] indicesBuffer;
            var mesh = CreateMesh<Mesh, ModelVertex>(fbxNode,
                o => new Mesh(VertexDescriptor.Get<ModelVertex>()),
                (pos, normal, tc) => new ModelVertex(position: pos, normal: normal, texCoord: tc),
                (v, pos, tc) => v.Position == pos && v.TexCoord == tc,
                out vertexMappingTable, out verticesBuffer, out indicesBuffer);

            return new StaticMeshSceneNode(fbxNode.Target.Name, mesh);
        }

        private SkelletalMeshNode CreateSkelletalMeshNode(FBXNode fbxNode, FBXNode skinNode)
        {
            Dictionary<int, List<int>> vertexMappingTable;
            SkinVertex[] verticesBuffer;
            uint[] indicesBuffer;

            var mesh = CreateMesh<SkelletalMesh, SkinVertex>(fbxNode,
                o => new SkelletalMesh(VertexDescriptor.Get<ModelVertex>()),
                (pos, normal, tc) => new SkinVertex(position: pos, normal: normal, texCoord: tc),
                (v, pos, tc) => v.Position == pos && v.TexCoord == tc,
                out vertexMappingTable, out verticesBuffer, out indicesBuffer);

            var skeletalRender = (SkelletalMeshRender)Engine.RenderManager.GetRender<SkelletalMeshNode>();
            if (skeletalRender == null)
            {
                skeletalRender = new SkelletalMeshRender();
                Engine.RenderManager.PushRender<SkelletalMeshNode>(skeletalRender);
            }

            var maxPalleteEntries = skeletalRender.MaxPaletteMatrices;

            List<int>[] verticesIndices = new List<int>[vertexMappingTable.Keys.Count];
            List<float>[] verticesWeights = new List<float>[vertexMappingTable.Keys.Count];
            List<Bone> meshBones = new List<Bone>();

            List<List<Vector3>> bonesPositions = new List<List<Vector3>>();

            List<Matrix> offsetMatrices = new List<Matrix>();

            #region Get SkinInfo

            int maxVertexInfluences = int.MinValue;

            foreach (var deformer in from x in skinNode.Connections
                                     where x.Value.Target.RelatedType == "Cluster"
                                     select x.Value)
            {

                var vertices = deformer.Target.GetDeclaration<FBXFloatListProperty>("Indexes").FloatValues;
                var weights = deformer.Target.GetDeclaration<FBXFloatListProperty>("Weights").FloatValues;
                var offsetBoneMatrix = Matrix.Invert(GetMatrix4x4(deformer.Target.GetDeclaration<FBXFloatListProperty>("TransformLink").FloatValues));

                var fbxBone = deformer.Connections[0].Value.Target;
                Bone bone = sceneBones[fbxNode];
                int boneIndex = meshBones.IndexOf(bone);

                if (boneIndex < 0)
                {
                    boneIndex = meshBones.Count;
                    meshBones.Add(bone);
                    offsetMatrices.Add(offsetBoneMatrix);
                    bonesPositions.Add(new List<Vector3>(vertices.Count));
                }

                var list = bonesPositions[boneIndex];
                for (int i = 0; i < vertices.Count; i++)
                {
                    int posIndex = (int)vertices[i];
                    list.Add(verticesBuffer[vertexMappingTable[posIndex][0]].Position);

                    if (verticesIndices[posIndex] == null)
                    {
                        verticesIndices[posIndex] = new List<int>();
                        verticesWeights[posIndex] = new List<float>();
                    }

                    verticesIndices[posIndex].Add(boneIndex);
                    verticesWeights[posIndex].Add(weights[i]);
                    maxVertexInfluences = Math.Max(maxVertexInfluences, verticesIndices[posIndex].Count);
                }
            }

            if (maxVertexInfluences > 4)
                maxVertexInfluences = 4; //throw new InvalidOperationException("BlendIndices out of Range");           

            #endregion

            #region Generate IndexedBlendMesh

            //Set the vertex bone indices and weights respect to the mesh bone index of a bone 
            mesh.MaxVertexInfluences = maxVertexInfluences;

            foreach (var item in vertexMappingTable)
            {
                var list = item.Value;
                var blendIndices = verticesIndices[item.Key];
                var blendWeights = verticesWeights[item.Key];

                for (int i = 0; i < list.Count; i++)
                {
                   // verticesBuffer[list[i]].BlendIndices = GetFloat4(blendIndices);
                    verticesBuffer[list[i]].BlendWeights = GetFloat4(blendWeights);
                }
            }

            #endregion

            mesh.Bones = meshBones.ToArray();
            mesh.BoneBindingMatrices = offsetMatrices.ToArray();

            if (meshBones.Count > maxPalleteEntries)
                SetupBoneLayers(mesh, verticesBuffer, indicesBuffer, maxPalleteEntries);
            else
            {
                mesh.CreateVertexBuffer(verticesBuffer);
            }

            return new SkelletalMeshNode(fbxNode.Target.Name, mesh);
        }

        private bool IsBone(FBXObject target)
        {
            if (target == null)
                return false;
            return target.RelatedType == "LimbNode" || target.RelatedType == "Root";
        }

        int nameIndex;

        private unsafe void SetupBoneLayers(SkelletalMesh mesh, SkinVertex[] vertices, uint[] indices, int maxPalleteEntries)
        {
            List<BoneCluster>[] clusters = new List<BoneCluster>[mesh.Materials.Length];

            fixed (SkinVertex* pVertices = vertices)
            {
                #region Create Clusters

                for (int matIndex = 0; matIndex < mesh.Materials.Length; matIndex++)
                {
                    clusters[matIndex] = new List<BoneCluster>();

                    foreach (var layer in mesh.GetLayersByMaterial(matIndex))
                    {
                        for (int k = 0; k < layer.primitiveCount; k++)
                        {
                            int polygonIndex = layer.startIndex / 3 + k;
                            bool added = false;

                            for (int j = 0; j < clusters[matIndex].Count; j++)
                                if (clusters[matIndex][j].AddPolygon(pVertices, indices, polygonIndex))
                                {
                                    added = true;
                                    break;
                                }

                            if (!added)
                            {
                                BoneCluster c = new BoneCluster(maxPalleteEntries);
                                c.AddPolygon(pVertices, indices, polygonIndex);
                                clusters[matIndex].Add(c);
                            }
                        }
                    }
                }

                #endregion

                #region CreateLayers

                List<MeshLayer> newLayers = new List<MeshLayer>(mesh.Materials.Length);
                List<int[]> layerBonesIds = new List<int[]>(mesh.Materials.Length);

                SkinVertex[] vbuffer = new SkinVertex[vertices.Length];
                uint[] newIndices = new uint[indices.Length];
                int iIndex = 0;
                int vIndex = 0;

                for (int matIndex = 0; matIndex < clusters.Length; matIndex++)
                {
                    foreach (var cluster in clusters[matIndex])
                    {
                        var polygons = cluster.Polygons;
                        MeshLayer layer = new MeshLayer();

                        layer.materialIndex = matIndex;
                        layer.startVertex = vIndex;
                        layer.startIndex = iIndex;
                        layer.primitiveCount = polygons.Count;

                        #region Polygons

                        for (int i = 0; i < polygons.Count; i++)
                        {
                            int starPolygonVertex = 3 * polygons[i];
                            for (int j = 0; j < 3; j++)
                            {
                                int oldIndex = (int)indices[starPolygonVertex + j];
                                SkinVertex v = pVertices[oldIndex];
                                int index = Array.IndexOf<SkinVertex>(vbuffer, v, layer.startVertex);
                                if (index < 0)
                                {
                                    index = vIndex;
                                    vbuffer[vIndex++] = v;
                                    layer.vertexCount++;
                                }
                                newIndices[iIndex++] = (uint)index;
                            }
                        }

                        #endregion

                        newLayers.Add(layer);
                        layerBonesIds.Add(cluster.BonesIDs);
                        ReIndex(vbuffer, layer, cluster);
                    }
                }

                mesh.SetLayers(newLayers.ToArray());

                for (int i = 0; i < mesh.Layers.Length; i++)
                    mesh.SetLayerBones(i, layerBonesIds[i]);

                mesh.CreateVertexBuffer(vbuffer);

                if (mesh.Is16BitIndices)
                    mesh.CreateIndexBuffer(newIndices.Select(x => (ushort)x).ToArray());
                else
                {
                    mesh.CreateIndexBuffer(newIndices);
                }

                #endregion
            }
        }

        private unsafe void ReIndex(SkinVertex[] vbuffer, MeshLayer layer, BoneCluster cluster)
        {
            //int startVertex = layer.startVertex;
            //int[] bonesIds = cluster.BonesIDs;
            //for (int i = 0; i < layer.vertexCount; i++)
            //{
            //    SkinVertex v = vbuffer[startVertex + i];

            //    Vector4 indicesTemp = v.BlendIndices;
            //    Vector4 weightsTemp = v.BlendWeights;

            //    byte* sourceBlendIndices = (byte*)&indicesTemp;
            //    float* sourceBlendWeights = (float*)&weightsTemp;
            //    byte* destBlendIndices = (byte*)&v.BlendIndices;
            //    float* destBlendWeights = (float*)&v.BlendWeights;

            //    int k = 0;
            //    for (int j = 0; j < 4; j++)
            //    {
            //        if (sourceBlendIndices[j] != 0xFF)
            //        {
            //            int index = Array.IndexOf<int>(bonesIds, sourceBlendIndices[j]);
            //            destBlendIndices[k] = (byte)index;
            //            destBlendWeights[k] = sourceBlendWeights[j];
            //            k++;
            //        }
            //    }

            //    vbuffer[startVertex + i] = v;
            //}
        }

        #endregion

        #region Meshes Load

        private TMesh CreateMesh<TMesh, TVertex>(FBXNode fbxNode,
                                              Func<FBXObject, TMesh> meshActivator,
                                              Func<Vector3, Vector3, Vector2, TVertex> vertexFunc,
                                              Func<TVertex, Vector3, Vector2, bool> vertexComparer,
                                              out Dictionary<int, List<int>> vertexMappingTable,
                                              out TVertex[] verticesBuffer,
                                              out uint[] indicesBuffer)
          where TVertex : struct
          where TMesh :Mesh
        {

            var meshDataObject = fbxNode.Target;
            var vd = VertexDescriptor.Get<TVertex>();
            TMesh mesh = meshActivator(meshDataObject);
            mesh.Materials = (from m in fbxNode.Connections
                                            where m.Value.Target.Type == "Material"
                                            select this.materials.First(x => x.Name == m.Value.Target.Name)).ToArray();

            Matrix transform = Matrix.Identity;

            try
            {
                var gtranslation = meshDataObject.GetProperty("GeometricTranslation").Values;
                var grotation = meshDataObject.GetProperty("GeometricRotation").Values;
                var gscaling = meshDataObject.GetProperty("GeometricScaling").Values;
                transform *= Matrix.RotationX(GMaths.ToRadians(float.Parse(grotation[3]))) *
                             Matrix.RotationY(GMaths.ToRadians(float.Parse(grotation[4]))) *
                             Matrix.RotationZ(GMaths.ToRadians(float.Parse(grotation[5])));

                transform *= Matrix.Scaling(float.Parse(gscaling[3]), float.Parse(gscaling[4]), float.Parse(gscaling[5]));
                transform *= Matrix.Translation(float.Parse(gtranslation[3]), float.Parse(gtranslation[4]), float.Parse(gtranslation[5]));
            }
            catch (NullReferenceException)
            {
                transform = Matrix.Identity;
            }


            #region Materials         

            string matMappingInfoType = null;
            string matReferenceInfoType = null;
            bool allSame = false;
            var materialObject = meshDataObject.GetDeclaration<FBXObject>("LayerElementMaterial");
            int materialPolygonsLength = 0;

            if (materialObject != null)
            {
                materialPolygonsLength = materialPolygons.Length;

                for (int i = 0; i < materialPolygonsLength; i++)
                    if (materialPolygons[i] != null) materialPolygons[i].Clear();

                List<float> materialsList = materialObject.GetDeclaration<FBXFloatListProperty>("Materials").FloatValues;
                matMappingInfoType = materialObject.GetDeclaration<FBXListProperty>("MappingInformationType").Values[0];
                matReferenceInfoType = materialObject.GetDeclaration<FBXListProperty>("ReferenceInformationType").Values[0];

                if (matMappingInfoType == "ByPolygon")
                {
                    for (int i = 0; i < materialsList.Count; i++)
                    {
                        int matIndex = (int)materialsList[i];
                        if (materialPolygons[matIndex] == null)
                            materialPolygons[matIndex] = new List<int>();
                        materialPolygons[matIndex].Add(i);
                    }
                }
                else if (matMappingInfoType == "AllSame")
                {
                    allSame = true;
                    materialPolygonsLength = 1;
                }
            }
            else
            {
                allSame = true;
                materialPolygonsLength = 1;
            }

            #endregion

            #region Pick up Data

            bool computeNormal = true;
            bool computeTexCoords = true;
            string normalMappingInfoType = null;
            string normalReferenceInfoType = null;
            string texMappingInfoType = null;
            string texReferenceInfoType = null;
            int[] texCoordIndex = null;
            Vector3[] positions = null;
            Vector3[] normals = null;
            Vector2[] texCoords = null;

            var positionList = meshDataObject.GetDeclaration<FBXFloatListProperty>(FBXDocument.Vertices).FloatValues;
            positions = GetVectors3(positionList);
            var polygonVertexIndex = meshDataObject.GetDeclaration<FBXFloatListProperty>(FBXDocument.PolygonVertexIndex).FloatValues;
            int maxPrimitiveSize;
            var polygonStarts = GetPrimitivesStarts(polygonVertexIndex, out maxPrimitiveSize);

            var normalData = meshDataObject.GetDeclaration<FBXObject>("LayerElementNormal");
            if (normalData != null)
            {
                computeNormal = false;
                normalMappingInfoType = normalData.GetDeclaration<FBXListProperty>("MappingInformationType").Values[0];
                normalReferenceInfoType = normalData.GetDeclaration<FBXListProperty>("ReferenceInformationType").Values[0];
                normals = GetVectors3(normalData.GetDeclaration<FBXFloatListProperty>("Normals").FloatValues);
            }

            var texCoordData = meshDataObject.GetDeclaration<FBXObject>("LayerElementUV");
            if (texCoordData != null)
            {
                computeTexCoords = false;
                texMappingInfoType = texCoordData.GetDeclaration<FBXListProperty>("MappingInformationType").Values[0];
                texReferenceInfoType = texCoordData.GetDeclaration<FBXListProperty>("ReferenceInformationType").Values[0];
                texCoords = GetTexCoords(texCoordData.GetDeclaration<FBXFloatListProperty>("UV").FloatValues);
                if (texReferenceInfoType == "IndexToDirect")
                    texCoordIndex = GetIndices(texCoordData.GetDeclaration<FBXFloatListProperty>("UVIndex").FloatValues);
            }

            #endregion

            vertexMappingTable = new Dictionary<int, List<int>>(positions.Length);
            bool[] taked = null;
            List<MeshLayer> layers = new List<MeshLayer>();
            bool byPolygonVertex = normalMappingInfoType == "ByPolygonVertex" && normalReferenceInfoType == "Direct";
            bool byVertice = normalMappingInfoType == "ByVertice" && normalReferenceInfoType == "Direct";

            List<uint> indices = new List<uint>(polygonVertexIndex.Count);
            uint[] tempIndices = new uint[maxPrimitiveSize];
            List<TVertex> buffer = new List<TVertex>(polygonVertexIndex.Count);

            taked = new bool[positions.Length];

            for (int matIndex = 0; matIndex < materialPolygonsLength; matIndex++)
            {
                List<int> polygons = !allSame ? polygons = materialPolygons[matIndex] : null;
                if (polygons == null && !allSame)
                    continue;

                int polyCount = allSame ? polygonStarts.Length : polygons.Count;

                int startVertex = buffer.Count;
                int startIndex = indices.Count;
                int vertexCount = 0;
                int primitiveCount = 0;

                #region LayerPolygons
                for (int poly = 0; poly < polyCount; poly++)
                {
                    int startPolyIndex = allSame ? polygonStarts[poly] : polygonStarts[polygons[poly]];

                    #region polygonsVertex
                    int k = 0;
                    for (k = 0; true; k++)
                    {
                        //PolygonVertex
                        bool endPrimitive = false;
                        int vIndex = startPolyIndex + k;
                        int posIndex = (int)polygonVertexIndex[vIndex];

                        if (posIndex < 0)
                        {
                            endPrimitive = true;
                            posIndex = -posIndex - 1;
                        }

                        var pos = Vector3.TransformCoordinate(positions[posIndex], transform);
                        var normal = normals != null ? Vector3.Normalize(Vector3.TransformNormal(normals[byPolygonVertex ? vIndex : posIndex], transform))
                                                           : new Vector3();
                        var texCoord = texCoords != null ? (texCoords[texCoordIndex != null ? texCoordIndex[vIndex] : posIndex])
                                                                  : new Vector2();
                        if (!taked[posIndex])
                        {                                                       
                            tempIndices[k] = ((uint)buffer.Count);
                            vertexMappingTable.Add(posIndex, new List<int> { buffer.Count });
                            buffer.Add(vertexFunc(pos, normal, texCoord));
                            vertexCount++;
                            taked[posIndex] = true;
                        }
                        else
                        {
                            List<int> vertexList = vertexMappingTable[posIndex];
                            int index = FindIndex(buffer, startVertex, ref normal, ref texCoord, vertexList, vertexComparer);
                            if (index < 0)
                            {
                                pos = Vector3.TransformCoordinate(positions[posIndex], transform);
                                normal = normals != null ? Vector3.Normalize(Vector3.TransformNormal(normals[byPolygonVertex ? vIndex : posIndex], transform))
                                                                   : new Vector3();

                                tempIndices[k] = ((uint)buffer.Count);
                                vertexList.Add(buffer.Count);
                                buffer.Add(vertexFunc(pos, normal, texCoord));
                                vertexCount++;
                            }
                            else
                            {
                                tempIndices[k] = ((uint)index);
                            }
                        }

                        if (endPrimitive)
                            break;
                    }

                    //expand primitive if necesary
                    for (int i = 0; i < k - 1; i++)
                    {
                        //indices.Add(tempIndices[i]);
                        //indices.Add(tempIndices[i + 1]);
                        //indices.Add(tempIndices[k]);
                        indices.Add(tempIndices[k]);
                        indices.Add(tempIndices[i + 1]);
                        indices.Add(tempIndices[i]);

                        primitiveCount++;
                    }

                    #endregion
                }

                #endregion

                MeshLayer layer = new MeshLayer(startIndex, primitiveCount, startVertex, vertexCount);
                layer.MaterialIndex = matIndex;
                layers.Add(layer);
            }

            verticesBuffer = buffer.ToArray();
            indicesBuffer = indices.ToArray();

            //GCHandle handle = GCHandle.Alloc(verticesBuffer, GCHandleType.Pinned);
            //IntPtr addr = Marshal.UnsafeAddrOfPinnedArrayElement(verticesBuffer, 0);

            //VertexStreamView<Vector3> positionsReader = new VertexStreamView<Vector3>(addr, vd, SlimDX.Direct3D9.DeclarationUsage.Position, 0, verticesBuffer.Length);
            //VertexStreamView<Vector3> normalsReader = new VertexStreamView<Vector3>(addr, vd, SlimDX.Direct3D9.DeclarationUsage.Normal, 0, verticesBuffer.Length);
        
            //var translation = meshDataObject.GetProperty("Lcl Translation").Values;
            //var rotation = meshDataObject.GetProperty("Lcl Rotation").Values;
            //var scaling = meshDataObject.GetProperty("Lcl Scaling").Values;

            //transform *= Matrix.RotationX(GMaths.ToRadians(float.Parse(rotation[3]))) *
            //             Matrix.RotationY(GMaths.ToRadians(float.Parse(rotation[4]))) *
            //             Matrix.RotationZ(GMaths.ToRadians(float.Parse(rotation[5])));

            //transform *= Matrix.Scaling(float.Parse(scaling[3]), float.Parse(scaling[4]), float.Parse(scaling[5]));
            //transform *= Matrix.Translation(float.Parse(translation[3]), float.Parse(translation[4]), float.Parse(translation[5]));


            //for (int i = 0; i < verticesBuffer.Length; i++)
            //{
            //    Vector3 pos = Vector3.TransformCoordinate(positionsReader[i], transform);
            //    Vector3 nor = Vector3.Normalize(Vector3.TransformNormal(normalsReader[i], transform));

            //    positionsReader[i] = new Vector3(pos.X, pos.Z, pos.Y);
            //    normalsReader[i] = new Vector3(nor.X, nor.Z, nor.Y);
            //}           

            //for (int i = 0; i < indicesBuffer.Length - 2; i += 3)
            //{
            //    uint i1 = indicesBuffer[i];
            //    uint i2 = indicesBuffer[i + 1];
            //    uint i3 = indicesBuffer[i + 2];

            //    indicesBuffer[i] = i3;
            //    indicesBuffer[i + 1] = i2;
            //    indicesBuffer[i + 2] = i1;
            //}

            mesh.CreateVertexBuffer(verticesBuffer);

            if (buffer.Count < ushort.MaxValue)
                mesh.CreateIndexBuffer(indicesBuffer.Select(x => (ushort)x).ToArray());
            else
                mesh.CreateIndexBuffer(indicesBuffer);
            
            mesh.SetLayers(layers.ToArray());

            if (computeNormal)
                mesh.ComputeNormals();
            if (computeTexCoords)
                mesh.ComputeTextureCoords(CoordMappingType.Spherical);

            mesh.ComputeTangents();

            //mesh.Translation = new Vector3(float.Parse(translation[3]), float.Parse(translation[5]), float.Parse(translation[4]));
            //mesh.Orientation = orientation;
            //mesh.Scale = new Vector3(float.Parse(scaling[3]), float.Parse(scaling[5]), float.Parse(scaling[4]));
            return mesh;

        }
     
        #endregion

        #region Animations

        private List<AnimationCollection> LoadAnimations()
        {
            throw new NotImplementedException();
        }

        #endregion         

        #region Miscelaneas

        private unsafe Int4 GetInt4(List<int> blendIndices)
        {
            Int4 v = (Int4)(-1);
            int* pter = (int*)&v;

            for (int i = 0; i < blendIndices.Count && i < 4; i++)
                pter[i] = blendIndices[i];

            return v;
        }

        private unsafe Vector4 GetFloat4(List<float> blendWeights)
        {
            Vector4 v = new Vector4();
            if (blendWeights == null)
                return v;

            float* pter = (float*)&v;

            for (int i = 0; i < blendWeights.Count && i < 4; i++)           
                pter[i] = blendWeights[i];

            return v;
        }

        private unsafe uint GetInt(List<int> blendIndices)
        {
            uint v = 0xFFFFFFFF;
            if (blendIndices == null)
                return v;
   
            byte* pter = (byte*)&v;

            for (int i = 0; i < blendIndices.Count && i < 4; i++)
                pter[i] = (byte)blendIndices[i];

            return v;
        }

        private int FindIndex<TVertex>(List<TVertex> vertexes, int startVertex, ref Vector3 normal, ref Vector2 texCoord, List<int> vertexList ,Func<TVertex, Vector3, Vector2,bool> vertexComparer)
            where TVertex : struct
        {
            for (int j = 0; j < vertexList.Count; j++)
            {
                int i = vertexList[j];
                if (i >= startVertex)
                {
                    TVertex v = vertexes[i];
                    if (vertexComparer(v, normal, texCoord))
                    {
                        return vertexList[j];
                    }
                }
            }
            return -1;
        }
        
        private Vector3[] GetVectors3(List<float> values)
        {
            int length = values.Count / 3;
            Vector3[] positions = new Vector3[length];           
            for (int i = 0; i < length; i++)
            {
                int index = 3 * i;
                positions[i] = new Vector3(values[index], values[index + 1], values[index + 2]);
            }
            return positions;
        }

        private Vector2[] GetTexCoords(List<float> values)
        {          
            int length = values.Count / 2;

            Vector2[] positions = new Vector2[length];            

            for (int i = 0; i < length; i++)
            {
                int index = 2 * i;
                positions[i] = new Vector2(values[index], 1 - values[index + 1]);
            }
            return positions;
        }      

        private int[] GetIndices(List<float> values)
        {
            int[] indices = new int[values.Count];          
            for (int i = 0; i < values.Count; i++)            
                indices[i] = (int)values[i];            
            return indices;
        }

        private int[] GetPrimitives(List<float> values ,out int primitiveSize)
        {
            int[] indices = new int[values.Count];            
            for (primitiveSize = 0; primitiveSize < values.Count && values[primitiveSize] >= 0; primitiveSize++) ;
            primitiveSize++;

            for (int i = 0; i < values.Count; i++)
            {
                int index = (int)values[i];             
                if (index < 0)
                    index = -index - 1;
                indices[i] = index;
            }
            return indices;
        }

        private int[] GetPrimitivesStarts(List<float> values ,out int maxPrimitiveSize)
        {
            List<int> starts = new List<int>();
            maxPrimitiveSize = int.MinValue;
            int positiveCount = -1;

            for (int i = 0; i < values.Count; i++)
            {
                int index = (int)values[i];
                positiveCount++;

                if (index < 0)
                {                    
                    maxPrimitiveSize = Math.Max(maxPrimitiveSize, positiveCount + 1);
                    starts.Add(i - positiveCount);
                    positiveCount = -1;
                }                
            }

            return starts.ToArray();
        }

        private Matrix GetMatrix4x4(List<float> values)
        {
            Matrix m = new Matrix();
            for (int i = 0; i < 4; i++)               
                    m.set_Rows(i, new Vector4(values[4 * i], values[4 * i + 2], values[4 * i + 1], values[4 * i + 3]));

            Vector4 row1 = m.get_Rows(1);
            Vector4 row2 = m.get_Rows(2);
            m.set_Rows(1, row2);
            m.set_Rows(2, row1);
            return  m;
        }

        #endregion

        class BoneCluster
        {
            int index;
            int end;
            public int[] BonesIDs;           
            public List<int> Polygons;

            public BoneCluster(int maxPalleteEntries)
            {
                Polygons = new List<int>();
                BonesIDs = new int[maxPalleteEntries];
                index = 0;
                end = BonesIDs.Length;

                for (int i = 0; i < maxPalleteEntries; i++)
                    BonesIDs[i] = -1;

            }

            private unsafe bool AddBoneIndices(int * boneIndices)
            {
                int bonesContained = 0;              
                byte* blend= (byte *)boneIndices;
                
                for (int i = 0; i < 4; i++)
                {
                    if (blend[i] != 0xFF)
                    {
                        int k = Array.IndexOf(BonesIDs, blend[i]);
                        if (k < 0) //bone is not in bonesIds 
                        {
                            //check if it can be added  
                            if (index < end)
                                BonesIDs[index++] = blend[i];                            
                            else                                                           
                                return false;                            
                        }
                        bonesContained++;                        
                    }
                }

                return bonesContained > 0;

            }

            public unsafe bool AddPolygon(SkinVertex* vertices, uint[] indices ,int polygonIndex)
            {
                int starIndex = polygonIndex * 3;
                int savedIndex = index;

                if (AddBoneIndices((int*)&vertices[indices[starIndex]].BlendIndices) &&
                    AddBoneIndices((int*)&vertices[indices[starIndex + 1]].BlendIndices) &&
                    AddBoneIndices((int*)&vertices[indices[starIndex + 2]].BlendIndices))
                {
                    Polygons.Add(polygonIndex);
                    return true;
                }
                else
                {
                    //rollback
                    // if bone can not be added roll back
                    for (int j = savedIndex; j < index; j++)
                        BonesIDs[j] = -1;

                    return false;
                }

            }
        }

        //private void LoadAnimation(Model3D model)
        //{
        //    AnimationCollection animations = new AnimationCollection();

        //    var takes = doc.GetDeclaration<FBXObject>("Takes");
        //    if (takes.Declarations.ContainsKey("Take"))
        //    {
        //        foreach (FBXObject take in takes.Declarations["Take"])
        //        {
        //            Igneel.Animations.AnimationSet aniSet = new Igneel.Animations.AnimationSet();
        //            aniSet.Name = take.Name;

        //            foreach (FBXObject mRef in take.Declarations["Model"])
        //            {
        //                FBXObject node = objectDefitions.GetObjectById(mRef.Id, mRef.Type);
        //                if (IsBone(node))
        //                {
        //                    var transform = mRef.GetObjectById("Transform", "Channel");

        //                    Animation animation = new Animation();
        //                    animation.BoneName = node.Name;
        //                    animation.Bone = model.RootBone.FindChild(node.Name);

        //                    Dictionary<int, Vector3> translationKeys = LoadAnimationKeys(transform, "T");
        //                    Dictionary<int, Vector3> rotationKeys = LoadAnimationKeys(transform, "R");
        //                    Dictionary<int, Vector3> scalingKeys = LoadAnimationKeys(transform, "S");

        //                    foreach (var item in translationKeys)
        //                        animation.Translations.Add(new AnimationKey<Vector3> { Time = item.Key, Value = item.Value });
        //                    foreach (var item in rotationKeys)
        //                    {
        //                        Matrix rotationMatrix = Matrix.RotationX(-GMaths.ToRadians(item.Value.X)) *
        //                                                Matrix.RotationY(-GMaths.ToRadians(item.Value.Z)) *
        //                                                Matrix.RotationZ(-GMaths.ToRadians(item.Value.Y));
        //                        Quaternion qt = Quaternion.RotationMatrix(rotationMatrix);
        //                        animation.Rotations.Add(new AnimationKey<Quaternion> { Time = item.Key, Value = qt });
        //                    }
        //                    foreach (var item in scalingKeys)
        //                        animation.Scalings.Add(new AnimationKey<Vector3> { Time = item.Key, Value = item.Value });

        //                    aniSet.Add(animation);
        //                }
        //            }

        //            animations.Add(aniSet);
        //        }

        //        model.Animations = animations;
        //    }
        //}

        //private Dictionary<int, Vector3> LoadAnimationKeys(FBXObject transform, string transformType)
        //{
        //    Dictionary<int, Vector3> lookup = new Dictionary<int, Vector3>();

        //    var channel = transform.GetObjectById(transformType, "Channel");
        //    var x = channel.GetObjectById("X", "Channel");
        //    var y = channel.GetObjectById("Y", "Channel");
        //    var z = channel.GetObjectById("Z", "Channel");

        //    //(time ,value ,u, a, n)+

        //    var key = x.GetProperty("Key").Values;            
        //    for (int i = 0; i < key.Count / 5; i++)
        //    {
        //        int time =int.Parse(key[5 * i]);
        //        float value = float.Parse(key[5 * i + 1]);
        //        lookup[time] += new Vector3(value, 0, 0);
        //    }
        //    key = y.GetProperty("Key").Values;
        //    for (int i = 0; i < key.Count / 5; i++)
        //    {
        //        int time = int.Parse(key[5 * i]);
        //        float value = float.Parse(key[5 * i + 1]);
        //        lookup[time] += new Vector3(0, value, 0);
        //    }

        //    key = z.GetProperty("Key").Values;
        //    for (int i = 0; i < key.Count / 5; i++)
        //    {
        //        int time = int.Parse(key[5 * i]);
        //        float value = float.Parse(key[5 * i + 1]);
        //        lookup[time] += new Vector3(0, 0, value);
        //    }

        //    return lookup;
        //}

        #region IEngineComponent Members

        public bool Disposed
        {
            get { return false; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
          
        }

        #endregion

        //private FBXNode GetRootBone(FBXNode skin)
        //{
        //    if (skin.Connections.Count > 0)
        //    {
        //        var deformer = skin.FindFirstConnection(x => x.Target.RelatedType == "Cluster");
        //        var bone = deformer.FindFirstConnection(x => x.Target.RelatedType == "LimbNode" || x.Target.RelatedType == "Root");
        //        return bone.FindFirstConnectedTo(x => IsBone(x.Target) && x.IsDirectChildOf(doc.BindingManager.SceneGraph));
        //    }
        //    return null;
        //}                
        //private ModelMesh CreateMesh(FBXNode meshNode)
        //{
        //    ModelMesh mesh;
        //    bool computeNormal = false, computeTexCoord = false;

        //    var skin = meshNode.FindFirstConnection(x => x.Target.RelatedType == "Skin");
        //    if (skin == null)
        //    {
        //        mesh = CreateBasicModelMesh(meshNode.Target, out computeNormal, out computeTexCoord);
        //    }
        //    else
        //    {
        //        FBXNode rootBone = GetRootBone(skin);
        //        if (rootBone == null) throw new InvalidOperationException("Invalid bones configuration");

        //        Bone root;
        //        if (!sceneBones.TryGetValue(rootBone, out root))
        //        {
        //            root = CreateBoneHeirarchy(rootBone);
        //            sceneBones.Add(rootBone, root);                   
        //        }
        //        mesh = CreateSkeletalMesh(meshNode, root, out computeNormal, out computeTexCoord);
        //    }

        //    if (computeNormal)
        //        mesh.ComputeNormals();
        //    if (computeTexCoord)
        //        mesh.ComputeTextureCoords(MappingType.Spherical);

        //    mesh.ComputeTangents();

        //    return mesh;
        //}

        //private ModelGroup CreateGroupContainer(FBXObject fbxGroup)
        //{
        //    ModelGroup group = new ModelGroup(null);
        //    group.Name = fbxGroup.Name;

        //    var translationProp = fbxGroup.GetProperty("Lcl Translation");
        //    var rotationProp = fbxGroup.GetProperty("Lcl Rotation");
        //    var scalingProp = fbxGroup.GetProperty("Lcl Scaling");

        //    var gtranslation = fbxGroup.GetProperty("GeometricTranslation");
        //    var grotation = fbxGroup.GetProperty("GeometricRotation");
        //    var gscaling = fbxGroup.GetProperty("GeometricScaling");

        //    Vector3 scaling = new Vector3(float.Parse(scalingProp.Values[3]), float.Parse(scalingProp.Values[5]), float.Parse(scalingProp.Values[4]))
        //                       + new Vector3(float.Parse(gscaling.Values[3]), float.Parse(gscaling.Values[5]), float.Parse(gscaling.Values[4]));

        //    Vector3 rotation = new Vector3(GMaths.ToRadians(-float.Parse(rotationProp.Values[3])), GMaths.ToRadians(-float.Parse(rotationProp.Values[5])), GMaths.ToRadians(-float.Parse(rotationProp.Values[4]))) +
        //                       new Vector3(GMaths.ToRadians(-float.Parse(grotation.Values[3])), GMaths.ToRadians(-float.Parse(grotation.Values[5])), GMaths.ToRadians(-float.Parse(grotation.Values[4])));

        //    Vector3 translation = new Vector3(float.Parse(translationProp.Values[3]), float.Parse(translationProp.Values[5]), float.Parse(translationProp.Values[4])) +
        //                          new Vector3(float.Parse(gtranslation.Values[3]), float.Parse(gtranslation.Values[5]), float.Parse(gtranslation.Values[4]));

        //    Matrix rot = Matrix.RotationX(rotation.X) * Matrix.RotationY(rotation.Y) * Matrix.RotationZ(rotation.Z);
        //    //Matrix transformation = rot * Matrix.Scaling(scaling) * Matrix.Translation(translation);

        //    group.Translation = translation;
        //    group.Orientation = Attitude.FromMatrix(rot);
        //    group.Scale = scaling;

        //    return group;
        //}

        //private Model3D CreateBasicModel(FBXNode meshDataObject)
        //{
        //    bool computeNormals, computeTexC;

        //    ModelMesh mesh = CreateBasicModelMesh(meshDataObject.Target, out computeNormals, out computeTexC);
        //    processedMeshes.Add(meshDataObject, mesh);
        //    meshesNodelookup.Add(mesh, meshDataObject);

        //    Model3D model = new Model3D(name: meshDataObject.Target.Name, vd: mesh.VertexDescriptor);
        //    List<MeshMaterial> materials = new List<MeshMaterial>();
        //    CreateMaterials(meshDataObject, mesh, materials);
        //    model.Materials = materials.ToArray();

        //    model.SetMeshes(new ModelMesh[] { mesh });

        //    if (computeNormals)
        //        model.ComputeNormals();

        //    if (computeTexC)
        //        model.ComputeTextureCoords(MappingType.Spherical);

        //    model.ComputeTangents();

        //    model.CommitChanges();

        //    return model;
        //}

        //private Model3D CreateSkeletalModel(FBXNode rootBoneNode, Bone rootBone)
        //{
        //    List<FBXNode> mesheNodes = new List<FBXNode>();
        //    List<ModelMesh> meshes = new List<ModelMesh>();
        //    Model3D model = new Model3D("skeletalMode" + nameIndex++, GTools.GetVertexDescriptor<SkinVertex>(GEngine.Graphics), rootBone);
        //    bool computeNormal = false, computeTexCoord = false;

        //    //Find all meshes that are connected to the rootBone
        //    foreach (var node in doc.BindingManager.SceneGraph.Connections.Select(x => x.Value))
        //    {
        //        FBXObject target = node.Target;
        //        if (target.Type == "Model" && target.RelatedType == "Mesh")
        //        {
        //            var skin = node.FindFirstConnection(x => x.Target.RelatedType == "Skin");
        //            if (skin != null)
        //            {
        //                var meshRootBone = GetRootBone(skin);
        //                if (meshRootBone == rootBoneNode)
        //                    mesheNodes.Add(node);
        //            }
        //        }
        //    }

        //    List<MeshMaterial> materials = new List<MeshMaterial>();
        //    //Create meshes
        //    Bone[] bones = rootBone.EnumerateBones().ToArray();
        //    foreach (var meshNode in mesheNodes)
        //    {
        //        var mesh = CreateSkeletalMesh(meshNode, rootBone, out computeNormal, out computeTexCoord);

        //        processedMeshes.Add(meshNode, mesh);
        //        meshesNodelookup.Add(mesh, meshNode);

        //        CreateMaterials(meshNode, mesh, materials);

        //        meshes.Add(mesh);
        //    }

        //    model.Materials = materials.ToArray();
        //    model.SetMeshes(meshes.ToArray());
        //    model.Bones = bones;

        //    if (computeNormal)
        //        model.ComputeNormals();
        //    if (computeTexCoord)
        //        model.ComputeTextureCoords(MappingType.Spherical);

        //    model.ComputeTangents();

        //    model.CommitChanges();
        //    model.ComputeBoneOffsetMatrices();
        //    return model;
        //}

        //private Model3D CreateGroupedModel(FBXNode rootGroup)
        //{
        //    VertexDescriptor vd; Bone root;
        //    ModelGroup g = CreateGroupHeirarchy(rootGroup, out vd, out root);

        //    if (g.Groups == null && g.Meshes == null)
        //        return null;

        //    List<MeshMaterial> materials = new List<MeshMaterial>();
        //    Model3D model = new Model3D(g.Name, vd, root);

        //    if (g.Meshes != null)
        //    {
        //        foreach (var mesh in g.Meshes)
        //        {
        //            mesh.Group = null;
        //        }
        //    }

        //    if (root != null)
        //        model.Bones = root.EnumerateBones().ToArray();

        //    if (g.Groups != null)
        //        model.Groups = g.Groups;
        //    else
        //        model.SetMeshes(g.Meshes);

        //    foreach (var m in model.Meshes)
        //    {
        //        var node = meshesNodelookup[m];
        //        CreateMaterials(node, m, materials);
        //    }
        //    model.Materials = materials.ToArray();

        //    model.Translation = g.Translation;
        //    model.Orientation = g.Orientation;
        //    model.Scale = g.Scale;

        //    model.CommitChanges();

        //    return model;
        //}

        //private ModelGroup CreateGroupHeirarchy(FBXNode fBXNode, out VertexDescriptor vd ,out Bone root)
        //{
        //    vd = null;
        //    root = null;
        //    List<ModelGroup> components = new List<ModelGroup>();
        //    List<ModelMesh> meshes = new List<ModelMesh>();

        //    ModelGroup group = CreateGroupContainer(fBXNode.Target);           

        //    foreach (var g in fBXNode.Connections)
        //    {
        //        var target = g.Value.Target;
        //        if (target.Type == "Model" && target.RelatedType == "Null")
        //        {
        //            ModelGroup node = CreateGroupHeirarchy(g.Value, out vd, out root);
        //            components.Add(node);
        //        }
        //        else if (target.Type == "Model" && target.RelatedType == "Mesh")
        //        {
        //            ModelMesh mesh;
        //            if (!processedMeshes.TryGetValue(g.Value, out mesh))
        //            {
        //                mesh = CreateMesh(g.Value);
        //                vd = mesh.VertexDescriptor;

        //                if (root == null && mesh.bones != null)
        //                    root = mesh.bones[0].Root;

        //                processedMeshes.Add(g.Value, mesh);
        //                meshesNodelookup.Add(mesh, g.Value);
        //            }
        //            meshes.Add(mesh);
        //        }
        //    }

        //    if(components.Count > 0)
        //    {
        //        if (meshes.Count > 0)
        //        {
        //            ModelGroup newGroup = new ModelGroup(group.Model, group);
        //            newGroup.Meshes = meshes.ToArray();
        //            components.Add(newGroup);
        //        }

        //        group.Groups = components.ToArray();               
        //    }
        //    else if(meshes.Count > 0)
        //        group.Meshes = meshes.ToArray();

        //    return group;
        //}     

        //private void CreateMaterials(FBXNode meshDataObject, ModelMesh mesh ,List<MeshMaterial> materials)
        //{
        //    var connections = meshDataObject.FindConnections(x => x.Target.Type == "Material").ToArray();
        //    MeshLayer[] layers = mesh.Layers;
        //    if (connections.Length > 0)
        //    {            
        //        for (int i = 0; i < connections.Length; i++)
        //        {
        //            var conectedMaterial = connections[i];
        //            var matIndex = materials.FindIndex(x => x.Name == conectedMaterial.Target.Name);
        //            if (matIndex < 0)
        //            {
        //                var material = CreateMaterial(conectedMaterial);
        //                matIndex = materials.Count;
        //                materials.Add(material);
        //            }

        //            //update layer materials 
        //            for (int j = 0; j < layers.Length; j++)
        //            {
        //                if (layers[j].materialIndex == i)
        //                    layers[j].materialIndex = matIndex;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MeshMaterial material = MeshMaterial.CreateDefaultMaterial();                
        //        var diffuse = meshDataObject.Target.GetProperty("Color").Values;                
        //        material.Diffuse = new Vector3(float.Parse(diffuse[3]), float.Parse(diffuse[4]), float.Parse(diffuse[5]));
        //        material.Name = "Color[" + material.Diffuse.ToString() + "]";
        //        int matIndex = materials.Count;
        //        materials.Add(material);

        //        //update layer materials 
        //        for (int j = 0; j < layers.Length; j++)
        //        {
        //            layers[j].materialIndex = matIndex;
        //        }
        //    }

        //    mesh.SetupMaterialsLayers();
        //}
        //private ModelMesh CreateBasicModelMesh(FBXObject meshDataObject, out bool computeNormal,out bool computeTexCoords)
        //{
        //    Dictionary<int, List<int>> vertexMappingTable;
        //    ModelVertex[] vertices;
        //    uint[] indices;          

        //    var mesh = CreateMesh(meshDataObject, materialPolygons, out computeNormal, out computeTexCoords,              
        //        (pos, normal, texC) => new ModelVertex(pos, normal, new Vector3(), texC, 0),
        //        (v, normal, texC) => v.Normal == normal && v.TexCoord == texC,
        //        out vertexMappingTable,
        //        out vertices, out indices);

        //    return mesh;
        //}
        //private ModelMesh CreateSkeletalMesh(FBXNode node, Bone rootBone, out bool computeNormal, out bool computeTexCoords)
        //{
        //    var skeletalRender = (Igneel.Shaders.SkeletalModelRender)GEngine.RenderManager.GetRender(Model3D.SkeletalModelRenderClassId);
        //    if (skeletalRender == null)
        //    {
        //        skeletalRender = new Igneel.Shaders.SkeletalModelRender();
        //        GEngine.RenderManager.RegisterRenderStack(Model3D.SkeletalModelRenderClassId, skeletalRender);
        //    }

        //    Dictionary<int, List<int>> vertexMappingTable;
        //    SkinVertex[] skinVertices;
        //    uint[] indices;

        //    var mesh = CreateMesh(node.Target, materialPolygons, out computeNormal, out computeTexCoords,            
        //        (pos, normal, texC) => new SkinVertex(pos, normal, new Vector3(), texC, 0),
        //        (v, normal, texC) => v.Normal == normal && v.TexCoord == texC,
        //        out vertexMappingTable,
        //        out skinVertices, out indices);


        //    mesh.MaxPalleteEntries = skeletalRender.MaxPaletteMatrices;

        //    Dictionary<string, Bone> bonesNameMapping = rootBone.EnumerateBones().ToDictionary(x => x.Name);

        //    List<int>[] verticesIndices = new List<int>[vertexMappingTable.Keys.Count];
        //    List<float>[] verticesWeights = new List<float>[vertexMappingTable.Keys.Count];
        //    List<Bone> meshBones = new List<Bone>();

        //    List<List<Vector3>> bonesPositions = new List<List<Vector3>>();

        //    List<Matrix> offsetMatrices = new List<Matrix>();
        //    var skinNode = node.Connections.Single(x => x.Value.Target.RelatedType == "Skin").Value;

        //    #region Get SkinInfo

        //    int maxVertexInfluences = int.MinValue;

        //    foreach (var deformer in from x in skinNode.Connections
        //                             where x.Value.Target.RelatedType == "Cluster"
        //                             select x.Value)
        //    {

        //        var vertices = deformer.Target.GetDeclaration<FBXFloatListProperty>("Indexes").FloatValues;
        //        var weights = deformer.Target.GetDeclaration<FBXFloatListProperty>("Weights").FloatValues;
        //        var offsetBoneMatrix = Matrix.Invert(GetMatrix4x4(deformer.Target.GetDeclaration<FBXFloatListProperty>("TransformLink").FloatValues));

        //        var fbxBone = deformer.Connections[0].Value.Target;
        //        Bone bone = bonesNameMapping[fbxBone.Name];
        //        int boneIndex = meshBones.IndexOf(bone);

        //        if (boneIndex < 0)
        //        {
        //            boneIndex = meshBones.Count;                  
        //            meshBones.Add(bone);
        //            offsetMatrices.Add(offsetBoneMatrix);                    
        //            bonesPositions.Add(new List<Vector3>(vertices.Count));
        //        }

        //        var list = bonesPositions[boneIndex];
        //        for (int i = 0; i < vertices.Count; i++)
        //        {
        //            int posIndex = (int)vertices[i];
        //            list.Add(skinVertices[vertexMappingTable[posIndex][0]].Position);

        //            if (verticesIndices[posIndex] == null)
        //            {
        //                verticesIndices[posIndex] = new List<int>();
        //                verticesWeights[posIndex] = new List<float>();
        //            }

        //            verticesIndices[posIndex].Add(boneIndex);
        //            verticesWeights[posIndex].Add(weights[i]);
        //            maxVertexInfluences = Math.Max(maxVertexInfluences, verticesIndices[posIndex].Count);
        //        }
        //    }

        //    if (maxVertexInfluences > 4)
        //        maxVertexInfluences = 4; //throw new InvalidOperationException("BlendIndices out of Range");

        //    //for (int i = 0; i < bonesPositions.Count; i++)
        //    //    meshBones[i].CreateBoundingVolumes(bonesPositions[i]);

        //    #endregion

        //    #region Generate IndexedBlendMesh

        //    //Set the vertex bone indices and weights respect to the mesh bone index of a bone 
        //    mesh.MaxVertexInfluences = maxVertexInfluences;

        //    foreach (var item in vertexMappingTable)
        //    {
        //        var list = item.Value;
        //        var blendIndices = verticesIndices[item.Key];
        //        var blendWeights = verticesWeights[item.Key];

        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            skinVertices[list[i]].BlendIndices = GetInt(blendIndices);
        //            skinVertices[list[i]].BlendWeights = GetFloat4(blendWeights);
        //        }
        //    }

        //    #endregion

        //    mesh.Bones = meshBones.ToArray();
        //    mesh.BoneOffsetMatrices = offsetMatrices.ToArray();

        //    if (meshBones.Count > mesh.MaxPalleteEntries)
        //        SetupBoneLayers(mesh, skinVertices, indices);
        //    else
        //    {
        //        mesh.CreateVertexBuffer(skinVertices);
        //    }

        //    return mesh;

        //}
    }
}
