using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
    public enum RayCastBit
    {
        SHAPE = (1 << 0),								//!< "shape" member of #NxRaycastHit is valid
        IMPACT = (1 << 1),								//!< "worldImpact" member of #NxRaycastHit is valid
        NORMAL = (1 << 2),								//!< "worldNormal" member of #NxRaycastHit is valid
        FACE_INDEX = (1 << 3),								//!< "faceID" member of #NxRaycastHit is valid
        DISTANCE = (1 << 4),								//!< "distance" member of #NxRaycastHit is valid
        UV = (1 << 5),								//!< "u" and "v" members of #NxRaycastHit are valid
        FACE_NORMAL = (1 << 6),								//!< Same as NX_RAYCAST_NORMAL but computes a non-smoothed normal
        MATERIAL = (1 << 7),								//!< "material" member of #NxRaycastHit is valid
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct RaycastHit
    {
        public ActorShape Shape;
        public Vector3 WorldImpact;
        public Vector3 WorldNormal;
        public int FaceID;
        public float Distance;
        public float U;
        public float V;
        public ushort MaterialIndex;
        public RayCastBit Flags;
    }

    public interface IRayCastReport
    {
        bool OnHit(RaycastHit hit);
    }
}
