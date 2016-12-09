using System;
using System.Collections.Generic;

namespace Igneel.Assets
{
    public class ResourceManager
    {        
        readonly Dictionary<int, ResourceHandle> _resources = new Dictionary<int,ResourceHandle>();        

        public event EventHandler<IResource> Disposing;       

        /// <summary>
        /// Find the ResourceHandle
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        //private ResourceHandle _GetHandle(IResource resource)
        //{
        //    ResourceHandle handle;
        //    if (resource.Id > 0)
        //    {               
        //        _resources.TryGetValue(resource.Id, out handle);
        //        return handle;
        //    }

        //    int id;
        //    if (_createdResources.TryGetValue(resource, out id))
        //    {
        //        _resources.TryGetValue(resource.Id, out handle);
        //        return handle;
        //    }
        //}


        public void AddReference(IResource resource)
        {
            ResourceHandle handle;
            if (_resources.TryGetValue(resource.Id, out handle))
            {
                handle.AddRef();
            }
        }

        public void Clear()
        {
            _resources.Clear();
        }

        public bool ContainsResource(int id)
        {
            return _resources.ContainsKey(id);
        }

        public bool Release(IResource resource)
        {
            var key = resource.Id;
            ResourceHandle handle;

            if (_resources.TryGetValue(key, out handle))
            {
                if (handle.ReleaseRef())
                {
                    //dispose the resource and remove it from the cache                 
                    _resources.Remove(key);
                    OnDisposing(handle.Resource);
                    return true;
                }
            }
            return false;
        }

        public bool TryGetResource(int id, out IResource value)
        {
            ResourceHandle handle;
            if (_resources.TryGetValue(id, out handle))
            {
                value = handle.Resource;
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>
        /// returns a enumerator for the resources contained in the cache
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IResource> EnumerateResources()
        {
            foreach (var item in _resources.Values)
            {
                var res = item.Resource;
                if (res != null)
                    yield return item.Resource;
            }
        }

        /// <summary>
        /// returns the resource        
        /// </summary>     
        /// <param name="id">The Resource Database Id</param>
        /// <returns>The resource or null if the resource has been disposed or has not been loaded</returns>
        public IResource this[int id]
        {
            get
            {
                ResourceHandle handle;
                if (_resources.TryGetValue(id, out handle))
                {
                    var value = handle.Resource;                    
                    return value;
                }
                return null;
            }          
        }

        /// <summary>
        /// initialize the reference counter.
        /// The resource must be store in the database with an Id greather than 0 
        /// </summary>
        /// <param name="resource"></param>
        public void Add(IResource resource)
        {
            _resources.Add(resource.Id, new ResourceHandle(resource));
        }

        public bool Remove(int id)
        {
            ResourceHandle handle = GetHandle(id);
            if (handle != null)
            {
                if (handle.Resource != null)
                    OnDisposing(handle.Resource);
                return _resources.Remove(id);
            }
            return false;
        }

        public bool Remove(IResource resource)
        {
            OnDisposing(resource);
            return Remove(resource.Id);
        }

        private ResourceHandle GetHandle(int id)
        {
            ResourceHandle handle;
            _resources.TryGetValue(id, out handle);
            return handle;
        }

        class ResourceHandle
        {
            /// <summary>
            /// Reference counter
            /// </summary>
            public int ReferenceCounter;

            /// <summary>
            /// Reference of the Resource
            /// </summary>
            public readonly WeakReference<IResource> Reference;

            public ResourceHandle(IResource resource)
            {
                this.Reference = new WeakReference<IResource>(resource);
                this.ReferenceCounter = 1;
            }

            /// <summary>
            /// The Resource's reference .
            /// returns null if the resource was already disposed by the garbage collector
            /// </summary>
            public IResource Resource
            {
                get
                {
                    IResource value;
                    if (Reference.TryGetTarget(out value))
                    {
                        return value;
                    }
                    return null;
                }
            }

            public void AddRef()
            {
                ++ReferenceCounter;
            }


            /// <summary>
            /// returns true if it is safe to dispose the resource
            /// </summary>
            /// <returns></returns>
            public bool ReleaseRef()
            {
                //there are no reference to the resource
                if (ReferenceCounter == 0)
                    return false;

                IResource value = Resource;
                if (value == null || value.Disposed)
                {
                    //the resorce is already disposed by the garbage collector
                    ReferenceCounter = 0;
                    return false;
                }

                --ReferenceCounter;
                return ReferenceCounter == 0;
            }
        }

        protected virtual void OnDisposing(IResource e)
        {
            var handler = Disposing;
            if (handler != null) handler(this, e);
        }     
     
    }
}
