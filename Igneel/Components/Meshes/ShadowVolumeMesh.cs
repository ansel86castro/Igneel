using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Igneel.Rendering;

namespace Igneel.Components
{
    //struct ShadowVertex : IVertex
    //{
    //    public Vector3 position;
    //    public Vector3 normal;

    //    public ShadowVertex(Vector3 position, Vector3 normal)
    //    {
    //        this.position = position;
    //        this.normal = normal;
    //    }

    //    public static readonly VertexElement[] Elements = new VertexElement[]
    //    {
    //        new VertexElement(0,0,DeclarationType.Float3,DeclarationMethod.Default,DeclarationUsage.Position,0),
    //        new VertexElement(0,12,DeclarationType.Float3,DeclarationMethod.Default,DeclarationUsage.Normal,0),
    //        VertexElement.VertexDeclarationEnd
    //    };

    //    public static VertexFormat Format = VertexFormat.PositionNormal;

    //    #region IVertex Members

    //    public VertexElement[] VertexElements
    //    {
    //        get { return Elements; }
    //    }

    //    #endregion
    //}

    //public class StaticShadowVolumeModel:IEngineComponent
    //{
    //    StaticShadowVolumeMesh[] meshes;
    //    VertexDescriptor vd;
    //    Model3D model;
    //    bool disposed;
    //    public StaticShadowVolumeModel(Model3D model)
    //    {
    //        this.model = model;
    //        vd = Geometry.GetVertexDescriptor<ShadowVertex>(GEngine.Graphics);
    //        meshes = new StaticShadowVolumeMesh[model.Meshes.Count];

    //        for (int i = 0; i < meshes.Length; i++)
    //        {
    //            meshes[i] = new StaticShadowVolumeMesh(model.Meshes[i], vd);
    //            meshes[i].BuildShadowFromMesh();
    //        }
    //    }

    //    public StaticShadowVolumeMesh[] Meshes { get { return meshes; } }

    //    public bool Visible
    //    {
    //        get
    //        {
    //            return true;
    //        }
    //        set
    //        {
                
    //        }
    //    }       

    //    public VertexDescriptor VertexDescriptor
    //    {
    //        get { return vd; }
    //    }
        
    //    public void Dispose()
    //    {
    //        if (!disposed)
    //        {
    //            vd.Dispose();
    //            foreach (var m in meshes)
    //                m.Dispose();
    //            disposed = true;
    //        }
    //    }     

    //    public bool Disposed
    //    {
    //        get { return disposed; }
    //    }      
    //}

    //public class StaticShadowVolumeMesh : IEngineComponent
    //{                
    //    ShadowVertex[]shadowVertexes;
    //    int[] shadowIndices;
    //    ModelMesh modelGroup;
    //    Dictionary<Edge, List<int>> edgeFaceLookup;
    //    Dictionary<Edge, Edge> edgeQuadsLookup;
    //    VertexP[] meshVertexes;
    //    ushort[] meshIndices;
    //    int[] adjacency;
    //    VertexDescriptor vd;
    //    VertexBuffer vb;
    //    IndexBuffer ib;
    //    private bool disposed;

    //    public VertexBuffer VertexBuffer { get { return vb; } }
    //    public IndexBuffer IndexBuffer { get { return ib; } }
    //    public ModelMesh ModelGroup { get { return modelGroup; } }
    //    public int VertexCount { get { return shadowVertexes.Length; } }
    //    public int IndicesCount { get { return shadowIndices.Length; } }

    //    public StaticShadowVolumeMesh(ModelMesh modelGroup, VertexDescriptor vd)
    //    {
    //        this.vd = vd;
    //        ModelVertex[] vertexes = (ModelVertex[])modelGroup.GetVertexBufferData();
    //        meshVertexes = new VertexP[vertexes.Length];
    //        for (int i = 0; i < vertexes.Length; i++)
    //        {
    //            meshVertexes[i] = new VertexP(vertexes[i].Position);
    //        }
                
    //        meshIndices = (ushort[])modelGroup.GetIndexBufferData();
    //        if (modelGroup.Adjacency == null)
    //            modelGroup.GenerateAdjacency();
    //        adjacency = modelGroup.Adjacency;
    //        edgeQuadsLookup = new Dictionary<Edge,Edge>(edgeFaceLookup.Keys.Count);
    //        this.modelGroup = modelGroup;
    //    }

