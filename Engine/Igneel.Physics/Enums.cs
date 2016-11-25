using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
   public enum SimulationType
	{
		SIMULATION_SW	= 0,		//!< Create a software master PhysicScene.
		SIMULATION_HW	= 1,		//!< Create a hardware master PhysicScene.	
		NX_STY_FORCE_DWORD = 0x7fffffff			//!< Just to make sure sizeof(enum) == 4, not a valid value.
	}

	[Flags]
	public enum SceneFlags
	{
		DisableCollisions = 2,
		SeparateThread = 4,
		EnableMultiThread = 8,
		EnableActiveTransforms = 16,
		RestrictedScene = 32,
		DisableSceneMutex = 64,
		ForceConeFriction = 128,
		SequentialPrimary = 256,
		FluidPerformanceHint = 512,
	}

	[Flags]
	public enum ThreadPriorities
	{
		High = 0,
		AboveNormal = 1,
		Normal = 2,
		BelowNormal = 3,
		Low = 4
	}
	
	[Flags]
	public enum BroadPhaseTypes
	{
		SAPSingle = 0,
		SAPMulti = 1,
	}
	
	[Flags]
	public enum PruningStructures
	{
		None = 0,
		Octree = 1,
		Quadtree = 2,
		DynamicAABBTree = 3,
		StaticAABBTree = 4,
	}

	[Flags]
	public enum  TimeStepMethods
    {
        Fixed = 0,
        Variable = 1,
        Inherit = 2,
		NumMethods = 3,
    }

    public enum CombineMode : uint
    {
        AVERAGE = 0,		//!< Average: (a + b)/2
        MIN = 1,			//!< Minimum: min(a,b)
        MULTIPLY = 2,		//!< Multiply: a*b
        MAX = 3,			//!< Maximum: max(a,b)
        N_VALUES = 4,	//!< This is not a valid combine mode, it is a sentinel to denote the number of possible values. We assert that the variable's value is smaller than this.
        PAD_32 = 0xffffffff //!< This is not a valid combine mode, it is to assure that the size of the enum type is big enough.
    }

    public enum MaterialFlag
    {
        /**
        \brief Flag to enable anisotropic friction computation. 

        For a pair of actors, anisotropic friction is used only if at least one of the two actors' materials are anisotropic.
        The anisotropic friction parameters for the pair are taken from the material which is more anisotropic (i.e. the difference
        between its two dynamic friction coefficients is greater).

        The anisotropy direction of the chosen material is transformed to world space:

        dirOfAnisotropyWS = shape2world * dirOfAnisotropy

        Next, the directions of anisotropy in one or more contact planes (i.e. orthogonal to the contact normal) have to be determined. 
        The two directions are:

        uAxis = (dirOfAnisotropyWS ^ contactNormal).normalize()
        vAxis = contactNormal ^ uAxis

        This way [uAxis, contactNormal, vAxis] forms a basis.

        It may happen, however, that (dirOfAnisotropyWS | contactNormal).magnitude() == 1 
        and then (dirOfAnisotropyWS ^ contactNormal) has zero length. This happens when 
        the contactNormal is coincident to the direction of anisotropy. In this case we perform isotropic friction. 

        <b>Platform:</b>
        \li PC SW: Yes
        \li GPU  : Yes [SW]
        \li PS3  : Yes
        \li XB360: Yes
        \li WII	 : Yes

        @see NxMaterialDesc.dirOfAnisotropy
        */
        ANISOTROPIC = 1 << 0,

        /**
        If this flag is set, friction computations are always skipped between shapes with this material and any other shape.
        It may be a good idea to use this when all friction is to be performed using the tire friction model (see ::NxWheelShape).

        <b>Platform:</b>
        \li PC SW: Yes
        \li GPU  : Yes [SW]
        \li PS3  : Yes
        \li XB360: Yes
        \li WII	 : Yes

        @see NxWheelShape
        */
        DISABLE_FRICTION = 1 << 4,

        /**
        The difference between "normal" and "strong" friction is that the strong friction feature
        remembers the "friction error" between simulation steps. The friction is a force trying to
        hold objects in place (or slow them down) and this is handled in the solver. But since the
        solver is only an approximation, the result of the friction calculation can include a small
        "error" - e.g. a box resting on a slope should not move at all if the static friction is in
        action, but could slowly glide down the slope because of a small friction error in each 
        simulation step. The strong friction counter-acts this by remembering the small error and
        taking it to account during the next simulation step.

        However, in some cases the strong friction could cause problems, and this is why it is
        possible to disable the strong friction feature by setting this flag. One example is
        raycast vehicles, that are sliding fast across the surface, but still need a precise
        steering behavior. It may be a good idea to reenable the strong friction when objects
        are coming to a rest, to prevent them from slowly creeping down inclines.

        Note: This flag only has an effect if the DISABLE_FRICTION bit is 0.

        <b>Platform:</b>
        \li PC SW: Yes
        \li GPU  : Yes [SW]
        \li PS3  : Yes
        \li XB360: Yes
        \li WII	 : Yes

        @see NxWheelShape
        */
        DISABLE_STRONG_FRICTION = 1 << 5,

        //Note: Bits 16-31 are reserved for internal use!
    }

    [Flags]
	public enum MeshFlag
	{
		FLIPNORMALS		=	(1<<0),
        BIT_INDICES_16  = (1 << 1),	//<! Denotes the use of 16-bit vertex indices
		HARDWARE_MESH	=	(1<<2),	//<! The mesh will be used in hardware scenes
	}

    [Flags]
    public enum InternalArray
    {
        TRIANGLES,			//!< Array of triangles (index buffer). One triangle = 3 vertex references in returned format.
        VERTICES,			//!< Array of vertices (vertex buffer). One vertex = 3 coordinates in returned format.
        NORMALS,			//!< Array of vertex normals. One normal = 3 coordinates in returned format.
        HULL_VERTICES,		//!< Array of hull vertices. One vertex = 3 coordinates in returned format.
        HULL_POLYGONS,		//!< Array of hull polygons
        TRIANGLES_REMAP,	//!< Array of triangle remap indices. One triangle index = 1 original triangle index in returned format. (NxTriangleMesh only).
    }

    [Flags]
    public enum InternalFormat
    {
        NODATA,		//!< No data available
        FLOAT,		//!< Data is in floating-point format
        BYTE,			//!< Data is in byte format (8 bit)
        SHORT,		//!< Data is in short format (16 bit)
        INT			//!< Data is in int format (32 bit)
    }

    public enum FilterOperation
    {
        AND,
        OR,
        XOR,
        NAND,
        NOR,
        NXOR,
        //UBISOFT : FILTERING
        SWAP_AND
    }
}
