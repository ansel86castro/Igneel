using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Igneel.Collections;

namespace Igneel.Physics
{
    public abstract class Actor : Resource, INameable,  IPositionable, ITranslatable, IRotable, IAffector
    {      
        protected bool isDynamic;
        protected IAffectable affectable;
        protected ActorFlag flags;
        protected BodyActorFlags bodyFlags;
        protected Physic scene;
        protected ReadOnlyDictionary<string, ActorShape> shapes = new ReadOnlyDictionary<string, ActorShape>();
        protected bool visible;     

        public Actor()
        {

        }      

        public Physic Scene { get { return scene; } protected set { scene = value; } }

        public ReadOnlyDictionary<string, ActorShape> Shapes { get { return shapes; } }

        public ActorFlag Flags { get { return flags; } set { flags = value; } }

        public BodyActorFlags BodyFlags { get { return bodyFlags; } set { bodyFlags = value; } }

        public bool IsDynamic
        {
            get { return isDynamic; }
            protected set { isDynamic = value; }
        }
     
        Vector3 ITranslatable.LocalPosition
        {
            get
            {
                return GlobalPosition;
            }
            set
            {
                GlobalPosition = value;
            }
        }

        Matrix IRotable.LocalRotation
        {
            get
            {
                return GlobalOrientation;
            }
            set
            {
                GlobalOrientation = value;
            }
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
        
        public bool IsTransparent
        {
            get { return false; }
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

        public object UserData { get; set; }

        #region Abstract 

        public abstract float Mass { get; set; }

        public abstract Vector3 LinearVelocity { get; set; }
       
        public abstract float LinearDamping { get; set; }

        public abstract Vector3 AngularVelocity { get; set; }

        public abstract float AngularDamping { get; set; }

        public abstract float MaxAngularVelocity { get; set; }
       
        public abstract Vector3 GlobalPosition { get; set; }

        public abstract Matrix GlobalOrientation { get; set; }

        public abstract Quaternion GlobalOrientationQuat { get; set; }

        public abstract Matrix GlobalPose { get; }

        public abstract Matrix CMassGlobalPose { get; set; }

        public abstract Vector3 CMassGlobalPosition { get; set; }

        public abstract Matrix CMassGlobalOrientation { get; set; }

        public abstract Matrix CMassLocalPose { get;  }

        public abstract Vector3 MassSpaceInertiaTensor { get; set; }

        public abstract bool IsKinematic { get; set; }

        public abstract bool IsSleeping { get; }

        public abstract float SleepEnergyThreshold { get; set; }

        public abstract float SleepAngularVelocity { get; set; }

        public abstract float SleepLinearVelocity { get; set; }

        public abstract bool IsGroupSleeping { get; }

        public abstract int SolverIterationCount { get; set; }

        public abstract ushort Group { get; set; }

        public abstract ushort DominanceGroup { get; set; }        

        public abstract float CCDMotionThreshold { get; set; }

        public abstract Vector3 LinearMomentum { get; set; }

        public abstract Vector3 AngularMomentum { get; set; }

        public abstract float ContactReportThreshold { get; set; }

        public abstract ContactPairFlag ContactReportFlags { get; set; }

        public abstract void MoveGlobalPose(Matrix pose);

        public abstract void MoveGlobalOrientation(Matrix rotationMat);

        public abstract void MoveGlobalPosition(Vector3 position);

        public abstract void MoveGlobalOrientationQuat(Quaternion v);        

        public abstract void RaiseActorFlag(ActorFlag flag);

        public abstract bool ReadActorFlag(ActorFlag flag);

        public abstract void ClearActorFlag(ActorFlag flag);

        public abstract void RaiseBodyFlag(BodyActorFlags flag);

        public abstract void ClearBodyFlag(BodyActorFlags flag);

        public abstract bool ReadBodyFlag(BodyActorFlags flag);

        public abstract void AddForceAtPos(Vector3 force, Vector3 pos, ForceMode mode = ForceMode.FORCE, bool wakeup = true);

        public abstract void AddForceAtLocalPos(Vector3 force, Vector3 pos, ForceMode mode = ForceMode.FORCE, bool wakeup = true);

        public abstract void AddLocalForceAtPos(Vector3 force, Vector3 pos, ForceMode mode = ForceMode.FORCE, bool wakeup = true);

        public abstract void AddLocalForceAtLocalPos(Vector3 force, Vector3 pos, ForceMode mode = ForceMode.FORCE, bool wakeup = true);

        public abstract void AddForce(Vector3 force, Vector3 pos, ForceMode mode = ForceMode.FORCE, bool wakeup = true);

        public abstract void AddLocalForce(Vector3 force, Vector3 pos, ForceMode mode = ForceMode.FORCE, bool wakeup = true);

        public abstract void AddTorque(Vector3 torque, Vector3 pos, ForceMode mode = ForceMode.FORCE, bool wakeup = true);

        public abstract void AddLocalTorque(Vector3 torque, Vector3 pos, ForceMode mode = ForceMode.FORCE, bool wakeup = true);

        public abstract float ComputeKineticEnergy();

        public abstract Vector3 GetPointVelocity(Vector3 point);

        public abstract Vector3 GetLocalPointVelocity(Vector3 point);

        public abstract void WakeUp(float wakeCounterValue = 20.0f*0.02f);// Corresponds to 20 frames for the standard time step.

        public abstract void PutToSleep();

        public abstract void ResetUserActorPairFiltering();

        public abstract void SetCMassOffsetLocalOrientation(Matrix mat);

        public abstract void SetCMassOffsetLocalPosition(Vector3 v);

        public abstract void SetCMassOffsetLocalPose(Matrix mat);

        public abstract void SetCMassOffsetGlobalPose(Matrix mat);

        public abstract void SetCMassOffsetGlobalPosition(Vector3 v);

        public abstract void SetCMassOffsetGlobalOrientation(Matrix mat);

        public abstract Matrix GetGlobalInertiaTensor();

        public abstract Matrix GetGlobalInertiaTensorInverse();

        public abstract void UpdateMassFromShapes(float density, float totalMass);

        protected abstract ActorShape CreateShapeImp(ActorShapeDesc desc);

        #endregion

        public abstract SweepQueryHit LinearSweep(Vector3 motion, SweepFlags flags, Predicate<SweepQueryHit> callback = null, SweepCache cache = null);

        public abstract int LinearSweep(Vector3 motion, SweepFlags flags, ICollection<SweepQueryHit> collection, SweepCache cache = null);

        public ActorShape CreateShape(ActorShapeDesc desc)
        {
            var shape = CreateShapeImp(desc);           
            shapes.Add(shape.Name, shape);          
            return shape;
        }

        void shape_Disposing(object sender, EventArgs e)
        {
            var shape = ((ActorShape)sender);
            shapes.Remove(shape.Name);          
        }            

        void IDeferreable.CommitChanges()
        {
            if (affectable != null)
                affectable.UpdatePose(GlobalPose);
        }

     

        //public Render GetRender()
        //{
        //    return Engine.GetRender<Actor>();
        //}

        //public void Draw(SceneNode node, Render render, PixelClipping clipping = PixelClipping.None)
        //{          
        //    var actorRender = (Render<Actor>)render;
        //    if (actorRender != null)
        //    {
        //        actorRender.Draw(this);
        //    }
        //}

        protected void AddShape(ActorShape shape)
        {
            shapes.Add(shape.Name, shape);
        }

        protected override void OnDispose(bool disposing)
        {            
            foreach (var s in shapes)
            {
                s.Dispose();
            }              
                     
            scene.RemoveActor(this);
        }
           
    }
}
