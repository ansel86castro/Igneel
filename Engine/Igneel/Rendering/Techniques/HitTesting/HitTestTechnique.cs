using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Igneel.Collections;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
using Igneel.States;

namespace Igneel.Techniques
{
    //public interface IPickContainer
    //{
    //    Scene Scene { get; set; }

    //    IReadOnlyList<IIdentifable> PickedObjects { get; }

    //    void BeginPicking();

    //    unsafe void AddPickedIds(int* ids, int count);

    //    void EndPicking();
    //}

    //public class PickContainer : IPickContainer
    //{

    //    List<IIdentifable> _pickedObjects = new List<IIdentifable>();
    //    ReadOnlyList<IIdentifable> _pickedObjectsCollection;
    //    Scene _scene;

    //    public event EventHandler PickingEnd;

    //    public PickContainer()
    //    {
    //        _pickedObjectsCollection = new ReadOnlyList<IIdentifable>(_pickedObjects);
    //    }

    //    public IReadOnlyList<IIdentifable> PickedObjects { get { return _pickedObjectsCollection; } }

    //    public Scene Scene { get { return _scene; } set { _scene = value; } }

    //    public void BeginPicking()
    //    {
    //        _pickedObjects.Clear();
    //    }

    //    public unsafe void AddPickedIds(int* ids, int count)
    //    {
    //        int[] data = new int[count];

    //        Marshal.Copy(new IntPtr(ids), data, 0, count);

    //        Array.Sort(data);
    //        int lastId = -1;
    //        for (int i = 0; i < count; i++)
    //        {
    //            int id = ids[i];
    //            if (lastId != id)
    //            {
    //                //_pickedObjects.Add(_scene.Identificables[i]);
    //            }
    //            lastId = id;
    //        }
    //    }

    //    public void EndPicking()
    //    {
    //        if (PickingEnd != null)
    //            PickingEnd(this, EventArgs.Empty);
    //    }
    //}
  
    public class HitTestTechnique : Technique
    {
        public enum HitTestMode
        {
            Single, Multiple
        }

        private RenderTexture2D _rtx;
        private int _width;
        private int _height;
        private Format _depthFormat;
        private ViewPort _vp;
        private BlendState _blend;
        private RasterizerState _rast;

        IHitTestRender hitTestRender;

        private DepthStencilState _depth;

        public HitTestMode Mode { get; set; }

        public Vector2 HitTestLocation { get; set; }

        public Rectangle HitTestRegion { get; set; }

        public List<HitTestResult> HitTestResults { get { return hitTestRender.HitTestResults; } }

        public RenderTexture2D Texture { get { return _rtx; } }
        
        public HitTestTechnique(int width, int height, Format depthFormat, IHitTestRender hitTestRender = null)
        {
            this._width = width;
            this._height = height;
            this._depthFormat = depthFormat;            

            Initialize();

            this.hitTestRender = hitTestRender;
            if (this.hitTestRender == null)
                this.hitTestRender = new SceneHitTestRender();
        }

        public HitTestTechnique(int width, int height, IHitTestRender hitTestRender = null)
            : this(width, height, Format.D24_UNORM_S8_UINT, hitTestRender)
        {

        }

        public void Initialize()
        {
            if (_rtx != null)
                _rtx.Dispose();

            _rtx = new RenderTexture2D(_width, _height, Format.R8G8B8A8_UNORM, _depthFormat, Multisampling.Disable, true);
            _vp = new ViewPort(0, 0, _width, _height);

            if (_blend != null)
                _blend.Dispose();

            _blend = GraphicDeviceFactory.Device.CreateBlendState(new BlendDesc(
                blendEnable:false,
                alphaToCoverageEnable:false));

            if (_rast != null)
                _rast.Dispose();

            _rast = GraphicDeviceFactory.Device.CreateRasterizerState(new RasterizerDesc
            (
                cull : CullMode.None,
                fill : FillMode.Solid,
                multisampleEnable : false,
                antialiasedLineEnable : false
            ));

            _depth = Engine.Graphics.CreateDepthStencilState(new DepthStencilStateDesc(
                depthFunc:Comparison.Less
                ));

            Require<IdPhysicRenderTechnique>();
        }

