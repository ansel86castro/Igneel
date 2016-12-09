// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#include <d3d10.h>
#include <d3dx10.h>
#include <dxerr.h>
#include <dxgi.h>
#include <Windows.h>

using namespace System;
using namespace Igneel;
using namespace Igneel::Graphics;
using namespace System::Runtime::InteropServices;

#define SAFECALL(x) if(FAILED(x)){ Marshal::ThrowExceptionForHR(x);} else{ }
#define OVERRIDE(x) virtual x override

