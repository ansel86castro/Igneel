using System;
using System.Collections.Generic;
using Igneel.SceneComponents;

namespace Igneel.SceneManagement
{
  
    public class QuadTree<T> : ICuller<T> ,IDeferreable
        where T : ICullable
    {
        private int _maxLevel;
        private HashSet<T> _items = new HashSet<T>();        
        private Node _root;
        private RectangleF _bounds;

        public QuadTree(RectangleF bounds, int maxlevel = 10)
        {
            this._bounds = bounds;
            this._maxLevel = maxlevel;
        }

        public int MaxLevel
        {
            get { return _maxLevel; }
            set
            {

                _maxLevel = value;
            }
        }

        public RectangleF Bounds
        {
            get { return _bounds; }
            set
            {
                _bounds = value;
            }
        }

        public void CommitChanges()
        {
            _Resize();
        }

        public void Resize(RectangleF bounds, int maxLevel)
        {
            _bounds = bounds;
            _maxLevel = maxLevel;
            _Resize();
        }

        private void _Resize()
        {
            if (_root == null)
                return;

            HashSet<T> collection = new HashSet<T>();
            _root.GetItems(collection);

            _root = new Node(_bounds, 0, null, this);
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        //public bool ComputeDimensions(Scene scene, IBoundable boundable)
        //{
        //    var sphere = boundable.BoundingSphere;
        //    if (sphere.Radius == 0)
        //        return false;

        //    if (_root == null || _root.Bounds.TestInside(sphere) != InsideTestResult.Inside)
        //    {
        //        float width = sphere.Radius * 2f;
        //        float height = sphere.Radius * 2f;
        //        float x = sphere.Center.X - sphere.Radius;
        //        float y = sphere.Center.Z + sphere.Radius;
        //        RectangleF bounds = new RectangleF(x, y, width, height);
        //        if (_root != null)
        //        {
        //            bounds = bounds.Combine(_root.Bounds);
        //        }
        //        List<Node<T>.ValueStore> objects = null;
        //        //get objects
        //        if (_root != null)
        //        {
        //            objects = new List<Node<T>.ValueStore>(_root.NbElements);
        //            _root.AddItems(objects);
        //        }

        //        _root = new Node<T>(bounds, 0, null, this);
        //        //reallocate objects               
        //        if (objects != null)
        //        {
        //            foreach (var item in objects)
        //            {
        //                item.IsCulled = false;
        //                _root._Add(item);
        //            }
        //        }

        //        return true;
        //    }
        //    return false;
        //}

        public bool Add(T item)
        {
            var region = new CullRegion(this);
            if (Add(item, region))
            {
                item.CullRegion = region;
                return true;
            }
            return false;
        }

        public bool Add(T item, CullRegion region)
        {
            if (_root == null)
            {
                _root = new Node(_bounds, 0, null, this);
            }
            bool isContained;
            return _root.Add(item, region, out isContained) > 0;
        }

        public bool Contains(T item)
        {
            if (_root == null)
                return false;
            return _root.Contains(item);
        }

        public bool Remove(T item)
        {
            if (_root == null) 
                return false;
            return _root.Remove(item);
        }

        public void GetVisibleObjects(Camera camera, ICollection<T> collection)
        {
            if (_root == null)
                return;

            _items.Clear();
            _root.CullItems(camera, _items);
         
            foreach (var item in _items)
            {
                collection.Add(item);
            }
        }

        public HashSet<T> CullItems(Camera camera)
        {
            _items.Clear();

            if (_root == null)
                return _items;

            _root.CullItems(camera, _items);
            return _items;
        }

        public void Clear()
        {
            _root.Clear();
        }



        public class Node
        {            
            QuadTree<T> _quadTree;
            private RectangleF _bounds;

            /*
                *************
                *  NW *  NE *
                *************
                *  SW *  SE *
                *************
            */
            private Node _northEast = null;
            private Node _northWest = null;
            private Node _southWest = null;
            private Node _southEast = null;
            private Node _parent = null;

            private int _level;
            private Sphere _boundSphere;
            private LinkedList<T> _items;
            private int _nbElements;

            public Node(RectangleF bounds, int level, Node parent, QuadTree<T> quadTree)
            {
                if (level < 0)
                    throw new ArgumentOutOfRangeException();

                if (!(bounds.Width > Numerics.Epsilon && bounds.Height > Numerics.Epsilon))
                    throw new ArgumentOutOfRangeException();

                this._level = level;
                this._bounds = bounds;
                this._quadTree = quadTree;
                this._parent = parent;

                float halfDx = bounds.Width * 0.5f;
                float halfDz = bounds.Height * 0.5f;

                _boundSphere.Center = new Vector3(bounds.X + halfDx, 0, bounds.Y - halfDz);
                _boundSphere.Radius = 0.5f * (float)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height);
            }

            public bool IsLeaft
            {
                get
                {
                    return _northEast == null &&
                        _northWest == null &&
                        _southEast == null &&
                        _southWest == null;
                }
            }

            public RectangleF Bounds { get { return _bounds; } }

            public int Level { get { return _level; } }

            public int NbElements { get { return _nbElements; } }

            public LinkedList<T> Items { get { return _items; } }

            public bool CanSubDivide
            {
                get
                {
                    return _level < _quadTree._maxLevel;
                }
            }

            public void CullItems(Camera camera, HashSet<T> collection)
            {
                if (_nbElements == 0)
                    return;

                var check = camera.TestFrustum(_boundSphere);

                switch (check)
                {
                    case FrustumTest.Inside:
                        GetItems(camera, collection, false);
                        break;
                    case FrustumTest.Partial:
                        if (_items != null)
                        {                            
                            foreach (var item in _items)
                            {
                                if (!collection.Contains(item) && camera.Contains(item.BoundingSphere))
                                {
                                    collection.Add(item);
                                }
                            }
                        }

                        if (_northWest != null && _northWest._nbElements > 0)
                            _northWest.CullItems(camera, collection);
                        if (_northEast != null && _northEast._nbElements > 0)
                            _northEast.CullItems(camera, collection);
                        if (_southWest != null && _southWest._nbElements > 0)
                            _southWest.CullItems(camera, collection);
                        if (_southEast != null && _southEast._nbElements > 0)
                            _southEast.CullItems(camera, collection);

                        break;
                }

            }

            private void GetItems(Camera camera, HashSet<T> collection, bool partial)
            {
                if (_nbElements == 0)
                    return;

                if (_items != null)
                {
                    foreach (var item in _items)
                    {
                        if (!collection.Contains(item))
                        {
                            if (!partial || camera.Contains(item.BoundingSphere))
                                collection.Add(item);
                        }
                    }
                }

                if (_northWest != null )
                    _northWest.GetItems(camera, collection, partial);
                if (_northEast != null )
                    _northEast.GetItems(camera, collection, partial);
                if (_southWest != null )
                    _southWest.GetItems(camera, collection, partial);
                if (_southEast != null )
                    _southEast.GetItems(camera, collection, partial);
            }

            public void GetItems(HashSet<T> collection)
            {
                if (_nbElements == 0)
                    return;

                if (_items != null)
                {
                    foreach (var item in _items)
                    {
                        collection.Add(item);
                    }
                }

                if (_northWest != null )
                    _northWest.GetItems(collection);
                if (_northEast != null )
                    _northEast.GetItems(collection);
                if (_southWest != null )
                    _southWest.GetItems(collection);
                if (_southEast != null )
                    _southEast.GetItems(collection);
            }

            //public void AddItems(List<ValueStore> collection)
            //{
            //    if (_nbElements == 0)
            //        return;
            //    if (_items != null)
            //    {
            //        foreach (var item in _items)
            //        {
            //            if (!item.IsCulled)
            //            {
            //                item.IsCulled = true;
            //                collection.Add(item);

            //            }
            //        }
            //    }

            //    if (_northWest != null && _northWest._nbElements > 0)
            //        _northWest.AddItems(collection);
            //    if (_northEast != null && _northEast._nbElements > 0)
            //        _northEast.AddItems(collection);
            //    if (_southWest != null && _southWest._nbElements > 0)
            //        _southWest.AddItems(collection);
            //    if (_southEast != null && _southEast._nbElements > 0)
            //        _southEast.AddItems(collection);
            //}

            public int Add(T item, CullRegion region, out bool isContained)
            {
                isContained = false;
                var test = _bounds.TestInside(item.BoundingSphere);

                if (test == InsideTestResult.Inside || test == InsideTestResult.PartialInside)
                {
                    if (CanSubDivide)
                    {
                        float halfDx = _bounds.Width * 0.5f;
                        float halfDz = _bounds.Height * 0.5f;

                        var northWestRec = new RectangleF(_bounds.X, _bounds.Y, halfDx, halfDz);
                        var northEastRec = new RectangleF(_bounds.X + halfDx, _bounds.Y, halfDx, halfDz);
                        var southWestRec = new RectangleF(_bounds.X, _bounds.Y - halfDz, halfDx, halfDz);
                        var southEastRec = new RectangleF(_bounds.X + halfDx, _bounds.Y - halfDz, halfDx, halfDz);

                        bool b1, b2, b3, b4;
                        int regions = _AddOrCreate(item, northWestRec, ref _northWest, region, out b1);
                        regions += _AddOrCreate(item, northEastRec, ref _northEast, region, out b2);
                        regions += _AddOrCreate(item, southWestRec, ref _southWest, region, out b3);
                        regions += _AddOrCreate(item, southEastRec, ref _southEast, region, out b4);

                        if (regions == 0 || b1 || b2 || b3 || b4)
                        {
                            _AddItem(item, region);
                            return regions + 1;
                        }
                        ++_nbElements;
                        return regions;
                    }
                    else
                    {
                        _AddItem(item, region);
                        return 1;
                    }
                }
                else if (test == InsideTestResult.Contained || test == InsideTestResult.PartialContained)
                {
                    isContained = true;
                }
                return 0;
            }

            private void _AddItem(T item, CullRegion region)
            {
                if (_items == null)
                    _items = new LinkedList<T>();

                _items.AddLast(item);
                region.Add(this);
                ++_nbElements;
            }

            private int _AddOrCreate(T item, RectangleF quadBound, ref Node node, CullRegion region, out  bool isContained)
            {
                if (node != null)
                {
                    return node.Add(item, region, out isContained);
                }
                else
                {
                    if (item.BoundingSphere.Intersect(quadBound))
                    {
                        node = new Node(quadBound, _level + 1, this, _quadTree);
                        return node.Add(item, region, out isContained);
                    }
                    isContained = false;
                    return 0;
                }
            }         

            public void Clear()
            {
                if (_items != null)
                {
                    foreach (var item in _items)
                    {
                        item.CullRegion = null;
                    }
                    _items.Clear();
                }

                if (_northEast != null)
                    _northEast.Clear();
                if (_northWest != null)
                    _northWest.Clear();
                if (_southEast != null)
                    _southEast.Clear();
                if (_southWest != null)
                    _southWest.Clear();

            }

            //public void Resize(RectangleF bounds)
            //{
            //    var invalids = new List<T>();

            //    _Rezise(bounds, invalids);

            //    foreach (var item in invalids)
            //    {
            //        _ReAlloc(item);
            //    }
            //}         

            //private void _Rezise(RectangleF bounds, List<T> invalids)
            //{
            //    this._bounds = bounds;
            //    float halfDx = bounds.Width * 0.5f;
            //    float halfDz = bounds.Height * 0.5f;
            //    _boundSphere.Center = new Vector3(bounds.X + halfDx, 0, bounds.Y - halfDz);
            //    _boundSphere.Radius = (float)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height) * 0.5f;

            //    if (IsLeaft)
            //    {
            //        foreach (var item in _items)
            //        {
            //            if (item.BoundingSphere.Intersect(bounds) && !item.IsCulled)
            //            {
            //                item.IsCulled = true;
            //                invalids.Add(item);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (_northWest != null)
            //            _northWest._Rezise(new RectangleF(bounds.X, bounds.Y, halfDx, halfDz), invalids);
            //        if (_northEast != null)
            //            _northEast._Rezise(new RectangleF(bounds.X + halfDx, bounds.Y, halfDx, halfDz), invalids);
            //        if (_southWest != null)
            //            _southWest._Rezise(new RectangleF(bounds.X, bounds.Y - halfDz, halfDx, halfDz), invalids);
            //        if (_southEast != null)
            //            _southEast._Rezise(new RectangleF(bounds.X + halfDx, bounds.Y - halfDz, halfDx, halfDz), invalids);
            //    }
            //}

            public override string ToString()
            {
                return "L" + _level + ":X" + _bounds.Left + "Y" + _bounds.Top;

            }

            public bool Contains(T item)
            {
                if (item.BoundingSphere.Intersect(_bounds))
                {
                    if (_items != null)
                    {
                        foreach (var x in _items)
                        {
                            if (x.Equals(item))
                                return true;
                        }
                    }
                    return (_northWest != null && _northWest.Contains(item)) ||
                           (_northEast != null && _northEast.Contains(item)) ||
                           (_southWest != null && _southWest.Contains(item)) ||
                           (_southEast != null && _southEast.Contains(item));
                }
                return false;
            }

            public bool RemoveItem(T item)
            {
                if (_items == null)
                    return false;

                if (_items.Remove(item))
                {
                    _nbElements--;
                    return true;
                }
                return false;
            }

            public bool Remove(T item)
            {
                if (_nbElements == 0)
                    return false;

                if (!item.BoundingSphere.Intersect(_bounds))
                    return false;

                bool result = false;
                if (_items != null)
                {
                    var current = _items.First;
                    while (current != null)
                    {
                        if (current.Value.Equals(item))
                            break;
                        current = current.Next;
                    }
                    if (current != null)
                    {
                        CullRegion region = item.CullRegion as CullRegion;
                        if (region != null)
                        {
                            region.Remove(this);
                        }
                        _items.Remove(current);
                        _nbElements--;
                        result = true;
                    }
                }
                if (_northWest != null)
                    result |= _northWest.Remove(item);
                if (_northEast != null)
                    result |= _northEast.Remove(item);
                if (_southEast != null)
                    result |= _southEast.Remove(item);
                if (_southWest != null)
                    result |= _southWest.Remove(item);
                return result;
            }

        }


        public class CullRegion : ICullRegion
        {
            LinkedList<Node> nodes;
            QuadTree<T> quadTree;

            public CullRegion(QuadTree<T> quadTree)
            {
                this.quadTree = quadTree;
                nodes = new LinkedList<Node>();
            }

            public void Add(Node node)
            {
                nodes.AddLast(node);
            }

            public void Remove(Node node)
            {
                nodes.Remove(node);
            }

            public void Update(T item)
            {
                Remove(item);
                Add(item);
            }

            public bool Add(T item)
            {
              return  quadTree.Add(item ,this);
            }

            public bool Remove(T item)
            {
                bool r = false;
                foreach (var node in nodes)
                {
                    if (node.RemoveItem(item))
                        r = true;
                }
                nodes.Clear();
                return r;
            }

            void ICullRegion.Update(ICullable item)
            {
                Update((T)item);
            }

            bool ICullRegion.Add(ICullable item)
            {
                return Add((T)item);
            }

            bool ICullRegion.Remove(ICullable item)
            {
                return Remove((T)item);
            }
        }

       
    }   
}
