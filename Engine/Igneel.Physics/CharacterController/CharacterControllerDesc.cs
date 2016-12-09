using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NxExtendedVec3 = Igneel.Vector3;
using NxF32 = System.Single;

namespace Igneel.Physics
{
    public class CharacterControllerDesc
    {
        public string Name;
        public NxExtendedVec3 Position = new NxExtendedVec3();
        public HeightFieldAxis UpDirection = HeightFieldAxis.Y;
        public NxF32 SlopeLimit = 0.707f;
        public NxF32 SkinWidth = 0.1f;
        public NxF32 StepOffset = 0.5f;
        public CCTInteraction InteractionFlag = CCTInteraction.INTERACTION_INCLUDE;
        public IUserControllerHitReport HitReport;
        public ControllerType Type { get; protected set; }
    }


    public class BoxControllerDesc : CharacterControllerDesc
    {
        public Vector3 Extents = new Vector3(0.5f, 1f, 0.5f);

        public BoxControllerDesc()
        {
            Type = ControllerType.BOX;
        }
    }

    public class CapsuleControllerDesc : CharacterControllerDesc
    {
        public NxF32 Radius = 0;
        public NxF32 Height = 0;
        public CapsuleClimbingMode ClimbingMode = CapsuleClimbingMode.CLIMB_CONSTRAINED;

        public CapsuleControllerDesc()
        {
            Type = ControllerType.CAPSULE;
        }
    }
}
