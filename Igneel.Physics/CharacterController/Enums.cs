using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
    public enum CCTInteraction
    {
        INTERACTION_INCLUDE,		//!< Always collide character controllers.
        INTERACTION_EXCLUDE,		//!< Never collide character controllers.

        /**
        \brief Collide based on the shape group.

        The groups to collide against are passed in the activeGroups member of #NxController::move(). The active
        groups flags are combined with the shape flags for the controllers kinematic actor to determine if a 
        collision should occur:

            activeGroups & ( 1 << shape->getGroup())

        So to set the shape flags the NxController::getActor() method can be called, then the getShapes() method to
        retrieve the shape. Then NxShape::setGroup() method is used to set the shape group.

        @see NxController.move() NxController.getActor()
        */
        INTERACTION_USE_FILTER
    }

    public enum CollitionResult
    {
        /// <summary>
        /// Character is not colliding
        /// </summary>
        NONE = 0,
        /// <summary>
        /// Character is colliding to the sides
        /// </summary>
        COLLISION_SIDES = (1 << 0),	//!< Character is colliding to the sides.
        /// <summary>
        /// Character has collision above
        /// </summary>
        COLLISION_UP = (1 << 1),	//!< Character has collision above.
        /// <summary>
        /// Character has collision below.
        /// </summary>
        COLLISION_DOWN = (1 << 2),	//!< Character has collision below.
    }

    public enum CollisionGroup
    {
        GROUP_NON_COLLIDABLE,
        GROUP_COLLIDABLE_NON_PUSHABLE,
        GROUP_COLLIDABLE_PUSHABLE,
    }

    public enum CapsuleClimbingMode
    {
        CLIMB_EASY,			//!< Standard mode, let the capsule climb over surfaces according to impact normal
        CLIMB_CONSTRAINED,	//!< Constrained mode, try to limit climbing according to the step offset

        CLIMB_LAST
    }

    public enum HeightFieldAxis
    {
        X = 0, //!< X Axis
        Y = 1, //!< Y Axis
        Z = 2, //!< Z Axis
        NOT_HEIGHTFIELD = 0xff //!< Not a heightfield
    }

    public enum ControllerAction
    {
        ACTION_NONE,				//!< Don't apply forces to touched actor
        ACTION_PUSH,				//!< Automatically compute & apply forces to touched actor (push)
    }

    public enum ControllerType : int
    {
        /**
        \brief A box controller.

        @see NxBoxController NxBoxControllerDesc
        */
        BOX,

        /**
        \brief A capsule controller

        @see NxCapsuleController NxCapsuleControllerDesc
        */
        CAPSULE
    }
}
