#pragma once

namespace IgneelOpenGL {

	public ref class GlGraphicBuffer : public GraphicBufferBase
{
internal:
	GLuint _bufferId;
	GLenum _type;

public:
	GlGraphicBuffer(					
					int size, 
					int stride, 
					GLenum type, 
					ResourceUsage usage, 
					CpuAccessFlags cpuAcces, 
					ResBinding binding, 
					IntPtr data);

	 virtual IntPtr Map(MapType map, bool doNotWait) override;

	 virtual void Unmap()override;

};

}

