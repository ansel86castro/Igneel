using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel.Graphics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using System.ComponentModel;
using Igneel.Assets;


using System.Drawing;

namespace Igneel.Scenering
{
  
    public class HeightField : GraphicObject<HeightField>
    {
        GraphicBuffer vbPosUV;
        GraphicBuffer ibStrips;       
        VertexDescriptor vd = VertexDescriptor.GetDescriptor<HeightFieldVertex>();
        HeightFieldSection[,] sections;
        TerrainMaterial[] materials;
        TerrainSectionTester tester = new TerrainSectionTester();        
        int sectionVertexCount;
        int sectionPrimitives;       

        /// <summary>
        /// local space quadtree
        /// </summary>
        QuadTree<HeightFieldSection> quadTree;
        List<HeightFieldSection> visibleSections;
        Matrix transform;
        private Texture2D heightMap;
        float[] heights;

        public HeightField(Texture2D heightMap, int nbSectionsX, int nbSectionsY)           
        {
            this.heightMap = heightMap;
            AABB extends = new AABB(new Vector3(0, 0, 0), new Vector3(1, 1, 1));       
            var sd = heightMap.Description;

            int xVerts = sd.Width / nbSectionsX; // horizontal number of vertices 
            int yVerts = sd.Height / nbSectionsY; // vertical number of vertices
            ushort stride = (ushort)xVerts; // number of vertices in one row

           ibStrips = _CreateTriangleStrips(xVerts, yVerts, stride);
           visibleSections = new List<HeightFieldSection>(nbSectionsX * nbSectionsY);

           //quadTree = new QuadTree<HeightFieldSection>(new RectangleF(extends.Minimum.X, extends.Maximum.Z,
           //                                                           extends.Maximum.X - extends.Minimum.X,
           //                                                           extends.Maximum.Z - extends.Minimum.Z),
           //                                            (int)Math.Log(nbSectionsX * nbSectionsY, 4) - 1, tester);
                                                
           
           _CreateGeometry(heightMap, nbSectionsX, nbSectionsY);

           sectionVertexCount = xVerts  * yVerts;
          
           materials = new TerrainMaterial[] { new TerrainMaterial() { Name = "Dafault" } };
           //BoundingSphere =quadTree.BoundSphere;            
        }

        public int SectionVertexCount
        {
            get { return sectionVertexCount; }
        }

        public int SectionPrimitives
        {
            get { return sectionPrimitives; }         
        }

        public HeightFieldSection[,] Sections { get { return sections; } }

        public GraphicBuffer PositionUVBuffer { get { return vbPosUV; } }

        public GraphicBuffer StripIndices { get { return ibStrips; } }

        public VertexDescriptor VertDescriptor { get { return vd; } }

        public TerrainMaterial[] Materials
        {
            get { return materials; }
            set { materials = value; }
        }

        public List<HeightFieldSection> VisibleSections { get { return visibleSections; } }

        public Texture2D HeightMap { get { return heightMap; } }

        public void UpdateVisibleSections(Camera camera)
        {
            var points = camera.ViewFrustum.Corners;
            for (int i = 0; i < points.Length; i++)
            {
                tester.Points[i] = Vector3.Transform(points[i], transform); 
            }

           Frustum.CreatePlanes(tester.LocalFrustum, tester.Points);

            quadTree.CullItems(camera, visibleSections);
        }

        public override int  GetGraphicObjects(SceneNode node , ICollection<DrawingEntry> collection)
        {
            int count = base.GetGraphicObjects(node, collection);
            if (count > 0)
            {
                UpdateVisibleSections(SceneManager.Scene.ActiveCamera);
            }
            return count;
        }

        public override void OnPoseUpdated(SceneNode node)
        {            
            //_UpdateCulling(node.GlobalPose);

            transform = Matrix.Invert(node.GlobalPose);
        }

        public override void OnNodeAttach(SceneNode node)
        {          
            node.LocalSphere = BoundingSphere;
            node.BoundingSphere = BoundingSphere;

            base.OnNodeAttach(node);
        }

        private void _UpdateCulling(Matrix pose)
        {
            foreach (var item in sections)
            {
                item.boundSphere = item.boundSphere.GetTranformed(pose);
                //item.ComputeBoundings(_UpdateBounds(pose, item.BoundRect));
            }            

           // quadTree.Reshape(_UpdateBounds(pose, quadTree.BoundRect));
        }

        private static RectangleF _UpdateBounds(Matrix pose, RectangleF bounds)
        {
            var rightBottomv2 = bounds.RightBottom;

            Vector3 lefTop = Vector3.Transform(new Vector3(bounds.X, 0, bounds.Y), pose);
            Vector3 rightBottom = Vector3.Transform(new Vector3(rightBottomv2.X, 0, rightBottomv2.Y), pose);

            bounds = new RectangleF(lefTop.X, lefTop.Z, rightBottom.X - lefTop.X, lefTop.Z - rightBottom.Z);
            return bounds;
        }

        private GraphicBuffer _CreateTriangleStrips(int xVerts, int yVerts, ushort stride)
        {
            int totalStrips = yVerts - 1;
            int total_indexes_per_strip = xVerts * 2;
            int total_indices = total_indexes_per_strip * totalStrips + totalStrips * 2 - 2;

            sectionPrimitives = 2 + total_indices - 4;

            ushort[] indices = new ushort[total_indices];
            unsafe
            {
                fixed (ushort* pIndex = indices)
                {
                    ushort* index = pIndex;
                    ushort startVertex = 0;
                    for (int j = 0; j < totalStrips; ++j)
                    {
                        ushort vert = startVertex;

                        //creat a strip for this row
                        for (int k = 0; k < xVerts; k++)
                        {
                            *(index++) = vert;
                            *(index++) = (ushort)(vert + stride);
                            vert++;
                        }

                        startVertex += stride;

                        if (j + 1 < totalStrips)
                        {
                            //add a degenerate triangle to attach to the next row                           
                            *(index++) = (ushort)((vert - 1) + stride);
                            *(index++) = (ushort)(startVertex);
                        }
                    }
                }
            }

            GraphicBuffer ib = GraphicDeviceFactory.Device.CreateIndexBuffer(data:indices);            
            return ib;
        }

