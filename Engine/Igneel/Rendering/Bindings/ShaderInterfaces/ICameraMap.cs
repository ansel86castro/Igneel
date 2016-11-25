namespace Igneel.Rendering.Bindings
{
    public interface ICameraMap : IViewProjectionMap
    {
        Vector3 EyePos { get; set; }
    }
}