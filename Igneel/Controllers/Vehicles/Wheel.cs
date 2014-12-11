using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Physics;

namespace Igneel.Controllers
{
    [Flags]
    public enum WheelFlags 
    {
	    STEERABLE_INPUT		    = (1 << 0),
	    STEERABLE_AUTO		    = (1 << 1),
	    AFFECTED_BY_HANDBRAKE	= (1 << 2),
	    ACCELERATED			    = (1 << 3),
	
	    BUILD_LOWER_HALF		= (1 << 8),
	    USE_WHEELSHAPE		    = (1 << 9),

	    ALL_WHEEL_FLAGS		    = STEERABLE_INPUT
							    | STEERABLE_AUTO
							    | AFFECTED_BY_HANDBRAKE
							    | ACCELERATED,
    }

    [Serializable]
    public abstract class Wheel:INameable,IDeferreable
    {
        WheelFlags flags;
        string name;
        private object userData;

        private float _suspension = 1.0f;
        private float _springRestitution=1.0f;
        private float _springBias =0;
        private float _frictionToFront = 1.0f;
        private float _frictionToSide = 1.0f;
        private float _springDamping= 0f;

        public event Action<string> NameChanged;

        public string Name 
        { 
            get { return name; } 
            set {
                if (name != value && NameChanged != null)
                {
                    NameChanged(value);
                }
                name = value; 
            } 
        }

        public abstract float Angle { get; set; }

        public abstract float Rpm { get; }        

        public abstract float Radius { get; }        

        public abstract Vector3 Position { get; }        

        public WheelFlags Flags { get { return flags; } set { flags = value; } }

        public abstract void Tick(bool handbrake, float motorTorque, float brakeTorque, float dt);

        public abstract Actor GetTouchedActor();

        public abstract Vector3 GetGroundContactPos();

        public float Suspension
        {
            get { return _suspension; }
            set { _suspension = value; }
        }

        public float SpringRestitution
        {
            get { return _springRestitution; }
            set { _springRestitution = value; }
        }

        public float SpringBias
        {
            get { return _springBias; }
            set { _springBias = value; }
        }

        public float FrictionToFront
        {
            get { return _frictionToFront; }
            set { _frictionToFront = value; }
        }

        public float FrictionToSide
        {
            get { return _frictionToSide; }
            set { _frictionToSide = value; }
        }

        public float SpringDamping
        {
            get { return _springDamping; }
            set { _springDamping = value; }
        }

        public object UserData { get { return userData; } set { userData = value; } }

        public bool HasFlag(WheelFlags flag)
        {
            return (flags & flag) != 0;
        }

        public bool HasGroundContact()
        {
            return GetTouchedActor() != null;
        }

        public virtual void CommitChanges()
        {
           
        }

        public override string ToString()
        {
            if (name != null)
                return name;

            return base.ToString();
        }


    }

   
}
