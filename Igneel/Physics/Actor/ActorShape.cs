using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Physics
{
    public interface ActorShape : IResourceAllocator, INameable, ITranslatable, IRotable ,IPoseable
    {
        int Id { get; }

        Actor Actor { get; }
       
        object UserData { get; set; }

        bool GetFlag(ShapeFlag f);

        void SetFlag(ShapeFlag f, bool value);

        PhysicMaterial Material { get; set; }

        float SkinWidth { get; set; }

        Matrix LocalPose { get; set; }           

        Vector3 GlobalPosition { get; set; }

        Matrix GlobalOrientation { get; set; }

        ushort CollisionGroup { get; set; }

        GroupsMask CollisionGroupMask { get; set; }

        AABB WorldBounds { get; }

        bool CheckOverlapAABB(Bounds3 worldBounds);

        bool CheckOverlapOBB(Box worldBounds);

        bool CheckOverlapSphere(Sphere sphere);

        bool CheckOverlapCapsule(Capsule capsule);              
       
    }

    public interface PlaneShape : ActorShape
    {
        Plane Plane { get; set; }
    }

    public interface BoxShape : ActorShape
    {
        Vector3 Dimensions { get; set; }
    }

    public interface SphereShape : ActorShape
    {
        float Radius { get; set; }
    }

    public interface CapsuleShape : ActorShape
    {
        float Radius { get; set; }

        float Height { get; set; }
    }

    public interface TriangleMeshShape : ActorShape
    {
        TriangleMesh Mesh { get; }
    }

    public interface WheelShape : ActorShape
    {
        float Radius { get; set; }

        float SuspensionTravel { get; set; }

        float InverseWheelMass { get; set; }

        WheelShapeFlags WheelFlags { get; set; }

        float MotorTorque { get; set; }

        float BrakeTorque { get; set; }

        TireFunctionDesc LongitudalTireForceFunction { get; set; }

        TireFunctionDesc LateralTireForceFunction { get; set; }

        /// <summary>
        /// steering angle around Y axis in radians
        /// </summary>

        float SteerAngle { get; set; }

        float AxleSpeed { get; set; }

        SpringDesc Suspension { get; set; }

        WheelContact GetContact();
    }


}
