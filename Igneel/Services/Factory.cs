using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Services
{
    public interface IFactory<out T>
    {
        T CreateInstance();
    }

    public interface IFactory<out T, in P>
    {
        T CreateInstance(P data);
    }

    public class Factory<T> : IFactory<T> where T : new()
    {
        public T CreateInstance()
        {
            try
            {
                return new T();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    throw e.InnerException;
                else
                    throw e;
            }
        }
    }

    public class DelegateFactory<T> : IFactory<T>
    {
        private Func<T> factoryMethod;
        public DelegateFactory(Func<T> factoryMethod)
        {
            this.factoryMethod = factoryMethod;
        }

        public T CreateInstance()
        {
            return factoryMethod();
        }
    }

    public class SingletonFactory<T> : IFactory<T>
    {
        static T instance;      

        public T CreateInstance()
        {
            if (instance == null)
            {
                var factory = Service.Get<IFactory<T>>();
                instance = factory.CreateInstance();
            }
            return instance;

        }
    }

    public class SingletonDisposableFactory<T> : IFactory<T>
        where T : IResourceAllocator
    {
        static T instance;
        IFactory<T>factory;

        public SingletonDisposableFactory() { }

        public SingletonDisposableFactory(IFactory<T> factory)
        {
            this.factory = factory;
        }

        public T CreateInstance()
        {
            if (instance == null || instance.Disposed)
            {
                if (factory == null)
                    factory = Service.Get<IFactory<T>>();
                instance = factory.CreateInstance();
            }
            return instance;
        }
    }

    public class SingletonDisposableFactoryNew<T> : SingletonDisposableFactory<T>
        where T : class, IResourceAllocator,  new()
    {
        public SingletonDisposableFactoryNew()
            : base(new Factory<T>())
        {

        }
    }

    public class SingletonDisposableFactoryDelegate<T> : SingletonDisposableFactory<T>
       where T : class, IResourceAllocator
    {
        public SingletonDisposableFactoryDelegate(Func<T>factoryMethod)
            : base(new DelegateFactory<T>(factoryMethod))
        {

        }
    }
    
    
}
