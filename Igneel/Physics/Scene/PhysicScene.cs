using Igneel.Assets;
using Igneel.Collections;
using Igneel.Components;
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
    public abstract class Physic : ResourceAllocator, INameable, IAssetProvider
    {
        private string name;
        private Scene scene;
        protected SceneFlags flags;
        protected SimulationType simulationType;

        private ReadOnlyList<PhysicMaterial> materials = new ReadOnlyList<PhysicMaterial>();
        private ReadOnlyDictionary<string, Actor> actors = new ReadOnlyDictionary<string,Actor>();
        private Vector3 gravity;        

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Scene Scene { get { return scene; } set { scene = value; } }

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
            a.Disposing+=a_Disposing;
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(a);
            return a;
        }

        void a_Disposing(object sender, EventArgs e)
        {
 	        Actor a = (Actor)sender;            
            RemoveActor(a);
        }

        public PhysicMaterial CreateMaterial(PhysicMaterialDesc desc)
        {
            PhysicMaterial mat = CreateMaterialImp(desc);
            mat.Disposing+=mat_Disposing;
            AddMaterial(mat);

            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(mat);
            return mat;
        }

        void mat_Disposing(object sender, EventArgs e)
        {
 	       PhysicMaterial m = (PhysicMaterial)sender;
            RemoveMaterial(m);
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

        public void AddToPackage(ContentPackage pk)
        {
            throw new NotImplementedException();
        }

        public Asset CreateAsset()
        {
            throw new NotImplementedException();
        }

        protected override void OnDispose(bool disposing)
        {
            if(disposing)
                PhysicManager.Sigleton.scenes.Remove(name);

            foreach (var actor in actors.Values.ToArray())
            {
                actor.Dispose();
            }

            foreach (var mat in materials.ToArray())
            {
                mat.Dispose();
            }
            base.OnDispose(disposing);
        }

        public override string ToString()
        {
            return name ?? base.ToString();
        }
    }
}
