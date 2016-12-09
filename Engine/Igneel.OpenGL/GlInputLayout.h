#pragma once
#include "GlGraphicBuffer.h"

namespace IgneelOpenGL {
	public ref class GlInputLayout:public InputLayout
	{
		array<VertexElement>^ _elements;
		int _size;
	public:
		GlInputLayout(array<VertexElement>^ elements);

		void SetupBuffer(int stream ,GLint* buffers, int count);

		void DisableBuffers(int stream ,GLint* buffers, int count);

		void DisableAllAttribs();
	private:
		void Setup_ARBVP1(int stream ,GLint* buffers, int count);
		void Setup_VP2_0(int stream ,GLint* buffers, int count);		
		void Setup_VP4_0(int stream ,GLint* buffers, int count);		
		void Setup_GP4VP(int stream ,GLint* buffers, int count);

		void DisableBuffer(int stream ,GLint* buffers, int count);
		void Disable_ARBVP1(int stream ,GLint* buffers, int count);
		void Disable_VP2_0(int stream ,GLint* buffers, int count);
		void Disable_VP4_0(int stream ,GLint* buffers, int count);
		void Disable_GP4VP(int stream ,GLint* buffers, int count);


	};
}