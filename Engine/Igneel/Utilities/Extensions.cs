using System;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Utilities
{  
    public static class GraphicDsl
    {
        public static Frame Transformed(this Frame obj, Vector3 translation = default(Vector3), Matrix rotation = default(Matrix), Vector3 scale = default(Vector3))           
        {
            obj.LocalPosition = translation;
            obj.LocalRotation = rotation;
            if (scale == default(Vector3))
                scale = new Vector3(1, 1, 1);
            obj.LocalScale = scale;
            obj.ComputeLocalPose();
            obj.CommitChanges();
            return obj;
        }

        public static Frame Transformed(this Frame obj, Vector3 translation = default(Vector3), Euler orientation = default(Euler), Vector3 scale = default(Vector3))            
        {
            return Transformed(obj, translation, orientation.ToMatrix(), scale);
        }
        public static T Translated<T>(this T obj, float x,float y,float z) where T : ITranslatable
        {
            obj.LocalPosition = new Vector3(x, y, z);
            return obj;
        }
        public static T Translated<T>(this T obj, Vector3 translation) where T : ITranslatable
        {
            obj.LocalPosition = translation;
            return obj;
        }

        public static T Rotated<T>(this T obj, float heading, float pitch, float roll) where T : IRotable
        {
            obj.LocalRotation = new Euler(heading, pitch, roll).ToMatrix();
            return obj;
        }

        public static T Rotated<T>(this T obj, Matrix rotation) where T : IRotable
        {
            obj.LocalRotation = rotation;
            return obj;
        }

        public static T Scaled<T>(this T obj, float sx, float sy, float sz) where T : IScalable
        {
            obj.LocalScale  = new Vector3(sx, sy, sz);
            return obj;
        }

        public static T Commit<T>(this T obj) where T : IDeferreable
        {
            obj.CommitChanges();
            return obj;
        }

        public static T Events<T>(this T obj,
          UpdateEventHandler updateEvent = null) where T : IDynamicNotificable, IDrawable
        {
            if (updateEvent != null)
                obj.UpdateEvent += updateEvent;            
            return obj;
        }      

        public static T Initialized<T>(this T obj, Action<T> action) where T : class ,IResourceAllocator
        {
            action(obj);
            return obj;
        }     
    }

}
