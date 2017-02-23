using System;
using System.Diagnostics.Contracts;
using Igneel.SceneComponents;

namespace Igneel.SceneManagement
{
    public interface ICullTester<in T>
    {
        bool IsInsideRect(T item, RectangleF rec);

        bool IsInsideFrustum(T item, Camera camera);

        FrustumTest GetCullState(Camera camera, Sphere sphere);
    }

    public sealed class CullTester<T> : ICullTester<T>
    {
        private Func<T, Camera, bool> _insideFrustumTest;
        private Func<T, RectangleF, bool> _insideRecTest;
        private Func<Camera, Sphere, FrustumTest> _culltester;

        public CullTester(Func<T, RectangleF, bool> insideRecTest, Func<T, Camera, bool> insideFrustumTest, Func<Camera, Sphere, FrustumTest> cullTester)
        {
            Contract.Requires<NullReferenceException>(insideRecTest != null);
            Contract.Requires<NullReferenceException>(insideFrustumTest != null);           

            this._insideRecTest = insideRecTest;
            this._insideFrustumTest = insideFrustumTest;
            this._culltester = cullTester;
        }

        public bool IsInsideRect(T item ,RectangleF rec)
        {
            return _insideRecTest(item ,rec);
        }

        public bool IsInsideFrustum(T item, Camera camera)
        {
            return _insideFrustumTest(item, camera);
        }


        public FrustumTest GetCullState(Camera camera, Sphere sphere)
        {
            return _culltester(camera, sphere);
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
            return Sphere.IntersectRect(item.BoundingSphere, rec);
        }

        public bool IsInsideFrustum(T item, Camera camera)
        {
            return camera.ViewFrustum.Contains(item.BoundingSphere);//  Sphere.IsInsideFrustum(item.GlobalSphere, camera.FrustumPlanes);
        }


        public FrustumTest GetCullState(Camera camera, Sphere sphere)
        {
            return camera.ViewFrustum.TestFrustum(sphere);// camera.GetCullTest(sphere.Center, sphere.Radius);
        }
    }

    public static class CullTesterUtils
    {
        public static bool Contains<T>(this ICullTester<T> tester, Camera camera, Sphere sphere)
        {
            var cont =  tester.GetCullState(camera, sphere);
            return cont == FrustumTest.Inside || cont == FrustumTest.Partial;
        }
    }
}
