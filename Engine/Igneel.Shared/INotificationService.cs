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
            public static Action<T> CreateListerner;
            public static Action<T> DestroyListerner;
        }      
      

        public virtual void OnObjectCreated<T>(T value)
        {
            if (CallBackStore<T>.CreateListerner != null)
            {
                CallBackStore<T>.CreateListerner(value);
            }
        }

        public virtual void OnObjectDestroyed<T>(T value)
        {
            if (CallBackStore<T>.DestroyListerner != null)
            {
                CallBackStore<T>.DestroyListerner(value);
            }
        }

        public void AddCreateEventListener<T>(Action<T> listerner)
        {
            CallBackStore<T>.CreateListerner += listerner;
        }

        public void RemoveCreateEventListener<T>(Action<T> listerner)
        {
            CallBackStore<T>.CreateListerner -= listerner;
        }

        public void AddDestroyEventListener<T>(Action<T> listerner)
        {
            CallBackStore<T>.DestroyListerner += listerner;
        }

        public void RemoveDestroyEventListener<T>(Action<T> listerner)
        {
            CallBackStore<T>.DestroyListerner -= listerner;
        }

    }
}
