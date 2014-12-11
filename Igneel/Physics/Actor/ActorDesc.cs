using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Physics
{
    public class ActorDesc
    {
        public string Name;
        public float Density;
        public Matrix GlobalPose = Matrix.Identity;
        public ActorFlag Flags;
        public ushort CollitionGroup;
        public ushort DominanceGroup;
        public ContactPairFlag ContactReportFlags;
        public ushort ForceFieldMaterial;
        public List<ActorShapeDesc> Shapes = new List<ActorShapeDesc>();
        public BodyDesc Body;
    }

    public class BodyDesc
    {
        public Matrix MassLocalPose = Matrix.Identity;
        public Vector3 MassSpaceInertia = new Vector3();
        public float Mass = 0.0f;
        public Vector3 LinearVelocity = new Vector3();
        public Vector3 AngularVelocity = new Vector3();
        public float WakeUpCounter = 20.0f * 0.02f;
        public float LinearDamping = 0.0f;
        public float AngularDamping = 0.05f;
        public float MaxAngularVelocity = -1.0f;
        public float CCDMotionThreshold = 0.0f;
        public BodyActorFlags Flags = BodyActorFlags.VISUALIZATION | BodyActorFlags.ENERGY_SLEEP_TEST;
        public float SleepLinearVelocity = -1.0f;
        public float SleepAngularVelocity = -1.0f;
        public int SolverIterationCount = 4;
        public float SleepEnergyThreshold = -1.0f;
        public float SleepDamping = 0.0f;
        public float ContactReportThreshold = float.MaxValue;
    }
}
