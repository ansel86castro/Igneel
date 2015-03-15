using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Igneel.Graphics;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
 

using Igneel.Assets;
using System.ComponentModel;

namespace Igneel.Scenering
{
     
    [ProviderActivator(typeof(SkyDome.Activator))]
    public class SkyDome:GraphicObject<SkyDome>,IInitializable
    {
        Texture2D cubTexture;             
        GraphicBuffer vb;
        GraphicBuffer ib;       
        VertexP[] vertices;      
        ushort[] indices;
        int subdivitions;
        float intensity = 1;             
        VertexDescriptor vd;

        public SkyDome()
            : this(null, 2)
        {
            
        }

        public SkyDome(Texture2D skyTexture, int subdivitions)            
        {           
            this.subdivitions = subdivitions;
            this.cubTexture = skyTexture;
            Initialize();
        }
      
        [AssetMember]
        public float LuminanceIntensity { get { return intensity; } set { intensity = value; } }

        public GraphicBuffer VertexBuffer { get { return vb; } }

        public GraphicBuffer IndexBuffer { get { return ib; } }

        public int VertexCount { get { return vertices.Length; } }

        public int IndexCount { get { return indices.Length; } }

        [AssetMember(storeAs : StoreType.Reference)]
        public Texture2D Texture
        { 
            get { return cubTexture; } 
            set { cubTexture = value; } 
        }

        [AssetMember(storeAs: StoreType.Reference)]
        public int SubDivitions 
        { 
            get { return subdivitions; } 
            set 
            {
                subdivitions = value;
                Initialize();
            } 
        }

        public VertexDescriptor VertexDescriptor
        {
            get { return vd; }
        }

        public void Initialize()
        {
            var positions = new List<Vector3>();
            var indices = new List<int>();
           
            vd = VertexDescriptor.GetDescriptor<VertexP>(); // new VertexDescriptor(new VertexDeclaration(GGraphicDeviceFactory.Device, VertexP.Elements));

            _BuildGeoSphere(subdivitions, 1, positions, indices);

            vertices = new VertexP[positions.Count];
            for (int i = 0; i < positions.Count; i++)
                vertices[i] = new VertexP(positions[i]);
            this.indices = new ushort[indices.Count];
            for (int i = 0; i < indices.Count; i++)
                this.indices[i] = (ushort)indices[i];

            if (vb != null)
            {
                vb.Dispose();
            }

            vb = GraphicDeviceFactory.Device.CreateVertexBuffer(data: vertices);            

            if (ib != null)
            {
                ib.Dispose();
            }

            ib = GraphicDeviceFactory.Device.CreateIndexBuffer(data: this.indices);
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

            const float X = 0.525731f;
            const float Z = 0.850651f;

            Vector3[] pos = 
            {
                new Vector3(-X, 0.0f, Z), new Vector3(X, 0.0f, Z),  
                new Vector3(-X, 0.0f, -Z), new Vector3(X, 0.0f, -Z),    
                new Vector3(0.0f, Z, X),   new Vector3(0.0f, Z, -X), 
                new Vector3(0.0f, -Z, X),  new Vector3(0.0f, -Z, -X),    
                new Vector3(Z, X, 0.0f),   new Vector3(-Z, X, 0.0f), 
                new Vector3(Z, -X, 0.0f),  new Vector3(-Z, -X, 0.0f)
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
                if (cubTexture != null)
                    cubTexture.Dispose();
                if (ib != null)
                    ib.Dispose();
                if (vb != null)
                    vb.Dispose();             
            }
            base.OnDispose(disposing);
        }

        [Serializable]
        class Activator : IProviderActivator
        {
           
            public void Initialize(IAssetProvider provider)
            {
                
            }

            public IAssetProvider CreateInstance()
            {
                return new SkyDome();
            }
            
        }          
    }
}
