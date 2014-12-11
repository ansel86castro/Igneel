#include "Stdafx.h"
#include "GraphicDevice.h"
#include "DeviceManager.h"
#include "SamplerState10.h"
#include "Texture1D10.h"
#include "Texture2D10.h"
#include "Texture3D10.h"
#include "RenderTarget.h"
#include "GraphicBuffer10.h"
#include "ShaderProgram.h"
#include "ConstantBuffer.h"
#include "ShaderProgram.h"

using namespace System::Drawing::Imaging;

namespace IgneelD3D10
{
	GraphicDevice10::GraphicDevice10(GraphicDeviceDesc^ desc)
		:GraphicDevice(desc)
	{
		OpenConstantBuffers = gcnew List<CbHandle^>();
	}
	
	DeviceInfo GraphicDevice10::InitDevice(GraphicDeviceDesc^ desc)
	{		
		GraphicManager10^ manager = static_cast<GraphicManager10^>(Service::Get<GraphicDeviceFactory^>());
		
		IDXGISwapChain *swapChain = NULL;
		ID3D10Device * device = NULL;
		DeviceInfo di = DeviceInfo();

		if(desc->WindowsHandle != IntPtr::Zero)
		{		
			DXGI_SWAP_CHAIN_DESC sd;
			ZeroMemory( &sd, sizeof(DXGI_SWAP_CHAIN_DESC) );

			sd.BufferCount = 1;
			sd.BufferDesc.Width = desc->BackBufferWidth;
			sd.BufferDesc.Height = desc->BackBufferHeight;
			sd.BufferDesc.Format = static_cast<DXGI_FORMAT>(desc->BackBufferFormat); 
			sd.BufferDesc.RefreshRate.Numerator = 60;
			sd.BufferDesc.RefreshRate.Denominator = 1;
			sd.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
			sd.OutputWindow = (HWND) desc->WindowsHandle.ToPointer();	
			sd.SampleDesc.Count = desc->MSAA.Count;
			sd.SampleDesc.Quality = desc->MSAA.Quality;
			sd.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;
			//sd.Flags = DXGI_SWAP_CHAIN_FLAG_GDI_COMPATIBLE;
			sd.Windowed = !desc->FullScreen;
			if(desc->FullScreen)
				sd.Flags|=DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH;

				switch (desc->DriverType)
				{
					case GraphicDeviceType::Hardware:
						if( FAILED( D3D10CreateDeviceAndSwapChain( manager->_adapter, D3D10_DRIVER_TYPE_HARDWARE, NULL, 0, D3D10_SDK_VERSION, &sd, &swapChain, &device ) ) )
						{				
							if( FAILED( D3D10CreateDeviceAndSwapChain( manager->_adapter, D3D10_DRIVER_TYPE_REFERENCE, NULL, 0, D3D10_SDK_VERSION, &sd, &swapChain, &device ) ) )
									 throw gcnew GraphicDeviceFailException("Invalid DeviceType");
							di.DriverType = GraphicDeviceType::Reference;
						}
						else
							di.DriverType = GraphicDeviceType::Hardware;
						break;
					case GraphicDeviceType::Reference:
						if( FAILED( D3D10CreateDeviceAndSwapChain( manager->_adapter, D3D10_DRIVER_TYPE_REFERENCE, NULL, 0, D3D10_SDK_VERSION, &sd, &swapChain, &device ) ) )
									 throw gcnew GraphicDeviceFailException("Invalid DeviceType");
						di.DriverType = GraphicDeviceType::Reference;
						break;				
					case GraphicDeviceType::NullReference:
						if( FAILED( D3D10CreateDeviceAndSwapChain( manager->_adapter, D3D10_DRIVER_TYPE_NULL, NULL, 0, D3D10_SDK_VERSION, &sd, &swapChain, &device ) ) )
									 throw gcnew GraphicDeviceFailException("Invalid DeviceType");
						di.DriverType = GraphicDeviceType::NullReference;
						break;		
					case GraphicDeviceType::Software:
						if( FAILED( D3D10CreateDeviceAndSwapChain( manager->_adapter, D3D10_DRIVER_TYPE_SOFTWARE, NULL, 0, D3D10_SDK_VERSION, &sd, &swapChain, &device ) ) )
									 throw gcnew GraphicDeviceFailException("Invalid DeviceType");
						di.DriverType = GraphicDeviceType::Software;
						break;		
				}
		}
		else
		{
			switch (desc->DriverType)
			{
				case GraphicDeviceType::Hardware:
					if( FAILED( D3D10CreateDevice( manager->_adapter, D3D10_DRIVER_TYPE_HARDWARE, NULL, 0, D3D10_SDK_VERSION, &device ) ) )
					{				
						if( FAILED( D3D10CreateDevice( manager->_adapter, D3D10_DRIVER_TYPE_REFERENCE, NULL, 0, D3D10_SDK_VERSION, &device ) ) )
									throw gcnew GraphicDeviceFailException("Invalid DeviceType");
						di.DriverType = GraphicDeviceType::Reference;
					}
					else
						di.DriverType = GraphicDeviceType::Hardware;
					break;
				case GraphicDeviceType::Reference:
					if( FAILED( D3D10CreateDevice( manager->_adapter, D3D10_DRIVER_TYPE_REFERENCE, NULL, 0, D3D10_SDK_VERSION, &device ) ) )
									throw gcnew GraphicDeviceFailException("Invalid DeviceType");
					di.DriverType = GraphicDeviceType::Reference;
					break;				
				case GraphicDeviceType::NullReference:
					if( FAILED( D3D10CreateDevice( manager->_adapter, D3D10_DRIVER_TYPE_NULL, NULL, 0, D3D10_SDK_VERSION, &device ) ) )
									throw gcnew GraphicDeviceFailException("Invalid DeviceType");
					di.DriverType = GraphicDeviceType::NullReference;
					break;		
				case GraphicDeviceType::Software:
					if( FAILED( D3D10CreateDevice( manager->_adapter, D3D10_DRIVER_TYPE_SOFTWARE, NULL, 0, D3D10_SDK_VERSION,  &device ) ) )
									throw gcnew GraphicDeviceFailException("Invalid DeviceType");
					di.DriverType = GraphicDeviceType::Software;
					break;		
			}
		}

		if(!device)
			throw gcnew GraphicDeviceFailException("Invalid DeviceType");

		_device = device;
		_swapChain = swapChain;

		D3D10_VIEWPORT vp;
	    vp.Width = desc->BackBufferWidth;
		vp.Height = desc->BackBufferHeight;
		vp.MinDepth = 0.0f;
		vp.MaxDepth = 1.0f;
		vp.TopLeftX = 0;
		vp.TopLeftY = 0;
	   _device->RSSetViewports( 1, &vp );

	   RSViewPort = *(ViewPort*)&vp;

	   di.DisplayFormat = desc->BackBufferFormat;
	   di.DisplayHeight = desc->BackBufferWidth;
	   di.DisplayHeight = desc->BackBufferHeight;
	   di.MSAA = desc->MSAA;

	 /*  if(swapChain)
	   {
		   DXGI_SWAP_CHAIN_DESC scdesc;
			swapChain->GetDesc(&scdesc);
	   }*/

	   //di.RefreshRate = scdesc.BufferDesc.RefreshRate.Numerator/sd.BufferDesc.RefreshRate.Denominator;

	   GC::AddMemoryPressure(sizeof(ID3D10Device));
	   GC::AddMemoryPressure(sizeof(IDXGISwapChain));

		/*IDXGIDevice * pDXGIDevice;
	    SAFECALL(_device->QueryInterface(__uuidof(IDXGIDevice), (void **)&pDXGIDevice));*/
	
		di.SimultaneousRTCount = D3D10_SIMULTANEOUS_RENDER_TARGET_COUNT;
		di.MaxSimultaneousTextures = 128;
		di.MaxStreams = 16;
		di.MaxTextureWidth = 8000;
		di.MaxTextureHeight = 8000;

		DXGI_ADAPTER_DESC ad;
		manager->_adapter->GetDesc(&ad);
		di.DeviceName = System::Runtime::InteropServices::Marshal::PtrToStringUni(static_cast<IntPtr>(ad.Description), lstrlenW(ad.Description));
		di.DeviceId = ad.DeviceId;	

		return di;
	}

