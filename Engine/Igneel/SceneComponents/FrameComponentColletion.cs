using System.Collections.Generic;
using System.Linq;
using Igneel.Graphics;
using Igneel.SceneManagement;

namespace Igneel.SceneComponents
{
   //[ResourceActivator(typeof(FrameComponentColletion.Activator))]
    public class FrameComponentColletion : ComponentInstance, ICollection<IGraphicsProvider> ,IBoundable,IInitializable
    {
       List<IGraphicsProvider> _objects;         

        public FrameComponentColletion()          
        {
            _objects = new List<IGraphicsProvider>(); 
        }

        public Sphere BoundingSphere
        {
            get;
            private set;
        }

        public OrientedBox BoundingBox
        {
            get;
            private set;
        }

       /// <summary>
       /// Must be called after all the items were added
       /// </summary>
        public void Initialize()
        {
            var positions = _GetVolumePoints().ToArray();
            this.BoundingSphere = new Sphere(positions);
            this.BoundingBox = OrientedBox.Create(positions);            
        }       


        public override void OnNodeAttach(Frame node)
        {
            base.OnNodeAttach(node);       

            foreach (var item in _objects)
            {
                item.OnNodeAttach(node);
            }            
        }

        public override void OnNodeDetach(Frame node)
        {          
            foreach (var item in _objects)
            {
                item.OnNodeDetach(node);
            }
            base.OnNodeDetach(node);
        }

        public override void OnPoseUpdated(Frame node)
        {           
            foreach (var item in _objects)
            {
                item.OnPoseUpdated(node);
            }
        }

        public override int SubmitGraphics(Scene scene, Frame node, ICollection<GraphicSubmit> collection)
        {
            int count = 0;
            foreach (var item in _objects)
            {
                count += item.SubmitGraphics(scene ,node, collection);
            }
            return count;
        }

        public IGraphicsProvider this[int idx]
        {
            get { return _objects[idx]; }
        }

        public void Add(IGraphicsProvider item)
        {
            _objects.Add(item);
            if (Node != null)
                item.OnNodeAttach(Node);            
        }

        public void Clear()
        {
            if (Node != null)
            {
                foreach (var item in _objects)
                {
                    item.OnNodeDetach(Node);
                }
            }
            _objects.Clear();
        }

        public bool Contains(IGraphicsProvider item)
        {
            return _objects.Contains(item);
        }

        public void CopyTo(IGraphicsProvider[] array, int arrayIndex)
        {
            _objects.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _objects.Count; }
        }

        bool ICollection<IGraphicsProvider>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IGraphicsProvider item)
        {
            if (_objects.Remove(item))
            {
                var node = Node;
                if (node != null)
                    item.OnNodeDetach(node);
                return true;
            }
            return false;
        }

        IEnumerator<IGraphicsProvider> IEnumerable<IGraphicsProvider>.GetEnumerator()
        {
            return _objects.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _objects.GetEnumerator();
        }

        public List<IGraphicsProvider>.Enumerator GetEnumerator()
        {
            return _objects.GetEnumerator();
        }
       
       private IEnumerable<Vector3> _GetVolumePoints()
        {
            foreach (var obj in _objects)
            {
                IBoundable bound = obj as IBoundable;
                if (bound == null)
                    continue;

                var boundingBox = bound.BoundingBox;
                var boundingSphere = bound.BoundingSphere;
                if (boundingBox != null)
                {
                    BoxBuilder box = new BoxBuilder(2, 2, 2);

                    for (int i = 0; i < box.Vertices.Length; i++)
                    {
                        var mat = Matrix.Scale(boundingBox.Extends) * boundingBox.GlobalPose;
                        yield return Vector3.Transform(box.Vertices[i].Position, mat);
                    }
                }
                else if (boundingSphere.Radius > 0)
                {
                    SphereBuilder sphere = new SphereBuilder(16, 16, boundingSphere.Radius );
                    for (int i = 0; i < sphere.Vertices.Length; i++)
                    {
                        var mat = Matrix.Translate(boundingSphere.Center);
                        yield return Vector3.Transform(sphere.Vertices[i].Position, mat);
                    }
                }
            }
            
        }        

        //[Serializable]
        //class Activator : IResourceActivator
        //{
        //    AssetReference[] _references;

        //    public void Initialize(IAssetProvider provider)
        //    {
        //        var objects = (FrameComponentColletion)provider;
        //        _references = new AssetReference[objects.Count];
        //        for (int i = 0; i < _references.Length; i++)
        //        {
        //            _references[i] = AssetManager.Instance.GetAssetReference(objects[i]);
        //        }
        //    }

        //    public IAssetProvider OnCreateResource()
        //    {
        //        var provider = new FrameComponentColletion();
        //        foreach (var refe in _references)
        //        {
        //            provider.Add((IGraphicsProvider)AssetManager.Instance.GetAssetProvider(refe));
        //        }
        //        provider.Initialize();
        //        return provider;
        //    }
        //}

      
    }
}
