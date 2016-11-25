#include "Stdafx.h"
#include "XSweepCache.h"

namespace Igneel { namespace PhysX {

	XSweepCache::XSweepCache(NxScene* scene)
	{
		cache = scene->createSweepCache();		
		this->scene = scene;
	}

	void XSweepCache::OnDispose(bool disposing)
	{
		scene->releaseSweepCache(cache);

		__super::OnDispose(disposing);
	}

	void XSweepCache::SetVolume(Box box)
	{
		cache->setVolume(*(NxBox*)&box);
	}
}}