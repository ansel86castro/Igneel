using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

using Igneel.Assets;
using Igneel.Graphics;
using Igneel.Components;

namespace Igneel.Rendering
{
    
    public abstract class CubeMapTechique : NodeTechnique, IDeferreable
    {
        protected Vector3 positionOffset;
        protected RenderTextureCube renderTarget;
        protected int size;
        protected float zn, zf;
        protected Camera[] cameras;      
        protected bool isDynamic;
        protected Format format;       
        bool rendered;
        ViewPort vp;     
        public CubeMapTechique()
        {
            cameras = new Camera[6];           
        }      
        
        public CubeMapTechique(Vector3 position, int size, float zn, float zf, Format format)            
        {          
            this.size = size;
            this.zn = zn;
            this.zf = zf;
            this.format = format;

            cameras = new Camera[6];                               
        }

       
        public Texture2D Texture { get { return renderTarget.Texture; } }        

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

        
        
        public float NearClipPlane { get { return zn; } set { if (value <= 0)throw new ArgumentOutOfRangeException("value"); zn = value; } }

        
        
        public float FarClipPlane { get { return zf; } set { if (value <= 0)throw new ArgumentOutOfRangeException("value"); zf = value; } }
      
        public bool IsDynamic { get { return isDynamic; } set { isDynamic = value; } }

        [Browsable(true), ReadOnly(true)]
        
        
        public Format Format { get { return format; } set { if (value == Format.UNKNOWN) throw new ArgumentException("value"); format = value; } }

       
        public Camera[] Cameras { get { return cameras; } }

        public override void UpdatePose(Matrix affectorPose)
        {
            var pos = Vector3.Transform(positionOffset, globalToAffector * affectorPose);
            for (int i = 0; i < 6; i++)
            {
                cameras[i].Position = pos;
                cameras[i].CommitChanges();
            }            
        }
       
        public virtual void Initialize()
        {
            rendered = false;
            Vector3 position = Vector3.Zero;
            cameras[0] = Camera.FromLookAt(null, Vector3.Zero, Vector3.UnitX, Vector3.UnitY, zn, zf); //PositiveX
            cameras[1] = Camera.FromLookAt(null, Vector3.Zero, -Vector3.UnitX, Vector3.UnitY, zn, zf); //NegativeX

            cameras[2] = Camera.FromLookAt(null, position, Vector3.UnitY, -1 * Vector3.UnitZ, zn, zf); //PositiveY
            cameras[3] = Camera.FromLookAt(null, position, -Vector3.UnitY, Vector3.UnitZ, zn, zf); //NegativeY

            cameras[4] = Camera.FromLookAt(null, position, Vector3.UnitZ, Vector3.UnitY, zn, zf);  //PositiveZ
            cameras[5] = Camera.FromLookAt(null, position, -Vector3.UnitZ, Vector3.UnitY, zn, zf); //NegativeZ          

            for (int i = 0; i < 6; i++)
            {
                cameras[i].AspectRatio = 1;
                cameras[i].FieldOfView = Numerics.PIover2;
                cameras[i].CommitChanges();
            }

            if (format == Format.UNKNOWN) throw new InvalidOperationException("Invalid format");
            if (size <= 0) throw new InvalidOperationException("size less than 0");

            if (renderTarget != null)
                renderTarget.Dispose();
            renderTarget = new RenderTextureCube(size, format, Engine.Graphics.BackDepthBuffer.SurfaceFormat);
            rendered = false;
            vp = new ViewPort(0, 0, size, size);
        }
               
        public override void Apply()
        {
            if (!isDynamic && this.rendered) return;          

            var device = Engine.Graphics;
            device.SaveRenderTarget();
            var oldvp = device.ViewPort;

            device.ViewPort = vp;

            var scene = Engine.Scene;
            var oldCamera = scene.ActiveCamera;
            foreach (var item in nodes)            
                item.Visible = false;

            bool restore = false;
            var useDefaulTechnique = Engine.Lighting.Reflection.UseDefaultTechnique;
            if (useDefaulTechnique && !Engine.IsTechniqueActive<SceneTechnique>())
            {
                Engine.PushTechnique<SceneTechnique>();
                restore = true;
            }         

            for (int i = 0; i < 6; i++)
            {
                renderTarget.SetTarget(i, device);                

                scene.ActiveCamera = cameras[i];

                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Engine.BackColor, 1, 0);

                Engine.ApplyTechnique();               
            }

            foreach (var item in nodes)
                item.Visible = true;

            device.ViewPort = oldvp;
            device.RestoreRenderTarget();
            scene.ActiveCamera = oldCamera;

            if (restore)
            {
                Engine.PopTechnique();
            }

            this.rendered = true;
        }        
        
        public void UpdateCameras(Vector3 position , float zn ,float zf)
        {           
            this.zn = zn;
            this.zf = zf;
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
            if (zn != cameras[0].ZNear || zf != cameras[0].ZFar)
            {
                for (int i = 0; i < cameras.Length; i++)
                {
                    cameras[i].ZNear = zn;
                    cameras[i].ZFar = zf;
                    cameras[i].CommitChanges();
                }
            }
            if (renderTarget.TargetFormat != format || renderTarget.EdgeSize != size)
            {
                if(renderTarget!= null)
                    renderTarget.Dispose();
                renderTarget = new RenderTextureCube(size, format, Format.D16_UNORM);
                rendered = false;
            }
           
        }        
        
        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (renderTarget != null)
                    renderTarget.Dispose();
            }
            base.OnDispose(disposing);
        }

       
    }
}
