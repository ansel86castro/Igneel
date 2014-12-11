using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{
    public interface IAnimContext: IAssetProvider
    {       
        IAnimContext Next { get; set; }     

        object Target { get; set; }

        void OnSample(bool isBlended, float blend);

        void CaptureTargetState();

        void RestoreTargetState();

        IAnimContext Clone();               
    }    

    public interface ITargetNamedContext : IAnimContext
    {
        string TargetName { get; set; }
    }

    public interface IAnimContext<T> : IAnimContext
    {
        T Data { get; }

        new IAnimContext<T> Next { get; set; }
    }
  
}
