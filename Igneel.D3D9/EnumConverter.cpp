#include "stdafx.h"
#include "EnumConverter.h"

#include < stdio.h >
#include < stdlib.h >
#include < vcclr.h >


D3DFORMAT GetD3DFORMAT(Format f)
{
	switch (f)
	{
	case Igneel::Graphics::Format::UNKNOWN: return D3DFMT_UNKNOWN;			

	case Igneel::Graphics::Format::R16G16B16A16_FLOAT:    return D3DFMT_A16B16G16R16F;

	case Igneel::Graphics::Format::R16G16B16A16_TYPELESS: 
	case Igneel::Graphics::Format::R16G16B16A16_UINT:     
	case Igneel::Graphics::Format::R16G16B16A16_SINT:	   return D3DFMT_A16B16G16R16;					

	case Igneel::Graphics::Format::R8G8B8A8_TYPELESS:     					
	case Igneel::Graphics::Format::R8G8B8A8_UNORM:			
	case Igneel::Graphics::Format::R8G8B8A8_UNORM_SRGB:					
	case Igneel::Graphics::Format::R8G8B8A8_UINT:	          
	case Igneel::Graphics::Format::R8G8B8A8_SNORM:		
	case Igneel::Graphics::Format::R8G8B8A8_SINT: return D3DFMT_A8B8G8R8;	

	case Igneel::Graphics::Format::D32_FLOAT: return D3DFMT_D32;		
	case Igneel::Graphics::Format::R32_FLOAT: return D3DFMT_R32F;			
	case Igneel::Graphics::Format::D24_UNORM_S8_UINT: return D3DFMT_D24S8;		
	case Igneel::Graphics::Format::R24_UNORM_X8_TYPELESS: return D3DFMT_D24X8; 		

	case Igneel::Graphics::Format::X24_TYPELESS_G8_UINT:return D3DFMT_A8;
		break;
	case Igneel::Graphics::Format::R8G8_TYPELESS: 
		break;
	case Igneel::Graphics::Format::R8G8_UNORM:
		break;
	case Igneel::Graphics::Format::R8G8_UINT:
		break;
	case Igneel::Graphics::Format::R8G8_SNORM:
		break;
	case Igneel::Graphics::Format::R8G8_SINT:
		break;
	case Igneel::Graphics::Format::R16_TYPELESS:
		break;

	case Igneel::Graphics::Format::R16_FLOAT:return D3DFMT_R16F;		
	case Igneel::Graphics::Format::D16_UNORM: return D3DFMT_D16;				
	case Igneel::Graphics::Format::R8_TYPELESS: return D3DFMT_A8;
		
	case Igneel::Graphics::Format::R8_UNORM:	
	case Igneel::Graphics::Format::R8_UINT:	
	case Igneel::Graphics::Format::R8_SNORM:	
	case Igneel::Graphics::Format::R8_SINT:		
	case Igneel::Graphics::Format::A8_UNORM: return D3DFMT_A8;	
	case Igneel::Graphics::Format::R1_UNORM:  return D3DFMT_A1;			
		
	case Igneel::Graphics::Format::R8G8_B8G8_UNORM:	
	case Igneel::Graphics::Format::G8R8_G8B8_UNORM:  return D3DFMT_G8R8_G8B8;	
	case Igneel::Graphics::Format::BC1_TYPELESS: 		
	case Igneel::Graphics::Format::BC1_UNORM:	
	case Igneel::Graphics::Format::BC1_UNORM_SRGB:		return D3DFMT_DXT1;

	case Igneel::Graphics::Format::BC2_TYPELESS:		
	case Igneel::Graphics::Format::BC2_UNORM:		
	case Igneel::Graphics::Format::BC2_UNORM_SRGB: return D3DFMT_DXT2;

	case Igneel::Graphics::Format::BC3_TYPELESS: 		
	case Igneel::Graphics::Format::BC3_UNORM:		
	case Igneel::Graphics::Format::BC3_UNORM_SRGB:return D3DFMT_DXT3;
		

	case Igneel::Graphics::Format::BC4_TYPELESS:	
	case Igneel::Graphics::Format::BC4_UNORM:		
	case Igneel::Graphics::Format::BC4_SNORM:return D3DFMT_DXT4;	

	case Igneel::Graphics::Format::BC5_TYPELESS:		
	case Igneel::Graphics::Format::BC5_UNORM:		
	case Igneel::Graphics::Format::BC5_SNORM:	return D3DFMT_DXT5;	

	case Igneel::Graphics::Format::B5G6R5_UNORM:	return D3DFMT_R5G6B5;		
	case Igneel::Graphics::Format::B5G5R5A1_UNORM:  return D3DFMT_A1R5G5B5;	

	case Igneel::Graphics::Format::B8G8R8A8_TYPELESS:
	case Igneel::Graphics::Format::B8G8R8A8_UNORM_SRGB:
	case Igneel::Graphics::Format::B8G8R8A8_UNORM:	return D3DFMT_A8R8G8B8;	 	

	case Igneel::Graphics::Format::B8G8R8X8_TYPELESS:
	case Igneel::Graphics::Format::B8G8R8X8_UNORM_SRGB:
	case Igneel::Graphics::Format::B8G8R8X8_UNORM: return D3DFMT_X8R8G8B8;	 							
	
		
	case Igneel::Graphics::Format::BC6H_TYPELESS:
		break;
	case Igneel::Graphics::Format::BC6H_UF16:
		break;
	case Igneel::Graphics::Format::BC6H_SF16:
		break;
	case Igneel::Graphics::Format::BC7_TYPELESS:
		break;
	case Igneel::Graphics::Format::BC7_UNORM:
		break;
	case Igneel::Graphics::Format::BC7_UNORM_SRGB:
		break;
	default:
		throw gcnew FormatNotSupportedException(f);
		break;
	}

	throw gcnew FormatNotSupportedException(f);
}

