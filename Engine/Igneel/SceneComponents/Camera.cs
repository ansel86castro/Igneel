using System.ComponentModel;
using Igneel.Assets;
using Igneel.SceneManagement;

namespace Igneel.SceneComponents
{
    public enum ProjectionType { Perspective, Orthographic }

    [Asset("CAMERA")]
    //[ResourceActivator(typeof(Camera.Activator))]
    public class Camera : FrameComponent, IDeferreable, IBoundable
    {       
        private Vector3 _position;        
        private Vector3 _right = new Vector3(1,0,0);
        private Vector3 _up = new Vector3(0, 1, 0);
        private Vector3 _front = new Vector3(0, 0, 1);        
        private float _zn;
        private float _zf;
        private float _fov = Numerics.PIover3;
        private float _aspectRatio = 4 / 3;
        private float _width = 512;
        private float _height = 512;
        private ProjectionType _projectionType = ProjectionType.Perspective;
        private Matrix _local = Matrix.Identity;
        private Matrix _view = Matrix.Identity;
        private Matrix _proj;
        private Matrix _viewProj = Matrix.Identity;
        private Matrix _invViewProj = Matrix.Identity;
        private Frustum _frustum = new Frustum();
        private bool _enableCulling = true;
        private bool _isSync;

        public Camera(string name = null, float zn = 1f, float zf = 1000f)                
        {
            this._zn = zn;
            this._zf = zf;
            Name = name;                    

            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }      

        public Camera(string name, Vector3 position, Vector3 right, Vector3 up, Vector3 front, float zn =1f , float zf = 1000f)
            :this(name, zn, zf)
        {
            this._position = position;
            this._right = right;
            this._up = up;
            this._front = front;

            BoundingSphere = new Sphere(Vector3.Zero, zn);        

            _view = Matrix.View(right, up, front, position);
            _UpdateProjection();         
                    
        }


        public Sphere BoundingSphere
        {
            get;
            private set;
        }

        public OrientedBox BoundingBox
        {
            get { return null; }
        }

        public static Camera FromLookAt(string name , Vector3 position, Vector3 lookAt, Vector3 up, float zn, float zf)
        {
            Vector3 front , right;
            var m = Matrix.LookAt(position, lookAt, up, out right, out front, out up);
            return new Camera(name, position, right, up, front, zn, zf);
        }

        public static Camera FromOrientation(string name, Vector3 position = default(Vector3), Euler orientation = default(Euler), float zn = 1f, float zf = 1000f)
        {
            Vector3 front , up,right;
            Euler.GetFrame(orientation, out right, out up, out front);
            return new Camera(name, position, right, up, front, zn, zf);
        }       

        public Camera SetPerspective(float fov, float aspectRatio)
        {
            this._fov = fov;
            this._aspectRatio = aspectRatio;
            _projectionType = ProjectionType.Perspective;

            _UpdateProjection();                    

            return this;
        }

        public Camera SetOrthographic(float width, float height)
        {
            this._width = width;
            this._height = height;
            _projectionType = ProjectionType.Orthographic;
            _UpdateProjection();
            CommitChanges();

            return this;
        }

        [AssetMember]
        public Matrix LocalFrame { get { return _local; } set { _local = value; } }
                       
        [AssetMember]
        public Matrix View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value;
                Matrix t;
                Matrix.Transpose(ref value, out t);

