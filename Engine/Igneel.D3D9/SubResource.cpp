#include "stdafx.h"
#include "SubResource.h"
#include "D3D9Textures.h"

namespace Igneel { namespace D3D9 {
	
	void D3DSubResource::OnDispose(bool disposing)
	{
		if(_pSurface)
			_pSurface->Release();	
	}

	//******************  sub resource1d
	D3DSubResource1D::D3DSubResource1D(int level, Texture1D^ texture)
	{
		this->_level = level;

		auto tex = static_cast<D3D9Texture1D^>(texture);

		tex->TextureLost += gcnew Action<Texture1D^>(this, &D3DSubResource1D::TextureLost);
		tex->TextureReset += gcnew Action<Texture1D^>(this, &D3DSubResource1D::TextureReset);

		auto surf = _pSurface;
		SAFECALL( tex->_pTexture->GetSurfaceLevel(_level, &surf) );
		_pSurface = surf;

		_level = level;
		_texture = texture;
	}

	void D3DSubResource1D::TextureLost(Texture1D^ tex)
	{
		SubResourceLost(this);

		if(_pSurface)
			_pSurface->Release();
	}

	void D3DSubResource1D::TextureReset(Texture1D^ tex)
	{
		auto texImp = static_cast<D3D9Texture1D^>(tex);

		auto surf = _pSurface;
		SAFECALL( texImp->_pTexture->GetSurfaceLevel(_level, &surf)) ;
		_pSurface = surf;

		SubResourceReset(this);
	}

	//************* sub resource 2d ************************************
	D3DSubResource2D::D3DSubResource2D(int level, Texture2D^ texture)
	{
		this->_level = level;

		auto tex = static_cast<D3D9Texture2D^>(texture);

		tex->TextureLost += gcnew Action<Texture2D^>(this, &D3DSubResource2D::TextureLost);
		tex->TextureReset += gcnew Action<Texture2D^>(this, &D3DSubResource2D::TextureReset);

		auto surf = _pSurface;
		SAFECALL( tex->_pTexture->GetSurfaceLevel(_level, &surf));
		_pSurface = surf;

		_level = level;
		_texture = texture;
	}

	void D3DSubResource2D::TextureLost(Texture2D^ tex)
	{
		SubResourceLost(this);

		if(_pSurface)
			_pSurface->Release();
	}

	void D3DSubResource2D::TextureReset(Texture2D^ tex)
	{
		auto texImp = static_cast<D3D9Texture2D^>(tex);

		auto surf = _pSurface;
		SAFECALL( texImp->_pTexture->GetSurfaceLevel(_level, &surf) );
		_pSurface = surf;

		SubResourceReset(this);
	}

	//************** 3d ******************************
	D3DSubResourceCube::D3DSubResourceCube(D3DCUBEMAP_FACES face , int level, TextureCube^ texture)
	{
		this->_level = level;

		auto tex = static_cast<D3D9TextureCube^>(texture);

		tex->TextureLost += gcnew Action<TextureCube^>(this, &D3DSubResourceCube::TextureLost);
		tex->TextureReset += gcnew Action<TextureCube^>(this, &D3DSubResourceCube::TextureReset);

		auto surf = _pSurface;		
		SAFECALL( tex->_pTexture->GetCubeMapSurface(face, level, &surf));			
		_pSurface = surf;

		_level = level;
		_texture = texture;
		_face = face;
	}

	void D3DSubResourceCube::TextureLost(TextureCube^ tex)
	{
		SubResourceLost(this);

		if(_pSurface)
			_pSurface->Release();
	}

	void D3DSubResourceCube::TextureReset(TextureCube^ tex)
	{
		auto texImp = static_cast<D3D9TextureCube^>(tex);

		auto surf = _pSurface;		
		SAFECALL( texImp->_pTexture->GetCubeMapSurface(_face, _level, &surf) );			
		_pSurface = surf;

		SubResourceReset(this);
	}
}}