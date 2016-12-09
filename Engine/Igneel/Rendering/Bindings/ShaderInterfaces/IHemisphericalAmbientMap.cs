namespace Igneel.Rendering.Bindings
{
    public interface IHemisphericalAmbientMap
    {
        Vector3 SkyColor { get; set; }
        Vector3 GroundColor { get; set; }
        Vector3 NorthPole { get; set; }
    }
}