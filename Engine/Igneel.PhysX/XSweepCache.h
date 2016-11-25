#pragma once
#include "Common.h"

using namespace System;
using namespace Igneel::Physics;
using namespace Igneel;

namespace Igneel { namespace PhysX {

	public ref class XSweepCache : public SweepCache
	{
	internal:
		NxSweepCache* cache;
		NxScene* scene;

	internal:
		XSweepCache(NxScene* scene);

	public:
		virtual void SetVolume(Box box) override;

	protected:
		virtual void OnDispose(bool) override;
	};

}}