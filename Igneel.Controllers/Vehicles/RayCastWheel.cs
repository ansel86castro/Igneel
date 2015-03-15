using Igneel.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Controllers
{    
    public class RayCastWheel : Wheel
    {
        Actor actor;
        WheelShape wheelShape;      

        public RayCastWheel(WheelShape shape)
        {
            if (shape == null) throw new ArgumentNullException("shape");

            wheelShape = shape;
            actor = shape.Actor;
            var material = shape.Material;
            material.Flags |= MaterialFlag.DISABLE_FRICTION;
            Name = shape.Name;
            shape.InverseWheelMass = 0.1f;
            
        }

        public Actor Actor { get { return actor; } set { actor = value; } }

        public WheelShape ActorShape { get { return wheelShape; } }

        public override float Radius
        {
            get { return wheelShape.Radius; }
        }

        /// <summary>
        /// steering angle around Y axis in radians
        /// </summary>
        public override float Angle
        {
            get
            {
                return wheelShape.SteerAngle;
            }
            set
            {
                wheelShape.SteerAngle = -value;
            }
        }

        public override float Rpm
        {
            get { return Math.Abs(wheelShape.AxleSpeed) / Numerics.PIover2 * 60; }
        }

        public override Vector3 Position
        {
            get { return wheelShape.LocalPosition; }
        }
      
        public override Vector3 GetGroundContactPos()
        {
            return Position + new Vector3(0, -wheelShape.Radius, 0);
        }

        public override Actor GetTouchedActor()
        {

            WheelContact data = wheelShape.GetContact();
            ActorShape shape = data.Shape;

            return shape != null ? shape.Actor : null;
        }

        public override void Tick(bool handbrake, float motorTorque, float brakeTorque, float dt)
        {
            brakeTorque *= 500f;
            if (handbrake && HasFlag(WheelFlags.AFFECTED_BY_HANDBRAKE))
                brakeTorque = 1000.0f;

            if (HasFlag(WheelFlags.ACCELERATED))
                wheelShape.MotorTorque = motorTorque;

            wheelShape.BrakeTorque = brakeTorque;
        }

        public override void CommitChanges()
        {
            base.CommitChanges();

            float heightModifier = (Suspension + Radius) / Suspension;

            var suspention = wheelShape.Suspension;
            suspention.Spring = SpringRestitution * heightModifier;
            //suspention.Damper = SpringDamping * heightModifier;
            suspention.TargetValue = SpringBias * heightModifier;
            wheelShape.Suspension = suspention;

            wheelShape.SuspensionTravel = Suspension;
            //wheelShape.InverseWheelMass = 0.1f;

            var lateral = wheelShape.LateralTireForceFunction;
            lateral.StiffnessFactor *= FrictionToSide;
            wheelShape.LateralTireForceFunction = lateral;

            var longitudinal = wheelShape.LongitudalTireForceFunction;
            longitudinal.StiffnessFactor *= FrictionToFront;
            wheelShape.LongitudalTireForceFunction = longitudinal;
        }
    }

}
