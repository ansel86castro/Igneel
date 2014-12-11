using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
using System.IO;
using System.Threading;
using Igneel.Importers;
using Igneel.Assets;
using Igneel.Rendering;

namespace Igneel.Importers
{
    class OBJModel
    {
        public enum VertexFormat { Position = 1, Normal = 2, TexCoord = 4 }

        string name;      
        List<Group> groups;
        SortedList<string, Material> materials;
        VertexFormat vertexFormat;

        Vector3[] positions;
        Vector3[] normals;
        Vector2[] texCoords;

        public OBJModel()
        {
            groups = new List<Group>();
            materials = new SortedList<string, Material>();
            vertexFormat = OBJModel.VertexFormat.Position;
        }
        public string Name { get { return name; } set { name = value; } }

        public List<Group> Groups { get { return groups; } }

        public VertexFormat Format { get { return vertexFormat; } set { vertexFormat = value; } }

        public IList<Material> Materials { get { return materials.Values; } }

        public Vector3[] Positions { get { return positions; } set { positions = value; } }

        public Vector3[] Normals { get { return normals; } set { normals = value; } }

        public Vector2[] TexCoords { get { return texCoords; } set { texCoords = value; } }

        public void SetFormat(VertexFormat format)
        {
            vertexFormat = vertexFormat | format;
        }

        public void AddGroup(Group g)
        {
            g.model = this;
            groups.Add(g);
        }

        public Group CreateGroup(string name)
        {
            Group g = new Group(name) { model = this };
            groups.Add(g);

            return g;
        }

