using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
   public enum ContactPairFlag
	{
		IGNORE_PAIR								= (1<<0),	//!< Disable contact generation for this pair
		NOTIFY_ON_START_TOUCH					= (1<<1),	//!< Pair callback will be called when the pair starts to be in contact
		NOTIFY_ON_END_TOUCH						= (1<<2),	//!< Pair callback will be called when the pair stops to be in contact
		NOTIFY_ON_TOUCH							= (1<<3),	//!< Pair callback will keep getting called while the pair is in contact
		NOTIFY_ON_IMPACT						= (1<<4),	//!< [Not yet implemented] pair callback will be called when it may be appropriate for the pair to play an impact sound
		NOTIFY_ON_ROLL							= (1<<5),	//!< [Not yet implemented] pair callback will be called when the pair is in contact and rolling.
		NOTIFY_ON_SLIDE							= (1<<6),	//!< [Not yet implemented] pair callback will be called when the pair is in contact and sliding (and not rolling).
		NOTIFY_FORCES							= (1<<7),	//!< The (summed total) friction force and normal force will be given in the NxContactPair variable in the contact report.
		NOTIFY_ON_START_TOUCH_FORCE_THRESHOLD	= (1<<8),	//!< Pair callback will be called when the contact force between two actors exceeds one of the actor-defined force thresholds
		NOTIFY_ON_END_TOUCH_FORCE_THRESHOLD		= (1<<9),	//!< Pair callback will be called when the contact force between two actors falls below the actor-defined force thresholds
		NOTIFY_ON_TOUCH_FORCE_THRESHOLD			= (1<<10),	//!< Pair callback will keep getting called while the contact force between two actors exceeds one of the actor-defined force thresholds
		NOTIFY_CONTACT_MODIFICATION				= (1<<16),	//!< Generate a callback for all associated contact constraints, making it possible to edit the constraint. This flag is not included in NOTIFY_ALL for performance reasons. \see NxUserContactModify

		NOTIFY_ALL								= (NOTIFY_ON_START_TOUCH|NOTIFY_ON_END_TOUCH|NOTIFY_ON_TOUCH|NOTIFY_ON_IMPACT|NOTIFY_ON_ROLL|NOTIFY_ON_SLIDE|NOTIFY_FORCES|
													   NOTIFY_ON_START_TOUCH_FORCE_THRESHOLD|NOTIFY_ON_END_TOUCH_FORCE_THRESHOLD|NOTIFY_ON_TOUCH_FORCE_THRESHOLD)
	}

    public enum  ActorFlag
	{
		/**
		\brief Enable/disable collision detection

		Turn off collision detection, i.e. the actor will not collide with other objects. Please note that you might need to wake 
		the actor up if it is sleeping, this depends on the result you wish to get when using this flag. (If a body is asleep it 
		will not start to fall through objects unless you activate it).

		Note: Also excludes the actor from overlap tests!
		*/
		DISABLE_COLLISION			= (1<<0),

		/**
		\brief Enable/disable collision response (reports contacts but don't use them)

		@see NxUserContactReport
		*/
		DISABLE_RESPONSE			= (1<<1), 

		/**
		\brief Disables COM update when computing inertial properties at creation time.

		When sdk computes inertial properties, by default the center of mass will be calculated too.  However, if lockCOM is set to a non-zero (true) value then the center of mass will not be altered.
		*/
		LOCK_COM					= (1<<2),

		/**
		\brief Enable/disable collision with fluid.
		*/
		FLUID_DISABLE_COLLISION	= (1<<3),

		/**
		\brief Turn on contact modification callback for the actor.

		@see NxScene.setUserContactModify(), NOTIFY_CONTACT_MODIFICATION
		*/
		CONTACT_MODIFICATION		= (1<<4),


		/**
		\brief Force cone friction to be used for this actor.	

		This ensures that all contacts involving the actor will use cone friction, rather than the default
		simplified scheme. This will however have a negative impact on performance in software scenes. Use this
		flag if sliding objects show an affinity for moving along the world axes.

		\note Only applies to software scenes. Hardware scenes always force cone friction.

		Cone friction may also be applied wholesale to a PhysicScene using the SF_FORCE_CONE_FRICTION 
		flag, see #NxSceneFlags.
		*/
		FORCE_CONE_FRICTION		= (1<<5),

		/**
		\brief Enable/disable custom contact filtering. 

		When enabled the user will be queried for contact filtering for all contacts involving this actor.
		*/
		USER_ACTOR_PAIR_FILTERING	= (1<<6)
	}

    [Flags]
	public enum BodyActorFlags{
		/**
		\brief Set if gravity should not be applied on this body

		@see NxBodyDesc.flags NxScene.setGravity()
		*/
		DISABLE_GRAVITY	= (1<<0),
	
		/**	
		\brief Enable/disable freezing for this body/actor. 

		\note This is an EXPERIMENTAL feature which doesn't always work on in all situations, e.g. 
		for actors which have joints connected to them.
	
		To freeze an actor is a way to simulate that it is static. The actor is however still simulated
		as if it was dynamic, it's position is just restored after the simulation has finished. A much
		more stable way to make an actor temporarily static is to raise the KINEMATIC flag.
		*/
		FROZEN_POS_X		= (1<<1),
		FROZEN_POS_Y		= (1<<2),
		FROZEN_POS_Z		= (1<<3),
		FROZEN_ROT_X		= (1<<4),
		FROZEN_ROT_Y		= (1<<5),
		FROZEN_ROT_Z		= (1<<6),
		FROZEN_POS		= FROZEN_POS_X|FROZEN_POS_Y|FROZEN_POS_Z,
		FROZEN_ROT		= FROZEN_ROT_X|FROZEN_ROT_Y|FROZEN_ROT_Z,
		FROZEN			= FROZEN_POS|FROZEN_ROT,


		/**
		\brief Enables kinematic mode for the actor.
	
		Kinematic actors are special dynamic actors that are not 
		influenced by forces (such as gravity), and have no momentum. They are considered to have infinite
		mass and can be moved around the world using the moveGlobal*() methods. They will push 
		regular dynamic actors out of the way. Kinematics will not collide with static or other kinematic objects.
	
		Kinematic actors are great for moving platforms or characters, where direct motion control is desired.

		You can not connect Reduced joints to kinematic actors. Lagrange joints work ok if the platform
		is moving with a relatively low, uniform velocity.

		@see NxActor NxActor.raiseActorFlag()
		*/
		KINEMATIC			= (1<<7),		//!< Enable kinematic mode for the body.

		/**
		\brief Enable debug renderer for this body

		@see NxScene.getDebugRenderable() NxDebugRenderable NxParameter
		*/
		VISUALIZATION		= (1<<8),

		DUMMY_0			= (1<<9), // deprecated flag placeholder

		/**
		\brief Filter velocities used keep body awake. The filter reduces rapid oscillations and transient spikes.
		@see NxActor.isSleeping()
		*/
		FILTER_SLEEP_VEL	= (1<<10),

		/**
		\brief Enables energy-based sleeping algorithm.
		@see NxActor.isSleeping() NxBodyDesc.sleepEnergyThreshold 
		*/
		ENERGY_SLEEP_TEST	= (1<<11),
	}

    	public enum ForceMode
		{
			FORCE,                   //!< parameter has unit of mass * distance/ time^2, i.e. a force
			IMPULSE,                 //!< parameter has unit of mass * distance /time
			VELOCITY_CHANGE,			//!< parameter has unit of distance / time, i.e. the effect is mass independent: a velocity change.
			SMOOTH_IMPULSE,          //!< same as IMPULSE but the effect is applied over all substeps. Use this for motion controllers that repeatedly apply an impulse.
			SMOOTH_VELOCITY_CHANGE,	//!< same as VELOCITY_CHANGE but the effect is applied over all substeps. Use this for motion controllers that repeatedly apply an impulse.
			ACCELERATION				//!< parameter has unit of distance/ time^2, i.e. an acceleration. It gets treated just like a force except the mass is not divided out before integration.
		}

        public enum SweepFlags
        {
            STATICS = (1 << 0),	//!< Sweep vs static objects
            DYNAMICS = (1 << 1),	//!< Sweep vs dynamic objects
            ASYNC = (1 << 2),	//!< Asynchronous sweeps (else synchronous) (Note: Currently disabled)
            ALL_HITS = (1 << 3),	//!< Reports all hits rather than just closest hit

            DEBUG_SM = (1 << 5),	//!< DEBUG - temp - don't use
            DEBUG_ET = (1 << 6),	//!< DEBUG - temp - don't use
        }

            [Flags]
			public enum ShapeFlag
			{
						/**
				\brief Trigger callback will be called when a shape enters the trigger volume.

				@see NxUserTriggerReport NxScene.setUserTriggerReport()
				*/
				TRIGGER_ON_ENTER				= (1<<0),
	
				/**
				\brief Trigger callback will be called after a shape leaves the trigger volume.

				@see NxUserTriggerReport NxScene.setUserTriggerReport()
				*/
				TRIGGER_ON_LEAVE				= (1<<1),
	
				/**
				\brief Trigger callback will be called while a shape is intersecting the trigger volume.

				@see NxUserTriggerReport NxScene.setUserTriggerReport()
				*/
				TRIGGER_ON_STAY				= (1<<2),

				/**
				\brief Combination of all the other trigger enable flags.

				@see NxUserTriggerReport NxScene.setUserTriggerReport()
				*/
				TRIGGER_ENABLE				= TRIGGER_ON_ENTER|TRIGGER_ON_LEAVE|TRIGGER_ON_STAY,

				/**
				\brief Enable debug renderer for this shape

				@see NxScene.getDebugRenderable() NxDebugRenderable NxParameter
				*/
				VISUALIZATION				= (1<<3),

				/**
				\brief Disable collision detection for this shape (counterpart of AF_DISABLE_COLLISION)

				\warning IMPORTANT: this is only used for compound objects! Use AF_DISABLE_COLLISION otherwise.
				*/
				DISABLE_COLLISION			= (1<<4),

				/**
				\brief Enable feature indices in contact stream.

				@see NxUserContactReport NxContactStreamIterator NxContactStreamIterator.getFeatureIndex0()
				*/
				FEATURE_INDICES			= (1<<5),

				/**
				\brief Disable raycasting for this shape
				*/
				DISABLE_RAYCASTING		= (1<<6),

				/**
				\brief Enable contact force reporting per contact point in contact stream (otherwise we only report force per actor pair)
				*/
				POINT_CONTACT_FORCE		= (1<<7),

				FLUID_DRAIN				= (1<<8),	//!< Sets the shape to be a fluid drain.
				FLUID_DISABLE_COLLISION	= (1<<10),	//!< Disable collision with fluids.
				FLUID_TWOWAY				= (1<<11),	//!< Enables the reaction of the shapes actor on fluid collision.

				/**
				\brief Disable collision response for this shape (counterpart of AF_DISABLE_RESPONSE)

				\warning not supported by cloth / soft bodies
				*/
				DISABLE_RESPONSE			= (1<<12),

				/**
				\brief Enable dynamic-dynamic CCD for this shape. Used only when CCD is globally enabled and shape have a CCD skeleton.
				*/
				DYNAMIC_DYNAMIC_CCD		= (1<<13),

				/**
				\brief Disable participation in ray casts, overlap tests and sweeps.

				NOTE: Setting this flag for static non-trigger shapes may cause incorrect behavior for 
				colliding fluid and cloth.
				*/
				DISABLE_SCENE_QUERIES			= (1<<14),

				CLOTH_DRAIN					= (1<<15),	//!< Sets the shape to be a cloth drain.
				CLOTH_DISABLE_COLLISION		= (1<<16),	//!< Disable collision with cloths.

				/**
				\brief  Enables the reaction of the shapes actor on cloth collision.
				\warning Compound objects cannot use a different value for each constituent shape.
				*/
				CLOTH_TWOWAY					= (1<<17),	

				SOFTBODY_DRAIN				= (1<<18),	//!< Sets the shape to be a soft body drain.
				SOFTBODY_DISABLE_COLLISION	= (1<<19),	//!< Disable collision with soft bodies.

				/**
				\brief  Enables the reaction of the shapes actor on soft body collision.
				\warning Compound objects cannot use a different value for each constituent shape.
				*/
				SOFTBODY_TWOWAY				= (1<<20),
			}

		[Flags]
		public enum CapsuleShapeFlag
		{
			/**
			\brief If this flag is set, the capsule shape represents a moving sphere, 
			moving along the ray defined by the capsule's positive Y axis.

			Currently this behavior is only implemented for points (zero radius spheres).

			Note:Used only in conjunction with the old-style capsule based wheel shape (see
			the Guide for more information).
			*/
			SWEPT_SHAPE	= (1<<0)
		}

        [Flags]
		public enum WheelShapeFlags
		{
			/**
			\brief Determines whether the suspension axis or the ground contact normal is used for the suspension constraint.

			*/
			WHEEL_AXIS_CONTACT_NORMAL = 1 << 0,
	
			/**
			\brief If set, the laterial slip velocity is used as the input to the tire function, rather than the slip angle.

			*/
			INPUT_LAT_SLIPVELOCITY = 1 << 1,
	
			/**
			\brief If set, the longutudal slip velocity is used as the input to the tire function, rather than the slip ratio.  
			*/
			INPUT_LNG_SLIPVELOCITY = 1 << 2,

			/**
			\brief If set, does not factor out the suspension travel and wheel radius from the spring force computation.  This is the legacy behavior from the raycast capsule approach.
			*/
			UNSCALED_SPRING_BEHAVIOR = 1 << 3, 

			/**
			\brief If set, the axle speed is not computed by the simulation but is rather expected to be provided by the user every simulation step via NxWheelShape::setAxleSpeed().
			*/
			AXLE_SPEED_OVERRIDE = 1 << 4,

			/**
			\brief If set, the NxWheelShape will emulate the legacy raycast capsule based wheel.
			See #NxTireFunctionDesc
			*/
			EMULATE_LEGACY_WHEEL = 1 << 5,

			/**
			\brief If set, the NxWheelShape will clamp the force in the friction constraints.
			See #NxTireFunctionDesc
			*/
			CLAMPED_FRICTION = 1 << 6,
		}

        [Flags]
		public enum MeshShapeFlag
		{
			/**
			\brief Select between "normal" or "smooth" sphere-mesh/raycastcapsule-mesh contact generation routines.

			The 'normal' algorithm assumes that the mesh is composed from flat triangles. 
			When a ball rolls or a raycast capsule slides along the mesh surface, it will experience small,
			sudden changes in velocity as it rolls from one triangle to the next. The smooth algorithm, on
			the other hand, assumes that the triangles are just an approximation of a surface that is smooth.  
			It uses the Gouraud algorithm to smooth the triangles' vertex normals (which in this 
			case are particularly important). This way the rolling sphere's/capsule's velocity will change
			smoothly over time, instead of suddenly. We recommend this algorithm for simulating car wheels
			on a terrain.

			@see NxSphereShape NxTriangleMeshShape NxHeightFieldShape
			*/
			SMOOTH_SPHERE_COLLISIONS	= (1<<0),
			DOUBLE_SIDED				= (1<<1)	//!< The mesh is double-sided. This is currently only used for raycasting.
		}
    public enum ShapeType
	{
	/**
	\brief A physical plane
    @see NxPlaneShape
	*/
	PLANE,

	/**
	\brief A physical sphere
	@see NxSphereShape
	*/
	SPHERE,

	/**
	\brief A physical box (OBB)
	@see NxBoxShape
	*/
	BOX,

	/**
	\brief A physical capsule (LSS)
	@see NxCapsuleShape
	*/
	CAPSULE,

	/**
	\brief A wheel for raycast cars
	@see NxWheelShape
	*/
	WHEEL,

	/**
	\brief A physical convex mesh
	@see NxConvexShape NxConvexMesh
	*/
	CONVEX,

	/**
	\brief A physical mesh
	@see NxTriangleMeshShape NxTriangleMesh
	*/
	MESH,

	/**
	\brief A physical height-field
	@see NxHeightFieldShape NxHeightField
	*/
	HEIGHTFIELD,

	/**
	\brief internal use only!

	*/
	RAW_MESH,

	COMPOUND,

	COUNT,

	FORCE_DWORD = 0x7fffffff
	}

    public enum ShapesType
    {
        STATIC_SHAPES = (1 << 0),								//!< Hits static shapes
        DYNAMIC_SHAPES = (1 << 1),								//!< Hits dynamic shapes
        ALL_SHAPES = STATIC_SHAPES | DYNAMIC_SHAPES	//!< Hits both static & dynamic shapes
    };
}
