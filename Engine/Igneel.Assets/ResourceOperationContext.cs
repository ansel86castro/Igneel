using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Assets.Data;

namespace Igneel.Assets
{
    public class ResourceOperationContext
    {        
        private User _user;
        private Folder _rootFolder;
        private IResource _scene;
        private readonly HashSet<int> _savedResources = new HashSet<int>();      

        public User CurrentUser
        {
            get { return _user; }
            set { _user = value; }
        }

        public Folder RootFolder
        {
            get { return _rootFolder; }
            set { _rootFolder = value; }
        }

        public IResource Scene
        {
            get { return _scene; }
            set { _scene = value; }
        }

        public object Data { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IReferenceManager ReferenceManager { get; set; }

        public ResourceOperationContext(User user, Folder rootFolder, IResource scene)
        {
            this._user = user;
            this._rootFolder = rootFolder;
            this._scene = scene;
        }

        public void SetIsSaved(IResource resource, bool saved)
        {
            if (!saved)
                _savedResources.Remove(resource.Id);
            else if (!_savedResources.Contains(resource.Id))
            {
                _savedResources.Add(resource.Id);
            }
        }

        public bool IsSaved(IResource resource)
        {
            return _savedResources.Contains(resource.Id);
        }

        public void ClearSaved()
        {
            _savedResources.Clear();
        }
    }
}
