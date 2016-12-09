using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components.Particles
{
    public enum EmitterType
    {
        None, Directional, Spherical, Box, Radial
    }

    public class ParticleEmitter
    {
        Random _rand = new Random();
        EmitterType _type;
        private float _angularVelocity;
        protected float linearVelocity;
        protected Spherical emitDirection;
        protected Spherical EmitDirectionWindow;
        private Vector3 _boxEntends;
        private float _emitRadius;

        public EmitterType Type { get { return _type; } set { _type = value; } }

        public float LinearVelocity { get { return linearVelocity; } set { linearVelocity = value; } }

        public float AngularVelocity { get { return _angularVelocity; } set { _angularVelocity = value; } }

        public Spherical EmitDirection { get { return emitDirection; } set { emitDirection = value; } }

        public Spherical EmitRange { get { return EmitDirectionWindow; } set { EmitDirectionWindow = value; } }

        public Vector3 EmitBoxExtends { get { return _boxEntends; } set { _boxEntends = value; } }

        public float EmitRadius { get { return _emitRadius; } set { _emitRadius = value; } }

        public Vector3 GetRandDirection(Spherical directionPivot, Spherical extends)
        {
            float jitHeading = -extends.Phi + (float)_rand.NextDouble() * (2 * extends.Phi);
            float jitPith = -extends.Theta + (float)_rand.NextDouble() * (2 * extends.Theta);
            Vector3 direction = Spherical.ToCartesian(directionPivot.Theta + jitPith, directionPivot.Phi + jitHeading);
            return direction;
        }

        public Vector3 GetRandDirection()
        {
            float jitPith = (float)_rand.NextDouble() * Numerics.PI;
            float jitHeading = (float)_rand.NextDouble() * Numerics.TwoPI;

            Vector3 direction = Spherical.ToCartesian(jitPith, jitHeading);
            return direction;
        }

        public Vector3 GetRandPosition(Vector3 extends)
        {
            Vector3 position = new Vector3();

            float randX = (float)_rand.NextDouble();
            float randY = (float)_rand.NextDouble();
            float randZ = (float)_rand.NextDouble();

            position.X = -extends.X + randX * (extends.X + extends.X);
            position.Y = -extends.Y + randY * (extends.Y + extends.Y);
            position.Z = -extends.Z + randZ * (extends.Z + extends.Z);

            return position;
        }

        public virtual unsafe void Emit(Particle* ptc)
        {
            switch (_type)
            {
                case EmitterType.Directional:
                    ptc->GlobalVelocity = GetRandDirection(emitDirection, EmitDirectionWindow);
                    break;
                case EmitterType.Box:
                    ptc->GlobalVelocity = GetRandDirection(emitDirection, EmitDirectionWindow);
                    ptc->GlobalPosition += GetRandPosition(_boxEntends);
                    break;
                case EmitterType.Spherical:
                    ptc->GlobalVelocity = GetRandDirection();
                    ptc->GlobalPosition += ptc->GlobalVelocity * _emitRadius;
                    break;
            }

            ptc->Life = 0;
            ptc->AngularVelocity = _angularVelocity;
            ptc->GlobalVelocity *= linearVelocity;
        }

        public Particle Emit(Particle ptc)
        {
            unsafe
            {
                Emit(&ptc);
            }

            return ptc;
        }

    }        
}
