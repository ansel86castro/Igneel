using Igneel.Assets;
using Igneel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
    public abstract class Physic : Resource
    {           
        protected SceneFlags flags;
        protected SimulationType simulationType;

        private ReadOnlyList<PhysicMaterial> materials = new ReadOnlyList<PhysicMaterial>();
        private ReadOnlyDictionary<string, Actor> actors = new ReadOnlyDictionary<string,Actor>();
        private Vector3 gravity;


        public Physic()
        {
           
        }     

        public bool Enable { get; set; }

        public object UserData { get; set; }

        public SceneFlags Flags { get { return flags; } protected set { flags = value; } }

        public Vector3 Gravity { get { return gravity; } set { gravity = value; SetGravity(value); } }

        public SimulationType Simulation { get { return simulationType; } protected set { simulationType = value; } }

        public ReadOnlyList<PhysicMaterial> Materials { get { return materials; } }

        public ReadOnlyDictionary<string, Actor> Actors { get { return actors; } }

        public bool Visible { get; set; }

        protected internal void AddActor(Actor actor)
        {
            actors.itemLookup.Add(actor.Name, actor);
            actors.items.Add(actor);
        }

        protected internal void AddMaterial(PhysicMaterial mat)
        {
            materials.items.Add(mat);
        }

        protected internal void RemoveActor(Actor actor)
        {
            if(actors.itemLookup.Remove(actor.Name))
                actors.items.Remove(actor);
        }

        protected internal void RemoveMaterial(PhysicMaterial mat)
        {
            materials.items.Remove(mat);
        }

        public Actor CreateActor(ActorDesc desc)
        {
            Actor a = CreateActorImp(desc);          
            AddActor(a);
        
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(a);
            return a;
        }        

        public PhysicMaterial CreateMaterial(PhysicMaterialDesc desc)
        {
            PhysicMaterial mat = CreateMaterialImp(desc);          
            AddMaterial(mat);

            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(mat);
            return mat;
        }
        
      

        public abstract void Simulate(float elapsedTime);
        
        protected abstract void SetGravity(Vector3 value);

        #region Creation 

        protected abstract Actor CreateActorImp(ActorDesc desc);

        protected abstract PhysicMaterial CreateMaterialImp(PhysicMaterialDesc desc);

        public abstract SweepCache CreateSweepCache();

        #endregion

        #region Collision Filtering and Grouping

        public abstract void SetGroupCollisionFlag(ushort g1 , ushort g2 ,bool enable );

		public abstract void SetActorGroupPairFlags(ushort g1 , ushort g2 , ContactPairFlag flags);

		public abstract void SetActorPairFlags(Actor actor1, Actor actor2 ,  ContactPairFlag flags); 

		public abstract void SetShapePairFlags(ActorShape shape1, ActorShape shape2, ContactPairFlag flags);

		public abstract void SetFilterOperations(FilterOperation op0, FilterOperation op1 , FilterOperation op2);

		public abstract void SetFilterBool(bool value);

		public abstract void SetFilterConstant0(GroupsMask mask);

		public abstract void SetFilterConstant1(GroupsMask mask);

        #endregion

        #region RayCasting

        public abstract void RaycastAllShapes(Ray ray, IRayCastReport report , ShapesType shapeType ,int groups, float maxDistance, RayCastBit hintflags ,GroupsMask mask );

        public abstract bool RaycastAnyShapes(Ray ray, ShapesType shapeType ,int groups, float maxDistance, GroupsMask mask );

        public abstract RaycastHit RaycastClosestShape(Ray ray, ShapesType shapeType, int groups, float maxDistance, RayCastBit hintflags, GroupsMask mask);

        #endregion

        #region Callbacks

        public abstract void SetUserContactReport(IUserContactReport report);

        public abstract void SetTriggerCallback(ITriggerReport report);

        #endregion

        //public void AddToPackage(ContentPackage pk)
        //{
        //    throw new NotImplementedException();
        //}

       
        protected override void OnDispose(bool disposing)
        {
            if(disposing)
                PhysicManager.Sigleton.scenes.Remove(Name);

            foreach (var actor in actors.Values.ToArray())
            {
                actor.Dispose();
            }

            foreach (var mat in materials.ToArray())
            {
                mat.Dispose();
            }            
        }
       
    }
}
