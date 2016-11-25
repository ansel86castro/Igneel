using System;
using System.Collections.Generic;
using System.Linq;
using Igneel.Assets;
using Igneel.Assets.StorageConverters;
using Igneel.Graphics;
using Igneel.Physics;
using Igneel.Rendering;
using Igneel.SceneManagement;

namespace Igneel.Components
{
    public enum CoordMappingType
    {
        None, Spherical, Cylindrical
    }


    //[ResourceActivator(typeof(Mesh.Activator))]
    [Asset("VISUAL_MESH")]
    public class Mesh : Resource,IBoundable
    {            
        MeshPart[] _layers = new MeshPart[0];                    
        string[] _materialSlotsNames;         
        bool _is16BitIndices;
        int[] _adjacency;       
        int _vertexCount;
        int _faceCount;            
        GraphicBuffer _vb;
        GraphicBuffer _ib;        
        internal MeshPart[][] MaterialLayersLookup; //material per layer lookup      
        VertexDescriptor _vd;
        OrientedBox _box;
        Sphere _sphere;
        AABB _aabb;
      
        private IntPtr _vbStream, _ibStream;      

        public Mesh(VertexDescriptor vd)
        {
            if (vd == null) throw new ArgumentNullException("vertex descriptor");
            this._vd = vd;
            var noti = Service.Get<INotificationService>();
            if (noti != null)
                noti.OnObjectCreated(this);
        }              

        public int VertexCount { get { return _vertexCount; } }

        public int FaceCount { get { return _faceCount; } }

        public int LayerCount { get { return _layers.Length; } }

        public int MaterialSlots { get { return MaterialLayersLookup.Length; } }

        [AssetMember]
        public MeshPart[] Layers 
        {
            get { return _layers; }
            set
            {
                this._layers = value;

                for (int i = 0; i < _layers.Length; i++)
                    _layers[i].LayerId = i;

                _SetupMaterialsLayers();
            }
        }

       
        [AssetMember(typeof(MeshVbSt))]
        public GraphicBuffer VertexBuffer { get { return _vb; } }

       
        [AssetMember(typeof(MeshIbSt))]
        public GraphicBuffer IndexBuffer { get { return _ib; } }

        public bool Is16BitIndices { get { return _is16BitIndices; } }
       
        [AssetMember]
        public int[] Adjacency { get { return _adjacency; } set { _adjacency = value; } }
       
        public VertexDescriptor VertexDescriptor
        {
            get { return _vd; }
        }     

        public string[] MaterialSlotNames { get { return _materialSlotsNames; } set { _materialSlotsNames = value; } }

        public OrientedBox BoundingBox { get { return _box; } }

        public Sphere BoundingSphere { get { return _sphere; } }

        public AABB AxisAlignedBox { get { return _aabb; } }

        #region Public

        public void ComputeBoundingVolumenes()
        {
            unsafe
            {
                var positions = GetVertexBufferView<Vector3>(IASemantic.Position, 0);                
                this._box =new OrientedBox(positions);
                this._sphere = new Sphere((byte*)positions.BasePter, positions.Count, positions.Stride);                

                _aabb = new AABB(new Vector3(float.MaxValue, float.MaxValue,float.MaxValue), 
                                new Vector3(float.MinValue, float.MinValue,float.MinValue));

                for (int i = 0; i < positions.Count; i++)
                {
                    _aabb.Maximum = Vector3.Max(positions[i], _aabb.Maximum);
                    _aabb.Minimum = Vector3.Min(positions[i], _aabb.Minimum);
                }

                ReleaseVertexBufferViews();                 
            }
        }      
      
        public void CreateVertexBuffer<T>(T[] vertexes, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAccess = CpuAccessFlags.ReadWrite)
            where T : struct
        {
            if (_vb != null)
                _vb.Dispose();

            int size = ClrRuntime.Runtime.SizeOf<T>();
            _vertexCount = (vertexes.Length * size) / _vd.Size;

            _vb = GraphicDeviceFactory.Device.CreateVertexBuffer<T>(_vd.Size, usage, cpuAccess, data: vertexes);
        }
        public void CreateVertexBuffer(Array vertexes, int vertexCount, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAccess = CpuAccessFlags.ReadWrite)        
        {
            if (_vb != null)
                _vb.Dispose();

            this._vertexCount = vertexCount;
            int size = vertexCount * _vd.Size;
            _vb = GraphicDeviceFactory.Device.CreateVertexBuffer(size, _vd.Size, vertexes, usage, cpuAccess);
        }      