    //    public void BuildShadowFromMesh()
    //    {
    //        /*
    //         * Iterar por cada triangulo y por cada triangulo  hacer :
    //         *      1 - Tres nuevos vertices son generados y un nuevo triángulo , cada triángulo debe tener sus propios vertices
    //         *          ya que cada triángulo esta separado de su adjacente por un quad degenerado                 
    //         *      2 - Las normales de los vertices del nuevo tríangulo  son las del tríangulo.
    //         *      3 - For each edge of the added face, the algorithm looks through the edge mapping table,
    //         *          and if it cannot find an existing entry for the source edge, it creates one and initializes 
    //         *          the source edge and one output edge of the edge mapping entry. However, if it finds that the source 
    //         *          edge already has an entry in the table, then it has the four vertices of the quad for this edge, 
    //         *          so it adds the two faces for the quad to the output mesh and remove the edge mapping entry from the 
    //         *          table
    //         */

    //        int faceCount = meshIndices.Length / 3;

    //        shadowVertexes = new ShadowVertex[meshIndices.Length];
    //        shadowIndices = new int[meshIndices.Length + (edgeFaceLookup.Keys.Count * 6)];

    //       int iIndex = 0;
         
    //        //recorrer los triangulo de la src mesh
    //        for (int i = 0; i < meshIndices.Length; i += 3)
    //        {
    //            //create a copy of the vertices for each triangle
    //            Vector3 normal = Geometry.ComputeFaceNormal(meshVertexes[meshIndices[i]].Position, meshVertexes[meshIndices[i + 1]].Position, meshVertexes[meshIndices[i + 2]].Position);

    //            shadowVertexes[i] = new ShadowVertex(meshVertexes[meshIndices[i]].Position, normal);
    //            shadowVertexes[i + 1] = new ShadowVertex(meshVertexes[meshIndices[i + 1]].Position, normal);
    //            shadowVertexes[i + 2] = new ShadowVertex(meshVertexes[meshIndices[i + 2]].Position, normal);

    //            shadowIndices[iIndex] = i;
    //            shadowIndices[iIndex++] = i + 1;
    //            shadowIndices[iIndex++] = i + 2;

    //            for (int j = 0; j < 3; j++)
    //            {
    //                int current = i + j;
    //                int next = i + (j + 1) % 3;                    

    //                Edge srcEdge = new Edge(meshIndices[current], meshIndices[next]);
    //                Edge destEdge = new Edge(current, next);
    //                if (!edgeQuadsLookup.ContainsKey(srcEdge))
    //                    edgeQuadsLookup.Add(srcEdge, destEdge);
    //                else
    //                {                      
    //                    //create the edge quad
    //                    Edge edge = edgeQuadsLookup[srcEdge];

    //                    //1 first triangle
    //                    shadowIndices[iIndex++] = srcEdge.V1;
    //                    shadowIndices[iIndex++] = srcEdge.V2;
    //                    shadowIndices[iIndex++] = edge.V1;

    //                    //2 triangle
    //                    shadowIndices[iIndex++] = edge.V1;
    //                    shadowIndices[iIndex++] = edge.V2;
    //                    shadowIndices[iIndex++] = srcEdge.V2;

    //                    edgeQuadsLookup.Remove(srcEdge);
    //                }
    //            }                                          
    //        }            

    //        // Now the entries in the edge mapping table represent
    //        // non-shared edges.  What they mean is that the original
    //        // mesh has openings (holes), so we attempt to patch them.

    //        vb =GEngine.Graphics.CreateVertexBuffer<ShadowVertex>(shadowVertexes.Length, Usage.WriteOnly, VertexFormat.None, Pool.Default);
    //        vb.SetData(shadowVertexes);            

    //        ib =GEngine.Graphics.CreateIndexBuffer(shadowIndices.Length, Usage.WriteOnly, Pool.Default,true);
    //        ib.SetData(shadowIndices);
    //    }                 

    //    public void Dispose()
    //    {
    //        if (!disposed)
    //        {
    //            vb.Dispose();
    //            ib.Dispose();
    //            disposed = true;
    //        }
    //    }
       
    //    public VertexDescriptor VertexDescriptor
    //    {
    //        get { return vd; }
    //    }
      
    //    public bool Disposed
    //    {
    //        get { return disposed; }
    //    }     
    //}

