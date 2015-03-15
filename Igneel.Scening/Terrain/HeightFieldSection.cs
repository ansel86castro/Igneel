using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering
{
    public struct HeightFieldVertex
    {
        [VertexElement(IASemantic.Position, usageIndex: 0, stream: 0, offset: 0)]
        public Vector2 Position;

        [VertexElement(IASemantic.TextureCoordinate, usageIndex: 0, stream: 0, offset: 8)]
        public Vector2 TexCoord;

        [VertexElement(IASemantic.Position, usageIndex: 1, stream: 1, offset: 0)]
        public float Height;

        [VertexElement(IASemantic.Normal, usageIndex: 0, stream: 1, offset: 4)]
        public Vector3 Normal;

        [VertexElement(IASemantic.TextureCoordinate, usageIndex: 1, stream: 1, offset: 16)]
        public Vector2 BlendCoord;
    }

    public class HeightFieldSection: IBoundable
    {        
        public Vector2 Offset;
        public GraphicBuffer NormalHeightVb;
        public int MaterialIndex;
        public RectangleF BoundRect;
        public Sphere boundSphere;

        public void ComputeBoundings(RectangleF bounds)
        {
            this.BoundRect = bounds;
            boundSphere.Center = new Vector3(bounds.X + bounds.Width * 0.5f, 0, bounds.Y - bounds.Height * 0.5f);
            boundSphere.Radius = (float)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height);
        }

        public Sphere BoundingSphere
        {
            get { return boundSphere; }
        }

        public OrientedBox BoundingBox
        {
            get { return null; }
        }
    }

    public class TerrainSectionTester : ICullTester<HeightFieldSection>
    {
        public Plane[] LocalFrustum = new Plane[6];
        public Vector3[] Points = new Vector3[8];

        public bool IsInsideRect(HeightFieldSection item, RectangleF rec)
        {
            //return rec.Contains(item.Bounds);
            return Sphere.IsInsideRect(item.boundSphere, rec);
        }

        public bool IsInsideFrustum(HeightFieldSection item, Camera camera)
        {         
            //var cull = Camera.GetCullTest(camera.FrustumPlanes, item.BoundSphere);
            var cull = Frustum.GetContainment(LocalFrustum, item.boundSphere);
            return cull == CullTestResult.Inside || cull == CullTestResult.Partial;
        }


        public CullTestResult GetCullState(Camera camera, Sphere sphere)
        {
            //return Camera.GetCullTest(camera.FrustumPlanes, sphere.Center, sphere.Radius);
            return Frustum.GetContainment(LocalFrustum, sphere);
        }
    }
}
