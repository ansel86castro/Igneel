#include "stdafx.h"
#include "GlTexture1D.h"
#include "Utils.h"

namespace IgneelOpenGL {

	GlTexture1D::GlTexture1D(Texture1DDesc desc, array<IntPtr>^ data, String^ filename)
		:Texture1DBase(desc)
	{
		this->_location = filename;

		GLuint texture;

		GLenum glClientFormat;
		GLenum glType; 
		GLenum interalFormat;

		GetFormat(desc.Format, &glClientFormat, &glType, &interalFormat);
		_glClientFormat = glClientFormat;
		_glType = glType;
		_internalFormat = interalFormat;

		glGenTextures(1, &texture);
		_texture = texture;		

		 if (desc.ArraySize == 1)
        {
			_target = GL_TEXTURE_1D;
			glBindTexture(_target, texture);	
			glTexStorage1D(_target, desc.MipLevels,  interalFormat, desc.Width);

        }
        else if (desc.ArraySize == 6 && desc.Options == ResourceOptionFlags::TextureCube)
        { 
			_target = GL_TEXTURE_CUBE_MAP;
			throw gcnew InvalidOperationException(L"CubeTexture is not supported for 1D texture. It's supported only for 2D textures");
		}
		else
		{
			_target = GL_TEXTURE_1D_ARRAY;
			glBindTexture(_target, texture);	
			glTexStorage2D(_target, desc.MipLevels,  interalFormat, desc.Width, desc.ArraySize);
		}

		if(data!=nullptr)
		{
			if(desc.ArraySize == 1)
			{
				glTexSubImage1D(_target, 0, 0, desc.Width, glClientFormat, glType, static_cast<GLvoid*>(data[0]));
			}
			else
			{
				if(data->Length > desc.ArraySize)
					throw gcnew InvalidOperationException("Invalid data");

				for(int i = 0; i < desc.ArraySize; i++)
				{
					glTexSubImage2D(_target, 0, 0, i,desc.Width, 1,glClientFormat, glType, static_cast<GLvoid*>(data[i]));
				}
			}
		}

		//end clear the texture binding
		glBindTexture(_target, 0);	
		_size = GetSize(glType) * GetComponents(interalFormat) * desc.Width;
		if(_size % PACK_ALIGNMENT > 0)
			_size+= PACK_ALIGNMENT;
	}

	IntPtr GlTexture1D::Map(int subResource, MapType map, bool doNotWait)
	{
		GLenum target;
		GLenum access;
		GLenum bufferTarget;
		int lod = subResource;
		
		if(!_readBuffer)
		{
			GLuint buffer;
			glGenBuffers(1, &buffer);
			_readBuffer  = buffer;

			//allocate space for the buffer
			glBufferData(GL_PIXEL_UNPACK_BUFFER,_size,NULL,GL_DYNAMIC_COPY);			
		}
		if(Description.Options == ResourceOptionFlags::TextureCube)
		{
			throw gcnew InvalidOperationException("Invalid Option TextureCube");
		}
		else
		{
			//Bind the texture (possibly creating it)
			target = _target;
			glBindTexture(_target, _texture);
		}

	
		switch (map)
		{
		case Igneel::Graphics::MapType::Read:
			 access = GL_READ_ONLY;
			 bufferTarget = GL_PIXEL_PACK_BUFFER;
			 glBindBuffer(bufferTarget, _readBuffer);
			 glGetTexImage(target,lod, _glClientFormat, _glType, 0);

			break;
		case Igneel::Graphics::MapType::Write:
			 access = GL_WRITE_ONLY;
			 bufferTarget = GL_PIXEL_PACK_BUFFER;
			 glBindBuffer(bufferTarget, _readBuffer);
			 glGetTexImage(target,lod, _glClientFormat, _glType, 0);
			break;
		case Igneel::Graphics::MapType::ReadWrite:
			 access = GL_READ_WRITE;
			 bufferTarget = GL_PIXEL_PACK_BUFFER;
			 glBindBuffer(bufferTarget, _readBuffer);
			 glGetTexImage(target,lod, _glClientFormat, _glType, 0);
			break;
		case Igneel::Graphics::MapType::Write_Discard:
			 access = GL_MAP_WRITE_BIT | GL_MAP_INVALIDATE_BUFFER_BIT;
			 bufferTarget = GL_PIXEL_UNPACK_BUFFER;
			 glBindBuffer(bufferTarget, _readBuffer);
			break;
		case Igneel::Graphics::MapType::Write_No_OverWrite:
			 access = GL_MAP_WRITE_BIT;
			 bufferTarget = GL_PIXEL_PACK_BUFFER;
			 glBindBuffer(bufferTarget, _readBuffer);
			 glGetTexImage(target,lod, _glClientFormat, _glType, 0);
			break;
		default:
			break;
		}

		 void* pter = glMapBufferRange(bufferTarget ,0, _size,access);
		 _bufferTarget = bufferTarget;
		 _mapAcces  =access;
		 _mapSubResource = subResource;
		 return IntPtr(pter);
	}

	void GlTexture1D::UnMap()
	{
		if(!_readBuffer)
			throw gcnew InvalidOperationException();

		  glUnmapBuffer(_bufferTarget);

		  if(_mapAcces != GL_READ_ONLY)
		  {
			  glBindBuffer(GL_PIXEL_UNPACK_BUFFER, _readBuffer);
			  glBindTexture(_target, _texture);
			  glTexSubImage1D(_target, _mapSubResource,  0,0, _size,_glType, 0); 
		  }
		 
	}
}
