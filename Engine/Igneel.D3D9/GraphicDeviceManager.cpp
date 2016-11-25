#include "Stdafx.h"
#include "GraphicDeviceManager.h"
#include "GraphicDevice.h"
#include "Shaders.h"
#include <memory>

using namespace std;

namespace Igneel { namespace D3D9 {

	D3DGrahicDeviceManager::D3DGrahicDeviceManager()
	{
		_pD3D = Direct3DCreate9(D3D_SDK_VERSION);
		if(!_pD3D)
			throw gcnew GraphicDeviceFailException(L"Direct3D creation fail");		

		GC::AddMemoryPressure(sizeof(IDirect3D9));
	}

	void D3DGrahicDeviceManager::OnDispose(bool)
	{
		if(_pD3D)
			_pD3D->Release();
	}

	bool  D3DGrahicDeviceManager::CheckFormatSupport(int adapter, GraphicDeviceType devType, Format format, BindFlags binding, ResourceType type)
	{
		D3DDISPLAYMODE dm;	

		SAFECALL(_pD3D->GetAdapterDisplayMode(adapter,&dm));
		return SUCCEEDED(_pD3D->CheckDeviceFormat(adapter, GetD3DDEVTYPE(devType), dm.Format, GetD3DUSAGE(binding), GetD3DD3DRESOURCETYPE(type), GetD3DFORMAT(format)));
	}

	GraphicDevice^  D3DGrahicDeviceManager::CreateDevice(GraphicDeviceDesc^ desc)
	{
		return gcnew D3D9GraphicDevice(desc);
	}

	int D3DGrahicDeviceManager::CheckMultisampleQualityLevels(int adapter, GraphicDeviceType devType, Format format, int multySampleCount, bool windowed)
	{
		DWORD quality;

		SAFECALL(_pD3D->CheckDeviceMultiSampleType(adapter, GetD3DDEVTYPE(devType), GetD3DFORMAT(format), windowed, (D3DMULTISAMPLE_TYPE)multySampleCount, &quality));

		return quality;
	}

	bool D3DGrahicDeviceManager::CheckDeviceType(int adapter, GraphicDeviceType type, Format backBufferFormat ,bool windowed)
	{
		D3DDISPLAYMODE dm;	
		SAFECALL(_pD3D->GetAdapterDisplayMode(adapter,&dm));
		return SUCCEEDED(_pD3D->CheckDeviceType(adapter, GetD3DDEVTYPE(type), dm.Format, GetD3DFORMAT(backBufferFormat), windowed ));
	}

	/*class IncludeWrapper: public ID3DXInclude
	{
	public:
		gcroot<Include^> inst;

		virtual HRESULT Open(D3DXINCLUDE_TYPE IncludeType, LPCSTR pFileName, LPCVOID pParentData, LPCVOID *ppData, UINT *pBytes)
		{
			String^ filename = Marshal::PtrToStringAnsi(IntPtr((void*)pFileName), strlen(pFileName));

			IntPtr pter;
			int bytes; 

			interior_ptr<IntPtr> ipter = &pter;
			interior_ptr<int> ipbytes = &bytes;

			inst->Open(static_cast<Igneel::Graphics::IncludeType>(IncludeType), filename, IntPtr((void*)pParentData), &pter, &ipbytes);

			*ppData = (*pter).ToPointer();
			*pBytes = *bytes;
		}

		virtual HRESULT Close(LPCVOID pData)
		{

		}
	};*/


