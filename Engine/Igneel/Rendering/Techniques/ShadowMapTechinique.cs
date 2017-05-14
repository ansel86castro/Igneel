using System;
using System.ComponentModel;
using Igneel.Assets;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
using Igneel.States;

namespace Igneel.Techniques
{
    public enum ShadowMapType { Simple, Cube }
   
   
    //[ResourceActivator(typeof(ShadowMapTechnique.Activator))]
    public class ShadowMapTechnique : BindableFrameTechnique<ShadowMapTechnique> ,IDeferreable
    {
        internal Camera camera;
        internal int size;
        internal bool isDynamic;        
        internal RenderTexture2D ShadowMap;
        internal bool Rendered;      
        internal float bias;
        Vector4[] _texCoordOffset;
        float[] _texCoordWeights;
        ViewPort _vp;
        int _kernelSize = 3;
        static RasterizerState _rasterizer;
        static DepthStencilState _detpthStencil;
        static BlendState _blendState;
      
        public ShadowMapTechnique(int size)         
        {
            Require<DepthSceneRender>();
            this.size = size;
            this.bias = EngineState.Shadow.ShadowMapping.Bias;

            Initialize();
        }

        public ShadowMapTechnique() :
            this(EngineState.Shadow.ShadowMapping.Size)
        { }


        public void Initialize()
        {
            if (_blendState == null)
            {
                _blendState = DefaultTechnique.NoBlend;
                _rasterizer = DefaultTechnique.NoCulling;
                _detpthStencil = DefaultTechnique.DephtState;
            }

            _texCoordOffset = new Vector4[EngineState.Shadow.ShadowMapping.PcfBlurSize * EngineState.Shadow.ShadowMapping.PcfBlurSize];
            _texCoordWeights = new float[EngineState.Shadow.ShadowMapping.PcfBlurSize * EngineState.Shadow.ShadowMapping.PcfBlurSize];

            if (camera == null)
            {
                camera = new Camera()
                {
                    ZNear = 1,
                    ZFar = 1000,
                    AspectRatio = 1,
                    FieldOfView = Numerics.PIover2,
                    Type = ProjectionType.Perspective
                };
            }
            camera.CommitChanges();

            GenerateSamples();

            ShadowMap = new RenderTexture2D(size, size, Format.R32_FLOAT, Format.D16_UNORM);
            _vp = new ViewPort(0, 0, size, size);
            Rendered = false;
        }      

        #region PROPERTYS

        [Category("ShadowMap")]
                
        public float Bias { get { return bias; } set { bias = value; } }

        [Category("ShadowMap")]
       
        [AssetMember(StoreType.Reference)]
        public Camera Camera
        {
            get { return camera; }
            set
            {
                if (camera != value)
                {
                    camera = value;
                    if (camera != null)
                        camera.CommitChanges();
                }
            }
        }

        [Category("ShadowMap")]
       
        public Texture2D DepthTexture
        {
            get { return ShadowMap.Texture; }
        }

        [Category("ShadowMap")]
               
        public bool IsDynamic { get { return isDynamic; } set { isDynamic = value; } }

        [Category("ShadowMap")]
       
        
                
        public int Size
        {
            get { return size; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("value must be > 0");
                if (size != value)
                {
                    size = value;
                    if (ShadowMap != null)
                        ShadowMap.Dispose();
                    ShadowMap = new RenderTexture2D(size, size, Format.R32_FLOAT, Format.D16_UNORM);
                    _vp = new ViewPort(0, 0, size, size);
                    Rendered = false;

                    GenerateSamples();
                }
            }
        }

        public Vector4[] TexCoordOffset { get { return _texCoordOffset; } }

        public float[] TexCoordWeights { get { return _texCoordWeights; } }

        public int KernelSize
        {
            get { return _kernelSize; }
            set 
            {
                _kernelSize = value;
                GenerateSamples();
            }
        }

        #endregion

        //private void GetFilterOffsets(int size)
        //{
        //    //float fTexelSize = 3.0f / (2.0f * (float)size);
        //    float fTexelSize = 1.0f / (float)size;
        //    // Generate the tecture co-ordinates for the specified depth-map size
        //    // 4 3 5
        //    // 1 0 2
        //    // 7 6 8
        //    offsets[0] = new Vector2(0, 0);
        //    offsets[1] = new Vector2(-fTexelSize, 0.0f);
        //    offsets[2] = new Vector2(fTexelSize, 0.0f);
        //    offsets[3] = new Vector2(0.0f, -fTexelSize);
        //    offsets[6] = new Vector2(0.0f, fTexelSize);
        //    offsets[4] = new Vector2(-fTexelSize, -fTexelSize);
        //    offsets[5] = new Vector2(fTexelSize, -fTexelSize);
        //    offsets[7] = new Vector2(-fTexelSize, fTexelSize);
        //    offsets[8] = new Vector2(fTexelSize, fTexelSize);
        //}      

