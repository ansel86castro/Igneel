using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel.Graphics;
using System.Runtime.Serialization;
using System.ComponentModel;
using Igneel.Assets;
using Igneel.Rendering;
using Igneel.Scenering;


namespace Igneel.Rendering
{
    public enum ShadowMapType { Simple, Cube }
   
   
    [ProviderActivator(typeof(ShadowMapTechnique.Activator))]
    public class ShadowMapTechnique : BindedSceneNodeTechnique<ShadowMapTechnique> ,IDeferreable
    {
        internal Camera camera;
        internal int size;
        internal bool isDynamic;        
        internal RenderTexture2D shadowMap;
        internal bool rendered;      
        internal float bias;
        Vector4[] texCoordOffset;
        float[] texCoordWeights;
        ViewPort vp;
        int kernelSize = 3;
        static RasterizerState rasterizer;
        static DepthStencilState detpthStencil;
        static BlendState blendState;
      
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
            if (blendState == null)
            {
                blendState = SceneTechnique.NoBlend;
                rasterizer = SceneTechnique.NoCulling;
                detpthStencil = SceneTechnique.DephtState;
            }

            texCoordOffset = new Vector4[EngineState.Shadow.ShadowMapping.PCFBlurSize * EngineState.Shadow.ShadowMapping.PCFBlurSize];
            texCoordWeights = new float[EngineState.Shadow.ShadowMapping.PCFBlurSize * EngineState.Shadow.ShadowMapping.PCFBlurSize];

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

            shadowMap = new RenderTexture2D(size, size, Format.R32_FLOAT, Format.D16_UNORM);
            vp = new ViewPort(0, 0, size, size);
            rendered = false;
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
       
        public Texture DepthTexture
        {
            get { return shadowMap.Texture; }
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
                    if (shadowMap != null)
                        shadowMap.Dispose();
                    shadowMap = new RenderTexture2D(size, size, Format.R32_FLOAT, Format.D16_UNORM);
                    vp = new ViewPort(0, 0, size, size);
                    rendered = false;

                    GenerateSamples();
                }
            }
        }

        public Vector4[] TexCoordOffset { get { return texCoordOffset; } }

        public float[] TexCoordWeights { get { return texCoordWeights; } }

        public int KernelSize
        {
            get { return kernelSize; }
            set 
            {
                kernelSize = value;
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
            if (EngineState.Shadow.Enable && (isDynamic || rendered))
            {                
                var device = GraphicDeviceFactory.Device;
                Scene scene = SceneManager.Scene;
                var oldvp = device.ViewPort;

                device.ViewPort = vp;
                device.SaveRenderTarget();

                shadowMap.SetTarget(device);                
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color4.White, 1, 0);               
                
                var oldCamera = scene.ActiveCamera;
                device.Rasterizer = rasterizer;
                device.Blend = blendState;
                device.DepthTest = detpthStencil;

                scene.ActiveCamera = camera;                
                RenderManager.ApplyTechnique<DepthSceneRender>();                

                scene.ActiveCamera = oldCamera;

                device.RestoreRenderTarget();

                device.ViewPort = oldvp;
                rendered = true;               
            }
        }

        public void CommitChanges()
        {
            if (size != shadowMap.Width)
            {
                if (shadowMap != null)
                    shadowMap.Dispose();
                shadowMap = new RenderTexture2D(size, size, Format.R32_FLOAT, Format.D16_UNORM);
                rendered = false;             
            }
        }

        public override void UpdatePose(Matrix affectorPose)
        {
            camera.Transform(globalToAffector * affectorPose);
        }
     
        protected override void OnDispose(bool d)
        {
            if(d)
                shadowMap.Dispose();
            base.OnDispose(d);
        }

        private void GenerateSamples()
        {
            float texSize = 1.0f / (float)size;
            var smState = EngineState.Shadow.ShadowMapping;           
            int start = kernelSize / -2;
            int end = kernelSize / 2 + 1;
            float stdDv = smState.GaussianDeviation;
            int kernelSize2 = kernelSize * kernelSize;

            if (texCoordOffset.Length != kernelSize2)
            {
                texCoordOffset = new Vector4[kernelSize2];
                texCoordWeights = new float[kernelSize2];
            }

            float dev2 = stdDv * stdDv;
            float g = 1.0f / (float)Math.Sqrt(Numerics.TwoPI * dev2);            

            int k = 0;
            for (int y = start; y < end; y++)
            {
                for (int x = start; x < end; x++)
                {
                    texCoordOffset[k] = new Vector4((float)x * texSize, (float)y * texSize, 0 , 0);
                    texCoordWeights[k] = smState.UseGaussianFilter ?
                                            g * (float)Math.Exp(-(x * x + y * y) / (2 * dev2)) :
                                            1.0f / (float)kernelSize2;

                    k++;
                }
            }            
        }

        [Serializable]
        class Activator : IProviderActivator
        {
            int size;
            float bias;
            bool dynamic;
            public void Initialize(IAssetProvider provider)
            {
                ShadowMapTechnique technique = (ShadowMapTechnique)provider;
                size = technique.Size;
                bias = technique.Bias;
                dynamic = technique.IsDynamic;
            }

            public IAssetProvider CreateInstance()
            {
                return new ShadowMapTechnique(size) { IsDynamic = dynamic };                
            }
        }
    }

    public class StaticShadowMapTechnique : ShadowMapTechnique, INameable
    {
        private string name;
        static int counter = 0;

        public StaticShadowMapTechnique()
        {
            name = "shadowMap" + counter++;
        }

        public StaticShadowMapTechnique( int size)
            : base(size)
        {
            name = "shadowMap" + counter++;
        }

        [AssetMember]
        public string Name { get { return name; } set { name = value; } }

        public override void Bind(Render render)
        {
            render.Bind(this);
        }
               
    }   
    
}
