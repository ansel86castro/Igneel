using Igneel.Collections;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Scenering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Igneel.Rendering
{
    public interface IPickContainer
    {
        Scene Scene { get; set; }

        IReadOnlyList<IIdentificable> PickedObjects { get; }

        void BeginPicking();

        unsafe void AddPickedIds(int* ids, int count);

        void EndPicking();        
    }

    public class PickContainer:IPickContainer
    {

        List<IIdentificable> pickedObjects = new List<IIdentificable>();
        ReadOnlyList<IIdentificable> pickedObjectsCollection;
        Scene scene;

        public event EventHandler PickingEnd;

        public PickContainer()
        {
            pickedObjectsCollection = new ReadOnlyList<IIdentificable>(pickedObjects);
        }

        public IReadOnlyList<IIdentificable> PickedObjects { get { return pickedObjectsCollection; } }

        public Scene Scene { get { return scene; } set { scene = value; } }

        public void BeginPicking()
        {
            pickedObjects.Clear();
        }

        public unsafe void AddPickedIds(int* ids, int count)
        {
            int[] data = new int[count];

            Marshal.Copy(new IntPtr(ids), data, 0, count);

            Array.Sort(data);
            int lastId = -1;         
            for (int i = 0; i < count; i++)
            {
                int id = ids[i];
                if (lastId != id)
                {
                    pickedObjects.Add(scene.Identificables[i]);
                }
                lastId = id;
            }
        }

        public void EndPicking()
        {
            if (PickingEnd != null)
                PickingEnd(this, EventArgs.Empty);
        }
    }

    public class HitTestTechnique: Technique
    {
        private RenderTexture2D rtx;

        bool selectMultiple;      
        int width;
        int height;
        Format depthFormat;
        private ViewPort vp;
        BlendState blend;
        RasterizerState rast;
        IPickContainer resolver;

        public HitTestTechnique(IPickContainer resolver = null)            
        {
            this.resolver = resolver;
            Initialize();            
        }

        public HitTestTechnique(int width, int height, Format depthFormat, IPickContainer resolver = null)            
        {
            this.width = width;
            this.height = height;
            this.depthFormat = depthFormat;
            this.resolver = resolver;

            Initialize();
        }

        public HitTestTechnique(int width, int height, IPickContainer resolver = null)
            : this(width, height, Format.D16_UNORM, resolver)
        {

        }

        public bool SelectMultiple { get { return selectMultiple; } set { selectMultiple = value; } }

        public Vector2 Location { get; set; }

        public Rectangle Rectangle { get; set; }

        public IPickContainer PickContainer { get { return resolver; } set { resolver = value; } }

        private void _RenderObjectIds(Action renderCallback = null)
        {            
            var graphics = GraphicDeviceFactory.Device;                    
            var lighting = EngineState.Lighting;            
          
            var oldvp = graphics.ViewPort;
            graphics.ViewPort = vp;

            graphics.RasterizerStack.Push(rast);
            graphics.BlendStack.Push(blend);            

            graphics.SaveRenderTarget();

            rtx.SetTarget(graphics);            
            
            var transp = lighting.TransparencyEnable;
            lighting.TransparencyEnable = false;
          
            graphics.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color4.Black, 1, 0);
          
            if (renderCallback != null)
                renderCallback();
            else
            {                
                foreach (var item in SceneManager.Scene.GetRenderList())
                {
                    item.Draw(PixelClipping.None);
                }
             
            }

            graphics.RestoreRenderTarget();
            graphics.BlendStack.Pop();
            graphics.RasterizerStack.Pop();            
                       
            graphics.ViewPort = oldvp;
            lighting.TransparencyEnable = transp;          
        }

        public void Resize(int width, int height)
        {
            this.width = width;
            this.height = height;

            Initialize();
        }

        public override void Apply()
        {
            Apply(null);
        }

        /// <summary>
        /// return the id
        /// </summary>
        /// <param name="renderCallback"></param>
        /// <returns></returns>
        public unsafe void Apply(Action renderCallback)
        {
            if (SceneManager.Scene == null)
                return;

            _RenderObjectIds(renderCallback);           

            resolver.BeginPicking();

            //perform simple selection
            if (!selectMultiple)
            {
                var point = Location;
                int x = (int)point.X;
                int y = (int)point.Y;

                try
                {
                    int id = 0;
                    var data = rtx.Texture.Map(0, MapType.Read);

                    int offset = y * data.RowPitch + x * sizeof(int);
                    id = *(int*)(data.DataPointer + offset);                  
                    resolver.AddPickedIds(&id, 1);
                }
                finally
                {
                    rtx.Texture.UnMap(0);
                }                          
               
            }
            else
            {
                var rec = this.Rectangle;
                try
                {
                    var stream = rtx.Texture.Map(0, MapType.Read);

                    int offset = rec.Y * stream.RowPitch + rec.X * sizeof(int);                    

                    byte* pter = (byte*)stream.DataPointer + offset;

                    for (int i = 0; i < rec.Height; i++, pter += stream.RowPitch)
                    {
                        resolver.AddPickedIds((int*)pter, rec.Width);
                    }
                }
                finally
                {
                    rtx.Texture.UnMap(0);
                }
            }

            resolver.EndPicking();
        }

        protected override void OnDispose(bool d)
        {
            if (d)
            {
                rtx.Dispose();

                Dispose<IDPhysicRenderTechnique>();
            }
            base.OnDispose(d);
        }

        public void Initialize()
        {
            rtx = new RenderTexture2D(width, height, Format.B8G8R8A8_UNORM, Format.D16_UNORM, Multisampling.Disable, true);            
            vp = new ViewPort(0, 0, width, height);

            blend = GraphicDeviceFactory.Device.CreateBlendState(new BlendDesc { BlendEnable = false, AlphaToCoverageEnable = false });
            rast = GraphicDeviceFactory.Device.CreateRasterizerState(new RasterizerDesc { Cull = CullMode.None, Fill = FillMode.Solid, MultisampleEnable = false, AntialiasedLineEnable = false });

            Require<IDPhysicRenderTechnique>();
        }
    }
}
