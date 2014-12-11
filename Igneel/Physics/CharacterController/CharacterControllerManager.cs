using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Collections;
using Igneel.Services;

namespace Igneel.Physics
{
    public abstract class CharacterControllerManager:ResourceAllocator
    {
        public static CharacterControllerManager Instance
        {
            get { return instance; }
        }

        public CharacterControllerManager()
        {
            if (instance != null)
                throw new InvalidOperationException("Only one instance of this type is supported");

            instance = this; 
        }

        ReadOnlyList<CharacterController>controllers = new ReadOnlyList<CharacterController>();
        private static CharacterControllerManager instance;

        public ReadOnlyList<CharacterController> Controllers { get { return controllers; } }

        public CharacterController CreateController(Physic scene, CharacterControllerDesc desc)
        {
            var controller = _CreateController(scene, desc);          
            controllers.items.Add(controller);

            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(controller);

            return controller;
        }

        public abstract void UpdateControllers();

        protected internal void Remove(CharacterController c)
        {
            controllers.items.Remove(c);
        }       

        protected override void OnDispose(bool disposing)
        {
            foreach (var item in controllers)
            {
                item.Dispose();
            }
            base.OnDispose(disposing);
        }

        protected abstract CharacterController _CreateController(Physic scene, CharacterControllerDesc desc);
    }
}
