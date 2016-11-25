using System;

namespace Igneel
{
    [Serializable]    
    public class ResourceAllocator : IResourceAllocator
    {
        [NonSerialized]
        private bool _disposed;

        public event EventHandler Disposing;

        ~ResourceAllocator()
        {            
            OnDispose(false);
        }

       
        [NonSerializedProp]
        public bool Disposed
        {
            get { return _disposed; }
            protected set { _disposed = value; }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (Disposing != null)
                    Disposing(this, EventArgs.Empty);

                Disposing = null;

                OnDispose(true);

                _disposed = true;
                GC.SuppressFinalize(this);

            }
        }

        /// <summary>
        /// Dispose unmanaged resources
        /// </summary>
        /// <param name="disposing">set to true if this is called explicit by the user and it is false if
        /// this method is called from the object finalizer. In both cases you must release the unmanaged resources here
        /// and set the null references to managed objects. You must call Dipose on referenced objets only when the parameter disposing is true
        /// </param>
        /// <returns></returns>
        protected virtual void OnDispose(bool disposing) { }
        
    }
}