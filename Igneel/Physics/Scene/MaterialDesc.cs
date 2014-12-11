using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NxReal = System.Single;
using NxVec3 = Igneel.Vector3;
using NxU32 = System.UInt32;
using System.Runtime.InteropServices;

namespace Igneel.Physics
{
    [Serializable]
    [StructLayout( LayoutKind.Sequential)]
    public class PhysicMaterialDesc
    {
        public float DynamicFriction;
        public NxReal StaticFriction;
        public NxReal Restitution;
        public NxReal DynamicFrictionV;
        public NxReal StaticFrictionV;
        public NxVec3 DirOfAnisotropy;
        public MaterialFlag Flags;
        public CombineMode FrictionCombineMode;
        public CombineMode RestitutionCombineMode;

        public PhysicMaterialDesc()
        {
            DynamicFriction = 0.0f;
            StaticFriction = 0.0f;
            Restitution = 0.0f;

            DynamicFrictionV = 0.0f;
            StaticFrictionV = 0.0f;

            DirOfAnisotropy = new NxVec3(1, 0, 0);
            Flags = 0;
            FrictionCombineMode = CombineMode.AVERAGE;
            RestitutionCombineMode = CombineMode.AVERAGE;
        }
    }
}