        /// <summary>
        /// Draw the scene`s dephts from the camera to the DephtTexture
        /// </summary>
        /// <param name="data">Scene , the current Scene</param>
        public override void Apply()
        {
            if (EngineState.Shadow.Enable && (isDynamic || Rendered))
            {                
                var device = GraphicDeviceFactory.Device;
                Scene scene = Engine.Scene;
                var oldvp = device.ViewPort;

                device.ViewPort = _vp;
                device.SaveRenderTarget();

                ShadowMap.SetTarget(device);                
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color4.White, 1, 0);               
                
                var oldCamera = scene.ActiveCamera;
                device.Rasterizer = _rasterizer;
                device.Blend = _blendState;
                device.DepthTest = _detpthStencil;

                scene.ActiveCamera = camera;                
                RenderManager.ApplyTechnique<DepthSceneRender>();                

                scene.ActiveCamera = oldCamera;

                device.RestoreRenderTarget();

                device.ViewPort = oldvp;
                Rendered = true;               
            }
        }

        public void CommitChanges()
        {
            if (size != ShadowMap.Width)
            {
                if (ShadowMap != null)
                    ShadowMap.Dispose();
                ShadowMap = new RenderTexture2D(size, size, Format.R32_FLOAT, Format.D16_UNORM);
                Rendered = false;             
            }
        }

        public override void UpdatePose(Matrix affectorPose)
        {
            camera.Transform(GlobalToAffector * affectorPose);
        }
     
        protected override void OnDispose(bool d)
        {
            if(d)
                ShadowMap.Dispose();
            base.OnDispose(d);
        }

        private void GenerateSamples()
        {
            float texSize = 1.0f / (float)size;
            var smState = EngineState.Shadow.ShadowMapping;           
            int start = _kernelSize / -2;
            int end = _kernelSize / 2 + 1;
            float stdDv = smState.GaussianDeviation;
            int kernelSize2 = _kernelSize * _kernelSize;

            if (_texCoordOffset.Length != kernelSize2)
            {
                _texCoordOffset = new Vector4[kernelSize2];
                _texCoordWeights = new float[kernelSize2];
            }

            float dev2 = stdDv * stdDv;
            float g = 1.0f / (float)Math.Sqrt(Numerics.TwoPI * dev2);            

            int k = 0;
            for (int y = start; y < end; y++)
            {
                for (int x = start; x < end; x++)
                {
                    _texCoordOffset[k] = new Vector4((float)x * texSize, (float)y * texSize, 0 , 0);
                    _texCoordWeights[k] = smState.UseGaussianFilter ?
                                            g * (float)Math.Exp(-(x * x + y * y) / (2 * dev2)) :
                                            1.0f / (float)kernelSize2;

                    k++;
                }
            }            
        }

        //[Serializable]
        //class Activator : IResourceActivator
        //{
        //    int _size;
        //    float _bias;
        //    bool _dynamic;
        //    public void Initialize(IAssetProvider provider)
        //    {
        //        ShadowMapTechnique technique = (ShadowMapTechnique)provider;
        //        _size = technique.Size;
        //        _bias = technique.Bias;
        //        _dynamic = technique.IsDynamic;
        //    }

        //    public IAssetProvider OnCreateResource()
        //    {
        //        return new ShadowMapTechnique(_size) { IsDynamic = _dynamic };                
        //    }
        //}
    }

    public class StaticShadowMapTechnique : ShadowMapTechnique, INameable
    {
        private string _name;
        static int _counter = 0;

        public StaticShadowMapTechnique()
        {
            _name = "shadowMap" + _counter++;
        }

        public StaticShadowMapTechnique( int size)
            : base(size)
        {
            _name = "shadowMap" + _counter++;
        }

        [AssetMember]
        public string Name { get { return _name; } set { _name = value; } }

        public override void Bind(Render render)
        {
            render.Bind(this);
        }
               
    }   
    
}
