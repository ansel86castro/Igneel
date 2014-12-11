#include "stdafx.h"
#include "GraphicDevice.h"
#include "EnumConverter.h"
#include "SampleState.h"
#include "D3D9Textures.h"
#include "GraphicDeviceManager.h"
#include "D3DSwapChain.h"
#include "SubResource.h"

namespace Igneel {
	namespace D3D9 {	
	
		D3D9GraphicDevice::D3D9GraphicDevice(GraphicDeviceDesc^ desc) : GraphicDevice(desc)
		{

		}

		void D3D9GraphicDevice::InitializeGraphicDevice(GraphicDeviceDesc^ desc)
		{
			D3DGrahicDeviceManager^ manager = static_cast<D3DGrahicDeviceManager^>(GraphicManager::Manager);
			IDirect3D9* pD3D = manager->_pD3D;

			D3DDISPLAYMODE dm;				
			D3DFORMAT backBufferformat = GetD3DFORMAT(desc->BackBufferFormat);
			D3DDEVTYPE devType;
			DWORD createFlags = 0;
			pD3D->GetAdapterDisplayMode(desc->Adapter, &dm);				

			_deviceInfo->DisplayFormat = GetFORMAT(dm.Format);
			_deviceInfo->DisplayWidth = dm.Width;
			_deviceInfo->DisplayHeight = dm.Height;
			_deviceInfo->RefreshRate = dm.RefreshRate;			
			
			if(SUCCEEDED(pD3D->CheckDeviceType(desc->Adapter, GetD3DDEVTYPE(desc->DriverType) ,dm.Format, backBufferformat, desc->FullScreen)))
			{
				devType = D3DDEVTYPE_HAL;
				_deviceInfo->DriverType = Igneel::Graphics::GraphicDeviceType::Hardware;
			}
			else
			{
				devType = D3DDEVTYPE_REF;
				_deviceInfo->DriverType = Igneel::Graphics::GraphicDeviceType::Reference;
			}

			DWORD msaaQuality;
			if(SUCCEEDED(pD3D->CheckDeviceMultiSampleType(desc->Adapter, devType, backBufferformat, desc->FullScreen, static_cast<D3DMULTISAMPLE_TYPE>(desc->MSAA.Count),  &msaaQuality )))
			{
				desc->MSAA.Quality = msaaQuality - 1;
				_deviceInfo->MSAA = desc->MSAA;
			}
			else
			{
				_deviceInfo->MSAA = Multisampling::Disable;
			}

			D3DCAPS9 caps;
			if(SUCCEEDED(pD3D->GetDeviceCaps(desc->Adapter, devType, &caps)))
			{
				_deviceInfo->MaxSimultaneousTextures = caps.MaxSimultaneousTextures;
				_deviceInfo->MaxStreams = caps.MaxStreams;
				_deviceInfo->SimultaneousRTCount = caps.NumSimultaneousRTs;
				_deviceInfo->MaxTextureWidth = caps.MaxTextureWidth;
				_deviceInfo->MaxTextureHeight = caps.MaxTextureHeight;

				if(caps.DevCaps & D3DDEVCAPS_HWTRANSFORMANDLIGHT)
				{
					createFlags |= D3DCREATE_HARDWARE_VERTEXPROCESSING;
				}
				else
				{
					createFlags |= D3DCREATE_SOFTWARE_VERTEXPROCESSING;
				}
			}
			else throw gcnew GraphicDeviceFailException(L"Could not retrieve Device Capabilities");

			D3DADAPTER_IDENTIFIER9 ai;
			if(SUCCEEDED(pD3D->GetAdapterIdentifier(desc->Adapter ,0, &ai)))
			{
				_deviceInfo->DeviceId = ai.DeviceId;
				_deviceInfo->DeviceName = System::Runtime::InteropServices::Marshal::PtrToStringAnsi(static_cast<IntPtr>(ai.DeviceName), strlen(ai.DeviceName));

			}else throw gcnew GraphicDeviceFailException(L"Could not retrieve Adapter Information");

			D3DPRESENT_PARAMETERS pp;
			pp.BackBufferFormat = backBufferformat;			
			pp.BackBufferCount = 1;
			pp.BackBufferWidth = desc->BackBufferWidth;
			pp.BackBufferHeight = desc->BackBufferHeight;

			pp.MultiSampleType =  static_cast<D3DMULTISAMPLE_TYPE>(_deviceInfo->MSAA.Count);
			pp.MultiSampleQuality = _deviceInfo->MSAA.Quality;

			pp.AutoDepthStencilFormat = GetD3DFORMAT(desc->DepthStencilFormat);
			pp.EnableAutoDepthStencil = TRUE;
			pp.PresentationInterval = static_cast<UINT>(desc->Interval);
			pp.Flags = D3DPRESENTFLAG_DISCARD_DEPTHSTENCIL;
			pp.SwapEffect = D3DSWAPEFFECT_DISCARD;
			pp.Windowed = desc->FullScreen? FALSE: TRUE;
			pp.FullScreen_RefreshRateInHz = pp.Windowed? 0 : Info->RefreshRate;
			pp.hDeviceWindow = (HWND) desc->WindowsHandle.ToPointer();			
			
			IDirect3DDevice9 * device; 
			auto hr = pD3D->CreateDevice(0, devType, (HWND)desc->WindowsHandle.ToPointer(),  createFlags, &pp, &device);
			auto lost = D3DERR_DEVICELOST;
			auto inv = D3DERR_INVALIDCALL;
			auto invd = D3DERR_INVALIDDEVICE;
			if(FAILED(hr))
				throw gcnew InvalidOperationException();
			_pDevice = device;		

			SupportStreamOutput = false;
			_soTargetBind = gcnew array<BufferBind>(0);

			_fullScreen = desc->FullScreen;
			_pp = new D3DPRESENT_PARAMETERS();
			*_pp = pp;

			GC::AddMemoryPressure(sizeof(IDirect3DDevice9));
			GC::AddMemoryPressure(sizeof(D3DPRESENT_PARAMETERS));
		}

