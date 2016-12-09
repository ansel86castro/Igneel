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

   
  
}
