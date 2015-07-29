using Microsoft.Xna.Framework;
using System;

namespace liwq
{
    public struct Rect
    {
        public static readonly Rect Zero = new Rect(0, 0, 0, 0);

        public Point Origin;
        public Size Size;

        public Rect(Size size)
        {
            this.Origin = Point.Zero;
            this.Size = size;
        }

        public Rect(float x, float y, float width, float height)
        {
            this.Origin.X = x;
            this.Origin.Y = y;
            this.Size.Width = width;
            this.Size.Height = height;
        }

        public Rect InvertedSize
        {
            get { return new Rect(this.Origin.X, this.Origin.Y, this.Size.Height, this.Size.Width); }
        }

        public float MinX { get { return this.Origin.X; } }
        public float MidX { get { return this.Origin.X + this.Size.Width / 2.0f; } }
        public float MaxX { get { return this.Origin.X + this.Size.Width; } }

        public float MinY { get { return this.Origin.Y; } }
        public float MidY { get { return this.Origin.Y + this.Size.Height / 2.0f; } }
        public float MaxY { get { return this.Origin.Y + this.Size.Height; } }

        public Point Center { get { return new Point(this.MidX, this.MinY); } }

        public Rect Union(Rect rect)
        {
            float minx = Math.Min(MinX, rect.MinX);
            float miny = Math.Min(MinY, rect.MinY);
            float maxx = Math.Max(MaxX, rect.MaxX);
            float maxy = Math.Max(MaxY, rect.MaxY);
            return new Rect(minx, miny, maxx - minx, maxy - miny);
        }

        public Rect Intersection(Rect rect)
        {
            if (IntersectsRect(rect) == false)
                return Zero;

            /*       +-------------+
             *       |             |
             *       |         +---+-----+
             * +-----+---+     |   |     |
             * |     |   |     |   |     |
             * |     |   |     +---+-----+
             * |     |   |         |
             * |     |   |         |
             * +-----+---+         |
             *       |             |
             *       +-------------+
             */
            float minx = 0, miny = 0, maxx = 0, maxy = 0;
            // X
            if (rect.MinX < this.MinX) minx = this.MinX;
            else if (rect.MinX < this.MaxX) minx = rect.MinX;
            if (rect.MaxX < this.MaxX) maxx = rect.MaxX;
            else if (rect.MaxX > this.MaxX) maxx = this.MaxX;

            //  Y
            if (rect.MinY < this.MinY) miny = this.MinY;
            else if (rect.MinY < this.MaxY) miny = rect.MinY;
            if (rect.MaxY < this.MaxY) maxy = rect.MaxY;
            else if (rect.MaxY > this.MaxY) maxy = this.MaxY;
            return new Rect(minx, miny, maxx - minx, maxy - miny);
        }

        public bool IntersectsRect(Rect rect)
        {
            return !(this.MaxX < rect.MinX || rect.MaxX < this.MinX || this.MaxY < rect.MinY || rect.MaxY < this.MinY);
        }
        public bool ContainsPoint(float x, float y)
        {
            return x >= this.MinX && x <= this.MaxX && y >= this.MinY && y <= this.MaxY;
        }
        public bool ContainsPoint(Point point)
        {
            return this.ContainsPoint(point.X, point.Y);
        }

        public static bool Equal(ref Rect rect1, ref Rect rect2)
        {
            return rect1.Origin.Equals(rect2.Origin) && rect1.Size.Equals(rect2.Size);
        }

        public static bool operator ==(Rect p1, Rect p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Rect p1, Rect p2)
        {
            return !p1.Equals(p2);
        }

        public override int GetHashCode()
        {
            return Origin.GetHashCode() + Size.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (Equals((Rect)obj));
        }

        public bool Equals(Rect rect)
        {
            return Origin.Equals(rect.Origin) && Size.Equals(rect.Size);
        }

        public override string ToString()
        {
            return String.Format("Rect : (x={0}, y={1}, width={2}, height={3})", Origin.X, Origin.Y, Size.Width, Size.Height);
        }

        public static implicit operator Rectangle (Rect rect)
        {
            return new Rectangle((int)rect.Origin.X, (int)rect.Origin.Y, (int)rect.Size.Width, (int)rect.Size.Height);
        }
    }
}