#pragma once
#include "NxUserAllocator.h"
#include <crtdbg.h>

class UserAllocator :public NxUserAllocator
{
public:
	UserAllocator(void);
	
	virtual void* mallocDEBUG(size_t size, const char* fileName, int line);
	virtual void* malloc(size_t size) ;
	virtual void* realloc(void* memory, size_t size);
	virtual void free(void* memory);
};

