using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Igneel.Physics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SweepQueryHit
    {
        public float T;					//!< Distance to hit expressed as a percentage of the source motion vector ([0,1] coeff)
        public ActorShape HitShape;			//!< Hit shape
        public ActorShape SweepShape;			//!< Only nonzero when using NxActor::linearSweep. Shape from NxActor that hits the hitShape.

        public int InternalFaceID;		//!< ID of touched triangle (internal)
        public int FaceID;				//!< ID of touched triangle (external)
        public Vector3 Point;				//!< World-space impact point
        public Vector3 Normal;				//!< World-space impact normal
    }
   
}
