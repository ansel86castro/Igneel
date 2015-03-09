// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once
#include <dinput.h>
#include <winerror.h>
#include <strsafe.h>

using namespace System;
using namespace Igneel;
using namespace Igneel::Input;
using namespace Igneel::Windows;
using namespace System::Runtime::InteropServices;

#define SAFECALL(x) if(FAILED(x)) throw gcnew InvalidOperationException(L"Invalid Call")
#define OVERRIDE(x) virtual x override
