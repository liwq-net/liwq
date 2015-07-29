using System;
using System.ComponentModel;

namespace liwq
{
    public struct Size
    {
        public static readonly Size Zero = new Size(0, 0);

        public float Width;
        public float Height;

        public Size(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public Size Clamp(Size max)
        {
            float w = (this.Width > max.Width) ? max.Width : Width;
            float h = (this.Height > max.Height) ? max.Height : Height;
            return new Size(w, h);
        }

        /// <summary>
        /// Computes the diagonal length of this size. 
        /// This method will always compute the length using Sqrt()
        /// </summary>
        public float Diagonal
        {
            get
            {
                return ((float)Math.Sqrt(Width * Width + Height * Height));
            }
        }

        /// <summary>
        /// Returns the inversion of this size, which is the height and width swapped.
        /// </summary>
        public Size Inverted
        {
            get { return new Size(Height, Width); }
        }

        public static bool Equal(ref Size size1, ref Size size2)
        {
            return ((size1.Width == size2.Width) && (size1.Height == size2.Height));
        }

        public override int GetHashCode()
        {
            return this.Width.GetHashCode() + this.Height.GetHashCode();
        }

        public bool Equals(Size s)
        {
            return Width == s.Width && Height == s.Height;
        }

        public override bool Equals(object obj)
        {
            return (Equals((Size)obj));
        }

        public Point Center
        {
            get { return new Point(this.Width / 2f, this.Height / 2f); }
        }

        public override string ToString()
        {
            return String.Format("{0} x {1}", Width, Height);
        }

        public static bool operator ==(Size p1, Size p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Size p1, Size p2)
        {
            return !p1.Equals(p2);
        }

        public static Size operator *(Size p, float f)
        {
            return (new Size(p.Width * f, p.Height * f));
        }

        public static Size operator /(Size p, float f)
        {
            return (new Size(p.Width / f, p.Height / f));
        }

        public static Size operator +(Size p, float f)
        {
            return (new Size(p.Width + f, p.Height + f));
        }

        public static Size operator +(Size p, Size q)
        {
            return (new Size(p.Width + q.Width, p.Height + q.Height));
        }

        public static Size operator -(Size p, float f)
        {
            return (new Size(p.Width - f, p.Height - f));
        }

        public static explicit operator Size(Point point)
        {
            Size size;
            size.Width = point.X;
            size.Height = point.Y;
            return size;
        }

        public Rect AsRect
        {
            get { return (new Rect(0, 0, Width, Height)); }
        }
    }
}