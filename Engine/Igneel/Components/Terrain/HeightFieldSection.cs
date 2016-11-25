using Igneel.Graphics;
using Igneel.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components.Terrain
{
    public class HeightFieldSection : ICullable
    {
        public Vector2 Offset;
        public GraphicBuffer NormalHeightVb;
        public int MaterialIndex;
        public RectangleF BoundRect;
        public Sphere BoundSphere;

        public void ComputeBoundings(RectangleF bounds)
        {
            this.BoundRect = bounds;
            BoundSphere.Center = new Vector3(bounds.X + bounds.Width * 0.5f, 0, bounds.Y - bounds.Height * 0.5f);
            BoundSphere.Radius = (float)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height);
        }

        public Sphere BoundingSphere
        {
            get { return BoundSphere; }
        }

        public OrientedBox BoundingBox
        {
            get { return null; }
        }

        public ICullRegion CullRegion { get; set; }
    }
}