    //public class DynamicShadowVolumeModel : IEngineComponent
    //{
    //    Model3D model;
    //    VertexDescriptor vd;
    //    DynamicShadowVolumeMesh[] meshes;
    //    bool disposed;
    //    public DynamicShadowVolumeMesh[] Meshes { get { return meshes; } }
    //    public Model3D Model { get { return model; } }

    //    public DynamicShadowVolumeModel(Model3D model)
    //    {
    //        this.model = model;
    //        this.vd = Geometry.GetVertexDescriptor<VertexP>(GEngine.Graphics);

    //        meshes = new DynamicShadowVolumeMesh[model.Meshes.Count];
    //        for (int i = 0; i < meshes.Length; i++)
    //        {
    //            meshes[i] = new DynamicShadowVolumeMesh(model.Meshes[i], this);
    //        }
    //    }        

    //    public void Reset()
    //    {
    //        Array.ForEach(meshes, x => x.Reset());
    //    }    

    //    public VertexDescriptor VertexDescriptor
    //    {
    //        get { return vd; }
    //    }
        
    //    public void Dispose()
    //    {
    //        if (!disposed)
    //        {
    //            vd.Dispose();
    //            for (int i = 0; i < meshes.Length; i++)
    //            {
    //                meshes[i].Dispose();
    //            }
    //            disposed = true;
    //        }
    //    }     
    //    public bool Visible
    //    {
    //        get
    //        {
    //            return true;
    //        }
    //        set
    //        {
                
    //        }
    //    }     
    //    public bool Disposed
    //    {
    //        get { return disposed; }
    //    }
      
    //}

    //public class DynamicShadowVolumeMesh : IEngineComponent
    //{
    //    Vector3[] vertices;
    //    int numEdges;
    //    private int numVertices;
    //    ModelMesh modelMesh;
    //    ushort[] meshIndices;
    //    Vector3[] meshVertexes;
    //    ushort[] edges;
    //    int[] adyacency;
    //    private DynamicShadowVolumeModel container;

    //    public ModelMesh ModelMesh { get { return modelMesh; } }
    //    public DynamicShadowVolumeModel Container { get { return container; } }
    //    public Matrix WorldMatrix { get { return modelMesh.WorldMatrix; } }
    //    public float EXTRUDE_BIAS;
    //    private bool disposed;
    //    public DynamicShadowVolumeMesh(ModelMesh modelMesh , DynamicShadowVolumeModel container)
    //    {
    //        this.modelMesh = modelMesh;
    //        this.container = container;
    //        this.meshIndices = (ushort[])modelMesh.GetIndexBufferData();
    //        ModelVertex[] vertexes = (ModelVertex[])modelMesh.GetVertexBufferData();
    //        meshVertexes = new Vector3[vertexes.Length];
    //        for (int i = 0; i < vertexes.Length; i++)
    //        {
    //            meshVertexes[i] = vertexes[i].Position;
    //        }
    //        int numFaces = meshIndices.Length / 3;
    //        edges = new ushort[numFaces * 6];

    //       // Dictionary<Edge,List<int>>hast;            
    //        //adyacency = modelMesh.GenerateAdjacency(out hast);

    //        //Mesh m = new Mesh(meshIndices.Length / 3, meshVertexes.Length, MeshFlags.SystemMemory, VertexFormats.Position, GEngine.Graphics);
    //        //m.SetVertexBufferData(meshVertexes, LockFlags.None);
    //        //m.SetIndexBufferData(meshIndices, LockFlags.None);

    //        //adyacency = new int[meshIndices.Length];
    //        //m.GenerateAdjacency(0, adyacency);

    //        //m.Dispose();
    //    }

    //    /// <summary>
    //    /// Reset the shadow volume
    //    /// </summary>
    //    public void Reset()
    //    {
    //        numVertices = 0;
    //        numEdges = 0;
    //    }      

    //    /// <summary>
    //    /// Render the shadow volume
    //    /// </summary>
    //    public void Render()
    //    {
    //        if (numVertices > 0)
    //        {
    //            Device device = GEngine.Graphics;
    //            device.VertexDeclaration = container.VertexDescriptor.VertexDeclaration;
    //            device.DrawUserPrimitives(PrimitiveType.TriangleList, numVertices / 3, vertices);
    //        }
    //    }

