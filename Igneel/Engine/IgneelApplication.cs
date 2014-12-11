using Igneel.Assets;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Igneel
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
            Service.Set<InitializationService>();          
            Service.Set<IFactory<AssetManager>>(new AssetManagerFactory());          
            Service.Set<IFactory<GraphicPresenter>>(new Factory<SingleViewPresenter>());
            Service.SetFactory<IAssetRepository>(() => new LocalAssetRepository());            
            Service.Set<IFactory<ApplicationStateAsset>>(new Factory<EngineAsset>());            
            Service.Set<DynamicService>();
            Service.Set<PostRenderService>();
            Service.Set<PresentService>();                    
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
                Engine.Lighting.HemisphericalAmbient = true;
                Engine.Lighting.Reflection.Enable = true;
                Engine.Lighting.TransparencyEnable = true;
                Engine.Shading.BumpMappingEnable = true;

                manager = Service.Require<AssetManager>();

                InitializeRendering();

                var srv = Service.Get<InitializationService>();
                srv.InitializeItems();
                Service.Remove<InitializationService>();

                initialized = true;

                manager.OnApplicationStart();
            }
        }                         
    }
}
