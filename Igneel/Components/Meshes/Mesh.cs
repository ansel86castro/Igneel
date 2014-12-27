using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Data;
using Igneel.Assets;
using Igneel.Rendering;
using Igneel.Design;
using Igneel.Graphics;
using System.Runtime.InteropServices;
using Igneel.Physics;
using Igneel.Services;

namespace Igneel.Components
{
    public enum CoordMappingType
    {
        None, Spherical, Cylindrical
    }
   
    [TypeConverter(typeof(DesignTypeConverter))]
    [ProviderActivator(typeof(MeshAct))]
    [Asset(Assets.AssetType.Mesh, ".mesh")]
    public class Mesh : ResourceAllocator, IAssetProvider, INameable, IBoundable
    {       
        string name;
        MeshPart[] layers = new MeshPart[0];                    
        string[] materialSlotsNames;         
        bool is16BitIndices;
        int[] adjacency;       
        int vertexCount;
        int faceCount;            
        GraphicBuffer vb;
        GraphicBuffer ib;        
        internal MeshPart[][] materialLayersLookup; //material per layer lookup      
        VertexDescriptor vd;
        OrientedBox box;
        Sphere sphere;
        AABB aabb;
      
        private IntPtr vbStream, ibStream;      

        public Mesh(VertexDescriptor vd)
        {
            if (vd == null) throw new ArgumentNullException("vertex descriptor");
            this.vd = vd;
            var noti = Service.Get<INotificationService>();
            if (noti != null)
                noti.OnObjectCreated(this);
        }
       
        [AssetMember]
        public string Name { get { return name; } set { name = value; } }

        public int VertexCount { get { return vertexCount; } }

        public int FaceCount { get { return faceCount; } }

        public int LayerCount { get { return layers.Length; } }

        public int MaterialSlots { get { return materialLayersLookup.Length; } }

        [AssetMember]
        public MeshPart[] Layers 
        {
            get { return layers; }
            set
            {
                this.layers = value;

                for (int i = 0; i < layers.Length; i++)
                    layers[i].layerID = i;

                _SetupMaterialsLayers();
            }
        }

        [Browsable(false)]
        [AssetMember(typeof(MeshVbSt))]
        public GraphicBuffer VertexBuffer { get { return vb; } }

        [Browsable(false)]
        [AssetMember(typeof(MeshIbSt))]
        public GraphicBuffer IndexBuffer { get { return ib; } }

        public bool Is16BitIndices { get { return is16BitIndices; } }

        [Browsable(false)]
        [AssetMember]
        public int[] Adjacency { get { return adjacency; } set { adjacency = value; } }

        [Browsable(false)]
        public VertexDescriptor VertexDescriptor
        {
            get { return vd; }
        }     

        public string[] MaterialSlotNames { get { return materialSlotsNames; } set { materialSlotsNames = value; } }


        public OrientedBox BoundingBox { get { return box; } }

        public Sphere BoundingSphere { get { return sphere; } }

        public AABB AALocalBox { get { return aabb; } }

        #region Public

        public void ComputeBoundingVolumenes()
        {
            unsafe
            {
                var positions = GetVertexBufferView<Vector3>(IASemantic.Position, 0);                
                this.box =new OrientedBox(positions);
                this.sphere = new Sphere((byte*)positions.BasePter, positions.Count, positions.Stride);                

                aabb = new AABB(new Vector3(float.MaxValue, float.MaxValue,float.MaxValue), 
                                new Vector3(float.MinValue, float.MinValue,float.MinValue));

                for (int i = 0; i < positions.Count; i++)
                {
                    aabb.Maximum = Vector3.Max(positions[i], aabb.Maximum);
                    aabb.Minimum = Vector3.Min(positions[i], aabb.Minimum);
                }

                ReleaseVertexBufferViews();                 
            }
        }      

        /// <summary>
        /// arrange the layers by materials 
        /// </summary>
        private void _SetupMaterialsLayers()
        {
            var materialsIndex = layers.Select(x => x.materialIndex).Distinct().ToArray();
        
            //store for each material index the list of layers
            List<MeshPart>[] materialLayers = new List<MeshPart>[materialsIndex.Length];
            for (int i = 0; i < materialsIndex.Length; i++)
            {
                materialLayers[i] = new List<MeshPart>();
                foreach (var layer in layers)
                {
                    if (layer.materialIndex == i)
                        materialLayers[i].Add(layer);
                }
            }
            materialLayersLookup = new MeshPart[materialsIndex.Length][];
            for (int i = 0; i < materialLayersLookup.Length; i++)
            {
                materialLayersLookup[i] = materialLayers[i].ToArray();
            }           
        }