Format GetFORMAT(D3DFORMAT f)
{
	switch (f)	
	{
		case D3DFMT_UNKNOWN: return Format::UNKNOWN;
		case D3DFMT_A16B16G16R16F: return Format::R16G16B16A16_FLOAT;
		case D3DFMT_A16B16G16R16: return Format::R16G16B16A16_TYPELESS;		
		case D3DFMT_D32: return Format::D32_FLOAT;
		case D3DFMT_R32F: return Format::R32_FLOAT;
		case D3DFMT_R16F: return Format::R16_FLOAT;		
		case D3DFMT_D16: return Format::D16_UNORM;
		case D3DFMT_D24S8: return Format::D24_UNORM_S8_UINT;		
		case D3DFMT_A8: return Format::R8_TYPELESS;
		case D3DFMT_A1: return Format::R1_UNORM;
		case D3DFMT_G8R8_G8B8: return Format::R8G8_B8G8_UNORM;
		case D3DFMT_DXT1: return Format::BC1_TYPELESS;
		case D3DFMT_DXT2: return Format::BC2_TYPELESS;
		case D3DFMT_DXT3: return Format::BC3_TYPELESS;
		case D3DFMT_DXT4: return Format::BC4_TYPELESS;
		case D3DFMT_DXT5: return Format::BC5_TYPELESS;
		case D3DFMT_R5G6B5: return Format::B5G6R5_UNORM;
		case D3DFMT_A1R5G5B5: return Format::B5G5R5A1_UNORM;
		case D3DFMT_A8R8G8B8: return Format::B8G8R8A8_UNORM;
		case D3DFMT_X8R8G8B8: return Format::B8G8R8X8_UNORM;		

	default:
		throw gcnew InvalidOperationException(L"Not D3DFORMAT Supported");
		break;
	}

	throw gcnew InvalidOperationException(L"Not D3DFORMAT Supported");
}

