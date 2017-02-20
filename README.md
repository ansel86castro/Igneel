# Igneel Engine
Game engine for .NET 

In order to compile the code you must download and install the DirectXSDK at https://www.microsoft.com/en-us/download/details.aspx?id=6812

Additional SDKs
Nvidia PhysX runtime: http://www.nvidia.com/object/physx-9.16.0318-driver.html


#Using the Graphics API

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
				//Setup the shader model and directory
				ShaderRepository.SetupD3D10_SM40("Shaders");

				//Use Direct3D10 by creating the corresponding GraphicDeviceFactory
				var devFactory = new IgneelD3D10.GraphicManager10();

				//Use the GraphicDeviceFactory to create the CreateDevice passing the WindowContext
				//with presentation settings

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


				//Creates a shader program holding the code for the vertex and pixel shader
				//specify the IA input layout by using the vertex struct
				shaderProgram = device.CreateProgram<TriangleVertex>("VertexShaderVS", "PixelShaderPS");

				//Get the shader program input mapping. Use this mapping for sending app values to the shaders
				input = shaderProgram.Map<ProgramMapping>();

				//Create and set default pipeline states
				//then your are ready to render				
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


				//Create the vertex buffer specifing the data array
				vertexBuffer = device.CreateVertexBuffer(data: data);

				//Create the scene transforms
				world = Matrix.Identity;
				view = Matrix.LookAt(new Vector3(0, 0, -1), new Vector3(0, 0, 1), Vector3.UnitY);
				projection = Matrix.PerspectiveFovLh((float)Width / (float)Height, Igneel.Numerics.PIover6, 1, 1000);            
			}        

		   
			//This method is where rendering takes places. Its executed when the app is no processing any message
			private void RenderFrame()
			{
				//Set render target and the depth stencil buffers
				//In order to render to the screen just use the device BackBuffer and BackDepthBuffer
				//you can specify order render targets to render to textures for example
				device.SetRenderTarget(device.BackBuffer, device.BackDepthBuffer);

				//Specify the render viewport

				device.ViewPort = new ViewPort(0, 0, Width, Height);

				//Clear the render target and depth stencil buffers
				device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, new Color4(0, 0, 0, 0), 1, 0);
				
				//Set the primitive type and vertex buffer
				device.PrimitiveTopology = IAPrimitive.TriangleList;
				device.SetVertexBuffer(0, vertexBuffer, 0);

				//Set the shader program
				device.Program = shaderProgram;
				
				//Send the transforms to the shader using the input mapping

				input.WorldViewProj = world * view * projection;

				//Draw the geometry
				device.Draw(3, 0);

				//finally present the render target to the screen.
				//This will call the default SwapChain.Present() 

				device.Present();
			}

		  
		}
	

