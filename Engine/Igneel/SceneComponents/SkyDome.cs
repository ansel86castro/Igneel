using System;
using System.Collections.Generic;
using Igneel.Assets;
using Igneel.Graphics;

namespace Igneel.SceneComponents
{
     [Asset("FRAME_OBJECT")]
    [ResourceActivator(typeof(SkyDome.Activator))]
    public class SkyDome:GraphicObject<SkyDome>,IInitializable
    {
        Texture2D _cubTexture;
        GraphicBuffer _vb;
        GraphicBuffer _ib;       
        VertexP[] _vertices;      
        ushort[] _indices;
        int _subdivitions;
        float _intensity = 1;             
        VertexDescriptor _vd;

        public SkyDome()
            : this(null, 2)
        {
            
        }

        public SkyDome(Texture2D skyTexture, int subdivitions)            
        {           
            this._subdivitions = subdivitions;
            this._cubTexture = skyTexture;
            Initialize();
        }
      
        [AssetMember]
        public float LuminanceIntensity { get { return _intensity; } set { _intensity = value; } }

        public GraphicBuffer VertexBuffer { get { return _vb; } }

        public GraphicBuffer IndexBuffer { get { return _ib; } }

        public int VertexCount { get { return _vertices.Length; } }

        public int IndexCount { get { return _indices.Length; } }

        [AssetMember(storeAs : StoreType.Reference)]
        public Texture2D Texture
        { 
            get { return _cubTexture; } 
            set { _cubTexture = value; } 
        }

        [AssetMember(storeAs: StoreType.Reference)]
        public int SubDivitions 
        { 
            get { return _subdivitions; } 
            set 
            {
                _subdivitions = value;
                Initialize();
            } 
        }

        public VertexDescriptor VertexDescriptor
        {
            get { return _vd; }
        }

        public void Initialize()
        {
            var positions = new List<Vector3>();
            var indices = new List<int>();
           
            _vd = VertexDescriptor.GetDescriptor<VertexP>(); // new VertexDescriptor(new VertexDeclaration(GGraphicDeviceFactory.Device, VertexP.Elements));

            _BuildGeoSphere(_subdivitions, 1, positions, indices);

            _vertices = new VertexP[positions.Count];
            for (int i = 0; i < positions.Count; i++)
                _vertices[i] = new VertexP(positions[i]);
            this._indices = new ushort[indices.Count];
            for (int i = 0; i < indices.Count; i++)
                this._indices[i] = (ushort)indices[i];

            if (_vb != null)
            {
                _vb.Dispose();
            }

            _vb = GraphicDeviceFactory.Device.CreateVertexBuffer(data: _vertices);            

            if (_ib != null)
            {
                _ib.Dispose();
            }

            _ib = GraphicDeviceFactory.Device.CreateIndexBuffer(data: this._indices);
        }

        //***************************************************************************************
        // Name: BuildGeoSphere
        // Desc: Function approximates a sphere by tesselating an icosahedron.
        //***************************************************************************************
        private void _BuildGeoSphere(int numSubdivisions, float radius, List<Vector3> vertices, List<int> indices)
        {
            // Put a cap on the number of subdivisions.
            numSubdivisions = Math.Min(numSubdivisions, 5);

            // Approximate a sphere by tesselating an icosahedron.

            const float x = 0.525731f;
            const float z = 0.850651f;

            Vector3[] pos = 
            {
                new Vector3(-x, 0.0f, z), new Vector3(x, 0.0f, z),  
                new Vector3(-x, 0.0f, -z), new Vector3(x, 0.0f, -z),    
                new Vector3(0.0f, z, x),   new Vector3(0.0f, z, -x), 
                new Vector3(0.0f, -z, x),  new Vector3(0.0f, -z, -x),    
                new Vector3(z, x, 0.0f),   new Vector3(-z, x, 0.0f), 
                new Vector3(z, -x, 0.0f),  new Vector3(-z, -x, 0.0f)
            };

            int[] k = 
            {
                1,4,0,  4,9,0,  4,5,9,  8,5,4,  1,8,4,    
                1,10,8, 10,3,8, 8,3,5,  3,2,5,  3,7,2,    
                3,10,7, 10,6,7, 6,11,7, 6,0,11, 6,1,0, 
                10,1,6, 11,0,9, 2,11,9, 5,2,9,  11,2,7 
            };

            vertices.AddRange(pos);
            indices.AddRange(k);

            for (int i = 0; i < numSubdivisions; ++i)
                _Subdivide(vertices, indices);

            // Project vertices onto sphere and scale.
            for (int i = 0; i < vertices.Count; ++i)
            {
                vertices[i].Normalize();
                vertices[i] *= radius;
            }
        }

        //***************************************************************************************
        // Name: Subdivide
        // Desc: Function subdivides every input triangle into four triangles of equal area.
        //***************************************************************************************
        private void _Subdivide(List<Vector3> vertices, List<int> indices)
        {

            List<Vector3> vin = new List<Vector3>(vertices);
            List<int> iin = new List<int>(indices);

            vertices.Clear();
            indices.Clear();

            //       v1
            //       *
            //      / \
            //     /   \
            //  m0*-----*m1
            //   / \   / \
            //  /   \ /   \
            // *-----*-----*
            // v0    m2     v2

            int numTris = iin.Count / 3;

            for (int i = 0; i < numTris; ++i)
            {
                Vector3 v0 = vin[iin[i * 3 + 0]];
                Vector3 v1 = vin[iin[i * 3 + 1]];
                Vector3 v2 = vin[iin[i * 3 + 2]];

                Vector3 m0 = 0.5f * (v0 + v1);
                Vector3 m1 = 0.5f * (v1 + v2);
                Vector3 m2 = 0.5f * (v0 + v2);

                vertices.Add(v0); // 0
                vertices.Add(v1); // 1
                vertices.Add(v2); // 2
                vertices.Add(m0); // 3
                vertices.Add(m1); // 4
                vertices.Add(m2); // 5

                int index = i * 6;
                indices.Add(index + 0);
                indices.Add(index + 3);
                indices.Add(index + 5);

                indices.Add(index + 3);
                indices.Add(index + 4);
                indices.Add(index + 5);

                indices.Add(index + 5);
                indices.Add(index + 4);
                indices.Add(index + 2);

                indices.Add(index + 3);
                indices.Add(index + 1);
                indices.Add(index + 4);
            }
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (_cubTexture != null)
                    _cubTexture.Dispose();
                if (_ib != null)
                    _ib.Dispose();
                if (_vb != null)
                    _vb.Dispose();             
            }
            base.OnDispose(disposing);
        }

        [Serializable]
        class Activator : IResourceActivator
        {

            public void OnAssetCreated(object provider, ResourceOperationContext context)
            {
               
            }

            public object OnCreateResource(ResourceOperationContext context)
            {
                return new SkyDome();
            }
        }          
    }
}
