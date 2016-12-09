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
	    SteerableInput		    = (1 << 0),
	    SteerableAuto		    = (1 << 1),
	    AffectedByHandbrake	= (1 << 2),
	    Accelerated			    = (1 << 3),
	
	    BuildLowerHalf		= (1 << 8),
	    UseWheelshape		    = (1 << 9),

	    AllWheelFlags		    = SteerableInput
							    | SteerableAuto
							    | AffectedByHandbrake
							    | Accelerated,
    }

    [Serializable]
    public abstract class Wheel:INameable,IDeferreable
    {
        WheelFlags _flags;
        string _name;
        private object _userData;

        private float _suspension = 1.0f;
        private float _springRestitution=1.0f;
        private float _springBias =0;
        private float _frictionToFront = 1.0f;
        private float _frictionToSide = 1.0f;
        private float _springDamping= 0f;

        public event Action<string> NameChanged;

        public string Name 
        { 
            get { return _name; } 
            set {
                if (_name != value && NameChanged != null)
                {
                    NameChanged(value);
                }
                _name = value; 
            } 
        }

        public abstract float Angle { get; set; }

        public abstract float Rpm { get; }        

        public abstract float Radius { get; }        

        public abstract Vector3 Position { get; }        

        public WheelFlags Flags { get { return _flags; } set { _flags = value; } }

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

        public object UserData { get { return _userData; } set { _userData = value; } }

        public bool HasFlag(WheelFlags flag)
        {
            return (_flags & flag) != 0;
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
            if (_name != null)
                return _name;

            return base.ToString();
        }


    }

   
}