        public void CreateVertexBuffer<T>(T[] vertexes, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAccess = CpuAccessFlags.ReadWrite)
            where T : struct
        {
            if (vb != null)
                vb.Dispose();

            int size = Marshal.SizeOf(typeof(T));
            vertexCount = (vertexes.Length * size) / vd.Size;

            vb = Engine.Graphics.CreateVertexBuffer<T>(vd.Size, usage, cpuAccess, data: vertexes);
        }
        public void CreateVertexBuffer(Array vertexes, int vertexCount, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAccess = CpuAccessFlags.ReadWrite)        
        {
            if (vb != null)
                vb.Dispose();

            this.vertexCount = vertexCount;
            int size = vertexCount * vd.Size;
            vb = Engine.Graphics.CreateVertexBuffer(size, vd.Size, vertexes, usage, cpuAccess);
        }      

        public void CreateIndexBuffer<T>(T[] indices, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAccess = CpuAccessFlags.ReadWrite)
        {
            faceCount = indices.Length / 3;
            is16BitIndices = Marshal.SizeOf(typeof(T)) == 2;
            if (ib != null)
                ib.Dispose();

            ib = Engine.Graphics.CreateIndexBuffer<T>(usage, cpuAccess, indices);
        }

     
        public void CreateIndexBuffer(Array indices, IndexFormat format = IndexFormat.Index16, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAccess = CpuAccessFlags.ReadWrite)
        {          
            is16BitIndices = format == IndexFormat.Index16;
            faceCount = indices.Length / (is16BitIndices ? 2 : 4) / 3;
            if (ib != null)
                ib.Dispose();

            ib = Engine.Graphics.CreateIndexBuffer(indices.Length, indices, format, usage, cpuAccess);            
        }    

        public BufferView GetVertexViewStream(IASemantic semantic, int index = 0)
        {
            if(vbStream == null)
                vbStream = vb.Map(MapType.ReadWrite);
            return new BufferView(vbStream, vd, semantic, index);
        }

        public void ReleaseVertexBufferViews()
        {
            if(vbStream!=IntPtr.Zero)
                vb.Unmap();
            vbStream = IntPtr.Zero;
        }

        public void ReleaseIndexBufferViews()
        {
            if (ibStream != IntPtr.Zero)
                ib.Unmap();
            ibStream = IntPtr.Zero;
        }

        public void ReleaseViews()
        {
            ReleaseVertexBufferViews();
            ReleaseIndexBufferViews();
        }

        public BufferView<T> GetVertexBufferView<T>(IASemantic usage, int index = 0) where T : struct
        {
            if (vbStream == IntPtr.Zero)
                vbStream = vb.Map(MapType.ReadWrite);
            return new BufferView<T>(vbStream, vd, usage, index, vertexCount);
        }

        public BufferView<T> GetVertexBufferView<T>(int offset) where T : struct
        {
            if (vbStream == IntPtr.Zero)
                vbStream = vb.Map(MapType.ReadWrite);
            return new BufferView<T>(vbStream + offset, Marshal.SizeOf(typeof(T)), vertexCount);
        }

        public IndexBufferView GetIndexBufferView()
        {
            if (ibStream == IntPtr.Zero)
                ibStream = ib.Map(MapType.ReadWrite);
            return new IndexBufferView(ibStream, ib.Stride == 2, (int)ib.SizeInBytes / ib.Stride);
        }

        public IndexedBufferView<T> GetIndexedBufferView<T>(IASemantic semantic, int index = 0) where T : struct
        {
            if (vbStream == IntPtr.Zero)
                vbStream = vb.Map(MapType.ReadWrite);
            if (ibStream == IntPtr.Zero)
                ibStream = ib.Map(MapType.ReadWrite);

            return new IndexedBufferView<T>(vbStream, vd, semantic, index, ibStream, ib.Stride, (int)ib.SizeInBytes / ib.Stride);
        }      

        public void ComputeNormals()
        {
            var trianglePos = GetIndexedBufferView<Vector3>(IASemantic.Position);
            var triangleNormals = GetIndexedBufferView<Vector3>(IASemantic.Normal);

            for (int i = 0; i < trianglePos.Count; i += 3)
            {
                var p0 = trianglePos[i];
                var p1 = trianglePos[i + 1];
                var p2 = trianglePos[i + 2];                

                Vector3 normal = Triangle.ComputeFaceNormal(p0, p1, p2);

                triangleNormals[i] += normal;
                triangleNormals[i + 1] += normal;
                triangleNormals[i + 2] += normal;
            }

            var normals = GetVertexBufferView<Vector3>(IASemantic.Normal);

            for (int i = 0; i < normals.Count; i++)
            {
                normals[i] = Vector3.Normalize(normals[i]);
            }

            ReleaseViews();
        }