    //    public void BuildShadowFromSilohuette(GLight light)
    //    {
    //        Matrix invWorld = Matrix.Invert(modelMesh.WorldMatrix);
    //        //light Direction in local space
    //        Vector3 lightDir = Vector3.Normalize(Vector3.TransformNormal(light.Direction, invWorld));
    //        //light Positon in local space
    //        Vector3 lightPos = Vector3.TransformCoordinate(light.Position, invWorld);
    //        bool useDirectional = light.Type == LightType.Directional;

    //        int numFaces = meshIndices.Length / 3;
    //        ushort[] indices = meshIndices;
    //        EXTRUDE_BIAS = GEngine.Scene.ActiveCamera.FarClipPlane + Vector3.Distance(container.Model.Position , GEngine.Scene.ActiveCamera.Position);
    //        // Allocate a temporary edge list           
    //        edges.Initialize();
          
    //        // For each face
    //        for (int i = 0; i < numFaces; i++)
    //        {
    //            ushort face0 = indices[3 * i + 0];
    //            ushort face1 = indices[3 * i + 1];
    //            ushort face2 = indices[3 * i + 2];
    //            Vector3 v0 = meshVertexes[face0];
    //            Vector3 v1 = meshVertexes[face1];
    //            Vector3 v2 = meshVertexes[face2];

    //            // Transform vertices or transform light?
    //            Vector3 vCross1 = v2 - v1;
    //            Vector3 vCross2 = v1 - v0;
    //            Vector3 vNormal = Vector3.Normalize(Vector3.Cross(vCross1, vCross2));
    //            if (!useDirectional)
    //            {
    //                Vector3 center = new Vector3((v0.X + v1.X + v2.X) / 3.0f, (v0.Y + v1.Y + v2.Y) / 3.0f, (v0.Z + v1.Z + v2.Z) / 3.0f);
    //                lightDir = lightPos - center;
    //                lightDir.Normalize();
    //            }
    //            //if we are facing the light
    //            if (Vector3.Dot(vNormal, lightDir) >= 0.0f)
    //            {
    //                AddEdge(edges, ref numEdges, face0, face1);
    //                AddEdge(edges, ref numEdges, face1, face2);
    //                AddEdge(edges, ref numEdges, face2, face0);
    //            }
    //        }

    //        Vector3 extrude1 = lightDir;
    //        Vector3 extrude2 = lightDir;
    //        int _numVertices = numEdges * 6;
    //        if (vertices == null || vertices.Length < _numVertices)
    //        {
    //            vertices = new Vector3[_numVertices];
    //        }
    //        //solo quedan las aristas que forman parte de la silueta
    //        for (int i = 0; i < numEdges; i++)
    //        {
    //            Vector3 v1 = meshVertexes[edges[2 * i + 0]];
    //            Vector3 v2 = meshVertexes[edges[2 * i + 1]];
    //            if (!useDirectional)
    //            {
    //                extrude1 = v1 - lightPos;
    //                extrude2 = v2 - lightPos;
    //            }
    //            Vector3 v3 = v1 + extrude1 * 100000;
    //            Vector3 v4 = v2 + extrude2 * 100000;

    //            // Add a quad (two triangles) to the vertex list
    //            //vertices[numVertices++] = v1;
    //            //vertices[numVertices++] = v2;
    //            //vertices[numVertices++] = v3;

    //            //vertices[numVertices++] = v2;
    //            //vertices[numVertices++] = v4;
    //            //vertices[numVertices++] = v3;

    //            //side faces
    //            vertices[numVertices++] = v3;
    //            vertices[numVertices++] = v2;
    //            vertices[numVertices++] = v1;

    //            vertices[numVertices++] = v3;
    //            vertices[numVertices++] = v4;
    //            vertices[numVertices++] = v2;
           
    //        }


    //    }

    //    public void AddEdge(ushort[] edges, ref int numEdges, ushort v0, ushort v1)
    //    {
    //        // Remove interior edges (which appear in the list twice)
    //        for (int i = 0; i < numEdges; i++)
    //        {
    //            if ((edges[2 * i + 0] == v0 && edges[2 * i + 1] == v1) ||
    //                (edges[2 * i + 0] == v1 && edges[2 * i + 1] == v0))
    //            {
    //                if (numEdges > 1)
    //                {
    //                    edges[2 * i + 0] = edges[2 * (numEdges - 1) + 0];
    //                    edges[2 * i + 1] = edges[2 * (numEdges - 1) + 1];                       
    //                }
    //                numEdges--;
    //                return;
    //            }
    //        }
    //        edges[2 * numEdges + 0] = v0;
    //        edges[2 * numEdges + 1] = v1;           
    //        numEdges++;
    //    }

