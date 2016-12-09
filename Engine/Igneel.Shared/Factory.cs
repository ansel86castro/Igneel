using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public interface IFactory<out T>
    {
        T CreateInstance();
    }

    public interface IFactory<out T, in TP>
    {
        T CreateInstance(TP data);
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
        private Func<T> _factoryMethod;
        public DelegateFactory(Func<T> factoryMethod)
        {
            this._factoryMethod = factoryMethod;
        }

        public T CreateInstance()
        {
            return _factoryMethod();
        }
    }

    public class SingletonFactory<T> : IFactory<T>
    {
        static T _instance;      

        public T CreateInstance()
        {
            if (_instance == null)
            {
                var factory = Service.Get<IFactory<T>>();
                _instance = factory.CreateInstance();
            }
            return _instance;

        }
    }

    public class SingletonDisposableFactory<T> : IFactory<T>
        where T : IResourceAllocator
    {
        static T _instance;
        IFactory<T>_factory;

        public SingletonDisposableFactory() { }

        public SingletonDisposableFactory(IFactory<T> factory)
        {
            this._factory = factory;
        }

        public T CreateInstance()
        {
            if (_instance == null || _instance.Disposed)
            {
                if (_factory == null)
                    _factory = Service.Get<IFactory<T>>();
                _instance = _factory.CreateInstance();
            }
            return _instance;
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
