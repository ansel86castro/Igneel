using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{
    public interface ICurveOutput :IAssetProvider
    {
        void SetOutput(IntPtr value, IAnimContext context);
    }

    public class CurveOutput<T> : ICurveOutput           
    {
        Action<T, IntPtr> action;        
      
        public CurveOutput() { }

        public CurveOutput(Action<T, IntPtr> action)
        {            
            this.action = action;
        }
        
        [AssetMember(typeof(DelegateConverter))]
        public Action<T, IntPtr> OutputAction { get { return action; } set { action = value; } }        

        public void SetOutput(IntPtr value, IAnimContext context)
        {
            IAnimContext<T> cursor =  ClrRuntime.Runtime.StaticCast<IAnimContext<T>>(context);
            while (cursor != null)
            {
                action(cursor.Data, value);
                cursor = cursor.Next;
            }
        }
   
        public Asset CreateAsset()
        {
            return Asset.Create(this);
        }
    }
}
