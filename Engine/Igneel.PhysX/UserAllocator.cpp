#include "stdafx.h"
#include "UserAllocator.h"


UserAllocator::UserAllocator(void)
{
}

void * UserAllocator::malloc(size_t size)
{
	return ::malloc(size);
}

void* UserAllocator::mallocDEBUG(size_t size, const char* fileName, int line)
{
	return ::_malloc_dbg(size, _NORMAL_BLOCK, fileName, line);
}

void* UserAllocator::realloc(void* memory, size_t size)
{
	return ::realloc(memory, size);
}

void UserAllocator::free(void* memory)
{
	::free(memory);
}