	SOInitialization GraphicDevice10::GetSOInitialization()
	{
		SOInitialization ini;
		ini.NbStremOutputBuffers = 4;
		return ini;
	}

	bool GraphicDevice10::CheckFormatSupport(Format format, BindFlags binding, ResourceType type)
	{
		UINT result;
		SAFECALL(_device->CheckFormatSupport(static_cast<DXGI_FORMAT>(format), & result));
		
		switch (type)
		{				
			case Igneel::Graphics::ResourceType::Buffer:
				switch (binding)
				{
					case Igneel::Graphics::BindFlags::ConstantBuffer: return result && D3D10_FORMAT_SUPPORT_BUFFER;				
					case Igneel::Graphics::BindFlags::IndexBuffer: return result && D3D10_FORMAT_SUPPORT_IA_INDEX_BUFFER;							
					case Igneel::Graphics::BindFlags::StreamOutput: return result && D3D10_FORMAT_SUPPORT_SO_BUFFER;
					case Igneel::Graphics::BindFlags::VertexBuffer: return result && D3D10_FORMAT_SUPPORT_IA_VERTEX_BUFFER;
					default: return false;
				}				
			case Igneel::Graphics::ResourceType::Texture1D: return result && D3D10_FORMAT_SUPPORT_TEXTURE1D;								
			case Igneel::Graphics::ResourceType::Texture2D: return result && D3D10_FORMAT_SUPPORT_TEXTURE2D;
			case Igneel::Graphics::ResourceType::Texture3D: return result && D3D10_FORMAT_SUPPORT_TEXTURE3D;							
			default: return false;	
		}
	}

