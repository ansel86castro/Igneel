#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;
using namespace System::Collections::Generic;

namespace IgneelD3D10{

	public ref class D3DShaderProgram : public ShaderProgram
	{				
	public:		
		GraphicDevice^ _graphicDevice;

		D3DShaderProgram(GraphicDevice^ graphicDevice, ShaderProgramDesc^ desc);

		static void RegisterSetters(ID3D10Device * device);

		OVERRIDE(IUniformSetter^ CreateUniformSetter(String^ name));

        OVERRIDE( bool IsUniformDefined(String^ name));
       
        OVERRIDE( array<UniformDesc^>^ GetUniformDescriptions() );

        OVERRIDE( UniformDesc^ GetUniformDescription(String^ name));
       
		void SetShaders(ID3D10Device* device);
	};
}