        public void ComputeTangents()
        {

            var trianglePos = GetIndexedBufferView<Vector3>(IASemantic.Position);
            var triangleTangent = GetIndexedBufferView<Vector3>(IASemantic.Tangent);
            var triangleTexC = GetIndexedBufferView<Vector2>(IASemantic.TextureCoordinate);

            for (int i = 0; i < trianglePos.Count; i += 3)
            {
                var p0 = trianglePos[i];
                var p1 = trianglePos[i + 1];
                var p2 = trianglePos[i + 2];

                Vector3 tangent = Triangle.ComputeFaceTangent(trianglePos[i], triangleTexC[i],
                                                            trianglePos[i + 1], triangleTexC[i + 1],
                                                             trianglePos[i + 2], triangleTexC[i + 2]);

                triangleTangent[i] += tangent;
                triangleTangent[i + 1] += tangent;
                triangleTangent[i + 2] += tangent;
            }

            var tangents = GetVertexBufferView<Vector3>(IASemantic.Tangent);

            for (int i = 0; i < tangents.Count; i++)
            {
                tangents[i] = Vector3.Normalize(tangents[i]);
            }

            ReleaseViews();

        }

        public void ComputeTextureCoords(CoordMappingType mapping)
        {           
            var positions = GetVertexBufferView<Vector3>(IASemantic.Position);
            var texCoords = GetVertexBufferView<Vector2>(IASemantic.TextureCoordinate);

                
                    Vector3 maxValues =box.GlobalTraslation + box.Extends;
                    Vector3 minValues = box.GlobalTraslation - box.Extends;
                    float height = maxValues.Y - minValues.Y;
                    Vector3 pos;
                    Vector2 tc = new Vector2();
                    var center = box.GlobalTraslation;
                    for (int i = 0; i < vertexCount; i++)
                    {
                        #region Map
                        switch (mapping)
                        {
                            case CoordMappingType.None:
                                tc = (Vector2)positions[i];                                
                                break;
                            case CoordMappingType.Spherical:
                                //translate the center of the object to the origin
                                pos = positions[i] - center;
                                //compute the sphical coordinate of the vertex
                                Vector3 spherical = Vector3.CartesianToSpherical(pos);
                                // u =theta / 2PI
                                tc.X = spherical.Y / Numerics.TwoPI;
                                // v = phi / PI
                                tc.Y = spherical.X / Numerics.PI;
                                break;
                            case CoordMappingType.Cylindrical:
                                //translate the center of the object to the origin
                                pos = positions[i] - center;
                                //compute the sphical coordinate of the vertex
                                Vector3 cylindrical = Vector3.CartesianToCylindrical(pos);
                                // u =theta / 2PI
                                tc.X = cylindrical.X / Numerics.TwoPI;
                                // v = (y - minY)/ (maxY - minY)
                                tc.Y = (cylindrical.Y - minValues.Y) / height;
                                break;
                        }
                        #endregion
                        texCoords[i] = tc;                        
                    }

            ReleaseViews();
         
        }     

        public MeshPart[] GetLayersByMaterial(int materialSlot)
        {
            if (materialSlot >= materialLayersLookup.Length) throw new IndexOutOfRangeException("meshMaterialindex");
            return materialLayersLookup[materialSlot];
        }

