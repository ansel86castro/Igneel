
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    public enum EmitterType
    {
        None, Directional, Spherical, Box, Radial
    }

    public class ParticleEmitter
    {
        Random rand = new Random();
        EmitterType type;
        private float angularVelocity;
        protected float linearVelocity;    
        protected Spherical emitDirection;
        protected Spherical emitDirectionWindow;
        private Vector3 boxEntends;
        private float emitRadius;               

        public EmitterType Type { get { return type; } set { type = value; } }      

        public float LinearVelocity { get { return linearVelocity; } set { linearVelocity = value; } }

        public float AngularVelocity { get { return angularVelocity; } set { angularVelocity = value; } }

        public Spherical EmitDirection { get { return emitDirection; } set { emitDirection = value; } }

        public Spherical EmitRange { get { return emitDirectionWindow; } set { emitDirectionWindow = value; } }

        public Vector3 EmitBoxExtends { get { return boxEntends; } set { boxEntends = value; } }

        public float EmitRadius { get { return emitRadius; } set { emitRadius = value; } }

        public Vector3 GetRandDirection(Spherical directionPivot, Spherical extends)
        {
            float jitHeading = -extends.Phi + (float)rand.NextDouble() * (2 * extends.Phi);
            float jitPith = -extends.Theta + (float)rand.NextDouble() * (2 * extends.Theta);
            Vector3 direction = Spherical.ToCartesian(directionPivot.Theta + jitPith, directionPivot.Phi + jitHeading);
            return direction;
        }

        public Vector3 GetRandDirection()
        {
            float jitPith = (float)rand.NextDouble() * Numerics.PI;
            float jitHeading = (float)rand.NextDouble() * Numerics.TwoPI;

            Vector3 direction = Spherical.ToCartesian(jitPith, jitHeading);
            return direction;
        }

        public Vector3 GetRandPosition(Vector3 extends)
        {
            Vector3 position = new Vector3();

            float randX = (float)rand.NextDouble();
            float randY = (float)rand.NextDouble();
            float randZ = (float)rand.NextDouble();

            position.X = -extends.X + randX * (extends.X + extends.X);
            position.Y = -extends.Y + randY * (extends.Y + extends.Y);
            position.Z = -extends.Z + randZ * (extends.Z + extends.Z);

            return position;
        }

        public virtual unsafe void Emit(Particle* ptc)
        {          
            switch (type)
            {
                case EmitterType.Directional:
                    ptc->GlobalVelocity = GetRandDirection(emitDirection, emitDirectionWindow);
                    break;
                case EmitterType.Box:
                    ptc->GlobalVelocity = GetRandDirection(emitDirection, emitDirectionWindow);
                    ptc->GlobalPosition += GetRandPosition(boxEntends);
                    break;
                case EmitterType.Spherical:
                    ptc->GlobalVelocity = GetRandDirection();
                    ptc->GlobalPosition += ptc->GlobalVelocity * emitRadius;
                    break;
            }

            ptc->Life = 0;
            ptc->AngularVelocity = angularVelocity;
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