        private unsafe void _CreateGeometry(Texture2D heightMap, int nbSectionsX, int nbSectionsY)
        {
            var sd = heightMap.Description;
            float dz = 1.0f / (float)sd.Height;
            float dx = 1.0f / (float)sd.Width;
           
            var width = sd.Width;
            var height = sd.Height;

            var positions = stackalloc Vector3[4];
            var normals = new Vector3[width * height];
            var blendCoords = new Vector2[width * height];
            heights = new float[width * height]; ;

            int stride = 0;
            switch (sd.Format)
            {                
                case Format.B8G8R8X8_UNORM:
                case Format.B8G8R8A8_UNORM:
                case Format.R8G8B8A8_UNORM: stride = 4; break;
                case Format.R16G16B16A16_UNORM: stride = 8; break;
                case Format.R16G16B16A16_FLOAT: stride = 8; break;
                case Format.R32G32B32A32_TYPELESS: stride = 16; break;
                case Format.A8_UNORM: stride = 1; break;                
            }
            if (stride == 0)
                throw new InvalidOperationException("Invalid Texture Format");

            var dataRec = heightMap.Map(0, MapType.Read);
            byte* pixels = (byte*)dataRec.DataPointer;
            var pitch = dataRec.RowPitch;

            #region  create shared tables

            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    //Quad Corners
                    int p0 = i * width + j;
                    int p1 = i * width + j + 1;
                    int p2 = (i + 1) * width + j;
                    int p3 = (i + 1) * width + j + 1;

                    heights[p0] =  (float)pixels[i * pitch + j * stride] / 255.0f;
                    blendCoords[p0] = new Vector2((float)j / (float)(width - 1), (float)i / (float)(height - 1));
                    positions[0] = new Vector3(j * dx, heights[p0], i * dz);

                    heights[p1] = (float)pixels[i * pitch + (j + 1) * stride] / 255.0f;
                    blendCoords[p1] = new Vector2((float)(j + 1) / (float)(width - 1), (float)i / (float)(height - 1));
                    positions[1] = new Vector3((j + 1) * dx, heights[p1], i * dz);

                    heights[p2] = (float)pixels[(i + 1) * pitch + j * stride] / 255.0f;
                    blendCoords[p2] = new Vector2((float)j / (float)(width - 1), (float)(i + 1) / (float)(height - 1));
                    positions[2] = new Vector3(j * dx, heights[p2], (i + 1) * dz);


                    heights[p3] = (float)pixels[(i + 1) * pitch + (j + 1) * stride] / 255.0f;
                    blendCoords[p3] = new Vector2((float)(j + 1) / (float)(width - 1), (float)(i + 1) / (float)(height - 1));
                    positions[3] = new Vector3((j + 1) * dx, heights[p3], (i + 1) * dz);

                    //Vector3 normal1 = Geometry.ComputeFaceNormal(positions[0], positions[1], positions[2]);
                    //Vector3 normal2 = Geometry.ComputeFaceNormal(positions[2], positions[1], positions[3]);
                    Vector3 normal1 = Triangle.ComputeFaceNormal(positions[2], positions[1], positions[0]);
                    Vector3 normal2 = Triangle.ComputeFaceNormal(positions[3], positions[1], positions[2]);
                    Vector3 avgNormal = Vector3.Normalize((normal1 + normal2) * 0.5f);

                    normals[p0] += normal1;
                    normals[p1] += avgNormal;
                    normals[p2] += normal2;
                    normals[p3] += avgNormal;
                }
            }

            heightMap.UnMap(0);            

            for (int i = 0; i < normals.Length; i++)
                normals[i].Normalize();
            #endregion

            #region create sections

            int xVertPerSections = width / nbSectionsX;
            int yVertPerSections = height / nbSectionsY;
            sections = new HeightFieldSection[nbSectionsX, nbSectionsY];

             stride = 24;
            int dataSize = xVertPerSections * yVertPerSections * stride;
            byte[] vbData = new byte[dataSize];

            fixed (byte* sectionVbData = vbData)
            {

                int heightOffset = 0;
                int normalOffset = 4;
                int blendTexCoordOffset = 16;

                for (int i = 0; i < nbSectionsY; i++)
                {
                    int y = i * (yVertPerSections - 1);
                    for (int j = 0; j < nbSectionsX; j++)
                    {
                        int x = j * (xVertPerSections - 1);

                        HeightFieldSection section = new HeightFieldSection();
                        section.Offset.X = x * dx;  // nbVertices in x-direction times dx(world displacement)
                        section.Offset.Y = y * dz; // nbVertices in z-direction times dz(world displacement)

                        float halfX = xVertPerSections * dx * 0.5f;
                        float halfY = yVertPerSections * dz * 0.5f;

                        RectangleF sectionBounds;
                        sectionBounds.Width = (xVertPerSections - 1) * dx;
                        sectionBounds.Height = (yVertPerSections - 1) * dz;
                        sectionBounds.X = section.Offset.X;
                        sectionBounds.Y = section.Offset.Y + sectionBounds.Height;

                        section.ComputeBoundings(sectionBounds);

                        #region Add Section Data

                        for (int yOffset = 0; yOffset < yVertPerSections; yOffset++)
                        {
                            for (int xOffset = 0; xOffset < xVertPerSections; xOffset++)
                            {
                                int offset = (yOffset * xVertPerSections + xOffset) * stride;
                                int tableIndex = (y + yOffset) * width + x + xOffset;

                                *(float*)(sectionVbData + offset + heightOffset) = heights[tableIndex];
                                *(Vector3*)(sectionVbData + offset + normalOffset) = normals[tableIndex];
                                *(Vector2*)(sectionVbData + offset + blendTexCoordOffset) = blendCoords[tableIndex];
                            }
                        }

                        section.NormalHeightVb = GraphicDeviceFactory.Device.CreateVertexBuffer(dataSize, 24, data: vbData);

                        #endregion

                        sections[j, i] = section;
                        quadTree.Add(section);
                    }
                }

                #region Create Shared Data

                stride = 16;
                dataSize = xVertPerSections * yVertPerSections * stride;

                for (int yOffset = 0; yOffset < yVertPerSections - 1; yOffset++)
                {
                    for (int xOffset = 0; xOffset < xVertPerSections - 1; xOffset++)
                    {
                        int offset0 = (yOffset * xVertPerSections + xOffset) * stride;
                        int offset1 = (yOffset * xVertPerSections + xOffset + 1) * stride;
                        int offset2 = ((yOffset + 1) * xVertPerSections + xOffset) * stride;
                        int offset3 = ((yOffset + 1) * xVertPerSections + xOffset + 1) * stride;

                        *(Vector2*)(sectionVbData + offset0 + 0) = new Vector2(xOffset * dx, yOffset * dz);
                        *(Vector2*)(sectionVbData + offset0 + 8) = new Vector2(xOffset, yOffset);

                        *(Vector2*)(sectionVbData + offset1 + 0) = new Vector2((xOffset + 1) * dx, yOffset * dz);
                        *(Vector2*)(sectionVbData + offset1 + 8) = new Vector2(xOffset + 1, yOffset);

                        *(Vector2*)(sectionVbData + offset2 + 0) = new Vector2(xOffset * dx, (yOffset + 1) * dz);
                        *(Vector2*)(sectionVbData + offset2 + 8) = new Vector2(xOffset, yOffset + 1);

                        *(Vector2*)(sectionVbData + offset3 + 0) = new Vector2((xOffset + 1) * dx, (yOffset + 1) * dz);
                        *(Vector2*)(sectionVbData + offset3 + 8) = new Vector2(xOffset + 1, yOffset + 1);
                    }
                }

                vbPosUV = GraphicDeviceFactory.Device.CreateVertexBuffer(dataSize, 16, data: vbData);

                #endregion
            }

