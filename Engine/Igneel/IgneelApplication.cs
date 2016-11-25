using System;
using Igneel.Assets;
using Igneel.Assets;
using Igneel.Presenters;
using Igneel.Rendering;
using Igneel.Rendering.Presenters;
using Igneel.States;

namespace Igneel
{
    public abstract class IgneelApplication
    {
        static IgneelApplication _instance;      
        bool _initialized;          

        protected IgneelApplication()
        {
            if (_instance != null)
                throw new InvalidOperationException();
            _instance = this;
            InitializeServices();
        }
                
        public static IgneelApplication CurrentApplication { get { return _instance; } }     

        public bool IsInitialized { get { return _initialized; } }      

        protected virtual void InitializeServices()
        {
            Service.Set<IgneelApplication>(this);                           
            Service.Set<IFactory<GraphicPresenter>>(new Factory<SingleViewPresenter>());            
            Service.Set<INotificationService>(new NotificationService());           
        }

        protected virtual void InitializeRendering()
        {
            foreach (var type in typeof(IgneelApplication).Assembly.ExportedTypes)
            {
                if (!type.IsAbstract && !type.IsInterface && type.IsClass && type.GetInterface("IRenderRegistrator") != null)
                {
                    ((IRenderRegistrator)Activator.CreateInstance(type)).RegisterInstance();
                }
            }          
        }

        public virtual void Initialize()
        {
            if (!_initialized)
            {
                EngineState.Lighting.HemisphericalAmbient = true;
                EngineState.Lighting.Reflection.Enable = true;
                EngineState.Lighting.TransparencyEnable = true;
                EngineState.Shading.BumpMappingEnable = true;
            

                InitializeRendering();                

                _initialized = true;                
            }
        }                         
    }
}
