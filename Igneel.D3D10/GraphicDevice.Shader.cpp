#include "Stdafx.h"
#include "GraphicDevice.h"
#include "ShaderProgram.h"
#include "Shaders.h"
#include "ShaderStages.h"
#include "GraphicBuffer10.h"

namespace IgneelD3D10 {

	ShadingInitialization GraphicDevice10::GetShadingInitialization()
	{
		ShadingInitialization ini = ShadingInitialization();
		ini.VS = gcnew VSStage10(_device);
		ini.PS = gcnew PSStage10(_device);
		ini.GS = gcnew GSStage10(_device);
		ini.CS = nullptr;
		ini.DS = nullptr;
		ini.HS = nullptr;			

		return ini;
	}

	void GraphicDevice10::SetProgram(ShaderProgram^ program)
	{
		_device->VSSetShader(NULL);
		_device->PSSetShader(NULL);
		_device->GSSetShader(NULL);

		D3DShaderProgram^ _program = static_cast<D3DShaderProgram^>(program);	

		if(program != nullptr)
		{			
			_program->SetShaders(_device);
		}		
	}

	ShaderProgram^ GraphicDevice10::CreateProgram(ShaderProgramDesc^ desc)
	{
		return gcnew D3DShaderProgram(this, desc);
	}

	void GraphicDevice10::ClearProgram(ShaderProgram^ program)
	{
		_device->VSSetShader(NULL);
		_device->PSSetShader(NULL);
		_device->GSSetShader(NULL);
	}

	 /*ShaderBuffer^ GraphicDevice10::CreateShaderBuffer(BufferDesc desc)
	 {
		 return gcnew GraphicBuffer10(_device, desc);
	 }*/
}