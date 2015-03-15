using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering
{
   
    public class EngineState: IInitializable
    {
        private static ShadowState shadow;
        private static LightingState lighting;
        private static ShadingState shading;
        private static PhysicsState physicsState;  

        static EngineState()
        {
            physicsState = new PhysicsState();
            shadow = new ShadowState();
            lighting = new LightingState();
            shading = new ShadingState();

            //var service = Service.Get<InitializationService>();
            //if (service != null)
            //    service.Add(this);          
        }

        [AssetMember(typeof(StateStoreConverter))]
        public static ShadowState Shadow { get { return shadow; } set { shadow = value; } }

        [AssetMember(typeof(StateStoreConverter))]
        public static LightingState Lighting { get { return lighting; } set { lighting = value; } }

        [AssetMember(typeof(StateStoreConverter))]
        public static ShadingState Shading { get { return shading; } set { shading = value; } }

        [AssetMember(typeof(StateStoreConverter))]
        public static PhysicsState PhysicsState { get { return physicsState; } set { physicsState = value; } }        

        public virtual void Initialize()
        {
            
        }
    }

    public class EnabilitableState:EngineState,IEnabletable
    {
        private bool enable;

        public event EventHandler EnableChanged;

        public EnabilitableState() : this(false) { }

        public EnabilitableState(bool enable)
        {
            this.enable = enable;
        }


        
        public bool Enable
        {
            get { return enable; }
            set
            {
                if (enable != value)
                {
                    enable = value;
                    OnEnableChanged();
                }
            }
        }

        public virtual void OnEnableChanged()
        {
            if(EnableChanged != null)
                EnableChanged(this, EventArgs.Empty);
        }
    }
}