        public void CreateIndexBuffer<T>(T[] indices, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAccess = CpuAccessFlags.ReadWrite)
            where T : struct
        {
            _faceCount = indices.Length / 3;
            _is16BitIndices = ClrRuntime.Runtime.SizeOf<T>() == 2;
            if (_ib != null)
                _ib.Dispose();

            _ib = GraphicDeviceFactory.Device.CreateIndexBuffer<T>(usage, cpuAccess, indices);
        }
     
        public void CreateIndexBuffer(Array indices, IndexFormat format = IndexFormat.Index16, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAccess = CpuAccessFlags.ReadWrite)
        {          
            _is16BitIndices = format == IndexFormat.Index16;
            _faceCount = indices.Length / (_is16BitIndices ? 2 : 4) / 3;
            if (_ib != null)
                _ib.Dispose();

            _ib = GraphicDeviceFactory.Device.CreateIndexBuffer(indices.Length, indices, format, usage, cpuAccess);            
        }    

        public BufferView GetVertexViewStream(IASemantic semantic, int index = 0)
        {
            if(_vbStream == null)
                _vbStream = _vb.Map(MapType.ReadWrite);
            return new BufferView(_vbStream, _vd, semantic, index);
        }

        public void ReleaseVertexBufferViews()
        {
            if(_vbStream!=IntPtr.Zero)
                _vb.Unmap();
            _vbStream = IntPtr.Zero;
        }

        public void ReleaseIndexBufferViews()
        {
            if (_ibStream != IntPtr.Zero)
                _ib.Unmap();
            _ibStream = IntPtr.Zero;
        }

        public void ReleaseViews()
        {
            ReleaseVertexBufferViews();
            ReleaseIndexBufferViews();
        }

        public BufferView<T> GetVertexBufferView<T>(IASemantic usage, int index = 0) where T : struct
        {
            if (_vbStream == IntPtr.Zero)
                _vbStream = _vb.Map(MapType.ReadWrite);
            return new BufferView<T>(_vbStream, _vd, usage, index, _vertexCount);
        }

        public BufferView<T> GetVertexBufferView<T>(int offset) where T : struct
        {
            if (_vbStream == IntPtr.Zero)
                _vbStream = _vb.Map(MapType.ReadWrite);
            return new BufferView<T>(_vbStream + offset, ClrRuntime.Runtime.SizeOf<T>(), _vertexCount);
        }

        public IndexBufferView GetIndexBufferView()
        {
            if (_ibStream == IntPtr.Zero)
                _ibStream = _ib.Map(MapType.ReadWrite);
            return new IndexBufferView(_ibStream, _ib.Stride == 2, (int)_ib.SizeInBytes / _ib.Stride);
        }

        public IndexedBufferView<T> GetIndexedBufferView<T>(IASemantic semantic, int index = 0) where T : struct
        {
            if (_vbStream == IntPtr.Zero)
                _vbStream = _vb.Map(MapType.ReadWrite);
            if (_ibStream == IntPtr.Zero)
                _ibStream = _ib.Map(MapType.ReadWrite);

            return new IndexedBufferView<T>(_vbStream, _vd, semantic, index, _ibStream, _ib.Stride, (int)_ib.SizeInBytes / _ib.Stride);
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

                
                    Vector3 maxValues =_box.GlobalTraslation + _box.Extends;
                    Vector3 minValues = _box.GlobalTraslation - _box.Extends;
                    float height = maxValues.Y - minValues.Y;
                    Vector3 pos;
                    Vector2 tc = new Vector2();
                    var center = _box.GlobalTraslation;
                    for (int i = 0; i < _vertexCount; i++)
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
            if (materialSlot >= MaterialLayersLookup.Length) throw new IndexOutOfRangeException("meshMaterialindex");
            return MaterialLayersLookup[materialSlot];
        }

