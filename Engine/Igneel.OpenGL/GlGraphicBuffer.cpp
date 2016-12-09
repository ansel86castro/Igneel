#include "stdafx.h"
#include "GlGraphicBuffer.h"
#include "Utils.h"

namespace IgneelOpenGL {

	GlGraphicBuffer::GlGraphicBuffer(		
		int size,
		int stride, 
		GLenum type,
		ResourceUsage usage, 
		CpuAccessFlags cpuAcces, 
		ResBinding binding, 
		IntPtr data)
	{				
		_type = type;
        _lenght = size;
        _stride = stride;
        _usage = usage;
        _cpuAccesType = cpuAcces;
        _binding = binding;
		
		// This variable will hold the name of our buffer.
		GLuint buffer;
		glGenBuffers(1, &buffer);
		_bufferId = buffer;
		
		//static_cast<void*>(data);
		// succeeded here. We’re just going to bind it and hope for the best. 
		glBindBuffer(type, _bufferId); 

		// There is no storage space allocated for the buffer until we put 
		// some data into it. This copies the contents of the ‘data’ array 
		// into the buffer. 
		glBufferData(type, size, static_cast<void*>(data), GetUsage(usage)); 

		// Now, we set the vertex attribute pointer. The location is zero (we 
		// somehow know this), the size is 4 (the attribute in the vertex 
		// shader is declared as vec4), we have floating point data that is 
		// not normalized. Stride is zero because the data in this case is 
		// tightly packed. Finally, notice that we’re passing zero as the 
		// pointer to the data. This is legal because it will be interpreted as
		// an offset into ‘my_buffer’, 
		// and the data really does start at offset zero. 
		//glVertexAttribPointer(0, 4, GL_FLOAT, GL_FALSE, 0, (const GLvoid *)0);

	}

	 IntPtr GlGraphicBuffer::Map(MapType map, bool doNotWait)
	 {
		 GLenum access;
		 switch (map)
		 {
		 case MapType::Read:		
			 access = GL_READ_ONLY;
			 break;
		 case MapType::ReadWrite:
			 access = GL_READ_WRITE;
		 case MapType::Write:
			 access = GL_WRITE_ONLY;
		 case MapType::Write_Discard:
			 access = GL_MAP_WRITE_BIT | GL_MAP_INVALIDATE_BUFFER_BIT;
		 case MapType::Write_No_OverWrite:
			  access = GL_MAP_WRITE_BIT;
			 break;		
		 }
		 void* pter = glMapBufferRange(_type,0, _lenght,access);
		 return IntPtr(pter);
	 }

	 void GlGraphicBuffer::Unmap()
	 {
		 glUnmapBuffer(_type);
	 }
}