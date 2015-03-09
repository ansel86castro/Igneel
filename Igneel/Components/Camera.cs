using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.ComponentModel;



using Igneel.Assets;
using Igneel.Services;

namespace Igneel.Components
{
    public enum ProjectionType { Perspective, Orthographic }

    
    [ProviderActivator(typeof(Camera.Activator))]
    public class Camera : ExclusiveNodeObject, INameable, IDeferreable, IShadingInput
    {
        private string name;
        private Vector3 position;        
        private Vector3 right = new Vector3(1,0,0);
        private Vector3 up = new Vector3(0, 1, 0);
        private Vector3 front = new Vector3(0, 0, 1);        
        private float zn;
        private float zf;
        private float fov = Numerics.PIover3;
        private float aspectRatio = 4 / 3;
        private float width = 512;
        private float height = 512;
        private ProjectionType projectionType = ProjectionType.Perspective;
        private Matrix local = Matrix.Identity;
        private Matrix view = Matrix.Identity;
        private Matrix proj;
        private Matrix viewProj = Matrix.Identity;
        private Matrix invViewProj = Matrix.Identity;
        private Frustum frustum = new Frustum();
        private bool enableCulling = true;
        private bool isSync;

        public Camera(string name = null, float zn = 1f, float zf = 1000f)            
        {
            this.zn = zn;
            this.zf = zf;
            this.name = name;                    

            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }      

        public Camera(string name, Vector3 position, Vector3 right, Vector3 up, Vector3 front, float zn =1f , float zf = 1000f)
            :this(name, zn, zf)
        {
            this.position = position;
            this.right = right;
            this.up = up;
            this.front = front;

            sphere.Center = new Vector3();
            sphere.Radius = zn;

            view = Matrix.View(right, up, front, position);
            _UpdateProjection();         
                    
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
            this.fov = fov;
            this.aspectRatio = aspectRatio;
            projectionType = ProjectionType.Perspective;

            _UpdateProjection();                    

            return this;
        }

        public Camera SetOrthographic(float width, float height)
        {
            this.width = width;
            this.height = height;
            projectionType = ProjectionType.Orthographic;
            _UpdateProjection();
            CommitChanges();

            return this;
        }

        [AssetMember]
        public Matrix LocalFrame { get { return local; } set { local = value; } }

        //[Category("Camera")]
        //[AssetMember]
        //public bool Active
        //{
        //    get
        //    {
        //        return Scene != null && Scene.ActiveCamera == this;
        //    }
        //    set
        //    {
        //        if (Scene != null)
        //            Scene.ActiveCamera = this;
        //    }
        //}
         
       
        [AssetMember]
        public Matrix View
        {
            get
            {
                return view;
            }
            set
            {
                view = value;
                Matrix t;
                Matrix.Transpose(ref value, out t);

                right = t.Right;
                up = t.Up;
                front = t.Front;
                position = -view.M41 * right - view.M42 * up - view.M43 * front;               
            }
        }

       
        [AssetMember]
        public Matrix Projection 
        {
            get { return proj; }
            set
            {
                proj = value;                
            }
        }

               
        public Matrix ViewProj
        {
            get
            {              
                return viewProj;
            }
        }

       
        public Matrix InvViewProjection { get { return invViewProj; } }

       
        public Vector3 Right
        {
            get { return right; }
            set
            {
                right = value;
                view.M11 = right.X;
                view.M21 = right.Y;
                view.M31 = right.Z;
            }
        }

       
        public Vector3 Front
        { 
            get { return front; } 
            set
            { 
                front = value;
                view.M13 = front.X;
                view.M23 = front.Y;
                view.M33 = front.Z;
            } 
        }

       
        public Vector3 Up 
        {
            get { return up; } 
            set 
            { 
                up = value;
                view.M12 = up.X;
                view.M22 = up.Y;
                view.M32 = up.Z;            
            } 
        }

        
        
        [AssetMember]
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;

