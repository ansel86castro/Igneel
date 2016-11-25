namespace Igneel.SceneManagement
{
    public interface IBoundable
    {
        Sphere BoundingSphere { get; }
        
        OrientedBox BoundingBox { get; }       
    }

    public static class Boundable
    {
        public static FrustumTest GetCullState(this IBoundable obj, Plane[] planes)
        {
            return Sphere.GetCullTest(obj.BoundingSphere, planes);
        }

        public static bool IsInsideFrustum(this IBoundable obj, Plane[] planes)
        {
            return Sphere.IsInsideFrustum(obj.BoundingSphere, planes);
        }

        public static bool IsInsideRect(this IBoundable obj, RectangleF rect)
        {
            return Sphere.IntersectRect(obj.BoundingSphere, rect);
        }
    }
}