        public void DefragmentParts()
        {
            if (vb == null || layers.Length == 1)
                return;

            var vds = vb.Map(MapType.Read);
            var ids = ib.Map(MapType.Read);

            byte[] newIndices = new byte[ib.SizeInBytes];

            List<uint> vertList = new List<uint>(vertexCount);

            //guarda para el viejo indice el nuevo indice 
            Dictionary<uint, uint> indexHash = new Dictionary<uint, uint>(newIndices.Length);

            int componentCount = layers.Length;
            int k = 0;

            unsafe
            {
                byte* indices = (byte*)ids;
                fixed (byte* pNewIndices = newIndices)
                {
                    foreach (var c in layers)
                    {
                        int vertCount = 0;
                        uint oldindex = 0;
                        uint newIndex = 0;
                        uint oldStartIndex = (uint)c.StartIndex;

                        c.startIndex = k;
                        c.startVertex = vertList.Count;

                        //recorrer cada indice de la componente.
                        for (int i = 0; i < c.PrimitiveCount * 3; i++)
                        {
                            //antiguo indice
                            oldindex = is16BitIndices ? ((ushort*)indices)[oldStartIndex + i] : ((uint*)indices)[oldStartIndex + i];

                            if (!indexHash.ContainsKey(oldindex))
                            {
                                newIndex = (uint)vertList.Count;
                                indexHash.Add(oldindex, newIndex);
                                if (is16BitIndices)
                                    ((ushort*)pNewIndices)[k] = (ushort)newIndex;
                                else
                                    ((uint*)pNewIndices)[k] = newIndex;

                                vertList.Add(oldindex);
                                vertCount++;
                            }
                            else
                            {
                                newIndex = indexHash[oldindex];
                                if (is16BitIndices)
                                    ((ushort*)pNewIndices)[k] = (ushort)newIndex;
                                else
                                    ((uint*)pNewIndices)[k] = newIndex;
                            }
                            k++;
                        }

                        c.vertexCount = vertCount;
                        indexHash.Clear();
                    }
                }


                int size = VertexDescriptor.Size;
                byte[] vertexes = new byte[vertList.Count * size];
                fixed (byte* pVertexes = vertexes)
                {                   
                    for (int i = 0; i < vertList.Count; i++)
                    {
                        byte* pVertex = (byte*)((uint)vds + vertList[i] * (uint)size);
                        byte* pDestVertex = pVertexes + i * size;
                        for (int j = 0; j < size; j++)
                        {
                            *(pDestVertex + j) = *(pVertex + j);
                        }
                    }
                }

                vb.Unmap();
                ib.Unmap();                

                CreateVertexBuffer(vertexes);
                CreateIndexBuffer(newIndices, is16BitIndices ? IndexFormat.Index16 : IndexFormat.Index32);

            }
        }

        public void BlendLayers()
        {
            unsafe
            {
                Dictionary<int, List<MeshPart>> materialList = new Dictionary<int, List<MeshPart>>(materialLayersLookup.Length);
                for (int i = 0; i < materialLayersLookup.Length; i++)
                    materialList.Add(i, new List<MeshPart>(materialLayersLookup[i]));

                List<MeshPart> newLayers = new List<MeshPart>();

                var ds = ib.Map(MapType.Read);

                byte* pIndices = (byte*)ds;
                byte[] newIndices = new byte[ib.SizeInBytes];

                fixed (byte* pNewIndices = newIndices)
                {
                    int iIndex = 0;
                    Dictionary<uint, bool> vertexLookup = new Dictionary<uint, bool>();

                    foreach (var v in materialList)
                    {
                        vertexLookup.Clear();

                        MeshPart newLayer = new MeshPart();
                        if (newLayer == null)
                        {
                            ib.Unmap();
                            return;
                        }
                        newLayers.Add(newLayer);
                        newLayer.materialIndex = v.Key;
                        newLayer.startIndex = iIndex;
                        newLayer.startVertex = int.MaxValue;

                        foreach (var layer in v.Value)
                        {
                            newLayer.primitiveCount += layer.primitiveCount;
                            newLayer.startVertex = Math.Min(newLayer.startVertex, layer.startVertex);

                            int indicesCount = layer.primitiveCount * 3;
                            for (int i = layer.startIndex; i < layer.startIndex + indicesCount; i++)
                            {
                                uint vertexIndex;
                                if (is16BitIndices)
                                {
                                    vertexIndex = ((ushort*)pIndices)[i];
                                    ((ushort*)pNewIndices)[iIndex] = ((ushort*)pIndices)[i];
                                }
                                else
                                {
                                    vertexIndex = ((uint*)pIndices)[i];
                                    ((uint*)pNewIndices)[iIndex] = ((uint*)pIndices)[i];
                                }
                                iIndex++;
                                vertexLookup[vertexIndex] = true;
                            }
                        }
                        newLayer.vertexCount = vertexLookup.Keys.Count;
                        newLayer.indexCount = newLayer.primitiveCount * 3;
                        materialList[v.Key].Clear();
                        materialList[v.Key].Add(newLayer);
                    }

                    Layers = newLayers.ToArray();

                    ib.Unmap();
                    ib.Write(pNewIndices, 0, newIndices.Length);
                }
            }            
        }       

        #endregion

        protected override void OnDispose(bool d)
        {
            if (d)
            {
                vb.Dispose();
                ib.Dispose();
            }

            if (AssetManager.Instance != null)
                AssetManager.Instance.RemoveAssetReference(this);

            base.OnDispose(d);
        }

        public Asset CreateAsset()
        {
            return Asset.Create(this, name);
        }

        public void OnAssetDestroyed(AssetReference assetRef)
        {           
            Dispose();
        }

        public override string ToString()
        {
            return name??base.ToString();
        }

