#pragma once
#include "EnumConverter.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;

namespace Igneel { namespace D3D9 {

		public ref class D3D9GraphicDevice : public GraphicDevice
		{		
			bool begin;
			D3DPRESENT_PARAMETERS* _pp;
		internal:
			IDirect3DDevice9 * _pDevice;
			
			event Action<D3D9GraphicDevice^>^ Reset;

			event Action<D3D9GraphicDevice^>^ Lost;

			void ResetDevice();
		public:
			D3D9GraphicDevice(GraphicDeviceDesc^ desc);

		protected:		
			OVERRIDE( void InitializeGraphicDevice(GraphicDeviceDesc^ desc));
	
			OVERRIDE( void SOSetTargetImpl(int slot, GraphicBuffer^ buffer, int offset));

			OVERRIDE(void OnDispose(bool));

		public:
			OVERRIDE (SamplerState^ CreateSamplerState(SamplerDesc desc));

			OVERRIDE (Texture1D^ CreateTexture1D(Texture1DDesc^ desc));

			OVERRIDE (Texture2D^ CreateTexture2D(Texture2DDesc^ desc));

			OVERRIDE (Texture3D^ CreateTexture3D(Texture3DDesc^ desc));

			OVERRIDE (TextureCube^ CreateTextureCube(TextureCubeDesc^ desc));
		
			OVERRIDE (Texture^ CreateTextureFromFile(ResourceType type, String^ filename));

			OVERRIDE( void Clear(ClearFlags flags, int color, float depth, int stencil) );

			OVERRIDE( void Draw(int startVertexIndex, int primitiveCount) );

			OVERRIDE( void DrawIndexed(int baseVertexIndex, int minVertexIndex , int numVertices, int startIndex, int primitiveCount) );

			OVERRIDE( void DrawUser(int primitiveCount, void * vertices , int vertexStride ) );

			OVERRIDE( void DrawIndexedUser(int minVertexIndex, int numVertices, int primitiveCount , void* indices, IndexFormat format, void* vertices, int vertexStride) );		

			OVERRIDE( void BeginRender());

			OVERRIDE( void Present());

			OVERRIDE( void EndRender());

			OVERRIDE( void ResizeBackBuffer(int width, int height));

			OVERRIDE( void UpdateSubResource(SubResource^ src, SubResource^ dest));

			//*******************Input Assembler Methods*****************************************

		protected:
			OVERRIDE(void InitializeInputAssemblerStage());

			OVERRIDE(void IASetInputLayout(InputLayout^ value));

			OVERRIDE(void IASetPrimitiveType(IAPrimitive value));

			OVERRIDE(void IASetVertexBufferImpl(int slot, GraphicBuffer^ vertexBuffer, int offset, int stride));

			OVERRIDE(void IASetIndexBufferImpl(GraphicBuffer^ indexBuffer, int offset));

		public:
			 OVERRIDE(InputLayout^ CreateInputLayout(array<VertexElement>^ elements ,DataBuffer^ signature));

			 OVERRIDE(GraphicBuffer^ CreateVertexBuffer(int size, int stride,  ResourceUsage usage, MapType cpuAcces, Array^ data));

			 OVERRIDE(GraphicBuffer^ CreateIndexBuffer(int size,  IndexFormat format , ResourceUsage usage, MapType cpuAcces, Array^ data));

			/* OVERRIDE(GraphicBuffer^ CreateConstantBuffer(long size, Array^ data));*/			


			//***************** OUTPUT MERGE STAGE *************************************************

		protected:
			OVERRIDE( void InitializeOMStage());

			OVERRIDE( void OMSetBlendState() );

			OVERRIDE( void OMSetDepthStencilState() );		

			OVERRIDE(  void OMSetDepthStencil(DepthStencil^ stencil) );

			OVERRIDE( SwapChain^ CreateSwapChainImp(SwapChainDesc desc) );

		public:
			OVERRIDE( RenderTarget^ CreateRenderTarget(int width, int height , Format format, Multisampling sampling, bool cpuReadEnable) );

			OVERRIDE( RenderTarget^ CreateRenderTarget(SubResource^ subResource, Multisampling sampling, bool cpuReadEnable ) );			

			OVERRIDE( DepthStencil^ CreateDepthStencil(int width, int height, Format format, Multisampling sampling, bool cpuReadEnable) );

			OVERRIDE( DepthStencil^ CreateDepthStencil(SubResource^ subResource) );

			OVERRIDE( BlendState^ CreateBlendState(BlendDesc desc) );

			OVERRIDE( DepthStencilState^ CreateDepthStencilState(DepthStencilDesc desc));

			OVERRIDE( void OMSetRenderTargetsImp(int numTargets, array<RenderTarget^>^ renderTargets, DepthStencil^ dephtStencil) );

			OVERRIDE( void OMSetRenderTargetImpl(int slot, RenderTarget^ renderTarget, DepthStencil^ dephtStencil));						

			//************** Rasterizer Stage *****************************************************

		protected:
			OVERRIDE( void InitializeRasterizerStage() );

			OVERRIDE( void RSSetState() );

			OVERRIDE( void RSSetViewPort() );

			OVERRIDE( void RSSetScissorRects() );

		public:
			OVERRIDE( RasterizerState^ CreateRasterizerState(RasterizerDesc desc) );		


			//******************* Shader Stage ***************************************************
		protected:
			OVERRIDE( void InitializeShaders() );

			OVERRIDE( void VSSetSamplerImpl(int slot, SamplerState^ state) );

			OVERRIDE( void PSSetSamplerImpl(int slot, SamplerState^ state) );
		
			OVERRIDE( void SetTextureImp(int register, Texture^ texture) );

			OVERRIDE( void UseProgram(ShaderProgram^ program) );

			OVERRIDE( void ClearProgram(ShaderProgram^ program) );

		public:
			OVERRIDE( ShaderProgram^ CreateProgram(ShaderProgramDesc^ desc) );

		};		

		//public ref class DeviceUT
		//{
		//public:
		//	DeviceUT(void);
		//
		//	static void SetVertexBufferData(IntPtr comVBPter, void * data, int offset, int size, int lockFlags)
//		//	{
		//		IDirect3DVertexBuffer9* pVb =static_cast<IDirect3DVertexBuffer9*>(comVBPter.ToPointer());
		//		void* pvbData;
		//		pVb->Lock(offset, size, &pvbData, lockFlags);
		//
		//		memcpy(pvbData, data, size);
		//
		//		pVb->Unlock();
		//	}
		//};

	}
}
