#include "stdafx.h"
#include "Utils.h" 

//GLenum GetFormat(Format format)
//{
//	 switch (format)
//            {
//                case Format::B8G8R8A8_UNORM_SRGB:
//                case Format::B8G8R8A8_UNORM:
//					*glType = GL_UNSIGNED_BYTE;
//					*glClientFormat =GL_BGRA_EXT;
//					*interalFormat = GL_RGBA;
//                    break;
//
//                case Format::R8G8B8A8_UNORM_SRGB:
//                case Format::R8G8B8A8_UNORM:
//                    *glType = GL_UNSIGNED_BYTE;
//                    *glClientFormat = GL_RGBA;
//                    *interalFormat = GL_RGBA;
//                    break;
//
//
//                case Format::R16G16B16A16_UINT:
//                case Format::R16G16B16A16_UNORM:
//					*glType = GL_UNSIGNED_SHORT;
//                    *glClientFormat = GL_RGBA;
//                    *interalFormat = GL_RGBA;
//                    break;
//                case Format::R16G16B16A16_SINT:
//                    *glType = GL_SHORT;
//                    *glClientFormat = GL_RGBA;
//                    *interalFormat = GL_RGBA;
//                    break;
//                case Format::R16G16B16A16_FLOAT:
//					*glType = GL_HALF_FLOAT;
//                    *glClientFormat = GL_RGBA;
//                    *interalFormat = GL_RGBA;
//                    break;
//
//
//                case Format::R32G32B32A32_FLOAT:
//                    *glType = GL_FLOAT;
//                    *glClientFormat = GL_RGBA;
//                    *interalFormat = GL_RGBA;
//                    break;
//                case Format::R32G32B32A32_SINT:
//                    *glType = GL_INT;
//                    *glClientFormat = GL_RGBA;
//                    *interalFormat = GL_RGBA;
//                    break;
//
//                case Format::R32G32B32_UINT:
//					*glType =GL_UNSIGNED_INT;
//                    *glClientFormat = GL_RGB;
//                    *interalFormat = GL_RGB;
//                    break;
//                case Format::R32G32B32_FLOAT:
//                    *glType = GL_FLOAT;
//                    *glClientFormat = GL_RGB;
//                    *interalFormat = GL_RGB;
//                    break;
//                case Format::R32G32B32_SINT:
//                    *glType = GL_INT;
//                    *glClientFormat = GL_RGB;
//                    *interalFormat = GL_RGB;
//                    break;
//
//
//                case Format::R32_FLOAT:
//                    *glType = GL_FLOAT;
//                    *glClientFormat =GL_RED;
//                    *interalFormat = GL_RED;
//                    break;
//
//                case Format::R32_UINT:
//					*glType = GL_UNSIGNED_INT;
//                    *glClientFormat =GL_RED;
//                    *interalFormat = GL_RED;
//                    break;
//                case Format::R32_SINT:
//                    *glType = GL_INT;
//                    *glClientFormat = GL_RED;
//                    *interalFormat = GL_RED;
//                    break;
//
//                case Format::D16_UNORM:
//					*glType = GL_DEPTH_COMPONENT16;
//                    *glClientFormat = GL_DEPTH_COMPONENT16;
//                    *interalFormat = GL_DEPTH_COMPONENT16;
//                    break;
//                case Format::D24_UNORM_S8_UINT:
//					*glType = GL_DEPTH24_STENCIL8;
//                    *glClientFormat = GL_DEPTH24_STENCIL8;
//                    *interalFormat = GL_DEPTH24_STENCIL8;
//                    break;
//                case Format::D32_FLOAT:
//                    *glType = GL_DEPTH_COMPONENT32;
//                    *glClientFormat = GL_DEPTH_COMPONENT32;
//                    *interalFormat = GL_DEPTH_COMPONENT32;
//                    break;
//                default:
//                    throw gcnew NotImplementedException();
//
//            }
//}

