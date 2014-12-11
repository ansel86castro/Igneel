using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Igneel
{
    /// <summary>
    /// Encapsulate all the GEngine Components
    /// </summary>
    public interface IResourceAllocator : IDisposable
    {
        /// <summary>
        /// Tells if the object have been disposed
        /// </summary>
        /// 
        [Browsable(false)]
        [NonSerializedPropAttribute]
        bool Disposed { get; }
    }  

    public interface INameable
    {
        /// <summary>
        /// Identifier of the object, is used for quickly lookup
        /// </summary>
        /// 
        string Name { get;  }
    }

    public interface IIdentificable
    {
        int Id { get; }
    }

    public interface INameChangingNotificator
    {
        event Action<object, string> NameChanged;
    }

    [Serializable]    
    public class ResourceAllocator : IResourceAllocator
    {
        [NonSerialized]
        private bool disposed;

        public event EventHandler Disposing;

        ~ResourceAllocator()
        {            
            OnDispose(false);
        }

        [Browsable(false)]
        [NonSerializedPropAttribute]
        public bool Disposed
        {
            get { return disposed; }
            protected set { disposed = value; }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (Disposing != null)
                    Disposing(this, EventArgs.Empty);

                Disposing = null;

                OnDispose(true);

                disposed = true;
                GC.SuppressFinalize(this);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing">dispose managed objects</param>
        protected virtual void OnDispose(bool disposing) { }
        
    }
    
}
