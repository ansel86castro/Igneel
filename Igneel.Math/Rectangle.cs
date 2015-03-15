using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle : IEquatable<Rectangle>
    {
        public int X;
        public int Y;
        public int Height;
        public int Width;

        public Rectangle(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Height = height;
            this.Width = width;
        }

        public bool Contains(int x, int y)
        {
            return x >= X && x <= (X + Width) && y >= Y && y <= (Y + Height);
        }

        public int Right { get { return X + Width; } }
        public int Bottom { get { return Y + Height; } }

        public bool IsEmpty
        {
            get { return Width == 0 && Height == 0; }
        }

        public override string ToString()
        {
            return string.Format("X:{0}, Y:{1}, Width:{2}, Height:{3}", X, Y, Width, Height);
        }

        public bool Equals(Rectangle other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }
    }

    [Serializable]
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct RectangleF : IEquatable<RectangleF>
    {
        public float X;
        public float Y;
        public float Height;
        public float Width;

        public float Top { get { return Y; } set { Y = value; } }
        public float Left { get { return X; } set { X = value; } }
        public float Right { get { return X + Width; } }
        public float Bottom { get { return Y - Height; } }
        public Vector2 LeftTop { get { return new Vector2(X, Y); } }
        public Vector2 RightTop { get { return new Vector2(X + Width, Y); } }
        public Vector2 RightBottom { get { return new Vector2(X + Width, Y - Height); } }
        public Vector2 LeftBottom { get { return new Vector2(X, Y - Height); } }

        public float Radius { get { return (float)Math.Sqrt(Width * Width + Height * Height) * 0.5f; } }
        public Vector2 Center { get { return new Vector2(X + Width * 0.5f, Y - Height * 0.5f); } }

        public RectangleF(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool Contains(Vector2 p)
        {
            return p.X > X && p.X < X + Width && p.Y < Y && p.Y > Y - Height;
        }

        public bool Contains(float x, float y)
        {
            return x >= X && x <= (X + Width) && y <= Y && y >= (Y - Height);
        }

        public bool Contains(RectangleF rec)
        {
            return Contains(rec.X, rec.Y) && Contains(rec.Right, rec.Bottom);
        }

        public bool IsEmpty
        {
            get { return Width == 0 && Height == 0; }
        }

        public override string ToString()
        {
            return string.Format("X:{0}, Y:{1}, Width:{2}, Height:{3}", X, Y, Width, Height);
        }

        public bool Equals(RectangleF other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        public RectangleF Combine(RectangleF other)
        {
            RectangleF rec;
            rec.X = Math.Min(other.X, X);
            rec.Y = Math.Max(other.Y, Y);
            rec.Width = Math.Max(Right, other.Right) - rec.X;
            rec.Height = rec.Y - Math.Min(Bottom, other.Bottom);

            return rec;
        }

        public InsideTestResult TestInside(Sphere sphere)
        {
            float width = sphere.Radius * 2f;
            float height = sphere.Radius * 2f;
            float x = sphere.Center.X - sphere.Radius;
            float y = sphere.Center.Z + sphere.Radius;
            RectangleF recSphere = new RectangleF(x, y, width, height);

            if (Contains(recSphere))
                return InsideTestResult.Inside;
            else if (recSphere.Contains(this))
                return InsideTestResult.Contained;
            else if (Contains(recSphere.LeftTop) ||
                    Contains(recSphere.RightTop) ||
                    Contains(recSphere.LeftBottom) ||
                    Contains(recSphere.RightBottom))
                return InsideTestResult.PartialInside;

            return InsideTestResult.Outside;
        }


        [Serializable]
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct CoordRectangle
        {
            public float LeftU;
            public float RightU;
            public float TopV;
            public float BottomV;
        }
    }
   
}
