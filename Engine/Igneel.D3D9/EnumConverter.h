#pragma once

using namespace System;
using namespace Igneel::Graphics;

#define SAFECALL(x) if(FAILED(x)) throw gcnew InvalidOperationException(GetErr(x));
#define OVERRIDE(x) virtual x override

D3DFORMAT GetD3DFORMAT(Format f);

Format GetFORMAT(D3DFORMAT f);

void GetTextureUsage(ResourceUsage usage, BindFlags binding , CpuAccessFlags cpuAcces, OUT DWORD * d3dUsage , OUT D3DPOOL * pool);

DWORD GetD3DLOCK(MapType mapType , bool doNotWait);

D3DDEVTYPE GetD3DDEVTYPE(GraphicDeviceType devType);

D3DRESOURCETYPE GetD3DD3DRESOURCETYPE(ResourceType rsType);

DWORD GetD3DUSAGE(BindFlags binding);

void GetBufferUsage(MapType cpuAcces , DWORD*	d3dusage, D3DPOOL* pool);

String^ GetErr(HRESULT x);

