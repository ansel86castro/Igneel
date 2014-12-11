using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Igneel.Services;
using Igneel.Assets;
using Igneel.Graphics;
using Igneel.Physics;
using Igneel.Input;
using Igneel.Components;
using Igneel.Rendering;

namespace Igneel
{
    public partial class Engine
    {
        public enum LoopStage { None, PreRender, Render, PostRender, Update, Present }

        public enum RunningState { None, Stoped, Started, Paused, Running ,Locked }

        //#region Fields                     
        private static bool initialized;        
        //private static bool paused;     
        private static Timer gametimer = new Timer();
        private static GraphicDevice graphics;     
        private static ShadowState shadow;
        private static LightingState lighting;
        private static ShadingState shading;
        private static PhysicsState physicsState;        
        private static Scene scene;      
        private static Multisampling sampling = Multisampling.Disable;
        private static bool allowAntialiazing;
        private static bool constantFrameRate;
        private static float frameRate = 1.0f / 60.0f;
        private static float elapseTime;                     
        private static SceneManager sceneManager;          
        private static bool firstFrame = true;
        private static GraphicPresenter presenter;
        private static Keyboard keyboard;
        private static Mouse mouse;
        private static Joystick[] joysticks;
        private static CharacterControllerManager controllerManager;
        private static Thread loopThread;
        private static RunningState mainLoopState = RunningState.Running;
        private static ManualResetEvent waitHandler;
        private static ManualResetEvent proccedWaitHandler;
        private static object lockSync = new object();   

        public static event Action<float> Update;
        public static event Action RenderFrame;

        private static LoopStage engineLoopStage;
        private static int pausedCount;            

        public static GraphicDevice Graphics { get { return graphics; } }

        public static Keyboard KeyBoard { get { return keyboard; } }

        public static Mouse Mouse { get { return mouse; } }

        public Joystick[] Joysticks { get { return joysticks; } }       

        public static float ElapsedTime { get { return elapseTime; } }

        [AssetMember]
        public static bool ConstantFrameRate { get { return constantFrameRate; } set { constantFrameRate = value; } }

        [AssetMember]
        public static float FrameRate { get { return frameRate; } set { frameRate = value; } }

        public static bool Paused 
        { 
            get { return mainLoopState == RunningState.Paused || mainLoopState == RunningState.Locked; } 
            set 
            { 
                if(mainLoopState== RunningState.Running || mainLoopState == RunningState.Paused)
                    mainLoopState = value ? RunningState.Paused : RunningState.Running; 
            } 
        }
            
        public static Timer Time { get { return gametimer; } }

        [AssetMember(typeof(StateStoreConverter))]
        public static ShadowState Shadow { get { return shadow; } set { shadow = value; } }

        [AssetMember(typeof(StateStoreConverter))]
        public static LightingState Lighting { get { return lighting; } set { lighting = value; } }

        [AssetMember(typeof(StateStoreConverter))]
        public static ShadingState Shading { get { return shading; } set { shading = value; } }                                          

        [AssetMember(storeAs: StoreType.Reference)]
        public static Scene Scene { get { return scene; } set { scene = value; } }

        public static Physic PhyScene { get { return scene != null ? scene.Physics : null; } }       

        public static CharacterControllerManager ControllerManager { get { return controllerManager; } }

        public static bool Initialized { get { return initialized; } }              

        [AssetMember(storeAs:StoreType.Reference)]
        public static SceneManager SceneManager { get { return sceneManager; } internal set { sceneManager = value; } }

       static IntPtr hwnd;

        [AssetMember]
        public static Color4 BackColor { get; set; }

        public static GraphicPresenter Presenter
        {
            get { return presenter; }
            set
            {
                if (value == null) throw new NullReferenceException();
                presenter = value;
            }
        }            

        public static void InitializeEngine(IntPtr inputHWND, GraphicDeviceDesc desc)
        {
            if (initialized) throw new InvalidOperationException();

            hwnd = inputHWND;
            loopThread = Thread.CurrentThread;
            waitHandler = new ManualResetEvent(false);
            proccedWaitHandler = new ManualResetEvent(false);

            var graphicFactory = Service.Require<GraphicDeviceFactory>();
            graphics = graphicFactory.CreateInstance(desc);

            var physicManager = Service.Get<PhysicManager>();
            if (physicManager != null)
            {
                controllerManager = physicManager.CreateControllerManager();
            }            

            physicsState = new PhysicsState();
            shadow = new ShadowState();
            lighting = new LightingState();
            shading = new ShadingState();
            sceneManager = new SceneManager();

            var input = Service.Get<InputManager>();            
            if (input != null)
            {
                keyboard = input.CreateKeyboard(inputHWND);
                mouse = input.CreateMouse(inputHWND);
                joysticks = input.CreateJoysticks(inputHWND);
            }
            

            PushTechnique<SceneTechnique>();
            initialized = true;
        }

        static object lockedSync = new object();      

