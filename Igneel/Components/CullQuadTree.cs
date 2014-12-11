using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    public interface ICuller<T>
        where T:IBoundable
    {
        bool ComputeDimensions(IBoundable boundable);

        bool Add(T item);

        bool Contains(T item);

        bool Remove(T item);

        void Clear();

        void CullItems(Camera camera, ICollection<T> collection);
    }
   

    public class QuadTree<T> : ICuller<T>
        where T:IBoundable 
    {                    
        private int maxLevel;
        private List<QuadTreeNode<T>.ValueStore> cullingList = new List<QuadTreeNode<T>.ValueStore>();
        private Scene scene;       
        QuadTreeNode<T> root;      

        public QuadTree(Scene scene, int maxlevel = 10)
        {
            this.scene = scene;
            this.maxLevel = maxlevel;
        }

        //private QuadTree(RectangleF bounds, int level, int maxLevel, QuadTree<T> parent, ICullTester<T> tester)
        //{
        //    this.maxLevel = maxLevel;
        //    this.tester = tester;
        //    this.level = level;
        //    this.bounds = bounds;

        //    if(tester == null)throw new ArgumentNullException();
        //    if(level < 0) throw new ArgumentOutOfRangeException();
        //    if (!(bounds.Width > Numerics.Epsilon && bounds.Height > Numerics.Epsilon)) throw new ArgumentOutOfRangeException();

        //    this.parent = parent;
        //    float halfDx = bounds.Width * 0.5f;
        //    float halfDz = bounds.Height * 0.5f;

        //    boundSphere.Center = new Vector3(bounds.X + halfDx, 0, bounds.Y - halfDz);
        //    boundSphere.Radius = (float)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height) * 0.5f;

        //    if (level == maxLevel)
        //        objects = new LinkedList<ValueStore>();
        //    //else
        //    //{
        //    //    level++;
        //    //    northWest = new QuadTree<T>(new RectangleF(bounds.X, bounds.Y, halfDx, halfDz), level, maxLevel, this, tester);
        //    //    northEast = new QuadTree<T>(new RectangleF(bounds.X + halfDx, bounds.Y, halfDx, halfDz), level, maxLevel, this, tester);
        //    //    southWest = new QuadTree<T>(new RectangleF(bounds.X, bounds.Y - halfDz, halfDx, halfDz), level, maxLevel, this, tester);
        //    //    southEast = new QuadTree<T>(new RectangleF(bounds.X + halfDx, bounds.Y - halfDz, halfDx, halfDz), level, maxLevel, this, tester);
        //    //}
        //}

        public int MaxLevel { get { return maxLevel; } }    

        public Scene Scene { get { return scene; } }

         public bool ComputeDimensions(IBoundable boundable)
        {
            var sphere = boundable.BoundingSphere;
            if (sphere.Radius == 0)
                return false;

            if (root == null || root.Bounds.TestInside(sphere) != InsideTestResult.Inside)
            {
                float width = sphere.Radius * 2f;
                float height = sphere.Radius * 2f;
                float x = sphere.Center.X - sphere.Radius;
                float y = sphere.Center.Z + sphere.Radius;
                RectangleF bounds = new RectangleF(x, y, width, height);
                if (root != null)
                {
                    bounds = bounds.Combine(root.Bounds);
                }
                List<QuadTreeNode<T>.ValueStore> objects = null;
                //get objects
                if (root != null)
                {                    
                    objects = new List<QuadTreeNode<T>.ValueStore>(root.NbElements);
                    root.AddItems(objects);
                }

                root = new QuadTreeNode<T>(bounds, 0, null, this);
                //reallocate objects               
                if (objects != null)
                {
                    foreach (var item in objects)
                    {
                        item.IsCulled = false;
                        root._Add(item);
                    }
                }

                return true;
            }
            return false;
        }

        public bool Add(T item)
         {           
             ComputeDimensions(item);
             return root._Add(new QuadTreeNode<T>.ValueStore { Value = item });
         }
      
        public bool Contains(T item)
        {
            if (root == null)
                return false;
            return root.Contains(item);
        }

        public bool Remove(T item)
        {
            if (root == null) return false;
            return root.Remove(item);
        }

        public void CullItems(Camera camera, ICollection<T> collection)
        {
            if (root == null)
                return;         
            
            cullingList.Clear();

            root._CullItems(camera, cullingList);

            collection.Clear();            
            foreach (var item in cullingList)
            {
                collection.Add(item.Value);
                item.IsCulled = false;
            }
        }

        public List<QuadTreeNode<T>.ValueStore> CullItems(Camera camera)
        {
            cullingList.Clear();

            if (root == null)
                return cullingList;
            
            root._CullItems(camera, cullingList);

            int count = cullingList.Count;
            for (int i = 0; i < count; i++)
            {
                cullingList[i].IsCulled = false;
            }

            return cullingList;
        }

        public void Clear()
        {
            root.Clear();
        }
    }

    public class QuadTreeNode<T>
        where T:IBoundable
    {
        public class ValueStore
        {
            public bool IsCulled;
            public T Value;

            //public LinkedList<QuadTreeNode<T>> Quads = new LinkedList<QuadTreeNode<T>>();

            public override string ToString()
            {
                return (IsCulled ? "Culled, " : "") + Value.ToString();
            }
        }

        QuadTree<T> quadTree;
        private RectangleF bounds;

        /*
            *************
            *  NW *  NE *
            *************
            *  SW *  SE *
            *************
        */
        private QuadTreeNode<T> northEast = null;
        private QuadTreeNode<T> northWest = null;
        private QuadTreeNode<T> southWest = null;
        private QuadTreeNode<T> southEast = null;
        private QuadTreeNode<T> parent = null;
        private int level;
        private Sphere boundSphere;
        private LinkedList<ValueStore> objects;      
        private int nbElements;

        public QuadTreeNode(RectangleF bounds, int level, QuadTreeNode<T> parent, QuadTree<T> quadTree)
        {         
            this.level = level;
            this.bounds = bounds;
            this.quadTree = quadTree;

            if(level < 0) throw new ArgumentOutOfRangeException();

            if (!(bounds.Width > Numerics.Epsilon && bounds.Height > Numerics.Epsilon)) throw new ArgumentOutOfRangeException();

            this.parent = parent;
            float halfDx = bounds.Width * 0.5f;
            float halfDz = bounds.Height * 0.5f;

            boundSphere.Center = new Vector3(bounds.X + halfDx, 0, bounds.Y - halfDz);
            boundSphere.Radius = (float)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height) * 0.5f;            
        }

        public bool IsLeaft { get { return level == quadTree.MaxLevel; } }

        public RectangleF Bounds { get { return bounds; } }

        public int Level { get { return level; } }

        public int NbElements { get { return nbElements; } }

        public LinkedList<ValueStore> Objects { get { return objects; } }

        public void _CullItems(Camera camera, List<ValueStore> collection)
        {
            if (nbElements == 0)
                return;

            var check = camera.ViewFrustum.GetCullResult(boundSphere);

            switch (check)
            {
                case CullTestResult.Inside:
                    _AddItems(camera, collection, false);
                    break;
                case CullTestResult.Partial:
                    if (objects != null)
                    {
                        var frustum = camera.ViewFrustum;
                        foreach (var item in objects)
                        {
                            if (!item.IsCulled && frustum.Contains(item.Value.BoundingSphere))
                            {
                                item.IsCulled = true;
                                collection.Add(item);
                            }
                        }
                    }

                    if (northWest != null && northWest.nbElements > 0)
                        northWest._CullItems(camera, collection);
                    if (northEast != null && northEast.nbElements > 0)
                        northEast._CullItems(camera, collection);
                    if (southWest != null && southWest.nbElements > 0)
                        southWest._CullItems(camera, collection);
                    if (southEast != null && southEast.nbElements > 0)
                        southEast._CullItems(camera, collection);

                    break;
            }

        }

        private void _AddItems(Camera camera, List<ValueStore> collection, bool partial)
        {
            if (nbElements == 0)
                return;
            if (objects != null)
            {
                var frustum = camera.ViewFrustum;
                foreach (var item in objects)
                {
                    if (!item.IsCulled && (!partial || frustum.Contains(item.Value.BoundingSphere)))
                    {
                        item.IsCulled = true;
                        collection.Add(item);

                    }
                }
            }

            if (northWest != null && northWest.nbElements > 0)
                northWest._AddItems(camera, collection, partial);
            if (northEast != null && northEast.nbElements > 0)
                northEast._AddItems(camera, collection, partial);
            if (southWest != null && southWest.nbElements > 0)
                southWest._AddItems(camera, collection, partial);
            if (southEast != null && southEast.nbElements > 0)
                southEast._AddItems(camera, collection, partial);

        }

        public void AddItems(List<ValueStore> collection)
        {
            if (nbElements == 0)
                return;
            if (objects != null)
            {              
                foreach (var item in objects)
                {
                    if (!item.IsCulled)
                    {
                        item.IsCulled = true;
                        collection.Add(item);

                    }
                }
            }

            if (northWest != null && northWest.nbElements > 0)
                northWest.AddItems(collection);
            if (northEast != null && northEast.nbElements > 0)
                northEast.AddItems(collection);
            if (southWest != null && southWest.nbElements > 0)
                southWest.AddItems(collection);
            if (southEast != null && southEast.nbElements > 0)
                southEast.AddItems(collection);
        }

        internal bool _Add(ValueStore item)
        {
            var testResult = bounds.TestInside(item.Value.BoundingSphere);
            if (testResult == InsideTestResult.Inside || testResult == InsideTestResult.PartialInside)
            {
                nbElements++;
                if (IsLeaft)
                {
                    if(objects==null)                        
                           objects = new LinkedList<ValueStore>();
                    objects.AddLast(item);
                   // item.Quads.AddLast(this);
                    return true;
                }
                else
                {
                    float halfDx = bounds.Width * 0.5f;
                    float halfDz = bounds.Height * 0.5f;

                    var northWestRec = new RectangleF(bounds.X, bounds.Y, halfDx, halfDz);
                    var northEastRec = new RectangleF(bounds.X + halfDx, bounds.Y, halfDx, halfDz);
                    var southWestRec = new RectangleF(bounds.X, bounds.Y - halfDz, halfDx, halfDz);
                    var southEastRec = new RectangleF(bounds.X + halfDx, bounds.Y - halfDz, halfDx, halfDz);

                    var b1 = AddOrCreate(item, northWestRec, ref northWest);
                    var b2 = AddOrCreate(item, northEastRec, ref northEast);
                    var b3 = AddOrCreate(item, southWestRec, ref southWest);
                    var b4 = AddOrCreate(item, southEastRec, ref southEast);
                    var result = b1 || b2 || b3 || b4;
                    if (!result)
                    {
                        if(objects==null)
                            objects = new LinkedList<ValueStore>();

                        objects.AddLast(item);
                        //item.Quads.AddLast(this);
                        return true;
                    }
                    return result;
                }
            }
            return false;
        }

        private bool AddOrCreate(ValueStore item, RectangleF quadBound, ref QuadTreeNode<T> node)
        {
            if (node != null)
            {
                return node._Add(item);
            }
            else
            {
                if (item.Value.BoundingSphere.IsInsideRect(quadBound))
                {
                    node = new QuadTreeNode<T>(quadBound, level + 1, this, quadTree);
                    return node._Add(item);
                }
                return false;
            }
        }

        public ValueStore _FindValueStorage(T item)
        {
            if (nbElements == 0)
                return null;

            if (!item.BoundingSphere.IsInsideRect(bounds))
                return null;

            if (objects!=null)
            {
                foreach (var x in objects)
                {
                    if (x.Value.Equals(item))
                        return x;
                }                
            }

            return (northWest != null ? northWest._FindValueStorage(item) : null) ??
                   (northEast != null ? northEast._FindValueStorage(item) : null) ??
                   (southWest != null ? southWest._FindValueStorage(item) : null) ??
                   (southEast != null ? southEast._FindValueStorage(item) : null);
        }

        public void Clear()
        {
            if (objects != null)
                objects.Clear();

            if (northEast != null)
                northEast.Clear();
            if (northWest != null)
                northWest.Clear();
            if (southEast != null)
                southEast.Clear();
            if (southWest != null)
                southWest.Clear();

        }

        public void Reshape(RectangleF bounds)
        {
            var invalids = new List<ValueStore>();

            _Reshape(bounds, invalids);

            foreach (var item in invalids)
            {
                _ReAlloc(item);
            }
        }

        public bool ReAlloc(T item)
        {
            //if (Remove(item))
            //{
            //    _Add(item);
            //}
            return false;
        }

        private void _ReAlloc(ValueStore store)
        {
            //foreach (var q in store.Quads)
            //    q.objects.Remove(store);

            //store.Quads.Clear();

            //store.IsCulled = false;          
        }

        private void _Reshape(RectangleF bounds, List<ValueStore> invalids)
        {
            this.bounds = bounds;
            float halfDx = bounds.Width * 0.5f;
            float halfDz = bounds.Height * 0.5f;
            boundSphere.Center = new Vector3(bounds.X + halfDx, 0, bounds.Y - halfDz);
            boundSphere.Radius = (float)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height) * 0.5f;
            
            if (IsLeaft)
            {
                foreach (var item in objects)
                {
                    if (item.Value.BoundingSphere.IsInsideRect(bounds) && !item.IsCulled)
                    {
                        item.IsCulled = true;
                        invalids.Add(item);
                    }
                }
            }
            else
            {
                if (northWest != null)
                    northWest._Reshape(new RectangleF(bounds.X, bounds.Y, halfDx, halfDz), invalids);
                if (northEast != null)
                    northEast._Reshape(new RectangleF(bounds.X + halfDx, bounds.Y, halfDx, halfDz), invalids);
                if (southWest != null)
                    southWest._Reshape(new RectangleF(bounds.X, bounds.Y - halfDz, halfDx, halfDz), invalids);
                if (southEast != null)
                    southEast._Reshape(new RectangleF(bounds.X + halfDx, bounds.Y - halfDz, halfDx, halfDz), invalids);
            }
        }

        public override string ToString()
        {
            return "L" + level + ":X" + bounds.Left + "Y" + bounds.Top;

        }

        public bool Contains(T item)
        {           
            if (item.BoundingSphere.IsInsideRect(bounds))
            {
                if (objects!=null)
                {
                    foreach (var x in objects)
                    {
                        if (x.Value.Equals(item))
                            return true;
                    }                  
                }
                return (northWest != null && northWest.Contains(item)) ||
                       (northEast != null && northEast.Contains(item)) ||
                       (southWest != null && southWest.Contains(item)) ||
                       (southEast != null && southEast.Contains(item));
            }
            return false;
        }

        public bool Remove(T item)
        {         
            if (nbElements == 0)
                return false;

            if (!item.BoundingSphere.IsInsideRect(bounds))
                return false;

            bool result = false;
            if (objects != null)
            {
                var current = objects.First;
                while (current != null)
                {
                    if (current.Value.Value.Equals(item))
                        break;
                    current = current.Next;
                }
                if (current != null)
                {
                    objects.Remove(current);
                    result = true;
                }
            }
            if (northWest != null)
                result |= northWest.Remove(item);
            if (northEast != null)
                result |= northEast.Remove(item);
            if (southEast != null)
                result |= southEast.Remove(item);
            if (southWest != null)
                result |= southWest.Remove(item);
            return result;
        }

        //public IEnumerable<ValueStore> EnumerateObjects()
        //{
        //    if (nbElements == 0)
        //        yield break;
        //    if (objects != null)
        //    {
        //        foreach (var item in objects)
        //        {
        //            if (!item.IsCulled)
        //            {
        //                item.IsCulled = true;
        //                yield return item;

        //            }
        //        }
        //    }

        //    if (northWest != null && northWest.nbElements > 0)
        //    {
        //        foreach (var item in northWest)
        //        {
                    
        //        }
        //    }
        //    if (northEast != null && northEast.nbElements > 0)
        //        northEast._AddItems(collection);
        //    if (southWest != null && southWest.nbElements > 0)
        //        southWest._AddItems(collection);
        //    if (southEast != null && southEast.nbElements > 0)
        //        southEast._AddItems(collection);
        //}
    }
}