	int GraphicDevice10::CheckMultisampleQualityLevels(Format format, int multySampleCount , bool windowed)
	{
		UINT quality;
		_device->CheckMultisampleQualityLevels(static_cast<DXGI_FORMAT>(format), multySampleCount, &quality);
		return quality;
	}

	SamplerState^ GraphicDevice10::CreateSamplerState(SamplerDesc desc)
	{
		return gcnew SamplerState10(_device,desc);
	}

	Texture1D^ GraphicDevice10::CreateTexture1D(Texture1DDesc desc, array<IntPtr>^data)
	{
		return gcnew Texture1D10(_device, desc, data);		
	}

	Texture2D^ GraphicDevice10::CreateTexture2D(Texture2DDesc desc , array<MappedTexture2D>^data)
	{
		return gcnew Texture2D10(_device, desc, data);
	}

	Texture3D^ GraphicDevice10::CreateTexture3D(Texture3DDesc desc, array<MappedTexture3D>^data)
	{
		return gcnew Texture3D10(_device, desc, data);
	}		

	Texture^ GraphicDevice10::CreateTextureFromFile(ResourceType type, String^ filename)
	{
		auto ext = System::IO::Path::GetExtension(filename);
		if(ext->Equals(L".tga",StringComparison::CurrentCultureIgnoreCase))
		{
			//filename = filename->Substring(0,filename->Length - 4) + ".dds";
			return CreateTextureFromFileTga(type, filename);

		}
		LPCWSTR pter =  (LPCWSTR)Marshal::StringToHGlobalUni(filename).ToPointer();
		ID3D10Resource * resource = NULL;
		Texture^ texture;
		try
		{
			SAFECALL( D3DX10CreateTextureFromFile(_device,pter,NULL,NULL,&resource, NULL) );			
		}
		finally
		{
			Marshal::FreeHGlobal(IntPtr((void*)pter));
		}			
		switch (type)
		{			
		case Igneel::Graphics::ResourceType::Texture1D:
			texture = gcnew Texture1D10(_device, (ID3D10Texture1D*)resource, filename);
			break;
		case Igneel::Graphics::ResourceType::Texture2D:
			texture = gcnew Texture2D10(_device, (ID3D10Texture2D*)resource, filename);
			break;
		case Igneel::Graphics::ResourceType::Texture3D:
			texture = gcnew Texture3D10(_device, (ID3D10Texture3D*)resource, filename);
			break;	
		default:
			throw gcnew InvalidOperationException("Invalid Resource Type");
			break;
		}

		if(texture == nullptr)
			throw gcnew InvalidOperationException("Invalid Resource Type");				

		auto noti = Service::Get<Igneel::Services::INotificationService^>();
		if(noti!=nullptr)
			noti->OnObjectCreated(texture);
		return texture;
	}