		void D3D9GraphicDevice::OnDispose(bool disposing)
		{						
			__super::OnDispose(disposing);
			
		/*	auto vs = GetShader<VertexShader^>();
			if(vs!=nullptr)vs->~VertexShader();

			auto ps = GetShader<PixelShader^>();
			if(ps!=nullptr)ps->~PixelShader();*/

			_pDevice->Release();
		}

		void D3D9GraphicDevice::SOSetTargetImpl(int slot, GraphicBuffer^ buffer, int offset)
		{
			throw gcnew CapabiityNotSupportedException(L"This implementation on Direct3D9 doesn't support stream output capabilities");
		}

		SamplerState^ D3D9GraphicDevice::CreateSamplerState(SamplerDesc desc)
		{
			return gcnew D3D9SampleState(desc);
		}

		Texture1D^ D3D9GraphicDevice::CreateTexture1D(Texture1DDesc^ desc)
		{
			return gcnew D3D9Texture1D(_pDevice, desc);
		}

		Texture2D^ D3D9GraphicDevice::CreateTexture2D(Texture2DDesc^ desc)
		{
			return gcnew D3D9Texture2D(_pDevice, desc);
		}

		Texture3D^ D3D9GraphicDevice::CreateTexture3D(Texture3DDesc^ desc)
		{
			return gcnew D3D9Texture3D(_pDevice, desc);
		}

		TextureCube^ D3D9GraphicDevice::CreateTextureCube(TextureCubeDesc^ desc)
		{
			return gcnew D3D9TextureCube(_pDevice, desc);
		}

		Texture^ D3D9GraphicDevice::CreateTextureFromFile(ResourceType type, String^ filename)
		{			
			LPCWSTR pter =  (LPCWSTR)Marshal::StringToHGlobalUni(filename).ToPointer();

			IDirect3DTexture9 * ptex;
			IDirect3DVolumeTexture9 * ptexV;
			IDirect3DCubeTexture9 * ptexC;
			Texture^ texture;

			try
			{
				switch (type)
				{			
				case Igneel::Graphics::ResourceType::Texture1D:
					{
						SAFECALL(D3DXCreateTextureFromFile(_pDevice, pter, &ptex));
						texture =  gcnew D3D9Texture1D(ptex);				
					}
					break;
				case Igneel::Graphics::ResourceType::Texture2D:
					{
						SAFECALL(D3DXCreateTextureFromFile(_pDevice, pter, &ptex));
						texture = gcnew D3D9Texture2D(ptex);				
					}
					break;
				case Igneel::Graphics::ResourceType::Texture3D:
					{
						SAFECALL(D3DXCreateVolumeTextureFromFile(_pDevice, pter, &ptexV));
						texture = gcnew D3D9Texture3D(ptexV);				
					}
					break;
				case Igneel::Graphics::ResourceType::TextureCube:
					{
						SAFECALL(D3DXCreateCubeTextureFromFile(_pDevice, pter, &ptexC));
						texture = gcnew D3D9TextureCube(ptexC);				
					}
					break;			
				}			
				
			}
			finally
			{
				Marshal::FreeHGlobal(IntPtr((void*)pter));
			}			

			if(texture == nullptr)
				throw gcnew InvalidOperationException("Invalid Resource Type");				

			return texture;
			
		}