        public TriangleMesh CreateTriangleMesh()
        {
            TriangleMesh triangleMesh;    
        
            TriangleMeshDesc desc = new TriangleMeshDesc();
            desc.Name = name;
            desc.Flags |= Is16BitIndices ? MeshFlag.BIT_INDICES_16 : 0;            
            desc.Flags |= MeshFlag.HARDWARE_MESH;
            
            desc.NumTriangles = FaceCount;
            desc.NumVertices = VertexCount;
            desc.TriangleStrideBytes = 3 * (Is16BitIndices ? sizeof(short) : sizeof(int));
            desc.PointStrideBytes = VertexDescriptor.SizeOf(IASemantic.Position, 0);            
            int stride = desc.PointStrideBytes;

            var ibData = ib.Map(MapType.Read);
            var vbStream = vb.Map(MapType.Read);

            try
            {
                int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
                int size = vd.Size;

                byte[] positions = new byte[desc.NumVertices * stride];
                unsafe
                {
                    fixed (byte* desPter = positions)
                    {
                        byte* srcPter = (byte*)vbStream + posOffset;
                        byte* pter = desPter;                       

                        for (int i = 0; i < desc.NumVertices; i++ , srcPter+=size , pter +=stride)                        
                            *(Vector3*)pter = *(Vector3*)(srcPter);

                        desc.Points = (IntPtr)desPter;
                        desc.Triangles = ibData;

                        //Cooking.Create();

                        //Cooking.InitCooking();
                        //byte[] stream = Cooking.CookTriangleMesh(desc);

                        //Cooking.CloseCooking();

                        //desc.Dispose();

                        //triangleMesh = new TriangleMesh(stream) { Name = Name, UserData = this };

                        triangleMesh = PhysicManager.Sigleton.CreateTriangleMesh(desc);
                        triangleMesh.graphicMesh = this;                        
                    }
                }

            }
            finally
            {
                vb.Unmap();
                ib.Unmap();
            }
          
            return triangleMesh;
        }         

        public unsafe bool IsPlane(out Plane plane)
        {
            plane = new Plane();
            if (faceCount != 2) return false;

            var vbStream = vb.Map(MapType.Read);
            var ibStream = ib.Map(MapType.Read);

            Plane* planes = stackalloc Plane[2];

            try
            {
                int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
                int size = VertexDescriptor.Size;

                byte* vbPter = (byte*)vbStream + posOffset;
                byte* ibPter = (byte*)ibStream;               

                for (int iface = 0; iface < faceCount; iface++)
                {
                    Vector3 p0;
                    Vector3 p1;
                    Vector3 p2;

                    if (is16BitIndices)
                    {
                        p0 = *(Vector3*)(vbPter + size * ((short*)ibPter)[iface * 3]);
                        p1 = *(Vector3*)(vbPter + size * ((short*)ibPter)[iface * 3 + 1]);
                        p2 = *(Vector3*)(vbPter + size * ((short*)ibPter)[iface * 3 + 2]);
                    }
                    else
                    {
                        p0 = *(Vector3*)(vbPter + size * ((int*)ibPter)[iface * 3]);
                        p1 = *(Vector3*)(vbPter + size * ((int*)ibPter)[iface * 3 + 1]);
                        p2 = *(Vector3*)(vbPter + size * ((int*)ibPter)[iface * 3 + 2]);
                    }
                    planes[iface] = new Plane(p0, p1, p2);
                }
            }
            finally
            {
                vb.Unmap();
                ib.Unmap();
            }

            if (Plane.Equals(planes[0] ,planes[1]))
            {
                plane = planes[0];
                return true;
            }            
            return false;
        }

        public unsafe bool IsBox(out Vector3 dimensions, out Vector3 center)
        {
            dimensions = new Vector3();
            center = new Vector3();

            if (faceCount != 12) 
                return false;

            Plane* planes = stackalloc Plane[6];

            var vbStream = vb.Map( MapType.Read);
            var ibStream = ib.Map(MapType.Read);

            CreateBoxPlanes(planes , 6, vbStream, ibStream);

            /*
             *  A box must have 4 orthogonal an one paraller plane to every plane conforming the box
             */           
             
            for (int i = 0; i < 6; i++)
            {
                int ortoCount = 0;
                int parallerCount = 0;

                for (int j = 0; j < 6; j++)
                {
                    if (i == j) continue;

                    var dot = Vector3.Dot(planes[i].Normal, planes[j].Normal);
                    if (dot == 0 || dot.IsZero())
                        ortoCount++;
                    else if (dot == -1 || dot.IsEqual(-1))
                        parallerCount++;
                }

                if (parallerCount != 1 || ortoCount != 4)
                {
                    vb.Unmap();
                    ib.Unmap();
                    return false;
                }
            }

            FindDimensions(vbStream, out center, out dimensions);

            vb.Unmap();
            ib.Unmap();

            return true;
        }