void GetTextureUsage(ResourceUsage usage, BindFlags binding , CpuAccessFlags cpuAcces, OUT DWORD * d3dUsage , OUT D3DPOOL * d3dPool)
{
	if((binding & BindFlags::RenderTarget) == BindFlags::RenderTarget)
	{
		*d3dPool = D3DPOOL_DEFAULT;
		*d3dUsage = D3DUSAGE_RENDERTARGET;
	}
	else if((binding & BindFlags::DepthStencil) == BindFlags::DepthStencil)
	{
		*d3dPool = D3DPOOL_DEFAULT;
		*d3dUsage = D3DUSAGE_DEPTHSTENCIL;
	}
	else if((binding & BindFlags::ShaderResource) == BindFlags::ShaderResource)
	{
		if(usage == ResourceUsage::Default)
		{
			*d3dPool = D3DPOOL_MANAGED;
			*d3dUsage = cpuAcces == CpuAccessFlags::Write ? D3DUSAGE_WRITEONLY: 0;
		}
		else if(usage== ResourceUsage::Dynamic)
		{
			*d3dPool = D3DPOOL_DEFAULT;
			*d3dUsage = D3DUSAGE_DYNAMIC;
		}
		else if(usage == ResourceUsage::Immutable)
		{
			*d3dPool = D3DPOOL_DEFAULT;
			*d3dUsage = 0;
		}
	}
	
	else if(usage == ResourceUsage::Staging)
	{
		switch (cpuAcces)
		{
		case Igneel::Graphics::CpuAccessFlags::None:
			*d3dPool = D3DPOOL_MANAGED;
			*d3dUsage = 0;
			break;
		case Igneel::Graphics::CpuAccessFlags::Read:
			*d3dPool = D3DPOOL_SYSTEMMEM;
			*d3dUsage = 0;
			break;
		case Igneel::Graphics::CpuAccessFlags::Write:
			*d3dPool = D3DPOOL_SYSTEMMEM;
			*d3dUsage = 0;
			break;
		default:
			break;
		}
		
	}
	
}

DWORD GetD3DLOCK(MapType mapType , bool doNotWait)
{
	DWORD flags = 0;
	switch (mapType)
	{
	case Igneel::Graphics::MapType::Read:
		flags = D3DLOCK_READONLY;
		break;
	case Igneel::Graphics::MapType::Write:
		flags =  0;
		break;
	case Igneel::Graphics::MapType::ReadWrite:
		flags = 0;
		break;
	case Igneel::Graphics::MapType::Write_Discard:
		flags =  D3DLOCK_DISCARD;
		break;
	case Igneel::Graphics::MapType::Write_No_OverWrite:
		flags = D3DLOCK_NOOVERWRITE;
		break;
	default:
		flags  = 0;
		break;
	}

	if(doNotWait)
		flags |= D3DLOCK_DONOTWAIT;

	return flags;
}

D3DDEVTYPE GetD3DDEVTYPE(GraphicDeviceType devType)
{
	D3DDEVTYPE type;
	switch (devType)
	{
		case GraphicDeviceType::Hardware: type = D3DDEVTYPE_HAL;break;
		case GraphicDeviceType::NullReference:type = D3DDEVTYPE_NULLREF;break;
		case GraphicDeviceType::Reference:type = D3DDEVTYPE_REF;break;
		case GraphicDeviceType::Software:type = D3DDEVTYPE_SW;break;		
	}
	return type;
}

D3DRESOURCETYPE GetD3DD3DRESOURCETYPE(ResourceType rsType)
{
	D3DRESOURCETYPE d3dType;
	switch (rsType)
		{
		case Igneel::Graphics::ResourceType::Unknown:
			throw gcnew InvalidOperationException();
			break;
		case Igneel::Graphics::ResourceType::Buffer:
			d3dType = D3DRESOURCETYPE::D3DRTYPE_INDEXBUFFER;
			break;
		case Igneel::Graphics::ResourceType::Texture1D:
			d3dType = D3DRESOURCETYPE::D3DRTYPE_TEXTURE;
			break;
		case Igneel::Graphics::ResourceType::Texture2D:
			d3dType = D3DRESOURCETYPE::D3DRTYPE_TEXTURE;
			break;
		case Igneel::Graphics::ResourceType::Texture3D:
			d3dType = D3DRESOURCETYPE::D3DRTYPE_VOLUME;
			break;
		case Igneel::Graphics::ResourceType::TextureCube:
			d3dType = D3DRESOURCETYPE::D3DRTYPE_CUBETEXTURE;
			break;
		default:
			break;
		}

	return d3dType;
}

