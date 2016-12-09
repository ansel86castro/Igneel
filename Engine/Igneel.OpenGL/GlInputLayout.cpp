#include "Stdafx.h"
#include "GlInputLayout.h"
#include "Utils.h"

namespace IgneelOpenGL {

	void getVertexAttribParameters(VertexElement*e, GLint* out_size, GLenum* out_type, GLboolean* out_normalized)
	{
				GLint size;
				GLenum	 type;
				GLboolean normalized = false;

				 switch (e->Format)
				{
					case IAFormat::Color: 
						type = GL_UNSIGNED_BYTE;
						size = 4;		
						normalized = true;
						break;
					case IAFormat::Float1: 
						type = GL_FLOAT;
						size = 1;
						break;
					case IAFormat::Float2: 
						type = GL_FLOAT;
						size = 2;
						break;
					case IAFormat::Float3:
						type = GL_FLOAT;
						size = 3;
						break;
					case IAFormat::Float4: 
						type = GL_FLOAT;
						size = 4;
						break;
					case IAFormat::HalfFour:
						type = GL_HALF_FLOAT;
						size = 4;
						break;
					case IAFormat::HalfTwo: 
						type = GL_HALF_FLOAT;
						size = 2;
						break;
					case IAFormat::Short2: 
						type = GL_SHORT;
						size = 2;
						break;
					case IAFormat::Short2N: 
						type = GL_UNSIGNED_SHORT;
						size = 2;
						normalized = true;
						break;
					case IAFormat::Short4:
						type = GL_SHORT;
						size = 4;
						break;
					case IAFormat::Short4N: 
						type = GL_SHORT;
						size = 4;
						normalized = true;
						break;
					case IAFormat::Ubyte4: 
						type = GL_UNSIGNED_BYTE;
						size = 4;
						break;
					case IAFormat::UByte4N:
						type = GL_UNSIGNED_BYTE;
						size = 4;
						normalized = true;
						break;						
					case IAFormat::UShort2N: 
						type = GL_UNSIGNED_SHORT;
						size = 2;
						normalized = true;
						break;
					case IAFormat::UShort4N:
						type = GL_UNSIGNED_SHORT;
						size = 4;
						normalized=true;
						break;
					default:
						throw gcnew ArgumentException();
				 }

		*out_type = type;
		*out_size = size;
		*out_normalized = normalized;
	}

	GlInputLayout::GlInputLayout(array<VertexElement>^ elements)
	{
		_elements = elements;
		_size = VertexDescriptor::GetSize(elements);		
	}

	void GlInputLayout::SetupBuffer(int stream ,GLint* buffers, int count)
	{	

		CGprofile profile = cgGLGetLatestProfile(CG_GL_VERTEX);
		switch (profile)
		{
		case CG_PROFILE_ARBVP1:
			Setup_ARBVP1(stream, buffers, count);
			break;
		case CG_PROFILE_VP20:
		case CG_PROFILE_VP30:
			Setup_VP2_0(stream, buffers, count);
			break;
		case CG_PROFILE_VP40:
			Setup_VP4_0(stream, buffers, count);
			break;		
		case CG_PROFILE_GP4VP:
			Setup_VP4_0(stream, buffers, count);
			break;
		case CG_PROFILE_HLSLV:
			throw gcnew NotSupportedException(L"HLSL profile not supported");
			break;
		default:
			break;
		}
	}	

	void GlInputLayout::DisableBuffer(int stream ,GLint* buffers, int count)
	{	

		CGprofile profile = cgGLGetLatestProfile(CG_GL_VERTEX);
		switch (profile)
		{
		case CG_PROFILE_ARBVP1:
			Disable_ARBVP1(stream, buffers, count);
			break;
		case CG_PROFILE_VP20:
		case CG_PROFILE_VP30:
			Disable_VP2_0(stream, buffers, count);
			break;
		case CG_PROFILE_VP40:
			Disable_VP4_0(stream, buffers, count);
			break;		
		case CG_PROFILE_GP4VP:
			Disable_VP4_0(stream, buffers, count);
			break;
		case CG_PROFILE_HLSLV:
			throw gcnew NotSupportedException(L"HLSL profile not supported");
			break;
		default:
			break;
		}
	}	

