#pragma once

using namespace Igneel::Graphics;


namespace IgneelD3D10
{
	public ref class InputLayout10 : public InputLayout
	{
	internal:
		ID3D10InputLayout * _input;
	internal:
		InputLayout10(ID3D10InputLayout * input);

	protected:		
			OVERRIDE(void OnDispose(bool));
	};	

}