            #endregion
        }

        public void Smoot(int kernelSize, int times = 1, float std_dev = 3)
        {
            var sd = heightMap.Description;
            var kernel = GetGaussianKernel2D(kernelSize, std_dev);

            float[] output = new float[heights.Length];
            float[] src = heights;

            for (int i = 0; i < times; i++)
            {
                if (i > 0)
                {
                    var temp = src;
                    src = output;
                    output = temp;
                }

                Convolve2D(src, new Size(sd.Width, sd.Height), kernel, new Size(kernelSize, kernelSize),
                new Vector2(kernelSize / 2, kernelSize / 2), output);
            }
            

            var normals = ComputeNormals(output, sd.Width, sd.Height);

            _UpdateSectionData(output, normals);
        }

        private unsafe void _UpdateSectionData(float[] heights, Vector3[] normals)
        {
            this.heights = heights;
            var nbSectionsX = sections.GetLength(0);
            var nbSectionsY = sections.GetLength(1);
            var sd = heightMap.Description;
            var width = sd.Width;
            var height = sd.Height;
            int xVertPerSections = width / nbSectionsX;
            int yVertPerSections = height / nbSectionsY;
            var stride = 24;
            int dataSize = xVertPerSections * yVertPerSections * stride;
            var sectionVbData = new byte [dataSize];
            int heightOffset = 0;
            int normalOffset = 4;

            fixed (byte* pbuffer = sectionVbData)
            {
                for (int i = 0; i < nbSectionsY; i++)
                {
                    int y = i * (yVertPerSections - 1);
                    for (int j = 0; j < nbSectionsX; j++)
                    {
                        int x = j * (xVertPerSections - 1);

                        var section = sections[j, i];
                        var vb = section.NormalHeightVb;
                        vb.Read(sectionVbData, 0);                        

                        for (int yOffset = 0; yOffset < yVertPerSections; yOffset++)
                        {
                            for (int xOffset = 0; xOffset < xVertPerSections; xOffset++)
                            {
                                int offset = (yOffset * xVertPerSections + xOffset) * stride;
                                int tableIndex = (y + yOffset) * width + x + xOffset;

                                *(float*)(pbuffer + offset + heightOffset) = heights[tableIndex];
                                *(Vector3*)(pbuffer + offset + normalOffset) = normals[tableIndex];                                
                            }
                        }

                        vb.Write(sectionVbData);
                    }
                }
            }
        }

        private static float[] GetGaussianKernel2D(int kernelSize, float std_dev)
        {
            int i, j;
            float[] kernel = new float[kernelSize * kernelSize];
            int pivot = kernelSize / 2;
            float totalWeight = 0.0f;

            for (i = 0; i < kernelSize; i++)
            {
                for (j = 0; j < kernelSize; j++)
                {
                    float x = j - pivot;
                    float y = i - pivot;
                    float weight = Numerics.GaussianDistribution(x, y, std_dev);
                    kernel[i * kernelSize + j] = weight;
                    totalWeight += weight;
                }
            }

            float invTotalWeight = 1 / totalWeight;
            for (i = 0; i < kernel.Length; i++)
            {
                kernel[i] *= invTotalWeight;
            }

            return kernel;
        }

        private static void Convolve2D(float[] input, Size inputSize, float[] kernel, Size kernelSize, Vector2 kPivot, float[] output)
        {
            int pivotX = (int)kPivot.X;
            int pivotY = (int)kPivot.Y;

            for (int i = 0; i < inputSize.Height; i++)
            {
                for (int j = 0; j < inputSize.Width; j++)
                {
                    float convVal = 0;
                    for (int ki = 0; ki < kernelSize.Height; ki++)
                    {
                        for (int kj = 0; kj < kernelSize.Width; kj++)
                        {
                            int offsetX = j + kj - pivotX;
                            int offsetY = i + ki - pivotY;

                            if (offsetX >= 0 && offsetX < inputSize.Width && 
                                offsetY >= 0 && offsetY < inputSize.Height)
                            {
                                convVal += input[offsetY * inputSize.Width + offsetX] *
                                          kernel[ki * kernelSize.Width + kj];
                            }
                        }
                    }

                    output[i * inputSize.Width + j] =  convVal;
                }
            }
        }

        private static Vector3[] ComputeNormals(float[] heights, int width, int height)
        {
            Vector3[] normals = new Vector3[width * height];
            float dx = 1.0f / (float)width;
            float dz = 1.0f / (float)height;
            unsafe
            {
                var positions = stackalloc Vector3[4];

                for (int i = 0; i < height - 1; i++)
                {
                    for (int j = 0; j < width - 1; j++)
                    {
                        //Quad Corners
                        int p0 = i * width + j;
                        int p1 = i * width + j + 1;
                        int p2 = (i + 1) * width + j;
                        int p3 = (i + 1) * width + j + 1;
                                              
                        positions[0] = new Vector3(j * dx, heights[p0],  i * dz);                      
                        positions[1] = new Vector3((j + 1) * dx, heights[p1],  i * dz);                        
                        positions[2] = new Vector3(j * dx, heights[p2],  (i + 1) * dz);                        
                        positions[3] = new Vector3( (j + 1) * dx, heights[p3],  (i + 1) * dz);

                        //Vector3 normal1 = Geometry.ComputeFaceNormal(positions[0], positions[1], positions[2]);
                        //Vector3 normal2 = Geometry.ComputeFaceNormal(positions[2], positions[1], positions[3]);
                        Vector3 normal1 = Triangle.ComputeFaceNormal(positions[2], positions[1], positions[0]);
                        Vector3 normal2 = Triangle.ComputeFaceNormal(positions[3], positions[1], positions[2]);
                        Vector3 avgNormal = Vector3.Normalize((normal1 + normal2) * 0.5f);

                        normals[p0] += normal1;
                        normals[p1] += avgNormal;
                        normals[p2] += normal2;
                        normals[p3] += avgNormal;
                    }
                }
            }

            for (int i = 0; i < normals.Length; i++)
                normals[i].Normalize();

            return normals;
        }
       
    }

    public class TerrainMaterial : SurfaceMaterial, IAssetProvider, INameable
    {
        Texture2D[] layers = new Texture2D[0];
        Texture2D blendLayer;     
        private bool containsAlpha;
        private string name;

        public Texture2D[] Layers { get { return layers; } set { layers = value; } }

        public Texture2D BlendLayer { get { return blendLayer; } set { blendLayer = value; } }
     
        
        public string Name
        {
            get { return name; }
            set { name = value; }
        }                

        public bool ContainsTrasparency { get { return containsAlpha; } }

        public override Asset CreateAsset()
        {
            throw new NotImplementedException();
        }        

        //protected override void OnDispose(bool d)
        //{
        //    if (d)
        //    {
        //        if (layers != null)
        //        {
        //            foreach (var item in layers)
        //            {
        //                item.Dispose();
        //            }
        //        }
        //        if (blendLayer != null)
        //            blendLayer.Dispose();
        //    }

        //    base.OnDispose(d);
        //}
    }
   

    //[Serializable]          
    // public class TerrainHeightField : Terrain
    //{

    //    public enum TexCoordGeneration { Tiled = 0, Stretched =1}

    //    #region Fields 
    //    const int DefaultBufferSize = 63 * 63 * 4;

    //    float dx, dz;       
    //    uint xPoints;
    //    uint zPoints;        
    //    float z0;
    //    float x0;
    //    [NonSerialized]
    //    TerrainVertex[] vertexes;        
    //    int vOffset;
    //    int iOffset;
    //    int maxQuadTreeLevel = 4;

    //    ImageBuffer heightMap;
    //    int bufferSize = 127 * 127 * 4;

    //    [NonSerialized]
    //    byte[] buffer;

    //    [NonSerialized]
    //    byte[] indexes;

    //    [NonSerialized]
    //    VertexBuffer vBuffer;

    //    [NonSerialized]
    //    IndexBuffer iBuffer;

    //    byte[] _buffer;
    //    #endregion                          
        
    //    public TexCoordGeneration TextureGeneration { get; set; }

    //    public TerrainHeightField() 
    //    {          
    //        valid = false;          
    //    }

    //    public TerrainHeightField(float width, float depth, float minHeight, float maxHeight, int maxQuadTreeLevel,ImageBuffer heightMap , bool disposeHeighMap = false)
    //        :this()
    //    {
           
    //        this.minHeight = minHeight;
    //        this.maxHeight = maxHeight;
    //        this.xSize = width;
    //        this.zSize = depth;
    //        this.heightMap = heightMap;
    //        this.maxQuadTreeLevel = maxQuadTreeLevel;

    //        Initialize();

    //        if (disposeHeighMap)
    //        {
    //            heightMap.Dispose();
    //            this.heightMap = null;
    //        }          
    //    }

    //    #region Properties

    //    public int MaxQuadTreeLevel
    //    {
    //        get { return maxQuadTreeLevel; }
    //        set
    //        {
    //            if (value <= 0) throw new ArgumentOutOfRangeException("MaxQuadTreeLevel must be greather than 0");
    //            maxQuadTreeLevel = value;
    //        }
    //    }

    //    public ImageBuffer HeightMap
    //    {
    //        get { return heightMap; }
    //        set
    //        {
    //            if (value == null) throw new ArgumentNullException("height map cant not be null");
    //            if (heightMap != value)
    //            {
    //                heightMap = value;
    //                Initialize();
    //            }                
    //        }
    //    }

    //    public int BufferSize
    //    {
    //        get { return bufferSize; }
    //        set
    //        {
    //            if (value <= 0) throw new ArgumentOutOfRangeException("value must be greather than 0");
    //            bufferSize = value;
    //        }
    //    }


    //    public SizeF Size 
    //    {
    //        get { return new SizeF(xSize, zSize); }
    //        set
    //        {
    //            if (xSize != value.Width && zSize != value.Height)
    //            {
    //                xSize = value.Width;
    //                zSize = value.Height;
    //                Initialize();
    //            }
    //        }
    //    }

    //    public SizeF Heights
    //    {
    //        get { return new SizeF(minHeight, maxHeight); }
    //        set
    //        {
    //            if (minHeight != value.Width && maxHeight != value.Height)
    //            {
    //                minHeight = value.Width;
    //                maxHeight = value.Height;
    //                Initialize();
    //            }
    //        }
    //    }

    //   
    //    public float XSize
    //    {
    //        get { return xSize; }
    //        set
    //        {
    //            if (value <= 0) throw new ArgumentOutOfRangeException("Width must be greather than 0");
    //            xSize = value;
    //        }
    //    }

    //   
    //    public float ZSize
    //    {
    //        get { return zSize; }
    //        set { if (value <= 0) throw new ArgumentOutOfRangeException("Depth must be greather than 0"); zSize = value; }
    //    }

    //   
    //    public float MinHeight
    //    {
    //        get { return minHeight; }
    //        set
    //        {
    //            minHeight = value;
    //            pivot.Y = 0.5f * (maxHeight + minHeight);
    //        }
    //    }

    //   
    //    public float MaxHeight
    //    {
    //        get { return maxHeight; }
    //        set
    //        {
    //            maxHeight = value;
    //            pivot.Y = 0.5f * (maxHeight + minHeight);
    //        }
    //    }            


    //    #endregion

    //    public override void Initialize()
    //    {
    //        valid = false;
    //        if (heightMap != null && heightMap.Width > 0 && heightMap.Heigth > 0 &&
    //            xSize > 0 && zSize > 0 && maxQuadTreeLevel > 0)
    //        {
    //            try
    //            {
    //                this.xPoints = (uint)heightMap.Width;
    //                this.zPoints = (uint)heightMap.Heigth;
    //                this.dx = xSize / (heightMap.Width - 1);
    //                this.dz = zSize / (heightMap.Heigth - 1);
    //                this.x0 = -xSize * 0.5f;
    //                this.z0 = zSize * 0.5f;

    //                vertexes = new TerrainVertex[xPoints * zPoints];

    //                if (maxQuadTreeLevel <= 0)
    //                    maxQuadTreeLevel = 10;

    //                BuildTerrainVertices();
                  
    //                pivot = new Vector3(0, 0.5f * (maxHeight + minHeight), 0);
    //                globalPosition = pivot;
    //                localSphere.Radius = 0.5f * (float)Math.Sqrt(xSize * xSize + zSize * zSize);

    //                buffer = new byte[bufferSize * Marshal.SizeOf(typeof(TerrainVertex))];
    //                indexes = new byte[(bufferSize / 4) * 6 * sizeof(ushort)];
    //                //buffer = new byte[(xPoints - 1) * (zPoints - 1) * 4 * Marshal.SizeOf(typeof(TerrainVertex))];
    //                //indexes = new byte[6 * (xPoints - 1) * (zPoints - 1) * sizeof(ushort)];

    //                if (vBuffer != null && !vBuffer.Disposed)
    //                    vBuffer.Dispose();
    //                if (iBuffer != null && !iBuffer.Disposed)
    //                    iBuffer.Dispose();

    //                vBuffer = GraphicDeviceFactory.Device.CreateVertexBuffer<TerrainVertex>(bufferSize, Usage.WriteOnly, TerrainVertex.VertexFormat, Pool.Managed);
    //                iBuffer = GraphicDeviceFactory.Device.CreateIndexBuffer((bufferSize / 4) * 6, Usage.WriteOnly, Pool.Managed, true);

    //                if (vd == null)
    //                    vd = VertexDescriptor.GetDescriptorVertexDescriptor<TerrainVertex>();

    //                if (vertexes.Length <= DefaultBufferSize)
    //                {
    //                    vBuffer.SetData<TerrainVertex>(vertexes);
    //                    int k = 0;
    //                    unsafe
    //                    {
    //                        fixed (byte* addr = indexes)
    //                        {
    //                            ushort* pter = (ushort*)addr;
    //                            for (int j = 0; j < zPoints - 1; j++)
    //                            {
    //                                for (int i = 0; i < xPoints - 1; i++)
    //                                {
    //                                    pter[k++] = (ushort)(j * xPoints + i);
    //                                    pter[k++] = (ushort)(j * xPoints + i + 1);
    //                                    pter[k++] = (ushort)((j + 1) * xPoints + i);

    //                                    pter[k++] = (ushort)(j * xPoints + i + 1);
    //                                    pter[k++] = (ushort)((j + 1) * xPoints + i + 1);
    //                                    pter[k++] = (ushort)((j + 1) * xPoints + i);

    //                                }
    //                            }
    //                        }
    //                    }
    //                    iBuffer.SetData(indexes);
    //                }                    

    //                valid = true;
    //            }
    //            catch (Exception e)
    //            {
    //                valid = false;
    //                if (vBuffer != null && !vBuffer.Disposed)
    //                    vBuffer.Dispose();
    //                if (iBuffer != null && !iBuffer.Disposed)
    //                    iBuffer.Dispose();
    //                if (vd != null && !vd.Disposed)
    //                    vd.Dispose();

    //                throw e;
    //            }
    //        }
    //        else
    //            throw new InvalidOperationException("Invalid Terrain Data");
    //    }       

    //    //protected override void OnSerializing(StreamingContext context)
    //    //{
    //    //    base.OnSerializing(context);
    //    //    _buffer = GetBuffer();
    //    //}
       
    //    //protected override void OnSerialized(StreamingContext context)
    //    //{
    //    //    base.OnSerialized(context);
    //    //    _buffer = null;
    //    //    GC.Collect();
    //    //}

    //    private unsafe byte[] GetBuffer()
    //    {
    //        fixed (TerrainVertex* pter = vertexes)
    //        {
    //            byte[] buffer = new byte[sizeof(TerrainVertex) * vertexes.Length];

    //            Marshal.Copy((IntPtr)pter, buffer, 0, buffer.Length);             

    //            return buffer;
    //        }
    //    }

    //    private unsafe TerrainVertex[] GetVertexes(byte[] buffer)
    //    {
    //        TerrainVertex[] data = new TerrainVertex[buffer.Length / sizeof(TerrainVertex)];
    //        fixed (TerrainVertex* pter = data)
    //        {
    //            Marshal.Copy(buffer, 0, (IntPtr)pter, buffer.Length);
    //        }
    //        return data;
    //    }        

    //    //protected override void OnDeserialized(StreamingContext context)
    //    //{
    //    //    base.OnDeserialized(context);
    //    //    vertexes = GetVertexes(_buffer);
    //    //    _buffer = null;
    //    //    GC.Collect();

    //    //    buffer = new byte[bufferSize * Marshal.SizeOf(typeof(TerrainVertex))];
    //    //    indexes = new byte[6 * bufferSize * sizeof(ushort)];


    //    //    vBuffer = GraphicDeviceFactory.Device.CreateVertexBuffer<TerrainVertex>(bufferSize, Usage.WriteOnly, TerrainVertex.VertexFormat, Pool.Managed);
    //    //    iBuffer = GraphicDeviceFactory.Device.CreateIndexBuffer(6 * bufferSize, Usage.WriteOnly, Pool.Managed, true);

    //    //    if (vd == null)
    //    //        vd = VertexDescriptor.GetDescriptorVertexDescriptor<TerrainVertex>();

    //    //    if (vertexes.Length <= DefaultBufferSize)
    //    //    {
    //    //        vBuffer.SetData<TerrainVertex>(vertexes);
    //    //        iBuffer.SetData(indexes);
    //    //    }

    //    //    valid = true;
    //    //}       
       
    //    private Vector3 BuildPosition(int i, int j)
    //    {
    //        float z = z0 - i * dz;
    //        float x = x0 + j * dx;
    //        float y = heightMap.GetBlue(i, j);
    //        return new Vector3(x, maxHeight * y + minHeight * (1 - y), z);
    //    }

    //    public void BuildTerrainVertices()
    //    {                           
    //        for (int i = 0; i < zPoints; i++)
    //        {
    //            float z = z0 - i * dz;
    //            for (int j = 0; j < xPoints; j++)
    //            {
    //                float x = x0 + j * dx;

    //                float y = heightMap.GetBlue(i, j);
    //                y = maxHeight * y + minHeight * (1 - y);
    //                vertexes[i * xPoints + j].Position = new Vector3(x, y, z);
    //                vertexes[i * xPoints + j].TexCoord = new Vector2((float)j / (float)(xPoints - 1), (float)i / (float)(zPoints - 1));
    //                vertexes[i * xPoints + j].BlendTexCoord = vertexes[i * xPoints + j].TexCoord;
    //            }
    //        }

    //        BuildNormals();
    //    }

    //    private void BuildNormals()
    //    {
    //        uint stacks = zPoints - 1;
    //        uint slices = xPoints - 1;

    //        for (uint i = 0; i < stacks; i++)
    //        {
    //            for (uint j = 0; j < slices; j++)
    //            {
    //                //Quad Corners
    //                uint p0 = (i * xPoints + j);
    //                uint p1 = (i * xPoints + j + 1);
    //                uint p2 = ((i + 1) * xPoints + j + 1);
    //                uint p3 = ((i + 1) * xPoints + j);

    //                Vector3 normal1 = Geometry.ComputeFaceNormal(vertexes[p0].Position, vertexes[p1].Position, vertexes[p3].Position);
    //                Vector3 normal2 = Geometry.ComputeFaceNormal(vertexes[p3].Position, vertexes[p1].Position, vertexes[p2].Position);
    //                Vector3 avgNormal = Vector3.Normalize((normal1 + normal2) * 0.5f);                   

    //                vertexes[p0].Normal += normal1;
    //                vertexes[p1].Normal += avgNormal;
    //                vertexes[p2].Normal += normal2;
    //                vertexes[p3].Normal += avgNormal;
    //            }
    //        }

    //        for (int i = 0; i < vertexes.Length; i++)
    //        {
    //            vertexes[i].Normal.Normalize();
    //        }
    //    }

    //    public override void AdjustHeights(float minHeight, float maxHeight)
    //    {
    //        if (heightMap.Disposed) throw new InvalidOperationException("HeightMap have been disposed");

    //        if (valid)
    //        {
    //            this.minHeight = minHeight;
    //            this.maxHeight = maxHeight;

    //            BuildTerrainVertices();
    //        }
    //    }       

    //    public void Smoot(int passes)
    //    {
    //        TerrainVertex[] smooted = new TerrainVertex[vertexes.Length];
    //        uint stacks = zPoints - 1;
    //        uint slices = xPoints - 1;
    //        for (int k = 0; k < passes; k++)
    //        {               
    //            for (uint i = 0; i < stacks; i++)
    //            {
    //                for (uint j = 0; j < slices; j++)
    //                {
    //                    //Quad Corners
    //                    uint p0 = (i * xPoints + j);
    //                    uint p1 = (i * xPoints + j + 1);
    //                    uint p2 = ((i + 1) * xPoints + j + 1);
    //                    uint p3 = ((i + 1) * xPoints + j);

    //                    smooted[p0] = vertexes[p0];
    //                    smooted[p0].Position.Y = 0.25f * (vertexes[p0].Position.Y +
    //                                            vertexes[p1].Position.Y +
    //                                            vertexes[p2].Position.Y +
    //                                            vertexes[p3].Position.Y);
    //                }
    //            }
    //            for (uint i = 0; i < xPoints; i++)
    //            {
    //                uint index = (zPoints - 1) * xPoints + i;
    //                smooted[index] = vertexes[index];
    //            }
    //            for (uint i = 0; i < zPoints; i++)
    //            {
    //                uint index = i * xPoints + xPoints - 1;
    //                smooted[index] = vertexes[index];
    //            }
    //            var temp = vertexes;
    //            vertexes = smooted;
    //            smooted = temp;
    //        }

    //        BuildNormals();
    //    }

    //    //private CullState GetCullState(Rectangle rec, Plane[] planes)
    //    //{
    //    //    float x = (x0 + rec.X * dx) * scale.X + localOffset.X;
    //    //    float z = (z0 - rec.Y * dz) * scale.Z + localOffset.Z;
    //    //    float width = rec.Width * dx * scale.X;
    //    //    float depth = rec.Height * dz * scale.Z;

    //    //    float y0 = vertexes[rec.Y * xPoints + rec.X].Position.Y;
    //    //    float y1 = vertexes[rec.Y * xPoints + rec.X + rec.Width].Position.Y;
    //    //    float y2 = vertexes[(rec.Y + rec.Height) * xPoints + rec.X + rec.Width].Position.Y;
    //    //    float y3 = vertexes[(rec.Y + rec.Height) * xPoints + rec.X].Position.Y;
    //    //    float y = 0.25f * (y0 + y1 + y2 + y3) * scale.Y + localOffset.Y;

    //    //    Vector3 center = new Vector3(x + width * 0.5f, y, z - depth * 0.5f);
  
    //    //    float r2 = 0.25f * (width * width + depth * depth);
    //    //    float distance;
    //    //    float d2;
    //    //    int count = 0;
    //    //    for (int i = 0; i < planes.Length; i++)
    //    //    {
    //    //        distance = Plane.DotCoordinate(planes[i],center);
    //    //        d2 = distance * distance;
    //    //        if (distance < 0 && d2 >= r2)
    //    //            return CullState.AllOutside;
    //    //        if (d2 >= r2)
    //    //            count++;
    //    //    }
    //    //    return count == planes.Length ? CullState.Inside : CullState.Partial;
    //    //}      
     
    //    public override float HeightOfTerrain(Vector3 position)
    //    {
    //        return HeightAt(position.X, position.Z);
    //    }

    //    public float HeightAt(float x, float z)
    //    {
    //        unsafe
    //        {
    //            //transform to local coordinates 
    //            Vector3 v = Vector3.TransformCoordinate(new Vector3(x, 0, z), invWorldMtx);
    //            x = v.X;
    //            z = v.Z;

    //            float fi = Math.Max((z0 - z) / dz, 0);
    //            float fj = Math.Max((x - x0) / dx, 0);

    //            int i = (int)fi;
    //            int j = (int)fj;

    //            if (j >= xPoints - 1) j = (int)xPoints - 2;
    //            if (i >= zPoints - 1) i = (int)zPoints - 2;

    //            float y0 = vertexes[i * xPoints + j].Position.Y ;
    //            float y1 = vertexes[i * xPoints + j + 1].Position.Y;
    //            float y2 = vertexes[(i + 1) * xPoints + j + 1].Position.Y;
    //            float y3 = vertexes[(i + 1) * xPoints + j].Position.Y;

    //            float fragZ = Numerics.Frag(fi);
    //            float fragX = Numerics.Frag(fj);

    //            //interpolar las alturas en el quad

    //            float height = Numerics.Lerp(Numerics.Lerp(y0, y1, fragX),
    //                                      Numerics.Lerp(y3, y2, fragX), fragZ);

    //            return Vector3.TransformCoordinate(new Vector3(x, height, z), globalPose).Y;
    //        }
    //    }

    //    public override float HeightAboveTerrain(Vector3 position)
    //    {
    //        float y = Vector3.TransformCoordinate(position, invWorldMtx).Y;

    //        return y - HeightAt(position.X, position.Z);
    //    }        

    //    public override bool InLineOfSight(Vector3 position1, Vector3 position2)
    //    {
    //       position1 = Vector3.TransformCoordinate(position1, invWorldMtx);
    //       position2 = Vector3.TransformCoordinate(position2 , invWorldMtx);

    //       bool los = true;
    //       float z;

    //       float dx = position2.X - position1.X; 
    //       float dy = position2.Y - position1.Y; 
    //       float dz = position2.Z - position1.Z; 
    //       float dp = dz / dx; 

    //       float dist = (float)Math.Sqrt(dx*dx + dz*dz); 
    //       float de = dy / dist; 

    //       float IncX =  dz * 0.75f; 
    //       float y = position1.Y; 
    //       float x = position1.X; 

    //       while ( x < position2.X && los ) 
    //       { 
    //          z = position1.Z + ( x - position1.X ) * dp; 
    //          los = HeightAt(x, z ) <= y; 
    //          x += IncX; 
    //          y += (IncX*dp) * de; 
    //       } 
    //       return los; 

    //    }      

    //    public override Euler GetSlope(Vector3 position, float heading)
    //    {
    //        Euler attitude = new Euler();

    //        //Transform to local coordinate space
    //        position = Vector3.TransformCoordinate(position, invWorldMtx);
    //        float x = position.X;
    //        float z = position.Z;


    //        float fi = Math.Max((z0 - z) / dz, 0);
    //        float fj = Math.Max((x - x0) / dx, 0);

    //        int i = (int)fi;
    //        int j = (int)fj;

    //        if (j >= xPoints - 1) j = (int)xPoints - 2;
    //        if (i >= zPoints - 1) i = (int)zPoints - 2;

    //        Vector3 normal0 = vertexes[i * xPoints + j].Normal;
    //        Vector3 normal1 = vertexes[i * xPoints + j + 1].Normal;
    //        Vector3 normal2 = vertexes[(i + 1) * xPoints + j + 1].Normal;
    //        Vector3 normal3 = vertexes[(i + 1) * xPoints + j].Normal;

    //        float fragZ = Numerics.Frag(fi);
    //        float fragX = Numerics.Frag(fj);

    //        Vector3 normal = Numerics.Lerp(Numerics.Lerp(normal0, normal1, fragX),
    //                                 Numerics.Lerp(normal3, normal2, fragX), fragZ);

    //        Matrix invRot = Matrix.Transpose(Matrix.RotationY(heading));
    //        normal = Vector3.TransformNormal(normal,invRot);

    //        if (normal.Z == 0.0f)
    //            attitude.Pitch = 0.0f;
    //        else
    //        {
    //            attitude.Pitch = -(float)Math.Atan(normal.Y / normal.Z);
    //            attitude.Pitch = attitude.Pitch > 0.0f ? Numerics.PIover2 - attitude.Pitch : -(Numerics.PIover2 + attitude.Pitch);

    //        }

    //        if (normal.X == 0.0f)
    //            attitude.Roll = 0.0f;
    //        else
    //        {
    //            attitude.Roll = -(float)Math.Atan(normal.Y / normal.X);
    //            attitude.Roll = attitude.Roll > 0 ? Numerics.PIover2 - attitude.Roll : -(Numerics.PIover2 + attitude.Roll);
    //        }

    //        attitude.Heading = heading;

    //        return attitude;          
    //    }

    //    public CullState GetCullState(Rectangle rec, Camera camera)
    //    {
    //        float y0 = vertexes[rec.Y * xPoints + rec.X].Position.Y;
    //        float y1 = vertexes[rec.Y * xPoints + rec.X + rec.Width].Position.Y;
    //        float y2 = vertexes[(rec.Y + rec.Height) * xPoints + rec.X + rec.Width].Position.Y;
    //        float y3 = vertexes[(rec.Y + rec.Height) * xPoints + rec.X].Position.Y;

    //        float width = rec.Width * dx;
    //        float depth = rec.Height * dz;

    //        Vector3 p0 = new Vector3((x0 + rec.X * dx), 0.25f * (y0 + y1 + y2 + y3), (z0 - rec.Y * dz));
    //        Vector3 p1 = new Vector3(p0.X + width, p0.Y, p0.Z - depth);
    //        Vector3 center = new Vector3(p0.X + width * 0.5f, p0.Y, p0.Z - depth * 0.5f);

    //        p0 = Vector3.TransformCoordinate(p0, globalPose);
    //        p1 = Vector3.TransformCoordinate(p1, globalPose);
    //        center = Vector3.TransformCoordinate(center, globalPose);

    //        float radius = Vector3.Distance(p0, p1) * 0.5f;

    //        var cull = camera.CheckFrustum(center, radius);

    //        return cull;
    //    }

    //    public void CullQuads(Rectangle rec, Camera camera, int level)
    //    {
    //        if (rec.Width == 0 || rec.Height == 0)
    //            return;

    //        CullState state = GetCullState(rec, camera);

    //        if (state == CullState.Inside)
    //            FillBuffer(rec);

    //        else if (state == CullState.Partial)
    //        {
    //            if ((rec.Width == 1 && rec.Height == 1) || level >= maxQuadTreeLevel)
    //                FillBuffer(rec);
    //            else
    //            {
    //                int halfWidth = rec.Width > 1 ? rec.Width >> 1 : 1; // width / 2
    //                int halfHeight = rec.Height > 1 ? rec.Height >> 1 : 1; //height / 2                                      
    //                int restoWidth = rec.Width - halfWidth;
    //                int restoHeight = rec.Height - halfHeight;

    //                CullQuads(new Rectangle(rec.X, rec.Y, halfWidth, halfHeight), camera, level + 1); ;
    //                CullQuads(new Rectangle(rec.X + halfWidth, rec.Y, restoWidth, halfHeight), camera, level + 1);
    //                CullQuads(new Rectangle(rec.X, rec.Y + halfHeight, halfWidth, restoHeight), camera, level + 1);
    //                CullQuads(new Rectangle(rec.X + halfWidth, rec.Y + halfHeight, restoWidth, restoHeight), camera, level + 1);
    //            }
    //        }
    //    }

    //    private unsafe void FillBuffer(Rectangle rec)
    //    {
    //        Device device = GraphicDeviceFactory.Device;
    //        int iEnd = rec.Y + rec.Height;
    //        int jEnd = rec.X + rec.Width;
    //        var text = TextureGeneration;
    //        fixed (byte* pBuffer = buffer)
    //        {
    //            fixed (byte* pIndex = indexes)
    //            {
    //                TerrainVertex* vpter = (TerrainVertex*)pBuffer;
    //                ushort* ipter = (ushort*)pIndex;

    //                for (int i = rec.Y; i < iEnd; i++)
    //                {
    //                    for (int j = rec.X; j < jEnd; j++)
    //                    {
    //                        if (vOffset >= bufferSize)
    //                        {
    //                            vBuffer.SetData(buffer, 0, vOffset * sizeof(TerrainVertex), 0);
    //                            iBuffer.SetData(indexes, 0, iOffset * sizeof(ushort), 0);

    //                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vOffset, 0, iOffset / 3);
    //                            vOffset = 0;
    //                            iOffset = 0;
    //                        }

    //                        vpter[vOffset] = vertexes[xPoints * i + j];
    //                        vpter[vOffset + 1] = vertexes[xPoints * i + j + 1];
    //                        vpter[vOffset + 2] = vertexes[xPoints * (i + 1) + j + 1];
    //                        vpter[vOffset + 3] = vertexes[xPoints * (i + 1) + j];

    //                        if (text == TexCoordGeneration.Tiled)
    //                        {
    //                            vpter[vOffset].TexCoord = new Vector2(0, 0);
    //                            vpter[vOffset + 1].TexCoord = new Vector2(1, 0);
    //                            vpter[vOffset + 2].TexCoord = new Vector2(1, 1);
    //                            vpter[vOffset + 3].TexCoord = new Vector2(0, 1);
    //                        }

    //                        //triangle 1                   
    //                        ipter[iOffset++] = (ushort)vOffset;
    //                        ipter[iOffset++] = (ushort)(vOffset + 1);
    //                        ipter[iOffset++] = (ushort)(vOffset + 3);
    //                        //triangle 2
    //                        ipter[iOffset++] = (ushort)(vOffset + 1);
    //                        ipter[iOffset++] = (ushort)(vOffset + 2);
    //                        ipter[iOffset++] = (ushort)(vOffset + 3);

    //                        vOffset += 4;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    Matrix view;
    //    public override void RenderGeometry()
    //    {
    //        unsafe
    //        {
    //            var camera = RenderingCamera;
    //            Device device = GraphicDeviceFactory.Device;
    //            var fill = device.GetRenderState(RenderState.FillMode);
    //            device.SetRenderState(RenderState.FillMode, Engine.Shading.FillMode);
    //            device.SetStreamSource(0, vBuffer, 0, vd.Size);
    //            device.Indices = iBuffer;                

    //            if (vertexes.Length <= DefaultBufferSize)
    //            {
    //                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexes.Length, 0, indexes.Length / 3);
    //            }
    //            else
    //            {
    //                //if (view != camera.ViewMatrix)
    //                //{
    //                    vOffset = 0;
    //                    iOffset = 0;
    //                    CullQuads(new Rectangle(0, 0, (int)xPoints - 1, (int)zPoints - 1), camera, 0);
    //                    if (vOffset > 0)
    //                    {
    //                        vBuffer.SetData(buffer, 0, vOffset * sizeof(TerrainVertex), 0);
    //                        iBuffer.SetData(indexes, 0, iOffset * sizeof(ushort), 0);
    //                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vOffset, 0, iOffset / 3);
    //                    }
    //                //    view = camera.ViewMatrix;
    //                //}

    //                //RenderBuffers(device);
    //            }

    //            device.SetRenderState(RenderState.FillMode, fill);            
    //        }            
    //    }

    //    private unsafe void RenderBuffers(Device device)
    //    {      
    //        fixed (byte* vAddr = buffer)
    //        {
    //            fixed (byte* iAddr = indexes)
    //            {                   
    //                int indexSize = (bufferSize / 4) * 6;
    //                int vsize = sizeof(TerrainVertex);
    //                int isize = sizeof(ushort);
    //                int bufferOffset;
    //                int indexOffset;
    //                int count = vOffset / bufferSize;
    //                for (int i = 0; i < count; i++)
    //                {
    //                    bufferOffset = (count * bufferSize) * vsize;
    //                    indexOffset = (count * indexSize) * isize;

    //                    vBuffer.SetData((IntPtr)(vAddr + bufferOffset), bufferSize * vsize);
    //                    iBuffer.SetData((IntPtr)(iAddr + indexOffset), indexSize * isize);
    //                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, bufferSize, 0, indexSize / 3);
    //                }

    //                int resto = vOffset % bufferSize;
    //                if (resto > 0)
    //                {
    //                    indexSize = (resto / 4) * 6;
    //                    bufferOffset = (count * bufferSize) * vsize;
    //                    indexOffset = (count * indexSize) * isize;

    //                    vBuffer.SetData((IntPtr)(vAddr + bufferOffset), resto * vsize);
    //                    iBuffer.SetData((IntPtr)(iAddr + indexOffset), indexSize * isize);
    //                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, resto, 0, indexSize / 3);
    //                }
    //            }
    //        }
    //    }

    //    public override void Dispose()
    //    {
    //        if (!Disposed)
    //        {
    //            if (heightMap != null)
    //                heightMap.Dispose();

    //            if (vBuffer != null)
    //                vBuffer.Dispose();
    //            if (iBuffer != null)
    //                iBuffer.Dispose();

    //            base.Dispose();
    //        }
    //    }

    //    public void Dispose(bool onlyPrivateResources)
    //    {
    //        if (onlyPrivateResources)
    //        {
    //            vd.Dispose();
    //            if (vBuffer != null)
    //                vBuffer.Dispose();
    //            if (iBuffer != null)
    //                iBuffer.Dispose();
    //        }
    //        else
    //            Dispose();
    //    }             
       
    //}
 
}