        public void Resize(int width, int height)
        {
            if (this._width == width && this._height == height)
                return;
            this._width = width;
            this._height = height;

            if (_rtx != null)
                _rtx.Dispose();

            _rtx = new RenderTexture2D(_width, _height, Format.R8G8B8A8_UNORM, _depthFormat, Multisampling.Disable, true);
            _vp = new ViewPort(0, 0, _width, _height);

        }

        public override unsafe void Apply()
        {                    
            #region Render

            var graphics = GraphicDeviceFactory.Device;
            var lighting = EngineState.Lighting;

            var oldvp = graphics.ViewPort;
            graphics.ViewPort = _vp;
      
            graphics.RasterizerStack.Push(_rast);
            graphics.BlendStack.Push(_blend);
            graphics.DepthStencilStack.Push(_depth);

            graphics.SaveRenderTarget();

            _rtx.SetTarget(graphics);

            var transp = lighting.TransparencyEnable;
            lighting.TransparencyEnable = false;

            graphics.Clear(ClearFlags.Target | ClearFlags.ZBuffer, new Color4(0, 0, 0, 0), 1, 0);

            hitTestRender.RenderObjects();

            graphics.RestoreRenderTarget();

            graphics.RasterizerStack.Pop();
            graphics.BlendStack.Pop();
            graphics.DepthStencilStack.Pop();
            

            graphics.ViewPort = oldvp;
            lighting.TransparencyEnable = transp;

            #endregion

            
            #region Test Surface
           
            try
            {
              
                var stream = _rtx.Texture.Map(0, MapType.Read);

                if (Mode == HitTestMode.Single)
                {
                    var point = HitTestLocation;
                    int x = (int)point.X;
                    int y = (int)point.Y;

                    int offset = y * stream.RowPitch + x * sizeof(int);
                    var abgr = *(int*)(stream.DataPointer + offset);                 
                    HitTestResult result;                 
                    hitTestRender.FindResult(abgr, out result); // AddSelection(id - 1);

                    //var pter = (byte*)stream.DataPointer;
                    // for (int i = 0; i <  _rtx.Height; i++)
                    // {
                    //     for (int j = 0;  j < _rtx.Width; j++)
                    //     {
                    //         id = ((int*)pter)[j];
                    //         if (id >0)
                    //             AddSelection(id, visibleComponents, drawables);
                    //     }

                    //     pter += stream.RowPitch;
                    // }

                }
                else
                {
                    var rec = this.HitTestRegion;
                    int offset = rec.Y * stream.RowPitch + rec.X * sizeof(int);

                    byte* pter = (byte*)stream.DataPointer + offset;

                    for (int i = 0; i < rec.Height && i < _rtx.Height; ++i)
                    {
                        for (int j = 0; j < rec.Width && j < _rtx.Width; ++j)
                        {
                            var abgr = ((int*)pter)[j];                       
                            HitTestResult result;                         
                            hitTestRender.FindResult(abgr, out result); 
                        }

                        pter += stream.RowPitch;
                    }
                }
            }

            finally
            {
                _rtx.Texture.UnMap(0);
            }

            #endregion
        }

        //private void AddSelection(int id)
        //{
        //    var visibleComponents = Engine.Scene.VisibleComponents;
        //    var drawables = Engine.Scene.Drawables;

        //    if (id < visibleComponents.Count)
        //    {
        //        var entry = visibleComponents[id];
        //        results.Add(new HitTestResult
        //        {
        //            Drawable = entry.Graphic,
        //            Frame = entry.Node,
        //            RenderId = id
        //        });
        //    }
        //    else
        //    {
        //        id -= visibleComponents.Count;
        //        if (id < drawables.Count)
        //        {
        //            results.Add(new HitTestResult
        //            {
        //                Drawable = drawables[id],
        //                RenderId = id
        //            });
        //        }
        //    }
        //}

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                _rtx.Dispose();

                Dispose<IdPhysicRenderTechnique>();
            }
        }
       
    }

   
}
