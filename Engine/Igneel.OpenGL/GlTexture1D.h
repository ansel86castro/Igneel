#pragma once

namespace IgneelOpenGL {
	public ref class GlTexture1D: public Texture1DBase
	{
		GLuint  _texture;
		GLuint _readBuffer;

		GLenum _target;
		GLenum _glClientFormat;
		GLenum _glType;
		GLenum _internalFormat;
		GLint _size;
		GLenum _bufferTarget;

		GLenum _mapAcces;
		GLenum _mapSubResource;
	public:		
		const int PACK_ALIGNMENT = 4;        

	public:
		GlTexture1D(Texture1DDesc desc, array<IntPtr>^ data, String^ filename);

		virtual IntPtr Map(int subResource, MapType map, bool doNotWait) override;
		virtual void UnMap()override;
	};
}
