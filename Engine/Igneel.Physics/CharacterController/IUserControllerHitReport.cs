using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NxExtendedVec3 = Igneel.Vector3;
using NxVec3 = Igneel.Vector3;
using NxU32 = System.UInt32;
using NxF32 = System.Single;
using System.Runtime.InteropServices;

namespace Igneel.Physics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ControllerShapeHit
    {
        public CharacterController Controller;		//!< Current controller
        public ActorShape ActorShape;	        		//!< Touched shape
        public NxExtendedVec3 WorldPos;	    	//!< Contact position in world space
        public NxVec3 WorldNormal;	                //!< Contact normal in world space       
        public NxVec3 Dir;			                //!< Motion direction
        public NxF32 Length;			            //!< Motion length
    }
    public interface IUserControllerHitReport
    {
        ControllerAction OnShapeHit(ref ControllerShapeHit hit);

        ControllerAction OnControllerHit(CharacterController controller , CharacterController other);
    }
}
