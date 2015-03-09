#pragma once

using namespace Igneel::Graphics;
using namespace System::Runtime::InteropServices;

namespace IgneelD3D10
{
	value struct StageHolder
	{
		ID3D10ShaderResourceView** _pResouces;
		ID3D10SamplerState** _pSamplers;

		void Init()
		{
			ID3D10ShaderResourceView** pter = new ID3D10ShaderResourceView*[128];
			_pSamplers = new ID3D10SamplerState*[16];
			_pResouces = pter;
			ZeroMemory(pter, 128 * sizeof(ID3D10SamplerState*));
		}
		
		void Destroy()
		{
			delete[] _pResouces;
			delete[] _pSamplers;
		}
	};

	generic<typename T>
	public ref class ShaderStage10 abstract :ShaderStage<T> 
	{
	protected:
		ID3D10Device* _device;	
		StageHolder _holder;
		String^ _profile;
	public:
		ShaderStage10(ID3D10Device* device, String^ profile);
		!ShaderStage10();	

	protected:
		OVERRIDE(ShaderStage::ShaderStageInitialization GetStageInitialization())
		{
			ShaderStage::ShaderStageInitialization ini;
			ini.NbSamples = 16;
			ini.NbShaderResources = 128;
			return ini;
		}
		
		OVERRIDE(void OnSetSampler(int slot, SamplerState^ state));

		OVERRIDE(void OnSetSamplers(int slot, int numSamplers, array<SamplerState^,1>^ states));

		OVERRIDE(void OnSetResource(int index, ShaderResource^ resource));

		OVERRIDE(void OnSetResources(int index, int nbResources, array<ShaderResource^,1>^ resources));					

		virtual void SetSamplers(int slot, int num , ID3D10SamplerState** samplers) = 0;

		virtual void SetResources(int index, int num, ID3D10ShaderResourceView** resources) = 0;	

	public:
		OVERRIDE( ShaderCode^ CompileFromMemory(String^ shaderCode, String^ functionName , array<ShaderMacro>^ defines));

		OVERRIDE(ShaderCode^ CompileFromFile(String^ filename, String^ functionName, array<ShaderMacro>^ defines));
	};

	public ref class VSStage10 :  ShaderStage10<VertexShader^> ,IVertexShaderStage
	{
	public:
		VSStage10(ID3D10Device* device) : ShaderStage10(device, L"vs_"){ } 
	
	protected:
		OVERRIDE(void SetSamplers(int slot, int num , ID3D10SamplerState** samplers));

		OVERRIDE(void SetResources(int index, int num, ID3D10ShaderResourceView** resources));

	public:
		virtual VertexShader^ CreateShader(ShaderCode^ bytecode) override ;		
	};

	public ref class PSStage10 : public ShaderStage10<PixelShader^> , IPixelShaderStage
	{
	public:
		PSStage10(ID3D10Device* device) : ShaderStage10(device,L"ps_"){ } 
	
	protected:
		OVERRIDE(void SetSamplers(int slot, int num , ID3D10SamplerState** samplers));

		OVERRIDE(void SetResources(int index, int num, ID3D10ShaderResourceView** resources));

	public:
		virtual PixelShader^ CreateShader(ShaderCode^ bytecode)override;	

	};

	public ref class GSStage10 : public ShaderStage10<GeometryShader^> ,IGeometryShaderStage
	{
		 array<GraphicBuffer^>^ _soTargetBind;
         array<int>^ _soTargetOffets;
	public:
		GSStage10(ID3D10Device* device) : ShaderStage10(device, L"gs_")
		{ 
			_soTargetBind = gcnew array<GraphicBuffer^>(4);
			_soTargetOffets = gcnew array<int>(4);
		} 
	
	protected:
		OVERRIDE(void SetSamplers(int slot, int num , ID3D10SamplerState** samplers));

		OVERRIDE(void SetResources(int index, int num, ID3D10ShaderResourceView** resources));

	public:
		virtual GeometryShader^ CreateShader(ShaderCode^ bytecode)override;

		virtual GeometryShader^ CreateShaderWithSO(ShaderCode^ bytecode, array<StreamOutDeclaration>^ declaration ,array<int>^ bufferStrides, bool rasterizedStream0);	

		virtual property int NumberOfSOBuffers;

        virtual void SetSOBuffer(GraphicBuffer^ buffer, int offset);

        virtual void SetSOBuffer(array<GraphicBuffer^>^ buffers, array<int>^ offsets);

        virtual void GetSOBuffer(array<GraphicBuffer^>^ buffers, array<int>^ offsets);
	};

}