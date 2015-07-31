using Microsoft.Xna.Framework;
using System;

namespace liwq
{
    public struct Point
    {
        public static readonly Point Zero = new Point(0, 0);

        public static readonly Point AnchorCenter = new Point(0.5f, 0.5f);
        public static readonly Point AnchorLowerLeft = new Point(0f, 0f);
        public static readonly Point AnchorUpperLeft = new Point(0f, 1f);
        public static readonly Point AnchorLowerRight = new Point(1f, 0f);
        public static readonly Point AnchorUpperRight = new Point(1f, 1f);
        public static readonly Point AnchorCenterRight = new Point(1f, 0.5f);
        public static readonly Point AnchorCenterLeft = new Point(0f, 0.5f);
        public static readonly Point AnchorCenterTop = new Point(0.5f, 1f);
        public static readonly Point AnchorCenterBottom = new Point(0.5f, 0f);

        public float X;
        public float Y;

        public Point(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(Point p)
        {
            this.X = p.X;
            this.Y = p.Y;
        }

        public Point(Vector2 v)
        {
            this.X = v.X;
            this.Y = v.Y;
        }

        public static bool Equal(ref Point point1, ref Point point2)
        {
            return ((point1.X == point2.X) && (point1.Y == point2.Y));
        }

        public Point Offset(float dx, float dy)
        {
            return new Point(this.X + dx, this.Y + dy);
        }

        public Point Reverse
        {
            get { return new Point(-X, -Y); }
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (Equals((Point)obj));
        }

        public bool Equals(Point p)
        {
            return X == p.X && Y == p.Y;
        }

        public override string ToString()
        {
            return String.Format("Point : (x={0}, y={1})", X, Y);
        }

        public float DistanceSQ(ref Point v2)
        {
            return Sub(ref v2).LengthSQ;
        }

        public Point Sub(ref Point v2)
        {
            Point pt;
            pt.X = X - v2.X;
            pt.Y = Y - v2.Y;
            return pt;
        }

        public float LengthSQ
        {
            get { return X * X + Y * Y; }
        }

        public float LengthSquare
        {
            get { return LengthSQ; }
        }

        /// <summary>
        /// Computes the length of this point as if it were a vector with XY components relative to the origin.
        /// This is computed each time this property is accessed, so cache the value that is returned.
        /// </summary>
        public float Length
        {
            get { return (float)Math.Sqrt(X * X + Y * Y); }
        }

        /// <summary>
        /// Inverts the direction or location of the Y component.
        /// </summary>
        public Point InvertY
        {
            get
            {
                Point pt;
                pt.X = X;
                pt.Y = -Y;
                return pt;
            }
        }

        public bool IsZero
        {
            get
            {
                return (this.X == 0 && this.Y == 0);
            }
        }

        /// <summary>
        /// Normalizes the components of this point (convert to mag 1), 
        /// and returns the orignial magnitude of the vector defined by the XY components of this point.
        /// </summary>
        public float Normalize()
        {
            // On Arm float.Epsilon is too small and evalutates to 0
            // http://msdn.microsoft.com/en-us/library/system.single.epsilon(v=vs.110).aspx
            const float FLT_EPSILON = 1.175494351E-38f;

            var mag = (float)Math.Sqrt(X * X + Y * Y);
            if (mag < FLT_EPSILON)
            {
                return (0f);
            }
            float l = 1f / mag;
            X *= l;
            Y *= l;
            return (mag);
        }

        #region Static Methods

        /** Run a math operation function on each point component
         * absf, fllorf, ceilf, roundf
         * any function that has the signature: float func(float);
         * For example: let's try to take the floor of x,y
         * ccpCompOp(p,floorf);
         @since v0.99.1
         */
        public delegate float ComputationOperationDelegate(float a);

        public static Point ComputationOperation(Point p, ComputationOperationDelegate del)
        {
            Point pt;
            pt.X = del(p.X);
            pt.Y = del(p.Y);
            return pt;
        }

        /** Linear Interpolation between two points a and b
            @returns
              alpha == 0 ? a
              alpha == 1 ? b
              otherwise a value between a..b
            @since v0.99.1
       */
        public static Point Lerp(Point a, Point b, float alpha)
        {
            return (a * (1f - alpha) + b * alpha);
        }


        /** @returns if points have fuzzy equality which means equal with some degree of variance.
            @since v0.99.1
        */
        public static bool FuzzyEqual(Point a, Point b, float variance)
        {
            if (a.X - variance <= b.X && b.X <= a.X + variance)
                if (a.Y - variance <= b.Y && b.Y <= a.Y + variance)
                    return true;
            return false;
        }


        /** Multiplies a nd b components, a.X*b.X, a.y*b.y
            @returns a component-wise multiplication
            @since v0.99.1
        */
        public static Point MultiplyComponents(Point a, Point b)
        {
            Point pt;
            pt.X = a.X * b.X;
            pt.Y = a.Y * b.Y;
            return pt;
        }

        /** @returns the signed angle in radians between two vector directions
            @since v0.99.1
        */
        public static float AngleSigned(Point a, Point b)
        {
            // On Arm float.Epsilon is too small and evalutates to 0
            // http://msdn.microsoft.com/en-us/library/system.single.epsilon(v=vs.110).aspx
            const float FLT_EPSILON = 1.175494351E-38f;

            Point a2 = Normalize(a);
            Point b2 = Normalize(b);
            var angle = (float)Math.Atan2(a2.X * b2.Y - a2.Y * b2.X, DotProduct(a2, b2));
            if (Math.Abs(angle) < FLT_EPSILON)
            {
                return 0.0f;
            }
            return angle;
        }

        /** Rotates a point counter clockwise by the angle around a pivot
            @param v is the point to rotate
            @param pivot is the pivot, naturally
            @param angle is the angle of rotation cw in radians
            @returns the rotated point
            @since v0.99.1
        */
        public static Point RotateByAngle(Point v, Point pivot, float angle)
        {
            Point r = v - pivot;
            float cosa = (float)Math.Cos(angle), sina = (float)Math.Sin(angle);
            float t = r.X;
            r.X = t * cosa - r.Y * sina + pivot.X;
            r.Y = t * sina + r.Y * cosa + pivot.Y;
            return r;
        }

        /** A general line-line intersection test
         @param p1 
            is the startpoint for the first line P1 = (p1 - p2)
         @param p2 
            is the endpoint for the first line P1 = (p1 - p2)
         @param p3 
            is the startpoint for the second line P2 = (p3 - p4)
         @param p4 
            is the endpoint for the second line P2 = (p3 - p4)
         @param s 
            is the range for a hitpoint in P1 (pa = p1 + s*(p2 - p1))
         @param t
            is the range for a hitpoint in P3 (pa = p2 + t*(p4 - p3))
         @return bool 
            indicating successful intersection of a line
            note that to truly test intersection for segments we have to make 
            sure that s & t lie within [0..1] and for rays, make sure s & t > 0
            the hit point is		p3 + t * (p4 - p3);
            the hit point also is	p1 + s * (p2 - p1);
         @since v0.99.1
         */
        public static bool LineIntersect(Point A, Point B, Point C, Point D, ref float S, ref float T)
        {
            // FAIL: Line undefined
            if ((A.X == B.X && A.Y == B.Y) || (C.X == D.X && C.Y == D.Y))
            {
                return false;
            }

            float BAx = B.X - A.X;
            float BAy = B.Y - A.Y;
            float DCx = D.X - C.X;
            float DCy = D.Y - C.Y;
            float ACx = A.X - C.X;
            float ACy = A.Y - C.Y;

            float denom = DCy * BAx - DCx * BAy;

            S = DCx * ACy - DCy * ACx;
            T = BAx * ACy - BAy * ACx;

            if (denom == 0)
            {
                if (S == 0 || T == 0)
                {
                    // Lines incident
                    return true;
                }
                // Lines parallel and not incident
                return false;
            }

            S = S / denom;
            T = T / denom;

            // Point of intersection
            // CGPoint P;
            // P.X = A.X + *S * (B.X - A.X);
            // P.y = A.y + *S * (B.y - A.y);

            return true;
        }

        /*
        ccpSegmentIntersect returns YES if Segment A-B intersects with segment C-D
        @since v1.0.0
        */
        public static bool SegmentIntersect(Point A, Point B, Point C, Point D)
        {
            float S = 0, T = 0;
            if (LineIntersect(A, B, C, D, ref S, ref T) && (S >= 0.0f && S <= 1.0f && T >= 0.0f && T <= 1.0f))
            {
                return true;
            }
            return false;
        }

        /*
        ccpIntersectPoint returns the intersection point of line A-B, C-D
        @since v1.0.0
        */
        public static Point IntersectPoint(Point A, Point B, Point C, Point D)
        {
            float S = 0, T = 0;
            if (LineIntersect(A, B, C, D, ref S, ref T))
            {
                // Point of intersection
                Point P;
                P.X = A.X + S * (B.X - A.X);
                P.Y = A.Y + S * (B.Y - A.Y);
                return P;
            }
            return Zero;
        }

        /** Converts radians to a normalized vector.
            @return CCPoint
            @since v0.7.2
        */
        public static Point ForAngle(float a)
        {
            Point pt;
            pt.X = (float)Math.Cos(a);
            pt.Y = (float)Math.Sin(a);
            return pt;
        }

        /** Converts a vector to radians.
            @return CGFloat
            @since v0.7.2
        */
        public static float ToAngle(Point v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }

        /** Clamp a value between from and to.
            @since v0.99.1
        */
        public static float Clamp(float value, float min_inclusive, float max_inclusive)
        {
            if (min_inclusive > max_inclusive)
            {
                float ftmp = min_inclusive;
                min_inclusive = max_inclusive;
                max_inclusive = ftmp;
            }
            return value < min_inclusive ? min_inclusive : value < max_inclusive ? value : max_inclusive;
        }

        /** Clamp a point between from and to.
            @since v0.99.1
        */
        public static Point Clamp(Point p, Point from, Point to)
        {
            Point pt;
            pt.X = Clamp(p.X, from.X, to.X);
            pt.Y = Clamp(p.Y, from.Y, to.Y);
            return pt;
            //            return CreatePoint(Clamp(p.X, from.X, to.X), Clamp(p.Y, from.Y, to.Y));
        }

        /** Quickly convert CCSize to a CCPoint
            @since v0.99.1
        */
        [Obsolete("Use explicit cast (CCPoint)size.")]
        public static Point FromSize(Size s)
        {
            Point pt;
            pt.X = s.Width;
            pt.Y = s.Height;
            return pt;
        }

        /**
         * Allow Cast CCSize to CCPoint
         */
        public static explicit operator Point(Size size)
        {
            Point pt;
            pt.X = size.Width;
            pt.Y = size.Height;
            return pt;
        }

        public static Point Perp(Point p)
        {
            Point pt;
            pt.X = -p.Y;
            pt.Y = p.X;
            return pt;
        }

        public static float Dot(Point p1, Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        public static float Distance(Point v1, Point v2)
        {
            return (v1 - v2).Length;
        }

        public static Point Normalize(Point p)
        {
            float x = p.X;
            float y = p.Y;
            float l = 1f / (float)Math.Sqrt(x * x + y * y);
            Point pt;
            pt.X = x * l;
            pt.Y = y * l;
            return pt;
        }

        public static Point Midpoint(Point p1, Point p2)
        {
            Point pt;
            pt.X = (p1.X + p2.X) / 2f;
            pt.Y = (p1.Y + p2.Y) / 2f;
            return pt;
        }

        public static float DotProduct(Point v1, Point v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        /** Calculates cross product of two points.
            @return CGFloat
            @since v0.7.2
        */

        public static float CrossProduct(Point v1, Point v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        /** Calculates perpendicular of v, rotated 90 degrees counter-clockwise -- cross(v, perp(v)) >= 0
            @return CCPoint
            @since v0.7.2
        */

        public static Point PerpendicularCounterClockwise(Point v)
        {
            Point pt;
            pt.X = -v.Y;
            pt.Y = v.X;
            return pt;
        }

        /** Calculates perpendicular of v, rotated 90 degrees clockwise -- cross(v, rperp(v)) <= 0
            @return CCPoint
            @since v0.7.2
        */

        public static Point PerpendicularClockwise(Point v)
        {
            Point pt;
            pt.X = v.Y;
            pt.Y = -v.X;
            return pt;
        }

        /** Calculates the projection of v1 over v2.
            @return CCPoint
            @since v0.7.2
        */

        public static Point Project(Point v1, Point v2)
        {
            float dp1 = v1.X * v2.X + v1.Y * v2.Y;
            float dp2 = v2.LengthSQ;
            float f = dp1 / dp2;
            Point pt;
            pt.X = v2.X * f;
            pt.Y = v2.Y * f;
            return pt;
            // return Multiply(v2, DotProduct(v1, v2) / DotProduct(v2, v2));
        }

        /** Rotates two points.
            @return CCPoint
            @since v0.7.2
        */

        public static Point Rotate(Point v1, Point v2)
        {
            Point pt;
            pt.X = v1.X * v2.X - v1.Y * v2.Y;
            pt.Y = v1.X * v2.Y + v1.Y * v2.X;
            return pt;
        }

        /** Unrotates two points.
            @return CCPoint
            @since v0.7.2
        */

        public static Point Unrotate(Point v1, Point v2)
        {
            Point pt;
            pt.X = v1.X * v2.X + v1.Y * v2.Y;
            pt.Y = v1.Y * v2.X - v1.X * v2.Y;
            return pt;
        }

        #endregion


        public static bool operator ==(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public static Point operator -(Point p1, Size p2)
        {
            Point pt;
            pt.X = p1.X - p2.Width;
            pt.Y = p1.Y - p2.Height;
            return pt;
        }

        public static Point operator -(Point p1, Point p2)
        {
            Point pt;
            pt.X = p1.X - p2.X;
            pt.Y = p1.Y - p2.Y;
            return pt;
        }

        public static Point operator -(Point p1)
        {
            Point pt;
            pt.X = -p1.X;
            pt.Y = -p1.Y;
            return pt;
        }

        public static Point operator +(Point p1, Size p2)
        {
            Point pt;
            pt.X = p1.X + p2.Width;
            pt.Y = p1.Y + p2.Height;
            return pt;
        }

        public static Point operator +(Point p1, Point p2)
        {
            Point pt;
            pt.X = p1.X + p2.X;
            pt.Y = p1.Y + p2.Y;
            return pt;
        }

        public static Point operator *(Point p1, Point p2)
        {
            Point pt;
            pt.X = p1.X * p2.X;
            pt.Y = p1.Y * p2.Y;
            return pt;
        }

        public static Point operator +(Point p1)
        {
            Point pt;
            pt.X = +p1.X;
            pt.Y = +p1.Y;
            return pt;
        }

        public static Point operator *(Point p, float value)
        {
            Point pt;
            pt.X = p.X * value;
            pt.Y = p.Y * value;
            return pt;
        }

        public static Point operator /(Point p, float value)
        {
            Point pt;
            pt.X = p.X / value;
            pt.Y = p.Y / value;
            return pt;
        }

        public static implicit operator Point(Vector2 point)
        {
            return new Point(point.X, point.Y);
        }

        public static implicit operator Vector2(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static implicit operator Vector3(Point point)
        {
            return new Vector3(point.X, point.Y, 0);
        }

        public static Point Random()
        {
            return new Point(Utility.Random.Next((int)Application.SharedApplication.DesignSize.Width), Utility.Random.Next((int)Application.SharedApplication.DesignSize.Height));
        }
    }

}
