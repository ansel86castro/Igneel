#pragma once

using namespace Igneel::Graphics;

//GLenum GetFormat(Format format);

void GetFormat(Format format, GLenum* glClientFormat, GLenum* glType, GLenum* interalFormat);

int GetSize(GLenum glType);

int GetComponents(GLenum clientFormat);

//int GetElements(GLenum glFormat);
//
//GLenum GetTextureCubeFace(int resource);

GLenum GetUsage(ResourceUsage usage);

int GetElements(IAFormat iAFormat);
//
//int GetSize(IAFormat iAFormat);
//
GLenum GetType(IAFormat iAFormat);
//
//bool GetNormalized(IAFormat iAFormat)
//