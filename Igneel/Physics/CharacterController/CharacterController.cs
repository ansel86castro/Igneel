using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{

    public struct ControllerMoveInfo
    {

    }
    public abstract class CharacterControllerBase : ResourceAllocator, CharacterController
    {
        public static readonly uint COLLIDABLE_MASK = (1 << (int)CollisionGroup.GROUP_COLLIDABLE_NON_PUSHABLE);

        protected CharacterControllerManager manager;
        protected IAffectable affectable;
        protected IAffector affector;
        Matrix pose = Matrix.Identity;
        Matrix bindAffectorPose;

        public CharacterControllerManager Manager { get { return manager; } }

        public object UserData { get; set; }

        public abstract Vector3 Position {get;set;}

		public abstract CCTInteraction Interaction { get; set;}

        public abstract float StepOffset { get; set; }        

        public abstract void ReportSceneChanged();

        public abstract CollitionResult Move(Vector3 disp, uint activeGroups , float minDist, float sharpness, GroupsMask groupsMask);

        public abstract CollitionResult Move(Vector3 disp, uint activeGroups = (1 << (int)CollisionGroup.GROUP_COLLIDABLE_NON_PUSHABLE), float minDist = 0.000001f, float sharpness = 1.0f);

        public void Remove()
        {
            manager.Remove(this);
            Dispose();
        }      

        public IAffectable Affectable
        {
            get
            {
                return affectable;
            }
            set
            {
                affectable = value;
            }
        }

        public Matrix GlobalPose
        {
            get
            {
                var position = Position;
                pose.M41 = position.X;
                pose.M42 = position.Y;
                pose.M43 = position.Z;
                pose.M44 = 1;
                return pose;
            }
        }

        public Matrix BindAffectorPose
        {
            get
            {
                return bindAffectorPose;
            }
            set
            {
                bindAffectorPose = value;
            }
        }

        public IAffector Affector
        {
            get
            {
                return affector;
            }
            set
            {
                affector = value;
            }
        }

        public void UpdatePose(Matrix affectorPose)
        {
            Move(affectorPose.Translation - Position);
        }

        public bool IsTransparent
        {
            get { return false; }
        }

        public bool Visible { get; set; }

        public Rendering.RenderBinder RenderParam { get; set; }

        public Rendering.Render GetRender()
        {
            var render = Service.Require<ActorRender>();
            return render;
        }

        public void Draw(Components.SceneNode node, Render render, Rendering.PixelClipping clipping = PixelClipping.None)
        {
            var actorRender = (ActorRender)render;
            actorRender.DrawController(this);
        }
        
        public void Draw()
        {
            var render = Service.Require<ActorRender>();
            render.DrawController(this);
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }

    public interface BoxController : CharacterController
    {
        Vector3 Extents { get; set; }
    }

    public interface CapsuleController : CharacterController
    {
        float Radius { get; set; }

        float Height { get; set; }

        CapsuleClimbingMode ClimbingMode { get; set; }
    }
}
