using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{   
    public static class Service
    {
        static class Singleton<T>
        {
            public static T Instance;
            public static bool Locked;
        }        

        public static void Set<T>(T service) where T:class
        {
            Singleton<T>.Instance = service;
        }

        public static void Set<T>() where T : class,new()
        {
            T srv = Get<T>();
            if (srv == null)
                Set<T>(new T());
        }

        internal static void SetLockedService<T>(T service) where T: class
        {
            Singleton<T>.Instance = service;
            Singleton<T>.Locked = true;
        }

        internal static void SetLockedService<T>() where T : class, new()
        {
            T srv = Get<T>();
            if (srv == null)
                SetLockedService<T>(new T());
        }

        public static void Remove<T>() where T : class
        {
            if(Singleton<T>.Locked)
                throw new InvalidOperationException("Service is Locked");
            Singleton<T>.Instance = null;
        }

        internal static void RemoveLockedService<T>() where T : class
        {
            Singleton<T>.Instance = null;
        }

        public static T Get<T>() where T : class
        {            
            return Singleton<T>.Instance;
        }

        public static T GetNonNull<T>() where T : class
        {
            var instance = Singleton<T>.Instance;
            if (instance == null) throw new NullReferenceException("There is any service of type " + typeof(T).FullName);
            return instance;
        }

        public static bool Invoke<T>(Action<T> action) where T : class
        {
            T service = Get<T>();
            if (service != null)
            {
                action(service);
                return true;
            }
            return false;
        }

        public static T Require<T>() where T : class
        {
            var service = Get<T>();
            if (service == null)
            {
                service = CreateInstance<T>();
                Set(service);
            }
            return service;
        }

        public static void SetFactory<T>() where T : new()
        {
            Factory<T> fac = new Factory<T>();
            Singleton<IFactory<T>>.Instance = fac;            
        }

        public static void SetFactory<T>(IFactory<T> factory)
        {
            Singleton<IFactory<T>>.Instance = factory;               
        }

        public static void SetFactory<T>(Func<T>func)
        {            
            Singleton<IFactory<T>>.Instance = new DelegateFactory<T>(func);
        }

        public static IFactory<T> GetFactory<T>()
        {
            var fact =Singleton<IFactory<T>>.Instance;
            if (fact == null)
                throw new InvalidOperationException("No factory for " + typeof(T).FullName);
            return fact;
        }

        public static T CreateInstance<T>()
        {
            IFactory<T> factory = Get<IFactory<T>>();
            if (factory != null)
                return factory.CreateInstance();
            else
            {
                try
                {
                    return Activator.CreateInstance<T>();
                }
                catch (Exception e)
                {
                    throw e.InnerException ?? e;
                }
            }
        }
       
        public static event EventHandler Initialize;
        
        public static void OnInitialize()
        {
            if (Initialize != null)
                Initialize(null, EventArgs.Empty);
        }

        /// <summary>
        /// Slow version of GetService<T>, do not use this on realtime production if it not necesary. instead use the 
        /// faster generic version.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// 
        public static object Get(Type type)
        {
            var genType = typeof(Singleton<>).MakeGenericType(type);
            return genType.InvokeMember("Instance", System.Reflection.BindingFlags.GetField, null, null, null);
        }
    }
}
