using Igneel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Rendering
{
    
    //public static class RenderRegistrator
    //{
    //    public static void Register<TTech, TRender, TComp>(this IRenderRegistrator<TComp> registrator)
    //        where TComp : class
    //        where TTech : Technique
    //        where TRender : Render, new()
    //    {
    //        ServiceStore.SetService<IFactory<TRender>>(new SingletonDisposableFactoryNew<TRender>());
    //        Engine.RenderManager.SetRender<TComp, TTech, TRender>();
    //    }
    //}

    public abstract class Registrator<TComp> : IRenderRegistrator<TComp>
        where TComp : class
    {
       public static void Register<TTech, TRender>()
            where TTech : Technique
            where TRender : Render<TComp>, new()
        {          
            Service.SetFactory<TRender>(new SingletonDisposableFactoryNew<TRender>());
            Engine.RegisterRender<TComp, TTech, TRender>();
        }

       public static void Register<TTech, TRender>(Func<Render> factoryMethod = null)
           where TTech : Technique
           where TRender : Render<TComp>
       {
           if(factoryMethod!=null)
               Service.SetFactory<TRender>(new SingletonDisposableFactoryDelegate<TRender>( ()=> (TRender)factoryMethod()));
           Engine.RegisterRender<TComp, TTech, TRender>();
       }

       public static void RegisterNullRender<TTech>()
           where TTech : Technique
       {
           Engine.RegisterRender<TComp, TTech>(null);
       }

       public abstract void RegisterRenders();

       public void RegisterInstance()
       {
           Service.Set<IRenderRegistrator<TComp>>(this);
       }
    }

    public abstract class Registrator<TComp, TReg> : Registrator<TComp>
        where TComp : class
        where TReg : IRenderRegistrator<TComp>
    {
       
    }       
}
