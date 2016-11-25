#include "Stdafx.h"
#include "InputAssembler.h"

namespace Igneel { namespace D3D9 {

	D3DInputLayout::D3DInputLayout(IDirect3DDevice9* device, array<VertexElement>^ elements)
	{
		resetter = gcnew ResetTarget(this);

		e = new D3DVERTEXELEMENT9 [elements->Length + 1];		
		ZeroMemory(e + elements->Length, sizeof(D3DVERTEXELEMENT9));

		e[elements->Length].Stream  = 0xFF;
		e[elements->Length].Type = D3DDECLTYPE_UNUSED;

		for (int i = 0; i < elements->Length; i++)
		{
			e[i].Method = D3DDECLMETHOD_DEFAULT;
			e[i].Offset = elements[i].Offset;
			e[i].Stream = elements[i].Stream;
			e[i].Type = static_cast<D3DDECLTYPE>(elements[i].Format);
			e[i].Usage = static_cast<D3DDECLUSAGE>(elements[i].Semantic);
			e[i].UsageIndex = elements[i].UsageIndex;
		}

		IDirect3DVertexDeclaration9 * dec;
		SAFECALL( device->CreateVertexDeclaration(e, &dec) );
		_pVertexDecl = dec;
		
	}

	 void D3DInputLayout::OnDispose(bool disposing)
	 {
		 if(_pVertexDecl)
		{
			_pVertexDecl->Release();		 
			_pVertexDecl = NULL;

			delete e;

			if(resetter!=nullptr)
				resetter->TargetDisposed();
		}
	 }

	 void D3DInputLayout::DeviceReset(IDirect3DDevice9 * device)
	 {
		 if(!_pVertexDecl)
		 {
			 IDirect3DVertexDeclaration9 * dec;
			 SAFECALL( device->CreateVertexDeclaration(e, &dec) );
			 _pVertexDecl = dec;

			 if(Engine::Graphics->IAInputLayout == this)
			 {
				device->SetVertexDeclaration(_pVertexDecl);
			 }			
		 }
	 }

	 void D3DInputLayout::DeviceLost(IDirect3DDevice9 * device)
	 {
		 if(_pVertexDecl)
		 {						 
			 _pVertexDecl->Release();		 
			_pVertexDecl = NULL;
		 }		 
	 }
}}