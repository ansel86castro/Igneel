using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Igneel;
using Igneel.Graphics;
using Igneel.Windows;
using Igneel.Windows.Forms;

namespace BasicEarth
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// The Sphere vertex definition. 
        /// Attributes are used for defined the 
        /// vertex shader input layout
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SphereVertex
        {
            [VertexElement(IASemantic.Position)]
            public Vector3 Position;

            [VertexElement(IASemantic.Normal)]
            public Vector3 Normal;

            [VertexElement(IASemantic.Tangent)]
            public Vector3 Tangent;

            [VertexElement(IASemantic.TextureCoordinate, 0)]
            public Vector2 TexCoord;
         
            public SphereVertex(Vector3 position = default(Vector3), 
                Vector3 normal = default(Vector3), 
                Vector3 tangent = default(Vector3), 
                Vector2 texCoord = default(Vector2))
            {
                Position = position;
                Normal = normal;
                Tangent = tangent;
                TexCoord = texCoord;               
            }            
        }

        /// <summary>
        /// Data Structure that defines a Directional Light. 
        /// The structure use 16 bytes padding for efficiently transfer the data to GPU memory
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DirectionalLight
        {
            /// <summary>
            /// Light's direction
            /// </summary>
            public Vector3 Direction;
            private float pad0;
            
            /// <summary>
            /// Light's color
            /// </summary>
            public Color3 Color;
            private float pad1;
        }

        /// <summary>
        /// Contract to access shader uniform variables and textures.      
        /// </summary>
        public interface ProgramMapping
        {
            float Time { get; set; }

            Matrix World { get; set; }

            Matrix View { get; set; }

            Matrix Projection { get; set; }

            DirectionalLight DirectionalLight { get; set; }

            Sampler<Texture2D> DiffuseTexture { get; set; }

            Sampler<Texture2D> NightTexture { get; set; }

            Sampler<Texture2D> NormalMapTexture { get; set; }

            Sampler<Texture2D> ReflectionMask { get; set; }

            Vector3 CameraPosition { get; set; }

            float ReflectionRatio { get; set; }

            float SpecularRatio { get; set; }

            float SpecularStyleLerp { get; set; }

            int SpecularPower { get; set; }
        }


        //The Graphic Device
        private GraphicDevice device;

        //Buffer for storing the mesh vertexes in GPU memory
        private GraphicBuffer vertexBuffer;

        //Buffer for storing the triangles indices in GPU memory
		private GraphicBuffer indexBuffer;

        //The shader program mapping
        ProgramMapping input;

        //The shader program
        ShaderProgram shaderProgram;                

        //Transformation matrices
        Matrix world;
        Matrix view;
        Matrix projection;

        //Texture sampling settings
        SamplerState diffuseSampler;
       
        //Textures
        Texture2D diffuseTexture;
        Texture2D nightTexture;
        Texture2D normalMapTexture;
        Texture2D reflectionMask;

        //Camera position
        private Vector3 cameraPosition = new Vector3(0, 10, -15);


        public Form1()
        {
            SetStyle(ControlStyles.Opaque, true);

            InitializeComponent();

            Init();            

            Application.Idle += (sender, args) =>
            {
                NativeMessage message;
                while (!Native.PeekMessage(out message, IntPtr.Zero, 0, 0, 0))
                {
                    RenderFrame();
                }
            };

          
        }           

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (device != null)
            {
                //resize the device back buffer after the form's size changed
                device.ResizeBackBuffer(Width, Height);

                //set the new render target viewport
                device.ViewPort = new ViewPort(0, 0, Width, Height);

                //create the projection matrix with the new aspect ratio
                projection = Matrix.PerspectiveFovLh((float)Width / (float)Height, Igneel.Numerics.PIover6, 1, 1000);
            }
        }


        private void Init()
        {

            //Setup shader model version and default compiling options, 
            //also set the relative directory where the shaders are located
            ShaderRepository.SetupD3D10_SM40("Shaders");

            //Create an instance of the GraphicDeviceFactory.
            //The GraphicDeviceFactory abstract class is used to creates GraphicDevices without worrying about the native implementation.
            //This sample use a Direc3D10 native implementation, therefore an instance of a GraphicManager10 is created
            GraphicDeviceFactory devFactory = new IgneelD3D10.GraphicManager10();

            //A GraphicDevice is created using a WindowContext containing rendering and display settings.           
            device = devFactory.CreateDevice(new WindowContext(Handle)
            {
                BackBufferWidth = Width,
                BackBufferHeight = Height,
                BackBufferFormat = Format.R8G8B8A8_UNORM,
                DepthStencilFormat = Format.D24_UNORM_S8_UINT,
                FullScreen = false,
                Sampling = new Multisampling(1, 0),
                Presentation = PresentionInterval.Default                 
            });

            //Create a ShaderProgram using the input layout definition provided by the SphereVertex struct
            //and the code for the vertex and pixel shaders located in the VertexShaderVS and PixelShaderPS files.
            //As a convention the last 2 characters in the filename specify the type of shader to load.
            shaderProgram = device.CreateProgram<SphereVertex>("VertexShaderVS", "PixelShaderPS");

            //Get a typed mapping using the ProgramMapping interface for the ShaderProgram uniform variables and textures
            input = shaderProgram.Map<ProgramMapping>();

            //The application blending state allowing transparency blend
            device.Blend = device.CreateBlendState(new BlendDesc(
                blendEnable: true, 
                srcBlend: Blend.SourceAlpha, 
                destBlend: Blend.InverseSourceAlpha));

            //The application depth testing state
            device.DepthTest = device.CreateDepthStencilState(new DepthStencilStateDesc(
                depthFunc: Comparison.Less));

            //The application rasterizer state
            device.Rasterizer = device.CreateRasterizerState(new RasterizerDesc(
                cull: CullMode.Back,
                fill: FillMode.Solid));

            //Default texture sampling settings
            diffuseSampler = device.CreateSamplerState(new SamplerDesc(
                addressU: TextureAddressMode.Wrap,
                addressV: TextureAddressMode.Wrap,
                filter: Filter.MinPointMagMipLinear));

            //Load the textures
            diffuseTexture = device.CreateTexture2DFromFile("Textures/Earth_Diffuse.dds");
            nightTexture = device.CreateTexture2DFromFile("Textures/Earth_Night.dds");
            normalMapTexture = device.CreateTexture2DFromFile("Textures/Earth_NormalMap.dds");
            reflectionMask = device.CreateTexture2DFromFile("Textures/Earth_ReflectionMask.dds");            

            //Create transformation matrices
            world = Matrix.Identity;
            view = Matrix.LookAt(cameraPosition, new Vector3(0, 0, 1), Vector3.UnitY);
            projection = Matrix.PerspectiveFovLh((float)Width / (float)Height, Igneel.Numerics.PIover6, 1, 1000);

            CreateSphere();
        }

        
        private void CreateSphere()
        {
            var stacks = 128;
            var slices = 128;
            var radius = 10;

            var vertices = new SphereVertex[(stacks - 1) * (slices + 1) + 2];
            var indices = new ushort[(stacks - 2) * slices * 6 + slices * 6];

            float phiStep = Numerics.PI / stacks;
            float thetaStep = Numerics.TwoPI / slices;

            // do not count the poles as rings
            int numRings = stacks - 1;

            // Compute vertices for each stack ring.
            int k = 0;
            var v = new SphereVertex();

            for (int i = 1; i <= numRings; ++i)
            {
                float phi = i * phiStep;

                // vertices of ring
                for (int j = 0; j <= slices; ++j)
                {
                    float theta = j * thetaStep;

                    // spherical to cartesian
                    v.Position = Vector3.SphericalToCartesian(phi, theta, radius);
                    v.Normal = Vector3.Normalize(v.Position);
                    v.TexCoord = new Vector2(theta / (-2.0f * (float)Math.PI), phi / (float)Math.PI);

                    // partial derivative of P with respect to theta
                    v.Tangent = new Vector3(-radius * (float)Math.Sin(phi) * (float)Math.Sin(theta), 0, radius * (float)Math.Sin(phi) * (float)Math.Cos(theta));

                    vertices[k++] = v;
                }
            }
            // poles: note that there will be texture coordinate distortion
            vertices[vertices.Length - 2] = new SphereVertex(new Vector3(0.0f, -radius, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), Vector3.Zero, new Vector2(0.0f, 1.0f));
            vertices[vertices.Length - 1] = new SphereVertex(new Vector3(0.0f, radius, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), Vector3.Zero, new Vector2(0.0f, 0.0f));

            int northPoleIndex = vertices.Length - 1;
            int southPoleIndex = vertices.Length - 2;

            int numRingVertices = slices + 1;

            // Compute indices for inner stacks (not connected to poles).
            k = 0;
            for (int i = 0; i < stacks - 2; ++i)
            {
                for (int j = 0; j < slices; ++j)
                {
                    indices[k++] = (ushort)((i + 1) * numRingVertices + j);                    
                    indices[k++] = (ushort)(i * numRingVertices + j + 1);
                    indices[k++] = (ushort)(i * numRingVertices + j);

                    indices[k++] = (ushort)((i + 1) * numRingVertices + j + 1);                    
                    indices[k++] = (ushort)(i * numRingVertices + j + 1);
                    indices[k++] = (ushort)((i + 1) * numRingVertices + j);
                }
            }

            // Compute indices for top stack.  The top stack was written 
            // first to the vertex buffer.
            for (int i = 0; i < slices; ++i)
            {
                indices[k++] = (ushort)i;
                indices[k++] = (ushort)(i + 1);                
                indices[k++] = (ushort)northPoleIndex;
            }

            // Compute indices for bottom stack.  The bottom stack was written
            // last to the vertex buffer, so we need to offset to the index
            // of first vertex in the last ring.
            int baseIndex = (numRings - 1) * numRingVertices;
            for (int i = 0; i < slices; ++i)
            {
                indices[k++] = (ushort)(baseIndex + i + 1);
                indices[k++] = (ushort)(baseIndex + i);
                indices[k++] = (ushort)southPoleIndex;                
            }

            vertexBuffer = device.CreateVertexBuffer(data: vertices);
            indexBuffer = device.CreateIndexBuffer(data:indices);
                                          
        }
                  

        private void RenderFrame()
        {
            //Set the render target and the depth stencil buffers
            //for rendering to the display just set the device default 
            //BackBuffer and BackDepthBuffer
            device.SetRenderTarget(device.BackBuffer, device.BackDepthBuffer);

            //Set the ViewPort to used by the device during the viewport tranformation
            device.ViewPort = new ViewPort(0, 0, Width, Height);

            //Clear the render target and depth stencil buffers
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, new Color4(0, 0, 0, 0), 1, 0);

            //Set the primitive type
            device.PrimitiveTopology = IAPrimitive.TriangleList;

            //Bind the vertex buffer to slot 0 at offset 0
            device.SetVertexBuffer(0, vertexBuffer, 0);

            //Set the index buffer
            device.SetIndexBuffer(indexBuffer);

          
            //Send the transformation matrices to the vertex shader
            input.World = Matrix.RotationY(-(float)Environment.TickCount / 5000.0f);
            input.View = view;
            input.Projection = projection;          

            //Send the light info and other values to the pixel shader
            input.CameraPosition = cameraPosition;
            input.ReflectionRatio = 0.05f;
            input.SpecularRatio = 0.15f;
            input.SpecularStyleLerp = 0.15f;
            input.SpecularPower = 8;
            input.DirectionalLight = new DirectionalLight
            {
                Color = Color3.White,
                Direction = new Euler(45, 0, 0).ToDirection()
            };

            //Bind a texture with a sampler state. As a convetion the SamplerState
            //in the shader must have the same name as the texture with 's' as prefix
            //for example in the shader the sampler state is declared
            //SamplerState sDiffuseTexture;
            input.DiffuseTexture = diffuseTexture.ToSampler(diffuseSampler);

            device.GetShaderStage<PixelShader>().SetResource(0, diffuseTexture);
            device.GetShaderStage<PixelShader>().SetSampler(0, diffuseSampler);

            //Bind textures with default sampler state (linear filtering and wrap TextureAddressMode).
            //these statements have the same behavior that calling nightTexture.ToSampler()
            input.NightTexture = nightTexture;
            input.NormalMapTexture = normalMapTexture;
            input.ReflectionMask = reflectionMask;
            

            //Set the shader program
            device.Program = shaderProgram;

            //Draw the geometry using the indices count, the start index an the vertex base offset 
            device.DrawIndexed((int)indexBuffer.SizeInBytes / indexBuffer.Stride, 0, 0);

            //Present the render target buffer to the display.
            device.Present();
        }
    }
}