void GetFormat(Format format, GLenum* glClientFormat, GLenum* glType, GLenum* interalFormat)
{
	  switch (format)
            {
				case Format::R8G8B8A8_UNORM_SRGB:	
					*interalFormat =  GL_SRGB8_ALPHA8;
					*glClientFormat = GL_RGBA;
					*glType = GL_UNSIGNED_BYTE;					
					break;                                
                case Format::R8G8B8A8_UNORM:
					*interalFormat =  GL_RGBA8;
					*glClientFormat = GL_RGBA;
					*glType = GL_UNSIGNED_BYTE;		
                    break;
				case Format::R8G8B8A8_SNORM:
					*interalFormat =  GL_RGBA8;
					*glClientFormat = GL_RGBA;
					*glType = GL_BYTE;		
                    break;
				case Format::X8R8G8B8:
					*interalFormat =  GL_RGB8;
					*glClientFormat = GL_RGB;
					*glType = GL_UNSIGNED_BYTE;
                    break;				
				case Format::R8G8_UNORM: 
					*interalFormat = GL_RG8;
					*glClientFormat = GL_RG;
					*glType = GL_UNSIGNED_BYTE;
					break;
				case Format::R8G8_SNORM: 
					*interalFormat = GL_RG8;
					*glClientFormat = GL_RG;
					*glType = GL_BYTE;
					break;
				case Format::R8_UNORM: 
					*interalFormat = GL_R8;
					*glClientFormat = GL_RED;
					*glType = GL_BYTE;
					break;
				case Format::R8_SNORM: 
					*interalFormat = GL_R8;
					*glClientFormat = GL_RED;
					*glType = GL_UNSIGNED_BYTE;
					break;

				case Format::R16G16B16A16_UNORM:
					*interalFormat = GL_RGBA16UI;
					*glClientFormat = GL_RGBA;
					*glType = GL_UNSIGNED_SHORT;					                    
                    break;
				case Format::R16G16B16A16_SNORM:
					*interalFormat = GL_RGBA16I;
					*glClientFormat = GL_RGBA;
					*glType = GL_SHORT;		                    
                    break;
				case Format::R16G16B16A16_FLOAT:
					*interalFormat = GL_RGBA16F;
					*glClientFormat = GL_RGBA;
					*glType = GL_HALF_FLOAT;					                    
                    break;				
					//			
				case Format::R16G16_UNORM:
					*interalFormat = GL_RG16UI;
					*glClientFormat = GL_RG;
					*glType = GL_UNSIGNED_SHORT;					                    
                    break;
				case Format::R16G16_SNORM:
					*interalFormat = GL_RG16I;
					*glClientFormat = GL_RG;
					*glType = GL_SHORT;					                    
                    break;
				case Format::R16G16_FLOAT:
					*interalFormat = GL_RG16F;
					*glClientFormat = GL_RG;
					*glType = GL_HALF_FLOAT;
                    break;
				case Format::R16_FLOAT:
					*interalFormat = GL_R16F;
					*glClientFormat = GL_RED;
					*glType = GL_HALF_FLOAT;					                    
                    break;
                //
				case Format::R32G32B32A32_UINT:
					*interalFormat = GL_RGBA32UI;
					*glClientFormat = GL_RGBA;
					*glType = GL_UNSIGNED_INT;					                    
                    break;
				case Format::R32G32B32A32_SINT:
					*interalFormat = GL_RGBA32I;
					*glClientFormat = GL_RGBA;
					*glType = GL_INT;					                    
                    break;
				case Format::R32G32B32A32_FLOAT:
					*interalFormat = GL_RGBA32F;
					*glClientFormat = GL_RGBA;
					*glType = GL_FLOAT;					                    
                    break;
				case Format::R32G32B32_UINT:
					*interalFormat = GL_RGB32UI;
					*glClientFormat = GL_RGB;
					*glType = GL_UNSIGNED_INT;					                    
                    break;
				case Format::R32G32B32_SINT:
					*interalFormat = GL_RGB32I;
					*glClientFormat = GL_RGB;
					*glType = GL_INT;
                    break;
				case Format::R32G32B32_FLOAT:
					*interalFormat = GL_RGB32F;
					*glClientFormat = GL_RGB;
					*glType = GL_FLOAT;
                    break;
				case Format::R32G32_UINT:
					*interalFormat = GL_RG32UI;
					*glClientFormat = GL_RG;
					*glType = GL_UNSIGNED_INT;					                    
                    break;
				case Format::R32G32_SINT:
					*interalFormat = GL_RG32I;
					*glClientFormat = GL_RG;
					*glType = GL_INT;					                    
                    break;
				case Format::R32G32_FLOAT:
					*interalFormat = GL_RG32F;
					*glClientFormat = GL_RG;
					*glType = GL_FLOAT;					                    
                    break;
				case Format::R32_UINT:
					*interalFormat = GL_R32UI;
					*glClientFormat = GL_RED;
					*glType = GL_UNSIGNED_INT;			                    
                    break;
				case Format::R32_SINT:
					*interalFormat = GL_R32I;
					*glClientFormat = GL_RED;
					*glType = GL_INT;			                    
                    break;
				case Format::R32_FLOAT:
					*interalFormat = GL_R32F;
					*glClientFormat = GL_RED;
					*glType = GL_FLOAT;			                    
                    break;

				default:
                    throw gcnew NotImplementedException();
			

            }
}


