using System;
namespace Igneel.Physics
{
    public interface IActor
    {
        void AddForce(global::Igneel.Vector3 force, global::Igneel.Vector3 pos, global::Igneel.Physics.ForceMode mode = ForceMode.FORCE, bool wakeup = true);
        void AddForceAtLocalPos(global::Igneel.Vector3 force, global::Igneel.Vector3 pos, global::Igneel.Physics.ForceMode mode = ForceMode.FORCE, bool wakeup = true);
        void AddForceAtPos(global::Igneel.Vector3 force, global::Igneel.Vector3 pos, global::Igneel.Physics.ForceMode mode = ForceMode.FORCE, bool wakeup = true);
        void AddLocalForce(global::Igneel.Vector3 force, global::Igneel.Vector3 pos, global::Igneel.Physics.ForceMode mode = ForceMode.FORCE, bool wakeup = true);
        void AddLocalForceAtLocalPos(global::Igneel.Vector3 force, global::Igneel.Vector3 pos, global::Igneel.Physics.ForceMode mode = ForceMode.FORCE, bool wakeup = true);
        void AddLocalForceAtPos(global::Igneel.Vector3 force, global::Igneel.Vector3 pos, global::Igneel.Physics.ForceMode mode = ForceMode.FORCE, bool wakeup = true);
        void AddLocalTorque(global::Igneel.Vector3 torque, global::Igneel.Vector3 pos, global::Igneel.Physics.ForceMode mode = ForceMode.FORCE, bool wakeup = true);
        void AddTorque(global::Igneel.Vector3 torque, global::Igneel.Vector3 pos, global::Igneel.Physics.ForceMode mode = ForceMode.FORCE, bool wakeup = true);
        global::Igneel.IAffectable Affectable { get; set; }
        float AngularDamping { get; set; }
        global::Igneel.Vector3 AngularMomentum { get; set; }
        global::Igneel.Vector3 AngularVelocity { get; set; }
        global::Igneel.Physics.BodyActorFlags BodyFlags { get; set; }
        float CCDMotionThreshold { get; set; }
        void ClearActorFlag(global::Igneel.Physics.ActorFlag flag);
        void ClearBodyFlag(global::Igneel.Physics.BodyActorFlags flag);
        global::Igneel.Matrix CMassGlobalOrientation { get; set; }
        global::Igneel.Matrix CMassGlobalPose { get; set; }
        global::Igneel.Vector3 CMassGlobalPosition { get; set; }
        global::Igneel.Matrix CMassLocalPose { get; }
        float ComputeKineticEnergy();
        global::Igneel.Physics.ContactPairFlag ContactReportFlags { get; set; }
        float ContactReportThreshold { get; set; }
        ushort DominanceGroup { get; set; }
        global::Igneel.Physics.ActorFlag Flags { get; set; }
        global::Igneel.Matrix GetGlobalInertiaTensor();
        global::Igneel.Matrix GetGlobalInertiaTensorInverse();
        global::Igneel.Vector3 GetLocalPointVelocity(global::Igneel.Vector3 point);
        global::Igneel.Vector3 GetPointVelocity(global::Igneel.Vector3 point);
        global::Igneel.Matrix GlobalOrientation { get; set; }
        global::Igneel.Quaternion GlobalOrientationQuat { get; set; }
        global::Igneel.Matrix GlobalPose { get; }
        global::Igneel.Vector3 GlobalPosition { get; set; }
        ushort Group { get; set; }
        bool IsDynamic { get; }
        bool IsGroupSleeping { get; }
        bool IsKinematic { get; set; }
        bool IsSleeping { get; }
        float LinearDamping { get; set; }
        global::Igneel.Vector3 LinearMomentum { get; set; }
        global::Igneel.Vector3 LinearVelocity { get; set; }
        float Mass { get; set; }
        global::Igneel.Vector3 MassSpaceInertiaTensor { get; set; }
        float MaxAngularVelocity { get; set; }
        void MoveGlobalOrientation(global::Igneel.Matrix rotationMat);
        void MoveGlobalOrientationQuat(global::Igneel.Quaternion v);
        void MoveGlobalPose(global::Igneel.Matrix pose);
        void MoveGlobalPosition(global::Igneel.Vector3 position);
        void PutToSleep();
        void RaiseActorFlag(global::Igneel.Physics.ActorFlag flag);
        void RaiseBodyFlag(global::Igneel.Physics.BodyActorFlags flag);
        bool ReadActorFlag(global::Igneel.Physics.ActorFlag flag);
        bool ReadBodyFlag(global::Igneel.Physics.BodyActorFlags flag);
        void ResetUserActorPairFiltering();
        void SetCMassOffsetGlobalOrientation(global::Igneel.Matrix mat);
        void SetCMassOffsetGlobalPose(global::Igneel.Matrix mat);
        void SetCMassOffsetGlobalPosition(global::Igneel.Vector3 v);
        void SetCMassOffsetLocalOrientation(global::Igneel.Matrix mat);
        void SetCMassOffsetLocalPose(global::Igneel.Matrix mat);
        void SetCMassOffsetLocalPosition(global::Igneel.Vector3 v);
        float SleepAngularVelocity { get; set; }
        float SleepEnergyThreshold { get; set; }
        float SleepLinearVelocity { get; set; }
        int SolverIterationCount { get; set; }
        void UpdateMassFromShapes(float density, float totalMass);
        void WakeUp(float wakeCounterValue = 20.0f*0.02f);
    }
}
