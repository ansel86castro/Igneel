using Igneel.Assets;
using Igneel.Graphics;

namespace Igneel.SceneComponents
{
    [Asset("BILLBOARD")]
    public class BillBoard : GraphicObject<BillBoard>
    {
        private float _width = 20;
        private float _height = 20;
        private Texture2D _texture;
        private Vector4 _color = new Vector4(1, 1, 1, 1);

        public BillBoard()
        {
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);

        }

        public BillBoard(Texture2D texture)
        {
            this._texture = texture;
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }
       

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        
        public float Width { get { return _width; } set { _width = value; } }

        
        public float Height { get { return _height; } set { _height = value; } }

        public Vector4 Color { get { return _color; } set { _color = value; } }

        public Matrix GetBillboardMatrix(Camera camera, Vector3 position, Vector3 scale)
        {
            Vector3 w = camera.Position - position;
            w.Normalize();
            Vector3 v, u;
            v = camera.Up;

            v *= _height * scale.Y;
            u = Vector3.Normalize(Vector3.Cross(v, w)) * _width * scale.X;

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
                if (_texture != null)
                    _texture.Dispose();
            }
        }
    }
     
}