        public unsafe bool IsSphere(out Vector3 center, out float radius)
        {
            center = new Vector3();
            radius = 0;
            if (vertexCount == 0 || faceCount <= 12) return false;

            var vbStream = vb.Map( MapType.Read );
          
            int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
            int size = VertexDescriptor.Size;          
            byte* vbPter = (byte*)vbStream + posOffset;

            if (sphere.Radius == 0)
            {
                sphere = new Sphere(vbPter, vertexCount, size);                
            }
            center = sphere.Center;
            radius = sphere.Radius;

            bool result = true;
            for (int i = 0; i < vertexCount; i++)
            {
                float distance = Vector3.Distance(*(Vector3*)(vbPter + size * i), center);
                if (distance != radius && !distance.IsEqual(radius))
                {                                       
                    result = false;
                    break;
                }
            }

            vb.Unmap();

            return result;
        }

        public unsafe bool IsCylindre(out float radius, out float height, out Vector3 center)
        {
            radius = 0;
            height = 0;
            center = new Vector3();

            if (vertexCount == 0 || faceCount <= 12) return false;

            var vbStream = vb.Map(MapType.Read);          

            int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
            int size = VertexDescriptor.Size;

            float distance;
            float maxY = float.MinValue;
            float minY = float.MaxValue;

            byte* vbPter = (byte*)vbStream + posOffset;
          
            Vector3 xzCenter;                       
            float xzRadius;
            int nb;
            ComputeXZRadius(vbStream, out xzCenter, out xzRadius, out center, out nb);
            xzCenter.Y = 0;

            bool result = true;
            for (int i = 0; i < vertexCount; i++)
            {
                Vector3 point = *(Vector3*)(vbPter + size * i);
                maxY = Math.Max(maxY, point.Y);
                minY = Math.Min(minY, point.Y);

                point.Y = 0;
                distance = Vector3.Distance(point, xzCenter);

                if (!distance.IsEqual(xzRadius) && !distance.IsZero())
                {
                    result = false;
                    break;
                }
            }

        
            radius = xzRadius;
            height = maxY - minY;

            vb.Unmap();
            return result;
        }

        public unsafe bool IsCapsule(out float radius, out float height, out Vector3 center)
        {
            radius = 0;
            height = 0;
            center = new Vector3();

            if (vertexCount == 0 || faceCount <= 12) 
                return false;

            var vbStream = vb.Map( MapType.Read );         

            int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
            int size = VertexDescriptor.Size;

            float distance;
            float maxY = float.MinValue;
            float minY = float.MaxValue;

            byte* vbPter = (byte*)vbStream + posOffset;

            Vector3 xzCenter;          
            int nbZeroDistance;
            float xyRadius = 0;
            ComputeXZRadius(vbStream, out xzCenter ,out xyRadius, out center, out nbZeroDistance);
            radius = xyRadius;

            int nbCylindrePoints = 0;           
          
            xzCenter.Y = 0;
            
            //compute height and radius of the cylindre
            for (int i = 0; i < vertexCount; i++)
            {
                Vector3 point = *(Vector3*)(vbPter + size * i);
                float y  = point.Y;
                point.Y = 0;
                distance = Vector3.Distance(point, xzCenter);
                if (distance.IsEqual(xyRadius))
                {
                    nbCylindrePoints++;
                    maxY = Math.Max(maxY, y);
                    minY = Math.Min(minY, y);
                }                
            }

            height = maxY - minY;
            float halfHeight = height * 0.5f;
            if(height.IsZero())
            {
                vb.Unmap();
                return false;
            }

            //verify top semispheres
            int nbSphereSpherePoints = 0;
            //compute height and radius of the cylindre
            for (int i = 0; i < vertexCount; i++)
            {
                Vector3 point = *(Vector3*)(vbPter + size * i);                
                if (point.Y > center.Y)
                    point.Y -= halfHeight;
                else
                    point.Y +=halfHeight;
                
                distance = Vector3.Distance(point, center);
                if (distance.IsEqual(xyRadius,0.0005f))
                {
                    nbSphereSpherePoints++;
                }
            }

            vb.Unmap();

            return (nbSphereSpherePoints + nbCylindrePoints) >= vertexCount;            
        }

        public ActorShapeDesc CreateShapeDescriptor()
        {
            Plane plane;
            Vector3 center;
            Vector3 dimensions;
            float height;
            float radius;

            if (IsPlane(out plane))
                return new PlaneShapeDesc() { Plane = plane , Name = Name };

            else if (IsBox(out dimensions, out center))
                return new BoxShapeDesc { Dimensions = dimensions, LocalPose = Matrix.Translate(center), Name = Name };

            else if (IsSphere(out center, out radius))
                return new SphereShapeDesc { Radius = radius, LocalPose = Matrix.Translate(center), Name = Name };

            else if (IsCylindre(out radius, out height, out center))
                return new WheelShapeDesc { Radius = radius, LocalPose = Matrix.Translate(center), Name = Name };            

            else if (IsCapsule(out radius, out height, out center))
                return new CapsuleShapeDesc { Radius = radius, Height = height, LocalPose = Matrix.Translate(center), Name = Name };
            
            return null;
        }

