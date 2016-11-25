#pragma once

using namespace Igneel::Graphics;

#define FLAG_SET(x, y) (x & y) == y

DXGI_FORMAT getFormat(IAFormat format);

LPCSTR getSemanticName(IASemantic semantic);
