
using Igneel.Graphics;
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    public class BillBoard : GraphicObject<BillBoard>
    {
        private float width = 20;
        private float height = 20;
        private Texture2D texture;
        private Vector4 color = new Vector4(1, 1, 1, 1);

        public BillBoard()
        {
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);

        }

        public BillBoard(Texture2D texture)
        {
            this.texture = texture;
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }
       

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        
        public float Width { get { return width; } set { width = value; } }

        
        public float Height { get { return height; } set { height = value; } }

        public Vector4 Color { get { return color; } set { color = value; } }

        public Matrix GetBillboardMatrix(Camera camera, Vector3 position, Vector3 scale)
        {
            Vector3 w = camera.Position - position;
            w.Normalize();
            Vector3 v, u;
            v = camera.Up;

            v *= height * scale.Y;
            u = Vector3.Normalize(Vector3.Cross(v, w)) * width * scale.X;

            var world = new Matrix();

            world.M11 = u.X;
            world.M12 = u.Y;
            world.M13 = u.Z;
            world.M14 = 0;

            world.M21 = v.X;
            world.M22 = v.Y;
            world.M23 = v.Z;
            world.M24 = 0;

            world.M31 = w.X;
            world.M32 = w.Y;
            world.M33 = w.Z;
            world.M34 = 0;

            world.M41 = position.X;
            world.M42 = position.Y;
            world.M43 = position.Z;
            world.M44 = 1;

            return world;
        }

        public static Matrix GetBillBoardMatrix(Vector3 camPosition, Vector3 camUp, Vector3 position)
        {
            Vector3 w = camPosition - position;
            w.Normalize();

            Vector3 v, u;
            v = camUp;
            u = Vector3.Cross(v, w);
            u.Normalize();

            var world = new Matrix();

            world.M11 = u.X;
            world.M12 = u.Y;
            world.M13 = u.Z;
            world.M14 = 0;

            world.M21 = v.X;
            world.M22 = v.Y;
            world.M23 = v.Z;
            world.M24 = 0;

            world.M31 = w.X;
            world.M32 = w.Y;
            world.M33 = w.Z;
            world.M34 = 0;

            world.M41 = position.X;
            world.M42 = position.Y;
            world.M43 = position.Z;
            world.M44 = 1;

            return world;
        }

        protected override void OnDispose(bool d)
        {
            if (d)
            {
                if (texture != null)
                    texture.Dispose();
            }
            base.OnDispose(d);
        }
    }
     
}
