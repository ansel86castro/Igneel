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

namespace ColorTraingle
{
    public partial class Form1 : Form
    {
        [StructLayout(LayoutKind.Sequential)]        
        public struct TriangleVertex
        {
            [VertexElement(IASemantic.Position)]
            public Vector3 Position;

            [VertexElement(IASemantic.Color)]
            public Color4 Color;

            public TriangleVertex(Vector3 Position, Color4 color)
            {
                this.Position = Position;
                this.Color = color;
            }
        }

        public interface ProgramMapping
        {
            Matrix WorldViewProj { get; set; }
        }


        GraphicDevice device;

        GraphicBuffer vertexBuffer;

        ShaderProgram shaderProgram;

        //The shader program mapping
        ProgramMapping input;

        //Transforms
        Matrix world;
        Matrix view;
        Matrix projection;

        //pipeline states
        private RasterizerState cullingState;
        private BlendState blendState;
        private DepthStencilState depthStenciState;

        public Form1()
        {
            SetStyle(ControlStyles.Opaque, true);

            InitializeComponent();                                

            CreateDevice();

            CreateTriangle();

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

            device.ResizeBackBuffer(Width, Height);
            device.ViewPort = new ViewPort(0, 0, Width, Height);
            projection = Matrix.PerspectiveFovLh((float)Width / (float)Height, Igneel.Numerics.PIover6, 1, 1000);
        }

       
        private void CreateDevice()
        {
            
            //Tell the API were the folder containing the shader's code are ,and the ShaderModel
            ShaderRepository.SetupD3D10_SM40("Shaders");

            //Instantiate GraphicDeviceFactory in this case we are using Direct3D10 so an instance of GraphicManager10 is required
            var devFactory = new IgneelD3D10.GraphicManager10();

            //Create de GraphicDevice passsing a GraphicDeviceDesc containing device configuration and 
            //and the rendering environment. In this case we use the WindowContext because this is a WindowsForms application
            device = devFactory.CreateDevice(new GraphicDeviceDesc(new WindowContext(Handle)
                {
                    BackBufferWidth = Width,
                    BackBufferHeight = Height,
                    BackBufferFormat = Format.R8G8B8A8_UNORM_SRGB,
                    DepthStencilFormat = Format.D24_UNORM_S8_UINT,
                    FullScreen = false,
                    Sampling = new Multisampling(1, 0),
                    Presentation = PresentionInterval.Default
                }
            ));

            //Creates a ShaderProgram passsing the files containing the shader's code for the VertexShader and PixelShader stages
            shaderProgram = device.CreateProgram<TriangleVertex>("VertexShaderVS", "PixelShaderPS");

            //Retrieve a type safe mapping for setting the uniforms variables
            input = shaderProgram.Map<ProgramMapping>();          

            //Set pipeline states
            blendState = device.CreateBlendState(new BlendDesc(blendEnable: true, srcBlend: Blend.SourceAlpha, destBlend: Blend.InverseSourceAlpha));
            depthStenciState = device.CreateDepthStencilState(new DepthStencilStateDesc(depthFunc: Comparison.Less));
            cullingState = device.CreateRasterizerState(new RasterizerDesc(cull: CullMode.None));

            device.Blend = blendState;
            device.Rasterizer = cullingState;
            device.DepthTest = depthStenciState;

        }


        private void CreateTriangle()
        {
            //Creates a triangle with position and color data
            var data = new TriangleVertex[]{
                new TriangleVertex(new Vector3(0, 1, 0), Color4.Blue),
                new TriangleVertex(new Vector3(1, -1, 0), Color4.Green),                
                new TriangleVertex(new Vector3(-1, -1, 0), Color4.Red)
            };

            //Create the vertex buffer for olding the data in the device, passing data will feed the vertex buffer
            //with the provided array
            vertexBuffer = device.CreateVertexBuffer(data: data);

            //Create transformation matrices
            world = Matrix.Identity;
            view = Matrix.LookAt(new Vector3(0, 0, -1), new Vector3(0, 0, 1), Vector3.UnitY);
            projection = Matrix.PerspectiveFovLh((float)Width / (float)Height, Igneel.Numerics.PIover6, 1, 1000);            
        }        

       

        private void RenderFrame()
        {
            //Set the defaul render target and the depth stencil buffers
            device.SetRenderTarget(device.BackBuffer, device.BackDepthBuffer);

            //Specify the ViewPort
            device.ViewPort = new ViewPort(0, 0, Width, Height);

            //Clear the render target and depth stencil buffers
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, new Color4(0, 0, 0, 0), 1, 0);
            

            //Set the primitive type and vertex buffer
            device.PrimitiveTopology = IAPrimitive.TriangleList;
            device.SetVertexBuffer(0, vertexBuffer, 0);

            //Set the shader program to use
            device.Program = shaderProgram;
            
            //Pass the tranformation values to the shader code
            input.WorldViewProj = world * view * projection;

            //Draw the geometry
            device.Draw(3, 0);

            //Present the render target buffer to the screen
            device.Present();
        }

      
    }
}
