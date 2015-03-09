using Igneel.Assets;
using Igneel.Components;

using Igneel.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Services
{
    public interface INotificationService
    {
        
        event EventHandler ProjectCreated;
        event EventHandler ProjectLoaded;
        event EventHandler GraphicSceneCreated;
        event EventHandler GraphicSceneDestroyed;
        event EventHandler PhysicSceneCreated;
        event EventHandler PhysicSceneDestroyed;

        void OnProjectCreated();

        void OnProjectLoaded();

        void OnGraphicSceneCreated(Scene scene);

        void OnGraphicSceneDestroyed(Scene scene);

        void OnPhysicSceneCreated(Physic scene);

        void OnPhysicSceneDestroyed(Physic scene);

        void OnObjectCreated<T>(T value);

        void OnObjectDestroyed<T>(T value);

        void AddCreateEventListener<T>(Action<T> listerner);

        void RemoveCreateEventListener<T>(Action<T> listerner);

        void AddDestroyEventListener<T>(Action<T> listerner);

        void RemoveDestroyEventListener<T>(Action<T> listerner);       
    }

    public class NotificationService : INotificationService
    {
        class CallBackStore<T>
        {
           public static Action<T> createListerner;
           public static Action<T> destroyListerner;
        }

        public event EventHandler ProjectCreated;
        public event EventHandler ProjectLoaded;
        public event EventHandler GraphicSceneCreated;
        public event EventHandler GraphicSceneDestroyed;
        public event EventHandler PhysicSceneCreated;
        public event EventHandler PhysicSceneDestroyed;

        public virtual void OnProjectCreated()
        {
            if (ProjectCreated != null)
                ProjectCreated(this, EventArgs.Empty);
        }

        public virtual void OnProjectLoaded()
        {
            if (ProjectLoaded != null)
                ProjectLoaded(this, EventArgs.Empty);
        }

        public virtual void OnGraphicSceneCreated(Scene scene)
        {           
            if (GraphicSceneCreated != null)
                GraphicSceneCreated(this, EventArgs.Empty);
        }

        public virtual void OnGraphicSceneDestroyed(Scene scene)
        {
            if (GraphicSceneDestroyed != null)
                GraphicSceneDestroyed(this, EventArgs.Empty);
        }

        public virtual void OnPhysicSceneCreated(Physic scene)
        {
            if (PhysicSceneCreated != null)
                PhysicSceneCreated(this, EventArgs.Empty);
        }

        public virtual void OnPhysicSceneDestroyed(Physic scene)
        {
            if (PhysicSceneDestroyed != null)
                PhysicSceneDestroyed(this, EventArgs.Empty);
        }        

        public virtual void OnObjectCreated<T>(T value)
        {
            if (CallBackStore<T>.createListerner != null)
            {
                CallBackStore<T>.createListerner(value);
            }
        }

        public virtual void OnObjectDestroyed<T>(T value)
        {
            if (CallBackStore<T>.destroyListerner != null)
            {
                CallBackStore<T>.destroyListerner(value);
            }
        }

        public void AddCreateEventListener<T>(Action<T> listerner)
        {
            CallBackStore<T>.createListerner += listerner;
        }

        public void RemoveCreateEventListener<T>(Action<T> listerner)
        {
            CallBackStore<T>.createListerner -= listerner;
        }

        public void AddDestroyEventListener<T>(Action<T> listerner)
        {
            CallBackStore<T>.destroyListerner += listerner;
        }

        public void RemoveDestroyEventListener<T>(Action<T> listerner)
        {
            CallBackStore<T>.destroyListerner -= listerner;
        }
        
    }
  
}