                // Fill in the view matrix entries.            
                view.M41 = -Vector3.Dot(position, right);
                view.M42 = -Vector3.Dot(position, up);
                view.M43 = -Vector3.Dot(position, front);
                view.M44 = 1f;               
            }
        }

        
        
        [AssetMember]
        public float ZNear 
        { 
            get { return zn; } 
            set 
            { 
                zn = value;
                _UpdateProjection();
            } 
        }

        [AssetMember]
        
        
        public float ZFar 
         { 
             get { return zf; } set 
             { 
                 zf = value; sphere.Radius = value;
                 _UpdateProjection();
             } 
         }

       
        public Frustum ViewFrustum { get { return frustum; } }        

        
        [Category("Camera")]
        [AssetMember]
        public ProjectionType Type
        {
            get { return projectionType; }
            set 
            { 
                projectionType = value;
                _UpdateProjection();
            }
        }

        
        [Category("Camera")]
        [AssetMember]
        public float FieldOfView
        {
            get
            {
                return fov;
            }
            set
            {
                fov = value;
                if (projectionType == ProjectionType.Perspective)
                    _UpdateProjection();
            }
        }

        
        [Category("Camera")]
        [AssetMember]
        public float AspectRatio
        {
            get
            {
                return aspectRatio;
            }
            set
            {
                aspectRatio = value;
                if (projectionType == ProjectionType.Perspective)
                    _UpdateProjection();
            }
        }

        
        [Category("Camera")]
        [AssetMember]
        public float OrthoWidth
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                if (projectionType == ProjectionType.Orthographic)
                    _UpdateProjection();
            }
        }

        
        [Category("Camera")]
        [AssetMember]
        public float OrthoHeight
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                if (projectionType == ProjectionType.Orthographic)
                    _UpdateProjection();
            }
        }

        [AssetMember]
        public string Name
        {
            get
            {
                return name;
            }
            //set
            //{
            //    if (name != value && Scene != null)
            //    {
            //        var scene = Scene;
            //        OnRemoveFromScene(scene);
            //        name = value;
            //        OnAddToScene(scene);                    
            //    }
            //    name = value;
            //}
        }

        public bool IsGPUSync { get { return isSync; } set { isSync = value; } }

        public bool EnableCulling { get { return enableCulling; } set { enableCulling = value; } }     

        public float GetDistanceTo(IBoundable obj)
        {
            var plane = frustum.planes[3]; //front plane          
            var box = obj.BoundingBox;
            if (box != null)            
                return Plane.DotCoordinate(plane, box.GlobalTraslation) + zn;            
            else           
                return Plane.DotCoordinate(plane, obj.BoundingSphere.Center) + zn;            
        }             

        public void Transform(Matrix m)
        {
            m = local * m;

            right = new Vector3(m.M11, m.M12, m.M13);
            right.Normalize();

            up = new Vector3(m.M21, m.M22, m.M23);
            up.Normalize();

            front = new Vector3(m.M31, m.M32, m.M33);
            up.Normalize();

            position = new Vector3(m.M41, m.M42, m.M43);

            //view = Matrix.Invert(m);          
            // Fill in the view matrix entries.            
            float x = -Vector3.Dot(position, right);
            float y = -Vector3.Dot(position, up);
            float z = -Vector3.Dot(position, front);           

            view.M11 = right.X;
            view.M21 = right.Y;
            view.M31 = right.Z;
            view.M41 = x;

            view.M12 = up.X;
            view.M22 = up.Y;
            view.M32 = up.Z;
            view.M42 = y;

            view.M13 = front.X;
            view.M23 = front.Y;
            view.M33 = front.Z;
            view.M43 = z;

            view.M14 = 0f;
            view.M24 = 0f;
            view.M34 = 0f;
            view.M44 = 1f;          

            CommitChanges();
        }        

        public override void OnNodeAttach(SceneNode node)
        {            
            Transform(node.GlobalPose);
            base.OnNodeAttach(node);
        }        

        public override void OnPoseUpdated(SceneNode node)
        {          
            Transform(node.GlobalPose);            
        }      

        public override void OnAddToScene(Scene scene)
        {
            if (name != null)
            {
                Engine.Lock();
                try
                {
                    scene.Cameras.Add(this);
                    if (scene.ActiveCamera == null)
                        scene.ActiveCamera = this;
                }
                finally
                {
                    Engine.Unlock();
                }
            }
            base.OnAddToScene(scene);
        }

        public override void OnRemoveFromScene(Scene scene)
        {            
            if (scene != null && name != null)
            {
                if (scene.Cameras.Remove(this) && scene.ActiveCamera == this)
                    Engine.Scene.ActiveCamera = null;
            }

            base.OnRemoveFromScene(scene);
        }       

        public void CommitChanges()
        {
            isSync = false;
            viewProj = view * proj;
            invViewProj = Matrix.Invert(viewProj);
            frustum.Transform(invViewProj);
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
            switch (projectionType)
            {
                case ProjectionType.Orthographic:
                    proj = Matrix.OrthoLH(width, height, zn, zf);
                    break;
                case ProjectionType.Perspective:
                    proj = Matrix.PerspectiveFovLH(fov, aspectRatio, zn, zf);
                    break;
            }
            CommitChanges();
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }

        [Serializable]
        class Activator:IProviderActivator
        {
            string name;          
            public void Initialize(IAssetProvider provider)
            {
                var cam = (Camera)provider;
                name = cam.Name;
            }

            public IAssetProvider CreateInstance()
            {
                return new Camera(name);
            }           
        }

        
    }           
}

