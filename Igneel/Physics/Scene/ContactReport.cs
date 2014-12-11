using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
    public struct  ContactReport
    {
        public Actor Actor1;
        public Actor Actor2;
        public Vector3 SumNormalForce;
        public Vector3 SumFrictionForce;
        public IntPtr Stream;
    }

    public struct ContactPairInfo
    {
        public ActorShape Shape1;
        public ActorShape Shape2;
        public ShapeFlag Flags;
        public Vector3 Normal;
        public Vector3 Point;
        public float Separation;
    }

    public interface IUserContactReport
    {
        void OnContactNotify(ContactReport contact, ContactPairFlag contactEvent);
    }
    
}
