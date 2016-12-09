using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Igneel.Assets.Data;

namespace Igneel.Assets
{
    public class ResourceRepository : IReferenceManager
    {
        private readonly IgneelEngineResourcesEntities _dbContext;
        private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();
        private readonly MemoryStream _stream = new MemoryStream(1048576);// 1mb buffer
        private readonly ResourceManager _resourceManager;
        private readonly Dictionary<int, ResourceReference> _references = new Dictionary<int, ResourceReference>();
        

        public ResourceRepository(ResourceManager resourceManager)
        {
            _dbContext = new IgneelEngineResourcesEntities();
            _resourceManager = resourceManager;
        }
      

        public ResourceManager Manager
        {
            get { return _resourceManager; }
        }

     

        public ResourceReference Save(IResource resource, ResourceOperationContext context)
        {
            if (context.CurrentUser == null)
                throw new InvalidOperationException("User not found");

            Data.Resource dbResource = null;                       

            //check if the resource already exist in the database
            if (resource.Id > 0)             
                dbResource = _dbContext.Resources.Find(resource.Id);

            if (dbResource != null)
            {
                //Update the Resource and Content
                dbResource.Name = resource.Name;
                dbResource.LastUpdateDate = DateTime.Now;
                dbResource.UpdateUserId = context.CurrentUser.UserId;
            }
            else
            {
                //Else this is a new resource that was created or was delete from the respository.
                //So create the Resource ,the Content Store and add the resource to the ResourceManager's cache
                var resourceType = GetResourceType(resource);
                var folder = GetFolder(resourceType);

                dbResource = _dbContext.Resources.Create();
                dbResource.CreateUserId = context.CurrentUser.UserId;
                dbResource.UpdateUserId = context.CurrentUser.UserId;
                dbResource.LastUpdateDate = DateTime.Now;
                dbResource.Name = resource.Name;
                dbResource.ResourceTypeId = resourceType != null ? (int?) resourceType.ResourceTypeId : null;
                dbResource.FolderId = folder.FolderId;                

                dbResource = _dbContext.Resources.Add(dbResource);
                _dbContext.SaveChanges();
                
                resource.Id = dbResource.Id;

                //The resoures does'n has an Id until it was saved. 
                //Then initialize the reference counter
                _resourceManager.Add(resource);
            }

            //mark as saved
            context.SetIsSaved(resource, true);

            //Check if the resource is saved with a context Scene
            if (context.Scene != null && !context.Scene.IsDesignOnly)
            {
                //IF true then link the resource to the context Scene
                if (!_dbContext.SceneResources.Any(x => x.ResourceId == dbResource.Id &&
                                                        x.SceneId == context.Scene.Id))
                {
                    //create the SceneResource
                    SceneResource sceneResource = new SceneResource()
                    {
                        ResourceId = dbResource.Id,
                        SceneId = context.Scene.Id,
                    };
                    _dbContext.SceneResources.Add(sceneResource);
                }
            }

            //Creates the Reference
            ResourceReference reference;
            if (!_references.TryGetValue(dbResource.Id, out reference))
            {
                reference = new ResourceReference(dbResource.Id);
                _references.Add(dbResource.Id, reference);
            }

            if (dbResource.Content == null)
            {
                //Creates the resource content
                dbResource.Content = new Content();
            }

            //write the resource content data
            var data = CreateAsset(resource, context);
            Debug.Assert(data != null, "Invalid Resource " + resource.Name);
            dbResource.Content.Data = data;

            _dbContext.SaveChanges();

            return reference;
        }

        private ResourceType GetResourceType(IResource resource)
        {
            var resourceTypeAttribute = resource.GetType().GetCustomAttribute<ResourceTypeAtribute>();
            if (resourceTypeAttribute != null)
            {
                return 
                    _dbContext.ResourceTypes.Include(x => x.Folder)
                        .FirstOrDefault(x => x.Name == resourceTypeAttribute.ResourceType);
            }
            return null;
        }

        private Folder GetFolder(ResourceType resourceType)
        {          
            Folder folder = null;            
                //Locate the default Resource Folder                
            if (resourceType != null)
            {
                if (resourceType.DefaultFolderId != null)
                    folder = resourceType.Folder;
            }            

            if (folder == null)
            {
                folder = _dbContext.Folders.SingleOrDefault(x => x.Name == "Default");

                if (folder == null)
                    throw new InvalidOperationException("Default Folder Not Found");
            }
            return folder;
        }

        private byte[] CreateAsset(IResource resource, ResourceOperationContext context)
        {
            var asset = resource.CreateAsset(context);
            if (asset == null)
                return null;

            _stream.Seek(0, SeekOrigin.Begin);
           _binaryFormatter.Serialize(_stream, asset);
            return _stream.ToArray();
        }        
      