        public void DefragmentParts()
        {
            if (_vb == null || _layers.Length == 1)
                return;

            var vds = _vb.Map(MapType.Read);
            var ids = _ib.Map(MapType.Read);

            byte[] newIndices = new byte[_ib.SizeInBytes];

            List<uint> vertList = new List<uint>(_vertexCount);

            //guarda para el viejo indice el nuevo indice 
            Dictionary<uint, uint> indexHash = new Dictionary<uint, uint>(newIndices.Length);

            int componentCount = _layers.Length;
            int k = 0;

            unsafe
            {
                byte* indices = (byte*)ids;
                fixed (byte* pNewIndices = newIndices)
                {
                    foreach (var c in _layers)
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
                            oldindex = _is16BitIndices ? ((ushort*)indices)[oldStartIndex + i] : ((uint*)indices)[oldStartIndex + i];

                            if (!indexHash.ContainsKey(oldindex))
                            {
                                newIndex = (uint)vertList.Count;
                                indexHash.Add(oldindex, newIndex);
                                if (_is16BitIndices)
                                    ((ushort*)pNewIndices)[k] = (ushort)newIndex;
                                else
                                    ((uint*)pNewIndices)[k] = newIndex;

                                vertList.Add(oldindex);
                                vertCount++;
                            }
                            else
                            {
                                newIndex = indexHash[oldindex];
                                if (_is16BitIndices)
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

                _vb.Unmap();
                _ib.Unmap();                

                CreateVertexBuffer(vertexes);
                CreateIndexBuffer(newIndices, _is16BitIndices ? IndexFormat.Index16 : IndexFormat.Index32);

            }
        }

        public void BlendLayers()
        {
            unsafe
            {
                Dictionary<int, List<MeshPart>> materialList = new Dictionary<int, List<MeshPart>>(MaterialLayersLookup.Length);
                for (int i = 0; i < MaterialLayersLookup.Length; i++)
                    materialList.Add(i, new List<MeshPart>(MaterialLayersLookup[i]));

                List<MeshPart> newLayers = new List<MeshPart>();

                var ds = _ib.Map(MapType.Read);

                byte* pIndices = (byte*)ds;
                byte[] newIndices = new byte[_ib.SizeInBytes];

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
                            _ib.Unmap();
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
                                if (_is16BitIndices)
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

                    _ib.Unmap();
                    _ib.Write(pNewIndices, 0, newIndices.Length);
                }
            }            
        }       

        #endregion

        /// <summary>
        /// arrange the layers by materials 
        /// </summary>
        private void _SetupMaterialsLayers()
        {
            var materialsIndex = _layers.Select(x => x.materialIndex).Distinct().ToArray();

            //store for each material index the list of layers
            List<MeshPart>[] materialLayers = new List<MeshPart>[materialsIndex.Length];
            for (int i = 0; i < materialsIndex.Length; i++)
            {
                materialLayers[i] = new List<MeshPart>();
                foreach (var layer in _layers)
                {
                    if (layer.materialIndex == i)
                        materialLayers[i].Add(layer);
                }
            }
            MaterialLayersLookup = new MeshPart[materialsIndex.Length][];
            for (int i = 0; i < MaterialLayersLookup.Length; i++)
            {
                MaterialLayersLookup[i] = materialLayers[i].ToArray();
            }
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                _vb.Dispose();
                _ib.Dispose();
            }
        }            

        public TriangleMesh CreateTriangleMesh()
        {
            TriangleMesh triangleMesh;    
        
            TriangleMeshDesc desc = new TriangleMeshDesc();
            desc.Name = Name;
            desc.Flags |= Is16BitIndices ? MeshFlag.BIT_INDICES_16 : 0;            
            desc.Flags |= MeshFlag.HARDWARE_MESH;
            
            desc.NumTriangles = FaceCount;
            desc.NumVertices = VertexCount;
            desc.TriangleStrideBytes = 3 * (Is16BitIndices ? sizeof(short) : sizeof(int));
            desc.PointStrideBytes = VertexDescriptor.SizeOf(IASemantic.Position, 0);            
            int stride = desc.PointStrideBytes;

            var ibData = _ib.Map(MapType.Read);
            var vbStream = _vb.Map(MapType.Read);

            try
            {
                int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
                int size = _vd.Size;

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
                        triangleMesh.GraphicMesh = this;                        
                    }
                }

            }
            finally
            {
                _vb.Unmap();
                _ib.Unmap();
            }
          
