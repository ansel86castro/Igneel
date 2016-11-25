using System;
using System.ComponentModel;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.States;

namespace Igneel.Techniques
{
    
    public abstract class CubeMapTechique :FrameTechnique, IDeferreable
    {
        protected Vector3 positionOffset;
        protected RenderTextureCube RenderTarget;
        protected int size;
        protected float Zn, Zf;
        protected Camera[] cameras;      
        protected bool isDynamic;
        protected Format format;       
        bool _rendered;
        ViewPort _vp;     
        public CubeMapTechique()
        {
            cameras = new Camera[6];           
        }      
        
        public CubeMapTechique(Vector3 position, int size, float zn, float zf, Format format)            
        {          
            this.size = size;
            this.Zn = zn;
            this.Zf = zf;
            this.format = format;

            cameras = new Camera[6];                               
        }

       
        public Texture2D Texture { get { return RenderTarget.Texture; } }        

        public Vector3 PositionOffset
        {
            get { return positionOffset; }
            set { positionOffset = value; }
        }
              
        
        
        public int Size
        {
            get { return size; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("value");
                size = value;
            }
        }

        
        
        public float NearClipPlane { get { return Zn; } set { if (value <= 0)throw new ArgumentOutOfRangeException("value"); Zn = value; } }

        
        
        public float FarClipPlane { get { return Zf; } set { if (value <= 0)throw new ArgumentOutOfRangeException("value"); Zf = value; } }
      
        public bool IsDynamic { get { return isDynamic; } set { isDynamic = value; } }

        [Browsable(true), ReadOnly(true)]
        
        
        public Format Format { get { return format; } set { if (value == Format.UNKNOWN) throw new ArgumentException("value"); format = value; } }

       
        public Camera[] Cameras { get { return cameras; } }

        public override void UpdatePose(Matrix affectorPose)
        {
            var pos = Vector3.Transform(positionOffset, GlobalToAffector * affectorPose);
            for (int i = 0; i < 6; i++)
            {
                cameras[i].Position = pos;
                cameras[i].CommitChanges();
            }            
        }
       
        public virtual void Initialize()
        {
            _rendered = false;
            Vector3 position = Vector3.Zero;
            cameras[0] = Camera.FromLookAt(null, Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Zn, Zf); //PositiveX
            cameras[1] = Camera.FromLookAt(null, Vector3.Zero, -Vector3.UnitX, Vector3.UnitY, Zn, Zf); //NegativeX

            cameras[2] = Camera.FromLookAt(null, position, Vector3.UnitY, -1 * Vector3.UnitZ, Zn, Zf); //PositiveY
            cameras[3] = Camera.FromLookAt(null, position, -Vector3.UnitY, Vector3.UnitZ, Zn, Zf); //NegativeY

            cameras[4] = Camera.FromLookAt(null, position, Vector3.UnitZ, Vector3.UnitY, Zn, Zf);  //PositiveZ
            cameras[5] = Camera.FromLookAt(null, position, -Vector3.UnitZ, Vector3.UnitY, Zn, Zf); //NegativeZ          

            for (int i = 0; i < 6; i++)
            {
                cameras[i].AspectRatio = 1;
                cameras[i].FieldOfView = Numerics.PIover2;
                cameras[i].CommitChanges();
            }

            if (format == Format.UNKNOWN) throw new InvalidOperationException("Invalid format");
            if (size <= 0) throw new InvalidOperationException("size less than 0");

            if (RenderTarget != null)
                RenderTarget.Dispose();
            RenderTarget = new RenderTextureCube(size, format, GraphicDeviceFactory.Device.BackDepthBuffer.SurfaceFormat);
            _rendered = false;
            _vp = new ViewPort(0, 0, size, size);
        }
               
        public override void Apply()
        {
            if (!isDynamic && this._rendered) return;          

            var device = GraphicDeviceFactory.Device;
            device.SaveRenderTarget();
            var oldvp = device.ViewPort;

            device.ViewPort = _vp;

            var scene = Engine.Scene;
            var oldCamera = scene.ActiveCamera;

            //foreach (var item in nodes)            
            //    item.Component.Visibl = false;

            bool restore = false;
            var useDefaulTechnique = EngineState.Lighting.Reflection.UseDefaultTechnique;
            if (useDefaulTechnique && !RenderManager.IsTechniqueActive<DefaultTechnique>())
            {
                RenderManager.PushTechnique<DefaultTechnique>();
                restore = true;
            }         

            for (int i = 0; i < 6; i++)
            {
                RenderTarget.SetTarget(i, device);                

                scene.ActiveCamera = cameras[i];

                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Engine.BackColor, 1, 0);

                RenderManager.ApplyTechnique();               
            }

            //foreach (var item in nodes)
            //    item.Visible = true;

            device.ViewPort = oldvp;
            device.RestoreRenderTarget();
            scene.ActiveCamera = oldCamera;

            if (restore)
            {
                RenderManager.PopTechnique();
            }

            this._rendered = true;
        }        
        
        public void UpdateCameras(Vector3 position , float zn ,float zf)
        {           
            this.Zn = zn;
            this.Zf = zf;
            for (int i = 0; i < 6; i++)
            {
                cameras[i].Position = position;
                cameras[i].ZNear = zn;
                cameras[i].ZFar = zf;
                cameras[i].CommitChanges();
            }
        }

        //protected CubeMapFace GetFace(int index)
        //{
        //    switch (index)
        //    {
        //        case 0: return CubeMapFace.PositiveX;
        //        case 1: return CubeMapFace.NegativeX;
        //        case 2: return CubeMapFace.PositiveY;
        //        case 3: return CubeMapFace.NegativeY;
        //        case 4: return CubeMapFace.PositiveZ;
        //        case 5: return CubeMapFace.NegativeZ;
        //    }

        //    throw new IndexOutOfRangeException();
        //}        

        public void CommitChanges()
        {
            if (Zn != cameras[0].ZNear || Zf != cameras[0].ZFar)
            {
                for (int i = 0; i < cameras.Length; i++)
                {
                    cameras[i].ZNear = Zn;
                    cameras[i].ZFar = Zf;
                    cameras[i].CommitChanges();
                }
            }
            if (RenderTarget.TargetFormat != format || RenderTarget.EdgeSize != size)
            {
                if(RenderTarget!= null)
                    RenderTarget.Dispose();
                RenderTarget = new RenderTextureCube(size, format, Format.D16_UNORM);
                _rendered = false;
            }
           
        }        
        
        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (RenderTarget != null)
                    RenderTarget.Dispose();
            }
            base.OnDispose(disposing);
        }

       
    }
}
