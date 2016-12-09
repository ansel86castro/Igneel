namespace Igneel
{
    public interface IDynamicNotificable:IDynamic
    {
        event UpdateEventHandler UpdateEvent;
    }
}