            return triangleMesh;
        }         

        public unsafe bool IsPlane(out Plane plane)
        {
            plane = new Plane();
            if (_faceCount != 2) return false;

            var vbStream = _vb.Map(MapType.Read);
            var ibStream = _ib.Map(MapType.Read);

            Plane* planes = stackalloc Plane[2];

            try
            {
                int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
                int size = VertexDescriptor.Size;

                byte* vbPter = (byte*)vbStream + posOffset;
                byte* ibPter = (byte*)ibStream;               

                for (int iface = 0; iface < _faceCount; iface++)
                {
                    Vector3 p0;
                    Vector3 p1;
                    Vector3 p2;

                    if (_is16BitIndices)
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
                _vb.Unmap();
                _ib.Unmap();
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

            if (_faceCount != 12) 
                return false;

            Plane* planes = stackalloc Plane[6];

            var vbStream = _vb.Map( MapType.Read);
            var ibStream = _ib.Map(MapType.Read);

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
                    _vb.Unmap();
                    _ib.Unmap();
                    return false;
                }
            }

            FindDimensions(vbStream, out center, out dimensions);

            _vb.Unmap();
            _ib.Unmap();

            return true;
        }

        public unsafe bool IsSphere(out Vector3 center, out float radius)
        {
            center = new Vector3();
            radius = 0;
            if (_vertexCount == 0 || _faceCount <= 12) return false;

            var vbStream = _vb.Map( MapType.Read );
          
            int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
            int size = VertexDescriptor.Size;          
            byte* vbPter = (byte*)vbStream + posOffset;

            if (_sphere.Radius == 0)
            {
                _sphere = new Sphere(vbPter, _vertexCount, size);                
            }
            center = _sphere.Center;
            radius = _sphere.Radius;

            bool result = true;
            for (int i = 0; i < _vertexCount; i++)
            {
                float distance = Vector3.Distance(*(Vector3*)(vbPter + size * i), center);
                if (distance != radius && !distance.IsEqual(radius))
                {                                       
                    result = false;
                    break;
                }
            }

            _vb.Unmap();

            return result;
        }

        public unsafe bool IsCylindre(out float radius, out float height, out Vector3 center)
        {
            radius = 0;
            height = 0;
            center = new Vector3();

            if (_vertexCount == 0 || _faceCount <= 12) return false;

            var vbStream = _vb.Map(MapType.Read);          

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
            for (int i = 0; i < _vertexCount; i++)
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

            _vb.Unmap();
            return result;
        }

        public unsafe bool IsCapsule(out float radius, out float height, out Vector3 center)
        {
            radius = 0;
            height = 0;
            center = new Vector3();

            if (_vertexCount == 0 || _faceCount <= 12) 
                return false;

            var vbStream = _vb.Map( MapType.Read );         

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
            for (int i = 0; i < _vertexCount; i++)
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
                _vb.Unmap();
                return false;
            }

            //verify top semispheres
            int nbSphereSpherePoints = 0;
            //compute height and radius of the cylindre
            for (int i = 0; i < _vertexCount; i++)
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

            _vb.Unmap();

            return (nbSphereSpherePoints + nbCylindrePoints) >= _vertexCount;            
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

            for (int iface = 0; iface < _faceCount; iface++)
            {
                Vector3 p0;
                Vector3 p1;
                Vector3 p2;

                if (_is16BitIndices)
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

            for (int i = 0; i < _vertexCount; i++)
            {
                center += *(Vector3*)(vbPter + size * i);
                max = Vector3.Max(max, *(Vector3*)(vbPter + size * i));
                min = Vector3.Min(min, *(Vector3*)(vbPter + size * i));
            }

            dimensions = 0.5f * (max - min);
            center *= 1.0f / (float)_vertexCount;
        }

        private unsafe void ComputeXZRadius(IntPtr vbStream, out Vector3 centerXz ,out float radius ,out Vector3 center, out int nbZeroDistance)
        {
            centerXz = new Vector3();
            radius = float.MinValue;
            nbZeroDistance = 0;
            int posOffset = VertexDescriptor.OffsetOf(IASemantic.Position, 0);
            int size = VertexDescriptor.Size;

            byte* vbPter = (byte*)vbStream + posOffset;            

            for (int i = 0; i < _vertexCount; i++)            
                centerXz += *(Vector3*)(vbPter + size * i);

            centerXz *= 1f / (float)_vertexCount;
            center = centerXz;

            centerXz.Y = 0;

            for (int i = 0; i < _vertexCount; i++)
            {
                Vector3 point = *(Vector3*)(vbPter + size * i);
                point.Y = 0;

                float distance = Vector3.Distance(point, centerXz);

                if (distance == 0 || distance.IsZero())
                    nbZeroDistance++;

                radius = Math.Max(radius, distance);
            }
        }
       
        private unsafe void ComputeXZRadius(IntPtr vbStream, out Vector3 centerXz, out float radius)
        {
            Vector3 center;
            int nb;
            ComputeXZRadius(vbStream, out centerXz, out radius, out center, out nb);
        }

        private unsafe void ComputeXZRadius(IntPtr vbStream, out Vector3 centerXz, out float radius, out int nbZeroDistance)
        {
            Vector3 center;
            ComputeXZRadius(vbStream, out centerXz, out radius, out center, out nbZeroDistance);
        }

        #region Standar Meshes

        public static Mesh CreateMesh<TVert, TInd>(TVert[] vertices, TInd[] indices)
            where TVert : struct
            where TInd :struct
        {
            Mesh mesh = new Mesh(VertexDescriptor.GetDescriptor<TVert>());            
            mesh.Layers = new MeshPart[] { new MeshPart(0, indices.Length / 3, 0, vertices.Length, 0) };
            mesh.MaterialSlotNames = new string[] { "Material0" };
            mesh.CreateVertexBuffer(vertices);
            mesh.CreateIndexBuffer<TInd>(indices);
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
            device.SetVertexBuffer(0, _vb, 0);
            device.SetIndexBuffer(_ib);

            foreach (var pass in effect.Passes())
            {
                effect.Apply(pass);
                foreach (var part in _layers)
                {
                    device.DrawIndexed(part.IndexCount, part.StartIndex, 0);
                }                
            }
        }
    }

    class MeshVbSt : IStoreConverter
    {
        public object GetStorage(object provider, object propValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
        {
            var vb = (GraphicBuffer)propValue;
            byte[] vbuffer = vb.ToArray<byte>();
            return vbuffer;
        }

        public void SetStorage(object provider, object storeValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
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
            public byte[] Buffer;
            public bool SixteenBits;
        }

        public object GetStorage(object provider, object propValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
        {
            var ibuffer = (GraphicBuffer)propValue;
            byte[] buffer = ibuffer.ToArray<byte>();
            return new Data { Buffer = buffer, SixteenBits = ibuffer.Stride  == 2 };
        }

        public void SetStorage(object provider, object storeValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
        {
            Data data = (Data)storeValue;
            var mesh = (Mesh)provider;
            mesh.CreateIndexBuffer(data.Buffer, data.SixteenBits?IndexFormat.Index16: IndexFormat.Index32);
        }

        [Serializable]
        public class Activator : IResourceActivator
        {
            Type _vertexType;

            public void OnAssetCreated(object provider, ResourceOperationContext context)
            {
                var mesh = (Mesh)provider;
                _vertexType = mesh.VertexDescriptor.VertexType;
            }

            public object OnCreateResource(ResourceOperationContext context)
            {
                return new Mesh(VertexDescriptor.GetDescriptor(_vertexType));
            }
        }

    }
   
}
