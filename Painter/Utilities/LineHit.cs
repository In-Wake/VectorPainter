using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Painter.Utilities
{
    public class LineHit
    {
        public bool PointInLine(Point lineA, Point lineB, Point target, double lineThikness) {

            var a = lineA;
            var b = lineB;
            var e = target;

            var ab = b - a;
            var be = e - b;
            var ae = e - a;

            var ab_be = ab * be;
            var ab_ae = ab * ae;

            double h = 0;

            if (ab_be > 0)
            {
                h = be.Length;
            }
            else if (ab_ae < 0)
            {
                h = ae.Length;
            }
            else
            {
                var x1 = ab.X;
                var y1 = ab.Y;
                var x2 = ae.X;
                var y2 = ae.Y;
                var mod = Math.Sqrt(x1 * x1 + y1 * y1);
                h = Math.Abs(x1 * y2 - x2 * y1) / mod;
            }

            return h < lineThikness / 2;
        }

        public const int MISS_INDEX = -1;

        public int PointIndex(List<Point> points, Point target, double thikness) {
            var tolerance = GetPointHitTolerance(thikness);
            var hitArea = GetHitArea(target, tolerance);

            for (int i = 0; i < points.Count; i++)
            {
                if (hitArea.Contains(points[i]))
                {
                    return i;
                }
            }

            return MISS_INDEX;
        }

        public Rect GetHitArea(Point target, double tolerance) 
        {
            return new Rect(new Point(target.X - tolerance / 2, target.Y - tolerance / 2), new Size(tolerance, tolerance));
        }

        public double GetPointHitTolerance(double lineThikness)
        {
            var increment = lineThikness * 0.1;

            const double MIN_INCREMENT = 2;

            increment = lineThikness < MIN_INCREMENT ? MIN_INCREMENT : increment;

            return lineThikness + increment;
        }
    }
}
