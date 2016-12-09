using System;

namespace Igneel.Assets
{
    public abstract class Resource : IResource
    {        
        private int _id;

        private string _name;     

        [NonSerialized] private bool _disposed;

        [NonSerialized] private ResourceManager _resourceManager;
        
        private bool _isDesignOnly;

        protected Resource()
        {
            
        }

        protected Resource(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        protected Resource(string name, ResourceManager resourceManager)
        {
            _name = name;
            _resourceManager = resourceManager;
        }

        [AssetMember]
        public bool IsDesignOnly
        {
            get { return _isDesignOnly; }
            set { _isDesignOnly = value; }
        }

        [AssetMember]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [AssetMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
       

        [NonSerializedProp]
        public bool Disposed
        {
            get { return _disposed; }
            protected set { _disposed = value; }
        }

         [NonSerializedProp]
        public ResourceManager Manager
        {
            get { return _resourceManager; }
            set { _resourceManager = value; }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~Resource()
        {            
            _Dispose(false);
            _disposed = true;
        }

        public void Dispose()
        {
            if (!_disposed && _Dispose(true))
            {
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        private bool _Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_resourceManager != null)
                {
                    //called from user
                    if (_resourceManager.Release(this))
                    {
                        OnDispose(true);
                        return true;
                    }
                    return false;
                }
                OnDispose(true);
                return true;
            }
            else
            {
                //called from finalizer
                if (_resourceManager != null)
                    _resourceManager.Remove(this);

                OnDispose(false);
                return true;
            }
        }

        /// <summary>
        /// Dispose unmanaged resources
        /// </summary>
        /// <param name="disposing">True if this method is called explicitly by the user or it is false if
        /// it is called from the object's finalizer. In both cases you must release the unmanaged resources here
        /// and set to null referenced managed objects. 
        /// You must call Dipose on referenced objets only when calling explicitly this method ,this is when the parameter disposing is true
        /// </param>
        /// <returns></returns>
        protected abstract void OnDispose(bool disposing);

        public bool Equals(IResource other)
        {
            if (other == null)
                return false;

            //if id>0 then exist in database
            return _id > 0 ? other.Id == _id : other == this;
        }

        public int CompareTo(IResource other)
        {
            return _id.CompareTo(other.Id);
        }

        public Asset CreateAsset(ResourceOperationContext context)
        {
            return Asset.Create(this, context);
        }

        public override string ToString()
        {
            return _name ?? base.ToString();
        }
    }
}
