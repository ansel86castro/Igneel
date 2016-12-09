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
        Actor _actor;
        WheelShape _wheelShape;      

        public RayCastWheel(WheelShape shape)
        {
            if (shape == null) throw new ArgumentNullException("shape");

            _wheelShape = shape;
            _actor = shape.Actor;
            var material = shape.Material;
            material.Flags |= MaterialFlag.DISABLE_FRICTION;
            Name = shape.Name;
            shape.InverseWheelMass = 0.1f;
            
        }

        public Actor Actor { get { return _actor; } set { _actor = value; } }

        public WheelShape ActorShape { get { return _wheelShape; } }

        public override float Radius
        {
            get { return _wheelShape.Radius; }
        }

        /// <summary>
        /// steering angle around Y axis in radians
        /// </summary>
        public override float Angle
        {
            get
            {
                return _wheelShape.SteerAngle;
            }
            set
            {
                _wheelShape.SteerAngle = -value;
            }
        }

        public override float Rpm
        {
            get { return Math.Abs(_wheelShape.AxleSpeed) / Numerics.PIover2 * 60; }
        }

        public override Vector3 Position
        {
            get { return _wheelShape.LocalPosition; }
        }
      
        public override Vector3 GetGroundContactPos()
        {
            return Position + new Vector3(0, -_wheelShape.Radius, 0);
        }

        public override Actor GetTouchedActor()
        {

            WheelContact data = _wheelShape.GetContact();
            ActorShape shape = data.Shape;

            return shape != null ? shape.Actor : null;
        }

        public override void Tick(bool handbrake, float motorTorque, float brakeTorque, float dt)
        {
            brakeTorque *= 500f;
            if (handbrake && HasFlag(WheelFlags.AffectedByHandbrake))
                brakeTorque = 1000.0f;

            if (HasFlag(WheelFlags.Accelerated))
                _wheelShape.MotorTorque = motorTorque;

            _wheelShape.BrakeTorque = brakeTorque;
        }

        public override void CommitChanges()
        {
            base.CommitChanges();

            float heightModifier = (Suspension + Radius) / Suspension;

            var suspention = _wheelShape.Suspension;
            suspention.Spring = SpringRestitution * heightModifier;
            //suspention.Damper = SpringDamping * heightModifier;
            suspention.TargetValue = SpringBias * heightModifier;
            _wheelShape.Suspension = suspention;

            _wheelShape.SuspensionTravel = Suspension;
            //wheelShape.InverseWheelMass = 0.1f;

            var lateral = _wheelShape.LateralTireForceFunction;
            lateral.StiffnessFactor *= FrictionToSide;
            _wheelShape.LateralTireForceFunction = lateral;

            var longitudinal = _wheelShape.LongitudalTireForceFunction;
            longitudinal.StiffnessFactor *= FrictionToFront;
            _wheelShape.LongitudalTireForceFunction = longitudinal;
        }
    }

}