        public Material GetMaterialByName(string name)
        {
            try
            {
                return materials[name];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public void AddMaterial(Material material)
        {
            if (material.Name == null)
                material.Name = "default";

            if(!materials.ContainsKey(material.Name))
                materials.Add(material.Name, material);
        }

        public EngineContent ToSceneNode()
        {
            string name = Path.GetFileNameWithoutExtension(this.name);
            VertexDescriptor vd = EngineResources.CreateVertexDescriptor<ModelVertex>();
            List<Tuple<string, Mesh>> meshes = new List<Tuple<string, Mesh>>(groups.Count);
            List<SceneNode> nodes = new List<SceneNode>(groups.Count);

            MeshMaterial[] sceneMaterials =new MeshMaterial[materials.Count];
            Dictionary<string, MeshMaterial> materialLookup = new Dictionary<string, MeshMaterial>();

            for (int i = 0; i < materials.Values.Count; i++)
            {
                var sourceMat = materials.Values[i];
                sceneMaterials[i] = new MeshMaterial
                {
                    Name = name + "_" + sourceMat.Name,
                    Surface = sourceMat.ToSurfaceInfo(),
                    NormalMap = sourceMat.NormalMap,
                    DiffuseMap = sourceMat.Textures != null && sourceMat.Textures.Length > 0 ?
                                 sourceMat.Textures[0] : null
                };
                materialLookup.Add(sceneMaterials[i].Name, sceneMaterials[i]);
            }


            List<MeshLayer> layers = new List<MeshLayer>();

            //register for each positionIndex in the source container the List of the destination vertices that contain that position
            Dictionary<int, List<VertexInfo>> lookup = new Dictionary<int, List<VertexInfo>>();
            VertexInfo vi;
            ModelVertex v;

            List<ModelVertex> vertexes = new List<ModelVertex>();
            uint[] indexes;

            #region Groups

            foreach (var g in groups)
            {
                List<MeshMaterial> meshMaterials = new List<MeshMaterial>();
                Mesh mesh = new Mesh(vd: vd);
                mesh.Name = g.Name;
                indexes = new uint[g.FaceCount * 3];
                int k = 0;

                #region Layers

                layers.Clear();
                foreach (var layer in g.Layers)
                {
                    int startVertex = int.MaxValue;
                    int vertexCount = 0;

                    MeshLayer meshLayer = new MeshLayer();
                    meshLayer.startIndex = k;
                    meshLayer.primitiveCount = layer.Faces.Count;

                    var mat = materialLookup[name + "_" + layer.MaterialName];
                    meshLayer.materialIndex = meshMaterials.IndexOf(mat);
                    if (meshLayer.materialIndex < 0)
                    {
                        meshLayer.materialIndex = meshMaterials.Count;
                        meshMaterials.Add(mat);                       
                    }                                    

                    #region Faces

                    foreach (var face in layer.Faces)
                    {
                        //for each vertex of the face create a new mesh vertex if the vertex if not yet in the mesh add it to the VertexBuffer
                        //and create a new face in the IndexBuffer
                        for (int i = 0; i < 3; i++)
                        {
                            //vi describe a new vertex
                            vi = new VertexInfo() { PositionIndex = face.Vertexes[i].Position, NormalIndex = -1, TcIndex = -1 };

                            //if the vertex position is not in the VertexBuffer add it 
                            if (!lookup.ContainsKey(vi.PositionIndex))
                            {
                                v = new ModelVertex(position: positions[vi.PositionIndex]);

                                if ((vertexFormat & VertexFormat.Normal) == VertexFormat.Normal && face.Vertexes[i].Normal >= 0)
                                {
                                    vi.NormalIndex = face.Vertexes[i].Normal;
                                    v.Normal = normals[vi.NormalIndex];
                                }
                                if ((vertexFormat & VertexFormat.TexCoord) == VertexFormat.TexCoord && face.Vertexes[i].TexCoord >= 0)
                                {
                                    vi.TcIndex = face.Vertexes[i].TexCoord;
                                    v.TexCoord = texCoords[vi.TcIndex];
                                }

                                vi.VertexIndex = vertexes.Count;
                                lookup.Add(vi.PositionIndex, new List<VertexInfo>() { vi });

                                vertexes.Add(v);
                                vertexCount++;
                                indexes[k] = (uint)vi.VertexIndex;
                            }
                            else
                            {
                                //else get the list of vertices that contains that position and
                                // if new vertex is not in the list create the new destination vertex and add it to the VertexBuffer

                                var vlist = lookup[vi.PositionIndex];

                                if ((vertexFormat & VertexFormat.Normal) == VertexFormat.Normal)
                                    vi.NormalIndex = face.Vertexes[i].Normal;
                                if ((vertexFormat & VertexFormat.TexCoord) == VertexFormat.TexCoord)
                                    vi.TcIndex = face.Vertexes[i].TexCoord;

                                int index = vlist.FindIndex(x => x.Equals(vi));

                                if (index < 0)
                                {
                                    v = new ModelVertex(positions[vi.PositionIndex]);
                                    if (vi.NormalIndex >= 0) v.Normal = normals[vi.NormalIndex];
                                    if (vi.TcIndex >= 0) v.TexCoord = texCoords[vi.TcIndex];

                                    vi.VertexIndex = vertexes.Count;
                                    indexes[k] = (uint)vi.VertexIndex;
                                    vertexCount++;
                                    vertexes.Add(v);
                                    vlist.Add(vi);
                                }
                                else
                                {
                                    //else the vertex is already in the VertexBuffer so create add the vertex index
                                    //to the indexbuffer

                                    vi = vlist[index];
                                    indexes[k] = (uint)vi.VertexIndex;
                                }
                            }
                            k++;
                            startVertex = Math.Min(startVertex, vi.VertexIndex);
                        }
                    }

                    #endregion Faces

                    meshLayer.startVertex = startVertex;
                    meshLayer.vertexCount = vertexCount;
                    layers.Add(meshLayer);
                }

                #endregion Layers
               
                mesh.SetLayers(layers.ToArray());
                var data = vertexes.ToArray();
                mesh.CreateVertexBuffer(data);
                if (mesh.VertexCount < ushort.MaxValue)
                {
                    mesh.CreateIndexBuffer(indexes.Select(x => (ushort)x).ToArray());
                }
                else
                {
                    mesh.CreateIndexBuffer(indexes);
                }

                mesh.DefragmentLayers();
                mesh.BlendLayers();

                if ((vertexFormat & VertexFormat.Normal) != VertexFormat.Normal)
                    mesh.ComputeNormals();
                if ((vertexFormat & VertexFormat.TexCoord) != VertexFormat.TexCoord)
                    mesh.ComputeTextureCoords(CoordMappingType.Spherical);

                mesh.ComputeTangents();                
                nodes.Add(new SceneNode<MeshInstance>(g.Name, new MeshInstance(meshMaterials.ToArray(), mesh)));

                vertexes.Clear();

                //test
                lookup.Clear();

            }

            #endregion

            QuadTreeSceneNode node = new QuadTreeSceneNode(name, 10);
            node.Context = new TechniqueRenderContext(node);           

            foreach (var item in nodes)
            {
                node.Add(item);
            }

            node.UpdateLayout();

            var package = new EngineContent(name);
            package.Providers.AddRange(sceneMaterials);
            package.Providers.Add(node);
            return package;
        }

        struct VertexInfo:IEquatable<VertexInfo>
        {
            /// <summary>
            /// Vertex Index in the destination Mesh
            /// </summary>
            public int VertexIndex;

            /// <summary>
            /// Position index, Normal index , and TextureCoord index in the source Container
            /// </summary>
            public int PositionIndex, NormalIndex, TcIndex;

            #region IEquatable<VertexInfo> Members

            public bool Equals(VertexInfo other)
            {
                return PositionIndex == other.PositionIndex && NormalIndex == other.NormalIndex && TcIndex == other.TcIndex;
            }
            public override bool Equals(object obj)
            {
                if (!(obj is VertexInfo)) return false;
                VertexInfo other = (VertexInfo)obj;

                return PositionIndex == other.PositionIndex && NormalIndex == other.NormalIndex && TcIndex == other.TcIndex;
            }

            #endregion
        }
     
    }

    class Group
    {
        List<Layer> layers = new List<Layer>();
        internal OBJModel model;
        private string name;
       
        public int StartVertex;
        public int FaceCount;

        public Group() { }

        public Group(string name)
        {
            // TODO: Complete member initialization
            this.name = name;
        }      

        public List<Layer> Layers
        {
            get { return layers; }
            set { layers = value; }
        }

        internal OBJModel Model { get { return model; } set { model = value; } }
        public string Name { get { return name; } set { name = value; } }

        public Layer CreateLayer(string materialName)
        {
            Layer l = new Layer() { MaterialName = materialName };
            layers.Add(l);
            return l;
        }
       
        public void Validate()
        {
            //obtener startVertex & VertexCount
        }


        public bool VertexCount { get; set; }
    }

    class Layer
    {        
        public string MaterialName;
        public Material Material;
        public List<Face> Faces=new List<Face>();       
    }
    struct Vertex
    {
        public int Position;
        public int Normal;
        public int TexCoord;

        public Vertex(int[] indexes)
        {
            Normal = -1;
            TexCoord = -1;

            Position = indexes[0];
            if (indexes.Length > 1)
                TexCoord = indexes[1];
            if (indexes.Length > 2)
                Normal = indexes[2];
        }

        public Vertex(int position, int texCoord, int normal)
        {
            Position = position;
            TexCoord = texCoord;
            Normal = normal;

        }

        public override string ToString()
        {
            return string.Format("P:{0} T:{1} N:{2}", Position, TexCoord, Normal);
        }

    }      

    struct Face
    {
        /// <summary>
        /// Index[0] = v0 , Index[0][0] = index of Position0 ,Index[0][1] = index of Normal0 ,Index[0][2] = index of TexCoord0
        /// </summary>
        public Vertex[] Vertexes;

        public Face(int vertexCount)
        {
            this.Vertexes = new Vertex[vertexCount];
        }
        public Vertex V0 { get { return Vertexes[0]; } }
        public Vertex V1 { get { return Vertexes[1]; } }
        public Vertex V2 { get { return Vertexes[2]; } }
    }

    class Material
    {
        public string Name;
        public Vector3 Ambient;
        public Vector3 Difusse;
        public Vector3 Specular;
        public float SpecularPower;
        public float Reflectivity;
        public float Transparency;
        public float selftIlumination;
        //public string TextureDiffuseFile;
        //public string TextureNormalFile;
        //public string TextureCubeFile;
        public string[] Textures;
        public float Refractivity;
        public string NormalMap;

        public Material(string name)
        {
            // TODO: Complete member initialization
            this.Name = name;          
        }

        internal LayerSurface ToSurfaceInfo()
        {
            LayerSurface si = new LayerSurface()
            {
                Diffuse = Difusse,
                Specular = Specular,
                SpecularPower = SpecularPower,
                Alpha = Transparency,
                Reflectivity = Reflectivity,
                Refractitity = Refractivity
            };
            return si;
        }
    }
}