using System;

namespace OrigoDB.Examples.Proxy
{
    [Serializable]
    public struct Point
    {
        public readonly double X;
        public readonly double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point Translate(Point velocityPerSecond, TimeSpan timePeriod)
        {
            double seconds = timePeriod.TotalSeconds;
            return new Point(X + velocityPerSecond.X * seconds, Y + velocityPerSecond.Y * seconds);
        }

        internal double DistanceTo(Point point)
        {
            double a = Math.Abs(X - point.X);
            double b = Math.Abs(Y - point.Y);
            return Math.Sqrt(a*a + b*b);
        }
    }
}
