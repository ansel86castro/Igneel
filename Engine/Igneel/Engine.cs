using System;
using System.Threading;
using Igneel.Assets;
using Igneel.Techniques;
using Igneel.Graphics;
using Igneel.Input;
using Igneel.Physics;
using Igneel.Rendering;
using Igneel.Animations;
using Igneel.Animations;
using Igneel.Rendering.Presenters;
using Igneel.SceneManagement;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Igneel
{
    public static class Engine
    {
        public enum LoopStage { None, PreRender, Render, PostRender, Update, Present }

        public enum RunningState : int
        {
            None,
            Stoped,
            Paused,
            Running,
            Locked
        }
        
        private static bool _initialized;                   
        private static bool _constantFrameRate;
        private static float _frameRate = 1.0f / 60.0f;
        private static float _elapseTime;                          
        private static bool _firstFrame = true;
        private static GraphicPresenter _presenter;
        private static Keyboard _keyboard;
        private static Mouse _mouse;
        private static Joystick[] _joysticks;
        private static int _mainLoopState;
        private static LoopStage _engineLoopStage;
        private static int _pausedCount;
        private static ManualResetEvent _waitHandler;
        private static bool _LockProcced = false;
        private static object _lockSync = new object();        
        private static Timer _gametimer = new Timer();
        private static Thread gameLoopTask;

        private static object LoopStepLock = new object();

        static bool waits;
        private static GraphicDevice _graphics;

        private static MouseController _mouseController;
        private static KeyBoardController _keyboardController;

        private static ConcurrentQueue<Action> _messages = new ConcurrentQueue<Action>();


        /// <summary>
        /// Invoke User defined Update logic
        /// </summary>
        public static event Action<float> UpdateFrame;

        /// <summary>
        /// Invoke User defined Rendering logic. User Must Call GraphicDevice.Present() to display the content
        /// </summary>
        public static event Action RenderFrame;
                       

        public static Keyboard KeyBoard { get { return _keyboard; } }

        public static Mouse Mouse { get { return _mouse; } }

        public static MouseController MouseController { get { return _mouseController; } }

        public static KeyBoardController KeyboardController { get { return _keyboardController; } }

        public static Joystick[] Joysticks { get { return _joysticks; } }       

        public static float ElapsedTime { get { return _elapseTime; } }
        
        public static bool ConstantFrameRate { get { return _constantFrameRate; } set { _constantFrameRate = value; } }
        
        public static float FrameRate { get { return _frameRate; } set { _frameRate = value; } }

        public static bool Paused 
        {
            get { return _mainLoopState == (int)RunningState.Paused || _mainLoopState == (int)RunningState.Locked; } 
            set 
            {
                if (_mainLoopState == (int)RunningState.Running || _mainLoopState == (int)RunningState.Paused)
                    _mainLoopState = value ? (int)RunningState.Paused : (int)RunningState.Running; 
            } 
        }
            
        public static Timer Time { get { return _gametimer; } }

        public static Scene Scene { get; set; }

        public static bool IsInitialized { get { return _initialized; } }                     
      
        public static Color4 BackColor { get; set; }

        public static bool IsGameLoopAsync { get { return gameLoopTask != null; } }

        public static RunningState MainLoopState { get { return (RunningState)_mainLoopState; } }

        public static LoopStage FrameStepState { get { return _engineLoopStage; } }

        public static GraphicDevice Graphics { get { return _graphics; } }

        public static GraphicPresenter Presenter
        {
            get { return _presenter; }
            set
            {
                _presenter = value;
            }
        }

        static Engine()
        {
            _waitHandler = new ManualResetEvent(false);               
        }

        public static void Initialize(IInputContext inputContext, GraphicDeviceDesc graphicDeviceDesc)
        {
            if (_initialized) return;                                  

            var graphicFactory = Service.Require<GraphicDeviceFactory>();
            _graphics = graphicFactory.CreateDevice(graphicDeviceDesc);
                         
            var input = Service.Get<InputManager>();            
            if (input != null)
            {
                _keyboard = input.CreateKeyboard(inputContext);
                if (_keyboard != null)
                {
                    _keyboardController = new KeyBoardController(_keyboard);
                }

                _mouse = input.CreateMouse(inputContext);
                if(_mouse!=null)
                {
                    _mouseController = new MouseController(_mouse);
                }
                _joysticks = input.CreateJoysticks(inputContext);
            }            

            RenderManager.PushTechnique<DefaultTechnique>();
            _initialized = true;
        }

        public static void StartGameLoop()
        {
            if (!_initialized)
                throw new InvalidOperationException("Engine not Initialized");

            if (gameLoopTask != null && _mainLoopState != (int)RunningState.Running)
            {
                _mainLoopState = (int)RunningState.Stoped;
                gameLoopTask.Join();
            }

            _mainLoopState = (int)RunningState.Running;
            gameLoopTask = new Thread(new ThreadStart(GameLoop));
            gameLoopTask.Start();
        }

        public static void StopGameLoop()
        {
            if (gameLoopTask == null)
                return;

            Invoke(() => _mainLoopState = (int)RunningState.Stoped);          
            gameLoopTask = null;
        }

        public static void Invoke(Action action)
        {
            _messages.Enqueue(action);
        }

        private static void GameLoop()
        {
            while (_mainLoopState != (int)RunningState.None &&
                _mainLoopState != (int)RunningState.Stoped)
            {
                Action action;
                while (_messages.TryDequeue(out action))
                    action();

                LoopStep();
            }
        }

       
       public static void LoopStep()
       {
           if (!_initialized || _mainLoopState == (int)RunningState.Paused)
               return;

           waits = false;


           waits = _mainLoopState == (int)RunningState.Locked;

           if (waits)
           {
               _LockProcced = true;

               _waitHandler.WaitOne();
               _waitHandler.Reset();
           }

           #region Update Frame

           var input = Service.Get<InputManager>();
           var useInput = input != null;

           if (useInput)
               input.CheckInputStates();

           ComputeElapsedTime();

           if (_mouseController != null)
               _mouseController.Update(_elapseTime);
           if (_keyboardController != null)
               _keyboardController.Update(_elapseTime);

           _engineLoopStage = LoopStage.Update;
           var scene = Scene;
           if (scene != null)
               scene.Update(_elapseTime);

           OnUpdate();

           #endregion

           #region RenderFrame

           var graphics = GraphicDeviceFactory.Device;

           graphics.SetRenderTarget(graphics.BackBuffer, graphics.BackDepthBuffer);

           if (scene != null)
           {
               _engineLoopStage = LoopStage.PreRender;

               //Perform Preprocesing Step
               scene.UpdateTecniques();
           }

           //Render Scene
           if (_presenter != null)
           {
               _engineLoopStage = LoopStage.Render;
               _presenter.RenderFrame();
           }

           //Invoke User defined Rendering logic. User Must Call GraphicDevice.Present() to display the content
           _engineLoopStage = LoopStage.PostRender;
           if (RenderFrame != null)
               RenderFrame();

           _engineLoopStage = LoopStage.Present;

           #endregion

           if (useInput)
               input.ResetInputStates();
       }

        private static void ComputeElapsedTime()
        {
            float elapsed = _frameRate;
            if (_firstFrame)
            {
                _gametimer.Reset();
                _firstFrame = false;
            }
            if (!_constantFrameRate)
                elapsed = _gametimer.Elapsed();

            _elapseTime = elapsed;
        }

      
        
        private static void OnUpdate()
        {
            if (UpdateFrame != null)
                UpdateFrame(_elapseTime);
        }

        public static void Lock()
        {
            lock (_lockSync)
            {
                if (_mainLoopState != (int)RunningState.None)
                    _pausedCount++;

                if (_mainLoopState == (int)RunningState.Running)
                {
                    lock (LoopStepLock)
                    {
                        _mainLoopState = (int)RunningState.Locked;
                    }
                    while (!_LockProcced) 
                    {
                       
                    }
                }

            }
        }

        public static void Unlock()
        {
            if (_mainLoopState != (int)RunningState.Locked)
                return;

            _pausedCount--;
            if (_mainLoopState == (int)RunningState.Locked && _pausedCount == 0)
            {
                lock (LoopStepLock)
                {
                    _mainLoopState = (int)RunningState.Running;
                }
                _waitHandler.Set();
                _LockProcced = false;
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

        //public static void Run<T>()where T:IgneelApplication
        //{
        //    var app = Activator.CreateInstance<T>();
        //    app.Initialize();           
        //}
    }

    
}
