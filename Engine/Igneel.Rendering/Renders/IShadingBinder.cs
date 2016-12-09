using System;
namespace Igneel.Rendering
{
    public interface IRenderInputBinder
    {
        void Bind<TData>(TData value);

        IRenderBinding<TData> GetBinding<TData>();

        void UnBind<TData>(TData value);
    }
}
