using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NxReal = System.Single;

namespace Igneel.Physics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct GroupsMask
    {
        uint Bits0, Bits1, Bits2, Bits3;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SpringDesc
    {
        public float Spring;
        public float Damper;
        public float TargetValue;

        public bool IsValid()
        {
            return Spring >= 0 && Damper >= 0;
        }
    }

    [Serializable]
    [StructLayout( LayoutKind.Sequential)]
    public struct WheelContact
    {
        public Vector3 ContactPoint;

        /**
        \brief The normal at the point of contact.

        */
        public Vector3 ContactNormal;

        /**
        \brief The direction the wheel is pointing in.
        */
        public Vector3 LongitudalDirection;

        /**
        \brief The sideways direction for the wheel(at right angles to the longitudinal direction).
        */
        public Vector3 LateralDirection;

        /**
        \brief The magnitude of the force being applied for the contact.
        */
        public float ContactForce;

        /**
        \brief What these exactly are depend on NX_WF_INPUT_LAT_SLIPVELOCITY and NX_WF_INPUT_LNG_SLIPVELOCITY flags for the wheel.
        */
        public float LongitudalSlip, LateralSlip;

        /**
        \brief the clipped impulses applied at the wheel.
        */
        public float LongitudalImpulse, LateralImpulse;

        /**
        \brief The material index of the shape in contact with the wheel.

        @see NxMaterial NxMaterialIndex
        */
        public ushort OtherShapeMaterialIndex;

        /**
        \brief The distance on the spring travel distance where the wheel would end up if it was resting on the contact point.
        */
        public float ContactPosition;

        public ActorShape Shape;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct TireFunctionDesc
    {
        /**
	    \brief extremal point of curve.  Both values must be positive.

	    <b>Range:</b> (0,inf)<br>
	    <b>Default:</b> 1.0
	*/
        public float ExtremumSlip;

        /**
        \brief extremal point of curve.  Both values must be positive.

        <b>Range:</b> (0,inf)<br>
        <b>Default:</b> 0.02
        */
        public float ExtremumValue;

        /**
        \brief point on curve at which for all x > minumumX, function equals minimumY.  Both values must be positive.

        <b>Range:</b> (0,inf)<br>
        <b>Default:</b> 2.0
        */
        public float AsymptoteSlip;

        /**
        \brief point on curve at which for all x > minumumX, function equals minimumY.  Both values must be positive.

        <b>Range:</b> (0,inf)<br>
        <b>Default:</b> 0.01
        */
        public float AsymptoteValue;


        /**
        \brief Scaling factor for tire force.
	
        This is an additional overall positive scaling that gets applied to the tire forces before passing 
        them to the solver.  Higher values make for better grip.  If you raise the *Values above, you may 
        need to lower this. A setting of zero will disable all friction in this direction.

        <b>Range:</b> (0,inf)<br>
        <b>Default:</b> 1000000.0 (quite stiff by default)
        */
        public float StiffnessFactor;

        public void SetToDefault()
        {
            ExtremumSlip = 1.0f;
            ExtremumValue = 0.02f;
            AsymptoteSlip = 2.0f;
            AsymptoteValue = 0.01f;
            StiffnessFactor = 1000000.0f;	//quite stiff by default.
        }

        public NxReal HermiteEval(NxReal t)
        {

            // This fix for TTP 3429 & 3675 is from Sega.
            // Assume blending functions (look these up in a graph):
            // H0(t) =  2ttt - 3tt + 1
            // H1(t) = -2ttt + 3tt
            // H2(t) =   ttt - 2tt + t
            // H3(t) =   ttt -  tt 

            NxReal v = Math.Abs(t);
            NxReal s = (t >= 0) ? 1.0f : -1.0f;

            NxReal F;

            if (v < ExtremumSlip)
            {
                // For t in the interval 0 < t < extremumSlip
                // We normalize t:
                // a = t/extremumSlip;
                // and use H1 + H2 to compute F:
                // F = extremumValue * ( H1(a) + H2(a) )

                NxReal a = v / ExtremumSlip;
                NxReal a2 = a * a;
                NxReal a3 = a * a2;

                F = ExtremumValue * (-a3 + a2 + a);
            }
            else
            {
                if (v < AsymptoteSlip)
                {
                    // For the interval extremumSlip <= t < asymtoteSlip
                    // We normalize and remap t:
                    // a = (t-extremumSlip)/(asymptoteSlip - extremumSlip)
                    // and use H0 to compute F:
                    // F = extremumValue + (extremumValue - asymtoteValue) * H0(a)
                    // note that the above differs from the actual expression but this is how it looks with H0 factorized.

                    NxReal a = (v - ExtremumSlip) / (AsymptoteSlip - ExtremumSlip);
                    NxReal a2 = a * a;
                    NxReal a3 = a * a2;

                    NxReal diff = AsymptoteValue - ExtremumValue;
                    F = -2.0f * diff * a3 + 3.0f * diff * a2 + ExtremumValue;
                }
                else
                {
                    F = AsymptoteValue;
                }
            }
            return s * F;
        }
    }
}