	DataBuffer^ D3DGrahicDeviceManager::CompileFromMemory(
												  String^ shaderCode,												  
												  array<ShaderMacro>^ defines,
												  Include^ include,
												  String^ functionName,
												  String^ profile,
												  ShaderFlags flags) 
	{		

		LPCSTR srcData = (LPCSTR)Marshal::StringToHGlobalAnsi(shaderCode).ToPointer();				
		LPCSTR srcfunctionName = (LPCSTR)Marshal::StringToHGlobalAnsi(functionName).ToPointer();
		LPCSTR srcprofile = (LPCSTR)Marshal::StringToHGlobalAnsi(profile).ToPointer();	

		ShaderMacro* mcros = NULL;
		if(defines!=nullptr)
		{
			pin_ptr<ShaderMacro> macros = &defines[0];
			mcros = macros;
		}

		LPD3DXBUFFER buffer;
		LPD3DXBUFFER err;
		LPD3DXCONSTANTTABLE ct;
		//DefaultInclude incl;
		try
		{		 
			D3DXCompileShader(srcData, shaderCode->Length, (D3DXMACRO*)mcros, NULL, srcfunctionName, srcprofile, (DWORD)flags, &buffer, &err , & ct);
		}
		/**errors = Marshal::PtrToStringAnsi(IntPtr(err->GetBufferPointer()), err->GetBufferSize());
		err->Release();*/

		finally
		{
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcData));
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcfunctionName));
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcprofile));
		}

		return gcnew D3DShaderByteCode(buffer, ct);
	}

	DataBuffer^ D3DGrahicDeviceManager::CompileFromFile(
												  String^ filename,												  
												  array<ShaderMacro>^ defines,
												  Include^ include,
												  String^ functionName,
												  String^ profile,
												  ShaderFlags flags) 
	{

		LPWSTR srcFilename = (LPWSTR)Marshal::StringToHGlobalUni(filename).ToPointer();				
		LPCSTR srcfunctionName = (LPCSTR)Marshal::StringToHGlobalAnsi(functionName).ToPointer();
		LPCSTR srcprofile = (LPCSTR)Marshal::StringToHGlobalAnsi(profile).ToPointer();	

		ShaderMacro* mcros = NULL;
		if(defines!=nullptr)
		{
			pin_ptr<ShaderMacro> macros = &defines[0];
			mcros = macros;
		}

		LPD3DXBUFFER buffer;
		LPD3DXBUFFER err;
		LPD3DXCONSTANTTABLE ct;
		//DefaultInclude incl;
		auto dir = Environment::CurrentDirectory;
		try
		{
			if(FAILED( D3DXCompileShaderFromFile(srcFilename, (D3DXMACRO*)mcros, NULL, srcfunctionName, srcprofile, (DWORD)flags, &buffer, &err , & ct) ))
			 {
				 String^ errString = Marshal::PtrToStringAnsi(IntPtr(err->GetBufferPointer()), err->GetBufferSize());
				 throw gcnew InvalidOperationException(errString);
			 }
		}
		/**errors = Marshal::PtrToStringAnsi(IntPtr(err->GetBufferPointer()), err->GetBufferSize());
		err->Release();*/

		finally
		{
			Environment::CurrentDirectory = dir;
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcFilename));
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcfunctionName));
			Marshal::FreeHGlobal(static_cast<IntPtr>((void*)srcprofile));
		}


		return gcnew D3DShaderByteCode(buffer, ct);
	}

	HRESULT DefaultInclude::Open(D3DXINCLUDE_TYPE IncludeType, LPCSTR pFileName,LPCVOID pParentData,LPCVOID * ppData,UINT * pBytes)
	{
		/*String^ filename = Marshal::PtrToStringAnsi(static_cast<IntPtr>((void*)pFileName),strlen(pFileName));		
		auto buffer =  System::IO::File::ReadAllBytes(filename);
		void* pbuffer = malloc(buffer->Length);*/
		
		HANDLE hfile = CreateFileA(pFileName, GENERIC_READ, FILE_SHARE_READ , NULL, OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL|FILE_FLAG_SEQUENTIAL_SCAN , NULL);
		if(hfile == INVALID_HANDLE_VALUE)
		{
			return S_FALSE;
		}

		DWORD size = GetFileSize(hfile, NULL);
		void* buffer = malloc(size);
		DWORD nbBytes;

		if(!ReadFile(hfile, buffer, size, &nbBytes, NULL) || size != nbBytes)
		{
			CloseHandle(hfile);
			free(buffer);
			return S_FALSE;
		}

		*ppData = buffer;
		*pBytes = size;

		CloseHandle(hfile);

		return S_OK;
	}

	 HRESULT DefaultInclude::Close(LPCVOID pData)
	 {
		 return S_OK;
	 }

}}
