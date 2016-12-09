#include "Stdafx.h"
#include "Utils.h"

DXGI_FORMAT getFormat(IAFormat format)
{
	switch (format)
	{
	case Igneel::Graphics::IAFormat::Color: return DXGI_FORMAT_R8G8B8A8_UNORM;			
	case Igneel::Graphics::IAFormat::Float1: return DXGI_FORMAT_R32_FLOAT;
	case Igneel::Graphics::IAFormat::Float2: return DXGI_FORMAT_R32G32_FLOAT;	
	case Igneel::Graphics::IAFormat::Float3: return DXGI_FORMAT_R32G32B32_FLOAT;		
	case Igneel::Graphics::IAFormat::Float4: return DXGI_FORMAT_R32G32B32A32_FLOAT;		
	case Igneel::Graphics::IAFormat::HalfFour: return DXGI_FORMAT_R16G16_FLOAT;		
	case Igneel::Graphics::IAFormat::HalfTwo: return DXGI_FORMAT_R16G16B16A16_FLOAT;
	case Igneel::Graphics::IAFormat::Short2: return DXGI_FORMAT_R16G16_SINT;
	case Igneel::Graphics::IAFormat::Short2N: return DXGI_FORMAT_R16G16_SNORM;
	case Igneel::Graphics::IAFormat::Short4:  return DXGI_FORMAT_R16G16B16A16_SINT;		
	case Igneel::Graphics::IAFormat::Short4N: return DXGI_FORMAT_R16G16B16A16_SNORM;	
	case Igneel::Graphics::IAFormat::Ubyte4: return	DXGI_FORMAT_R8G8B8A8_UINT;		
	case Igneel::Graphics::IAFormat::UByte4N: DXGI_FORMAT_R8G8B8A8_UNORM;			
	case Igneel::Graphics::IAFormat::Unused: return DXGI_FORMAT_UNKNOWN;		
	case Igneel::Graphics::IAFormat::UShort2N: return DXGI_FORMAT_R16G16_UNORM;		
	case Igneel::Graphics::IAFormat::UShort4N: return DXGI_FORMAT_R16G16B16A16_UNORM;		
	default:
		throw gcnew InvalidOperationException("Formato no suportado");	
	}
}

LPCSTR getSemanticName(IASemantic semantic)
{
	switch (semantic)
	{
	case Igneel::Graphics::IASemantic::Binormal:return "BINORMAL";		
	case Igneel::Graphics::IASemantic::BlendIndices:return "BLENDINDICES";	
	case Igneel::Graphics::IASemantic::BlendWeight:return "BLENDWEIGHT";		
	case Igneel::Graphics::IASemantic::Color:return "COLOR";		
	case Igneel::Graphics::IASemantic::Depth:return "DEPTH";		
	case Igneel::Graphics::IASemantic::Fog:return "FOG";		
	case Igneel::Graphics::IASemantic::Normal: return "NORMAL";
	case Igneel::Graphics::IASemantic::PointSize: return "PSIZE";		
	case Igneel::Graphics::IASemantic::Position: return "POSITION";
	case Igneel::Graphics::IASemantic::PositionTransformed:	return "POSITIONT";
	case Igneel::Graphics::IASemantic::Sample: return "SAMPLE";
	case Igneel::Graphics::IASemantic::Tangent: return "TANGENT";		
	case Igneel::Graphics::IASemantic::TessellateFactor: return "TESSELLATEFACTOR";
	case Igneel::Graphics::IASemantic::TextureCoordinate: return "TEXCOORD";	
	default:
		throw gcnew InvalidOperationException(L"Invalid Semantic");
		break;
	}
}