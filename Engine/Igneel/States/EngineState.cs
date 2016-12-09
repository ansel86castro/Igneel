using System;
using Igneel.Assets;
using Igneel.Assets.StorageConverters;

namespace Igneel.States
{
   
    public class EngineState: IInitializable
    {
        private static ShadowState _shadow;
        private static LightingState _lighting;
        private static ShadingState _shading;
        private static PhysicsState _physicsState;  

        static EngineState()
        {
            _physicsState = new PhysicsState();
            _shadow = new ShadowState();
            _lighting = new LightingState();
            _shading = new ShadingState();

            //var service = Service.Get<InitializationService>();
            //if (service != null)
            //    service.Add(this);          
        }

        [AssetMember(typeof(StateStoreConverter))]
        public static ShadowState Shadow { get { return _shadow; } set { _shadow = value; } }

        [AssetMember(typeof(StateStoreConverter))]
        public static LightingState Lighting { get { return _lighting; } set { _lighting = value; } }

        [AssetMember(typeof(StateStoreConverter))]
        public static ShadingState Shading { get { return _shading; } set { _shading = value; } }

        [AssetMember(typeof(StateStoreConverter))]
        public static PhysicsState PhysicsState { get { return _physicsState; } set { _physicsState = value; } }        

        public virtual void Initialize()
        {
            
        }
    }

    public class EnabilitableState:EngineState,IEnabletable
    {
        private bool _enable;

        public event EventHandler EnableChanged;

        public EnabilitableState() : this(false) { }

        public EnabilitableState(bool enable)
        {
            this._enable = enable;
        }


        
        public bool Enable
        {
            get { return _enable; }
            set
            {
                if (_enable != value)
                {
                    _enable = value;
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