        public static void Render()
        {            
            if (!initialized || mainLoopState == RunningState.Paused)
                return;

            if (mainLoopState == RunningState.Locked)
            {
                lock (lockedSync)
                {
                    //signal to execute the pending task who call Lock
                    proccedWaitHandler.Set();
                }

                //wait until the pending task finish calling Unlock
                waitHandler.WaitOne();
                waitHandler.Reset();
            }          

            var input = Service.Get<InputManager>();
            var useInput = input != null;

            if (useInput)
                input.CheckInputStates();

            ComputeElapsedTime();

            engineLoopStage = LoopStage.Update;       
            if (scene != null)
                scene.Update(elapseTime);

            OnUpdate();

            graphics.OMSetRenderTarget(graphics.OMBackBuffer, graphics.OMBackDepthStencil);

            if (scene != null)
            {                
                engineLoopStage = LoopStage.PreRender;                                             
                scene.ApplyTechniques();                
            }

            if (presenter != null)
            {
                engineLoopStage = LoopStage.Render;
                presenter.RenderFrame();
            }

            engineLoopStage = LoopStage.PostRender;
            if (RenderFrame != null)
                RenderFrame();

            engineLoopStage = LoopStage.Present;

            if (useInput)
                input.ResetInputStates();
        }

        public static void ComputeElapsedTime()
        {
            float elapsed = frameRate;
            if (firstFrame)
            {
                gametimer.Reset();
                firstFrame = false;
            }
            if (!constantFrameRate)
                elapsed = gametimer.Elapsed();

            elapseTime = elapsed;
        }

        //public static Vector2 WorldToClient(Camera camera, Vector3 worldPostion)
        //{
        //    int halfWidth = pp.BackBufferWidth / 2;
        //    int halfHeight = pp.BackBufferHeight / 2;

        //    Vector3 projPosition = Vector3.TransformCoordinate(worldPostion, camera.ViewProj);
        //    Vector2 screenPosition = new Vector2(projPosition.X * halfWidth + halfWidth, projPosition.Y * -halfHeight + halfHeight);
        //    screenPosition.X = (float)Math.Ceiling((float)screenPosition.X);
        //    screenPosition.Y = (float)Math.Ceiling((float)screenPosition.Y);
        //    return  screenPosition;
        //}          

        //public static Vector3 ScreenToWorld(Camera camera, Vector2 screenPos ,Size vpSize , float projZ = 1.0f)
        //{
        //    Vector3 projPoint = new Vector3(((2 * screenPos.X / (float)vpSize.Width) - 1), -((2 * screenPos.Y / (float)vpSize.Height) - 1), projZ);
        //    Vector3 worldPos = Vector3.TransformCoordinate(projPoint, camera.InvViewProjection);
        //    return worldPos;
        //}

        //public static Vector3 ScreenToWorld(Camera camera, Vector2 screenPos, float projZ = 1.0f)
        //{
        //    return ScreenToWorld(camera, screenPos, new Size(pp.BackBufferWidth, pp.BackBufferHeight), 1);
        //}

        //public static Vector3 ComputeWorldDisplacement(float dx, float dy, Vector3 position ,Size vpSize, Matrix viewProj)
        //{
        //    float xCenter = vpSize.Width * 0.5f;
        //    float yCenter = -vpSize.Height * 0.5f;         

        //    //Vector3 viewPos = Vector3.TransformCoordinate(position, camera.View);
        //    Vector3 projPos = Vector3.TransformCoordinate(position, viewProj);

        //    projPos.X += (dx / xCenter);
        //    projPos.Y -= (dy / yCenter);

        //    Matrix invVProj = Matrix.Invert(viewProj);
        //    Vector3 posDisp = Vector3.TransformCoordinate(projPos, invVProj);

        //    Vector3 disp = posDisp - position;

        //    return disp;
        //}

        //public static Vector3 ComputeViewDisplacement(float dx, float dy, Vector3 position, Size vpSize,
        //    Matrix view, Matrix proj)
        //{
        //    float xCenter = vpSize.Width * 0.5f;
        //    float yCenter = -vpSize.Height * 0.5f;

        //    Vector3 viewPos = Vector3.TransformCoordinate(position, view);
        //    Vector3 projPos = Vector3.TransformCoordinate(viewPos, proj);

        //    projPos.X += (dx / xCenter);
        //    projPos.Y -= (dy / yCenter);

        //    Matrix invProj = Matrix.Invert(proj);
        //    Vector3 posDisp = Vector3.TransformCoordinate(projPos, invProj);

        //    Vector3 disp = posDisp - viewPos;

        //    return disp;
        //}

        public static void Run<T>()where T:IgneelApplication
        {
            var app = Activator.CreateInstance<T>();
            app.Initialize();           
        }
        
        public static void OnUpdate()
        {
            if (Update != null)
                Update(elapseTime);
        }

        public static void Lock()
        {
            var callingThread = Thread.CurrentThread;
            if (callingThread == loopThread)
                return;

            lock (lockSync)
            {
                if (mainLoopState != RunningState.None)
                    pausedCount++;
                if (mainLoopState == RunningState.Running || mainLoopState == RunningState.Started)
                {
                    lock (lockedSync)
                    {
                        mainLoopState = RunningState.Locked;                      
                    }
                    proccedWaitHandler.WaitOne();

                    proccedWaitHandler.Reset();
                }
            }            
        }

        public static void Unlock()
        {
            var callingThread = Thread.CurrentThread;
            if (callingThread == loopThread)
                return;

            lock (lockSync)
            {
                if (mainLoopState != RunningState.Locked)
                    return;
                pausedCount--;
                if (mainLoopState == RunningState.Locked && pausedCount == 0)
                {
                    mainLoopState = RunningState.Running;
                    waitHandler.Set();
                }
            }
        }

        public static void Lock(Action action)
        {
            Lock();

            try
            {
                action();
            }
            finally
            {
                Unlock();
            }
        }
    }

    
}
