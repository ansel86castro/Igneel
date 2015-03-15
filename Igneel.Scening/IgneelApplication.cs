using Igneel.Assets;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Scenering.Assets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Igneel.Scenering
{
    public abstract class IgneelApplication
    {
        static IgneelApplication instance;
        AssetManager manager;       
        bool initialized;          

        protected IgneelApplication()
        {
            if (instance != null)
                throw new InvalidOperationException();
            instance = this;
            InitializeServices();
        }
                
        public static IgneelApplication CurrentApplication { get { return instance; } }     

        public bool IsInitialized { get { return initialized; } }      

        protected virtual void InitializeServices()
        {
            Service.Set<IgneelApplication>(this);               
            Service.Set<IFactory<AssetManager>>(new AssetManagerFactory());          
            Service.Set<IFactory<GraphicPresenter>>(new Factory<SingleViewPresenter>());
            Service.SetFactory<IAssetRepository>(() => new LocalAssetRepository());            
            Service.Set<IFactory<ApplicationStateAsset>>(new Factory<EngineAsset>());                                                    
            Service.Set<INotificationService>(new NotificationService());           
        }

        protected virtual void InitializeRendering()
        {
            foreach (var type in typeof(Render).Assembly.ExportedTypes)
            {
                if (!type.IsAbstract && !type.IsInterface && type.IsClass && type.GetInterface("IRenderRegistrator") != null)
                {
                    ((IRenderRegistrator)Activator.CreateInstance(type)).RegisterInstance();
                }
            }          
        }

        public virtual void Initialize()
        {
            if (!initialized)
            {
                EngineState.Lighting.HemisphericalAmbient = true;
                EngineState.Lighting.Reflection.Enable = true;
                EngineState.Lighting.TransparencyEnable = true;
                EngineState.Shading.BumpMappingEnable = true;

                manager = Service.Require<AssetManager>();

                InitializeRendering();                

                initialized = true;

                manager.OnApplicationStart();
            }
        }                         
    }
}
