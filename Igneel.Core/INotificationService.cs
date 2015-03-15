using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public interface INotificationService
    {

        //event EventHandler ProjectCreated;
        //event EventHandler ProjectLoaded;
        //event EventHandler GraphicSceneCreated;
        //event EventHandler GraphicSceneDestroyed;
        //event EventHandler PhysicSceneCreated;
        //event EventHandler PhysicSceneDestroyed;

        //void OnProjectCreated();

        //void OnProjectLoaded();

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