        public TriangleMeshShapeDesc CreateTriangleMeshDescriptor()
        {
            return new TriangleMeshShapeDesc { Mesh = CreateTriangleMesh(), Name = Name };
        }

        unsafe private void CreateBoxPlanes(Plane* planes, int planesCount, IntPtr vbStream, IntPtr ibStream)
        {
           
            int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
            int size = VertexDescriptor.Size;

            byte* vbPter = (byte*)vbStream + posOffset;
            byte* ibPter = (byte*)ibStream;
            int k = 0;

            for (int iface = 0; iface < faceCount; iface++)
            {
                Vector3 p0;
                Vector3 p1;
                Vector3 p2;

                if (is16BitIndices)
                {
                    p0 = *(Vector3*)(vbPter + size * ((short*)ibPter)[iface * 3]);
                    p1 = *(Vector3*)(vbPter + size * ((short*)ibPter)[iface * 3 + 1]);
                    p2 = *(Vector3*)(vbPter + size * ((short*)ibPter)[iface * 3 + 2]);
                }
                else
                {
                    p0 = *(Vector3*)(vbPter + size * ((int*)ibPter)[iface * 3]);
                    p1 = *(Vector3*)(vbPter + size * ((int*)ibPter)[iface * 3 + 1]);
                    p2 = *(Vector3*)(vbPter + size * ((int*)ibPter)[iface * 3 + 2]);
                }
                var facePlane = new Plane(p0, p1, p2);
                if (!Contains(planes, planesCount, facePlane))
                {
                    if (k >= planesCount) throw new IndexOutOfRangeException();

                    planes[k++] = facePlane;
                }
            }
        }

        private unsafe bool Contains(Plane* planes, int size, Plane p)
        {
            for (int i = 0; i < size; i++)            
                if (Plane.Equals(planes[i],p)) 
                    return true;

            return false;
        }       

        public unsafe void FindDimensions(IntPtr vbStream,out Vector3 center, out Vector3 dimensions)
        {
            int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
            int size = VertexDescriptor.Size;

            byte* vbPter = (byte*)vbStream + posOffset;

            center = new Vector3();
            Vector3 max = new Vector3(float.MinValue);
            Vector3 min = new Vector3(float.MaxValue);

            for (int i = 0; i < vertexCount; i++)
            {
                center += *(Vector3*)(vbPter + size * i);
                max = Vector3.Max(max, *(Vector3*)(vbPter + size * i));
                min = Vector3.Min(min, *(Vector3*)(vbPter + size * i));
            }

            dimensions = 0.5f * (max - min);
            center *= 1.0f / (float)vertexCount;
        }

        private unsafe void ComputeXZRadius(IntPtr vbStream, out Vector3 centerXZ ,out float radius ,out Vector3 center, out int nbZeroDistance)
        {
            centerXZ = new Vector3();
            radius = float.MinValue;
            nbZeroDistance = 0;
            int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
            int size = VertexDescriptor.Size;

            byte* vbPter = (byte*)vbStream + posOffset;            

            for (int i = 0; i < vertexCount; i++)            
                centerXZ += *(Vector3*)(vbPter + size * i);

            centerXZ *= 1f / (float)vertexCount;
            center = centerXZ;

            centerXZ.Y = 0;

            for (int i = 0; i < vertexCount; i++)
            {
                Vector3 point = *(Vector3*)(vbPter + size * i);
                point.Y = 0;

                float distance = Vector3.Distance(point, centerXZ);

                if (distance == 0 || distance.IsZero())
                    nbZeroDistance++;

                radius = Math.Max(radius, distance);
            }
        }
       
        private unsafe void ComputeXZRadius(IntPtr vbStream, out Vector3 centerXZ, out float radius)
        {
            Vector3 center;
            int nb;
            ComputeXZRadius(vbStream, out centerXZ, out radius, out center, out nb);
        }

        private unsafe void ComputeXZRadius(IntPtr vbStream, out Vector3 centerXZ, out float radius, out int nbZeroDistance)
        {
            Vector3 center;
            ComputeXZRadius(vbStream, out centerXZ, out radius, out center, out nbZeroDistance);
        }

        #region Standar Meshes