        /// <summary>
        /// Finds a ResourceReference for the resource. This is used during saved operations and asset creations
        /// </summary>
        /// <param name="resource">The resoure</param>
        /// <param name="context">Context Information</param>
        /// <returns></returns>
        public ResourceReference GetReference(IResource resource, ResourceOperationContext context)
        {
            ResourceReference reference = null;
            if (resource.Id == 0)
            {
                //Else this is a new resource, so the resource doesn't exist in the database
                reference = Save(resource, context);
            }
            //check if the resource was loaded and a reference was store
            else if (_references.TryGetValue(resource.Id, out reference))
            {
                //check if the resource was modified
                if (!context.IsSaved(resource))
                    reference = Save(resource, context);
            }   
            //check if the resource was delete
            else if (_dbContext.Resources.Find(resource.Id) == null)
            {
                //it is invalid to return a reference to a deleted resource
                //throw new InvalidOperationException(String.Format("The Resource with Id='{0}' Name='{1}' was Delete", resource.Id, resource.Name));
                return null;
            }
            else
            {
                //Else the resource is already store in the database but was loaded
                //by another ResourceDatabase instance
                reference = new ResourceReference(resource.Id);
                _references.Add(resource.Id, reference);
            }
            return reference;
        }

        /// <summary>
        /// Used when resource loading to resolve resource references
        /// </summary>
        /// <param name="reference">The resource referebce</param>
        /// <param name="context">Contains additional data</param>
        /// <returns></returns>
        public IResource GetResource(ResourceReference reference, ResourceOperationContext context)
        {
            IResource resource = null;

            //Check if the user explicit request the loading of the user by passing a Path Reference.
            //The path must be absolute from a Root Folder. A Root Folder has no parent
            if (!string.IsNullOrEmpty(reference.Path))
            {
                Data.Resource dbResource = GetResourceFromPath(reference.Path, context);
                //deserialize the resource store and creates the resource
                resource = CreateResource(dbResource, context);
                if (resource == null)
                    throw new InvalidOperationException("Resource not found");                
            }
            //Else check if the resource if requested by another resource
            else if (reference.Id != null)
            {
               resource = GetResourceFromId((int)reference.Id, context);
            }

            if (resource == null)
                throw new InvalidOperationException("Resource not found");
            return resource;
        }

        private IResource GetResourceFromId(int id, ResourceOperationContext context)
        {
            IResource resource;
            //check if the resource is already loaded
            if (_resourceManager.TryGetResource(id, out resource))
                return resource;

            //else load the resource from the database
            var dbResource = _dbContext.Resources.Find(id);
            if (dbResource == null)
                throw new InvalidOperationException("Resource not found");

            resource = CreateResource(dbResource, context);
            return resource;
        }

        private Data.Resource GetResourceFromPath(string path, ResourceOperationContext context)
        {
            Data.Resource dbResource = null;

            IResource resource = null;
            //Get the list of folders's names and in the Path. the resource's name 
            //is in the last position of the array
            var names = path.Split('/');

            //Check if the path contains the name of the resource only 
            if (names.Length == 1)
            {
                //load the resource store from the database
                dbResource = _dbContext.Resources.FirstOrDefault(x => x.Name == names[0]);
              
            }
            else if (names.Length > 1)
            {
                //Find the last folder int the path

                int? parentId = null; // the parent Folder 
                Folder folder = null; //The current Folder
                for (int i = 0; i < names.Length - 1; i++)
                {
                    if (string.IsNullOrEmpty(names[i]))
                        continue;

                    folder = _dbContext.Folders.FirstOrDefault(x => x.Name == names[i] && x.ParentId == parentId);
                    if (folder == null)
                        throw new InvalidOperationException(String.Format("Folder not found in path '{0}'",
                            path));

                    parentId = folder.FolderId;
                }

               dbResource = _dbContext.Resources.FirstOrDefault(x => x.Name == names[names.Length - 1]);
             
            }

            if (dbResource == null)
                throw new InvalidOperationException("Resource not found");

            return dbResource;          
        }

        private IResource CreateResource(Data.Resource dbResource, ResourceOperationContext context)
        {
            if (dbResource == null)
            {
                Debug.Assert(dbResource != null, "Database Resource not found");
            }

            //get the Serialized Data Stream
            var content = dbResource.Content.Data;

            MemoryStream stream = new MemoryStream(content, false);

            //Deserialize the content into a Asset
            Asset asset = (Asset)_binaryFormatter.Deserialize(stream);

            //creates the resources
            var resource = asset.CreateResource(context) as IResource;
            if(resource == null)
            {
                Debug.Assert(resource != null, string.Format("Resource Id='{0}' Name='{1}' can not be crated", dbResource.Id, dbResource.Name));
            }

            //initialize the reference counter
            _resourceManager.Add(resource);

            return resource;
        }

        public void Remove(ResourceReference reference, ResourceOperationContext context)
        {
            Data.Resource resource = null;            
            if (!string.IsNullOrEmpty(reference.Path))
            {
                //Find the Resource Store
                resource = GetResourceFromPath(reference.Path, context);                
            }
            else if(reference.Id!=null)
            {
                resource = _dbContext.Resources.Find((int)reference.Id);
            }

            Debug.Assert(resource!=null);

            if (context.Scene != null)
            {
                //Remove the resource from the scene first
                var sceneResource = _dbContext.SceneResources.Find(resource.Id, context.Scene.Id);
                _dbContext.SceneResources.Remove(sceneResource);
            }

            //Remove the resource
            _dbContext.Resources.Remove(resource);
            _dbContext.SaveChanges();

            _references.Remove(resource.Id);

        }

    }
}
