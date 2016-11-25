using System;

namespace Igneel
{
    public interface INameChangingNotificator
    {
        event Action<object, string> NameChanged;
    }
}