	void GlInputLayout::Setup_ARBVP1(int stream ,GLint* buffers, int count)
	{
		int lastStream = -1;
		pin_ptr<VertexElement> pElements = &_elements[0];
		int lenght = _elements->Length;		

		GLint size;
		GLenum	 type;
		GLboolean normalized;

		 for (int i = 0; i < lenght; i++)
        {
            auto e = pElements+i;
			if(e->Stream < stream)
				continue;

			if(e->Stream != lastStream)
			{
				glBindBuffer(GL_ARRAY_BUFFER, buffers[e->Stream - stream]);
				lastStream = e->Stream;
			}

			GLvoid* offset = (GLvoid*)e->Offset;

            getVertexAttribParameters(e, &size,&type,&normalized);						

			switch (e->Semantic)
			{
				case IASemantic::Position:
				case IASemantic::PositionTransformed:
					glVertexPointer(size, type, _size, offset); 					
					glEnableClientState(GL_VERTEX_ARRAY);
					break;
				case IASemantic::Normal:
					glNormalPointer(type, size, offset);
					glEnableClientState(GL_NORMAL_ARRAY);
					break;
				case IASemantic::Tangent:
					glVertexAttribPointer(14, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(14);
					break;
				case IASemantic::Binormal:
					glVertexAttribPointer(15, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(15);
					break;
				case IASemantic::BlendIndices:
					glVertexAttribPointer(7, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(7);
					break;
				case IASemantic::BlendWeight:
					glVertexWeightPointerEXT(size, type, _size, offset);
					glEnableClientState(GL_WEIGHT_ARRAY_ARB);
					break;
				case IASemantic::Color:
					if(e->UsageIndex == 0)
					{	
						glColorPointer(size, type, _size, offset);
						glEnableClientState(GL_COLOR_ARRAY);
					}
					else if(e->UsageIndex == 1)
					{
						glSecondaryColorPointer(size, type, _size, offset);
						glEnableClientState(GL_SECONDARY_COLOR_ARRAY);
					}
					else 
						throw gcnew NotSupportedException(L"Semantic index not supported, only 0 and 1 are currently supported in profile arbvp1");
					break;
				case IASemantic::PointSize:
					glVertexAttribPointer(6, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(6);
					break;
				case IASemantic::Fog:
					glFogCoordPointer(type, _size, offset);
					glEnableClientState(GL_FOG_COORDINATE_ARRAY);
					break;
				case IASemantic::TessellateFactor:
					glVertexAttribPointer(5, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(5);
					break;
				case IASemantic::TextureCoordinate:
					glClientActiveTexture(GL_TEXTURE0_ARB + e->UsageIndex);
					glEnableClientState(GL_TEXTURE_COORD_ARRAY);
					glTexCoordPointer(size, type,_size, offset);
					break;
			default:
				throw gcnew NotSupportedException(L"Semantic '"+Enum::GetName(IASemantic::typeid, e->Semantic)+"' not supported in profile arbvp1");
				break;

			}			
        }
	}

	void GlInputLayout::Setup_VP2_0(int stream ,GLint* buffers, int count)
	{
		int lastStream = -1;
		pin_ptr<VertexElement> pElements = &_elements[0];
		int lenght = _elements->Length;		

		GLint size;
		GLenum	 type;
		GLboolean normalized;

		 for (int i = 0; i < lenght; i++)
        {
            auto e = pElements+i;
			if(e->Stream < stream)
				continue;

			if(e->Stream != lastStream)
			{
				glBindBuffer(GL_ARRAY_BUFFER, buffers[e->Stream - stream]);
				lastStream = e->Stream;
			}

			GLvoid* offset = (GLvoid*)e->Offset;

            getVertexAttribParameters(e, &size,&type,&normalized);						

			switch (e->Semantic)
			{
				case IASemantic::Position:
				case IASemantic::PositionTransformed:
					glVertexAttribPointer(0, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(0);
					break;
				case IASemantic::Normal:
					glVertexAttribPointer(2, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(2);
					break;
				case IASemantic::Tangent:
					glVertexAttribPointer(14, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(14);
					break;
				case IASemantic::Binormal:
					glVertexAttribPointer(15, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(15);
					break;
				case IASemantic::BlendIndices:
					glVertexAttribPointer(7, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(7);
					break;
				case IASemantic::BlendWeight:
					glVertexAttribPointer(1, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(1);
					break;
				case IASemantic::Color:
					if(e->UsageIndex == 0)
					{	
						glVertexAttribPointer(3, size, type, normalized, _size ,offset);
						glEnableVertexAttribArray(3);
					}
					else if(e->UsageIndex == 1)
					{
						glVertexAttribPointer(4, size, type, normalized, _size ,offset);
						glEnableVertexAttribArray(4);
					}
					else 
						throw gcnew NotSupportedException(L"Semantic index not supported, only 0 and 1 are currently supported in profile arbvp1");
					break;
				case IASemantic::PointSize:
					glVertexAttribPointer(6, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(6);
					break;
				case IASemantic::Fog:
					glVertexAttribPointer(5, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(5);
					break;
				case IASemantic::TessellateFactor:
					glVertexAttribPointer(5, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(5);
					break;
				case IASemantic::TextureCoordinate:
					glClientActiveTexture(GL_TEXTURE0_ARB + e->UsageIndex);
					glEnableClientState(GL_TEXTURE_COORD_ARRAY);
					glTexCoordPointer(size, type,_size, offset);
					break;
			default:
				throw gcnew NotSupportedException(L"Semantic '"+Enum::GetName(IASemantic::typeid, e->Semantic)+"' not supported in profile arbvp1");
				break;

			}			
        }
	}

	void GlInputLayout::Setup_VP4_0(int stream ,GLint* buffers, int count)
	{
			int lastStream = -1;
		pin_ptr<VertexElement> pElements = &_elements[0];
		int lenght = _elements->Length;		

		GLint size;
		GLenum	 type;
		GLboolean normalized;

		 for (int i = 0; i < lenght; i++)
        {
            auto e = pElements+i;
			if(e->Stream < stream)
				continue;

			if(e->Stream != lastStream)
			{
				glBindBuffer(GL_ARRAY_BUFFER, buffers[e->Stream - stream]);
				lastStream = e->Stream;
			}

			GLvoid* offset = (GLvoid*)e->Offset;

            getVertexAttribParameters(e, &size,&type,&normalized);						

			switch (e->Semantic)
			{
				case IASemantic::Position:
				case IASemantic::PositionTransformed:
					glVertexPointer(size, type, _size, offset); 					
					glEnableClientState(GL_VERTEX_ARRAY);
					break;
				case IASemantic::Normal:
					glNormalPointer(type, size, offset);
					glEnableClientState(GL_NORMAL_ARRAY);
					break;
				case IASemantic::Tangent:
					glVertexAttribPointer(14, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(14);
					break;
				case IASemantic::Binormal:
					glVertexAttribPointer(15, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(15);
					break;
				case IASemantic::BlendIndices:
					glVertexAttribPointer(7, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(7);
					break;
				case IASemantic::BlendWeight:
					glVertexWeightPointerEXT(size, type, _size, offset);
					glEnableClientState(GL_WEIGHT_ARRAY_ARB);
					break;
				case IASemantic::Color:
					if(e->UsageIndex == 0)
					{	
						glColorPointer(size, type, _size, offset);
						glEnableClientState(GL_COLOR_ARRAY);
					}
					else if(e->UsageIndex == 1)
					{
						glSecondaryColorPointer(size, type, _size, offset);
						glEnableClientState(GL_SECONDARY_COLOR_ARRAY);
					}
					else 
						throw gcnew NotSupportedException(L"Semantic index not supported, only 0 and 1 are currently supported in profile arbvp1");
					break;
				case IASemantic::PointSize:
					glVertexAttribPointer(6, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(6);
					break;
				case IASemantic::Fog:
					glFogCoordPointer(type, _size, offset);
					glEnableClientState(GL_FOG_COORDINATE_ARRAY);
					break;
				case IASemantic::TessellateFactor:
					glVertexAttribPointer(5, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(5);
					break;
				case IASemantic::TextureCoordinate:
					glClientActiveTexture(GL_TEXTURE0_ARB + e->UsageIndex);
					glEnableClientState(GL_TEXTURE_COORD_ARRAY);
					glTexCoordPointer(size, type,_size, offset);
					break;
			default:
				throw gcnew NotSupportedException(L"Semantic '"+Enum::GetName(IASemantic::typeid, e->Semantic)+"' not supported in profile arbvp1");
				break;

			}			
        }
	}

	void GlInputLayout::Setup_GP4VP(int stream ,GLint* buffers, int count)
	{
			int lastStream = -1;
		pin_ptr<VertexElement> pElements = &_elements[0];
		int lenght = _elements->Length;		

		GLint size;
		GLenum	 type;
		GLboolean normalized;

		 for (int i = 0; i < lenght; i++)
        {
            auto e = pElements+i;
			if(e->Stream < stream)
				continue;

			if(e->Stream != lastStream)
			{
				glBindBuffer(GL_ARRAY_BUFFER, buffers[e->Stream - stream]);
				lastStream = e->Stream;
			}

			GLvoid* offset = (GLvoid*)e->Offset;

            getVertexAttribParameters(e, &size,&type,&normalized);						

			switch (e->Semantic)
			{
				case IASemantic::Position:
				case IASemantic::PositionTransformed:
					glVertexAttribPointer(0, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(0);
					break;
				case IASemantic::Normal:
					glVertexAttribPointer(2, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(2);
					break;
				case IASemantic::Tangent:
					glVertexAttribPointer(14, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(14);
					break;
				case IASemantic::Binormal:
					glVertexAttribPointer(15, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(15);
					break;
				case IASemantic::BlendIndices:
					glVertexAttribPointer(7, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(7);
					break;
				case IASemantic::BlendWeight:
					glVertexAttribPointer(1, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(1);
					break;
				case IASemantic::Color:
					if(e->UsageIndex == 0)
					{	
						glVertexAttribPointer(3, size, type, normalized, _size ,offset);
						glEnableVertexAttribArray(3);
					}
					else if(e->UsageIndex == 1)
					{
							glVertexAttribPointer(4, size, type, normalized, _size ,offset);
							glEnableVertexAttribArray(4);
					}
					else 
						throw gcnew NotSupportedException(L"Semantic index not supported, only 0 and 1 are currently supported in profile arbvp1");
					break;
				case IASemantic::PointSize:
					glVertexAttribPointer(6, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(6);
					break;
				case IASemantic::Fog:
						glVertexAttribPointer(5, size, type, normalized, _size ,offset);
						glEnableVertexAttribArray(5);
					break;
				case IASemantic::TessellateFactor:
					glVertexAttribPointer(5, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(5);
					break;
				case IASemantic::TextureCoordinate:
					glClientActiveTexture(GL_TEXTURE0_ARB + e->UsageIndex);
					glEnableClientState(GL_TEXTURE_COORD_ARRAY);
					glVertexAttribPointer(8 + e->UsageIndex, size, type, normalized, _size ,offset);
					glEnableVertexAttribArray(8 + e->UsageIndex);
					break;
			default:
				throw gcnew NotSupportedException(L"Semantic '"+Enum::GetName(IASemantic::typeid, e->Semantic)+"' not supported in profile arbvp1");
				break;

			}			
        }
	}

	void GlInputLayout::Disable_ARBVP1(int stream ,GLint* buffers, int count)
	{
		int lastStream = -1;
		pin_ptr<VertexElement> pElements = &_elements[0];
		int lenght = _elements->Length;			

		 for (int i = 0; i < lenght; i++)
        {
            auto e = pElements+i;
			if(e->Stream < stream)
				continue;

			if(e->Stream != lastStream)
			{
				glBindBuffer(GL_ARRAY_BUFFER, buffers[e->Stream - stream]);
				lastStream = e->Stream;
			}						

			switch (e->Semantic)
			{
				case IASemantic::Position:
				case IASemantic::PositionTransformed:								
					glDisableClientState(GL_VERTEX_ARRAY);
					break;
				case IASemantic::Normal:					
					glDisableClientState(GL_NORMAL_ARRAY);
					break;
				case IASemantic::Tangent:					
					glDisableVertexAttribArray(14);
					break;
				case IASemantic::Binormal:					
					glDisableVertexAttribArray(15);
					break;
				case IASemantic::BlendIndices:					
					glDisableVertexAttribArray(7);
					break;
				case IASemantic::BlendWeight:					
					glDisableClientState(GL_WEIGHT_ARRAY_ARB);
					break;
				case IASemantic::Color:
					if(e->UsageIndex == 0)
					{							
						glDisableClientState(GL_COLOR_ARRAY);
					}
					else if(e->UsageIndex == 1)
					{						
						glDisableClientState(GL_SECONDARY_COLOR_ARRAY);
					}
					else 
						throw gcnew NotSupportedException(L"Semantic index not supported, only 0 and 1 are currently supported in profile arbvp1");
					break;
				case IASemantic::PointSize:					
					glDisableVertexAttribArray(6);
					break;
				case IASemantic::Fog:					
					glDisableClientState(GL_FOG_COORDINATE_ARRAY);
					break;
				case IASemantic::TessellateFactor:					
					glDisableVertexAttribArray(5);
					break;
				case IASemantic::TextureCoordinate:
					glClientActiveTexture(GL_TEXTURE0_ARB + e->UsageIndex);
					glDisableClientState(GL_TEXTURE_COORD_ARRAY);					
					break;
			default:
				throw gcnew NotSupportedException(L"Semantic '"+Enum::GetName(IASemantic::typeid, e->Semantic)+"' not supported in profile arbvp1");
				break;

			}			
	}
}

	void GlInputLayout::Disable_VP2_0(int stream ,GLint* buffers, int count)
	{
		int lastStream = -1;
		pin_ptr<VertexElement> pElements = &_elements[0];
		int lenght = _elements->Length;				

		 for (int i = 0; i < lenght; i++)
        {
            auto e = pElements+i;
			if(e->Stream < stream)
				continue;

			if(e->Stream != lastStream)
			{
				glBindBuffer(GL_ARRAY_BUFFER, buffers[e->Stream - stream]);
				lastStream = e->Stream;
			}
			
			switch (e->Semantic)
			{
				case IASemantic::Position:
				case IASemantic::PositionTransformed:					
					glDisableVertexAttribArray(0);
					break;
				case IASemantic::Normal:
					glVertexAttribPointer(2, size, type, normalized, _size ,offset);
					glDisableVertexAttribArray(2);
					break;
				case IASemantic::Tangent:
					glVertexAttribPointer(14, size, type, normalized, _size ,offset);
					glDisableVertexAttribArray(14);
					break;
				case IASemantic::Binormal:
					glVertexAttribPointer(15, size, type, normalized, _size ,offset);
					glDisableVertexAttribArray(15);
					break;
				case IASemantic::BlendIndices:
					glVertexAttribPointer(7, size, type, normalized, _size ,offset);
					glDisableVertexAttribArray(7);
					break;
				case IASemantic::BlendWeight:
					glVertexAttribPointer(1, size, type, normalized, _size ,offset);
					glDisableVertexAttribArray(1);
					break;
				case IASemantic::Color:
					if(e->UsageIndex == 0)
					{	
						glVertexAttribPointer(3, size, type, normalized, _size ,offset);
						glDisableVertexAttribArray(3);
					}
					else if(e->UsageIndex == 1)
					{
						glVertexAttribPointer(4, size, type, normalized, _size ,offset);
						glDisableVertexAttribArray(4);
					}
					else 
						throw gcnew NotSupportedException(L"Semantic index not supported, only 0 and 1 are currently supported in profile arbvp1");
					break;
				case IASemantic::PointSize:
					glVertexAttribPointer(6, size, type, normalized, _size ,offset);
					glDisableVertexAttribArray(6);
					break;
				case IASemantic::Fog:
					glVertexAttribPointer(5, size, type, normalized, _size ,offset);
					glDisableVertexAttribArray(5);
					break;
				case IASemantic::TessellateFactor:
					glVertexAttribPointer(5, size, type, normalized, _size ,offset);
					glDisableVertexAttribArray(5);
					break;
				case IASemantic::TextureCoordinate:
					glClientActiveTexture(GL_TEXTURE0_ARB + e->UsageIndex);
					glDisableClientState(GL_TEXTURE_COORD_ARRAY);					
					break;
			default:
				throw gcnew NotSupportedException(L"Semantic '"+Enum::GetName(IASemantic::typeid, e->Semantic)+"' not supported in profile arbvp1");
				break;

			}			
        }
	}

	void GlInputLayout::Disable_VP4_0(int stream ,GLint* buffers, int count)
	{
		int lastStream = -1;
		pin_ptr<VertexElement> pElements = &_elements[0];
		int lenght = _elements->Length;		
		
		 for (int i = 0; i < lenght; i++)
        {
            auto e = pElements+i;
			if(e->Stream < stream)
				continue;

			if(e->Stream != lastStream)
			{
				glBindBuffer(GL_ARRAY_BUFFER, buffers[e->Stream - stream]);
				lastStream = e->Stream;
			}
			switch (e->Semantic)
			{
				case IASemantic::Position:
				case IASemantic::PositionTransformed:					
					glDisableClientState(GL_VERTEX_ARRAY);
					break;
				case IASemantic::Normal:					
					glDisableClientState(GL_NORMAL_ARRAY);
					break;
				case IASemantic::Tangent:				
					glDisableVertexAttribArray(14);
					break;
				case IASemantic::Binormal:					
					glDisableVertexAttribArray(15);
					break;
				case IASemantic::BlendIndices:					
					glDisableVertexAttribArray(7);
					break;
				case IASemantic::BlendWeight:					
					glDisableClientState(GL_WEIGHT_ARRAY_ARB);
					break;
				case IASemantic::Color:
					if(e->UsageIndex == 0)
					{							
						glDisableClientState(GL_COLOR_ARRAY);
					}
					else if(e->UsageIndex == 1)
					{						
						glDisableClientState(GL_SECONDARY_COLOR_ARRAY);
					}
					else 
						throw gcnew NotSupportedException(L"Semantic index not supported, only 0 and 1 are currently supported in profile arbvp1");
					break;
				case IASemantic::PointSize:					
					glDisableVertexAttribArray(6);
					break;
				case IASemantic::Fog:					
					glDisableClientState(GL_FOG_COORDINATE_ARRAY);
					break;
				case IASemantic::TessellateFactor:					
					glDisableVertexAttribArray(5);
					break;
				case IASemantic::TextureCoordinate:
					glClientActiveTexture(GL_TEXTURE0_ARB + e->UsageIndex);
					glDisableClientState(GL_TEXTURE_COORD_ARRAY);					
					break;
			default:
				throw gcnew NotSupportedException(L"Semantic '"+Enum::GetName(IASemantic::typeid, e->Semantic)+"' not supported in profile arbvp1");
				break;

			}			
        }
	}

	void GlInputLayout::Disable_GP4VP(int stream ,GLint* buffers, int count)
	{
			int lastStream = -1;
		pin_ptr<VertexElement> pElements = &_elements[0];
		int lenght = _elements->Length;		

		 for (int i = 0; i < lenght; i++)
        {
            auto e = pElements+i;
			if(e->Stream < stream)
				continue;

			if(e->Stream != lastStream)
			{
				glBindBuffer(GL_ARRAY_BUFFER, buffers[e->Stream - stream]);
				lastStream = e->Stream;
			}						

			switch (e->Semantic)
			{
				case IASemantic::Position:
				case IASemantic::PositionTransformed:					
					glDisableVertexAttribArray(0);
					break;
				case IASemantic::Normal:					
					glDisableVertexAttribArray(2);
					break;
				case IASemantic::Tangent:					
					glDisableVertexAttribArray(14);
					break;
				case IASemantic::Binormal:					
					glDisableVertexAttribArray(15);
					break;
				case IASemantic::BlendIndices:					
					glDisableVertexAttribArray(7);
					break;
				case IASemantic::BlendWeight:					
					glDisableVertexAttribArray(1);
					break;
				case IASemantic::Color:
					if(e->UsageIndex == 0)
					{						
						glDisableVertexAttribArray(3);
					}
					else if(e->UsageIndex == 1)
					{							
							glDisableVertexAttribArray(4);
					}
					else 
						throw gcnew NotSupportedException(L"Semantic index not supported, only 0 and 1 are currently supported in profile arbvp1");
					break;
				case IASemantic::PointSize:
					glDisableVertexAttribArray(6);
					break;
				case IASemantic::Fog:						
						glDisableVertexAttribArray(5);
					break;
				case IASemantic::TessellateFactor:
					glVertexAttribPointer(5, size, type, normalized, _size ,offset);
					glDisableVertexAttribArray(5);
					break;
				case IASemantic::TextureCoordinate:
					glClientActiveTexture(GL_TEXTURE0_ARB + e->UsageIndex);										
					glDisableVertexAttribArray(8 + e->UsageIndex);
					break;
			default:
				throw gcnew NotSupportedException(L"Semantic '"+Enum::GetName(IASemantic::typeid, e->Semantic)+"' not supported in profile arbvp1");
				break;

			}			
        }
	}