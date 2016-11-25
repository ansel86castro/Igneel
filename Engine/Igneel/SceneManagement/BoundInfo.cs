namespace Igneel.SceneManagement
{
    public class BoundInfo : IBoundable
    {
        public Sphere LocalSphere;
        public Sphere GlobalSphere;
        public OrientedBox OrientedBox;


        Sphere IBoundable.BoundingSphere
        {
            get { return GlobalSphere; }
        }

        OrientedBox IBoundable.BoundingBox
        {
            get { return OrientedBox; }
        }

        public bool IsInside(Frustum frustum)
        {
            return GlobalSphere.Radius == 0 || frustum.Contains(GlobalSphere);
        }

        public void Update(Matrix _worldTransform)
        {
            if (OrientedBox != null)
                OrientedBox.Update(_worldTransform);

            GlobalSphere = LocalSphere.GetTranformed(_worldTransform);
        }
    }
}