    //    public void BuildShadowFromAdjacency(GLight light)
    //    {
    //        Matrix invWorld = Matrix.Invert(modelMesh.WorldMatrix);
    //        //light Direction in local space
    //        Vector3 lightDir = Vector3.Normalize(Vector3.TransformNormal(light.Direction, invWorld));
    //        //light Positon in local space
    //        Vector3 lightPos = Vector3.TransformCoordinate(light.Position, invWorld);
    //        bool useDirectional = light.Type == LightType.Directional;
          
    //        EXTRUDE_BIAS = GEngine.Scene.ActiveCamera.FarClipPlane + Vector3.Distance(container.Model.Position , GEngine.Scene.ActiveCamera.Position);                               

    //        Vector3 adjNormal , vNormal , adjVertex = new Vector3() ,reverLightDir = -lightDir;
    //        Vector3 extrude1 = lightDir, extrude2 = lightDir;
    //        Vector3 ext0 , ext1;
    //        int adjTriangle;

    //        if (vertices == null)
    //        {
    //            //size = numFaces * numEdgePerFace * vertexPerQuads
    //            vertices = new Vector3[meshIndices.Length * 6];
    //        }

    //        for (int k = 0; k < adyacency.Length - 3; k += 3)
    //        {
    //            vNormal = ComputeUnnormalizedTriangleNormal(k);
    //            if (!useDirectional)
    //                lightDir = Vector3.Normalize(lightPos - meshVertexes[meshIndices[k]]);
    //            //if the triangle face the light 
    //            if (Vector3.Dot(vNormal, reverLightDir) > 0)
    //            {
    //                //check for each adjacent triangles if the normal are pointing out the light
    //                for (int i = 0; i < 3; i++)
    //                {                        
    //                    Vector3 v0 = meshVertexes[meshIndices[k + i]];
    //                    Vector3 v1 = meshVertexes[meshIndices[k + (i + 1) % 3]];
    //                    adjTriangle = adyacency[k + i] * 3;
    //                    //if the triangle has adjacency by that edge 
    //                    if (adjTriangle > 0)
    //                    {
    //                        adjNormal = ComputeUnnormalizedTriangleNormal(adjTriangle);

    //                        //if the adjTriangle are not facing the light
    //                        if (Vector3.Dot(adjNormal, reverLightDir) <= 0)
    //                        {
    //                            if (!useDirectional)
    //                            {
    //                                extrude1 = v0 - lightPos;
    //                                extrude2 = v1 - lightPos;
    //                            }
    //                            ext0 = v0 + extrude1 * 100000;
    //                            ext1 = v1 + extrude2 * 100000;

    //                            vertices[numVertices++] = ext0;
    //                            vertices[numVertices++] = v1;
    //                            vertices[numVertices++] = v0;

    //                            vertices[numVertices++] = ext0;
    //                            vertices[numVertices++] = ext1;
    //                            vertices[numVertices++] = v1;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (!useDirectional)
    //                        {
    //                            extrude1 = v0 - lightPos;
    //                            extrude2 = v1 - lightPos;
    //                        }
    //                        ext0 = v0 + extrude1 * 100000;
    //                        ext1 = v1 + extrude2 * 100000;

    //                        vertices[numVertices++] = ext0;
    //                        vertices[numVertices++] = v1;
    //                        vertices[numVertices++] = v0;

    //                        vertices[numVertices++] = ext0;
    //                        vertices[numVertices++] = ext1;
    //                        vertices[numVertices++] = v1;
    //                    }
    //                }
                   
    //            }
                
    //        }
    //    }

    //    private Vector3 ComputeUnnormalizedTriangleNormal(int triangleIndex)
    //    {
    //        Vector3 v0 = meshVertexes[meshIndices[triangleIndex]];
    //        Vector3 v1 = meshVertexes[meshIndices[triangleIndex + 1]];
    //        Vector3 v2 = meshVertexes[meshIndices[triangleIndex + 2]];

    //        return Vector3.Cross(v1 - v0, v2 - v0);
    //    }
      
    //    public void Dispose()
    //    {
    //        if (!disposed)
    //        {
    //            disposed = true;
    //        }
    //    }        

    //    public bool Disposed
    //    {
    //        get { return disposed; }
    //    }

    //}
}
