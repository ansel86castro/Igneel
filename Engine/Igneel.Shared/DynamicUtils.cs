namespace Igneel
{
    public static class DynamicUtils
    {
        public static T SetUpdate<T>(this T d, UpdateEventHandler callback) where T : IDynamicNotificable
        {            
            d.UpdateEvent += callback;            
            return d;
        }
    }
}