	Texture^ GraphicDevice10::CreateTextureFromStream(ResourceType type, Stream^ stream, String^ location)
	{
		
		ID3D10Resource * resource = NULL;
		Texture^ texture;
		BinaryReader^ reader = gcnew BinaryReader(stream);
		auto bytes = reader->ReadBytes(stream->Length);
		reader->Close();

		pin_ptr<byte> pBytes = &bytes[0];
	    byte* pter = pBytes;
		SAFECALL( D3DX10CreateTextureFromMemory(_device, pter, stream->Length ,NULL, NULL, &resource, NULL) );
		
		switch (type)
		{			
		case Igneel::Graphics::ResourceType::Texture1D:
			texture = gcnew Texture1D10(_device, (ID3D10Texture1D*)resource, location);
			break;
		case Igneel::Graphics::ResourceType::Texture2D:
			texture = gcnew Texture2D10(_device, (ID3D10Texture2D*)resource, location);
			break;
		case Igneel::Graphics::ResourceType::Texture3D:
			texture = gcnew Texture3D10(_device, (ID3D10Texture3D*)resource, location);
			break;	
		default:
			throw gcnew InvalidOperationException("Invalid Resource Type");
			break;
		}

		if(texture == nullptr)
			throw gcnew InvalidOperationException("Invalid Resource Type");				

		auto noti = Service::Get<Igneel::Services::INotificationService^>();
		if(noti!=nullptr)
			noti->OnObjectCreated(texture);
		return texture;
	}

	/*Texture^ GraphicDevice10::CreateTextureFromFileTga(ResourceType type, String^ filename)
	{
		TargaImage^ tgaImg = gcnew TargaImage(filename);
		MemoryStream^  mStream = gcnew MemoryStream(tgaImg->Image->Width * tgaImg->Image->Height * 32);
		tgaImg->Image->Save(mStream, System::Drawing::Imaging::ImageFormat::Bmp);

		ID3D10Resource * resource = NULL;
		Texture^ texture;
	
		array<byte>^ bytes = mStream->ToArray();
		pin_ptr<byte> pBytes = &bytes[0];
	    byte* pter = pBytes;

		SAFECALL( D3DX10CreateTextureFromMemory(_device, pter, bytes->Length ,NULL, NULL, &resource, NULL) );
		
		mStream->Close();

		switch (type)
		{			
		case Igneel::Graphics::ResourceType::Texture1D:
			texture = gcnew Texture1D10(_device, (ID3D10Texture1D*)resource, filename);
			break;
		case Igneel::Graphics::ResourceType::Texture2D:
			texture = gcnew Texture2D10(_device, (ID3D10Texture2D*)resource, filename);
			break;
		case Igneel::Graphics::ResourceType::Texture3D:
			texture = gcnew Texture3D10(_device, (ID3D10Texture3D*)resource, filename);
			break;	
		default:
			throw gcnew InvalidOperationException("Invalid Resource Type");
			break;
		}

		tgaImg->~TargaImage();

		if(texture == nullptr)
			throw gcnew InvalidOperationException("Invalid Resource Type");				

		auto noti = Service::Get<Igneel::Services::INotificationService^>();
		if(noti!=nullptr)
			noti->OnObjectCreated(texture);
		return texture;
	}*/

	int find_Lod(int width, int height)
	{
		int lod = 1;
		while (width > 0 && height > 0)
		{
			width =  width >> 1;
			height =  height >> 1;
			if(width == 0 || height == 0)
			{				
				return lod;
			}
			lod++;
		}
		return lod;
	}

