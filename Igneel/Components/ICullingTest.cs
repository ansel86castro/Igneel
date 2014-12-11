
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    public interface ICullTester<in T>
    {
        bool IsInsideRect(T item, RectangleF rec);

        bool IsInsideFrustum(T item, Camera camera);

        CullTestResult GetCullState(Camera camera, Sphere sphere);
    }

    public sealed class CullTester<T> : ICullTester<T>
    {
        private Func<T, Camera, bool> insideFrustumTest;
        private Func<T, RectangleF, bool> insideRecTest;
        private Func<Camera, Sphere, CullTestResult> culltester;

        public CullTester(Func<T, RectangleF, bool> insideRecTest, Func<T, Camera, bool> insideFrustumTest, Func<Camera, Sphere, CullTestResult> cullTester)
        {
            Contract.Requires<NullReferenceException>(insideRecTest != null);
            Contract.Requires<NullReferenceException>(insideFrustumTest != null);           

            this.insideRecTest = insideRecTest;
            this.insideFrustumTest = insideFrustumTest;
            this.culltester = cullTester;
        }

        public bool IsInsideRect(T item ,RectangleF rec)
        {
            return insideRecTest(item ,rec);
        }

        public bool IsInsideFrustum(T item, Camera camera)
        {
            return insideFrustumTest(item, camera);
        }


        public CullTestResult GetCullState(Camera camera, Sphere sphere)
        {
            return culltester(camera, sphere);
        }
    }

    public class BoundableTester<T> : ICullTester<T>
        where T:IBoundable
    {

        static BoundableTester<T> _tester;

        public static BoundableTester<T> Tester
        {
            get { return _tester ?? (_tester = new BoundableTester<T>()); }
        }

        public bool IsInsideRect(T item, RectangleF rec)
        {
            return Sphere.IsInsideRect(item.BoundingSphere, rec);
        }

        public bool IsInsideFrustum(T item, Camera camera)
        {
            return camera.ViewFrustum.Contains(item.BoundingSphere);//  Sphere.IsInsideFrustum(item.GlobalSphere, camera.FrustumPlanes);
        }


        public CullTestResult GetCullState(Camera camera, Sphere sphere)
        {
            return camera.ViewFrustum.GetCullResult(sphere);// camera.GetCullTest(sphere.Center, sphere.Radius);
        }
    }
}