                _right = t.Right;
                _up = t.Up;
                _front = t.Front;
                _position = -_view.M41 * _right - _view.M42 * _up - _view.M43 * _front;               
            }
        }
       
        [AssetMember]
        public Matrix Projection 
        {
            get { return _proj; }
            set
            {
                _proj = value;                
            }
        }

               
        public Matrix ViewProj
        {
            get
            {              
                return _viewProj;
            }
        }

       
        public Matrix InvViewProjection { get { return _invViewProj; } }

       
        public Vector3 Right
        {
            get { return _right; }
            set
            {
                _right = value;
                _view.M11 = _right.X;
                _view.M21 = _right.Y;
                _view.M31 = _right.Z;
            }
        }

       
        public Vector3 Front
        { 
            get { return _front; } 
            set
            { 
                _front = value;
                _view.M13 = _front.X;
                _view.M23 = _front.Y;
                _view.M33 = _front.Z;
            } 
        }

       
        public Vector3 Up 
        {
            get { return _up; } 
            set 
            { 
                _up = value;
                _view.M12 = _up.X;
                _view.M22 = _up.Y;
                _view.M32 = _up.Z;            
            } 
        }

        
        
        [AssetMember]
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                // Fill in the view matrix entries.            
                _view.M41 = -Vector3.Dot(_position, _right);
                _view.M42 = -Vector3.Dot(_position, _up);
                _view.M43 = -Vector3.Dot(_position, _front);
                _view.M44 = 1f;               
            }
        }

        
        
        [AssetMember]
        public float ZNear 
        { 
            get { return _zn; } 
            set 
            { 
                _zn = value;
                _UpdateProjection();
            } 
        }

        [AssetMember]
        
        
        public float ZFar 
         { 
             get { return _zf; } set 
             { 
                 _zf = value;
                 var sphere = BoundingSphere;
                 sphere.Radius = _zf;
                 BoundingSphere = sphere;

                 _UpdateProjection();
             } 
         }

       
        public Frustum ViewFrustum { get { return _frustum; } }        

        
        [Category("Camera")]
        [AssetMember]
        public ProjectionType Type
        {
            get { return _projectionType; }
            set 
            { 
                _projectionType = value;
                _UpdateProjection();
            }
        }

        
        [Category("Camera")]
        [AssetMember]
        public float FieldOfView
        {
            get
            {
                return _fov;
            }
            set
            {
                _fov = value;
                if (_projectionType == ProjectionType.Perspective)
                    _UpdateProjection();
            }
        }

        
        [Category("Camera")]
        [AssetMember]
        public float AspectRatio
        {
            get
            {
                return _aspectRatio;
            }
            set
            {
                _aspectRatio = value;
                if (_projectionType == ProjectionType.Perspective)
                    _UpdateProjection();
            }
        }

        
        [Category("Camera")]
        [AssetMember]
        public float OrthoWidth
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                if (_projectionType == ProjectionType.Orthographic)
                    _UpdateProjection();
            }
        }

        
        [Category("Camera")]
        [AssetMember]
        public float OrthoHeight
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                if (_projectionType == ProjectionType.Orthographic)
                    _UpdateProjection();
            }
        }      

        public bool IsGpuSync { get { return _isSync; } set { _isSync = value; } }

        public bool EnableCulling { get { return _enableCulling; } set { _enableCulling = value; } }     

        public float Distance(IBoundable obj)
        {
            var plane = _frustum.Planes[3]; //front plane          
            var box = obj.BoundingBox;
            if (box != null)            
                return Plane.DotCoordinate(plane, box.GlobalTraslation) + _zn;            
            else           
                return Plane.DotCoordinate(plane, obj.BoundingSphere.Center) + _zn;            
        }             

        public void Transform(Matrix transform)
        {
            transform = _local * transform;

            _right = new Vector3(transform.M11, transform.M12, transform.M13);
            _right.Normalize();

            _up = new Vector3(transform.M21, transform.M22, transform.M23);
            _up.Normalize();

            _front = new Vector3(transform.M31, transform.M32, transform.M33);
            _front.Normalize();

            _position = new Vector3(transform.M41, transform.M42, transform.M43);

            //view = Matrix.Invert(m);          
            // Fill in the view matrix entries.            
            float x = -Vector3.Dot(_position, _right);
            float y = -Vector3.Dot(_position, _up);
            float z = -Vector3.Dot(_position, _front);           

            _view.M11 = _right.X;
            _view.M21 = _right.Y;
            _view.M31 = _right.Z;
            _view.M41 = x;

            _view.M12 = _up.X;
            _view.M22 = _up.Y;
            _view.M32 = _up.Z;
            _view.M42 = y;

            _view.M13 = _front.X;
            _view.M23 = _front.Y;
            _view.M33 = _front.Z;
            _view.M43 = z;

            _view.M14 = 0f;
            _view.M24 = 0f;
            _view.M34 = 0f;
            _view.M44 = 1f;          

            CommitChanges();
        }        

        public override void OnNodeAttach(Frame node)
        {            
            Transform(node.GlobalPose);
            base.OnNodeAttach(node);
        }        

        public override void OnPoseUpdated(Frame node)
        {          
            Transform(node.GlobalPose);            
        }

        public override void OnSceneAttach(Scene scene)
        {
            if (Name != null)
            {

                scene.Cameras.Add(this);
                if (scene.ActiveCamera == null)
                    scene.ActiveCamera = this;
            }
            base.OnSceneAttach(scene);
        }

        public override void OnSceneDetach(Scene scene)
        {
            if (scene != null && Name != null)
            {
                if (scene.Cameras.Remove(this) && scene.ActiveCamera == this)
                    Engine.Scene.ActiveCamera = null;
            }

            base.OnSceneDetach(scene);
        }       

        public void CommitChanges()
        {
            _isSync = false;
            _viewProj = _view * _proj;
            _invViewProj = Matrix.Invert(_viewProj);
            _frustum.Transform(_invViewProj);
        }

        //private void _AddToScene( )
        //{
        //    if (name != null)
        //    {
        //        Engine.Lock();
        //        try
        //        {
        //            Scene.Cameras.Add(this);
        //            if (Scene.ActiveCamera == null)
        //                Scene.ActiveCamera = this;
        //        }
        //        finally
        //        {
        //            Engine.Unlock();
        //        }
        //    }
        //}

        //private bool _RemoveFromScene()
        //{
        //    bool result = false;
        //    if (Scene != null && name != null)
        //    {
        //        result = Scene.Cameras.Remove(this);
        //        if (result && Scene.ActiveCamera == this)
        //            Scene.ActiveCamera = null;
        //    }
        //    return result;
        //}

        private void _UpdateProjection()
        {
            switch (_projectionType)
            {
                case ProjectionType.Orthographic:
                    _proj = Matrix.OrthoLh(_width, _height, _zn, _zf);
                    break;
                case ProjectionType.Perspective:
                    _proj = Matrix.PerspectiveFovLh(_aspectRatio, _fov, _zn, _zf);
                    break;
            }
            CommitChanges();
        }

        public FrustumTest TestFrustum(Sphere sphere)
        {
            return _frustum.TestFrustum(sphere);
        }

        public bool Contains(Sphere sphere)
        {
            return _frustum.Contains(sphere);
        }

        public bool Contains(Vector3 center, float radius)
        {
            return _frustum.Contains(center, radius);
        }

        //[Serializable]
        //class Activator:IResourceActivator
        //{
        //    string _name;          
        //    public void Initialize(IAssetProvider provider)
        //    {
        //        var cam = (Camera)provider;
        //        _name = cam.Name;
        //    }

        //    public IAssetProvider OnCreateResource()
        //    {
        //        return new Camera(_name);
        //    }           
        //}



    }           
}