	Texture^ GraphicDevice10::CreateTextureFromFileTga(ResourceType type, String^ filename)
	{
		TargaImage^ tgaImg = gcnew TargaImage(filename);
		auto bmp = tgaImg->Image;
		auto bmpData = bmp->LockBits(System::Drawing::Rectangle(0, 0, bmp->Width, bmp->Height), 
                        System::Drawing::Imaging::ImageLockMode::ReadWrite, bmp->PixelFormat);

		int lod = find_Lod(bmp->Width, bmp->Height);

		Texture2DDesc desc = Texture2DDesc();
		desc.ArraySize = 1;
		desc.BindFlags = BindFlags::ShaderResource;
		desc.CPUAccessFlags = CpuAccessFlags::None;
		desc.Format = Format::R8G8B8A8_UNORM;
		desc.Height = bmp->Height;
		desc.Width = bmp->Width;
		desc.MipLevels = lod;
		desc.Options = ResourceOptionFlags::None;
		desc.Usage = ResourceUsage::Default;

		Texture2D10^ texture = (Texture2D10^)CreateTexture2D(desc , nullptr);		
		byte * data = static_cast<byte*>(bmpData->Scan0.ToPointer());
		bool freedata = false;
		int pitch = bmpData->Stride;
		int bytes = Math::Abs(bmpData->Stride) * bmp->Height;

		if(bmp->PixelFormat != PixelFormat::Format32bppRgb && bmp->PixelFormat != PixelFormat::Format32bppArgb)
		{
			byte * src = data;
			pitch = desc.Width * 4;
			int length = desc.Height * pitch;
			freedata = true;
			data = new byte[length];			
			byte* pixel = data;			

			//convert BGR to BGRA
			for (int i = 0; i < bytes; i+=3 , pixel+=4 , src+=3)
			{
				/**pixel = *src;
				*(pixel + 1) = *(src + 1);
				*(pixel + 2) = *(src + 2);
				*(pixel + 3) = 0;*/

				*pixel = *(src + 2);
				*(pixel + 1) = *(src + 1);
				*(pixel + 2) = *src;
				*(pixel + 3) = 255;
			}			
		}
		if(!freedata)
		{
			byte* pixel = data;	
			for (int i = 0; i < bytes; i+=4, pixel+=4)
			{
				byte temp = *pixel;
				*pixel = *(pixel + 2);
				*(pixel + 2) = temp;
			}
		}

		_device->UpdateSubresource(texture->_texture, 0, NULL, data, pitch, 0);
		_device->GenerateMips(texture->_shaderResource);

		if(freedata)
			delete[]data;
		bmp->UnlockBits(bmpData);

		tgaImg->~TargaImage();
		return texture;
	}

	void GraphicDevice10::GenerateMips(Texture^ texture)
	{
		ID3D10ShaderResourceView* srv = NULL;
		switch (texture->Type)
		{
			case ResourceType::Texture1D:
			 srv = static_cast<Texture1D10^>(texture)->_shaderResource;
			break;
			case ResourceType::Texture2D:
			srv = static_cast<Texture2D10^>(texture)->_shaderResource;
			break;
			case ResourceType::Texture3D:
			srv = static_cast<Texture3D10^>(texture)->_shaderResource;
			break;		
		}
		if(srv)
			_device->GenerateMips(srv);
	}

	Texture1D^ GraphicDevice10::CreateTexture1DFromFile(String^ filename)
	{
		return static_cast<Texture1D^>(CreateTextureFromFile(ResourceType::Texture1D, filename));
	}

	Texture1D^ GraphicDevice10::CreateTexture1DFromStream(Stream^ stream)
	{
		return static_cast<Texture1D^>(CreateTextureFromStream(ResourceType::Texture1D, stream, nullptr));
	}

	Texture2D^ GraphicDevice10::CreateTexture2DFromFile(String^ filename)
	{
		return static_cast<Texture2D^>(CreateTextureFromFile(ResourceType::Texture2D, filename));
	}

	Texture2D^ GraphicDevice10::CreateTexture2DFromStream(Stream^ stream)
	{
		return static_cast<Texture2D^>(CreateTextureFromStream(ResourceType::Texture2D, stream, nullptr));
	}

	Texture3D^ GraphicDevice10::CreateTexture3DFromFile(String^ filename)
	{
		return static_cast<Texture3D^>(CreateTextureFromFile(ResourceType::Texture3D, filename));
	}

	Texture3D^ GraphicDevice10::CreateTexture3DFromStream(Stream^ stream)
	{
		return static_cast<Texture3D^>(CreateTextureFromStream(ResourceType::Texture3D, stream, nullptr));
	}