		void D3D9GraphicDevice::Clear(ClearFlags flags, int color, float depth, int stencil)
		{
			DWORD clearFlag = static_cast<DWORD>(flags);
			SAFECALL( _pDevice->Clear(0, NULL, clearFlag, color, depth, stencil));
		}

		void D3D9GraphicDevice::Draw(int startVertexIndex, int primitiveCount)
		{
			D3DPRIMITIVETYPE primitive = static_cast<D3DPRIMITIVETYPE>(_iaPrimitiveType);

			SAFECALL( _pDevice->DrawPrimitive(primitive, startVertexIndex , primitiveCount));
		}

		void D3D9GraphicDevice::DrawIndexed(int baseVertexIndex, int minVertexIndex , int numVertices, int startIndex, int primitiveCount) 
		{
			D3DPRIMITIVETYPE primitive = static_cast<D3DPRIMITIVETYPE>(_iaPrimitiveType);

		   SAFECALL(_pDevice->DrawIndexedPrimitive(primitive, baseVertexIndex, minVertexIndex, numVertices, startIndex, primitiveCount));
		}

		void D3D9GraphicDevice::DrawUser(int primitiveCount, void * vertices , int vertexStride ) 
		{
			D3DPRIMITIVETYPE primitive = static_cast<D3DPRIMITIVETYPE>(_iaPrimitiveType);

			SAFECALL( _pDevice->DrawPrimitiveUP(primitive, primitiveCount, vertices, vertexStride));
		}

		void D3D9GraphicDevice::DrawIndexedUser(int minVertexIndex, int numVertices, int primitiveCount , void* indices, IndexFormat format, void* vertices, int vertexStride)
		{
			auto d3dFormat = format == IndexFormat::Index16? D3DFMT_INDEX16: D3DFMT_INDEX32;
			D3DPRIMITIVETYPE primitive = static_cast<D3DPRIMITIVETYPE>(_iaPrimitiveType);

			SAFECALL( _pDevice->DrawIndexedPrimitiveUP(primitive ,minVertexIndex, numVertices, primitiveCount, indices,d3dFormat, vertices, vertexStride));
		}

		void D3D9GraphicDevice::BeginRender()
		{
			if(!begin)
			{
				begin = true;
				SAFECALL( _pDevice->BeginScene() );
			}
		}

		void D3D9GraphicDevice::EndRender()
		{
			if(begin)
			{
				begin = false;
				SAFECALL(_pDevice->EndScene());
			}
		}

		void D3D9GraphicDevice::Present()
		{			
			   auto result =_pDevice->Present(NULL, NULL, NULL, NULL);

			   if(result == D3DERR_DEVICELOST)
			   {
				   HRESULT err;
				   //wait until the device can be restore
				   while ((err = _pDevice->TestCooperativeLevel()) != D3DERR_DEVICENOTRESET)				   					
				   {  
					   if(err == D3DERR_DRIVERINTERNALERROR)
					   {
						    Lost(this);							
							throw gcnew GraphicDeviceFailException(L"Driver Internal Error");
					   }

					   Sleep(17);
				   }
				   
				 ResetDevice();
			   }
				else if(result == D3DERR_DEVICENOTRESET)
				{
					ResetDevice();
				}			  
			
		}

		void D3D9GraphicDevice::ResizeBackBuffer(int width, int height)
		{						
			_desc->BackBufferWidth = width;
			_desc->BackBufferHeight = height;
			
			ResetDevice();
		}

