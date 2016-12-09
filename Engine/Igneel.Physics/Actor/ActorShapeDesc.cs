using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Physics
{    
    public class ActorShapeDesc
    {
        public ShapeType Type;
        public string Name;
        public ShapeFlag Flags = ShapeFlag.VISUALIZATION | ShapeFlag.CLOTH_TWOWAY | ShapeFlag.SOFTBODY_TWOWAY;
        public ushort MaterialIndex = 0;
        public float Density = 1.0f;
        /// <summary>
        /// by default we let the mass be determined by its density.  
        /// </summary>
        public float Mass = -1.0f;
        public float SkinWidth = -1.0f;
        public Matrix LocalPose = Matrix.Identity;
        public ushort CollitionGroup = 0;
        public GroupsMask GroupsMask = new GroupsMask();
        public uint NonInteractingCompartmentTypes = 0;

        public bool IsValid()
        {          
            if (CollitionGroup >= 32)
                return false;	// We only support 32 different groups
            // dsullins: I removed this bogus shapeFlags check because it was preventing core dumps from loading
            //if(shapeFlags&0xffff0000)
            //	return false;	// Only 16-bit flags supported here

            if (MaterialIndex == 0xffff)
                return false;	// 0xffff is reserved for internal usage
            if (SkinWidth != -1 && SkinWidth < 0)
                return false;

            return true;
        }
    }

    public class BoxShapeDesc : ActorShapeDesc
    {
        public Vector3 Dimensions;

        public BoxShapeDesc()
        {
            Type = ShapeType.BOX;
        }
    }

    public class SphereShapeDesc : ActorShapeDesc
    {       
        public float Radius;

        public SphereShapeDesc()
        {
            Type = ShapeType.SPHERE;
        }
    }

    public class PlaneShapeDesc : ActorShapeDesc
    {
        public Plane Plane = new Plane(0, 1, 0, 0);

        public PlaneShapeDesc()
        {
            Type = ShapeType.PLANE;
        }
    }

    public class CapsuleShapeDesc : ActorShapeDesc
    {
        public float Radius;
        public float Height;
        public CapsuleShapeDesc()
        {
            Type = ShapeType.CAPSULE;
        }
    }

    public class WheelShapeDesc : ActorShapeDesc
    {
        public float Radius = 1.0f;
        public float SuspensionTravel = 1.0f;
        public float InverseWheelMass = 1.0f;
        public WheelShapeFlags WheelFlags = 0;
        public float MotorTorque;
        public float BrakeTorque;
        public float SteerAngle;
        public SpringDesc Suspension;
        public TireFunctionDesc LongitudalTireForceFunction;
        public TireFunctionDesc LateralTireForceFunction;

        public WheelShapeDesc()
        {            
            LongitudalTireForceFunction.SetToDefault();
            LateralTireForceFunction.SetToDefault();

            Type = ShapeType.WHEEL;
        }
    }

    public class TriangleMeshShapeDesc : ActorShapeDesc
    {
        public MeshShapeFlag MeshFlags;
        public TriangleMesh Mesh;

        public TriangleMeshShapeDesc()
        {
            Type = ShapeType.MESH;
        }
    }
}
