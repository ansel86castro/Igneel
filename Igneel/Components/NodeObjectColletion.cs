using Igneel.Assets;
using Igneel.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
   [ProviderActivator(typeof(NodeObjectColletion.Activator))]
    public class NodeObjectColletion : ExclusiveNodeObject, ICollection<INodeObject>
    {        
        List<INodeObject> objects = new List<INodeObject>();            

        public NodeObjectColletion()          
        {

        }       

        public override void OnNodeAttach(SceneNode node)
        {                       
            foreach (var item in objects)
            {
                item.OnNodeAttach(node);
            }

            base.OnNodeAttach(node);
        }

        public override void OnNodeDetach(SceneNode node)
        {          
            foreach (var item in objects)
            {
                item.OnNodeAttach(node);
            }         
            base.OnNodeDetach(node);
        }

        public override void OnPoseUpdated(SceneNode node)
        {           
            foreach (var item in objects)
            {
                item.OnPoseUpdated(node);
            }
        }

        public override int GetGraphicObjects(SceneNode node, ICollection<DrawingEntry> collection)
        {
            int count = 0;
            foreach (var item in objects)
            {
                count += item.GetGraphicObjects(node, collection);
            }
            return count;
        }

        public INodeObject this[int idx]
        {
            get { return objects[idx]; }
        }

        public void Add(INodeObject item)
        {
            objects.Add(item);
            if (Node != null)
                item.OnNodeAttach(Node);            
        }

        public void Clear()
        {
            if (Node != null)
            {
                foreach (var item in objects)
                {
                    item.OnNodeDetach(Node);
                }
            }
            objects.Clear();
        }

        public bool Contains(INodeObject item)
        {
            return objects.Contains(item);
        }

        public void CopyTo(INodeObject[] array, int arrayIndex)
        {
            objects.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return objects.Count; }
        }

        bool ICollection<INodeObject>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(INodeObject item)
        {
            if (objects.Remove(item))
            {
                var node = Node;
                if (node != null)
                    item.OnNodeDetach(node);
                return true;
            }
            return false;
        }

        IEnumerator<INodeObject> IEnumerable<INodeObject>.GetEnumerator()
        {
            return objects.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return objects.GetEnumerator();
        }

        public List<INodeObject>.Enumerator GetEnumerator()
        {
            return objects.GetEnumerator();
        }

       public void CollectionComplete()
       {
           var positions = _GetVolumePoints().ToArray();
           sphere = new Sphere(positions);
           box  =OrientedBox.Create(positions);
       }       

       private IEnumerable<Vector3> _GetVolumePoints()
        {
            foreach (var obj in objects)
            {
                var boundingBox = obj.BoundingBox;
                var boundingSphere = obj.BoundingSphere;
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

        [Serializable]
        class Activator : IProviderActivator
        {
            AssetReference[] references;

            public void Initialize(IAssetProvider provider)
            {
                var objects = (NodeObjectColletion)provider;
                references = new AssetReference[objects.Count];
                for (int i = 0; i < references.Length; i++)
                {
                    references[i] = AssetManager.Instance.GetAssetReference(objects[i]);
                }
            }

            public IAssetProvider CreateInstance()
            {
                var provider = new NodeObjectColletion();
                foreach (var refe in references)
                {
                    provider.Add((INodeObject)AssetManager.Instance.GetAssetProvider(refe));
                }
                provider.CollectionComplete();
                return provider;
            }
        }
    }
}
