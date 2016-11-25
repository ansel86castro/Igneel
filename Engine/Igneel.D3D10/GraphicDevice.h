#pragma once
#include "ConstantBuffer.h"

using namespace System;
using namespace Igneel::Graphics;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace System::IO;

namespace IgneelD3D10
{	
	ref class CbHandle;
	public ref class GraphicDevice10: public GraphicDevice
	{
	internal:
		ID3D10Device * _device;
		IDXGISwapChain * _swapChain;
		int _primitiveVertex;
		List<CbHandle^>^ OpenConstantBuffers;

		public:
			GraphicDevice10(GraphicDeviceDesc^ desc);		

	private:
			void CloseCBuffers();

		protected:		
			OVERRIDE( DeviceInfo InitDevice(GraphicDeviceDesc^ desc));	

			OVERRIDE(void OnDispose(bool));			

		public:
			void GraphicDevice10::CreateDepthStencil(ID3D10Device * device, IDXGISwapChain* swapChain , OUT ID3D10Texture2D** dephtTexture,  OUT ID3D10DepthStencilView** depthStencilView );

			OVERRIDE(bool CheckFormatSupport(Format format, BindFlags binding, ResourceType type));		

			OVERRIDE(int CheckMultisampleQualityLevels(Format format, int multySampleCount , bool windowed));

			OVERRIDE (SamplerState^ CreateSamplerState(SamplerDesc desc));

			OVERRIDE (Texture1D^ CreateTexture1D(Texture1DDesc desc , array<IntPtr>^data ));

			OVERRIDE (Texture2D^ CreateTexture2D(Texture2DDesc desc, array<MappedTexture2D>^data));

			OVERRIDE (Texture3D^ CreateTexture3D(Texture3DDesc desc, array<MappedTexture3D>^data));			
		
			Texture^ CreateTextureFromFile(ResourceType type, String^ filename);

			Texture^ CreateTextureFromStream(ResourceType type, Stream^ stream, String^ location);

			Texture^ CreateTextureFromFileTga(ResourceType type, String^ filename);

			OVERRIDE (Texture1D^ CreateTexture1DFromFile(String^ filename));

			OVERRIDE (Texture1D^ CreateTexture1DFromStream(Stream^ stream));

			OVERRIDE (Texture2D^ CreateTexture2DFromFile(String^ filename));

			OVERRIDE (Texture2D^ CreateTexture2DFromStream(Stream^ stream));			

			OVERRIDE (Texture3D^ CreateTexture3DFromFile(String^ filename));

			OVERRIDE (Texture3D^ CreateTexture3DFromStream(Stream^ stream));

			OVERRIDE(void GenerateMips(Texture^ texture));

			OVERRIDE( void Clear(ClearFlags flags, Color4 color, float depth, int stencil) );

			OVERRIDE( void Draw(int vertexCount, int startVertexIndex) );

			OVERRIDE( void DrawIndexed(int indexCount, int startIndex, int baseVertexIndex) );

			OVERRIDE( void DrawAuto() );			
		
			OVERRIDE( void ResizeBackBuffer(int width, int height));
			
			OVERRIDE( void ResolveSubresource(Texture^ src, int srcSub, Texture^ desc, int destSub) );

			OVERRIDE( void CopySubResource(Texture^ destTexture, int destSubRes, int dstX , int dstY, int dstZ, Texture^ srcTexture, int srcSubRes, DataBox destBox));

			OVERRIDE( void CopyTexture(Texture^ src, Texture^ desc) );

	protected:
			OVERRIDE( void UpdateSubResource(Texture^ dest, int subResource, DataBox* box, void* srcPointer, int srcRowPith, int srcDepthPitch));

			//*******************Input Assembler Methods*****************************************

		protected:
			OVERRIDE(IAInitialization GetIAInitialization());

			OVERRIDE(void IASetInputLayout(InputLayout^ value));

			OVERRIDE(void IASetPrimitiveType(IAPrimitive value));

			OVERRIDE(void IASetVertexBufferImpl(int slot, GraphicBuffer^ vertexBuffer, int offset, int stride));

			OVERRIDE(void IASetIndexBufferImpl(GraphicBuffer^ indexBuffer, int offset));

		public:
			 OVERRIDE(InputLayout^ CreateInputLayout(array<VertexElement>^ elements ,ShaderCode^ signature));

			 OVERRIDE(GraphicBuffer^ CreateVertexBuffer(int size, int stride,  ResourceUsage usage, CpuAccessFlags cpuAcces, ResourceBinding binding,IntPtr data));

			 OVERRIDE(GraphicBuffer^ CreateIndexBuffer(int size,  IndexFormat format , ResourceUsage usage, CpuAccessFlags cpuAcces, IntPtr data));		

			 OVERRIDE(void UpdateBuffer(GraphicBuffer^ buffer,int offset, IntPtr pterData, int dataSize));

			//***************** OUTPUT MERGE STAGE *************************************************

		protected:
			OVERRIDE( OMInitialization GetOMInitialization());

			OVERRIDE( void OMSetBlendState(BlendState^ state) );

			OVERRIDE( void OMSetDepthStencilState(DepthStencilState^ state) );			

			OVERRIDE( SwapChain^ CreateSwapChainImp(IGraphicContext^ context) );

		public:
			OVERRIDE( RenderTarget^ CreateRenderTarget(Texture2D^ texture, int subResource , int count) );			

			OVERRIDE( DepthStencil^ CreateDepthStencil(DepthStencilDesc^ desc) );			

			OVERRIDE( BlendState^ CreateBlendState(BlendDesc desc) );

			OVERRIDE( DepthStencilState^ CreateDepthStencilState(DepthStencilStateDesc desc));

			OVERRIDE( void OMSetRenderTargetsImp(int numTargets, array<RenderTarget^>^ renderTargets, DepthStencil^ dephtStencil) );

			OVERRIDE( void OMSetRenderTargetImpl(RenderTarget^ renderTarget, DepthStencil^ dephtStencil));						

			//************** Rasterizer Stage *****************************************************

		protected:
			OVERRIDE( RSInitialization GetRSInitialization() );

			OVERRIDE( void RSSetState(RasterizerState^ value) );

			OVERRIDE( void RSSetViewPort(Graphics::ViewPort vp) );

			OVERRIDE( void RSSetScissorRects(Igneel::Rectangle rec) );

		public:
			OVERRIDE( RasterizerState^ CreateRasterizerState(RasterizerDesc desc) );		


			//******************* Shader Stage ***************************************************
		protected:
			OVERRIDE( ShadingInitialization GetShadingInitialization() );			

			OVERRIDE( void SetProgram(ShaderProgram^ program) );

			void ClearProgram(ShaderProgram^ program); 

		public:
			OVERRIDE( ShaderProgram^ CreateProgram(ShaderProgramDesc^ desc) );

			//OVERRIDE( ShaderBuffer^ CreateShaderBuffer(BufferDesc desc));			

	};

	
}