DWORD GetD3DUSAGE(BindFlags binding)
{
	DWORD usage = 0;

	switch (binding)
	{		
		case Igneel::Graphics::BindFlags::DepthStencil: usage = D3DUSAGE_DEPTHSTENCIL;break;			
		case Igneel::Graphics::BindFlags::RenderTarget: usage = D3DUSAGE_RENDERTARGET;break;	
	}

	return usage;
}

void GetBufferUsage(MapType cpuAcces , DWORD*	d3dusage, D3DPOOL* pool)
{
	switch (cpuAcces)
		{
		case Igneel::Graphics::MapType::Read:
			*pool = D3DPOOL_MANAGED;
			break;
		case Igneel::Graphics::MapType::Write:			
				  *pool = D3DPOOL_DEFAULT;
				 *d3dusage = D3DUSAGE_WRITEONLY;
			break;
		case Igneel::Graphics::MapType::ReadWrite:			
				*pool = D3DPOOL_MANAGED;
				*d3dusage = 0;
			break;
		case Igneel::Graphics::MapType::Write_Discard:						
				 * pool = D3DPOOL_DEFAULT;
				  *d3dusage = D3DUSAGE_DYNAMIC;		  
			break;
		case Igneel::Graphics::MapType::Write_No_OverWrite:
			*pool = D3DPOOL_DEFAULT;
			*d3dusage = D3DUSAGE_DYNAMIC;			
			break;
		default:
			break;
		}
}

String^ GetErr(HRESULT x)
{
	switch (x)
	{
	case D3DERR_DEVICELOST:return L"DEVICELOST";
	case D3DERR_DEVICENOTRESET:return L"DEVICENOTRESET";
	case D3DERR_DEVICEHUNG:return L"DEVICEHUNG";
	case D3DERR_CONFLICTINGRENDERSTATE:return L"CONFLICTINGRENDERSTATE";
	case D3DERR_CANNOTPROTECTCONTENT:return L"CANNOTPROTECTCONTENT";
	case D3DERR_CONFLICTINGTEXTUREFILTER:return L"CONFLICTINGTEXTUREFILTER";
	case D3DERR_CONFLICTINGTEXTUREPALETTE:return L"CONFLICTINGTEXTUREPALETTE";
	case D3DERR_DEVICEREMOVED:return L"DEVICEREMOVED";
	case D3DERR_DRIVERINTERNALERROR:return L"DRIVERINTERNALERROR";
    case D3DERR_INVALIDCALL:return L"INVALIDCALL";
	case D3DERR_DRIVERINVALIDCALL:return L"DRIVERINVALIDCALL";
	case D3DERR_INVALIDDEVICE:return L"INVALIDDEVICE";
	case D3DERR_OUTOFVIDEOMEMORY:return L"OUTOFVIDEOMEMORY";
	case D3DERR_NOTAVAILABLE:return L"NOTAVAILABLE";
	default: return L"Invalid Call";
	}
}



//LPSTR ConvertToChar(String^ str)
//{	
//   // Pin memory so GC can't move it while native function is called
//   pin_ptr<const wchar_t> wch = PtrToStringChars(str);   
//
//   // Conversion to char* :
//   // Can just convert wchar_t* to char* using one of the 
//   // conversion functions such as: 
//   // WideCharToMultiByte()
//   // wcstombs_s()
//   // ... etc
//   size_t convertedChars = 0;
//   size_t  sizeInBytes = ((str->Length + 1) * 2);
//   errno_t err = 0;
//   char    *ch = new char[sizeInBytes];
//
//   err = wcstombs_s(&convertedChars, 
//                    ch, sizeInBytes,
//                    wch, sizeInBytes);
//   if (err != 0)
//   {
//	   delete ch;
//   }
//    return ch;
//}