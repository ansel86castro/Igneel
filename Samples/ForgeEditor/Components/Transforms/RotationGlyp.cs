using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ForgeEditor.Components.CoordinateSystems;
using Igneel;
using Igneel.Graphics;

namespace ForgeEditor.Components.Transforms
{
    public class RotationGlyp :DecalGlyp, ITranformGlyp
    {
        public const int None = 0;
        public const int X = 1;
        public const int Y = 2;
        public const int Z = 3;
        public const int SPHERE = 4;

        private float markersAlpha = 1.0f;

        public float MarkersAlpha
        {
            get { return markersAlpha; }
            set { markersAlpha = value; }
        }
        private float width = 2f;

        public float Width
        {
            get { return width; }
            set { width = value; }
        }
        private float radius = 5f;

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        private float thickness = 2;

        public float Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }
        Color4 sphereColor = new Color4(System.Drawing.Color.LightYellow.ToArgb());

        public Color4 SphereColor
        {
            get { return sphereColor; }
            set { sphereColor = value; }
        }

        public RotationGlyp(Rectangle screenRect)
            : base(screenRect)
        {

        }

        public void Initialize()
        {
            int stacks=32;
            int slices=32;
            var graphics = Engine.Graphics;
            var components = new[]
            {
                new GlypComponent{ Id = X},
                new GlypComponent{ Id = Y},
                new GlypComponent{ Id = Z},
                new GlypComponent{ Id = SPHERE} 
            };
            Components = components;

            CreateMarquer(Matrix.Identity,
              components[SPHERE - 1],
              new SphereBuilder(stacks, slices, radius),
              sphereColor,
              graphics, 
              0);

            var builder = new CylindreBuilder(stacks, slices, radius + 0.1f, thickness, true);
            //X
            CreateMarquer(Matrix.RotationZ(Numerics.PIover2) , 
                components[X-1],
                builder,
                new Color4(markersAlpha,1,0,0),
                graphics,
                0.1f);

            //Y
            CreateMarquer(Matrix.Identity,
                components[Y - 1],
                builder,
                new Color4(markersAlpha, 0, 1, 0),
                graphics,
                0.2f);

            //Z
            CreateMarquer(Matrix.RotationX(Numerics.PIover2),
                components[Z - 1],
                builder,
                new Color4(markersAlpha, 0, 0, 1),
                graphics,
                0.3f);      
        }

        private void CreateMarquer(Matrix transform, 
            GlypComponent component, ShapeBuilder<MeshVertex> builder, Color4 color, GraphicDevice graphics, float offset)
        {
            //x*radius = radius+offset
            //x=(radius+offset)/radius
            //x=1 + offset/radius
            var scale = 1 + offset / radius;
            var scaling = Matrix.Scale(scale, 1, scale);
            transform = scaling * transform;
            var data = new VertexPositionColor[builder.Vertices.Length]; 
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new VertexPositionColor(Vector3.TransformCoordinates(builder.Vertices[i].Position, transform),
                    color);
            }            
            component.VertexBuffer = graphics.CreateVertexBuffer(data: data);
            component.IndexBufffer = graphics.CreateIndexBuffer(data: builder.Indices);
        }

        #region ITranformGlyp Members

        public void Transform(Igneel.SceneManagement.Frame frame, GlypComponent component, 
            Igneel.Vector2 p0, Igneel.Vector2 p1)
        {
            var disp = p1 - p0;
            var position = frame.BoundingSphere.Radius > 0? 
                                frame.BoundingSphere.Center:
                                frame.GlobalPosition;
            var Tw = Matrix.Translate(-position);
            switch (component.Id)
            {
                case X:
                    Tw *=Matrix.RotationX(Numerics.ToRadians(disp.Y));
                    break;
                case Y:
                    Tw *= Matrix.RotationY(Numerics.ToRadians(disp.X));
                    break;
                case Z:
                    Tw *= Matrix.RotationZ(Numerics.ToRadians(disp.Y));
                    break;
            }

            Tw *= Matrix.Translate(position);
            var localPose = frame.LocalPose;          
            var P = Matrix.Invert(localPose) * frame.GlobalPose;
            var Tl = P * Tw * Matrix.Invert(P);
            frame.LocalPose *= Tl;
            frame.CommitChanges();            
        }

        #endregion
    }
}
