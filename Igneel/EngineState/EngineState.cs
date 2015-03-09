
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
   
    public class EngineState: IInitializable
    {
        public EngineState()
        {
            var service = Service.Get<InitializationService>();
            if (service != null)
                service.Add(this);
        }

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