int GetSize(GLenum glType)
{	
	switch (glType)
	{
	case  GL_UNSIGNED_BYTE:
 	case  GL_BYTE:
		return 1;
	case  GL_UNSIGNED_SHORT:
 	case  GL_SHORT:
	case GL_HALF_FLOAT:
		return 2;
	case  GL_UNSIGNED_INT:
 	case  GL_INT:
	case GL_FLOAT:
		return 4;

	default:
		break;
	}
}

int GetComponents(GLenum clientFormat)
{
	switch (clientFormat)
	{
		case GL_RED:
		case GL_BLUE:
		case GL_GREEN:
		case GL_ALPHA:
			return 1:
		case GL_RG:
			return 2:
		case GL_RGB:
			return 3;
		case GL_RGBA:
			return 4;
	default:
		break;
	}
}

//int GetElements(GLenum glFormat)
//{
//
//}
//
//GLenum GetTextureCubeFace(int resource);

GLenum GetUsage(ResourceUsage usage)
{
    switch (usage)
    {
	case ResourceUsage::Default:
            return GL_STATIC_DRAW;
	case ResourceUsage::Immutable:
			return GL_STATIC_DRAW;
	case ResourceUsage::Dynamic:
			return GL_DYNAMIC_DRAW;
	case ResourceUsage::Staging:
			return GL_STREAM_COPY;
    default:
           return GL_ZERO;
    }
}

int GetElements(IAFormat iAFormat)
{
	switch (iAFormat)
        {
		   case IAFormat::Color: return 4;             
            case IAFormat::Float1: return 1;
            case IAFormat::Float2: return 2;
            case IAFormat::Float3: return 3;
            case IAFormat::Float4: return 4;
            case IAFormat::HalfFour: return 4;
            case IAFormat::HalfTwo:
            case IAFormat::Short2:
            case IAFormat::Short2N: return 2;
            case IAFormat::Short4: return 4;
            case IAFormat::Short4N: return 4;
            case IAFormat::Ubyte4: return 4;
            case IAFormat::UByte4N: return 4;                
            case IAFormat::Unused: throw gcnew ArgumentException();
            case IAFormat::UShort2N: return 2;
            case IAFormat::UShort4N: return 4;
            default:
                throw gcnew ArgumentException();
        }
}

//
//int GetSize(IAFormat iAFormat);
//
GLenum GetType(IAFormat iAFormat)
{
	 switch (iAFormat)
            {
				case IAFormat::Color: return GL_UNSIGNED_BYTE;
                case IAFormat::Dec3N: return GL_INT;
                case IAFormat::Float1: 
                case IAFormat::Float2: 
                case IAFormat::Float3:
				case IAFormat::Float4: return GL_FLOAT;
                case IAFormat::HalfFour:
				case IAFormat::HalfTwo: return GL_HALF_FLOAT;
				case IAFormat::Short2: return GL_SHORT;
				case IAFormat::Short2N: return GL_UNSIGNED_SHORT;
                case IAFormat::Short4:
				case IAFormat::Short4N: return GL_SHORT;
				case IAFormat::Ubyte4: return GL_UNSIGNED_BYTE;
				case IAFormat::UByte4N: return GL_UNSIGNED_BYTE;
				case IAFormat::UDec3: return GL_INT;
                case IAFormat::Unused: throw gcnew ArgumentException();
				case IAFormat::UShort2N: return GL_UNSIGNED_SHORT;
                case IAFormat::UShort4N: return GL_UNSIGNED_SHORT;
                default:
                    throw gcnew ArgumentException();
            }
}
//
//bool GetNormalized(IAFormat iAFormat)