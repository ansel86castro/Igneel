using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;
using Igneel;
using System.Xml.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using System.Runtime.InteropServices;
using Igneel.Components;

namespace Igneel
{  
    public static class GraphicDSL
    {
        public static SceneNode Transformed(this SceneNode obj, Vector3 translation = default(Vector3), Matrix rotation = default(Matrix), Vector3 scale = default(Vector3))           
        {
            obj.LocalPosition = translation;
            obj.LocalRotation = rotation;
            if (scale == default(Vector3))
                scale = new Vector3(1, 1, 1);
            obj.LocalScale = scale;
            obj.UpdateLocalPose();
            obj.CommitChanges();
            return obj;
        }

        public static SceneNode Transformed(this SceneNode obj, Vector3 translation = default(Vector3), Euler orientation = default(Euler), Vector3 scale = default(Vector3))            
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
          UpdateEventHandler updateEvent = null) where T : IDynamicNotificable, IRenderable
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