	void GraphicDevice10::Clear(ClearFlags flags, Color4 color, float depth, int stencil)
	{
		DephtStencil10^ zbuffer = static_cast<DephtStencil10^>(_omDepthStencil);
		if(zbuffer!=nullptr)
		{
			if((flags & ClearFlags::ZBuffer) == ClearFlags::ZBuffer	)
			{
				UINT dsflags = D3D10_CLEAR_DEPTH;
				if((flags & ClearFlags::Stencil) == ClearFlags::Stencil)
				{
					dsflags |= D3D10_CLEAR_STENCIL;
				}
				_device->ClearDepthStencilView(zbuffer->_dsv, dsflags, depth , stencil);
			}
		}		
		for (int i = 0; i < _omRenderTargets->Length; i++)
		{
			RenderTarget10^ rt	= static_cast<RenderTarget10^>(_omRenderTargets[i]);
			if(rt != nullptr)
				_device->ClearRenderTargetView(rt->_rtv, (float*)&color);
			else
				break;
		}
	}

	void GraphicDevice10::CloseCBuffers()
	{	
		for (int i = 0, count = OpenConstantBuffers->Count; i < count; i++)
		{
			OpenConstantBuffers[i]->Close();			
		}		
		OpenConstantBuffers->Clear();
	}

	void GraphicDevice10::Draw(int vertexCount, int startVertexIndex)
	{		
		CloseCBuffers();
		_device->Draw(vertexCount , startVertexIndex);
	}

	void GraphicDevice10::DrawIndexed(int indexCount, int startIndex, int baseVertexIndex)
	{
		CloseCBuffers();
		_device->DrawIndexed(indexCount, startIndex, baseVertexIndex);
	}

	void GraphicDevice10::DrawAuto()
	{
		CloseCBuffers();
		_device->DrawAuto();
	}	

	void GraphicDevice10::ResizeBackBuffer(int width, int height)
	{		
		DefaultSwapChain->ResizeBackBuffer(width, height);
		_desc->BackBufferWidth = width;
		_desc->BackBufferHeight = height;

		ID3D10Texture2D* depthTex;
		ID3D10DepthStencilView *depthView;
		CreateDepthStencil(_device, _swapChain, &depthTex, &depthView);
		static_cast<DephtStencil10^>(_omBackDepthStencil)->setDepthStencilView(depthView, depthTex);		

	}

	void GraphicDevice10::ResolveSubresource(Texture^ src, int srcSub, Texture^ dest, int destSub) 
	{
		auto srcRes = GetResource(src);
		auto destRes = GetResource(dest);

		_device->ResolveSubresource(destRes, destSub, srcRes, srcSub, (DXGI_FORMAT)dest->Format);
	}

	void GraphicDevice10::CopySubResource(Texture^ destTexture, int destSubRes, int dstX , int dstY, int dstZ, Texture^ srcTexture, int srcSubRes, DataBox destBox)
	{
		_device->CopySubresourceRegion(GetResource(destTexture) ,destSubRes , dstX , dstY, dstZ, GetResource(srcTexture), srcSubRes, (D3D10_BOX*)&destBox);
	}

	void GraphicDevice10::CopyTexture(Texture^ src, Texture^ dest)
	{
		_device->CopyResource(GetResource(src), GetResource(dest));
	}

	void GraphicDevice10::UpdateSubResource(Texture^ dest, int subResource, DataBox* box, void* srcPointer, int srcRowPith, int srcDepthPitch)
	{
		auto destResource = GetResource(dest);
		_device->UpdateSubresource(destResource, subResource, (D3D10_BOX*)box, srcPointer, srcRowPith, srcDepthPitch);
	}

	void GraphicDevice10::SOSetTargetImpl(int slot, GraphicBuffer^ buffer, int offset)
	{
		GraphicBuffer10^ buff = static_cast<GraphicBuffer10^>(buffer);
		ID3D10Buffer* b[1] = { buff->_buffer };
		_device->SOSetTargets(1 , b, (UINT*)&offset);
	}

	void GraphicDevice10::OnDispose(bool disposing)
	{
		__super::OnDispose(disposing);
		_device->Release();
	}

}	
