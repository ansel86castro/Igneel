namespace Igneel.SceneComponents
{
    //[Serializable]   
    //[TypeConverter(typeof(DesignTypeConverter))]     
    //public class Water : SceneNode<Water>
    //{              
    //    private float xSize;
    //    private float zSize;      
    //    private int xPoints;
    //    private int zPoints;
    //    [NonSerialized] VertexDescriptor vd;                   
    //    [NonSerialized] VertexPNTTx[] vertexes;
    //    [NonSerialized] VertexPNTTx[] transformed;        
    //    int bufferSize;      
    //    private float xStart;
    //    private float zStart;
    //    private float dx;
    //    private float dz;
    //    int vOffset;
    //    int iOffset;
    //    float waveHeight= 0.8f;
    //    float timeScale = 1f;
    //    float waveLenght = 0.1f;
    //    float time;
    //    Vector3 winDirection;        
    //    List<SceneNodeTechnique> techniques;
    //    LayerSurface surface;
    //    TextureHandle<Texture> normalMap;       
    //    TextureHandle<Texture> diffuseMap;
    //    TextureHandle<Texture>[] caustics;
    //    int currentCaustic;
      
    //    [NonSerialized]
    //    VertexBuffer vBuffer;
    //    [NonSerialized]
    //    IndexBuffer iBuffer;
    //    [NonSerialized]
    //    VertexPNTTx[] buffer;
    //    [NonSerialized]
    //    ushort[] indexes;
    //    Action renderCallback;

    //    private bool wireframe;
    //    private Matrix causticTexM;
    //    private CausticMergeType causticMergeType;
    //    private Camera camera;

    //    public Water(string name, Vector3 position, float xSize, float zSize, int xPoints, int zPoints)
    //        : base(name)
    //    {
    //        if (name == null) throw new ArgumentNullException("name");
    //        if (xSize < 0) throw new ArgumentOutOfRangeException("xSize");
    //        if (zSize < 0) throw new ArgumentOutOfRangeException("ySize");
    //        if (xPoints < 0) throw new ArgumentOutOfRangeException("xPoints");
    //        if (zPoints < 0) throw new ArgumentOutOfRangeException("zPoints");
          
    //        causticTexM = Matrix.Identity;
    //        vd = VertexDescriptor.GetDescriptorVertexDescriptor<VertexPNTTx>();
    //        this.xSize = xSize;
    //        this.zSize = zSize;
    //        this.globalPosition = position;
    //        this.xPoints = xPoints;
    //        this.zPoints = zPoints;
    //        bufferSize = 256;
    //        buffer = new VertexPNTTx[bufferSize];
    //        indexes = new ushort[6 * bufferSize];

    //        vBuffer = GraphicDeviceFactory.Device.CreateVertexBuffer<VertexPNTTx>(bufferSize, Usage.WriteOnly, VertexFormat.None, Pool.Managed);
    //        iBuffer = GraphicDeviceFactory.Device.CreateIndexBuffer(indexes.Length, Usage.WriteOnly, Pool.Managed, true);

    //        vertexes = new VertexPNTTx[xPoints * zPoints];
    //        transformed = new VertexPNTTx[xPoints * zPoints];
    //        Initialize();
    //    }

    //    #region Propertys

    //   
    //    public int CurrentCaustic { get { return currentCaustic; } set { currentCaustic = value; } }

    //    public Matrix CausticTransformMatrix { get { return causticTexM; } set { causticTexM = value; } }

    //    public CausticMergeType CausticMerge
    //    {
    //        get { return causticMergeType; }
    //        set { causticMergeType = value; }
    //    }

    //    public bool Wireframe
    //    {
    //        get { return wireframe; }
    //        set { wireframe = value; }
    //    }

    //    public float TimeScale
    //    {
    //        get { return timeScale; }
    //        set { timeScale = value; }
    //    }

    //    public float WaveHeight
    //    {
    //        get { return waveHeight; }
    //        set { waveHeight = value; }
    //    }

    //    public float WaveLenght
    //    {
    //        get { return waveLenght; }
    //        set { waveLenght = value; }
    //    }

    //    public float Time
    //    {
    //        get { return time / timeScale; }
    //        set { time = value; }
    //    }

    //   
    //    public Vector3 WinDirection
    //    {
    //        get { return winDirection; }
    //        set { winDirection = value; }
    //    }

    //    public float XSize
    //    {
    //        get { return xSize; }
    //        set
    //        {
    //            xSize = value;
    //            localSphere.Radius = 0.5f * (float)Math.Sqrt(xSize * xSize + zSize * zSize);
    //        }
    //    }

    //    public float ZSize
    //    {
    //        get { return zSize; }
    //        set
    //        {
    //            zSize = value;
    //            localSphere.Radius = 0.5f * (float)Math.Sqrt(xSize * xSize + zSize * zSize);
    //        }
    //    }

    //    public float XPoints { get { return xPoints; } }

    //    public float ZPoints { get { return zPoints; } }

    //    public IList<SceneNodeTechnique> Techniques
    //    {
    //        get
    //        {
    //            if (techniques == null) techniques = new List<SceneNodeTechnique>();
    //            return techniques;
    //        }
    //    }

    //    public TextureHandle<Texture> NormalMap
    //    {
    //        get { return normalMap; }
    //        set { normalMap = value; }
    //    }

    //    public TextureHandle<Texture> DiffuseMap
    //    {
    //        get { return diffuseMap; }
    //        set { diffuseMap = value; }
    //    }

    //    [Editor(typeof(UITextureArrayTypeEditor), typeof(UITypeEditor))]
    //    public TextureHandle<Texture>[] Caustics
    //    {
    //        get { return caustics; }
    //        set { caustics = value; }
    //    }

    //    public LayerSurface Surface
    //    {
    //        get { return surface; }
    //        set { surface = value; }
    //    }      

    //    #endregion        


    //    //protected override void OnSerializing(StreamingContext context)
    //    //{
    //    //    base.OnSerializing(context);
    //    //    SaveContent(new Uri((string)context.Context));
    //    //}

    //    //protected override void OnDeserialized(StreamingContext context)
    //    //{
    //    //    base.OnDeserialized(context);
           
    //    //    buffer = new VertexPNTTx[bufferSize];
    //    //    indexes = new ushort[6 * bufferSize];

    //    //    vBuffer = GraphicDeviceFactory.Device.CreateVertexBuffer<VertexPNTTx>(bufferSize, Usage.WriteOnly, VertexFormat.None, Pool.Default);
    //    //    iBuffer = GraphicDeviceFactory.Device.CreateIndexBuffer(indexes.Length, Usage.WriteOnly, Pool.Default, true);

    //    //    vd = VertexDescriptor.GetDescriptorVertexDescriptor<VertexPNTTx>();

    //    //    vertexes = new VertexPNTTx[xPoints * zPoints];
    //    //    transformed = new VertexPNTTx[xPoints * zPoints];
    //    //    Initialize();

    //    //    if(techniques!=null)
    //    //        foreach (var item in techniques)
    //    //        {
    //    //            item.Node = this;
    //    //        }
    //    //}

    //    public void SaveContent(Uri location)
    //    {
    //        if (normalMap != null)
    //            normalMap.SaveToUri(location, ImageFileFormat.Dds);
    //        if (diffuseMap != null)
    //            diffuseMap.SaveToUri(location, ImageFileFormat.Dds);
    //        if (caustics != null)
    //        {
    //            Uri causticLocation = new Uri(location, name + "_Caustic/");
    //            foreach (var item in caustics)
    //            {
    //                item.SaveToUri(causticLocation, ImageFileFormat.Dds);
    //            }
    //        }

    //    }

    //    public void LoadContent(Uri location) { }       
      
    //    public void Initialize()
    //    {
    //        float _xPoints = xPoints - 1;
    //        float _zPoints = zPoints - 1;
    //        dx = xSize / _xPoints;
    //        dz = zSize / _zPoints;
    //        xStart = globalPosition.X - xSize * 0.5f;
    //        zStart = globalPosition.Z + zSize * 0.5f;
    //        localSphere.Radius = 0.5f * (float)Math.Sqrt(xSize * xSize + zSize * zSize);

    //        for (int i = 0; i <= _zPoints; i++)
    //        {
    //            float z = zStart - dz * i;
    //            for (int j = 0; j <= _xPoints; j++)
    //            {
    //                float x = xStart + dx * j;
    //                vertexes[xPoints * i + j] = new VertexPNTTx(x, globalPosition.Y, z, 1, 0, 0, 0, 1, 0, (float)j / _xPoints, (float)i / _zPoints);
    //                transformed[xPoints * i + j] = vertexes[xPoints * i + j];
    //            }
    //        }
    //    }

    //    public CullState GetCullState(Rectangle rec, Plane[] planes)
    //    {
    //        float x = xStart + rec.X * dx;
    //        float z = zStart - rec.Y * dz;
    //        float width = rec.Width * dx;
    //        float depth = rec.Height * dz;

    //        Vector3 center = new Vector3(x + width * 0.5f, globalPosition.Y, z - depth * 0.5f);
    //        float r2 = 0.25f * (width * width + depth * depth);
    //        float distance;
    //        float d2;
    //        int count = 0;
    //        for (int i = 0; i < planes.Length; i++)
    //        {
    //            distance = planes[i].Dot(center);
    //            d2 = distance * distance;
    //            if (distance < 0 && d2 >= r2)
    //                return CullState.AllOutside;
    //            if (d2 >= r2)
    //                count++;
    //        }
    //        return count == planes.Length ? CullState.Inside : CullState.Partial;
    //    }

    //    public void CullQuads(Rectangle rec , Camera camera)
    //    {
    //        if (rec.Width <= 0 || rec.Height <= 0)
    //            return;

    //        CullState state = GetCullState(rec, camera.Frustum);
    //        if (state == CullState.Inside)            
    //            FillBuffer(rec);            
    //        else if (state == CullState.Partial)
    //        {
    //            if (rec.Width == 1 && rec.Height == 1)                
    //                FillBuffer(rec);                
    //            else
    //            {
    //                int halfWidth = rec.Width > 1 ? rec.Width >> 1 : 1; // width / 2
    //                int halfHeight = rec.Height > 1 ? rec.Height >> 1 : 1; //height / 2                                      
    //                int restoWidth = rec.Width - halfWidth;
    //                int restoHeight = rec.Height - halfHeight;

    //                CullQuads(new Rectangle(rec.X, rec.Y, halfWidth, halfHeight), camera);
    //                CullQuads(new Rectangle(rec.X + halfWidth, rec.Y, restoWidth, halfHeight), camera);
    //                CullQuads(new Rectangle(rec.X, rec.Y + halfHeight, halfWidth, restoHeight), camera);
    //                CullQuads(new Rectangle(rec.X + halfWidth, rec.Y + halfHeight, restoWidth, restoHeight), camera);
    //            }
    //        }
    //    }

    //    private void FillBuffer(Rectangle rec)
    //    {
    //        Device device = GraphicDeviceFactory.Device;
    //        int iEnd = rec.Y + rec.Height;
    //        int jEnd = rec.X + rec.Width;

    //        for (int i = rec.Y ; i < iEnd; i++)
    //        {
    //            for (int j = rec.X; j < jEnd; j++)
    //            {
    //                if (vOffset >= bufferSize)
    //                {                             
    //                    vBuffer.SetData(buffer, 0, 0);
    //                    iBuffer.SetData(indexes, 0, 0);

    //                    //device.SetStreamSource(0, vBuffer, 0);
    //                    //device.Indices = iBuffer;

    //                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vOffset, 0, iOffset / 3);
    //                    vOffset = 0;
    //                    iOffset = 0;
    //                }
    //                buffer[vOffset] = transformed[xPoints * i + j];
    //                buffer[vOffset + 1] = transformed[xPoints * i + j + 1];
    //                buffer[vOffset + 2] = transformed[xPoints * (i + 1) + j];
    //                buffer[vOffset + 3] = transformed[xPoints * (i + 1) + j + 1];

    //                //triangle 1                   
    //                indexes[iOffset++] = (ushort)vOffset;
    //                indexes[iOffset++] = (ushort)(vOffset + 1);
    //                indexes[iOffset++] = (ushort)(vOffset + 2);
    //                //triangle 2
    //                indexes[iOffset++] = (ushort)(vOffset + 1);
    //                indexes[iOffset++] = (ushort)(vOffset + 3);
    //                indexes[iOffset++] = (ushort)(vOffset + 2);

    //                vOffset += 4;
    //            }
    //        }
    //    }
             
    //    #region IRenderable Members          

    //    private void RenderGeometry()
    //    {
    //        var device = GraphicDeviceFactory.Device;
    //        vOffset = 0;
    //        iOffset = 0;
    //        var fill = device.GetRenderState(RenderState.FillMode);
    //        if (wireframe)
    //            device.SetRenderState(RenderState.FillMode, FillMode.Wireframe);

    //        device.VertexDeclaration = vd.VertexDeclaration;
    //        var cull = device.GetRenderState(RenderState.CullMode);
    //        device.SetRenderState(RenderState.CullMode, Cull.None);

    //        device.SetStreamSource(0, vBuffer, 0, vd.Size);
    //        device.Indices = iBuffer;

    //        CullQuads(new Rectangle(0, 0, xPoints - 1, zPoints - 1), camera);
    //        if (vOffset > 0)
    //        {
    //            vBuffer.SetData(buffer, 0, 0);
    //            iBuffer.SetData(indexes, 0, 0);
    //            //device.SetStreamSource(0, vBuffer, 0);
    //            //device.Indices = iBuffer;

    //            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vOffset, 0, iOffset / 3);
    //        }
    //        device.SetRenderState(RenderState.CullMode, cull);
    //        device.SetRenderState(RenderState.FillMode, fill);
    //    }

    //    #endregion

    //    #region IDynamic Members

    //    public override void Update(float deltaT)
    //    {            
    //        time += deltaT;
    //        base.Update(deltaT);
    //    }

    //    #endregion

    //    #region IGeometrySource Members

    //   
    //    public VertexDescriptor VertexDescriptor
    //    {
    //        get { return vd; }
    //    }      

    //    #endregion

    //    #region IDisposable Members
       
    //    protected override void OnDispose()
    //    {
    //        vBuffer.Dispose();
    //        iBuffer.Dispose();
    //        vd.Dispose();

    //        if (normalMap != null)
    //            normalMap.Dispose();
    //        if (diffuseMap != null)
    //            diffuseMap.Dispose();

    //        if (caustics != null)
    //            foreach (var item in caustics)
    //            {
    //                item.Dispose();
    //            }

    //        if (techniques != null)
    //            foreach (var item in techniques)
    //            {
    //                item.Dispose();
    //            }
    //        base.OnDispose();
    //    }

    //    #endregion        
    
       
    //}
}