		void D3D9GraphicDevice::ResetDevice()
		{
			//release all device memory resources and swap chains
			_omBackBuffer->~RenderTarget();
			_omBackDepthStencil->~DepthStencil();

			D3DSwapChain^ s = static_cast<D3DSwapChain^>( _swapChains[0]);
			s->_pSwapChain->Release();

			 Lost(this);

			_pp->BackBufferWidth = _desc->BackBufferWidth;
			_pp->BackBufferHeight = _desc->BackBufferHeight;

			SAFECALL( _pDevice->Reset(_pp));

			//recreate all memory resource and swap chains
			Reset(this);
			
			//set states
			/*auto rs = RSState;
			RSState = nullptr;
			RSState = rs;

			auto blend = OMBlendState;
			OMBlendState = nullptr;
			OMBlendState = blend;

			auto depht =  OMDepthStencilState;
			OMDepthStencilState = nullptr;
			OMDepthStencilState = depht;*/

			InitializeOMStage();

			for (int i = 0; i < _iaVertexBufferBind->Length; i++)
			{
				_iaVertexBufferBind[i] = BufferBind();
			}

				D3DVIEWPORT9 vp;
			_pDevice->GetViewport(&vp);
			_rsVewPort.Width = vp.Width;
			_rsVewPort.Height = vp.Height;
			_rsVewPort.MaxDepth = vp.MaxZ;
			_rsVewPort.MinDepth = vp.MinZ;
			_rsVewPort.TopLeftX = vp.X;
			_rsVewPort.TopLeftY = vp.Y;
			RECT rec;
			_pDevice->GetScissorRect(&rec);

			_rsScissorRect.X = rec.left;
			_rsScissorRect.Y = rec.top;
			_rsScissorRect.Width = rec.right - rec.left;
			_rsScissorRect.Height = rec.bottom - rec.top;

			Array::Clear(_psTextures, 0, _psTextures->Length);
			Array::Clear(_psSamplers, 0, _psSamplers->Length);
			Array::Clear(_vsSamplers, 0, _vsSamplers->Length);
			program = nullptr;
		}

		void D3D9GraphicDevice::UpdateSubResource(SubResource^ src, SubResource^ dest)
		{
			D3DSubResource^ dsrc = static_cast<D3DSubResource^>(src);
			D3DSubResource^ ddest = static_cast<D3DSubResource^>(dest);

			D3DSURFACE_DESC srcDesc;
			D3DSURFACE_DESC destDesc;
			dsrc->_pSurface->GetDesc(&srcDesc);			
			ddest->_pSurface->GetDesc(&destDesc);			

			if(srcDesc.Pool == D3DPOOL_SYSTEMMEM && destDesc.Pool == D3DPOOL_DEFAULT &&
				srcDesc.MultiSampleType == D3DMULTISAMPLE_NONE && destDesc.MultiSampleType == D3DMULTISAMPLE_NONE)
			{
				if(srcDesc.Width != destDesc.Width || srcDesc.Height!= destDesc.Height)
					throw gcnew ArgumentOutOfRangeException(L"Resources Sizes do not match");

				SAFECALL( _pDevice->UpdateSurface(dsrc->_pSurface, NULL ,ddest->_pSurface ,NULL) );
			}
			else if(srcDesc.Pool == D3DPOOL_DEFAULT && destDesc.Pool == D3DPOOL_DEFAULT)
			{
				SAFECALL(_pDevice->StretchRect(dsrc->_pSurface,NULL, ddest->_pSurface,NULL, D3DTEXF_LINEAR));
			}
			else if(srcDesc.Pool == D3DPOOL_DEFAULT && destDesc.Pool == D3DPOOL_SYSTEMMEM
				&& (srcDesc.Usage & D3DUSAGE_RENDERTARGET) )
			{
				if(srcDesc.Width != destDesc.Width || srcDesc.Height!= destDesc.Height)
					throw gcnew ArgumentOutOfRangeException(L"Resources Sizes do not match");

				SAFECALL(_pDevice->GetRenderTargetData(dsrc->_pSurface, ddest->_pSurface));
			}
			else
			{
				throw gcnew InvalidOperationException(L"The driver does not support the operations for the given resources");
			}
		}
	}
}
