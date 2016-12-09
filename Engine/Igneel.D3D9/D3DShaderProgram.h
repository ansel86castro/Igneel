#pragma once
#include "EnumConverter.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace Igneel::Graphics;
using namespace System::Collections::Generic;

namespace Igneel { namespace D3D9 {

	public ref class D3DShaderProgram: public ShaderProgram
	{		
	public:		
		D3DShaderProgram(ShaderProgramDesc^ desc);

		static void RegisterSetters(IDirect3DDevice9 * device);

		OVERRIDE(IUniformSetter^ CreateUniformSetter(String^ name));

        OVERRIDE( bool IsUniformDefined(String^ name));
       
        OVERRIDE( array<UniformDesc^>^ GetUniformDescriptions() );

        OVERRIDE( UniformDesc^ GetUniformDescription(String^ name));

        UniformDesc^ GetUniformDescription(int index);

		void SetShaders(LPDIRECT3DDEVICE9 device);

        int GetUniformCount();		
	
		UniformDesc^ GetUniform(D3DXHANDLE handle, ID3DXConstantTable * ct);
	};
}}