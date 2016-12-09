using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Components.Terrain
{
    public class TerrainSectionTester : ICullTester<HeightFieldSection>
    {
        public Plane[] LocalFrustum = new Plane[6];
        public Vector3[] Points = new Vector3[8];

        public bool IsInsideRect(HeightFieldSection item, RectangleF rec)
        {
            //return rec.Contains(item.Bounds);
            return Sphere.IntersectRect(item.BoundSphere, rec);
        }

        public bool IsInsideFrustum(HeightFieldSection item, Camera camera)
        {
            //var cull = Camera.GetCullTest(camera.FrustumPlanes, item.BoundSphere);
            var cull = Frustum.TestFrustum(LocalFrustum, item.BoundSphere);
            return cull == FrustumTest.Inside || cull == FrustumTest.Partial;
        }


        public FrustumTest GetCullState(Camera camera, Sphere sphere)
        {
            //return Camera.GetCullTest(camera.FrustumPlanes, sphere.Center, sphere.Radius);
            return Frustum.TestFrustum(LocalFrustum, sphere);
        }
    }
}