        public static Mesh CreateMesh<TVert, Tind>(TVert[] vertices, Tind[] indices)
            where TVert : struct
            where Tind :struct
        {
            Mesh mesh = new Mesh(VertexDescriptor.GetDescriptor<TVert>());            
            mesh.Layers = new MeshPart[] { new MeshPart(0, indices.Length / 3, 0, vertices.Length, 0) };
            mesh.MaterialSlotNames = new string[] { "Material0" };
            mesh.CreateVertexBuffer(vertices);
            mesh.CreateIndexBuffer<Tind>(indices);
            mesh.ComputeBoundingVolumenes();
            return mesh;
        }

        public static Mesh CreateBox(float dimX , float dimY, float dimZ)
        {
            BoxBuilder boxBuilder = new BoxBuilder(dimX, dimY , dimZ);
            return CreateMesh(boxBuilder.Vertices, boxBuilder.Indices);
        }

        public static Mesh CreateSphere(int stacks, int slices, float radius)
        {
            SphereBuilder sphere = new SphereBuilder(stacks, slices, radius);
            return CreateMesh(sphere.Vertices, sphere.Indices);
        }

        public static Mesh CreateCapsule(float height, float radius, int capStacks, int capSlices, int trunkStacks, int trunkSlices)
        {
            CapsuleBuilder capsule = new CapsuleBuilder(height, radius, capStacks, capSlices, trunkStacks, trunkSlices);
            Mesh mesh = new Mesh(VertexDescriptor.GetDescriptor<MeshVertex>());
            mesh.Layers = new MeshPart[] 
            { 
                new MeshPart(capsule.Top.StartIndex, capsule.Top.PrimitiveCount, capsule.Top.StartVertex, capsule.Top.VertexCount),
                new MeshPart(capsule.Cylindre.StartIndex, capsule.Cylindre.PrimitiveCount, capsule.Cylindre.StartVertex, capsule.Cylindre.VertexCount),
                new MeshPart(capsule.Bottom.StartIndex, capsule.Bottom.PrimitiveCount, capsule.Bottom.StartVertex, capsule.Bottom.VertexCount)
            };
            mesh.MaterialSlotNames = new string[] { "Material0" };
            mesh.CreateVertexBuffer(capsule.Vertices);
            mesh.CreateIndexBuffer(capsule.Indices);
            return mesh;
        }

        public static Mesh CreateCylindre(int stacks, int slices, float radius, float height, bool open)
        {
            CylindreBuilder cylindre = new CylindreBuilder(stacks, slices, radius, height, open);
            return CreateMesh(cylindre.Vertices, cylindre.Indices);
        }

        #endregion

        public void Draw(GraphicDevice device, Effect effect)
        {
            device.PrimitiveTopology = IAPrimitive.TriangleList;
            device.SetVertexBuffer(0, vb, 0);
            device.SetIndexBuffer(ib);

            foreach (var pass in effect.Passes())
            {
                effect.Apply(pass);
                foreach (var part in layers)
                {
                    device.DrawIndexed(part.IndexCount, part.StartIndex, 0);
                }                
            }
        }
    }

    class MeshVbSt : IStoreConverter
    {       
        public object GetStorage(IAssetProvider provider, object propValue, System.Reflection.PropertyInfo pi)
        {
            var vb = (GraphicBuffer)propValue;
            byte[] vbuffer = vb.ToArray<byte>();
            return vbuffer;
        }

        public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
        {
            byte[] vbuffer = (byte[])storeValue;
            var mesh = (Mesh)provider;
            mesh.CreateVertexBuffer(vbuffer);
        }
    }

    class MeshIbSt : IStoreConverter
    {    
        [Serializable]
        class Data
        {
            public byte[] buffer;
            public bool sixteenBits;
        }

        public object GetStorage(IAssetProvider provider, object propValue, System.Reflection.PropertyInfo pi)
        {
            var ibuffer = (GraphicBuffer)propValue;
            byte[] buffer = ibuffer.ToArray<byte>();
            return new Data { buffer = buffer, sixteenBits = ibuffer.Stride  == 2 };
        }

        public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
        {
            Data data = (Data)storeValue;
            var mesh = (Mesh)provider;
            mesh.CreateIndexBuffer(data.buffer, data.sixteenBits?IndexFormat.Index16: IndexFormat.Index32);
        }
    }

    [Serializable]
    class MeshAct : IProviderActivator
    {
        Type vertexType;

        public void Initialize(IAssetProvider provider)
        {
            var mesh = (Mesh)provider;
            vertexType = mesh.VertexDescriptor.VertexType;
        }

        public IAssetProvider CreateInstance()
        {
            return new Mesh(VertexDescriptor.GetDescriptor(vertexType));
        }
      
    }

}
